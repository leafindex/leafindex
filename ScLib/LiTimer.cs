using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace ScLib
{
    public class LiTimer
    {
        Dictionary<string, LiTimerSubset> _dict;
        LiTimerSubset _current = null;
        public LiTimer( string subset )
        {
            _dict = new Dictionary<string, LiTimerSubset>();
            StartSubset(subset);
        }
        public void StartSubset(string subset)
        {
            if (_current != null)
                _current.Stop();
            if (!_dict.ContainsKey(subset))
                _dict.Add(subset, new LiTimerSubset(subset));
            _current = _dict[subset];
            _current.Start();
        }
        public List<string> GetResults()
        {
            List<string> results = new List<string>();
            foreach (string subset in _dict.Keys)
                results.Add(_dict[subset].ToString());
            return results;
        }
    }
    public class LiTimerSubset
    {
        string _name;
        int _count;
        private Stopwatch _sw;

        public LiTimerSubset(string name)
        {
            _name = name;
            _sw = new Stopwatch();
            _count = 0;
        }

        public void Start()
        {
            _sw.Start();
            _count++;
        }
        public void Stop()
        {
            _sw.Stop();
        }

        public override string ToString()
        {
            return String.Format("{0} : {1} calls, {2} milliseconds", _name, _count, _sw.ElapsedMilliseconds );
        }
    }
}
