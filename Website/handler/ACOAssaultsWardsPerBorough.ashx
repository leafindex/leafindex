<%@ WebHandler Language="C#" Class="ACOAssaultsWardsPerBorough"%>

using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Script.Serialization;
using System.Configuration;
using System.Text;
using ScLib;

public class ACOAssaultsWardsPerBorough : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {

			context.Response.ContentType = "application/json; charset=utf-8";
			string borough = (context.Request["borough"] ?? "").Trim();

			if (borough != String.Empty)
			{
				string xmlPath = context.Request.PhysicalApplicationPath + ConfigurationManager.AppSettings["ACOAssaultsXmlFile"];
				List<ACOAssaultWard1> wards = new ACOAssaultWardsPerBorough(xmlPath, borough).Wards;
				JavaScriptSerializer serializer = new JavaScriptSerializer();
				context.Response.Write(serializer.Serialize(wards));
			}
		}
 
    public bool IsReusable {
        get {
            return false;
        }
    }
}