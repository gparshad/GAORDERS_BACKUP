using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AccessCustByZip : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string cn = Request.Form["custemail"] == null ? "" : Request.Form["custemail"].ToString();
        string cz = Request.Form["custzip"] == null ? "" : Request.Form["custzip"].ToString();
	string ct = Request.Form["title"] == null ? "" : Request.Form["title"].ToString();
        RedBackLibrary  rb = new RedBackLibrary();

	if (ct == "")
        {
            Response.Write(rb.FetchCustomerByEmailZip(cn, cz));
        }
        else
        {
	    Response.Write(rb.FetchCustomerByEmailTitle(cn, ct));
            
        }
	

    }
}
