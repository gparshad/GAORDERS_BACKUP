using System;
using System.Collections.Generic;
using System.Web;
using System.Text;
using System.Linq;

/// <summary>
/// Summary description for Helpers
/// </summary>
public static class Helpers
{
    public static void LogOrders(List<CV3Library.Order> orders, string serviceID, string brandCode)
    {
        string logFile;
        if (brandCode.Length == 1)
            logFile = "C:\\GAORDERS\\Logs\\title0" + brandCode + "\\" + DateTime.Now.ToString("yyyyMMdd") + "-0" + brandCode + "-orders.txt";
        else
            logFile = "C:\\GAORDERS\\Logs\\title" + brandCode + "\\" + DateTime.Now.ToString("yyyyMMdd") + "-" + brandCode + "-orders.txt";

        using (System.IO.StreamWriter w = System.IO.File.AppendText(logFile))
        {
            int orderCount = 0;
            foreach (CV3Library.Order o in orders)
            {
                if (o.OrderNumber != "")
                    w.WriteLine("CV3:" + serviceID + ", " + o.OrderID + "; RB:" + o.OrderNumber + ", CUST:" + o.CustNumber);
                else
                    w.WriteLine("CV3:" + serviceID + ", " + o.OrderID + "; ERR:" + o.OrderErrCode + ", MSG:" + o.OrderErrMsg + ", VALERR:" + o.ValidErrCode + ", VALMSG:" + o.ValidErrMsg);
                orderCount++;
            }
            w.WriteLine(DateTime.Now.ToString() + ", COUNT:" + orderCount.ToString());
            w.WriteLine("==================================================");
            w.Close();
        }
    }

    public static void LogRequest(string brandCode, string requestType, string logText)
    {
        string logFile;

        if (requestType.Length <= 0)
            requestType = "misc";

        if (logText.Length <= 0)
            logText = "::" + DateTime.Now.ToString("f");

        if (brandCode.Length > 0)
        {
            if (brandCode.Length == 1)
                logFile = "C:\\GAORDERS\\Logs\\title0" + brandCode + "\\" + DateTime.Now.ToString("yyyyMMdd") + "-0" + brandCode + "-" + requestType + ".txt";
            else
                logFile = "C:\\GAORDERS\\Logs\\title" + brandCode + "\\" + DateTime.Now.ToString("yyyyMMdd") + "-" + brandCode + "-" + requestType + ".txt";
        }
        else
        {
            logFile = "C:\\GAORDERS\\Logs\\misc\\" + DateTime.Now.ToString("yyyyMMdd") + "-" + requestType + ".txt";
        }

        using (System.IO.StreamWriter w = System.IO.File.AppendText(logFile))
        {
            w.WriteLine(logText);
            w.Close();
        }
    }
	
	public static void LogCatRequest(string brandCode, string requestType, string logText)
    {
        string logFile;
		int brand;
		bool isNumeric = int.TryParse(brandCode, out brand);

		int[] brands = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 16, 17, 18, 19, 22, 35, 36, 49, 52, 66 }; 
		
        if (requestType.Length <= 0)
            requestType = "misc";

        if (logText.Length <= 0)
            logText = "::" + DateTime.Now.ToString("f");

        if (brandCode.Length > 0 && isNumeric && brands.Contains(brand))
        {
            if (brandCode.Length == 1)
                logFile = "C:\\GAORDERS\\Logs\\title0" + brandCode + "\\" + DateTime.Now.ToString("yyyyMMdd") + "-0" + brandCode + "-" + requestType + ".txt";
            else
                logFile = "C:\\GAORDERS\\Logs\\title" + brandCode + "\\" + DateTime.Now.ToString("yyyyMMdd") + "-" + brandCode + "-" + requestType + ".txt";
        }
        else
        {
			logText = "brandCode::" + brandCode + "::" + logText;
            logFile = "C:\\GAORDERS\\Logs\\misc\\" + DateTime.Now.ToString("yyyyMMdd") + "-" + requestType + ".txt";
        }

        using (System.IO.StreamWriter w = System.IO.File.AppendText(logFile))
        {
            w.WriteLine(logText);
            w.Close();
        }
    }

    public static string SanitizeXml(string source)
    {
        if (string.IsNullOrEmpty(source))
        {
            return source;
        }
        if (source.IndexOf('&') < 0)
        {
            return source;
        }
        StringBuilder result = new StringBuilder(source);
        result = result.Replace("&lt;", "<>lt;")
                        .Replace("&gt;", "<>gt;")
                        .Replace("&amp;", "<>amp;")
                        .Replace("&apos;", "<>apos;")
                        .Replace("&quot;", "<>quot;");
        result = result.Replace("&", "&amp;");
        result = result.Replace("<>lt;", "&lt;")
                        .Replace("<>gt;", "&gt;")
                        .Replace("<>amp;", "&amp;")
                        .Replace("<>apos;", "&apos;")
                        .Replace("<>quot;", "&quot;");

        return result.ToString();
    }

}
