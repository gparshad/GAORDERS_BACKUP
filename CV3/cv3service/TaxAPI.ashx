<%@ WebHandler Language="C#" Class="TaxAPI" Debug="true" %>
using System.Text;
using System.IO;
using System.Web;
using System.Collections.Generic;
using System.Data;
using System;
using Newtonsoft.Json;
using System.Linq;
using Newtonsoft.Json.Linq;
using TaxWebAPI.RedBackLibrary;
using System.Diagnostics;


public class TaxAPI : IHttpHandler
{
    public class RequestData
    {
        public string data;
    }

    public class Data
    {
        public string message;
        public string timestamp;
    }

    public class Message
    {
        public Order_Info orderInfo { get; set; }
        public All_Cart_Products cart { get; set; }
        public string storeName { get; set; }
        public string tmp_order_id { get; set; }
        public string price_cat { get; set; }
        public string custfields { get; set; }
        public Origin_Address origin_address { get; set; }
    }

    public class Origin_Address
    {
        public string origin_name { get; set; }
        public string origin_line_1 { get; set; }
        public string origin_line_2 { get; set; }
        public string origin_city { get; set; }
        public string origin_county { get; set; }
        public string origin_postal { get; set; }
        public string origin_state { get; set; }
        public string origin_country { get; set; }
    }


    public class Order_Info
    {
        public Shipping_Info shipping_info { get; set; }
        public Billing_Info billing_info { get; set; }
        public Totals totals { get; set; }
        public List<Product> productList { get; set; }
    }

    public class All_Cart_Products
    {
        public List<Product> productList { get; set; }
    }

    public class Shipping_Info
    {
        public string s_address1 { get; set; }
        public string s_address2 { get; set; }
        public string s_city { get; set; }
        public string s_state { get; set; }
        public string s_zip { get; set; }
        public string s_country { get; set; }
    }

    public class Billing_Info
    {
        public string billing_address1 { get; set; }
        public string billing_address2 { get; set; }
        public string billing_city { get; set; }
        public string billing_state { get; set; }
        public string billing_zip { get; set; }
        public string billing_country { get; set; }
        public string billing_email { get; set; }
    }

    public class Totals
    {
        public string total { get; set; }
        public string shipping { get; set; }
    }

    public class Product
    {
        public string sku { get; set; }
        public int qty { get; set; }
        public string price { get; set; }
        public string exempt { get; set; }
        public int cart_id { get; set; }
        public string tax_code { get; set; }
        public string gift_wrap { get; set; }
        public Gift_Wrap_Info gift_wrap_info { get; set; }
    }

    public class Gift_Wrap_Info
    {
        public string sku { get; set; }
        public int qty { get; set; }
        public string price { get; set; }
    }

    public HttpContext context;
    public HttpRequest request;
    public HttpResponse response;
    private Message message;
    private TaxInfo taxInfo;
    private Dictionary<string, string> customFields;
    private const string ResponseTimeKey = "RespTime";

