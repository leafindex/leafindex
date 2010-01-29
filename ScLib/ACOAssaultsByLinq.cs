using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace ScLib
{
    public class ACOAssaultsByLinq
    {
        private string _xmlfilename;
        public ACOAssaultsByLinq(string xmlfilename)
        {
            _xmlfilename = xmlfilename;
        }

        public ACOAssaultWard[] Find(string borough, string frequency)
        {
            List<ACOAssaultWard> results;
            borough = borough.Trim().ToUpper();
            FrequencyParser fp = new FrequencyParser(frequency);
            if (fp.MyMethod == FrequencyParser.Method.Range)
                results = GetWardsNumericRange(borough, fp.From, fp.To);
            else
                results = GetWardsTopBottom(borough, fp.Count, fp.DoTop);
            return results.ToArray();
        }

        private List<ACOAssaultWard> GetWardsTopBottom(string borough, int count, bool do_top)
        {
            IEnumerable<ACOAssaultWard> wards = GetWardsInBorough(borough);
            if (wards.Count() > count)
            {
                if (!do_top)
                    wards = wards.Reverse();
                int cutoff = wards.Take(count).Last().Total;
                if (do_top)
                    wards = wards.TakeWhile(w => w.Total >= cutoff);
                else
                    wards = wards.TakeWhile(w => w.Total <= cutoff).Reverse();
            }
            return wards.ToList();
        }

        private List<ACOAssaultWard> GetWardsNumericRange(string borough, int from, int to)
        {
            IEnumerable<ACOAssaultWard> wards = GetWardsInBorough(borough);
            List<ACOAssaultWard> results = new List<ACOAssaultWard>();
            foreach (var ward in wards)
            {
                if( ward.Total >= from && ward.Total <= to )
                    results.Add( ward );
            }
            return results;
        }

        private IEnumerable<ACOAssaultWard> GetWardsInBorough(string borough)
        {
            XDocument xdoc = XDocument.Load(_xmlfilename);
            return from w in xdoc.Descendants("ROW")
                   where w.Element("District_Name").Value.ToUpper().Contains(borough)
                   orderby MyIntParse(w.Element("Count_12-2007_to_11-2009").Value) descending
                   select new ACOAssaultWard( w.Element("Ward_Name").Value, 
                       w.Element("District_Name").Value, 
                       MyIntParse(w.Element("Count_12-2007_to_11-2009").Value)
                    );
        }

        public static int MyIntParse(string str)
        {
            str = str.Trim();
            if (str == "")
                return 0;
            else
                return Convert.ToInt32(str);
        }
    }
}
