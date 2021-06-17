<%@ WebHandler Language="C#" Class="CancelOrder" %>



using System.Text;
using System.IO;
using System.Web;
using System.Runtime.Serialization.Json;
using System.Collections.Generic;
using System.Data;
using System;
using System.Data.SqlClient;
using Newtonsoft.Json;
using System.Linq;

public class CreateCancelLogFiles
{
    private string sLogFormat;
    private string sErrorTime;

    public CreateCancelLogFiles()
    {
        //sLogFormat used to create log files format :
        // dd/mm/yyyy hh:mm:ss AM/PM ==> Log Message
        sLogFormat = DateTime.Now.ToShortDateString().ToString() + " " + DateTime.Now.ToLongTimeString().ToString() + " ==> ";

        //this variable used to create log filename format "
        //for example filename : ErrorLogYYYYMMDD
        string sYear = DateTime.Now.Year.ToString();
        string sMonth = DateTime.Now.Month.ToString();
        string sDay = DateTime.Now.Day.ToString();
        sErrorTime = sYear + sMonth + sDay;
    }

    public void ErrorLog(string sPathName, string sErrMsg)
    {
        StreamWriter sw = new StreamWriter(sPathName + sErrorTime, true);
        sw.WriteLine(sLogFormat + sErrMsg);
        sw.Flush();
        sw.Close();
    }
}


public class CancelOrder : IHttpHandler
{

    public class CancelOrderInfo
    {

        public string accountnumber;
        public string ordernumber;
        public string response;
        public string errormessage;
    }

    public class CancelOrderRequest
    {

        public string accountnumber;
        public string ordernumber;
        public string storename;
        public string title;
        public string cancelcode;
    }

    public HttpContext context;
    public HttpRequest request;
    public HttpResponse response;


    public CancelOrderRequest cancelOrderRequest;
    public string pathName;
      
    public void handleRequest()
    {
        //writeRaw("this is a message");
        writeJson(CancelOrderInformation);
    }

    public void ProcessRequest(HttpContext _context)
    {
        var storeBrandMapping = new Dictionary<string, string>
        {
            { "gardens", "1"},
            { "shdesktop", "4"},
            { "gurneysdesktop", "5"},
            { "brecks", "7"},
			{ "brecksredo", "7"},
            { "brecksmobile", "7"},
			{ "kvbwholesale","36"},
			{ "bitsus","9"},
			{ "bitsca","16"},
			{ "spilsburybuild","19"},
            { "mbdesktop", "8"},
			{ "brgifts", "52"}
        };
        context = _context;
        request = _context.Request;
        response = _context.Response;
        context.Response.ContentType = "application/json";
        context.Response.ContentEncoding = Encoding.UTF8;
        cancelOrderRequest = new CancelOrderRequest()
        {
            accountnumber = context.Request["accnum"] == null ? "" : context.Request["accnum"],
            ordernumber = context.Request["ordernum"] == null ? "" : context.Request["ordernum"],
            storename = context.Request["storename"] == null ? "" : context.Request["storename"],
            cancelcode = context.Request["cancelcode"] == null ? "" : context.Request["cancelcode"],
            title = string.Empty
        };

        if (!string.IsNullOrEmpty(cancelOrderRequest.storename))
        {
            string title;
            bool isStoreMapped = storeBrandMapping.TryGetValue(cancelOrderRequest.storename, out title);
            if (!string.IsNullOrEmpty(title))
                cancelOrderRequest.title = title;
        }

        pathName = HttpContext.Current.Server.MapPath("Logs/CancelRequests/CancelOrderLog");

        handleRequest();

    }

