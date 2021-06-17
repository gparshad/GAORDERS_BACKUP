using REDPAGESLib;
using System;

namespace TaxWebAPI.RedBackLibrary
{
    public class TaxInfo
    {
        public string Title { get; set; }
        public string ShipAddr1 { get; set; }
        public string ShipAddr2 { get; set; }
        public string ShipCity { get; set; }
        public string ShipState { get; set; }
        public string ShipZip { get; set; }
        public string Items { get; set; }
        public string QtyOrdered { get; set; }
        public string UnitPrice { get; set; }
        public string ReleaseDate { get; set; }
        public string StdFreightAmt { get; set; }
        public string XtraFrtAmt { get; set; }
        public string ReplCertAmt { get; set; }
        public string CouponAmt { get; set; }
        public string GiftCertAmt { get; set; }
        public string RefundChkAmt { get; set; }
        public string AcctgCreditAmt { get; set; }
        public string PostCardAmt { get; set; }
        public string OtherDiscAmt { get; set; }
        public string CustNumber { get; set; }
        public string WebReference { get; set; }
        public string SalesTaxAmt { get; set; }
        public string StatusError { get; set; }
        public string StatusMsg { get; set; }
        public string TaxCode { get; set; }

        public TaxInfo()
        {
            ReleaseDate = String.Empty;
            ReplCertAmt = "0.00";
            RefundChkAmt = "0.00";
            GiftCertAmt = "0.00";
            AcctgCreditAmt = "0.00";
            PostCardAmt = "0.00"; 
            OtherDiscAmt = "0.00";
            StdFreightAmt = "0.00";
            XtraFrtAmt = "0.00";   
        }
    }

    /// <summary>
    /// Summary description for RedBackLibraryTax
    /// </summary>
    public class RedBackLibraryTax
    {
        private const string RedBackAccount = "172.16.15.15:8402";

        public TaxInfo GetTaxInformation(TaxInfo taxInfo)
        {
            RedObject rb = new RedObject();
            rb.Open3(RedBackAccount, "OPM:GetSalesTax");
            ((RedProperty)rb.Property("Title")).Value = taxInfo.Title;
            ((RedProperty)rb.Property("ShipAddr1")).Value = taxInfo.ShipAddr1;
            ((RedProperty)rb.Property("ShipAddr2")).Value = taxInfo.ShipAddr2;
            ((RedProperty)rb.Property("ShipCity")).Value = taxInfo.ShipCity;
            ((RedProperty)rb.Property("ShipState")).Value = taxInfo.ShipState;
            ((RedProperty)rb.Property("ShipZip")).Value = taxInfo.ShipZip;
            ((RedProperty)rb.Property("Items")).Value = taxInfo.Items;
            ((RedProperty)rb.Property("QtyOrdered")).Value = taxInfo.QtyOrdered;
            ((RedProperty)rb.Property("UnitPrice")).Value = taxInfo.UnitPrice;
            ((RedProperty)rb.Property("StdFreightAmt")).Value = taxInfo.StdFreightAmt;
            ((RedProperty)rb.Property("CustNumber")).Value = taxInfo.CustNumber;
            ((RedProperty)rb.Property("CouponAmt")).Value = taxInfo.CouponAmt;
            ((RedProperty)rb.Property("WebReference")).Value = taxInfo.WebReference;
            rb.CallMethod("VtexGetSalesTax");
            taxInfo.ReleaseDate = ((RedProperty)rb.Property("ReleaseDate")).Value;
            taxInfo.SalesTaxAmt = ((RedProperty)rb.Property("SalesTaxAmt")).Value;
            taxInfo.StatusError = ((RedProperty)rb.Property("StatusError")).Value;
            taxInfo.StatusMsg = ((RedProperty)rb.Property("StatusMsg")).Value;
            taxInfo.TaxCode = ((RedProperty)rb.Property("TaxCode")).Value;
            //taxInfo.SalesTaxAmt = "1.6";
            return taxInfo;
        }

    }
}