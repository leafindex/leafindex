using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using ScLib;

namespace Website
{
	public partial class ACOAssaults : System.Web.UI.Page
	{
		private string _boroughs = "";

		protected void Page_Load(object sender, EventArgs e)
		{
			string xmlPath = Request.PhysicalApplicationPath + ConfigurationManager.AppSettings["ACOAssaultsXmlFile"];
			List<ACOAssaultBorough> boroughs = new ACOAssaultBoroughs(xmlPath).Boroughs;
			if (boroughs.Count > 0)
			{
				StringBuilder b = new StringBuilder("<table id=\"boroughTable\">");
				b.Append("<thead><tr>");
				b.Append("<th>Borough</th>");
				b.Append("<th class=\"number\">Total Number of Assaults</th>");
				b.Append("<th class=\"number\">Number of Wards in Borough</th>");
				b.Append("</tr></thead><tbody>");
				foreach (var borough in boroughs)
				{
					b.Append("<tr><td>" + borough.BoroughName + "</td><td>" + borough.AssaultCount + "</td><td>" + borough.WardCount + "</td></tr>");
				}
				b.Append("</tbody></table>");
				_boroughs = b.ToString();
			} else {
				_boroughs = "<p>No boroughs</p>";
			}
		}
		protected string Boroughs { get { return _boroughs; } }

	}
}
