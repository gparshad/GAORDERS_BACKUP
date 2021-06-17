﻿using System;
using System.Collections.Generic;
using System.Web;
using REDPAGESLib;
using System.Text;

/// <summary>
/// Summary description for RedBackLibrary
/// </summary>
public class RedBackLibraryTester
{
    private const string RedBackAccount = "172.16.15.15:8402";
    //private const string RedBackAccount = "208.7.138.199:8402";

    public RedBackLibraryTester()
    {
    }

    #region OrderProcess
    public string SendToRedback(ref List<CV3Library.Order> orders, string brandCode, string ordPrefix)
    {
        StringBuilder rsp = new StringBuilder();
        RedObject rb;
        bool isRedBackAvailable = true;

        rsp.Append("<RedBackResponse>\n");
        foreach (CV3Library.Order o in orders)
        {
            foreach (CV3Library.Order.ShipTo s in o.ShipTos)
            {
                if (isRedBackAvailable == true)
                {
                    try
                    {
                        rb = new RedObject();
                        rb.Open3(RedBackAccount, "OPM:Order_New");
                        ((RedProperty)rb.Property("CustFirstName")).Value = o.Billing.FirstName;
                        ((RedProperty)rb.Property("CustLastName")).Value = o.Billing.LastName;
                        ((RedProperty)rb.Property("CustAdd1")).Value = o.Billing.Address1;
                        ((RedProperty)rb.Property("CustAdd2")).Value = o.Billing.Address2;
                        ((RedProperty)rb.Property("CustCity")).Value = o.Billing.City;
                        ((RedProperty)rb.Property("CustState")).Value = o.Billing.State;
                        ((RedProperty)rb.Property("CustZip")).Value = o.Billing.Zip;
                        ((RedProperty)rb.Property("CustCountry")).Value = o.Billing.Country;
                        ((RedProperty)rb.Property("CustPhoneDay")).Value = o.Billing.Phone;
                        ((RedProperty)rb.Property("CustEmail1")).Value = o.Billing.Email;
                        ((RedProperty)rb.Property("ShipFirstName")).Value = s.FirstName;
                        ((RedProperty)rb.Property("ShipLastName")).Value = s.LastName;
                        ((RedProperty)rb.Property("ShipAdd1")).Value = s.Address1;
                        ((RedProperty)rb.Property("ShipAdd2")).Value = s.Address2;
                        ((RedProperty)rb.Property("ShipCity")).Value = s.City;
                        ((RedProperty)rb.Property("ShipState")).Value = s.State;
                        ((RedProperty)rb.Property("ShipZip")).Value = s.Zip;
                        ((RedProperty)rb.Property("ShipCountry")).Value = s.Country;
                        ((RedProperty)rb.Property("ShipPhone")).Value = s.Phone;
                        ((RedProperty)rb.Property("CustNumber")).Value = "";
                        ((RedProperty)rb.Property("WebReference")).Value = brandCode + "CV" + ordPrefix + o.OrderID;
                        ((RedProperty)rb.Property("PromoCode")).Value = o.PromoCode;
                        ((RedProperty)rb.Property("Title")).Value = brandCode;
                        ((RedProperty)rb.Property("ShipMethod")).Value = s.ShipMethodCode;
                        ((RedProperty)rb.Property("PaymentMethod")).Value = o.PayMethod;
                        ((RedProperty)rb.Property("CreditCardNumber")).Value = o.Billing.CCNum;
                        ((RedProperty)rb.Property("CardExpDate")).Value = o.Billing.CCExpM + "/" + o.Billing.CCExpY;
                        //((RedProperty)rb.Property("CardCVV")).Value = o.Billing.CCCVV;
                        ((RedProperty)rb.Property("CardAddress")).Value = o.Billing.Address1;
                        ((RedProperty)rb.Property("CardZip")).Value = o.Billing.Zip;

                        string d = "", i = "", q = "", u = "";
                        string[] pz = { "", "", "", "", "", "" };
                        double ma = 0;
                        foreach (CV3Library.Order.ShipTo.Product p in s.Products)
                        {
                            i += d + p.Sku;
                            q += d + p.Quantity.ToString();
                            u += d + p.Price.ToString("F");
                            ma += p.Quantity * p.Price;

                            int a = 0;
                            string[] cf = { "", "", "", "", "" };
                            foreach (CV3Library.Order.ShipTo.Product.CustomField f in p.CustomForm)
                            {
                                if (a < 5)
                                    cf[a] = f.Value;
                                a++;
                            }
                            pz[0] += d;
                            for (a = 0; a < 5; a++)
                                pz[a + 1] += d + cf[a];

                            d = "ý";
                        }
                        ((RedProperty)rb.Property("Items")).Value = i;
                        ((RedProperty)rb.Property("QtyOrdered")).Value = q;
                        ((RedProperty)rb.Property("UnitPrice")).Value = u;
                        for (int a = 1; a < 6; a++)
                            if (pz[a] != pz[0])
                                ((RedProperty)rb.Property("PersonalizationDetail" + a.ToString())).Value = pz[a];

                        ((RedProperty)rb.Property("MerchAmount")).Value = ma.ToString("F");

                        // for "percent" DiscAmount = ItemTotal * TotalDiscount * -0.01
                        if (o.TotalDiscountType.ToLower() == "percent")
                        {
                            //((RedProperty)rb.Property("DiscAmount")).Value = ((double)(ma * o.TotalDiscount * -0.01)).ToString("F");
                            ((RedProperty)rb.Property("CouponAmount")).Value = ((double)(ma * o.TotalDiscount * 0.01)).ToString("F");
                            //discAmt.Text = ((double)(ma * o.TotalDiscount * 0.01)).ToString("F");
                        }
                        // for all others CouponAmount = TotalDiscount
                        else
                        {
                            ((RedProperty)rb.Property("CouponAmount")).Value = o.TotalDiscount.ToString("F");
                            //discAmt.Text = o.TotalDiscount.ToString("F");
                        }
                        int gCount = 1;
                        int rCount = 1;
                        foreach (CV3Library.Order.BillingInfo.GifCertificate gcert in o.Billing.GiftCertificates)
                        {
                            if (gcert.GCCode.EndsWith("CM"))
                            {
                                ((RedProperty)rb.Property("Repc" + rCount + "_Number")).Value = gcert.GCCode;
                                ((RedProperty)rb.Property("Repc" + rCount + "_Amount")).Value = gcert.Amount.ToString("F");
                                rCount++;
                            }
                            else
                            {

                                ((RedProperty)rb.Property("Cert" + gCount + "_Number")).Value = gcert.GCCode;
                                ((RedProperty)rb.Property("Cert" + gCount + "_Amount")).Value = gcert.Amount.ToString("F");
                                gCount++;
                            }


                        }
                        ((RedProperty)rb.Property("ShipAmount")).Value = o.TotalShipping.ToString("F");
                        //((RedProperty)rb.Property("PremShipAmount")).Value = custNumber;
                        //((RedProperty)rb.Property("OverweightAmount")).Value = custNumber;
                        ((RedProperty)rb.Property("TaxAmount")).Value = o.TotalTax.ToString("F");
                        ((RedProperty)rb.Property("TotalAmount")).Value = o.TotalPrice.ToString("F");
                        ((RedProperty)rb.Property("Comments")).Value = o.Comments;

                        //((RedProperty)rb.Property("ReleaseDate")).Value = custNumber;
                        //((RedProperty)rb.Property("FreeFlag")).Value = custNumber;
                        //((RedProperty)rb.Property("PIN")).Value = custNumber;
                        //((RedProperty)rb.Property("PINHint")).Value = custNumber;
                        if (o.Billing.OptOut == "true")
                            ((RedProperty)rb.Property("OptOutDate")).Value = DateTime.Now.ToString("d");
                        else
                            ((RedProperty)rb.Property("OptInFlag")).Value = "W";
                        ((RedProperty)rb.Property("OptInDate")).Value = DateTime.Now.ToString("d");
                        //((RedProperty)rb.Property("Etrack")).Value = custNumber;
                        //((RedProperty)rb.Property("IPSource")).Value = o.;
                        rb.CallMethod("AddOrder");

                        o.CustNumber = ((RedProperty)rb.Property("CustNumber")).Value;
                        o.OrderNumber = ((RedProperty)rb.Property("OrderNumber")).Value;
                        o.OrderErrCode = ((RedProperty)rb.Property("OrderErrCode")).Value;
                        o.OrderErrMsg = ((RedProperty)rb.Property("OrderErrMsg")).Value;
                        o.ValidErrCode = ((RedProperty)rb.Property("ValidErrCode")).Value;
                        o.ValidErrMsg = ((RedProperty)rb.Property("ValidErrMsg")).Value;

                        rsp.Append("\t<Order>\n\t\t<OrderNumber>" + ((RedProperty)rb.Property("OrderNumber")).Value + "</OrderNumber>\n"
                            + "\t\t<CustNumber>" + ((RedProperty)rb.Property("CustNumber")).Value + "</CustNumber>\n"
                            + "\t\t<GVCFlag>" + ((RedProperty)rb.Property("GVCFlag")).Value + "</GVCFlag>\n"
                            + "\t\t<GVCDate>" + ((RedProperty)rb.Property("GVCDate")).Value + "</GVCDate>\n"
                            + "\t\t<ValidErrCode>" + ((RedProperty)rb.Property("ValidErrCode")).Value + "</ValidErrCode>\n"
                            + "\t\t<ValidErrMsg>" + ((RedProperty)rb.Property("ValidErrMsg")).Value + "</ValidErrMsg>\n"
                            + "\t\t<OrderErrCode>" + ((RedProperty)rb.Property("OrderErrCode")).Value + "</OrderErrCode>\n"
                            + "\t\t<OrderErrMsg>" + ((RedProperty)rb.Property("OrderErrMsg")).Value + "</OrderErrMsg>\n\t</Order>\n");

                        rb.Close();
                    }
                    catch (Exception ex)
                    {
                        isRedBackAvailable = false;
                        rsp.Append("\t<Error>\n\t\t<Message>" + ex.Message + "</Message>\n\t\t<AdditionalInfo>" + ex.StackTrace + "</AdditionalInfo>\n");
                    }
                }
            }
        }
        rsp.Append("</RedBackResponse>\n");

        return rsp.ToString();
    }
    public string SendToRedbackWithDefaultKeycode(ref List<CV3Library.Order> orders, string brandCode, string ordPrefix, string keycode)
    {
        StringBuilder rsp = new StringBuilder();
        RedObject rb;
        bool isRedBackAvailable = true;

        rsp.Append("<RedBackResponse>\n");
        foreach (CV3Library.Order o in orders)
        {
            int shipToCount = 0;
            foreach (CV3Library.Order.ShipTo s in o.ShipTos)
            {
                if (isRedBackAvailable == true)
                {
                    shipToCount++;
                    string shipToPrefix = shipToCount > 1 ? "-" + shipToCount.ToString() : "";
                    try
                    {
                        rb = new RedObject();
                        rb.Open3(RedBackAccount, "OPM:Order_New");
                        ((RedProperty)rb.Property("CustFirstName")).Value = o.Billing.FirstName;
                        ((RedProperty)rb.Property("CustLastName")).Value = o.Billing.LastName;
                        ((RedProperty)rb.Property("CustAdd1")).Value = o.Billing.Address1;
                        ((RedProperty)rb.Property("CustAdd2")).Value = o.Billing.Address2;
                        ((RedProperty)rb.Property("CustCity")).Value = o.Billing.City;
                        ((RedProperty)rb.Property("CustState")).Value = o.Billing.State;
                        ((RedProperty)rb.Property("CustZip")).Value = o.Billing.Zip;
                        ((RedProperty)rb.Property("CustCountry")).Value = o.Billing.Country;
                        ((RedProperty)rb.Property("CustPhoneDay")).Value = o.Billing.Phone;
                        ((RedProperty)rb.Property("CustEmail1")).Value = o.Billing.Email;
                        ((RedProperty)rb.Property("ShipFirstName")).Value = s.FirstName;
                        ((RedProperty)rb.Property("ShipLastName")).Value = s.LastName;
                        ((RedProperty)rb.Property("ShipAdd1")).Value = s.Address1;
                        ((RedProperty)rb.Property("ShipAdd2")).Value = s.Address2;
                        ((RedProperty)rb.Property("ShipCity")).Value = s.City;
                        ((RedProperty)rb.Property("ShipState")).Value = s.State;
                        ((RedProperty)rb.Property("ShipZip")).Value = s.Zip;
                        ((RedProperty)rb.Property("ShipCountry")).Value = s.Country;
                        ((RedProperty)rb.Property("ShipPhone")).Value = s.Phone;
                        ((RedProperty)rb.Property("CustNumber")).Value = o.CustNumber;
                        ((RedProperty)rb.Property("WebReference")).Value = brandCode + "CV" + ordPrefix + o.OrderID + shipToPrefix;
                        if(o.PromoCode == "")
                            ((RedProperty)rb.Property("PromoCode")).Value = keycode;
                        else
                            ((RedProperty)rb.Property("PromoCode")).Value = o.PromoCode;
                        ((RedProperty)rb.Property("Title")).Value = brandCode;
                        ((RedProperty)rb.Property("ShipMethod")).Value = s.ShipMethodCode;
                        ((RedProperty)rb.Property("PaymentMethod")).Value = o.PayMethod;
                        ((RedProperty)rb.Property("CreditCardNumber")).Value = o.Billing.CCNum;
                        ((RedProperty)rb.Property("CardExpDate")).Value = o.Billing.CCExpM + "/" + o.Billing.CCExpY;
                        //((RedProperty)rb.Property("CardCVV")).Value = o.Billing.CCCVV;
                        ((RedProperty)rb.Property("CardAddress")).Value = o.Billing.Address1;
                        ((RedProperty)rb.Property("CardZip")).Value = o.Billing.Zip;

                        string d = "", i = "", q = "", u = "";
                        string[] pz = { "", "", "", "", "", "" };
                        double ma = 0;
                        foreach (CV3Library.Order.ShipTo.Product p in s.Products)
                        {
                            i += d + p.Sku;
                            q += d + p.Quantity.ToString();
                            u += d + p.Price.ToString("F");
                            ma += p.Quantity * p.Price;

                            int a = 0;
                            string[] cf = { "", "", "", "", "" };
                            foreach (CV3Library.Order.ShipTo.Product.CustomField f in p.CustomForm)
                            {
                                if (a < 5)
                                    cf[a] = f.Value;
                                a++;
                            }
                            pz[0] += d;
                            for (a = 0; a < 5; a++)
                                pz[a + 1] += d + cf[a];

                            d = "ý";
                        }
                        ((RedProperty)rb.Property("Items")).Value = i;
                        ((RedProperty)rb.Property("QtyOrdered")).Value = q;
                        ((RedProperty)rb.Property("UnitPrice")).Value = u;
                        for (int a = 1; a < 6; a++)
                            if (pz[a] != pz[0])
                                ((RedProperty)rb.Property("PersonalizationDetail" + a.ToString())).Value = pz[a];

                        ((RedProperty)rb.Property("MerchAmount")).Value = ma.ToString("F");

                        // for "percent" DiscAmount = ItemTotal * TotalDiscount * -0.01
                        if (o.TotalDiscountType.ToLower() == "percent")
                        {
                            //((RedProperty)rb.Property("DiscAmount")).Value = ((double)(ma * o.TotalDiscount * -0.01)).ToString("F");
                            ((RedProperty)rb.Property("CouponAmount")).Value = ((double)(ma * o.TotalDiscount * 0.01)).ToString("F");
                            //discAmt.Text = ((double)(ma * o.TotalDiscount * 0.01)).ToString("F");
                        }
                        // for all others CouponAmount = TotalDiscount
                        else
                        {
                            ((RedProperty)rb.Property("CouponAmount")).Value = o.TotalDiscount.ToString("F");
                            //discAmt.Text = o.TotalDiscount.ToString("F");
                        }
                        int gCount = 1;
                        int rCount = 1;
                        foreach(CV3Library.Order.BillingInfo.GifCertificate gcert in o.Billing.GiftCertificates)
                        {
                            if (gcert.GCCode.EndsWith("CM"))
                            {
                                ((RedProperty)rb.Property("Repc" + rCount + "_Number")).Value = gcert.GCCode;
                                ((RedProperty)rb.Property("Repc" + rCount + "_Amount")).Value = gcert.Amount.ToString("F");
                                rCount++;
                            }
                            else
                            {
                            
                                ((RedProperty)rb.Property("Cert" + gCount + "_Number")).Value = gcert.GCCode;
                                ((RedProperty)rb.Property("Cert" + gCount + "_Amount")).Value = gcert.Amount.ToString("F");
                                gCount++;
                            }


                        }
                        ((RedProperty)rb.Property("ShipAmount")).Value = o.TotalShipping.ToString("F");
                        //((RedProperty)rb.Property("PremShipAmount")).Value = custNumber;
                        //((RedProperty)rb.Property("OverweightAmount")).Value = custNumber;
                        ((RedProperty)rb.Property("TaxAmount")).Value = o.TotalTax.ToString("F");
                        ((RedProperty)rb.Property("TotalAmount")).Value = o.TotalPrice.ToString("F");
                        ((RedProperty)rb.Property("Comments")).Value = o.Comments;

                        //((RedProperty)rb.Property("ReleaseDate")).Value = custNumber;
                        //((RedProperty)rb.Property("FreeFlag")).Value = custNumber;
                        //((RedProperty)rb.Property("PIN")).Value = custNumber;
                        //((RedProperty)rb.Property("PINHint")).Value = custNumber;
                        if (o.Billing.OptOut == "true")
                            ((RedProperty)rb.Property("OptOutDate")).Value = DateTime.Now.ToString("d");
                        else
                            ((RedProperty)rb.Property("OptInFlag")).Value = "W";
                        ((RedProperty)rb.Property("OptInDate")).Value = DateTime.Now.ToString("d");
                        //((RedProperty)rb.Property("Etrack")).Value = custNumber;
                        //((RedProperty)rb.Property("IPSource")).Value = o.;
                        rb.CallMethod("AddOrder");

                        o.CustNumber = ((RedProperty)rb.Property("CustNumber")).Value;
                        o.OrderNumber = ((RedProperty)rb.Property("OrderNumber")).Value;
                        o.OrderErrCode = ((RedProperty)rb.Property("OrderErrCode")).Value;
                        o.OrderErrMsg = ((RedProperty)rb.Property("OrderErrMsg")).Value;
                        o.ValidErrCode = ((RedProperty)rb.Property("ValidErrCode")).Value;
                        o.ValidErrMsg = ((RedProperty)rb.Property("ValidErrMsg")).Value;

                        rsp.Append("\t<Order>\n\t\t<OrderNumber>" + ((RedProperty)rb.Property("OrderNumber")).Value + "</OrderNumber>\n"
                            + "\t\t<CustNumber>" + ((RedProperty)rb.Property("CustNumber")).Value + "</CustNumber>\n"
                            + "\t\t<GVCFlag>" + ((RedProperty)rb.Property("GVCFlag")).Value + "</GVCFlag>\n"
                            + "\t\t<GVCDate>" + ((RedProperty)rb.Property("GVCDate")).Value + "</GVCDate>\n"
                            + "\t\t<ValidErrCode>" + ((RedProperty)rb.Property("ValidErrCode")).Value + "</ValidErrCode>\n"
                            + "\t\t<ValidErrMsg>" + ((RedProperty)rb.Property("ValidErrMsg")).Value + "</ValidErrMsg>\n"
                            + "\t\t<OrderErrCode>" + ((RedProperty)rb.Property("OrderErrCode")).Value + "</OrderErrCode>\n"
                            + "\t\t<OrderErrMsg>" + ((RedProperty)rb.Property("OrderErrMsg")).Value + "</OrderErrMsg>\n\t</Order>\n");

                        rb.Close();
                    }
                    catch (Exception ex)
                    {
                        isRedBackAvailable = false;
                        rsp.Append("\t<Error>\n\t\t<Message>" + ex.Message + "</Message>\n\t\t<AdditionalInfo>" + ex.StackTrace + "</AdditionalInfo>\n");
                    }
                }
            }
        }
        rsp.Append("</RedBackResponse>\n");

        return rsp.ToString();
    }    
    #endregion

