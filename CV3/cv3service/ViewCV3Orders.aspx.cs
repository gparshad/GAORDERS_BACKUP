using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ViewCV3Orders : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //78cdd310d3 Bits AU
        //if(uxStoreCode.Text == "")
        //    uxStoreCode.Text = "14aa6f11d4";
    }
    protected void uxGo_Click2(object sender, EventArgs e)
    {
        uxResponse.Text = "";
    }
    protected void uxGo_Click(object sender, EventArgs e)
    {
        string CV3USER = "orderdownload";
        string CV3PASS = "garden10";
        //1122

        CV3Library cv3 = new CV3Library(CV3USER, CV3PASS);
        if (uxStart.Text.Length > 0 && uxEnd.Text.Length > 0)
        {
            uxResponse.Text = cv3.CV3RetrieveOrdersRangeRawData(uxStoreCode.Text, int.Parse(uxStart.Text), int.Parse(uxEnd.Text));
        }
        else
        {
            uxResponse.Text = cv3.CV3RetrieveOrdersRawData(uxStoreCode.Text);
        }
    }
}
