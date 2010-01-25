using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ScLib
{
    public class FrequencyParser
    {
        public static Regex r_top = new Regex(@"top\D*(?<count>\d+)");
        public static Regex r_bottom = new Regex(@"bottom\D*(?<count>\d+)");
        public static Regex r_plusminus = new Regex(@"(?<from>\d+).*\+.*\-\D*(?<to>\d+)");
        public static Regex r_range = new Regex(@"(?<from>\d+)\D+(?<to>\d+)");
        public static Regex r_over = new Regex(@"(?<from>\d+).*\+");
        public static Regex r_exact = new Regex(@"(?<from>\d+)");

        public enum Method { Range, TopBottom };

        private Method _method;
        public Method MyMethod { get { return _method; } }
        
        private bool _do_top = false;
        private int _count, _from, _to;
        public bool DoTop { get { return _do_top; } }
        public int Count { get { return _count; } }
        public int From { get { return _from; } }
        public int To { get { return _to; } }

        public FrequencyParser( string frequency )
	    {
            DoParse( frequency.ToLower() );
        }

        private bool DoParse( string frequency )
        {
            Match m;
            m = r_top.Match(frequency);
            if (m.Success)
                return DoTopBottom(true, GetCount(m));

            m = r_bottom.Match(frequency);
            if (m.Success)
                return DoTopBottom(false, GetCount(m));

            m = r_plusminus.Match(frequency);
            if (m.Success)
                return DoRange(GetFrom(m) - GetTo(m), GetFrom(m) + GetTo(m));

            m = r_range.Match(frequency);
            if (m.Success)
                return DoRange(GetFrom(m), GetTo(m));

            m = r_over.Match(frequency);
            if (m.Success)
                return DoRange(GetFrom(m), int.MaxValue);

            m = r_exact.Match(frequency);
            if (m.Success)
                return DoRange(GetFrom(m), GetFrom(m));

            return DoRange(0, int.MaxValue);
	    }

        private bool DoTopBottom( bool do_top, int count )
        {
            _method = Method.TopBottom;
            _do_top = do_top;
            _count = count;
            return true;
        }
        private bool DoRange(int from, int to)
        {
            _method = Method.Range;
            _from = from;
            _to = to;
            return true;
        }

        public static int GetFrom(Match m)
        {
            return Convert.ToInt32(m.Groups["from"].Value);
        }
        public static int GetTo(Match m)
        {
            return Convert.ToInt32(m.Groups["to"].Value);
        }
        public static int GetCount(Match m)
        {
            return Convert.ToInt32(m.Groups["count"].Value);
        }

    }
}
