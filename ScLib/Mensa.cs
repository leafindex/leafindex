using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace ScLib
{
    public class Mensa
    {
        private static string _ConnectionString;
        public static void SetConnectionString(string str)
        {
            _ConnectionString = str;
        }
        private string _ErrorMessage;

        public DataSet LondonArtsEngagement(string arttype)
        {
            string sql = "";
            string year = "2008/09";
            if (arttype == "all")
                sql = MakeSpCallTextArgs("pr_rpt_arts_4_EngagementAll", year);
            else
                sql = MakeSpCallTextArgs("pr_rpt_arts_4_Engagement", arttype, year);
            //string sql = "pr_rpt_arts_4_Engagement '" + arttype + "', '" + year + "'";
            return ExecuteDataSet(sql);
        }
        public DataSet LondonBoroughCrime3OffencesPerBorough( string crime, string year )
        {
            DataTable dt = new DataTable();
            if (crime == "") crime = "Burglary";
            if (year == "") year = "2008/09";

            string sql = "pr_rpt_crime_3_offencesPerBorough '" + crime + "', '" + year + "'";
            return ExecuteDataSet(sql);
        }
        public DataSet LondonBoroughBegging(string year)
        {
            string sql = "pr_rpt_crime_5_Begging '" + year + "'";
            return ExecuteDataSet(sql);
        }
        public DataSet LondonSuicideData(string fact, string year)
        {
            return ExecuteDataSet(MakeSpCallTextArgs("pr_rpt_death_7_suicide", fact, year));
        }
        public DataSet LondonTourismData(string fact, string year)
        {
            return ExecuteDataSet(MakeSpCallTextArgs("pr_rpt_tourism_6_trips", fact, year ));
        }
        public DataSet LondonWasteData(string fact, string year)
        {
            return ExecuteDataSet(MakeSpCallTextArgs("pr_rpt_waste_8_reuse", fact, year));
        }

        private string MakeSpCallTextArgs( params string [] strs )
        {
            StringBuilder args = new StringBuilder();
            for (int i = 1; i < strs.Length; i++)
            {
                if (args.Length > 0)
                    args.Append(",");
                args.Append("'" + strs[i] + "'");
            }
            return strs[0] + " " + args.ToString();
        }



        public DataSet ExecuteDataSet(string sql)
        {
            SqlConnection conn = null;
            try
            {
                DataSet result = new DataSet();
                using (conn = new SqlConnection( _ConnectionString ))
                {
                    conn.Open();
                    using (SqlDataAdapter da = new SqlDataAdapter(sql, conn))
                    {
                        da.Fill(result);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                _ErrorMessage = ex.Message;
                return null;
            }
            finally
            {
                if (conn != null)
                    conn.Close();
            }
        }
    }
}
