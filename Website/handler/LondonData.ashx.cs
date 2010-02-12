using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using ScLib;
using System.Configuration;
using System.Text;
using System.Data;

namespace Website.handler
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class LondonData : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            string data_set_name = context.Request["arg1"] ?? "";
            string arg2 = context.Request["arg2"] ?? "";
            string arg3 = context.Request["arg3"] ?? "";

            //DataTable dt;
            //if (data_set_name == "Crimes")
            //    dt = GetCrimeData();

            Mensa.SetConnectionString(ConfigurationManager.ConnectionStrings["datasets"].ToString());
            DataTable dt = GetArtsData();

            context.Response.ContentType = "application/json; charset=utf-8";
            context.Response.Write(GetJSONString(dt));
        }

        private DataTable GetArtsData()
        {

            Mensa m = new Mensa();
            return m.LondonArtsEngagement("Public Library").Tables[0];
        }

        // http://weblogs.asp.net/navaidakhtar/archive/2008/07/08/converting-data-table-dataset-into-json-string.aspx

        public static string GetJSONString(DataTable Dt)
        {
            string[] StrDc = new string[Dt.Columns.Count];
            string HeadStr = string.Empty;

            for (int i = 0; i < Dt.Columns.Count; i++)
            {

                StrDc[i] = Dt.Columns[i].Caption;

                HeadStr += "\"" + StrDc[i] + "\" : \"" + StrDc[i] + i.ToString() + "¾" + "\",";
            }

            HeadStr = HeadStr.Substring(0, HeadStr.Length - 1);

            StringBuilder Sb = new StringBuilder();
            //Sb.Append("{\"" + Dt.TableName + "\" : [");
            Sb.Append("[");

            for (int i = 0; i < Dt.Rows.Count; i++)
            {

                string TempStr = HeadStr;
                Sb.Append("{");

                for (int j = 0; j < Dt.Columns.Count; j++)
                {

                    TempStr = TempStr.Replace(Dt.Columns[j] + j.ToString() + "¾", Dt.Rows[i][j].ToString());
                }

                Sb.Append(TempStr + "},");
            }

            Sb = new StringBuilder(Sb.ToString().Substring(0, Sb.ToString().Length - 1));
            Sb.Append("]");

            return Sb.ToString();
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