    #region OrderStatus and Customer Info
    public string FetchOrder(string custNumber, string custZip, string brandCode, string orderNumber)
    {
        RedObject rb = new RedObject();
        StringBuilder rsp = new StringBuilder();

        rb.Open3(RedBackAccount, "OPM:Order_Status");
        ((RedProperty)rb.Property("CustNumber")).Value = custNumber;
        ((RedProperty)rb.Property("CustZip")).Value = custZip;
        ((RedProperty)rb.Property("Title")).Value = brandCode;
        ((RedProperty)rb.Property("DetOrderNumber")).Value = orderNumber;
        rb.CallMethod("OrderStatus");

        string msg = ((RedProperty)rb.Property("StatusMsg")).Value;
        rsp.Append("<Order>\n");
        rsp.Append("\t<Message>" + msg + "</Message>\n");
        rsp.Append("\t<Details>\n");
        rsp.Append("\t\t<OrderNumber>" + ((RedProperty)rb.Property("DetOrderNumber")).Value + "</OrderNumber>\n");
        rsp.Append("\t\t<OrderDate>" + ((RedProperty)rb.Property("DetOrderDate")).Value + "</OrderDate>\n");
        rsp.Append("\t\t<OrderAmount>" + ((RedProperty)rb.Property("DetOrderAmount")).Value + "</OrderAmount>\n");
        rsp.Append("\t\t<ShipName>" + ((RedProperty)rb.Property("DetShipName")).Value + "</ShipName>\n");
        rsp.Append("\t\t<ShipAdd1>" + ((RedProperty)rb.Property("DetShipAdd1")).Value + "</ShipAdd1>\n");
        rsp.Append("\t\t<ShipAdd2>" + ((RedProperty)rb.Property("DetShipAdd2")).Value + "</ShipAdd2>\n");
        rsp.Append("\t\t<ShipCity>" + ((RedProperty)rb.Property("DetShipCity")).Value + "</ShipCity>\n");
        rsp.Append("\t\t<ShipState>" + ((RedProperty)rb.Property("DetShipState")).Value + "</ShipState>\n");
        rsp.Append("\t\t<ShipZip>" + ((RedProperty)rb.Property("DetShipZip")).Value + "</ShipZip>\n");
        rsp.Append(GetShipments(rb));
        rsp.Append(GetOpenItems("OpenLocs", "Loc", ((RedProperty)rb.Property("DetOpenLocs")).Value, "ü"));
        rsp.Append(GetOpenItems("OpenItems", "Item", ((RedProperty)rb.Property("DetOpenItems")).Value, "ü"));
        rsp.Append(GetOpenItems("OpenQtys", "Qtys", ((RedProperty)rb.Property("DetOpenQtys")).Value, "ü"));
        rsp.Append(GetOpenItems("OpenDesc", "Desc", ((RedProperty)rb.Property("DetOpenDesc")).Value, "ü"));
        rsp.Append(GetOpenItems("OpenEstDelBegin", "EstDelBegin", ((RedProperty)rb.Property("DetOpenEstDelBegin")).Value, "ü"));
        rsp.Append(GetOpenItems("OpenEstDelEnd", "EstDelEnd", ((RedProperty)rb.Property("DetOpenEstDelEnd")).Value, "ü"));
        rsp.Append("\t</Details>\n");
        rsp.Append("</Order>\n");

        return rsp.ToString();
    }

