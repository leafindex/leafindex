using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Xunit.Extensions;
using System.Text.RegularExpressions;
using System.IO;

namespace ScLib
{
    public class Tests
    {
        private ACOAssaultFinder _finder = null;
        private ACOAssaultFinder MyFinder
        {
            get
            {
                if (_finder == null)
                    _finder = new ACOAssaultFinder(TestData.XmlFileName);
                return _finder;
            }
        }

        [Fact]
        public void TestDataXmlFileName()
        {
            string xmlfilename = TestData.XmlFileName;
            Console.WriteLine(xmlfilename);
            Assert.True(System.IO.File.Exists(xmlfilename), "No file " + xmlfilename);
        }

        [Fact]
        public void NoSuchFile()
        {
            ACOAssaultFinder f = new ACOAssaultFinder(@"c:\temp\nosuchfile.xml");
            Assert.True( f.ErrorMessage != "" );
        }

        [Fact]
        public void FileExists()
        {
            Assert.True(MyFinder.ErrorMessage == "", MyFinder.ErrorMessage);
        }
        [Fact]
        public void WardsLoaded()
        {
            Assert.True(MyFinder.WardCount > 0);
        }
        [Fact]
        public void WardsInSouthwark()
        {
            ACOAssaultWard[] wards = MyFinder.Find("southw", "all");
            Assert.True(wards.Length > 0);
        }
        [Fact]
        public void WardsInBadBorough()
        {
            ACOAssaultWard[] wards = MyFinder.Find("bad_borough", "all");
            Assert.True(wards.Length == 0);
        }

        [Fact]
        public void WardsInOneBorough()
        {
            ACOAssaultWard[] wards = MyFinder.Find("southw", "all");
            Assert.True(ACOAssaultWard.CountDistricts(wards) == 1);
        }
        [Fact]
        public void WardsInManyBorough()
        {
            ACOAssaultWard[] wards = MyFinder.Find("ham", "all");
            Assert.True(ACOAssaultWard.CountDistricts(wards) > 1);
        }
        [Fact]
        public void Top3()
        {
            ACOAssaultWard[] wards = MyFinder.Find("southw", "top 3");
            Assert.True(wards.Length == 3);
        }
        [Fact]
        public void Bottom3()
        {
            ACOAssaultWard[] top_wards = MyFinder.Find("southw", "top 3");
            ACOAssaultWard[] bottom_wards = MyFinder.Find("southw", "bottom 3");
            foreach (ACOAssaultWard w in top_wards)
                foreach (ACOAssaultWard w2 in bottom_wards)
                    Assert.False(w.WardName == w2.WardName, w.WardName + " in both");
        }

        [Fact]
        public void NumericRange()
        {
            ACOAssaultWard[] wards = MyFinder.Find("southw", "200 300");
            Assert.True(wards.Length > 0);
            foreach (ACOAssaultWard w in wards)
                Assert.True(w.Total >= 200 && w.Total <= 300);
        }
        [Fact]
        public void Over300()
        {
            ACOAssaultWard[] wards = MyFinder.Find("southw", "300 +");
            Assert.True(wards.Length > 0);
            foreach (ACOAssaultWard w in wards)
                Assert.True(w.Total >= 300);
        }
        [Fact]
        public void Exact326()
        {
            ACOAssaultWard[] wards = MyFinder.Find("southw", " 326 ");
            Assert.True(wards.Length > 0);
            foreach (ACOAssaultWard w in wards)
                Assert.True(w.Total == 326 );            
        }
        [Fact]
        public void PlusMinus()
        {
            ACOAssaultWard[] wards = MyFinder.Find("southw", "330 +- 30");
            Assert.True(wards.Length > 0);
            foreach (ACOAssaultWard w in wards)
                Assert.True(w.Total >= 300 && w.Total <= 360);
        }

        [Fact]
        public void RegexTests()
        {
            Regex r_top = new Regex(@"top\D*(?<count>\d+)");
            Match m = r_top.Match("top 17");
            Assert.True(m.Success);
            Assert.True(m.Groups["count"].Value == "17");
        }

        [Theory]
        [InlineData("200 +- 10", true, 200, 10)]
        [InlineData(" 0+-0 ", true, 0, 0)]
        [InlineData(" 6 7 + - 8 9 ", true, 6, 8)]
        [InlineData("100 50 +-", false, 0, 0)]
        public void RegexPlusMinus(string input, bool success, int from, int to )
        {
            Match m = FrequencyParser.r_plusminus.Match(input);
            Assert.True(success == m.Success, "Did not expect " + m.Success + " on " + input);
            if (m.Success)
            {
                Assert.True(from == FrequencyParser.GetFrom(m), input + " from=" + FrequencyParser.GetFrom(m));
                Assert.True(to == FrequencyParser.GetTo(m), input + " to=" + FrequencyParser.GetTo(m));
            }
        }

        [Fact]
        public void CSharpTwoQuestionMarks()
        {
            string x = null;
            string y = x ?? "hello";
            string z = y ?? "goodbye";
            Assert.Null(x);
            Assert.True(y == "hello");
            Assert.True(z == "hello");
        }

        [Fact]
        public void CodeScannerHtmlCss()
        {
            CodeScanner cs = new CodeScanner( Path.Combine( TestData.RootDirectory, "Website" ) );
            Assert.True( Directory.Exists(cs.RootDirectory));

            int files_scanned = cs.DoHtmlCssScan( "*.css", "*.htm", "*.aspx");
            Assert.True(files_scanned > 0);
            Console.WriteLine("{0} inline, {1} head, rating {2}%", cs.Inline, cs.Head, cs.Percentage.ToString("0.00"));
            foreach (CodeScannerHtmlCssAspx f in cs.MyFiles)
                Console.WriteLine(f.Total + " in " + f.Filename);
        }

        [Fact]
        public void CodeScannerCheckFile()
        {
            string  dir = Path.Combine( TestData.RootDirectory, "Website" );
            string filename = Path.Combine(dir, "ACOMapR.aspx");
            Assert.True(File.Exists(filename));

            CodeScannerHtmlCssAspx file = new CodeScannerHtmlCssAspx(filename);
            Console.WriteLine("{0} inline, {1} head, rating {2}%", file.Inline, file.Head, file.Percentage.ToString("0.00"));
        }

        [Fact]
        public void CountInStrings()
        {
            Assert.True(3 == CodeScannerHtmlCssAspx.CountInStrings("this { has { three { braces", "{"));
        }
    }
}