    public void writeJson(object _object)
    {
        //JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
        //string jsondata = javaScriptSerializer.Serialize(_object);
        //writeRaw(jsondata );
        context.Response.AppendHeader("Access-Control-Allow-Origin", "*");
        MemoryStream stream1 = new MemoryStream();
        DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(CancelOrderInfo));
        jsonSerializer.WriteObject(stream1, _object);
        stream1.Position = 0;
        StreamReader sr = new StreamReader(stream1);
        writeRaw(sr.ReadToEnd());
    }

    public void writeRaw(string text)
    {
        context.Response.Write(text);
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

    public CancelOrderInfo CancelOrderInformation
    {
        get
        {
            return CancelWebOrder();
        }
    }

    bool IsDigitsOnly(string str)
    {
        foreach (char c in str)
        {
            if (c < '0' || c > '9')
                return false;
        }

        return true;
    }

    public CancelOrderInfo CancelWebOrder()
    {

        var errorMapping = new Dictionary<string, string>
        {
            { "5", "We are sorry your order cannot be cancelled. It has been printed."},
            { "6", "We are sorry your order cannot be cancelled. It has been batched."},
            { "7", "We are sorry your order cannot be cancelled. It has been shipped."},
            { "9", "We are sorry fundraiser order cannot be cancelled."},
            { "10", "We are sorry B2B order cannot be cancelled."},
            { "-1", "We are having trouble connecting with our servers, please try again later or contact our customer care."},
        };

        string[] validCancelCodes = { "4", "7", "8", "9", "22" };

        //throw new ArgumentOutOfRangeException();
        CancelOrderInfo orderInfo = new CancelOrderInfo();
        orderInfo.accountnumber = cancelOrderRequest.accountnumber;
        orderInfo.ordernumber = cancelOrderRequest.ordernumber;
        orderInfo.response = "S";
        // validate fields before sending to slice
       
        bool isNumericOrder = IsDigitsOnly(cancelOrderRequest.ordernumber);
        bool isNumericAccount = IsDigitsOnly(cancelOrderRequest.accountnumber);

        if (string.IsNullOrEmpty(cancelOrderRequest.ordernumber) || string.IsNullOrEmpty(cancelOrderRequest.accountnumber) || string.IsNullOrEmpty(cancelOrderRequest.cancelcode) || string.IsNullOrEmpty(cancelOrderRequest.storename))
        {
            orderInfo.response = "F";
            orderInfo.errormessage = "Missing Parameters";
        }
        else if (!isNumericAccount || !isNumericOrder)
        {
            orderInfo.response = "F";
            orderInfo.errormessage = "Non numeric account/order";
        }
        else if (cancelOrderRequest.ordernumber.Length != 11 || cancelOrderRequest.accountnumber.Length != 8)
        {
            orderInfo.response = "F";
            orderInfo.errormessage = "Invalid Parameter Length";
        }
        else if (string.IsNullOrEmpty(cancelOrderRequest.title))
        {
            orderInfo.response = "F";
            orderInfo.errormessage = "Invalid Store";
        }
        else if (!validCancelCodes.Contains(cancelOrderRequest.cancelcode))
        {
            orderInfo.response = "F";
            orderInfo.errormessage = "Invalid CancelCode";
        }

        CreateCancelLogFiles Err = new CreateCancelLogFiles();
        Err.ErrorLog(pathName, "------------------------------------------------------");
        Err.ErrorLog(pathName, "Web Request::" + JsonConvert.SerializeObject(cancelOrderRequest));

        if (orderInfo.response == "S")
        {
            RedBackLibraryGC rb = new RedBackLibraryGC();
            RedBackLibraryGC.CancelOrderResponse rbcancelOrderResp = rb.CancelWebOrder(cancelOrderRequest.accountnumber, cancelOrderRequest.ordernumber, cancelOrderRequest.title, cancelOrderRequest.cancelcode);
            orderInfo.response = rbcancelOrderResp.SuccessFail;
            orderInfo.errormessage = rbcancelOrderResp.StatusMsg;

            Err.ErrorLog(pathName, "Slice Response:" + JsonConvert.SerializeObject(rbcancelOrderResp));

            var errorCode = rbcancelOrderResp.StatusError;
            if (!String.IsNullOrEmpty(errorCode))
            {
                string mappedMessage;
                bool isMessageMapped = errorMapping.TryGetValue(errorCode, out mappedMessage);
                if (isMessageMapped)
                    orderInfo.errormessage = mappedMessage;
                else
                    orderInfo.errormessage = "Invalid Request";
            }
        }
        else
        {
            Err.ErrorLog(pathName, "Invalid Request::" + JsonConvert.SerializeObject(orderInfo));
            orderInfo.errormessage = "Invalid Request";
        }

        Err.ErrorLog(pathName, "Web Response::" + JsonConvert.SerializeObject(orderInfo));
        Err.ErrorLog(pathName, "------------------------------------------------------");
        return orderInfo;
    }

}
