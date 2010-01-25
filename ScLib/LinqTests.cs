using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Xunit.Extensions;
using System.Xml.Linq;

namespace ScLib
{
    public class LinqTests
    {
        public const string FILENAME = @"C:\CSharp\LeafIndex\DataSets\ambulance-all-assaults-ward.xml";

        private string xmlfilename { get { return FILENAME; } }

        [Fact]
        public void HelloWorld()
        {
            Assert.True(2 == 1 + 1);
        }

        [Fact]
        public void FileExists()
        {
            Assert.True( System.IO.File.Exists( FILENAME ) );
        }
        [Fact]
        public void CanLoadFile()
        {
            XDocument xdoc = XDocument.Load(xmlfilename);
            var boroughs = from b in xdoc.Descendants("ROW")
                           group b
                           by (string)b.Element("District_Name")
                               into g
                               select new
                               {
                                   borough = g.Key,
                                   sum = g.Sum(b =>
                                   {
                                       int r;
                                       Int32.TryParse(b.Element("Count_12-2007_to_11-2009").Value, out r);
                                       return r;
                                   }),
                                   count = g.Count()
                               };

            foreach (var borough in boroughs)
            {
                Console.WriteLine("{0} S:{1} C:{2}", borough.borough, borough.sum, borough.count);
            }
        }

        [Fact]
        public void CopeWithBlankCountUsingMyIntParse()
        {
            string borough = "City of London";
            //string borough = "Lambeth";
            XDocument xdoc = XDocument.Load(xmlfilename);
            var wards = from w in xdoc.Descendants("ROW")
                        where (string)w.Element("District_Name") == borough
                        orderby (string)w.Element("Ward_Name")
                        select new
                        {
                            ward = (string)w.Element("District_Name"),
                            a = MyIntParse( w.Element("Count_12-2007_to_11-2009").Value )
                        };
            int total = 0;
            foreach (var ward in wards)
            {
                Console.WriteLine(ward.ToString());
                Assert.True(ward.a >= 0);
                total += ward.a;
            }
            Console.WriteLine("Total = " + total);
        }
        [Fact]
        public void CopeWithBlankCount()
        {
            string borough = "City of London";
            //string borough = "Lambeth";
            XDocument xdoc = XDocument.Load(xmlfilename);
            var wards = from w in xdoc.Descendants("ROW")
                        where (string)w.Element("District_Name") == borough
                        orderby (string)w.Element("Ward_Name")
                        select new
                        {
                            ward = (string)w.Element("District_Name"),
                            a = Convert.ToInt32( "0" + w.Element("Count_12-2007_to_11-2009").Value )
                        };
            int total = 0;
            foreach (var ward in wards)
            {
                Console.WriteLine(ward.ToString());
                Assert.True(ward.a >= 0);
                total += ward.a;
            }
            Console.WriteLine("Total = " + total);
        }

        private const string SOUTHWARK = "Southwark";
        private const string CITY_OF_LONDON = "City of London";

        [Fact]
        public void TopAndBottom3InSouthwark()
        {
            Console.WriteLine("Top 3");
            foreach (var ward in GetWardsTopBottom(SOUTHWARK, 3, true))
                Console.WriteLine(ward.ToString());
            Console.WriteLine("Bottom 3");
            foreach (var ward in GetWardsTopBottom(SOUTHWARK, 3, false))
                Console.WriteLine(ward.ToString());
        }
        [Fact]
        public void Southwark()
        {
            DoTopBottom(SOUTHWARK, 3, true);
            DoTopBottom(SOUTHWARK, 6, true);
            DoTopBottom(SOUTHWARK, 10, true);
            DoTopBottom(SOUTHWARK, 3, false); ;
        }
        [Fact]
        public void CityOfLondon()
        {
            DoTopBottom(CITY_OF_LONDON, 3, true);
            DoTopBottom(CITY_OF_LONDON, 6, true);
            DoTopBottom(CITY_OF_LONDON, 10, true);
            DoTopBottom(CITY_OF_LONDON, 3, false);
        }

        [Fact]
        public void NumericRange()
        {
            DoNumericRange(SOUTHWARK, 200, 300);
        }
        [Fact]
        public void CaseIndependent()
        {
            DoTopBottom(CITY_OF_LONDON.ToUpper(), 3, true);
        }
        [Fact]
        public void Ham()
        {
            DoTopBottom("ham", 10, true);
        }
        [Fact]
        public void BlankBorough()
        {
            DoTopBottom("", 10, true);
        }

        [Fact]
        public void ContainsIsCaseSensitive()
        {
            Assert.False("Hello".Contains("HELL"));
            Assert.True("Hello".ToUpper().Contains("HELL"));
        }


        private void DoNumericRange(string borough, int from, int to)
        {
            object[] wards = GetWardsNumericRange(borough, from, to );
            Console.WriteLine( "Wards in " + borough + " in range " + from + " to " + to + " returned " + wards.Length);
            foreach (var ward in wards)
                Console.WriteLine(ward.ToString());
        }

        private void DoTopBottom(string borough, int count, bool do_top)
        {
            object[] wards = GetWardsTopBottom(borough, count, do_top);
            Console.WriteLine((do_top ? "Top " : "Bottom ") + count + " wards in " + borough + " returned " + wards.Length);
            foreach (var ward in wards )
                Console.WriteLine(ward.ToString());
        }

        private object[] GetWardsNumericRange(string borough, int from, int to)
        {
            borough = borough.Trim().ToUpper();
            XDocument xdoc = XDocument.Load(xmlfilename);
            var wards = from w in xdoc.Descendants("ROW")
                        where w.Element("District_Name").Value.ToUpper().Contains(borough)
                        orderby MyIntParse(w.Element("Count_12-2007_to_11-2009").Value) descending
                        select new
                        {
                            ward = w.Element("Ward_Name").Value,
                            borough = w.Element("District_Name").Value,
                            incidents = MyIntParse(w.Element("Count_12-2007_to_11-2009").Value)
                        };
            return wards.Where(w => w.incidents >= from && w.incidents <= to).ToArray();
        }

        private object[] GetWardsTopBottom(string borough, int count, bool do_top )
        {
            borough = borough.Trim().ToUpper();
            XDocument xdoc = XDocument.Load(xmlfilename);
            var wards = from w in xdoc.Descendants("ROW")
                        where w.Element("District_Name").Value.ToUpper().Contains(borough)
                        orderby MyIntParse(w.Element("Count_12-2007_to_11-2009").Value) descending
                        select new
                        {
                            ward = w.Element("Ward_Name").Value,
                            borough = w.Element("District_Name").Value,
                            incidents = MyIntParse(w.Element("Count_12-2007_to_11-2009").Value)
                        };

            if (wards.Count() <= count)
                return wards.ToArray();
            if (!do_top)
                wards = wards.Reverse();
            int cutoff = wards.Take(count).Last().incidents;
            if( do_top )
                return wards.TakeWhile( w => w.incidents >= cutoff).ToArray();
            else
                return wards.TakeWhile( w => w.incidents <= cutoff).Reverse().ToArray();
        }

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
