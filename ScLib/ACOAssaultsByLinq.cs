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

            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(@"c:\temp\xx.txt", true))
            {
                sw.WriteLine(results.Count + " returned by " + borough + "/" + frequency);
                sw.Close();
            }

            return results.ToArray();
        }

        private List<ACOAssaultWard> GetWardsTopBottom(string borough, int count, bool do_top)
        {
            IEnumerable<myWard> wards = GetWardsInBorough(borough);
            if (wards.Count() > count)
            {
                if (!do_top)
                    wards = wards.Reverse();
                int cutoff = wards.Take(count).Last().incidents;
                if (do_top)
                    wards = wards.TakeWhile(w => w.incidents >= cutoff);
                else
                    wards = wards.TakeWhile(w => w.incidents <= cutoff).Reverse();
            }
            List<ACOAssaultWard> results = new List<ACOAssaultWard>();
            foreach (var ward in wards)
                results.Add(new ACOAssaultWard(ward.ward, ward.borough, ward.incidents));
            return results;
        }

        private List<ACOAssaultWard> GetWardsNumericRange(string borough, int from, int to)
        {
            IEnumerable<myWard> wards = GetWardsInBorough(borough);
            List<ACOAssaultWard> results = new List<ACOAssaultWard>();
            foreach (var ward in wards)
            {
                if( ward.incidents >= from && ward.incidents <= to )
                    results.Add(new ACOAssaultWard(ward.ward, ward.borough, ward.incidents));
            }
            return results;
        }

        public class myWard // : Object
        {
            public string ward;
            public string borough;
            public int incidents;
        }

        private IEnumerable<myWard> GetWardsInBorough(string borough)
        {
            XDocument xdoc = XDocument.Load(_xmlfilename);
            return from w in xdoc.Descendants("ROW")
                   where w.Element("District_Name").Value.ToUpper().Contains(borough)
                   orderby MyIntParse(w.Element("Count_12-2007_to_11-2009").Value) descending
                   select new myWard()
                   {
                       ward = w.Element("Ward_Name").Value,
                       borough = w.Element("District_Name").Value,
                       incidents = MyIntParse(w.Element("Count_12-2007_to_11-2009").Value)
                   };
        }


        //private object GetWardsInBorough(string borough)
        //{
        //    XDocument xdoc = XDocument.Load(_xmlfilename);
        //    var wards = from w in xdoc.Descendants("ROW")
        //                where w.Element("District_Name").Value.ToUpper().Contains(borough)
        //                orderby MyIntParse(w.Element("Count_12-2007_to_11-2009").Value) descending
        //                select new
        //                {
        //                    ward = w.Element("Ward_Name").Value,
        //                    borough = w.Element("District_Name").Value,
        //                    incidents = MyIntParse(w.Element("Count_12-2007_to_11-2009").Value)
        //                };

        //    return wards;
        //}

        private int MyIntParse(string str)
        {
            str = str.Trim();
            if (str == "")
                return 0;
            else
                return Convert.ToInt32(str);
        }
    }
}
