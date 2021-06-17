using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json;

public class VisaCheckoutService
{

    public VisaCheckoutService()
    {
    }


    public CardInfo ReturnCardInfo(string callid,  string brandCode)
    {
        var appSettings = ConfigurationManager.AppSettings;
        string secret = appSettings["secretkey"];
        string apikey = appSettings["apikey"];

        string time = Convert.ToString((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds);
        string resource = "payment/data/" + callid;
        string query_string = "apikey=" + apikey + "&dataLevel=FULL";
        string token = secret + time + resource + query_string;
        string hashtoken = "x:" + time + ":" + this.GetHashSha256(token);
        string url = "https://secure.checkout.visa.com/wallet-services-web/" + resource + "?" + query_string;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        HttpWebRequest request;
        request = (HttpWebRequest)WebRequest.Create(url);

        request.Headers["X-PAY-TOKEN"] = hashtoken;
        request.Accept = "application/xml";
        request.ContentType = "application/json";

        string output = string.Empty;
        try
        {
            using (var response = request.GetResponse())
            {
                using (var stream = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(1252)))
                {

                    output = stream.ReadToEnd();
                }
            }
        }
        catch (WebException ex)
        {
            if (ex.Status == WebExceptionStatus.ProtocolError)
            {
                using (var stream = new StreamReader(ex.Response.GetResponseStream()))
                {
                    output = stream.ReadToEnd();
                }
            }
            else if (ex.Status == WebExceptionStatus.Timeout)
            {
                output = "Request timeout is expired.";
            }

            CardInfo cInfo = new CardInfo("", "", "", "", "", "", "error");
            return cInfo;
        }

        XmlDocument doc = new XmlDocument();
        doc.LoadXml(output);

        XmlNamespaceManager ns = new XmlNamespaceManager(doc.NameTable);
        ns.AddNamespace("x", "http://www.visa.com/vme/walletservices/external/common");

        XmlNode encPaymentDataNode = doc.SelectSingleNode("//x:encPaymentData", ns);
        XmlNode encPaymentKeyNode = doc.SelectSingleNode("//x:encKey", ns);



        string decryptedInfo = decryptPayload(secret, encPaymentKeyNode.InnerText, encPaymentDataNode.InnerText);
        CardInfo custInfo;
        doc = new XmlDocument();
        try
        {
            doc.LoadXml(decryptedInfo);
            XmlNode creditCardNumberNode = doc.SelectSingleNode("//x:accountNumber", ns);
            XmlNode cardBrandNode = doc.SelectSingleNode("//x:cardBrand", ns);
            XmlNode cardExpDateMonthNode = doc.SelectSingleNode("//x:month", ns);
            XmlNode cardExpDateYearNode = doc.SelectSingleNode("//x:year", ns);
            XmlNode cardAddressNode = doc.SelectSingleNode("//x:line1", ns);
            XmlNode cardZip = doc.SelectSingleNode("//x:postalCode", ns);
            custInfo = new CardInfo(creditCardNumberNode.InnerText, cardBrandNode.InnerText, cardExpDateMonthNode.InnerText, cardExpDateYearNode.InnerText, cardAddressNode.InnerText, cardZip.InnerText, "success");

        }
        catch (Exception ex)
        {
            dynamic paymentInfo = JsonConvert.DeserializeObject(decryptedInfo);
            string creditCardNumber = paymentInfo.paymentInstrument.accountNumber;
            string cardBrand = paymentInfo.paymentInstrument.paymentType.cardBrand;
            string cardExpDateMonth = paymentInfo.paymentInstrument.expirationDate.month;
            string cardExpDateYear = paymentInfo.paymentInstrument.expirationDate.year;
            string cardAddress = paymentInfo.paymentInstrument.billingAddress.line1;
            string cardZip = paymentInfo.paymentInstrument.billingAddress.postalCode;
            custInfo = new CardInfo(creditCardNumber, cardBrand, cardExpDateMonth, cardExpDateYear, cardAddress, cardZip, "success");
        }
        return custInfo;
    }

