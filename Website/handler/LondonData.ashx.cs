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
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class LondonData : IHttpHandler
    {
        private string HELP_MESSAGE = @"
            arg1 = Arts, arg2 = Public Library etc
            arg1 = Begging, arg2 = 2009 etc
            arg1 = Crimes, arg2 = Burglary etc, arg3 = 2008/09 etc";

        public void ProcessRequest(HttpContext context)
        {
            string data_set_name = context.Request["arg1"] ?? "";
            string arg2 = context.Request["arg2"] ?? "";
            string arg3 = context.Request["arg3"] ?? "";
            string fmt = context.Request["fmt"] ?? "";

            if (String.Compare(data_set_name, "Help", true) == 0)
            {
                context.Response.ContentType = "text/plain";
                context.Response.Write(HELP_MESSAGE);
                return;
            }

            Mensa.SetConnectionString(ConfigurationManager.ConnectionStrings["datasets"].ToString());
            DataTable dt;
            if (String.Compare(data_set_name, "Crimes", true) == 0)
                dt = GetCrimeData(arg2, arg3);
            else if (String.Compare(data_set_name, "Begging", true) == 0)
                dt = GetBeggingData(arg2);
            else if (String.Compare(data_set_name, "Suicide", true) == 0)
                dt = GetSuicideData(arg2,arg3);
            else if (String.Compare(data_set_name, "Tourism", true) == 0)
                dt = GetTourismData(arg2, arg3);
            else if (String.Compare(data_set_name, "Waste", true) == 0)
                dt = GetWasteData(arg2, arg3);
            else
                dt = GetArtsData(arg2);

            if( fmt.ToUpper().StartsWith( "T" ) )
                context.Response.ContentType = "text/plain";
            else
                context.Response.ContentType = "application/json; charset=utf-8";
            context.Response.Write(GetJSONString(dt));
        }

        private DataTable GetSuicideData(string fact, string year)
        {
            Mensa m = new Mensa();
            return m.LondonSuicideData(fact, year).Tables[0];
        }
        private DataTable GetTourismData(string fact, string year)
        {
            Mensa m = new Mensa();
            return m.LondonTourismData(fact, year).Tables[0];
        }
        private DataTable GetWasteData(string fact, string year)
        {
            Mensa m = new Mensa();
            return m.LondonWasteData(fact, year).Tables[0];
        }


        private DataTable GetBeggingData(string year)
        {
            if (year == "") year = "2009";
            Mensa m = new Mensa();
            return m.LondonBoroughBegging(year).Tables[0];
        }

        private DataTable GetArtsData(string arttype )
        {
            if (arttype == "") arttype = "Public Library";
            Mensa m = new Mensa();
            return m.LondonArtsEngagement(arttype).Tables[0];
        }

        private void PopulateTable(DataTable dt1, string colname1, DataTable dt2, string colname2 )
        {
            foreach (DataRow dr1 in dt1.Rows)
            {
                foreach (DataRow dr2 in dt2.Rows)
                {
                    if (dr1[0].ToString() == dr2[0].ToString())
                    {
                        dr1[colname1] = dr2[colname2];
                        break;
                    }
                }
            }
        }

        private DataTable GetCrimeData( string crime, string year )
        {
            if (crime == "") crime = "Burglary";
            if (year == "") year = "2008/09";

            Mensa m = new Mensa();
            DataTable dt_all = m.LondonBoroughCrime3OffencesPerBorough(crime, year).Tables[0];
            DataTable dt = dt_all.Clone();
            foreach (DataRow dr in dt_all.Select(MakeBoroughSelect()))
            {
                DataRow dr2 = dt.NewRow();
                for (int i = 0; i < dt.Columns.Count; i++)
                    dr2[i] = dr[i];
                dt.Rows.Add(dr2);
            }
            return dt;
        }

        private string MakeBoroughSelect()
        {
            StringBuilder sb = new StringBuilder();
            foreach (string code in DistrictCodes)
            {
                if (sb.Length > 0)
                    sb.Append(" or ");
                sb.Append("[ONS Code] = '" + code + "'");
            }
            return sb.ToString();
        }

        //private string[] Boroughs = new string[] { 
        //        "City of London", "Barking and Dagenham", "Barnet", "Bexley", "Brent", "Bromley", "Camden", "Croydon", 
        //        "Ealing", "Enfield", "Greenwich", "Hackney", "Hammersmith and Fulham", "Haringey", "Harrow", 
        //        "Havering", "Hillingdon", "Hounslow", "Islington", "Kensington and Chelsea", 
        //        "Kingston upon Thames", "Lambeth", "Lewisham", "Merton", "Newham", "Redbridge", 
        //        "Richmond upon Thames", "Southwark", "Sutton", "Tower Hamlets", "Waltham Forest", 
        //        "Wandsworth", "City of Westminster"
        //};

        private string[] DistrictCodes = new string[] {
            "00AA","00AB","00AC","00AD","00AE","00AF","00AG","00AH",
            "00AJ","00AK","00AL","00AM","00AN","00AP","00AQ",
            "00AR","00AS","00AT","00AU","00AW",
            "00AX","00AY","00AZ","00BA","00BB","00BC",
            "00BD","00BE","00BF","00BG","00BH",
            "00BJ","00BK" 
        };


        public static string GetJSONString(DataTable dt)
        {
			string s;
			decimal d;

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("[");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i > 0)
                    sb.Append(", ");
                sb.Append("{ ");

                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string column_name;
                    if (j == 0)
                        column_name = "Name";
                    else if (j == 1)
                        column_name = "Code";
                    else
                    {
                        int figno = j - 2;
                        column_name = "Fig" + figno.ToString().PadLeft(2, '0');
                    }
					if (j > 0)
					{
						sb.Append(", ");
					}
					s = dt.Rows[i][j].ToString();
					s = (j > 1 && decimal.TryParse(s, out d)) ? Convert.ToInt32(d).ToString() : AddQuotes(s);
					sb.Append(String.Format("{0} : {1}", AddQuotes(column_name), s));
                }
                sb.AppendLine(" }");
            }
            sb.Append("]");
            return sb.ToString();
        }

        private static string AddQuotes(string value)
        {
            char dquote = '"';
            return dquote + value + dquote;
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
