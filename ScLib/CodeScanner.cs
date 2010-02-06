using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace ScLib
{
    public class CodeScanner
    {
        private string _rootdirectory;
        public CodeScanner( string rootdirectory )
        {
            _rootdirectory = rootdirectory;
        }
        public string RootDirectory { get { return _rootdirectory; } }

        public List<CodeScannerHtmlCssAspx> MyFiles = new List<CodeScannerHtmlCssAspx>();
        public int Inline
        {
            get
            {
                int result = 0;
                foreach (CodeScannerHtmlCssAspx f in MyFiles)
                    result += f.Inline;
                return result;
            }
        }
        public int Head
        {
            get
            {
                int result = 0;
                foreach (CodeScannerHtmlCssAspx f in MyFiles)
                    result += f.Head;
                return result;
            }
        }
        public decimal Percentage { get { return CodeScannerHtmlCssAspx.CalcPercentage(Inline, Head); } }
        public int DoHtmlCssScan(params string [] relative_file_specs )
        {
            int result = 0;
            foreach( string rfs in relative_file_specs )
            {
                foreach (string filename in Directory.GetFiles( RootDirectory, rfs))
                {
                    result++;
                    CodeScannerHtmlCssAspx f = new CodeScannerHtmlCssAspx(filename);
                    if (MyFiles.Count == 0 || f.Total <= MyFiles[MyFiles.Count - 1].Total )
                        MyFiles.Add(f);
                    else
                    {
                        for (int i = 0; i < MyFiles.Count; i++)
                        {
                            if (f.Total > MyFiles[i].Total)
                            {
                                MyFiles.Insert(i, f);
                                break;
                            }
                        }
                    }
                }
            }
            return result;
        }
    }

    public class CodeScannerHtmlCssAspx
    {
        Regex r_style_start = new Regex(@"\<\s*style");
        Regex r_style_end = new Regex(@"\<\/\s*style>" );
        Regex r_body_start = new Regex(@"\<\s*body");
        Regex r_body_end = new Regex(@"\<\/\s*body>");

        private int _inline, _head;
        private string _Filename;
        private enum FilePart { Unknown, InStyle, InBody }
        public CodeScannerHtmlCssAspx( string filename )
        {
            _inline = _head = 0;
            FileInfo fi = new FileInfo(filename);
            _Filename = fi.Name;
            try
            {
                FilePart fp = FilePart.Unknown;
                using (StreamReader sr = new StreamReader(filename))
                {
                    while (sr.EndOfStream == false)
                    {
                        string str = sr.ReadLine().ToLower();
                        if (r_style_start.IsMatch(str))
                            fp = FilePart.InStyle;
                        if (r_style_end.IsMatch(str))
                            fp = FilePart.Unknown;
                        if (r_body_start.IsMatch(str))
                            fp = FilePart.InBody;
                        if (r_body_end.IsMatch(str))
                            fp = FilePart.Unknown;

                        if (fp == FilePart.InStyle)
                            _head += CountInStrings(str, "{");
                        if (fp == FilePart.InBody)
                            _inline += CountInStrings(str, @"\s+style=");
                    }
                    sr.Close();
                }
            }
            catch
            {
                _inline = _head = 0;
            }
        }

        public static int CountInStrings(string str, string pattern)
        {
            Regex r = new Regex(pattern);
            return r.Matches(str).Count;
        }
        public string Filename { get { return _Filename; } }
        public int Inline { get { return _inline; } }
        public int Head { get { return _head; } }
        public int Total { get { return _head + _inline; } }
        public decimal Percentage { get { return CalcPercentage( _inline, _head ); } }
        public static decimal CalcPercentage( int i, int h )
        {
                if (i + h == 0)
                    return 0m;
                return (h * 100m) / (i + h);
        }
    }
}