    private string GetShipments(RedObject rb)
    {
        string[] fields = { "DetShipNumber", "DetShipItems", "DetShipQty", "DetShipDesc", "DetShipVia", "DetShipDate", "DetShipTrackNums", "DetShipTrackLink", "DetShipEstDelBegin", "DetShipEstDelEnd" };

        string[] f = ((RedProperty)rb.Property("DetShipNumber")).Value.Split(new string[] { "ý" }, StringSplitOptions.None);
        string[] shipments = new string[f.GetLength(0)];
        int i = 0;
        foreach (string d in f)
        {
            shipments[i] = "\t\t\t\t<ShipNumber>" + d + "</ShipNumber>\n";
            i++;
        }

        string[] itms = ((RedProperty)rb.Property("DetShipItems")).Value.Split(new string[] { "ý" }, StringSplitOptions.None);
        string[] qtys = ((RedProperty)rb.Property("DetShipQty")).Value.Split(new string[] { "ý" }, StringSplitOptions.None);
        string[] dscs = ((RedProperty)rb.Property("DetShipDesc")).Value.Split(new string[] { "ý" }, StringSplitOptions.None);
        string[] bgns = ((RedProperty)rb.Property("DetShipEstDelBegin")).Value.Split(new string[] { "ý" }, StringSplitOptions.None);
        string[] ends = ((RedProperty)rb.Property("DetShipEstDelEnd")).Value.Split(new string[] { "ý" }, StringSplitOptions.None);
        string[] items = new string[itms.GetLength(0)];

        for (i = 0; i < itms.GetLength(0); i++)
        {
            string[] ix = itms[i].Split(new string[] { "ü" }, StringSplitOptions.None);
            string[] qx = qtys[i].Split(new string[] { "ü" }, StringSplitOptions.None);
            string[] dx = dscs[i].Split(new string[] { "ü" }, StringSplitOptions.None);
            string[] bx = bgns[i].Split(new string[] { "ü" }, StringSplitOptions.None);
            string[] ex = ends[i].Split(new string[] { "ü" }, StringSplitOptions.None);
            items[i] = "";
            for (int j = 0; j < ix.GetLength(0); j++)
            {
                items[i] += "\t\t\t\t\t<Item>\n"
                    + "\t\t\t\t\t\t<Sku>" + ix[j] + "</Sku>\n"
                    + "\t\t\t\t\t\t<Qty>" + qx[j] + "</Qty>\n"
                    + "\t\t\t\t\t\t<Desc>" + dx[j] + "</Desc>\n"
                    + "\t\t\t\t\t\t<EstDelBegin>" + bx[j] + "</EstDelBegin>\n"
                    + "\t\t\t\t\t\t<EstDelEnd>" + ex[j] + "</EstDelEnd>\n"
                    + "\t\t\t\t\t</Item>\n";
            }
            shipments[i] += "\t\t\t\t<Items>\n" + items[i] + "\t\t\t\t</Items>\n";
        }

        f = ((RedProperty)rb.Property("DetShipVia")).Value.Split(new string[] { "ý" }, StringSplitOptions.None);
        i = 0;
        foreach (string d in f)
        {
            shipments[i] += "\t\t\t\t<ShipVia>" + d + "</ShipVia>\n";
            i++;
        }

        f = ((RedProperty)rb.Property("DetShipDate")).Value.Split(new string[] { "ý" }, StringSplitOptions.None);
        i = 0;
        foreach (string d in f)
        {
            shipments[i] += "\t\t\t\t<ShipDate>" + d + "</ShipDate>\n";
            i++;
        }

        f = ((RedProperty)rb.Property("DetShipTrackNums")).Value.Split(new string[] { "ý" }, StringSplitOptions.None);
        i = 0;
        foreach (string d in f)
        {
            shipments[i] += "\t\t\t\t<TrackNum>" + d + "</TrackNum>\n";
            i++;
        }

        f = ((RedProperty)rb.Property("DetShipTrackLink")).Value.Split(new string[] { "ý" }, StringSplitOptions.None);
        i = 0;
        foreach (string d in f)
        {
            shipments[i] += "\t\t\t\t<TrackLink>" + d + "</TrackLink>\n";
            i++;
        }

        string rtn = "\t\t<Shipments>\n";
        foreach (string d in shipments)
            rtn += "\t\t\t<Shipment>\n" + d + "\t\t\t</Shipment>\n";
        rtn += "\t\t</Shipments>\n";
        return rtn;
    }

