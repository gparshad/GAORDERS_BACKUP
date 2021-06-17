using System;
using System.Collections.Generic;
using System.Web;
using REDPAGESLib;
using System.Text;

/// <summary>
/// Summary description for RedBackLibraryTest
/// </summary>
public class RedBackLibraryTest
{
    private const string RedBackAccount = "172.16.15.15:8402";
    //private const string RedBackAccount = "208.7.138.199:8402";

    public RedBackLibraryTest()
    {
    }

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
            rsp.Append("</Customer>\n");
        }
        return rsp.ToString();
    }

    public string FetchCustomer(string custEmail, string custZip)
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
            rsp.Append("</Customer>\n");
        }
        return rsp.ToString();
    }

    public class GiftCardInfo
    {
        public string cardnum;
        public string balance;
        public string response;
        public string message;
    }

    public GiftCardInfo GiftCerificateInfo(string code)
    {
        GiftCardInfo giftInfo = new GiftCardInfo();
        if ((code != ""))
        {
            RedObject rb = new RedObject();
            rb.Open3(RedBackAccount, "OPM:Gift_Replacement_Cert");
            ((RedProperty)rb.Property("Cert_Number")).Value = code;
            if (code.EndsWith("CM"))
            {
                rb.CallMethod("ReplaceCertificate");
            }
            else
            {
                rb.CallMethod("GiftCertificate");

            }
            string custMsg = "Success";

            if (((RedProperty)rb.Property("Cert_Err")).Value == "0")
            {
                custMsg = ((RedProperty)rb.Property("Cert_Errmsg")).Value;
                giftInfo.response = "success";
                giftInfo.balance = ((RedProperty)rb.Property("Cert_Amount")).Value;
                giftInfo.cardnum = ((RedProperty)rb.Property("Cert_Number")).Value;
                if (code.EndsWith("CM"))
                {
                    giftInfo.message = ((RedProperty)rb.Property("Cert_Status")).Value;
                }
                else
                {
                    if (float.Parse(giftInfo.balance) > 0)
                    {
                        giftInfo.message = "Available";
                    }
                    else
                    {
                        giftInfo.message = "Used";
                    }
                }


            }
            else
            {
                giftInfo.response = "error";
                giftInfo.balance = ((RedProperty)rb.Property("Cert_Amount")).Value;
                giftInfo.cardnum = ((RedProperty)rb.Property("Cert_Number")).Value;
                giftInfo.message = ((RedProperty)rb.Property("Cert_Errmsg")).Value;
            }
        }
        else
        {
            giftInfo.response = "error";
            giftInfo.balance = "";
            giftInfo.cardnum = "";
            giftInfo.message = "Please enter Gift Certificate number";
        }

        return giftInfo;
    }

    public class TaxInfo
    {

        public string Brand;
        public string ShipState;
        public string ShipPostCd;
        public string Items;
        public string QtyOrdered;
        public string InMerchAmt;
        public string InShipAmt;

        public string NewTaxCode;
        public string NewTaxRate;
        public string NewTaxAmt;
        public string NewTotalAmt;
        public string OrderErrCode;
        public string OrderErrMsg;
    }

    public TaxInfo GetTaxInformation(TaxInfo taxInformation)
    {


        RedObject rb = new RedObject();
        rb.Open3(RedBackAccount, "OPM:Order_Calc");
        ((RedProperty)rb.Property("Brand")).Value = taxInformation.Brand;
        ((RedProperty)rb.Property("ShipState")).Value = taxInformation.ShipState;
        ((RedProperty)rb.Property("ShipPostCd")).Value = taxInformation.ShipPostCd;
        ((RedProperty)rb.Property("Items")).Value = taxInformation.Items;
        ((RedProperty)rb.Property("QtyOrdered")).Value = taxInformation.QtyOrdered;
        ((RedProperty)rb.Property("InMerchAmt")).Value = taxInformation.InMerchAmt;
        ((RedProperty)rb.Property("InShipAmt")).Value = taxInformation.InShipAmt;
        rb.CallMethod("CalcOrder");
        taxInformation.NewTaxCode = ((RedProperty)rb.Property("NewTaxCode")).Value;
        taxInformation.NewTaxRate = ((RedProperty)rb.Property("NewTaxRate")).Value;
        taxInformation.NewTaxAmt = ((RedProperty)rb.Property("NewTaxAmt")).Value;
        taxInformation.NewTotalAmt = ((RedProperty)rb.Property("NewTotalAmt")).Value;
        taxInformation.OrderErrCode = ((RedProperty)rb.Property("OrderErrCode")).Value;
        taxInformation.OrderErrMsg = ((RedProperty)rb.Property("OrderErrMsg")).Value;

        return taxInformation;
    }

    public string FetchOrder(string custNumber, string custZip, string brandCode, string orderNumber)
    {
        RedObject rb = new RedObject();
        StringBuilder rsp = new StringBuilder();

        rb.Open3(RedBackAccount, "OPM:Order_Status_ESD");
        ((RedProperty)rb.Property("CustNumber")).Value = custNumber;
        ((RedProperty)rb.Property("CustZip")).Value = custZip;
        ((RedProperty)rb.Property("Title")).Value = brandCode;
        ((RedProperty)rb.Property("DetOrderNumber")).Value = orderNumber;
        rb.CallMethod("OrderStatusESD");

        string msg = ((RedProperty)rb.Property("StatusMsg")).Value;
        rsp.Append("<Order>\n");
        rsp.Append("\t<Message>" + msg + "</Message>\n");
        rsp.Append("\t<CustNumber>" + ((RedProperty)rb.Property("CustNumber")).Value + "</CustNumber>\n");
        rsp.Append("\t<Zip>" + ((RedProperty)rb.Property("CustZip")).Value + "</Zip>\n");
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
        rsp.Append(GetOpenItems("OpenEstDelBegin", "EstDelBegin", ((RedProperty)rb.Property("DetOpenEstShpDate")).Value, "ü"));
        rsp.Append(GetOpenItems("OpenEstDelEnd", "EstDelEnd", ((RedProperty)rb.Property("DetOpenEstShpDesc")).Value, "ü"));
        rsp.Append("\t</Details>\n");
        rsp.Append("</Order>\n");

        return Helpers.SanitizeXml(rsp.ToString());
    }

    public string FetchOrder(string custEmail, string brandCode, string orderNumber)
    {
        RedObject rb = new RedObject();
        StringBuilder rsp = new StringBuilder();

        rb.Open3(RedBackAccount, "OPM:Order_Status_ESD");
        ((RedProperty)rb.Property("CustEmail")).Value = custEmail;
        ((RedProperty)rb.Property("Title")).Value = brandCode;
        ((RedProperty)rb.Property("DetOrderNumber")).Value = orderNumber;
        rb.CallMethod("OrderStatusESD");

        string msg = ((RedProperty)rb.Property("StatusMsg")).Value;
        rsp.Append("<Order>\n");
        rsp.Append("\t<Message>" + msg + "</Message>\n");
        rsp.Append("\t<CustNumber>" + ((RedProperty)rb.Property("CustNumber")).Value + "</CustNumber>\n");
        rsp.Append("\t<Zip>" + ((RedProperty)rb.Property("CustZip")).Value + "</Zip>\n");
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
        rsp.Append(GetOpenItems("OpenEstDelBegin", "EstDelBegin", ((RedProperty)rb.Property("DetOpenEstShpDate")).Value, "ü"));
        rsp.Append(GetOpenItems("OpenEstDelEnd", "EstDelEnd", ((RedProperty)rb.Property("DetOpenEstShpDesc")).Value, "ü"));
        rsp.Append("\t</Details>\n");
        rsp.Append("</Order>\n");

        return Helpers.SanitizeXml(rsp.ToString());
    }

    private string GetShipments(RedObject rb)
    {
        string[] fields = { "DetShipNumber", "DetShipItems", "DetShipQty", "DetShipDesc", "DetShipVia", "DetShipDate", "DetShipTrackNums", "DetShipTrackLink", "DetShipEstShpDate", "DetShipEstShpDesc" };

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
        string[] bgns = ((RedProperty)rb.Property("DetShipEstShpDate")).Value.Split(new string[] { "ý" }, StringSplitOptions.None);
        string[] ends = ((RedProperty)rb.Property("DetShipEstShpDesc")).Value.Split(new string[] { "ý" }, StringSplitOptions.None);
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
            rb.Open3(RedBackAccount, "OPM:Order_Status_ESD");
            ((RedProperty)rb.Property("CustNumber")).Value = custNumber;
            ((RedProperty)rb.Property("CustZip")).Value = custZip;
            ((RedProperty)rb.Property("Title")).Value = brandCode;

            rb.CallMethod("OrderStatusESD");

            string[] o = ((RedProperty)rb.Property("SumOrderNumbers")).Value.Split(new string[] { "ý" }, StringSplitOptions.None);
            string[] d = ((RedProperty)rb.Property("SumOrderDates")).Value.Split(new string[] { "ý" }, StringSplitOptions.None);
            string[] n = ((RedProperty)rb.Property("SumShipNames")).Value.Split(new string[] { "ý" }, StringSplitOptions.None);
            string[] s = ((RedProperty)rb.Property("SumShipStatus")).Value.Split(new string[] { "ý" }, StringSplitOptions.None);
            string[] t = ((RedProperty)rb.Property("SumOrderTotal")).Value.Split(new string[] { "ý" }, StringSplitOptions.None);

            string msg = ((RedProperty)rb.Property("StatusMsg")).Value;


            rsp.Append("<Orders>\n");
            rsp.Append("\t<Message>" + msg + "</Message>\n");
            rsp.Append("\t<CustNumber>" + ((RedProperty)rb.Property("CustNumber")).Value + "</CustNumber>\n");
            rsp.Append("\t<Zip>" + ((RedProperty)rb.Property("CustZip")).Value + "</Zip>\n");
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

        return Helpers.SanitizeXml(rsp.ToString());
    }

    public string FetchOrders(string custEmail, string brandCode)
    {
        RedObject rb = new RedObject();
        StringBuilder rsp = new StringBuilder();

        try
        {
            rb.Open3(RedBackAccount, "OPM:Order_Status_ESD");
            ((RedProperty)rb.Property("CustEmail")).Value = custEmail;
            ((RedProperty)rb.Property("Title")).Value = brandCode;

            rb.CallMethod("OrderStatusESD");

            string[] o = ((RedProperty)rb.Property("SumOrderNumbers")).Value.Split(new string[] { "ý" }, StringSplitOptions.None);
            string[] d = ((RedProperty)rb.Property("SumOrderDates")).Value.Split(new string[] { "ý" }, StringSplitOptions.None);
            string[] n = ((RedProperty)rb.Property("SumShipNames")).Value.Split(new string[] { "ý" }, StringSplitOptions.None);
            string[] s = ((RedProperty)rb.Property("SumShipStatus")).Value.Split(new string[] { "ý" }, StringSplitOptions.None);
            string[] t = ((RedProperty)rb.Property("SumOrderTotal")).Value.Split(new string[] { "ý" }, StringSplitOptions.None);

            string msg = ((RedProperty)rb.Property("StatusMsg")).Value;


            rsp.Append("<Orders>\n");
            rsp.Append("\t<Message>" + msg + "</Message>\n");
            rsp.Append("\t<CustNumber>" + ((RedProperty)rb.Property("CustNumber")).Value + "</CustNumber>\n");
            rsp.Append("\t<Zip>" + ((RedProperty)rb.Property("CustZip")).Value + "</Zip>\n");
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

        return Helpers.SanitizeXml(rsp.ToString());
    }


}
