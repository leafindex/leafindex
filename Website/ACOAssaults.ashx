<%@ WebHandler Language="C#" Class="ACOAssaults" %>

using System;
using System.Web;
using System.Configuration;
using ScLib;

public class ACOAssaults : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        string grail = context.Request["borough"].Trim() + "/" + context.Request["frequency"].Trim();

        ACOAssaultFinder finder = new ACOAssaultFinder( ConfigurationManager.AppSettings["ACOAssaultsXmlFile"] );
        ACOAssaultWard[] results = finder.Find( context.Request["borough"], context.Request["frequency"] );
        
        bool show_district = ( ACOAssaultWard.CountDistricts( results ) > 1 );

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