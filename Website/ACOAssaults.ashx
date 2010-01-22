<%@ WebHandler Language="C#" Class="ACOAssaults"%>

using System;
using System.Web;
using System.Configuration;
using ScLib;

public class ACOAssaults : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {

			context.Response.ContentType = "text/plain";
			string borough = (context.Request["borough"] ?? "").Trim();
			if(borough.Length == 0){
				return;
			}
			string frequency = (context.Request["frequency"]??"").Trim();
			if (frequency.Length == 0)
			{
				return;
			}

			string grail = borough + "/" + frequency,
				path = context.Request.PhysicalApplicationPath;

			ACOAssaultFinder finder = new ACOAssaultFinder(path + ConfigurationManager.AppSettings["ACOAssaultsXmlFile"]);
      ACOAssaultWard[] results = finder.Find(borough, frequency);
        
      bool show_district = (ACOAssaultWard.CountDistricts(results) > 1);

      int count = results.Length;
      string  msg = "<result><grail>" + grail + "</grail><count>" + count + "</count>";
      msg += "<error>" + finder.ErrorMessage + "</error>";
      foreach (ACOAssaultWard w in results)
          msg += "<ward><name>" + w.GetFullName(show_district) + "</name><total>" + w.Total + "</total></ward>";
      msg += "</result>";
      
      context.Response.Write(msg);
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}