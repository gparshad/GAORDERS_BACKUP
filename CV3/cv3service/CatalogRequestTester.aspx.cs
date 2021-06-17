using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CatalogRequestTester : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string method = Request["method"] != null ? Request["method"].ToString() : "";
        if (method.ToLower() == "newsletter")
        {
testing.Text = "this happened 0";

            RedBackLibraryTester rb = new RedBackLibraryTester();
            string title = Request["title"] != null ? Request["title"].ToString() : "";
            string email = Request["email"] != null ? Request["email"].ToString() : "";
            string emip = Request["emip"] != null ? Request["emip"].ToString() : "";
            string optout = Request["optout"] != null ? Request["optout"].ToString() : "";
            string keycode = Request["keycode"] != null ? Request["keycode"].ToString() : "";
            string errors = "";
Response.Write("this happened 1");
            Response.Write(rb.NewsletterSignup(title, email, emip, optout, keycode, ref errors));
Response.Write("this happened 2");
            Helpers.LogRequest(title, "email", String.Format("{0} [email:{1}] [method:{2}] [keycode:{3}] [redback:{4}]", DateTime.Now.ToString("s"), email, method, keycode, errors));
Response.Write("this happened 3");
        }
        else if (method.ToLower() == "catalog")
        {
            RedBackLibraryTester rb = new RedBackLibraryTester();
            string title = Request["title"] != null ? Request["title"].ToString() : "";
            string firstname = Request["firstname"] != null ? Request["firstname"].ToString() : "";
            string lastname = Request["lastname"] != null ? Request["lastname"].ToString() : "";
            string company = Request["company"] != null ? Request["company"].ToString() : "";
            string address1 = Request["address1"] != null ? Request["address1"].ToString() : "";
            string address2 = Request["address2"] != null ? Request["address2"].ToString() : "";
            string city = Request["city"] != null ? Request["city"].ToString() : "";
            string state = Request["state"] != null ? Request["state"].ToString() : "";
            string zip = Request["zip"] != null ? Request["zip"].ToString() : "";
            string country = Request["country"] != null ? Request["country"].ToString() : "";
            string email = Request["email"] != null ? Request["email"].ToString() : "";
            string emip = Request["emip"] != null ? Request["emip"].ToString() : "";
            string phone = Request["phone"] != null ? Request["phone"].ToString() : "";
            string notes = Request["notes"] != null ? Request["notes"].ToString() : "";
            string optout = Request["optout"] != null ? Request["optout"].ToString() : "";
            string keycode = Request["keycode"] != null ? Request["keycode"].ToString() : "";
            string errors = "";
            Response.Write(rb.CatalogRequest(title, firstname, lastname, company, address1, address2, city, state, zip, country, email, emip, phone, notes, optout, keycode, ref errors));
            Helpers.LogRequest(title, "catalog", String.Format("{0} [email:{1}] [name:{2}] [keycode:{3}] [redback:{4}]", DateTime.Now.ToString("s"), email, firstname + " " + lastname, keycode, errors));
        }
    }
}
