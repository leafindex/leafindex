using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace ScLib
{
    public class ACOAssaultWard
    {
        string _wardname, _wardcode, _districtcode, _districtname;
        int _total = 0;

        public static string ColWard_Name = "Ward_Name";
        public static string ColWard_Code = "Ward_Code";
        public static string ColDistrict_Code = "District_Code";
        public static string ColDistrict_Name = "District_Name";
        public static string ColTotal = "Count_12-2007_to_11-2009";

        public ACOAssaultWard( DataRow dr )
        {
            _wardname = dr[AcoConsts.ColWard_Name].ToString();
            _wardcode = dr[AcoConsts.ColWard_Code].ToString();
            _districtcode = dr[AcoConsts.ColDistrict_Code].ToString();
            _districtname = dr[AcoConsts.ColDistrict_Name].ToString();
            int.TryParse( dr[AcoConsts.ColTotal].ToString(), out _total );
        }
        public string WardName { get { return _wardname; } }
        public string DistrictName { get { return _districtname; } }
        public int Total { get { return _total; } }
        public string GetFullName( bool show_district )
        {
            if (show_district)
                return WardName + " (" + DistrictName + ")";
            else
                return WardName;
        }

        public static int CountDistricts(ACOAssaultWard[] results)
        {
            List<string> districts = new List<string>();
            foreach (ACOAssaultWard w in results)
                if (!districts.Contains(w.DistrictName))
                    districts.Add(w.DistrictName);
            return districts.Count;
        }
    }
}