    private string GetOpenItems(string node, string sub, string data, string split)
    {
        string[] l = data.Split(new string[] { split }, StringSplitOptions.None);
        string rtn = "";
        if (l.GetUpperBound(0) >= 0)
        {
            rtn += "\t\t<" + node + ">\n";
            foreach (string x in l)
                rtn += "\t\t\t<" + sub + ">" + x + "</" + sub + ">\n";
            rtn += "\t\t</" + node + ">\n";
        }
        else
        {
            rtn += "\t\t<" + node + "></" + node + ">\n";
        }
        return rtn;
    }

    private string GetOpenItems(string node, string sub, string subsub, string data, string split, string subsplit)
    {
        string[] l = data.Split(new string[] { split }, StringSplitOptions.None);
        string rtn = "";
        if (l.GetUpperBound(0) >= 0)
        {
            rtn += "\t\t<" + node + ">\n";
            foreach (string x in l)
            {
                rtn += "\t\t\t<" + sub + ">\n";
                string[] ls = x.Split(new string[] { subsplit }, StringSplitOptions.None);
                foreach (string y in ls)
                {
                    rtn += "\t\t\t\t<" + subsub + ">" + y + "</" + subsub + ">\n";
                }
                rtn += "</" + sub + ">\n";
            }
            rtn += "\t\t</" + node + ">\n";
        }
        else
        {
            rtn += "\t\t<" + node + "></" + node + ">\n";
        }
        return rtn;
    }

