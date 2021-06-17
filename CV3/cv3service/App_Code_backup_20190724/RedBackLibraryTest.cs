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
            if(code.EndsWith("CM"))
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
  		if(code.EndsWith("CM"))
	        {
                	giftInfo.message = ((RedProperty)rb.Property("Cert_Status")).Value;
		}
		else
		{
			if(float.Parse(giftInfo.balance) > 0)
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


   
}
