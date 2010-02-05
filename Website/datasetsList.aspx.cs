using System;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Website
{
	public partial class datasetsList : System.Web.UI.Page
	{
		private string _datasets = "";

		protected void Page_Load(object sender, EventArgs e)
		{
			SqlCommand cmd = new SqlCommand("pr_datasetsList", new SqlConnection(ConfigurationManager.ConnectionStrings["datasets"].ConnectionString));
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Connection.Open();
			SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
			if (reader.HasRows)
			{
				StringBuilder b = new StringBuilder("<table id=\"datasetsTable\" cellspacing=\"4\">");
				b.Append("<thead><tr>");
				b.Append("<th>Title</th>"); //0
				b.Append("<th>Description</th>"); //1
				b.Append("<th>Category 1</th>"); //2
				b.Append("<th>Category 2</th>"); //3
				b.Append("<th>Information URL</th>"); //4
				b.Append("<th>Data URL</th>"); //5
				b.Append("<th>Source Data Download</th>"); //6
				b.Append("</tr></thead><tbody>");
				string temp;
				while (reader.Read())
				{
					b.Append("<tr>");
					for (int i = 0; i < 7; i++)
					{
						temp = (reader[i] ?? "").ToString();
						if(i < 4){
							b.Append("<td>" + (temp == "" ? "&nbsp;" : Server.HtmlEncode(temp)) + "</td>");
						}
						else {
							if(temp == ""){
								b.Append("<td>&nbsp;</td>");
							}
							else {
								temp = Server.HtmlEncode(temp);
								if(i < 6) {
									b.Append("<td><a href=\"" + temp + "\">" + shortUrl(temp, 20) + "</a></td>");
								}
								else { // i = 6
									b.Append("<td><a href=\"/handler/datasetSourceData.ashx?dataset=" + reader[7] + "\">" + temp + "</a></td>");
								}
							}
						}
					}
					b.Append("</tr>");
				}
				b.Append("</tbody></table>");
				_datasets = b.ToString();
			}
			else
			{
				_datasets = "<p>No datasets</p>";
			}
		}
		protected string Datasets { get { return _datasets; } }

		private string shortUrl(string url)
		{
			return shortUrl(url, 30);
		}
		private string shortUrl(string url, int maxLen)
		{
			const int minSplit = 4;
			const string dots = "...";
			int dotLen = dots.Length;
			string[] prefixes = new string[] {"http://", "https://", "www."};
			int i;

			if (url.Length <= maxLen) {
				return url;
			}
			for (i = 0; i < prefixes.Length; i++)
			{
				if (url.StartsWith(prefixes[i], StringComparison.CurrentCultureIgnoreCase))
				{
					url = url.Substring(prefixes[i].Length);
					if (url.Length <= maxLen)
					{
						return url;
					}
				}
			}
			i = url.IndexOf("/", url.IndexOf("//") + 1);
			if (i == -1){
				i = url.IndexOf("?");
				if (i == -1){
					i = url.Length - 1;
				}
			}
			if (i > maxLen - dotLen - minSplit)
			{
				string right = url.Substring(((maxLen - 1) / 2) - dotLen);
				return url.Substring(0, ((maxLen - 1) / 2) - dotLen) + dots
					+ (right.Length <= maxLen - ((maxLen - 1) / 2) ? right :
					right.Substring(right.Length - (maxLen - ((maxLen - 1) / 2))));
			}
			else
			{
				return url.Substring(0, i) + dots + url.Substring(url.Length - (maxLen - dotLen - i)); ;
			}
		}


	}
}