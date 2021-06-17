using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using System.Configuration;

/// <summary>
/// Summary description for Process
/// </summary>
[WebService(Namespace = "http://gaorders1/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class DataProcess : System.Web.Services.WebService {

    public DataProcess()
    {
        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public string OrderImportRange(string serviceID, string brandCode, string orderPrefix, string keycode, int start, int end)
    {
        CV3Library cv3 = new CV3Library(ConfigurationManager.AppSettings["cv3user"], ConfigurationManager.AppSettings["cv3pass"]);
        RedBackLibrary rb = new RedBackLibrary();
        string rsp = "";

        List<CV3Library.Order> orders = cv3.GetOrdersRange(serviceID, start, end);
        if (orders.Count > 0)
        {
            rsp = rb.SendToRedbackWithDefaultKeycode(ref orders, brandCode, orderPrefix, keycode, false);
            rsp += cv3.UpdateOrders(ref orders, serviceID);
        }
        Helpers.LogOrders(orders, serviceID, brandCode);
        return rsp;
    }
    [WebMethod]
    public string OrderImportRangeTokenize(string serviceID, string brandCode, string orderPrefix, string keycode, int start, int end)
    {
        CV3Library cv3 = new CV3Library(ConfigurationManager.AppSettings["cv3user"], ConfigurationManager.AppSettings["cv3pass"]);
        RedBackLibrary rb = new RedBackLibrary();
        string rsp = "";

        List<CV3Library.Order> orders = cv3.GetOrdersRange(serviceID, start, end);
        if (orders.Count > 0)
        {
            rsp = rb.SendToRedbackWithDefaultKeycode(ref orders, brandCode, orderPrefix, keycode, true);
            rsp += cv3.UpdateOrders(ref orders, serviceID);
        }
        Helpers.LogOrders(orders, serviceID, brandCode);
        return rsp;
    }
    [WebMethod]
    public string OrderImport(string serviceID, string brandCode, string orderPrefix)
    {
        CV3Library cv3 = new CV3Library(ConfigurationManager.AppSettings["cv3user"], ConfigurationManager.AppSettings["cv3pass"]);
        RedBackLibrary rb = new RedBackLibrary();
        string rsp = "";

        List<CV3Library.Order> orders = cv3.GetOrders(serviceID);
        if (orders.Count > 0)
        {
            rsp = rb.SendToRedbackWithDefaultKeycode(ref orders, brandCode, orderPrefix, "", false);
            rsp += cv3.UpdateOrders(ref orders, serviceID);
        }
        Helpers.LogOrders(orders, serviceID, brandCode);
        return rsp;
    }

    [WebMethod]
    public string OrderImportTokenize(string serviceID, string brandCode, string orderPrefix)
    {
        CV3Library cv3 = new CV3Library(ConfigurationManager.AppSettings["cv3user"], ConfigurationManager.AppSettings["cv3pass"]);
        RedBackLibrary rb = new RedBackLibrary();
        string rsp = "";

        List<CV3Library.Order> orders = cv3.GetOrders(serviceID);
        if (orders.Count > 0)
        {
            rsp = rb.SendToRedbackWithDefaultKeycode(ref orders, brandCode, orderPrefix, "", true);
            rsp += cv3.UpdateOrders(ref orders, serviceID);
        }
        Helpers.LogOrders(orders, serviceID, brandCode);
        return rsp;
    }

    [WebMethod]
    public string OrderImportWithDefaultKeycode(string serviceID, string brandCode, string orderPrefix, string keycode)
    {
        CV3Library cv3 = new CV3Library(ConfigurationManager.AppSettings["cv3user"], ConfigurationManager.AppSettings["cv3pass"]);
        RedBackLibrary rb = new RedBackLibrary();
        string rsp = "";

        List<CV3Library.Order> orders = cv3.GetOrders(serviceID);
        if (orders.Count > 0)
        {
            rsp = rb.SendToRedbackWithDefaultKeycode(ref orders, brandCode, orderPrefix, keycode, false);
            rsp += cv3.UpdateOrders(ref orders, serviceID);
        }
        Helpers.LogOrders(orders, serviceID, brandCode);
        Helpers.LogRequest(brandCode, "debug", rsp);
        return rsp;
    }
    [WebMethod]
    public string OrderImportWithDefaultKeycodeTokenize(string serviceID, string brandCode, string orderPrefix, string keycode)
    {
        CV3Library cv3 = new CV3Library(ConfigurationManager.AppSettings["cv3user"], ConfigurationManager.AppSettings["cv3pass"]);
        RedBackLibrary rb = new RedBackLibrary();
        string rsp = "";

        List<CV3Library.Order> orders = cv3.GetOrders(serviceID);
        if (orders.Count > 0)
        {
            rsp = rb.SendToRedbackWithDefaultKeycode(ref orders, brandCode, orderPrefix, keycode, true);
            rsp += cv3.UpdateOrders(ref orders, serviceID);
        }
        Helpers.LogOrders(orders, serviceID, brandCode);
        Helpers.LogRequest(brandCode, "debug", rsp);
        return rsp;
    }
}

