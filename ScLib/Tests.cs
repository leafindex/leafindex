using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using System.Text.RegularExpressions;

namespace ScLib
{
    public class Tests
    {
        private const string XMLFILENAME = @"C:\CSharp\LeafIndex\DataSets\ambulance-all-assaults-ward.xml";
        private ACOAssaultFinder _finder = null;
        private ACOAssaultFinder MyFinder
        {
            get
            {
                if (_finder == null)
                    _finder = new ACOAssaultFinder(XMLFILENAME);
                return _finder;
            }
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
    }
}
