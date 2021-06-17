using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class KeyCodeLogin: System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
	string keycode = Request.Form["keycode"] == null ? "" : Request.Form["keycode"].ToString();
        string title = Request.Form["title"] == null ? "" : Request.Form["title"].ToString();
	string cnum = Request.Form["custnumber"] == null ? "" : Request.Form["custnumber"].ToString();
        string czip = Request.Form["custzip"] == null ? "" : Request.Form["custzip"].ToString();
        RedBackLibrary rb = new RedBackLibrary();
	Response.Write(rb.KeyCodeLogin(keycode ,title ,cnum ,czip));
	
       
    }
}
