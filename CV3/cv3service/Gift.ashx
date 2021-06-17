<%@ WebHandler Language="C#" Class="Gift" %>



using System.Text;
using System.IO;
using System.Web;
using System.Runtime.Serialization.Json;
using System.Collections.Generic;
using System.Data;
using System;
using System.Data.SqlClient;



public class CreateLogFiles
{
    private string sLogFormat;
    private string sErrorTime;

    public CreateLogFiles()
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


public class Gift : IHttpHandler
{

    public class GiftCardInfo
    {

        public string cardnum;
        public float balance;
        public string response;
        public string message;
    }

    public HttpContext context;
    public HttpRequest request;
    public HttpResponse response;


    public string giftcode;
    public string action;
    public string amt;
    public string title;


    public void handleRequest()
    {
        //writeRaw("this is a message");
        writeJson(GiftCardInformation);
    }

    public void ProcessRequest(HttpContext _context)
    {
        context = _context;
        request = _context.Request;
        response = _context.Response;
        context.Response.ContentType = "application/json";
        context.Response.ContentEncoding = Encoding.UTF8;
        giftcode = context.Request["code"];
        action = context.Request["action"];
        amt = context.Request["amount"];
        //title = context.Request["title"];
        //if (context.Request["cart"] != null)
        //  cartProducts = context.Request["cart"];

        //CreateLogFiles Err = new CreateLogFiles();
        //Err.ErrorLog(HttpContext.Current.Server.MapPath("Logs/ErrorLog"), "Code::" + giftcode);
        //Err.ErrorLog(HttpContext.Current.Server.MapPath("Logs/ErrorLog"), "Action::" + action);
        //Err.ErrorLog(HttpContext.Current.Server.MapPath("Logs/ErrorLog"), "Amount::" + amt);

        handleRequest();

    }

    public void writeJson(object _object)
    {
        //JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
        //string jsondata = javaScriptSerializer.Serialize(_object);
        //writeRaw(jsondata );

        MemoryStream stream1 = new MemoryStream();
        DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(List<GiftCardInfo>));
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

    public List<GiftCardInfo> GiftCardInformation
    {
        get
        {
            return Giftdata();
        }
    }

    public List<GiftCardInfo> Giftdata()
    {

        List<GiftCardInfo> GiftCardInformation = new List<GiftCardInfo>();
        GiftCardInfo gftdata = new GiftCardInfo();
        RedBackLibraryTest rb = new RedBackLibraryTest();
        RedBackLibraryTest.GiftCardInfo rbGftInfo = rb.GiftCerificateInfo(giftcode);

        gftdata.response = rbGftInfo.response;
        try
        {
            if (rbGftInfo.message != "Available")
            {
                gftdata.balance = 0;
                gftdata.response = "error";

            }
            else
            {
                gftdata.balance = float.Parse(rbGftInfo.balance);

            }
        }
        catch
        {
            gftdata.balance = 0;
        }
        gftdata.cardnum = rbGftInfo.cardnum;

        gftdata.message = rbGftInfo.message;
        //if (!String.IsNullOrEmpty(title) && title == "6" && gftdata.balance > 0)
        //{
        //    gftdata.balance = 0;
        //    gftdata.response = "error";
        //}

        GiftCardInformation.Add(gftdata);

        return GiftCardInformation;


    }

}





