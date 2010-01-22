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
				StringBuilder b = new StringBuilder();
				foreach (var borough in boroughs)
				{
					b.Append("<tr><td>" + borough.BoroughName + "</td><td>" + borough.AssaultCount + "</td><td>" + borough.WardCount + "</td></tr>");
				}
				_boroughs = b.ToString();
			} else {
				_boroughs = "<tr><td colspan=3>No boroughs</td></tr>";
			}
		}
		protected string BoroughList { get { return _boroughs; } }

	}
}
