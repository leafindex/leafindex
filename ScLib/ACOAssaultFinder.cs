using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Text.RegularExpressions;

namespace ScLib
{
    public class ACOAssaultFinder
    {
        private string _ErrorMessage = String.Empty;
        private List<ACOAssaultWard> _wards = new List<ACOAssaultWard>();

        public ACOAssaultFinder( string xmlfilename )
        {
            if (System.IO.File.Exists(xmlfilename) == false)
                _ErrorMessage = "No file " + xmlfilename;
            else
                LoadWards(xmlfilename);
        }

        private void LoadWards(string xmlfilename)
        {
            _wards = new List<ACOAssaultWard>();
            try
            {
                DataSet ds = new DataSet();
                ds.ReadXml(xmlfilename);
                DataTable dt = ds.Tables[ AcoConsts.TableName];
                foreach (DataRow dr in dt.Rows)
                    _wards.Add(new ACOAssaultWard(dr));
            }
            catch (Exception ex)
            {
                _ErrorMessage = ex.Message;
                _wards.Clear();
            }
        }
        public int WardCount { get { return _wards.Count; } }
        public string ErrorMessage { get { return _ErrorMessage; } }

        public ACOAssaultWard[] Find(string borough, string frequency)
        {
            List<ACOAssaultWard> results = new List<ACOAssaultWard>();
            foreach (ACOAssaultWard w in _wards)
                if (w.DistrictName.ToUpper().Contains(borough.Trim().ToUpper()))
                    results.Add(w);
            return Filter(results, frequency).ToArray();
        }

        private List<ACOAssaultWard> Filter(List<ACOAssaultWard> wards, string frequency)
        {
            FrequencyParser fp = new FrequencyParser(frequency);
            if (fp.MyMethod == FrequencyParser.Method.TopBottom)
                return FilterEnd(wards, fp.Count, fp.DoTop);
            if (fp.MyMethod == FrequencyParser.Method.Range)
                return FilterRange(wards, fp.From, fp.To);

            return wards;
        }

        private List<ACOAssaultWard> FilterRange(List<ACOAssaultWard> wards, int from, int to )
        {
            if (to < from)
            {
                int tmp = to;
                to = from;
                from = tmp;
            }
            List<ACOAssaultWard> results = new List<ACOAssaultWard>();
            foreach (ACOAssaultWard w in wards)
                if (w.Total >= from && w.Total <= to)
                    results.Add(w);
            return results;
        }

        //private List<ACOAssaultWard> FilterBottom(List<ACOAssaultWard> wards, Match m)
        //{
        //    return FilterEnd(wards, Count(m), false);
        //}
        //private List<ACOAssaultWard> FilterTop(List<ACOAssaultWard> wards, Match m )
        //{
        //    return FilterEnd( wards, Count(m), true );
        //}
        private List<ACOAssaultWard> FilterEnd(List<ACOAssaultWard> wards, int count, bool largest )
        {
            if (count >= wards.Count)
                return wards;
            List<ACOAssaultWard> results = new List<ACOAssaultWard>();
            int cutoff = CalculateCutoff( wards, count, largest );
            if (largest)
                return FilterRange(wards, cutoff, int.MaxValue);
            else
                return FilterRange(wards, 0, cutoff );
        }

        private int CalculateCutoff(List<ACOAssaultWard> wards, int count, bool largest)
        {
            List<int> values = new List<int>();
            foreach (ACOAssaultWard w in wards)
            {
                if (values.Count == 0 || Beyond( w.Total, values[values.Count - 1], largest ) )
                    values.Add(w.Total);
                else
                {
                    for (int i = 0; i < values.Count; i++)
                    {
                        if( !Beyond(w.Total, values[i], largest ) )
                        {
                            values.Insert(i, w.Total);
                            break;
                        }
                    }
                }
            }
            if (count >= values.Count)
                return values[0];
            else
                return values[values.Count - count];
        }

        private bool Beyond(int x, int y, bool largest)
        {
            if (largest)
                return x >= y;
            else
                return x <= y;
        }
    }
}
