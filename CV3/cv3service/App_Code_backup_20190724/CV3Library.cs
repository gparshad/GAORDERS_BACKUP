using System;
using System.Collections.Generic;
using System.Web;
using com.cv3admin.service;
using System.Xml;
using System.Text;
using System.Net;

/// <summary>
/// Summary description for CV3Library
/// </summary>
public class CV3Library
{
    private string CV3USER;
    private string CV3PASS;

    public CV3Library()
    {
        CV3USER = "orderdownload";
        CV3PASS = "garden5";
    }

    public CV3Library(string user, string pass)
    {
        CV3USER = user;
        CV3PASS = pass;
    }

    public List<Order> GetOrders(string serviceID)
    {
        XmlTextReader reader = CV3RetrieveOrders(serviceID);
        List<Order> orders = new List<Order>();
        FillOrdersList(ref orders, reader);
        AdjustForGiftWrap(ref orders);
        return orders;
    }

    public List<Order> GetOrdersRange(string serviceID, int start, int end)
    {
        XmlTextReader reader = CV3RetrieveOrdersRange(serviceID, start, end);
        List<Order> orders = new List<Order>();
        FillOrdersList(ref orders, reader);
        AdjustForGiftWrap(ref orders);

        return orders;
    }

    public string UpdateOrders(ref List<Order> orders, string serviceID)
    {
        string orderConf = "";
        StringBuilder rsp = new StringBuilder();

        foreach (Order o in orders)
            if ((o.OrderNumber != "") || (o.ValidErrCode == "31") || o.OrderErrMsg.Contains("duplicate submission"))
                orderConf += "<orderConf>" + o.OrderID + "</orderConf>";

        if (orderConf != "")
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            CV3Dataxsd cv3 = new CV3Dataxsd();
            string cv3data = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>"
                        + "<CV3Data version=\"2.0\">"
                        + "<request>"
                        + "<authenticate>"
                        + "<user>" + CV3USER + "</user>"
                        + "<pass>" + CV3PASS + "</pass>"
                        + "<serviceID>" + serviceID + "</serviceID>"
                        + "</authenticate>"
                        + "</request>"
                        + "<confirm>"
                        + "<orderConfirm>"
                        + orderConf
                        + "</orderConfirm>"
                        + "</confirm>"
                        + "</CV3Data>";

//                        + "<user>gardensalive</user>"
//                        + "<pass>0rderz</pass>"

            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            byte[] cv3rsp = cv3.CV3Data(Convert.ToBase64String(enc.GetBytes(cv3data)));

            System.IO.MemoryStream xmlOutputAsStream = new System.IO.MemoryStream(cv3rsp);
            System.IO.StreamReader sr = new System.IO.StreamReader(xmlOutputAsStream);
            rsp.Append(sr.ReadToEnd());
        }
        return rsp.ToString();
    }

    private XmlTextReader CV3RetrieveOrders(string serviceID)
    {
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        CV3Dataxsd cv3 = new CV3Dataxsd();
        string cv3data = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>"
                    + "<CV3Data version=\"2.0\">"
                    + "<request>"
                    + "<authenticate>"
                    + "<user>" + CV3USER + "</user>"
                    + "<pass>" + CV3PASS + "</pass>"
                    + "<serviceID>" + serviceID + "</serviceID>"
                    + "</authenticate>"
                    + "<requests>"
                    + "<reqOrders>"
                    + "<reqOrderNew/>"
                    + "</reqOrders>"
                    + "</requests>"
                    + "</request>"
                    + "</CV3Data>";

        cv3.Timeout = 3600000;

        System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
        byte[] cv3rsp = cv3.CV3Data(Convert.ToBase64String(enc.GetBytes(cv3data)));

        System.IO.MemoryStream xmlOutputAsStream = new System.IO.MemoryStream(cv3rsp);
        System.IO.StreamReader sr = new System.IO.StreamReader(xmlOutputAsStream);
        string respText = sr.ReadToEnd();

        //XmlTextReader reader = new XmlTextReader(new System.IO.MemoryStream(cv3rsp));

        
         XmlTextReader reader = new XmlTextReader(new System.IO.StringReader(respText));
        
        reader.WhitespaceHandling = WhitespaceHandling.None;
        return reader;
    }

    private XmlTextReader CV3RetrieveOrdersRange(string serviceID, int start, int end)
    {
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        CV3Dataxsd cv3 = new CV3Dataxsd();
        string cv3data = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>"
                    + "<CV3Data version=\"2.0\">"
                    + "<request>"
                    + "<authenticate>"
                    + "<user>" + CV3USER + "</user>"
                    + "<pass>" + CV3PASS + "</pass>"
                    + "<serviceID>" + serviceID + "</serviceID>"
                    + "</authenticate>"
                    + "<requests>"
                    + "<reqOrders>"
                    + "<reqOrderRange start=\"" + start.ToString() + "\" end=\"" + end.ToString() + "\"/>"
                    + "</reqOrders>"
                    + "</requests>"
                    + "</request>"
                    + "</CV3Data>";

        cv3.Timeout = 3600000;

        System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
        byte[] cv3rsp = cv3.CV3Data(Convert.ToBase64String(enc.GetBytes(cv3data)));

        System.IO.MemoryStream xmlOutputAsStream = new System.IO.MemoryStream(cv3rsp);
        System.IO.StreamReader sr = new System.IO.StreamReader(xmlOutputAsStream);
        string respText = sr.ReadToEnd();


        XmlTextReader reader = new XmlTextReader(new System.IO.StringReader(respText));
        //XmlTextReader reader = new XmlTextReader(new System.IO.MemoryStream(cv3rsp));
        reader.WhitespaceHandling = WhitespaceHandling.None;
        return reader;
    }

    public string CV3RetrieveOrdersRawData(string serviceID)
    {
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        CV3Dataxsd cv3 = new CV3Dataxsd();
        string cv3data = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>"
                    + "<CV3Data version=\"2.0\">"
                    + "<request>"
                    + "<authenticate>"
                    + "<user>" + CV3USER + "</user>"
                    + "<pass>" + CV3PASS + "</pass>"
                    + "<serviceID>" + serviceID + "</serviceID>"
                    + "</authenticate>"
                    + "<requests>"
                    + "<reqOrders>"
                    + "<reqOrderNew/>"
                    + "</reqOrders>"
                    + "</requests>"
                    + "</request>"
                    + "</CV3Data>";

        cv3.Timeout = 300000;

        System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
        byte[] cv3rsp = cv3.CV3Data(Convert.ToBase64String(enc.GetBytes(cv3data)));
        System.IO.MemoryStream xmlOutputAsStream = new System.IO.MemoryStream(cv3rsp);
        System.IO.StreamReader sr = new System.IO.StreamReader(xmlOutputAsStream);
        string respText = sr.ReadToEnd();

        return respText;
    }

    public string CV3RetrieveOrdersRangeRawData(string serviceID, int start, int end)
    {
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        CV3Dataxsd cv3 = new CV3Dataxsd();
        string cv3data = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>"
                    + "<CV3Data version=\"2.0\">"
                    + "<request>"
                    + "<authenticate>"
                    + "<user>" + CV3USER + "</user>"
                    + "<pass>" + CV3PASS + "</pass>"
                    + "<serviceID>" + serviceID + "</serviceID>"
                    + "</authenticate>"
                    + "<requests>"
                    + "<reqOrders>"
                    + "<reqOrderRange start=\"" + start.ToString() + "\" end=\"" + end.ToString() + "\"/>"
                    + "</reqOrders>"
                    + "</requests>"
                    + "</request>"
                    + "</CV3Data>";

        cv3.Timeout = 3600000;

        System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
        byte[] cv3rsp = cv3.CV3Data(Convert.ToBase64String(enc.GetBytes(cv3data)));
        System.IO.MemoryStream xmlOutputAsStream = new System.IO.MemoryStream(cv3rsp);
        System.IO.StreamReader sr = new System.IO.StreamReader(xmlOutputAsStream, Encoding.UTF8);
        string respText = sr.ReadToEnd();

        return respText;
    }

    private void AdjustForGiftWrap(ref List<Order> orders)
    {
        foreach (Order o in orders)
        {
            foreach (Order.ShipTo s in o.ShipTos)
            {
                List<Order.ShipTo.Product> prodList = new List<Order.ShipTo.Product>();
                List<Order.ShipTo.Product> giftWrap = new List<Order.ShipTo.Product>();
                foreach (Order.ShipTo.Product p in s.Products)
                {
                    if (p.Sku.ToLower().EndsWith("g"))
                        giftWrap.Add(p);
                    else
                        prodList.Add(p);
                }
                foreach (Order.ShipTo.Product gw in giftWrap)
                {
                    string sku = gw.Note.Replace("gift wrap item:", "");
                    bool found = false;
                    foreach (Order.ShipTo.Product p in prodList)
                    {
                        if (p.Sku.ToLower() == sku.ToLower() && p.Quantity == gw.Quantity)
                        {
                            p.Sku += "G";
                            p.Price += gw.Price;
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                        prodList.Add(gw);
                }
                s.Products = prodList;
            }
        }
    }

    #region Fill Orders Methods

    private void FillOrdersList(ref List<Order> orders, XmlTextReader reader)
    {
        string curNode = "";
        string curInfo = "";
        Order ord = new Order();
        Order.ShipTo shp = new Order.ShipTo();
        Order.ShipTo.Product prd = new Order.ShipTo.Product();
        Order.ShipTo.Product.CustomField fld = new Order.ShipTo.Product.CustomField();
        Order.BillingInfo bill = new Order.BillingInfo();
        Order.BillingInfo.GifCertificate gc = new Order.BillingInfo.GifCertificate();
        Order.PaypalInfo ppal = new Order.PaypalInfo();
        Order.VisaCheckout vcheckout = new Order.VisaCheckout();
        while (reader.Read())
        {
            reader.MoveToContent();
            if (reader.NodeType == XmlNodeType.Element)
            {
                if (reader.Name == "order")
                {
                    ord = new Order();
                }
                else if (reader.Name == "shipTo")
                {
                    shp = new Order.ShipTo();
                    curInfo = "shipTo";
                }
                else if (reader.Name == "shipToProduct")
                {
                    prd = new Order.ShipTo.Product();
                    curInfo = "shipToProduct";
                }
                else if (reader.Name == "customField")
                {
                    fld = new Order.ShipTo.Product.CustomField();
                    curInfo = "customField";
                }
                else if (reader.Name == "billing")
                {
                    bill = new Order.BillingInfo();
                    curInfo = "billing";
                }
                else if (reader.Name == "giftCertificateUsed")
                {
                    gc = new Order.BillingInfo.GifCertificate();
                    curInfo = "giftCertificateUsed";
                }
                else if (reader.Name == "payPalInfo")
                {
                    ppal = new Order.PaypalInfo();
                    curInfo = "payPalInfo";
                }
                else if (reader.Name == "visaCheckoutInfo")
                {
                    vcheckout = new Order.VisaCheckout();
                    curInfo = "visaCheckoutInfo";
                }
                else if (reader.Name == "totalOrderDiscount")
                {
                    curInfo = "totalOrderDiscount";
                    ord.TotalDiscountType = reader.GetAttribute("type") != null ? reader.GetAttribute("type") : "";
                }
                curNode = reader.Name;
            }
            else if (reader.NodeType == XmlNodeType.EndElement)
            {
                if (reader.Name == "order")
                {
                    orders.Add(ord);
                }
                else if (reader.Name == "shipTo")
                {
                    ord.ShipTos.Add(shp);
                    curInfo = "";
                }
                else if (reader.Name == "customField")
                {
                    prd.CustomForm.Add(fld);
                    curInfo = "customField";
                }
                else if (reader.Name == "customForm")
                {
                    //shp.Products.Add(prd);
                    //curInfo = "shipToProduct";
                }
                else if (reader.Name == "shipToProduct")
                {
                    shp.Products.Add(prd);
                    curInfo = "shipToProduct";
                }
                else if (reader.Name == "billing")
                {
                    ord.Billing = bill;
                    curInfo = "";
                }
                else if (reader.Name == "giftCertificateUsed")
                {
                    bill.GiftCertificates.Add(gc);
                    curInfo = "giftCertificateUsed";
                }
                else if (reader.Name == "payPalInfo")
                {
                    ord.PPal = ppal;
                    curInfo = "";
                }
                else if (reader.Name == "visaCheckoutInfo")
                {
                    ord.VCheckout = vcheckout;
                    curInfo = "";
                }
                else if (reader.Name == "totalOrderDiscount")
                {
                    curInfo = "";
                }
            }
            else if (reader.NodeType == XmlNodeType.Text || reader.NodeType == XmlNodeType.CDATA)
            {
                string rValue = reader.Value;
                if (reader.NodeType == XmlNodeType.CDATA)
                {
                    rValue = HttpUtility.HtmlDecode(rValue);

                }

		rValue=rValue.Replace('\u2013', '-'); // en dash
		rValue=rValue.Replace('\u2014', '-'); // em dash
		rValue=rValue.Replace('\u2015', '-'); // horizontal bar
		rValue=rValue.Replace('\u2017', '_'); // double low line
		rValue=rValue.Replace('\u2018', '\''); // left single quotation mark
		rValue=rValue.Replace('\u2019', '\''); // right single quotation mark
		rValue=rValue.Replace('\u201a', ','); // single low-9 quotation mark
		rValue=rValue.Replace('\u201b', '\''); // single high-reversed-9 quotation mark
		rValue=rValue.Replace('\u201c', '\"'); // left double quotation mark
		rValue=rValue.Replace('\u201d', '\"'); // right double quotation mark
		rValue=rValue.Replace('\u201e', '\"'); // double low-9 quotation mark
		rValue=rValue.Replace("\u2026", "..."); // horizontal ellipsis
		rValue=rValue.Replace('\u2032', '\''); // prime
		rValue=rValue.Replace('\u2033', '\"'); // double prime



                if (curInfo == "shipToProduct")
                    UpdateShipToProduct(ref prd, curNode, rValue);
                else if (curInfo == "customField")
                {
                    if(rValue.Contains("mobilephone"))
                        ord.Mobile = rValue.Split('=')[1];
                    else
                        UpdateCustomField(ref fld, curNode, rValue);
                }
                else if (curInfo == "shipTo")
                    UpdateShipTo(ref shp, curNode, rValue);
                else if (curInfo == "billing")
                    UpdateBilling(ref bill, curNode, rValue);
                else if (curInfo == "giftCertificateUsed")
                    UpdateGiftCertificate(ref gc, curNode, rValue);
                else if (curInfo == "payPalInfo")
                    UpdatePaypalInfo(ref ppal, curNode, rValue);
                else if (curInfo == "visaCheckoutInfo")
                    UpdateVisaCheckout(ref vcheckout, curNode, rValue);
                else if ((curInfo == "totalOrderDiscount") && (curNode == "amount"))
                    ord.TotalDiscount += IsNumeric(rValue) ? Convert.ToDouble(rValue) : 0;
                else if ((curInfo == "totalOrderDiscount") && (curNode == "totalDiscount"))
                    ord.TotalDiscountAmount += IsNumeric(rValue) ? Convert.ToDouble(rValue) : 0;
                else
                    UpdateItem(ref ord, curNode, rValue);
            }
        }
    }

    private void UpdateItem(ref Order obj, string node, string value)
    {
        switch (node.ToLower())
        {
            case "orderid":
                obj.OrderID = value;
                break;
            case "totalprice":
                obj.TotalPrice = IsNumeric(value) ? Convert.ToDouble(value) : 0;
                break;
            case "totalshipping":
                obj.TotalShipping = IsNumeric(value) ? Convert.ToDouble(value) : 0;
                break;
            case "totaltax":
                obj.TotalTax = IsNumeric(value) ? Convert.ToDouble(value) : 0;
                break;
            case "dateordered":
                obj.DateOrdered = IsDateTime(value) ? Convert.ToDateTime(value) : DateTime.Now;
                break;
            case "promocode":
                obj.PromoCode = value;
                break;
            case "paymethod":
                if (value != "purchaseorder")
                {
                    obj.PayMethod = value;
                }
                break;
            case "sourcecode":
                obj.SourceCode = value;
                break;
            case "timeordered":
                obj.TimeOrdered = value;
                break;
            case "comments":
                obj.Comments = value;
                break;
  	        case "purchaseorder":
                if (value.IndexOf("-") > 0)
                {
                    string[] words = value.Split('-');
                    obj.PayMethod = words[0];
                    obj.CustNumber = words[1];
                    obj.IMS_AskIncome = words[2];
                }
                break;
            case "referrer":
                obj.Referrer = value;
                break;
        }
        return;
    }

    private void UpdateBilling(ref Order.BillingInfo obj, string node, string value)
    {

        switch (node.ToLower())
        {
            case "title":
                obj.Title = value;
                break;
            case "firstname":
                obj.FirstName = value;
                break;
            case "lastname":
                obj.LastName = value;
                break;
            case "address1":
                obj.Address1 = value;
                break;
            case "company":
                obj.Company = value;
                break;
            case "address2":
                obj.Address2 = value;
                break;
            case "city":
                obj.City = value;
                break;
            case "state":
                obj.State = value;
                break;
            case "zip":
                obj.Zip = value;
                break;
            case "country":
                obj.Country = value;
                break;
            case "email":
                obj.Email = value;
                break;
            case "phone":
                obj.Phone = value;
                break;
            case "optout":
                obj.OptOut = value;
                break;
            case "cctype":
                obj.CCType = value;
                break;
            case "ccname":
                obj.CCName = value;
                break;
            case "ccnum":
                obj.CCNum = value;
                break;
            case "ccexpm":
                obj.CCExpM = value;
                break;
            case "ccexpy":
                obj.CCExpY = value;
                break;
            case "gatewayorderid":
                obj.GatewayOrderID = value;
                break;
            case "authcode":
                obj.AuthCode = value;
                break;
            case "referencenum":
                obj.ReferenceNum = value;
                break;
            case "transactionid":
                obj.TransactionID = value;
                break;
            case "avsapproval":
                obj.AVSApproval = value;
                break;
            case "cvv2response":
                obj.CVV2Response = value;
                break;
            case "authamount":
                obj.AuthAmount = value;
                break;
        }
        return;
    }
    private void UpdateGiftCertificate(ref Order.BillingInfo.GifCertificate obj, string node, string value)
    {
        switch (node.ToLower())
        {
            case "giftcertificate":
                obj.GCCode = value;
                break;
            case "amountused":
                obj.Amount = IsNumeric(value) ? Convert.ToDouble(value) : 0;
                break;
        }
        return;
    }
    private void UpdatePaypalInfo(ref Order.PaypalInfo obj, string node, string value)
    {
        switch (node.ToLower())
        {
            case "buyer":
                obj.Buyer = value;
                break;
            case "transactionid":
                obj.TransactionId = value;
                break;
            case "amount":
                obj.Amount = IsNumeric(value) ? Convert.ToDouble(value) : 0;
                break;
        }
        return;
    }
    private void UpdateVisaCheckout(ref Order.VisaCheckout obj, string node, string value)
    {
        switch (node.ToLower())
        {
            
            case "transactionid":
                obj.TransactionId = value;
                break;
            case "amount":
                obj.Amount = IsNumeric(value) ? Convert.ToDouble(value) : 0;
                break;
        }
        return;
    }
    private void UpdateShipTo(ref Order.ShipTo obj, string node, string value)
    {
      
        switch (node.ToLower())
        {
            case "name":
                obj.Name = value;
                break;
            case "firstname":
                obj.FirstName = value;
                break;
            case "lastname":
                obj.LastName = value;
                break;
            case "company":
                obj.Company = value;
                break;
            case "title":
                obj.Title = value;
                break;
            case "address1":
                obj.Address1 = value;
                break;
            case "address2":
                obj.Address2 = value;
                break;
            case "city":
                obj.City = value;
                break;
            case "state":
                obj.State = value;
                break;
            case "zip":
                obj.Zip = value;
                break;
            case "country":
                obj.Country = value;
                break;
            case "phone":
                obj.Phone = value;
                break;
            case "tax":
                obj.Tax = IsNumeric(value) ? Convert.ToDouble(value) : 0;
                break;
            case "shipping":
                obj.Shipping = IsNumeric(value) ? Convert.ToDouble(value) : 0;
                break;
            case "shipon":
                obj.ShipOn = IsDateTime(value) ? Convert.ToDateTime(value) : DateTime.Now;
                break;
            case "message":
                obj.Message = value;
                break;
            case "shipmethod":
                obj.ShipMethod = value;
                break;
            case "shipmethodcode":
                obj.ShipMethodCode = value;
                break;
            case "giftwrap":
                obj.GiftWrap = value;
                break;
        }
        return;
    }

    private void UpdateCustomField(ref Order.ShipTo.Product.CustomField obj, string node, string value)
    {
        switch (node.ToLower())
        {
            case "label":
                obj.Key = value;
                break;
            case "value":
                obj.Value = value;
                break;
        }
        return;
    }

    private void UpdateShipToProduct(ref Order.ShipTo.Product obj, string node, string value)
    {
        switch (node.ToLower())
        {
            case "sku":
                obj.Sku = value;
                break;
            case "quantity":
                obj.Quantity = IsNumeric(value) ? Convert.ToInt32(value) : 0;
                break;
            case "price":
                obj.Price = IsNumeric(value) ? Convert.ToDouble(value) : 0;
                break;
            case "note":
                obj.Note = value;
                break;
        }
        return;
    }
    #endregion

    #region Helper Methods

    internal static bool IsDateTime(object ObjectToTest)
    {
        if (ObjectToTest == null)
        {
            return false;
        }
        else
        {
            DateTime OutValue;
            return DateTime.TryParse(ObjectToTest.ToString().Trim(), out OutValue);
        }
    }

    internal static bool IsNumeric(object ObjectToTest)
    {
        if (ObjectToTest == null)
        {
            return false;
        }
        else
        {
            double OutValue;
            return double.TryParse(ObjectToTest.ToString().Trim(),
                System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.CurrentCulture,
                out OutValue);
        }
    }

    #endregion

    #region Order Class

    public class Order
    {
        public string OrderID;
        public double TotalPrice;
        public double TotalShipping;
        public double TotalTax;
        public double TotalDiscount;
        public double TotalDiscountAmount;
        public string TotalDiscountType;
        public DateTime DateOrdered;
        public string TimeOrdered;
        public string PayMethod;
        public string SourceCode;
        public string PromoCode;
        public string Comments;
        public string Referrer;
        public string CustNumber;
        public string IMS_AskIncome;
        public string OrderNumber;
        public string ValidErrCode;
        public string ValidErrMsg;
        public string OrderErrCode;
        public string OrderErrMsg;
        public List<ShipTo> ShipTos;
        public BillingInfo Billing;
        public PaypalInfo PPal;
        public VisaCheckout VCheckout;
        public string Mobile;

        public Order()
        {
            OrderID = "";
            TotalPrice = 0.0;
            TotalShipping = 0.0;
            TotalTax = 0.0;
            TotalDiscount = 0.0;
            TotalDiscountAmount = 0.0;
            TotalDiscountType = "";
            DateOrdered = DateTime.Now;
            TimeOrdered = "";
            PayMethod = "";
            SourceCode = "";
            PromoCode = "";
            Comments = "";
            Referrer = "";
            CustNumber = "";
            Mobile = "";
            IMS_AskIncome = "";
            OrderNumber = "";
            OrderErrCode = "";
            OrderErrMsg = "";
            ValidErrCode = "";
            ValidErrMsg = "";
            ShipTos = new List<ShipTo>();
            Billing = new BillingInfo();
            PPal = new PaypalInfo();
            VCheckout = new VisaCheckout();

        }

        public class ShipTo
        {
            public string Name;
            public string FirstName;
            public string LastName;
            public string Company;
            public string Title;
            public string Address1;
            public string Address2;
            public string City;
            public string State;
            public string Zip;
            public string Country;
            public string Phone;
            public double Tax;
            public double Shipping;
            public DateTime ShipOn;
            public string Message;
            public string ShipMethod;
            public string ShipMethodCode;
            public string GiftWrap;
            public List<Product> Products;

            public ShipTo()
            {
                Name = "";
                FirstName = "";
                LastName = "";
                Company = "";
                Title = "";
                Address1 = "";
                Address2 = "";
                City = "";
                State = "";
                Zip = "";
                Country = "";
                Phone = "";
                Tax = 0.0;
                Shipping = 0.0;
                ShipOn = DateTime.Now;
                Message = "";
                ShipMethod = "";
                ShipMethodCode = "";
                GiftWrap = "";
                Products = new List<Product>();
            }
            public class Product
            {
                public string Sku;
                public int Quantity;
                public double Price;
                public string Note;
                public List<CustomField> CustomForm;

                public Product()
                {
                    Sku = "";
                    Quantity = 0;
                    Price = 0.0;
                    Note = "";
                    CustomForm = new List<CustomField>();
                }
                public Product(string sku, int qty, double price)
                {
                    Sku = sku;
                    Quantity = qty;
                    Price = price;
                    Note = "";
                    CustomForm = new List<CustomField>();
                }
                public Product(string sku, int qty, double price, string note)
                {
                    Sku = sku;
                    Quantity = qty;
                    Price = price;
                    Note = note;
                    CustomForm = new List<CustomField>();
                }
                public class CustomField
                {
                    public string Key;
                    public string Value;
                    public CustomField()
                    {
                        Key = "";
                        Value = "";
                    }
                    public CustomField(string key, string value)
                    {
                        Key = key;
                        Value = value;
                    }
                }
            }
        }
        public class PaypalInfo
        {
            public string Buyer;
            public string TransactionId;
            public double Amount;
            public PaypalInfo()
            {
                Buyer = "";
                TransactionId = "";
                Amount = 0.0;

            }
            public PaypalInfo(string buyer, string transactionId, double amount)
            {
                Buyer = buyer;
                TransactionId = transactionId;
                Amount = amount;
            }
        }
        public class VisaCheckout
        {
           
            public string TransactionId;
            public double Amount;
            public VisaCheckout()
            {
               
                TransactionId = "";
                Amount = 0.0;

            }
            public VisaCheckout(string transactionId, double amount)
            {
               
                TransactionId = transactionId;
                Amount = amount;
            }
        }
        public class BillingInfo
        {
            public class GifCertificate
            {
                public string GCCode;
                public double Amount;
                public GifCertificate()
                {
                    GCCode = "";
                    Amount = 0.0;
                }
                public GifCertificate(string gcCode, double amount)
                {
                    GCCode = gcCode;
                    Amount = amount;
                }
            }
            public string Title;
            public string FirstName;
            public string LastName;
            public string Company;
            public string Address1;
            public string Address2;
            public string City;
            public string State;
            public string Zip;
            public string Country;
            public string Email;
            public string Phone;
            public string OptOut;
            public string CCType;
            public string CCName;
            public string CCNum;
            public string CCExpM;
            public string CCExpY;
            public string GatewayOrderID;
            public string AuthCode;
            public string ReferenceNum;
            public string TransactionID;
            public string AVSApproval;
            public string CVV2Response;
            public string AuthAmount;
            public List<GifCertificate> GiftCertificates;
            public BillingInfo()
            {
                Title = "";
                FirstName = "";
                LastName = "";
                Company = "";
                Address1 = "";
                Address2 = "";
                City = "";
                State = "";
                Zip = "";
                Country = "";
                Email = "";
                Phone = "";
                OptOut = "false";
                CCType = "";
                CCName = "";
                CCNum = "";
                CCExpM = "";
                CCExpY = "";
                GatewayOrderID = "";
                AuthCode = "";
                ReferenceNum = "";
                TransactionID = "";
                AVSApproval = "";
                CVV2Response = "";
                AuthAmount = "";
                GiftCertificates = new List<GifCertificate>();
            }
        }
    }

    #endregion
}
