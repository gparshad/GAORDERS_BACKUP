using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Xml;
using System.IO;

public partial class ImportOrderNew : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string order = Request.Form["order"] == null ? "" : Request.Form["order"].ToString();
        string title = Request.Form["title"] == null ? "" : Request.Form["title"].ToString();
        string service = Request.Form["service"] == null ? "" : Request.Form["service"].ToString();
        string prefix = Request.Form["prefix"] == null ? "" : Request.Form["prefix"].ToString();
        string token = Request.Form["token"] == null ? "" : Request.Form["token"].ToString();
        bool tokenize = false;
        if(token == "true")
        {
            tokenize = true;

        }
        CV3Library cv3 = new CV3Library(ConfigurationManager.AppSettings["cv3user"], ConfigurationManager.AppSettings["cv3pass"]);
        RedBackLibrary rb = new RedBackLibrary();
        string rsp = "";

        List<CV3Library.Order> orders = cv3.GetOrdersRange(service, Convert.ToInt32(order), Convert.ToInt32(order));
        if (orders.Count > 0)
        {
            rsp = rb.SendToRedbackWithDefaultKeycode(ref orders, title, prefix, "", tokenize);
            string cv3resp = cv3.UpdateOrders(ref orders, service);
        }
        Helpers.LogOrders(orders, service, title);
        
        //Response.Write(rsp);

	 string ordersNumbers = "";
        using (XmlReader reader = XmlReader.Create(new StringReader(rsp)))
        {
            while (reader.Read())
            {
                // Only detect start elements.
                if (reader.IsStartElement())
                {
                    // Get element name and switch on it.
                    switch (reader.Name)
                    {
                      
                        case "OrderNumber":
                            // Detect this article element.
                            if(ordersNumbers != "")
                            {
                                ordersNumbers += ", ";
                            }
                            if (reader.Read())
                            {
                                ordersNumbers += reader.Value.Trim();
                            }
                            break;
                    }
                }
            }
        }

        string rspNew = "<RedBackResponse>";
        rspNew += "<Order>";
        rspNew += "<OrderNumber>";
        rspNew += ordersNumbers;
        rspNew += "</Order>";
        rspNew += "</OrderNumber>";
        rspNew += "</RedBackResponse>";
 

        Response.Write(rspNew);


    }
}