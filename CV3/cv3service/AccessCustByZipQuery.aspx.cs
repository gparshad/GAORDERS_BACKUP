using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AccessCustByZipQuery : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string cn = "ruth@officeoutfittersinc.com";
        string cz = "E5N6K9";
	string ct = "";
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
