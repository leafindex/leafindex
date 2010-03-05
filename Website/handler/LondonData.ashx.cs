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
            DataTable dt = new DataTable();
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
            else if (String.Compare(data_set_name, "Arts", true) == 0)
                dt = GetArtsData(arg2);
            else if (String.Compare(data_set_name, "Cars", true) == 0)
                dt = GetCarsData();

            if( fmt.ToUpper().StartsWith( "T" ) )
                context.Response.ContentType = "text/plain";
            else
                context.Response.ContentType = "application/json; charset=utf-8";
            context.Response.Write(GetJSONString(dt));
        }

        private DataTable GetCarsData()
        {
            DataTable result = new DataTable();
            result.Columns.Add(new DataColumn("Code", typeof(string)));
            result.Columns.Add(new DataColumn("Name", typeof(string)));
            result.Columns.Add( new DataColumn( "Val0", typeof(int)));
            result.Columns.Add( new DataColumn( "Val1", typeof(int)));
            result.Columns.Add( new DataColumn( "Val2", typeof(int)));
            result.Columns.Add( new DataColumn( "Val3", typeof(int)));
            result.Columns.Add( new DataColumn( "Val4", typeof(int)));

result.Rows.Add( "City of London","00AA",2691,1417,184,29,17 );
result.Rows.Add( "Barking and Dagenham","00AB",25511,30279,9688,1441,354 );
result.Rows.Add( "Barnet","00AC",33925,57039,28680,5648,1652 );
result.Rows.Add( "Bexley","00AD",21217,41958,20986,4160,1130 );
result.Rows.Add( "Brent","00AE",37287,42606,16207,3135,756 );
result.Rows.Add( "Bromley","00AF",28950,57751,31154,6180,1831 );
result.Rows.Add( "Camden","00AG",50946,33084,6280,984,309 );
result.Rows.Add( "Croydon","00AH",41461,63286,27640,5066,1546 );
result.Rows.Add( "Ealing","00AJ",37372,54259,21761,3742,889 );
result.Rows.Add( "Enfield","00AK",31496,50201,22814,4586,1301 );
result.Rows.Add( "Greenwich","00AL",37883,40160,12260,1976,509 );
result.Rows.Add( "Hackney","00AM",48219,31876,5018,689,240 );
result.Rows.Add( "Hammersmith and Fulham","00AN",36630,30461,7032,1058,257 );
result.Rows.Add( "Haringey","00AP",42820,38005,9622,1374,349 );
result.Rows.Add( "Harrow","00AQ",17972,34900,20789,4286,1165 );
result.Rows.Add( "Havering","00AR",21374,42078,22131,4734,1405 );
result.Rows.Add( "Hillingdon","00AS",20972,43116,25690,5225,1640 );
result.Rows.Add( "Hounslow","00AT",24049,38920,16868,3316,841 );
result.Rows.Add( "Islington","00AU",47413,29194,4795,670,209 );
result.Rows.Add( "Kensington and Chelsea","00AW",39870,31041,6633,1184,418 );
result.Rows.Add( "Kingston upon Thames","00AX",14621,29049,14336,2669,751 );
result.Rows.Add( "Lambeth","00AY",60338,46080,10166,1446,417 );
result.Rows.Add( "Lewisham","00AZ",45941,46679,12484,1831,477 );
result.Rows.Add( "Merton","00BA",23775,38143,13803,2517,646 );
result.Rows.Add( "Newham","00BB",44866,37811,7789,1089,266 );
result.Rows.Add( "Redbridge","00BC",24198,43047,20140,3856,1047 );
result.Rows.Add( "Richmond upon Thames","00BD",18047,37723,16871,2811,694 );
result.Rows.Add( "Southwark","00BE",54940,40947,8454,1120,345 );
result.Rows.Add( "Sutton","00BF",17790,35264,18470,3750,1128 );
result.Rows.Add( "Tower Hamlets","00BG",44582,28997,4250,545,156 );
result.Rows.Add( "Waltham Forest","00BH",34975,39562,12512,2177,562 );
result.Rows.Add( "Wandsworth","00BJ",47066,51440,14437,2164,546 );
result.Rows.Add( "Westminster","00BK",51452,32108,6241,1012,359 );

            return result;
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