    public string FetchOrders(string custNumber, string custZip, string brandCode)
    {
        RedObject rb = new RedObject();
        StringBuilder rsp = new StringBuilder();

        try
        {
            rb.Open3(RedBackAccount, "OPM:Order_Status");
            ((RedProperty)rb.Property("CustNumber")).Value = custNumber;
            ((RedProperty)rb.Property("CustZip")).Value = custZip;
            ((RedProperty)rb.Property("Title")).Value = brandCode;

            rb.CallMethod("OrderStatus");

            string[] o = ((RedProperty)rb.Property("SumOrderNumbers")).Value.Split(new string[] { "ý" }, StringSplitOptions.None);
            string[] d = ((RedProperty)rb.Property("SumOrderDates")).Value.Split(new string[] { "ý" }, StringSplitOptions.None);
            string[] n = ((RedProperty)rb.Property("SumShipNames")).Value.Split(new string[] { "ý" }, StringSplitOptions.None);
            string[] s = ((RedProperty)rb.Property("SumShipStatus")).Value.Split(new string[] { "ý" }, StringSplitOptions.None);
            string[] t = ((RedProperty)rb.Property("SumOrderTotal")).Value.Split(new string[] { "ý" }, StringSplitOptions.None);

            string msg = ((RedProperty)rb.Property("StatusMsg")).Value;
            msg += ((RedProperty)rb.Property("StatusErr")).Value;

            rsp.Append("<Orders>\n");
            rsp.Append("\t<Message>" + msg + "</Message>\n");
            if (((RedProperty)rb.Property("DetOrderNumber")).Value.Length > 0)
            {
                rsp.Append("\t<Order>\n");
                rsp.Append("\t\t<OrderNumber>" + ((RedProperty)rb.Property("DetOrderNumber")).Value + "</OrderNumber>\n");
                rsp.Append("\t\t<OrderDate>" + ((RedProperty)rb.Property("DetOrderDate")).Value + "</OrderDate>\n");
                rsp.Append("\t\t<ShipName>" + ((RedProperty)rb.Property("DetShipName")).Value + "</ShipName>\n");
                rsp.Append("\t\t<ShipStatus></ShipStatus>\n");
                rsp.Append("\t\t<OrderTotal>" + ((RedProperty)rb.Property("DetOrderAmount")).Value + "</OrderTotal>\n");
                rsp.Append("\t</Order>\n");
            }
            for (int i = 0; i <= o.GetUpperBound(0); i++)
            {
                rsp.Append("\t<Order>\n");
                rsp.Append("\t\t<OrderNumber>" + o[i] + "</OrderNumber>\n");
                rsp.Append("\t\t<OrderDate>" + d[i] + "</OrderDate>\n");
                rsp.Append("\t\t<ShipName>" + n[i] + "</ShipName>\n");
                rsp.Append("\t\t<ShipStatus>" + s[i] + "</ShipStatus>\n");
                rsp.Append("\t\t<OrderTotal>" + t[i] + "</OrderTotal>\n");
                rsp.Append("\t</Order>\n");
            }
            rsp.Append("</Orders>\n");
        }
        catch (Exception ex)
        {
            rsp.Append("<Orders>\n\t<Message>Error: " + ex.Message + "</Message>\n\t<AdditionalInfo>" + ex.StackTrace + "</AdditionalInfo>\n</Orders>");
        }

        return rsp.ToString();
    }