    public void ProcessRequest(HttpContext _context)
    {
        context = _context;
        context.Response.ContentType = "application/json";
        context.Response.ContentEncoding = Encoding.UTF8;
        context.Items[ResponseTimeKey] = Stopwatch.StartNew();

        StreamReader stream = new StreamReader(context.Request.InputStream);
        string x = stream.ReadToEnd();
        string base64Encoded = string.Empty;
        string step = String.Empty;

        try
        {
            step = "Deserialize RequestData";
            RequestData req = JsonConvert.DeserializeObject<RequestData>(x);
            base64Encoded = req.data;
            //step = "Log encoded data";
            //LogTaxRawRequest(base64Encoded);
            byte[] data = Convert.FromBase64String(base64Encoded);
            string base64Decoded = ASCIIEncoding.ASCII.GetString(data);

            step = "Deserialize data";
            Data objData = JsonConvert.DeserializeObject<Data>(base64Decoded);
            JObject jsonRequest = JObject.Parse(objData.message);

            step = "Retieve Order Information";
            var order_Info = jsonRequest.Value<JObject>("order_info").Properties().FirstOrDefault().Value.ToString();
            var map_id = jsonRequest.Value<JObject>("order_info").Properties().FirstOrDefault().Name;
            JObject jsonOrder = JObject.Parse(order_Info);
            var products = jsonOrder.Value<JObject>("products").Properties();
            List<Product> productList = new List<Product>();
            foreach (var product in products)
            {
                var prod = JsonConvert.DeserializeObject<Product>(product.Value.ToString());
                if (prod.gift_wrap == "y")
                {
                    prod.sku = prod.gift_wrap_info.sku;
                    prod.price = string.Format("{0:N2}", Convert.ToDecimal(prod.price) + Convert.ToDecimal(prod.gift_wrap_info.price));
                }
                productList.Add(prod);
            }
            message = JsonConvert.DeserializeObject<Message>(objData.message);
            message.orderInfo = JsonConvert.DeserializeObject<Order_Info>(order_Info);
            message.orderInfo.productList = productList;

            step = "Retrieve All Cart Products";
            var all_cart_products = jsonRequest.Value<JObject>("all_cart_products").Properties();
            List<Product> cartProductList = new List<Product>();
            foreach (var product in all_cart_products)
            {
                var prod = JsonConvert.DeserializeObject<Product>(product.Value.ToString());
                if (prod.gift_wrap == "y")
                {
                    prod.sku = prod.gift_wrap_info.sku;
                    prod.price = string.Format("{0:N2}", Convert.ToDecimal(prod.price) + Convert.ToDecimal(prod.gift_wrap_info.price));
                }
                cartProductList.Add(prod);
            }

            message.cart = new All_Cart_Products
            {
                productList = cartProductList
            };
            step = "Retrieve Custom Fields";
            customFields = new Dictionary<string, string>();
            if (message.custfields != null)
            {
                byte[] customData = Convert.FromBase64String(message.custfields);
                string base64DecodedCustomData = ASCIIEncoding.ASCII.GetString(customData);
                string arrayCustomstring = base64DecodedCustomData.Substring(base64DecodedCustomData.IndexOf('{') + 1);
                string[] arrayCustom = arrayCustomstring.Replace("\"", "").Split(';');

                for (int i = 0; i < arrayCustom.Length - 1; i = i + 2)
                {
                    string key = arrayCustom[i].Split(':').Last();
                    string value = arrayCustom[i + 1].Split(':').Last();
                    customFields.Add(key, value);
                }

            }

            step = "Get TaxInfo";
            var tax = GetTaxInfo();

            step = "Prepare Response";
            var taxResponse = new Dictionary<string, Dictionary<string, string>>
            {
                { map_id, tax}
            };

            var response = new Dictionary<string, string> {
            { "data", Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(JsonConvert.SerializeObject(taxResponse)))}
            };

            var return_value = new Dictionary<string, object> {
            { "return_value", response}
            };

            step = "Log Response";
            //LogTaxRequestResponse(message.storeName, base64Encoded, objData.message, JsonConvert.SerializeObject(taxResponse), JsonConvert.SerializeObject(customFields), JsonConvert.SerializeObject(taxInfo));
            Stopwatch stopwatch = (Stopwatch)context.Items[ResponseTimeKey];
            var timeElapsed = stopwatch.Elapsed.Milliseconds;
            LogTaxRequestResponse(message.storeName, base64Encoded, objData.message, JsonConvert.SerializeObject(taxResponse), JsonConvert.SerializeObject(customFields), JsonConvert.SerializeObject(taxInfo), timeElapsed);

            step = "Write Response";
            context.Response.Write(JsonConvert.SerializeObject(return_value));
        }
        catch (Exception ex)
        {
            LogTaxExceptions(ex.Message + "\r\n" + step + "\r\n" + base64Encoded);
        }
    }

    public static void LogTaxRequestResponse(string storeName, string rawData, string requestData, string response, string customFields, string taxAPIObject, int timeElapsed)
    {
        string logFile = "C:\\GAORDERS\\TaxLogs\\" + storeName + "\\" + storeName + "-" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
        try
        {
            var customInfo = JsonConvert.SerializeObject(customFields);
            using (StreamWriter w = File.AppendText(logFile))
            {
                w.WriteLine(DateTime.Now);
                w.WriteLine("Time Elapsed(ms): " + timeElapsed);
                w.WriteLine(rawData);
                w.WriteLine(requestData);
                w.WriteLine(customFields);
                w.WriteLine(taxAPIObject);
                w.WriteLine(response);
                w.WriteLine("==================================================");
                w.Close();
            }
        }
        catch (Exception ex)
        {
            LogTaxExceptions(ex.Message + "\r\n" + storeName + "\r\n" + rawData + "\r\n" + requestData + "\r\n" + response);
        }

    }

    public static void LogTaxRawRequest(string requestData)
    {
        string logFile = "C:\\GAORDERS\\TaxLogs\\RawRequest\\tax-raw-" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
        using (StreamWriter w = File.AppendText(logFile))
        {
            w.WriteLine(DateTime.Now);
            w.WriteLine(requestData);
            w.WriteLine("==================================================");
            w.Close();
        }
    }

    public static void LogTaxExceptions(string message)
    {
        string logFile = "C:\\GAORDERS\\TaxLogs\\Exceptions\\tax-exceptions-" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
        using (StreamWriter w = File.AppendText(logFile))
        {
            w.WriteLine(DateTime.Now);
            w.WriteLine(message);
            w.WriteLine("==================================================");
            w.Close();
        }
    }
    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

    public Dictionary<string, string> GetTaxInfo()
    {
        RedBackLibraryTax rb = new RedBackLibraryTax();
        var CalculationTypes = new Dictionary<string, string>
        {
            { "gardens", "totalorder"},
            { "shdesktop", "shipto"},
            { "shmobile", "shipto"},
            { "gurneysdesktop", "shipto"},
            { "gurneysmobile", "shipto"},
            { "henryfields", "shipto"},
            { "brecks", "shipto"},
            { "brecksredo", "shipto"},
            { "brecksmobile", "shipto"},
            { "mbdesktop", "shipto"},
            { "mbmobile", "shipto"},
            { "bitsus", "totalorder"},
            { "bitsusmobile", "totalorder"},
            { "paragon", "totalorder"},
            { "spilsburyus", "totalorder"},
            { "spilsburybuild", "totalorder"},
            { "spilsburymobile", "totalorder"},
            { "kvbwholesale", "shipto"},
            { "brgifts", "totalorder"},
            { "windspinners", "totalorder"}

        };
        var orderInfo = message.orderInfo;
        var cartProducts = message.cart.productList;
        var data = new Dictionary<string, string>();
        var valid = false;
        var sliceError = false;
        var numProducts = orderInfo.productList.Count();
        var isFreightCall = true;
        var calculationType = CalculationTypes[message.storeName];
        if (calculationType == "totalorder" && numProducts != 0)
        {
            isFreightCall = false;
        }
        if (isFreightCall)
        {
            taxInfo = PopulateTaxInfoFromOrder(orderInfo, cartProducts);
            valid = IsValidSliceRequest();
            if (valid)
            {
                taxInfo = rb.GetTaxInformation(taxInfo);
                if (String.IsNullOrEmpty(taxInfo.StatusError))
                {
                    data.Add("tax", taxInfo.SalesTaxAmt);
                }
                else
                {
                    sliceError = true;
                    taxInfo.StatusMsg = taxInfo.StatusMsg + " :ERROR";
                }
            }
            else
            {
                taxInfo.StatusMsg = "Missing Slice Required Fields :ERROR";
            }

        }

        if (!isFreightCall || !valid || sliceError)
        {
            data.Add("tax", "0");
        }

        return data;

    }

    private bool IsValidSliceRequest()
    {
        var isValid = true;
        if (String.IsNullOrEmpty(taxInfo.Title)
                || String.IsNullOrEmpty(taxInfo.ShipAddr1)
                || String.IsNullOrEmpty(taxInfo.ShipCity)
                || String.IsNullOrEmpty(taxInfo.ShipState)
                || String.IsNullOrEmpty(taxInfo.ShipZip)
                || String.IsNullOrEmpty(taxInfo.WebReference)
                || String.IsNullOrEmpty(taxInfo.Items))
            isValid = false;

        if (String.IsNullOrEmpty(taxInfo.WebReference) && !String.IsNullOrEmpty(taxInfo.ShipAddr1))
            LogTaxExceptions("WebREF Missing" + "\r\n" + JsonConvert.SerializeObject(taxInfo));
        return isValid;
    }

    private TaxInfo PopulateTaxInfoFromOrder(Order_Info orderInfo, List<Product> cartProducts)
    {
        var Titles = new Dictionary<string, string>
        {
            { "gardens", "1"},
            { "shdesktop", "4"},
            { "shmobile", "4"},
            { "gurneysdesktop", "5"},
            { "gurneysmobile", "5"},
            { "henryfields", "6"},
            { "brecks", "7"},
            { "brecksredo", "7"},
            { "brecksmobile", "7"},
            { "mbdesktop", "8"},
            { "mbmobile", "8"},
            { "bitsus", "9"},
            { "bitsusmobile", "9"},
            { "paragon", "10"},
            { "spilsburyus", "19"},
            { "spilsburybuild", "19"},
            { "spilsburymobile", "19"},
            { "kvbwholesale", "36"},
            { "brgifts", "52"},
            { "windspinners", "66"}

        };

        var accountNumber = customFields.Where(k => k.Key.Contains("accountnum")).FirstOrDefault().Value;
        var couponAmount = customFields.Where(k => k.Key.Contains("promodisc")).FirstOrDefault().Value;
        var isFreightCall = orderInfo.productList.Count() == 0;
        var clubmember = String.Empty;
        if (message.storeName == "gardens")
            clubmember = customFields.Where(k => k.Key == "clubmember").FirstOrDefault().Value;

        var taxInfo = new TaxInfo()
        {
            Title = Titles[message.storeName],
            ShipAddr1 = orderInfo.shipping_info.s_address1,
            ShipAddr2 = orderInfo.shipping_info.s_address2,
            ShipCity = orderInfo.shipping_info.s_city,
            ShipState = orderInfo.shipping_info.s_state,
            ShipZip = orderInfo.shipping_info.s_zip,
            Items = orderInfo.productList == null ? "" : string.Join("ý", orderInfo.productList.Select(p => p.sku)),
            QtyOrdered = orderInfo.productList == null ? "" : string.Join("ý", orderInfo.productList.Select(p => p.qty)),
            UnitPrice = orderInfo.productList == null ? "" : string.Join("ý", orderInfo.productList.Select(p => string.Format("{0:N2}", Convert.ToDecimal(p.price)))),
            StdFreightAmt = string.Format("{0:N2}", Convert.ToDecimal(orderInfo.totals.shipping)),
            XtraFreightAmt = "0.00",
            WebReference = message.tmp_order_id,
            //WebReference = customFields.Where(k => k.Key.Contains("taxwebref")).FirstOrDefault().Value,
            CustNumber = string.IsNullOrEmpty(accountNumber) ? "GUEST" : accountNumber,
            CouponAmt = string.IsNullOrEmpty(couponAmount) ? "0.00" : string.Format("{0:N2}", Convert.ToDecimal(couponAmount)),
            ClubDiscount = "0.00"
        };
        if (isFreightCall)
        {
            taxInfo.Items = cartProducts == null ? "" : string.Join("ý", cartProducts.Select(p => p.sku));
            taxInfo.QtyOrdered = cartProducts == null ? "" : string.Join("ý", cartProducts.Select(p => p.qty));
            taxInfo.UnitPrice = cartProducts == null ? "" : string.Join("ý", cartProducts.Select(p => string.Format("{0:N2}", Convert.ToDecimal(p.price))));
        }
        if (message.storeName == "gardens")
        {
            if (clubmember == "YES")
            {
                taxInfo.StdFreightAmt = "0.00";
                taxInfo.XtraFreightAmt = "0.00";
                taxInfo.ClubDiscount = "10.00";
            }
            else
            {
                var shptotal = customFields.Where(k => k.Key == "shptotal").FirstOrDefault().Value;
                if (!string.IsNullOrEmpty(shptotal))
                    taxInfo.XtraFreightAmt = string.Format("{0:N2}", Convert.ToDecimal(shptotal) - Convert.ToDecimal(taxInfo.StdFreightAmt));
            }
        }

        if (message.storeName == "brgifts")
        {

            var shptotal = customFields.Where(k => k.Key == "shptotal").FirstOrDefault().Value;
            if (!string.IsNullOrEmpty(shptotal))
                taxInfo.XtraFreightAmt = string.Format("{0:N2}", Convert.ToDecimal(shptotal) - Convert.ToDecimal(taxInfo.StdFreightAmt));

        }
        return taxInfo;
    }
}