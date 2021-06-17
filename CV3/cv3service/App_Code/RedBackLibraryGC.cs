using System;
using System.Collections.Generic;
using System.Web;
using REDPAGESLib;
using System.Text;

/// <summary>
/// Summary description for RedBackLibraryGC
/// </summary>
public class RedBackLibraryGC
{
    private const string RedBackAccount = "172.16.15.15:8403";
    //private const string RedBackAccount = "208.7.138.199:8402";

    public RedBackLibraryGC()
    {
    }


    public class GiftCardInfo
    {
        public string cardnum;
        public string balance;
        public string title;
        public string response;
        public string message;
    }
	
	public class CancelOrderResponse
    {
        public string SuccessFail;
        public string StatusError;
        public string StatusMsg; 
    }

    public GiftCardInfo GiftCerificateInfo(string code)
    {
        GiftCardInfo giftInfo = new GiftCardInfo();
        if ((code != ""))
        {
            RedObject rb = new RedObject();
            try
            {
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
                    giftInfo.title = ((RedProperty)rb.Property("Cert_Title")).Value;
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
                    giftInfo.title = ((RedProperty)rb.Property("Cert_Title")).Value;
                }

            }
            catch (Exception ex)
            {
                giftInfo.message = ex.Message;
            }

        }
        else
        {
            giftInfo.response = "error";
            giftInfo.balance = "";
            giftInfo.cardnum = "";
            giftInfo.title = "";
            giftInfo.message = "Please enter Gift Certificate number";
        }



        return giftInfo;
    }

	public CancelOrderResponse CancelWebOrder(string accountnumber, string ordernumber, string title, string cancelcode)
    {
        CancelOrderResponse resp = new CancelOrderResponse();

        if (!String.IsNullOrEmpty(accountnumber) && !String.IsNullOrEmpty(ordernumber) && !String.IsNullOrEmpty(title) && !String.IsNullOrEmpty(cancelcode))
        {
            RedObject rb = new RedObject();
            
            try
            {
                rb.Open3(RedBackAccount, "OPM:Cancel_Order");
               
                ((RedProperty)rb.Property("CustNumber")).Value = accountnumber;
                ((RedProperty)rb.Property("OrderNumber")).Value = ordernumber;
                ((RedProperty)rb.Property("CancelCode")).Value = cancelcode;
                ((RedProperty)rb.Property("Title")).Value = title;
				rb.CallMethod("WebCancelOrder");
                resp.SuccessFail = ((RedProperty)rb.Property("SuccessFail")).Value;
                resp.StatusError = ((RedProperty)rb.Property("StatusError")).Value;
                resp.StatusMsg = ((RedProperty)rb.Property("StatusMsg")).Value;
            }
            catch(Exception ex)
            {
                resp.SuccessFail = "F";
                resp.StatusError = "-1";
                resp.StatusMsg = ex.Message; 
            }
           
        }
        else
        {
            resp.SuccessFail = "F";
            resp.StatusError = "Error";
            resp.StatusMsg = "Missing required parameters";
        }

		// if(accountnumber == "12345")
		// {
			 // resp.SuccessFail = "F";
		// }
        return resp;
    }
}
