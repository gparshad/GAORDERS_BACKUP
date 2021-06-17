using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CatalogRequest : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string method = Request.Form["method"] != null ? Request.Form["method"].ToString() : "";
        if (method.ToLower() == "newsletter")
        {
            RedBackLibrary rb = new RedBackLibrary();
            string title = Request.Form["title"] != null ? Request.Form["title"].ToString() : "";
            string email = Request.Form["email"] != null ? Request.Form["email"].ToString() : "";
            string emip = Request.Form["emip"] != null ? Request.Form["emip"].ToString() : "";
            string optout = Request.Form["optout"] != null ? Request.Form["optout"].ToString() : "";
            string keycode = Request.Form["keycode"] != null ? Request.Form["keycode"].ToString() : "";
            string errors = "";
            Response.Write(rb.NewsletterSignup(title, email, emip, optout, keycode, ref errors));
            Helpers.LogCatRequest(title, "email", String.Format("{0} [email:{1}] [method:{2}] [keycode:{3}] [redback:{4}]", DateTime.Now.ToString("s"), email, method, keycode, errors));
        }
 	else if (method.ToLower() == "sweeps")
        {
            RedBackLibrary rb = new RedBackLibrary();
            string title = Request.Form["title"] != null ? Request.Form["title"].ToString() : "";
            string firstname = Request.Form["firstname"] != null ? Request.Form["firstname"].ToString() : "";
            string lastname = Request.Form["lastname"] != null ? Request.Form["lastname"].ToString() : "";
            string company = Request.Form["company"] != null ? Request.Form["company"].ToString() : "";
            string address1 = Request.Form["address1"] != null ? Request.Form["address1"].ToString() : "";
            string address2 = Request.Form["address2"] != null ? Request.Form["address2"].ToString() : "";
            string city = Request.Form["city"] != null ? Request.Form["city"].ToString() : "";
            string state = Request.Form["state"] != null ? Request.Form["state"].ToString() : "";
            string zip = Request.Form["zip"] != null ? Request.Form["zip"].ToString() : "";
            string country = Request.Form["country"] != null ? Request.Form["country"].ToString() : "";
            string email = Request.Form["email"] != null ? Request.Form["email"].ToString() : "";
            string emip = Request.Form["emip"] != null ? Request.Form["emip"].ToString() : "";
            string phone = Request.Form["phone"] != null ? Request.Form["phone"].ToString() : "";
            string notes = Request.Form["notes"] != null ? Request.Form["notes"].ToString() : "";
            string optout = Request.Form["optout"] != null ? Request.Form["optout"].ToString() : "";
            string keycode = Request.Form["keycode"] != null ? Request.Form["keycode"].ToString() : "";
            string errors = "";
            Response.Write(rb.SweepSignup(title, firstname, lastname, company, address1, address2, city, state, zip, country, email, emip, phone, notes, optout, keycode, ref errors));
            Helpers.LogRequest(title, "sweeps", String.Format("{0} [email:{1}] [name:{2}] [keycode:{3}] [redback:{4}]", DateTime.Now.ToString("s"), email, firstname + " " + lastname, keycode, errors));
        }
        else if (method.ToLower() == "catalog")
        {
            RedBackLibrary rb = new RedBackLibrary();
            string title = Request.Form["title"] != null ? Request.Form["title"].ToString() : "";
            string firstname = Request.Form["firstname"] != null ? Request.Form["firstname"].ToString() : "";
            string lastname = Request.Form["lastname"] != null ? Request.Form["lastname"].ToString() : "";
            string company = Request.Form["company"] != null ? Request.Form["company"].ToString() : "";
            string address1 = Request.Form["address1"] != null ? Request.Form["address1"].ToString() : "";
            string address2 = Request.Form["address2"] != null ? Request.Form["address2"].ToString() : "";
            string city = Request.Form["city"] != null ? Request.Form["city"].ToString() : "";
            string state = Request.Form["state"] != null ? Request.Form["state"].ToString() : "";
            string zip = Request.Form["zip"] != null ? Request.Form["zip"].ToString() : "";
            string country = Request.Form["country"] != null ? Request.Form["country"].ToString() : "";
            string email = Request.Form["email"] != null ? Request.Form["email"].ToString() : "";
            string emip = Request.Form["emip"] != null ? Request.Form["emip"].ToString() : "";
            string phone = Request.Form["phone"] != null ? Request.Form["phone"].ToString() : "";
            string notes = Request.Form["notes"] != null ? Request.Form["notes"].ToString() : "";
            string optout = Request.Form["optout"] != null ? Request.Form["optout"].ToString() : "";
            string keycode = Request.Form["keycode"] != null ? Request.Form["keycode"].ToString() : "";
            string errors = "";
			int zipcode;
			bool isNumeric = int.TryParse(zip, out zipcode);
			if((isNumeric && zip.Length == 5 ) || ((title == "17" || title == "12" || title == "16" ) && zip.Length == 6))
			{
				Response.Write(rb.CatalogRequest(title, firstname, lastname, company, address1, address2, city, state, zip, country, email, emip, phone, notes, optout, keycode, ref errors));
				Helpers.LogCatRequest(title, "catalog", String.Format("{0} [email:{1}] [name:{2}] [keycode:{3}] [redback:{4}]", DateTime.Now.ToString("s"), email, firstname + " " + lastname, keycode, errors));
			}
			else
			{
				Helpers.LogCatRequest(title + ":zip:" + zip, "catalog", String.Format("{0} [email:{1}] [name:{2}] [keycode:{3}] [redback:{4}]", DateTime.Now.ToString("s"), email, firstname + " " + lastname, keycode, errors));
			}
        }
    }
}
