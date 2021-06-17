using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class OrderStatus : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string cn = Request.Form["custnumber"] == null ? "" : Request.Form["custnumber"].ToString();
        string cz = Request.Form["custzip"] == null ? "" : Request.Form["custzip"].ToString();
        string t = Request.Form["title"] == null ? "" : Request.Form["title"].ToString();
        string n = Request.Form["ordernumber"] == null ? "" : Request.Form["ordernumber"].ToString();
        string em = Request.Form["custemail"] == null ? "" : Request.Form["custemail"].ToString();
        RedBackLibrary rb = new RedBackLibrary();
        if (n != "")
        {
            if(em != "")
                Response.Write(rb.FetchOrder(em, t, n));
            else
                Response.Write(rb.FetchOrder(cn, cz, t, n));

        }
        else
        {
            if (em != "")
                Response.Write(rb.FetchOrders(em, t));
            else
                Response.Write(rb.FetchOrders(cn, cz, t));
            
        }
    }
}
