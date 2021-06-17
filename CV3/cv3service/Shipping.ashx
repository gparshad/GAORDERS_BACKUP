<%@ WebHandler Language="C#" Class="Shipping" Debug="true" %>



	using System.Text;
	using System.IO;
	using System.Web;
	using System.Runtime.Serialization.Json;
	using System.Collections.Generic;
	using System.Data;
	using System;
	using System.Data.SqlClient;
	 
	  
 
    public class Shipping : IHttpHandler
    {
  
      

        public HttpContext context;
        public HttpRequest request;
        public HttpResponse response;

 	public string brand = String.Empty; 
	public string askus = String.Empty;
	public string sgrps = String.Empty;
	public string sstates = String.Empty;
	public string smeths = String.Empty;
	public string sprices = String.Empty; 
  

        public void ProcessRequest(HttpContext _context)
        {
            context = _context;
            request = _context.Request;
            response = _context.Response;
            context.Response.ContentType = "application/json";
            context.Response.ContentEncoding = Encoding.UTF8;
       
      
           // if (context.Request["brand"] != null)
              //brand = context.Request["brand"]; 
           // if (context.Request["askus"] != null)
             //  askus= context.Request["askus"];   
            //if (context.Request["sgrps"] != null)
            //   sgrps = context.Request["sgrps"];               
            //if (context.Request["skus"] != null)
            //  sstates = context.Request["sstates"];    
	    //if (context.Request["smeths"] != null)
            //  smeths = context.Request["smeths"]; 
           // if (context.Request["sprices"] != null)
              //sprices = context.Request["sprices"];    
                       
            handleRequest();

        }

	public bool IsReusable
        {
            get
            {
                return false;
            }
        }
        
 	public void handleRequest()
        {
	    
	    response.Write(GetShipInfo()); 
        }
 
	 public class ShipInfo
         {  
                public string Brand;
                public string Skus;
                public string ShipGroups;
                public string ShipStates;
		public string ShipMethods;
		public string ShipPrice;
                public string Message; 
         }
        
        public string GetShipInfo()
        {
          
	    
            ShipInfo shipInfo = new ShipInfo();  
            
            shipInfo.Brand = "2"; 
            shipInfo.Skus = askus;
            shipInfo.ShipGroups = sgrps;
            shipInfo.ShipStates = sstates;  
	    shipInfo.ShipMethods = smeths; 
	    shipInfo.ShipPrice = sprices; 
	    shipInfo.Message = "Please note: There is a change in shipping";
	    //Uri myUri = new Uri(request.URL); 
            //askus = HttpUtility.ParseQueryString(myUri.Query).Get("askus");
          
	    return  "smeths=,DEFAULT\ntadd=1000\nsmsg=shipping message for display " +   context.Request["askus"] +":" + context.Request["smeths"] + ":URL:" +  HttpContext.Current.Request.Url;
	      
        }

    }