    public string FetchCustomer(string custNumber, string custZip)
    {
        StringBuilder rsp = new StringBuilder();
        if ((custNumber != "") && (custZip != ""))
        {
            RedObject rb = new RedObject();
            rb.Open3(RedBackAccount, "OPM:Cust_Master");
            ((RedProperty)rb.Property("CustNumber")).Value = custNumber;
            ((RedProperty)rb.Property("CustZip")).Value = custZip;
            rb.CallMethod("AccessCust");
            string custMsg = "Success";
            if (((RedProperty)rb.Property("CustError")).Value != "0")
                custMsg = ((RedProperty)rb.Property("CustMessage")).Value;
            rsp.Append("<Customer>\n");
            rsp.Append("\t<Message>" + custMsg + "</Message>\n");
            rsp.Append("\t<Address>\n");
            rsp.Append("\t\t<Type>Shipping</Type>\n");
            rsp.Append("\t\t<FirstName>" + ((RedProperty)rb.Property("CustFirstName")).Value + "</FirstName>\n");
            rsp.Append("\t\t<LastName>" + ((RedProperty)rb.Property("CustLastName")).Value + "</LastName>\n");
            rsp.Append("\t\t<Address1>" + ((RedProperty)rb.Property("CustAdd1")).Value + "</Address1>\n");
            rsp.Append("\t\t<Address2>" + ((RedProperty)rb.Property("CustAdd2")).Value + "</Address2>\n");
            rsp.Append("\t\t<City>" + ((RedProperty)rb.Property("CustCity")).Value + "</City>\n");
            rsp.Append("\t\t<State>" + ((RedProperty)rb.Property("CustState")).Value + "</State>\n");
            rsp.Append("\t\t<Zip>" + ((RedProperty)rb.Property("CustZip")).Value + "</Zip>\n");
            rsp.Append("\t\t<Country>" + ((RedProperty)rb.Property("CustCountry")).Value + "</Country>\n");
            rsp.Append("\t\t<Email>" + ((RedProperty)rb.Property("CustEmail")).Value + "</Email>\n");
            rsp.Append("\t\t<Phone>" + ((RedProperty)rb.Property("CustPhone")).Value + "</Phone>\n");
            rsp.Append("\t</Address>\n");
            rsp.Append("\t<Address>\n");
            rsp.Append("\t\t<Type>Billing</Type>\n");
            rsp.Append("\t\t<FirstName>" + ((RedProperty)rb.Property("CustFirstName")).Value + "</FirstName>\n");
            rsp.Append("\t\t<LastName>" + ((RedProperty)rb.Property("CustLastName")).Value + "</LastName>\n");
            rsp.Append("\t\t<Address1>" + ((RedProperty)rb.Property("CustAdd1")).Value + "</Address1>\n");
            rsp.Append("\t\t<Address2>" + ((RedProperty)rb.Property("CustAdd2")).Value + "</Address2>\n");
            rsp.Append("\t\t<City>" + ((RedProperty)rb.Property("CustCity")).Value + "</City>\n");
            rsp.Append("\t\t<State>" + ((RedProperty)rb.Property("CustState")).Value + "</State>\n");
            rsp.Append("\t\t<Zip>" + ((RedProperty)rb.Property("CustZip")).Value + "</Zip>\n");
            rsp.Append("\t\t<Country>" + ((RedProperty)rb.Property("CustCountry")).Value + "</Country>\n");
            rsp.Append("\t\t<Email>" + ((RedProperty)rb.Property("CustEmail")).Value + "</Email>\n");
            rsp.Append("\t\t<Phone>" + ((RedProperty)rb.Property("CustPhone")).Value + "</Phone>\n");
            rsp.Append("\t</Address>\n");
            rsp.Append("\t<KeycodeValid></KeycodeValid>\n");
            rsp.Append("\t<RevCreditPlan></RevCreditPlan>\n");
            rsp.Append("\t<InterestRate></InterestRate>\n");
            rsp.Append("\t<CustNumber>" + ((RedProperty)rb.Property("CustNumber")).Value + "</CustNumber>\n");
            rsp.Append("</Customer>\n");
        }
        else
        {
            rsp.Append("<Customer>\n");
            rsp.Append("\t<Message>No data</Message>\n");
            rsp.Append("\t<Address>\n");
            rsp.Append("\t\t<Type>Shipping</Type>\n");
            rsp.Append("\t\t<FirstName></FirstName>\n");
            rsp.Append("\t\t<LastName></LastName>\n");
            rsp.Append("\t\t<Address1></Address1>\n");
            rsp.Append("\t\t<Address2></Address2>\n");
            rsp.Append("\t\t<City></City>\n");
            rsp.Append("\t\t<State></State>\n");
            rsp.Append("\t\t<Zip></Zip>\n");
            rsp.Append("\t\t<Country></Country>\n");
            rsp.Append("\t\t<Email></Email>\n");
            rsp.Append("\t\t<Phone></Phone>\n");
            rsp.Append("\t</Address>\n");
            rsp.Append("\t<Address>\n");
            rsp.Append("\t\t<Type>Billing</Type>\n");
            rsp.Append("\t\t<FirstName></FirstName>\n");
            rsp.Append("\t\t<LastName></LastName>\n");
            rsp.Append("\t\t<Address1></Address1>\n");
            rsp.Append("\t\t<Address2></Address2>\n");
            rsp.Append("\t\t<City></City>\n");
            rsp.Append("\t\t<State></State>\n");
            rsp.Append("\t\t<Zip></Zip>\n");
            rsp.Append("\t\t<Country></Country>\n");
            rsp.Append("\t\t<Email></Email>\n");
            rsp.Append("\t\t<Phone></Phone>\n");
            rsp.Append("\t</Address>\n");
            rsp.Append("\t<KeycodeValid></KeycodeValid>\n");
            rsp.Append("\t<RevCreditPlan></RevCreditPlan>\n");
            rsp.Append("\t<InterestRate></InterestRate>\n");
            rsp.Append("\t<CustNumber></CustNumber>\n");
            rsp.Append("</Customer>\n");
        }
        return rsp.ToString();
    }
    #endregion

    #region CatalogRequest and NewsletterSignup
    public string CatalogRequest(string brandCode, string firstName, string lastName, string company, string address1, string address2, string city, string state, string zip, string country, string email, string emip, string phone, string notes, string optout, string keycode, ref string errors)
    {
        string rsp = "";
        RedObject rb = new RedObject();

        rb.Open3(RedBackAccount, "OPM:Catalog_Request");
        ((RedProperty)rb.Property("Title")).Value = brandCode;
        ((RedProperty)rb.Property("CustEmail")).Value = email;
        ((RedProperty)rb.Property("CustEmIP")).Value = emip;
        ((RedProperty)rb.Property("CustFirstName")).Value = firstName;
        ((RedProperty)rb.Property("CustLastName")).Value = lastName;
        ((RedProperty)rb.Property("CustCompany")).Value = company;
        ((RedProperty)rb.Property("CustAddr1")).Value = address1;
        ((RedProperty)rb.Property("CustAddr2")).Value = address2;
        ((RedProperty)rb.Property("CustCity")).Value = city;
        ((RedProperty)rb.Property("CustState")).Value = state;
        ((RedProperty)rb.Property("CustZip")).Value = zip;
        ((RedProperty)rb.Property("CustPhone")).Value = phone;
        if (keycode.ToLower() != "")
            ((RedProperty)rb.Property("PromoCode")).Value = keycode.ToUpper();
        if (optout.ToLower() == "w")
            ((RedProperty)rb.Property("OptOutFlag")).Value = optout.ToUpper();
        rb.CallMethod("CatRequest");
        errors = ((RedProperty)rb.Property("CatErrCode")).Value + "," + ((RedProperty)rb.Property("CatErrMsg")).Value;
        rsp = "<Catalog>\n"
            + "\t<Code>" + ((RedProperty)rb.Property("CatErrCode")).Value + "</Code>\n"
            + "\t<Message>" + ((RedProperty)rb.Property("CatErrMsg")).Value + "</Message>\n"
            + "</Catalog>\n";

        if (optout.ToLower() == "w")
        {
            RedObject rb2 = new RedObject();
            rb2.Open3(RedBackAccount, "OPM:Catalog_Request");
            ((RedProperty)rb2.Property("Title")).Value = brandCode;
            ((RedProperty)rb2.Property("CustEmail")).Value = email;
            ((RedProperty)rb2.Property("CustEmIP")).Value = emip;
            ((RedProperty)rb2.Property("OptOutFlag")).Value = optout.ToUpper();
            rb2.CallMethod("OptOutRequest");
            errors += ";" + ((RedProperty)rb2.Property("CatErrCode")).Value + "," + ((RedProperty)rb2.Property("CatErrMsg")).Value;
            rsp += "<OptOut>\n"
                + "\t<Code>" + ((RedProperty)rb2.Property("CatErrCode")).Value + "</Code>\n"
                + "\t<Message>" + ((RedProperty)rb2.Property("CatErrMsg")).Value + "</Message>\n"
                + "</OptOut>\n";
        }
        return rsp;
    }

