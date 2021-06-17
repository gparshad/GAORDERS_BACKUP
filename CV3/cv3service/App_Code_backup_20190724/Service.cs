using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;

/// <summary>
/// Summary description for Service
/// </summary>
[WebService(Namespace = "http://gaorders1/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class Service : System.Web.Services.WebService {

    public Service () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public string CatalogRequest(string title, string firstname, string lastname, string company, string address1, string address2, string city, string state, string zip, string country, string email, string emip, string phone, string notes, string optout, string keycode)
    {
        RedBackLibrary rb = new RedBackLibrary();
        string errors = "";
        string rsp = rb.CatalogRequest(title, firstname, lastname, company, address1, address2, city, state, zip, country, email, emip, phone, notes, optout, keycode, ref errors);
        Helpers.LogRequest(title, "catalog", String.Format("{0} [email:{1}] [name:{2}] [keycode:{3}] [redback:{4}]", DateTime.Now.ToString("s"), email, firstname + " " + lastname, keycode, errors));
        return rsp;
    }

    [WebMethod]
    public string NewsletterSignup(string title, string email, string emip, string optout, string keycode)
    {
        RedBackLibrary rb = new RedBackLibrary();
        string errors = "";
        string rsp = rb.NewsletterSignup(title, email, emip, optout, keycode, ref errors);
        string method = (optout.ToLower() == "w") ? "OptOutRequest" : "CatRequest";
        Helpers.LogRequest(title, "email", String.Format("{0} [email:{1}] [method:{2}] [keycode:{3}] [redback:{4}]", DateTime.Now.ToString("s"), email, method, keycode, errors));
        return rsp;
    }

    [WebMethod]
    public string OrderStatus(string title, string custnumber, string custzip, string ordernumber)
    {
        RedBackLibrary rb = new RedBackLibrary();
        string rsp = "";
        if (ordernumber != "")
            rsp = rb.FetchOrder(custnumber, custzip, title, ordernumber);
        else
            rsp = rb.FetchOrders(custnumber, custzip, title);
        return rsp;
    }

    [WebMethod]
    public string AccessCustomer(string custnumber, string custzip)
    {
        RedBackLibrary rb = new RedBackLibrary();
        return rb.FetchCustomer(custnumber, custzip);
    }
}

