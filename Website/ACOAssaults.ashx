<%@ WebHandler Language="C#" Class="ACOAssaults"%>

using System;
using System.Text;
using System.Web;
using System.Configuration;
using ScLib;

public class ACOAssaults : IHttpHandler 
{
    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        string borough = (context.Request["borough"] ?? "").Trim();
        string frequency = (context.Request["frequency"] ?? "").Trim();
        string grail = borough + " and " + frequency;
        string path = context.Request.PhysicalApplicationPath;
        //string fullfilename = System.IO.Path.Combine(path, ConfigurationManager.AppSettings["ACOAssaultsXmlFile"]);
        //string fullfilename = ConfigurationManager.AppSettings["ACOAssaultsXmlFile"];
        string fullfilename = @"C:\CSharp\LeafIndex\DataSets\ambulance-all-assaults-ward.xml";

        ACOAssaultsByLinq finder = new ACOAssaultsByLinq(fullfilename);
        ACOAssaultWard[] results = finder.Find(borough, frequency);
        bool show_district = (ACOAssaultWard.CountDistricts(results) > 1);
        
        StringBuilder msg = new StringBuilder();
        msg.Append("<result><grail>" + grail + "</grail><count>" + results.Length + "</count>");
        msg.Append("<error>OK</error>");
        foreach (ACOAssaultWard w in results)
            msg.Append( "<ward><name>" + w.GetFullName(show_district) + "</name><total>" + w.Total + "</total></ward>" );
        
        msg.Append("</result>");
        context.Response.Write(msg.ToString());
    }
   
    //public void ProcessRequest (HttpContext context) {

    //        context.Response.ContentType = "text/plain";
    //        string borough = (context.Request["borough"] ?? "").Trim();
    //        string frequency = (context.Request["frequency"] ?? "").Trim();

    //        string grail = borough + "/" + frequency,
    //            path = context.Request.PhysicalApplicationPath;

    //        ACOAssaultFinder finder = new ACOAssaultFinder(path + ConfigurationManager.AppSettings["ACOAssaultsXmlFile"]);
    //  ACOAssaultWard[] results = finder.Find(borough, frequency);
        
    //  bool show_district = (ACOAssaultWard.CountDistricts(results) > 1);

    //  int count = results.Length;
    //  string  msg = "<result><grail>" + grail + "</grail><count>" + count + "</count>";
    //  msg += "<error>" + finder.ErrorMessage + "</error>";
    //  foreach (ACOAssaultWard w in results)
    //      msg += "<ward><name>" + w.GetFullName(show_district) + "</name><total>" + w.Total + "</total></ward>";
    //  msg += "</result>";
      
    //  context.Response.Write(msg);
    //}
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}