<%@ WebHandler Language="C#" Class="ACOAssaultsMonthsPerBorough" Debug="true"%>

using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Script.Serialization;
using System.Configuration;
using System.Text;
using ScLib;

public class ACOAssaultsMonthsPerBorough : IHttpHandler
{
    
    public void ProcessRequest (HttpContext context) {

			context.Response.ContentType = "application/json; charset=utf-8";
			string borough = (context.Request["borough"] ?? "").Trim();

			if (borough != String.Empty)
			{
				string xmlPath = context.Request.PhysicalApplicationPath + ConfigurationManager.AppSettings["ACOAssaultsXmlFile"];
				List<ACOAssaultMonth> months = new ACOAssaultMonthsPerBorough(xmlPath, borough).Months;
				JavaScriptSerializer serializer = new JavaScriptSerializer();
				context.Response.Write(serializer.Serialize(months));
			}
		}
 
    public bool IsReusable {
        get {
            return false;
        }
    }
}