    public string NewsletterSignup(string brandCode, string email, string emip, string optout, string keycode, ref string errors)
    {
        string rsp = "";
        RedObject rb = new RedObject();
        rb.Open3(RedBackAccount, "OPM:Catalog_Request");
        ((RedProperty)rb.Property("Title")).Value = brandCode;
        ((RedProperty)rb.Property("CustEmail")).Value = email;
        ((RedProperty)rb.Property("CustEmIP")).Value = emip;

        if (keycode.ToLower() != "")
            ((RedProperty)rb.Property("PromoCode")).Value = keycode.ToUpper();

        if (optout.ToLower() == "w")
        {
            ((RedProperty)rb.Property("OptOutFlag")).Value = optout.ToUpper();
            rb.CallMethod("OptOutRequest");
        }
        else
        {
            rb.CallMethod("CatRequest");
        }

        errors = ((RedProperty)rb.Property("CatErrCode")).Value + "," + ((RedProperty)rb.Property("CatErrMsg")).Value;
        rsp = "<Newsletter>\n"
            + "\t<Code>" + ((RedProperty)rb.Property("CatErrCode")).Value + "</Code>\n"
            + "\t<Message>" + ((RedProperty)rb.Property("CatErrMsg")).Value + "</Message>\n"
            + ((optout.ToLower() == "w") ? "\t<OptOutFlag>W</OptOutFlag>\n" : "")
            + "</Newsletter>\n";

        return rsp;
    }
    #endregion

    public string KeyCodeLogin(string keyCode, string title, string custNumber, string custZip)
    {
        StringBuilder rsp = new StringBuilder();
        if ((custNumber != "") && (custZip != ""))
        {
            RedObject rb = new RedObject();
            rb.Open3(RedBackAccount, "OPM:KeycodeLogin");
            ((RedProperty)rb.Property("Keycode")).Value = keyCode;
            ((RedProperty)rb.Property("Title")).Value = title;
	    ((RedProperty)rb.Property("ZipCode")).Value = custZip;
 	    ((RedProperty)rb.Property("CustNumber")).Value = custNumber;
            rb.CallMethod("KeycodeLogin");
            string custMsg = "Success";
            if (((RedProperty)rb.Property("CustError")).Value != "")
               custMsg = ((RedProperty)rb.Property("CustMessage")).Value;
            rsp.Append("<Customer>\n");
            rsp.Append("\t<Message>" + custMsg + "</Message>\n");
            rsp.Append("\t<Address>\n");
            rsp.Append("\t\t<Type>Shipping</Type>\n");
            rsp.Append("\t\t<FirstName>" + ((RedProperty)rb.Property("CustFirstName")).Value + "</FirstName>\n");
            rsp.Append("\t\t<LastName>" + ((RedProperty)rb.Property("CustLastName")).Value + "</LastName>\n");
            rsp.Append("\t\t<Address1>" + ((RedProperty)rb.Property("CustAdd1")).Value + "</Address1>\n");
            rsp.Append("\t\t<Address2>" + ((RedProperty)rb.Property("CustAdd2")).Value + "</Address2>\n");
            rsp.Append("\t\t<City>" + ((RedProperty)rb.Property("CustCity")).Value + "</City>\n");
            rsp.Append("\t\t<State>" + ((RedProperty)rb.Property("CustState")).Value + "</State>\n");
            rsp.Append("\t\t<Zip>" + ((RedProperty)rb.Property("CustZip")).Value + "</Zip>\n");
            rsp.Append("\t\t<Country>" + ((RedProperty)rb.Property("CustCountry")).Value + "</Country>\n");
            rsp.Append("\t\t<Email>" + ((RedProperty)rb.Property("CustEmail")).Value + "</Email>\n");
            rsp.Append("\t\t<Phone>" + ((RedProperty)rb.Property("CustPhone")).Value + "</Phone>\n");
            rsp.Append("\t</Address>\n");
            rsp.Append("\t<Address>\n");
            rsp.Append("\t\t<Type>Billing</Type>\n");
            rsp.Append("\t\t<FirstName>" + ((RedProperty)rb.Property("CustFirstName")).Value + "</FirstName>\n");
            rsp.Append("\t\t<LastName>" + ((RedProperty)rb.Property("CustLastName")).Value + "</LastName>\n");
            rsp.Append("\t\t<Address1>" + ((RedProperty)rb.Property("CustAdd1")).Value + "</Address1>\n");
            rsp.Append("\t\t<Address2>" + ((RedProperty)rb.Property("CustAdd2")).Value + "</Address2>\n");
            rsp.Append("\t\t<City>" + ((RedProperty)rb.Property("CustCity")).Value + "</City>\n");
            rsp.Append("\t\t<State>" + ((RedProperty)rb.Property("CustState")).Value + "</State>\n");
            rsp.Append("\t\t<Zip>" + ((RedProperty)rb.Property("CustZip")).Value + "</Zip>\n");
            rsp.Append("\t\t<Country>" + ((RedProperty)rb.Property("CustCountry")).Value + "</Country>\n");
            rsp.Append("\t\t<Email>" + ((RedProperty)rb.Property("CustEmail")).Value + "</Email>\n");
            rsp.Append("\t\t<Phone>" + ((RedProperty)rb.Property("CustPhone")).Value + "</Phone>\n");
            rsp.Append("\t</Address>\n");
            rsp.Append("\t<KeycodeValid>" + ((RedProperty)rb.Property("KeycodeValid")).Value + "</KeycodeValid>\n");
            rsp.Append("\t<RevCreditPlan>" + ((RedProperty)rb.Property("RevCreditPlan")).Value + "</RevCreditPlan>\n");
            rsp.Append("\t<InterestRate>" + ((RedProperty)rb.Property("InterestRate")).Value + "</InterestRate>\n");
            rsp.Append("\t<CustNumber>" + ((RedProperty)rb.Property("CustNumber")).Value + "</CustNumber>\n");
            rsp.Append("</Customer>\n");
        }
        else
        {
            rsp.Append("<Customer>\n");
            rsp.Append("\t<Message>No data</Message>\n");
            rsp.Append("\t<Address>\n");
            rsp.Append("\t\t<Type>Shipping</Type>\n");
            rsp.Append("\t\t<FirstName></FirstName>\n");
            rsp.Append("\t\t<LastName></LastName>\n");
            rsp.Append("\t\t<Address1></Address1>\n");
            rsp.Append("\t\t<Address2></Address2>\n");
            rsp.Append("\t\t<City></City>\n");
            rsp.Append("\t\t<State></State>\n");
            rsp.Append("\t\t<Zip></Zip>\n");
            rsp.Append("\t\t<Country></Country>\n");
            rsp.Append("\t\t<Email></Email>\n");
            rsp.Append("\t\t<Phone></Phone>\n");
            rsp.Append("\t</Address>\n");
            rsp.Append("\t<Address>\n");
            rsp.Append("\t\t<Type>Billing</Type>\n");
            rsp.Append("\t\t<FirstName></FirstName>\n");
            rsp.Append("\t\t<LastName></LastName>\n");
            rsp.Append("\t\t<Address1></Address1>\n");
            rsp.Append("\t\t<Address2></Address2>\n");
            rsp.Append("\t\t<City></City>\n");
            rsp.Append("\t\t<State></State>\n");
            rsp.Append("\t\t<Zip></Zip>\n");
            rsp.Append("\t\t<Country></Country>\n");
            rsp.Append("\t\t<Email></Email>\n");
            rsp.Append("\t\t<Phone></Phone>\n");
            rsp.Append("\t</Address>\n");
            rsp.Append("\t<KeycodeValid></KeycodeValid>\n");
            rsp.Append("\t<RevCreditPlan></RevCreditPlan>\n");
            rsp.Append("\t<InterestRate></InterestRate>\n");
            rsp.Append("\t<CustNumber></CustNumber>\n");
            rsp.Append("</Customer>\n");
        }
        return rsp.ToString();
    }

