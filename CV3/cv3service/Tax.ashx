<%@ WebHandler Language="C#" Class="Tax" Debug="true" %>



	using System.Text;
	using System.IO;
	using System.Web;
	using System.Runtime.Serialization.Json;
	using System.Collections.Generic;
	using System.Data;
	using System;
	using System.Data.SqlClient;
	 
	  
 
    public class Tax : IHttpHandler
    {
  
      

        public HttpContext context;
        public HttpRequest request;
        public HttpResponse response;

 	public string brand = String.Empty; 
	public string shipState = String.Empty;
	public string shipPostCd = String.Empty;
	public string items = String.Empty;
	public string qtyOrdered = String.Empty;
	public string inMerchAmt = String.Empty;
	public string inShipAmt = String.Empty;

 	public void handleRequest()
        {
            writeJson(TaxInformation); 
        }

        public void ProcessRequest(HttpContext _context)
        {
            context = _context;
            request = _context.Request;
            response = _context.Response;
            context.Response.ContentType = "application/json";
            context.Response.ContentEncoding = Encoding.UTF8;
       
      
            if (context.Request["brand"] != null)
              brand = context.Request["brand"]; 
            if (context.Request["ship_state"] != null)
              shipState = context.Request["ship_state"];   
            if (context.Request["ship_zip"] != null)
              shipPostCd = context.Request["ship_zip"];               
            if (context.Request["skus"] != null)
              items = context.Request["skus"]; 
            if (context.Request["qtys"] != null)
              qtyOrdered = context.Request["qtys"];   
	    if (context.Request["total"] != null)
              inMerchAmt = context.Request["total"]; 
            if (context.Request["shipping"] != null)
              inShipAmt = context.Request["shipping"];    
                       
            handleRequest();

        }

        public void writeJson(object _object)
        {
             
	    MemoryStream stream1 = new MemoryStream();
            DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(float));
            jsonSerializer.WriteObject(stream1, _object);
            stream1.Position = 0;
            StreamReader sr = new StreamReader(stream1);
            writeRaw(sr.ReadToEnd());
        }

        public void writeRaw(string text)
        {
            context.Response.Write(text);

        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public float TaxInformation
        {
            get
            {
                return GetTaxInfo();
            }
        }

	        public class TaxInfo
            {
                public string CustNumber;
                public string Brand;
                public string WebReference;
                public string ShipState;
                public string ShipPostCd;
                public string Items;
                public string QtyOrdered;
                public string UnitPrice;
                public string InMerchAmt;
                public string InTotDiscAmt;
                public string InClubDiscAmt;
                public string InShipAmt;
                public string InTaxAmt;
                public string InXshipAmt;
                public string InOtherAmt;
                public string InTotalAmt; 
			
                public string NewTaxCode;
                public string NewTaxRate;
                public string NewTaxAmt;
                public string NewTotalAmt;
                public string OrderErrCode; 
                public string OrderErrMsg; 
            }
        
        public float GetTaxInfo()
        {

            
            RedBackLibraryTest.TaxInfo taxInform = new RedBackLibraryTest.TaxInfo();  
            
            taxInform.Brand = "2"; 
            taxInform.ShipState = shipState;
            taxInform.ShipPostCd = shipPostCd;
            taxInform.Items = items;
            taxInform.QtyOrdered = qtyOrdered; 
	    taxInform.InMerchAmt = inMerchAmt; 
	    taxInform.InShipAmt = inShipAmt; 
            
            RedBackLibraryTest rb = new RedBackLibraryTest(); 
	    RedBackLibraryTest.TaxInfo rbtaxInfo = rb.GetTaxInformation(taxInform); 
	    return  float.Parse(taxInform.NewTaxAmt)/100; 
        }

    }





