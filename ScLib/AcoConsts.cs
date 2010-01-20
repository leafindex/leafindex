using System;
using System.Collections.Generic;
using System.Text;

namespace ScLib
{
    public class AcoConsts
    {
        public static string TableName = "ROW";
        public static string ColWard_Name = "Ward_Name";
        public static string ColWard_Code = "Ward_Code";
        public static string ColDistrict_Code = "District_Code";
        public static string ColDistrict_Name = "District_Name";
        public static string ColTotal = "Count_12-2007_to_11-2009";

        /// <summary>
        /// Range is 12,2007 to 11,2009
        /// </summary>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public string ColMonth(int month, int year)
        {
            return String.Format("Count_{0}-{1}" + month, year);
        }

    }
}