    const int HMAC_LENGTH = 32, IV_LENGTH = 16;
    public static String decryptPayload(String key, String wrappedKey, String payload)
    {
        return Encoding.UTF8.GetString(decrypt(decrypt(Encoding.UTF8.GetBytes(key),
            Convert.FromBase64String(wrappedKey)), Convert.FromBase64String(payload)));
    }
    public static byte[] decrypt(byte[] key, byte[] data)
    {
        if (data == null || data.Length <= IV_LENGTH + HMAC_LENGTH)
        {
            throw new ArgumentException("Bad input data", "data");
        }
        byte[] hmac = new byte[HMAC_LENGTH];
        Array.Copy(data, 0, hmac, 0, HMAC_LENGTH);
        byte[] iv = new byte[IV_LENGTH];
        Array.Copy(data, HMAC_LENGTH, iv, 0, IV_LENGTH);
        byte[] payload = new byte[data.Length - HMAC_LENGTH - IV_LENGTH];
        Array.Copy(data, HMAC_LENGTH + IV_LENGTH, payload, 0, payload.Length);
        //if (byteArrayEquals(hmac, dohmac(key, byteArrayConcat(iv, payload)))) {
        // TODO: Handle HMAC validation failure 
        //}
        Aes aes = new AesManaged();
        aes.BlockSize = 128;
        aes.KeySize = 256;
        aes.Key = hash(key);
        aes.IV = iv;
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;
        MemoryStream ms = new MemoryStream();
        CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write);
        cs.Write(payload, 0, payload.Length);
        cs.FlushFinalBlock();
        return ms.ToArray();
    }
    public static byte[] hash(byte[] key)
    {
        return (new SHA256Managed()).ComputeHash(key);
    }
    public static byte[] dohmac(byte[] key, byte[] data)
    {
        return (new HMACSHA256(key)).ComputeHash(data);
    }

    public string GetHashSha256(string text)
    {
        byte[] bytes = Encoding.ASCII.GetBytes(text);
        SHA256Managed hashstring = new SHA256Managed();
        byte[] hash = hashstring.ComputeHash(bytes);
        string hashString = string.Empty;

        foreach (byte x in hash)
        {
            hashString += String.Format("{0:x2}", x);
        }
        return hashString;
    }

    public string UpdatePaymentInfo(string callid, string brandCode, string orderId, string total, string subtotal, string discount, string shipping, string tax, string promoCode)
    {
        try
        {
            string json = "{\"orderInfo\" : {\"total\":\"" + total + "\"," +
                           "\"currencyCode\":\"USD\"," +
                           "\"subtotal\":\"" + subtotal + "\"," +
                           "\"shippingHandling\":\"" + shipping + "\"," +
                           "\"tax\":\"" + tax + "\"," +
                            "\"discount\":\"" + discount + "\"," +
                           "\"eventType\":\"Confirm\"," +
                           "\"orderId\":\"" + orderId + "\"," +
                           "\"reason\":\"Order Successfully Created\"}}";

            var appSettings = ConfigurationManager.AppSettings;
            string secret = appSettings["secretkey" + brandCode];
            string apikey = appSettings["apikey" + brandCode];

            string time = Convert.ToString((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds);
            string resource = "payment/info/" + callid;
            string query_string = "apikey=" + apikey;
            string token = secret + time + resource + query_string + json;
            string hashtoken = "x:" + time + ":" + this.GetHashSha256(token);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            string url = "https://secure.checkout.visa.com/wallet-services-web/" + resource + "?" + query_string;
            HttpWebRequest request;
            request = (HttpWebRequest)WebRequest.Create(url);

            request.Headers["X-PAY-TOKEN"] = hashtoken;
            request.Accept = "application/xml";
            request.ContentType = "application/json";
            request.Method = "PUT";


            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {

                streamWriter.Write(json);

            }

            var httpResponse = (HttpWebResponse)request.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var responseText = streamReader.ReadToEnd();
            }
            return "Success";
        }
        catch(Exception ex)
        {
            return "fail:" + ex.Message;

        }
        

    }

}

public class CardInfo
{
    public string _creditCardNumber;
    public string _cardBrand;
    public string _cardExpMonth;
    public string _cardExpYear;
    public string _cardAddress;
    public string _cardZip;
    public string _message;
    public CardInfo(string creditCardNumber, string cardBrand, string cardExpMonth, string cardExpYear, string cardAddress, string cardZip, string message)
    {
        this._creditCardNumber = creditCardNumber;
        this._cardBrand = cardBrand;
        this._cardExpMonth = cardExpMonth;
        this._cardExpYear = cardExpYear;
        this._cardAddress = cardAddress;
        this._cardZip = cardZip;
        this._message = message;

    }



}