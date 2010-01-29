using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Xml.Linq;

namespace ScLib
{
    public class ACOAssaultWard
    {
        public int MONTHS_HELD = 24;
        string _wardname, _wardcode, _districtcode, _districtname;
        int _total = 0;
        int[] _monthCount;

        public static string ColWard_Name = "Ward_Name";
        public static string ColWard_Code = "Ward_Code";
        public static string ColDistrict_Code = "District_Code";
        public static string ColDistrict_Name = "District_Name";
        public static string ColTotal = "Count_12-2007_to_11-2009";

        public ACOAssaultWard( XElement elt )
        {
            _wardname = elt.Element(AcoConsts.ColWard_Name).Value;
            _wardcode = elt.Element(AcoConsts.ColWard_Code).Value;
            _districtcode = elt.Element(AcoConsts.ColDistrict_Code).Value;
            _districtname = elt.Element(AcoConsts.ColDistrict_Name).Value;
            _total = ACOAssaultsByLinq.MyIntParse(elt.Element(AcoConsts.ColTotal).Value);
            InitMonthCount();
            for (int i = 0; i < MONTHS_HELD; i++)
            {
                string eltname = MakeMonthElementName(i);
                _monthCount[i] = ACOAssaultsByLinq.MyIntParse(elt.Element(eltname).Value);
            }
        }
        public ACOAssaultWard(DataRow dr)
        {
            _wardname = dr[AcoConsts.ColWard_Name].ToString();
            _wardcode = dr[AcoConsts.ColWard_Code].ToString();
            _districtcode = dr[AcoConsts.ColDistrict_Code].ToString();
            _districtname = dr[AcoConsts.ColDistrict_Name].ToString();
            int.TryParse( dr[AcoConsts.ColTotal].ToString(), out _total );
            InitMonthCount();
        }

        public ACOAssaultWard(string wardname, string districtname, int total)
        {
            _wardname = wardname;
            _wardcode = "";
            _districtname = districtname;
            _districtcode = "";
            _total = total;
            InitMonthCount();
        }

        private void InitMonthCount()
        {
            _monthCount = new int[MONTHS_HELD];
            for (int i = 0; i < MONTHS_HELD; i++)
                _monthCount[i] = 0;
        }
        public int GetAssaultsInMonth(int mth)
        {
            if (mth >= 0 && mth < MONTHS_HELD)
                return _monthCount[mth];
            else
                return 0;
        }
        public string MakeMonthElementName(int i)
        {
            if (i == 0)
                return MakeMonthElementName(12, 2007);
            int yr = 2008;
            while( i > 12 )
            {
                yr++;
                i -= 12;
            }
            return MakeMonthElementName( i, yr );
        }
        public string MakeMonthElementName(int mth, int yr)
        {
            return String.Format("Count_{0}-{1}", mth, yr);
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
