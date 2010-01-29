using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using ScLib;
using System.Text;

namespace Website
{
    public partial class ACOMap : System.Web.UI.Page
    {
        private string _myscript = "";        
        protected void Page_Load(object sender, EventArgs e)
        {
            string xmlPath = Request.PhysicalApplicationPath + ConfigurationManager.AppSettings["ACOAssaultsXmlFile"];
            List<ACOAssaultBorough> boroughs = ACOAssaultBoroughs.ReadInclMonths(xmlPath);
            if (boroughs.Count > 0)
            {
                StringBuilder b = new StringBuilder();
                int max_assaults = 0;
                b.AppendLine( "<p>" + boroughs.Count + " boroughs</p>" );
                b.AppendLine("<script type='text/javascript'>");
                b.AppendLine("function LoadData() {");
                b.AppendLine("var i = 0;");
                b.AppendLine("_data = new Array();");
                foreach (var borough in boroughs)
                {
                    b.AppendLine("_data[i++] = new Array( '" + borough.Borough + "', " + borough.MonthlyFiguresAsCsv() + " );" );
                    if (borough.MaxMonthlyFigure() > max_assaults)
                        max_assaults = borough.MaxMonthlyFigure();
                }
                b.AppendLine("}");
                b.AppendLine("var _max_assaults = " + max_assaults + ";");
                b.AppendLine("</script>");

                _myscript = b.ToString();
                //StringBuilder b = new StringBuilder("<table id=\"boroughTable\" cellspacing=\"0\">");
                //b.Append("<thead><tr>");
                //b.Append("<th>Borough</th>");
                //b.Append("<th class=\"number\">Number of Assaults</th>");
                //b.Append("<th class=\"number\">Number of Wards in Borough</th>");
                //b.Append("</tr></thead><tbody>");
                //foreach (var borough in boroughs)
                //{
                //    b.Append("<tr><td>" + borough.Borough + "</td><td>" + borough.Assaults + "</td><td>" + borough.Wards + "</td></tr>");
                //}
                //b.Append("</tbody></table>");
                //_boroughs = b.ToString();
            }
            else
            {
                _myscript = "<p>No boroughs</p>";
            }

        }
        protected string MyScript { get { return _myscript; } }

    }
}
