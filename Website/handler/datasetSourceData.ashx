<%@ WebHandler Language="C#" Class="datasetSourceData"%>

using System;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

public class datasetSourceData : IHttpHandler 
{
	public void ProcessRequest(HttpContext context)
	{
			int	datasetId;
			if (Int32.TryParse(context.Request["dataset"], out datasetId))
			{
				using (SqlCommand cmd = new SqlCommand("pr_datasetsSourceDataRead", new SqlConnection(ConfigurationManager.ConnectionStrings["datasets"].ConnectionString)))
				{
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.AddWithValue("@datasetId", datasetId);
					cmd.Connection.Open();
					using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
					{
						if (reader.Read())
						{
							context.Response.CacheControl = "public";
							context.Response.AddHeader("Content-Disposition", "attachment;filename=\"" + reader[0] + "\"");
							context.Response.ContentType = "application/octet-stream";
							context.Response.BinaryWrite((byte[])reader[1]);
						}
						reader.Close();
					}					
				}				
			}		
	}

	public bool IsReusable
	{
		get
		{
			return false;
		}
	}
}