    public string FetchCustomerByEmailZip(string custEmail, string custZip)
    {
        StringBuilder rsp = new StringBuilder();
        if ((custEmail != "") && (custZip != ""))
        {
            RedObject rb = new RedObject();
            rb.Open3(RedBackAccount, "OPM:Cust_Master");
            ((RedProperty)rb.Property("CustEmail")).Value = custEmail;
            ((RedProperty)rb.Property("CustPin")).Value = custZip;
            rb.CallMethod("AccessCust");
            string custMsg = "Success";
            if (((RedProperty)rb.Property("CustError")).Value != "0")
                custMsg = ((RedProperty)rb.Property("CustMessage")).Value;
            rsp.Append("<Customer>\n");
            rsp.Append("\t<Message>" + custMsg + "</Message>\n");
            rsp.Append("\t<Address>\n");
            rsp.Append("\t\t<Type>Shipping</Type>\n");
            rsp.Append("\t\t<FirstName>" + ((RedProperty)rb.Property("CustFirstName")).Value + "</FirstName>\n");
            rsp.Append("\t\t<LastName>" + ((RedProperty)rb.Property("CustLastName")).Value + "</LastName>\n");
            rsp.Append("\t\t<Address1>" + ((RedProperty)rb.Property("CustAdd1")).Value + "</Address1>\n");
            rsp.Append("\t\t<Address2>" + ((RedProperty)rb.Property("CustAdd2")).Value + "</Address2>\n");
            rsp.Append("\t\t<City>" + ((RedProperty)rb.Property("CustCity")).Value + "</City>\n");
            rsp.Append("\t\t<State>" + ((RedProperty)rb.Property("CustState")).Value + "</State>\n");
            rsp.Append("\t\t<Zip>" + ((RedProperty)rb.Property("CustZip")).Value + "</Zip>\n");
            rsp.Append("\t\t<Country>" + ((RedProperty)rb.Property("CustCountry")).Value + "</Country>\n");
            rsp.Append("\t\t<Email>" + ((RedProperty)rb.Property("CustEmail")).Value + "</Email>\n");
            rsp.Append("\t\t<Phone>" + ((RedProperty)rb.Property("CustPhone")).Value + "</Phone>\n");
            rsp.Append("\t</Address>\n");
            rsp.Append("\t<Address>\n");
            rsp.Append("\t\t<Type>Billing</Type>\n");
            rsp.Append("\t\t<FirstName>" + ((RedProperty)rb.Property("CustFirstName")).Value + "</FirstName>\n");
            rsp.Append("\t\t<LastName>" + ((RedProperty)rb.Property("CustLastName")).Value + "</LastName>\n");
            rsp.Append("\t\t<Address1>" + ((RedProperty)rb.Property("CustAdd1")).Value + "</Address1>\n");
            rsp.Append("\t\t<Address2>" + ((RedProperty)rb.Property("CustAdd2")).Value + "</Address2>\n");
            rsp.Append("\t\t<City>" + ((RedProperty)rb.Property("CustCity")).Value + "</City>\n");
            rsp.Append("\t\t<State>" + ((RedProperty)rb.Property("CustState")).Value + "</State>\n");
            rsp.Append("\t\t<Zip>" + ((RedProperty)rb.Property("CustZip")).Value + "</Zip>\n");
            rsp.Append("\t\t<Country>" + ((RedProperty)rb.Property("CustCountry")).Value + "</Country>\n");
            rsp.Append("\t\t<Email>" + ((RedProperty)rb.Property("CustEmail")).Value + "</Email>\n");
            rsp.Append("\t\t<Phone>" + ((RedProperty)rb.Property("CustPhone")).Value + "</Phone>\n");
            rsp.Append("\t</Address>\n");
            rsp.Append("\t<KeycodeValid></KeycodeValid>\n");
            rsp.Append("\t<RevCreditPlan></RevCreditPlan>\n");
            rsp.Append("\t<InterestRate></InterestRate>\n");
            rsp.Append("\t<CustNumber>" + ((RedProperty)rb.Property("CustNumber")).Value + "</CustNumber>\n");
            rsp.Append("</Customer>\n");
        }
        else
        {
            rsp.Append("<Customer>\n");
            rsp.Append("\t<Message>No data</Message>\n");
            rsp.Append("\t<Address>\n");
            rsp.Append("\t\t<Type>Shipping</Type>\n");
            rsp.Append("\t\t<FirstName></FirstName>\n");
            rsp.Append("\t\t<LastName></LastName>\n");
            rsp.Append("\t\t<Address1></Address1>\n");
            rsp.Append("\t\t<Address2></Address2>\n");
            rsp.Append("\t\t<City></City>\n");
            rsp.Append("\t\t<State></State>\n");
            rsp.Append("\t\t<Zip></Zip>\n");
            rsp.Append("\t\t<Country></Country>\n");
            rsp.Append("\t\t<Email></Email>\n");
            rsp.Append("\t\t<Phone></Phone>\n");
            rsp.Append("\t</Address>\n");
            rsp.Append("\t<Address>\n");
            rsp.Append("\t\t<Type>Billing</Type>\n");
            rsp.Append("\t\t<FirstName></FirstName>\n");
            rsp.Append("\t\t<LastName></LastName>\n");
            rsp.Append("\t\t<Address1></Address1>\n");
            rsp.Append("\t\t<Address2></Address2>\n");
            rsp.Append("\t\t<City></City>\n");
            rsp.Append("\t\t<State></State>\n");
            rsp.Append("\t\t<Zip></Zip>\n");
            rsp.Append("\t\t<Country></Country>\n");
            rsp.Append("\t\t<Email></Email>\n");
            rsp.Append("\t\t<Phone></Phone>\n");
            rsp.Append("\t</Address>\n");
            rsp.Append("\t<KeycodeValid></KeycodeValid>\n");
            rsp.Append("\t<RevCreditPlan></RevCreditPlan>\n");
            rsp.Append("\t<InterestRate></InterestRate>\n");
            rsp.Append("\t<CustNumber></CustNumber>\n");
            rsp.Append("</Customer>\n");
        }
        return rsp.ToString();
    }
 


}
