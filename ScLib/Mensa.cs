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
            string year = "2008/09";
            string sql = "pr_rpt_arts_4_Engagement '" + arttype + "', '" + year + "'";
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
