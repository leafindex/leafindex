using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace ScLib
{
    public class KmlTests
    {
        private string UkCountriesFile
        {
            get { return TestData.FullFilenameInDatasets("UKLocationsFromKml.kml"); }
        }
        private string PoliceFile
        {
            get { return TestData.FullFilenameInDatasets("UKPolice.kml"); }
        }

        [Fact]
        public void FileExists()
        {
            Assert.True(File.Exists(UkCountriesFile));
            Console.WriteLine("File exists " + UkCountriesFile);
        }
        [Fact]
        public void UKLocationsFromKmlLoad()
        {
            UKLocationsFromKml u = new UKLocationsFromKml(UkCountriesFile);
            List<string> places = u.GetPlacenames();
            Console.WriteLine(places.Count + " places");
            for (int i = 0; i < 20 && i < places.Count; i++)
                Console.WriteLine( i + ") " + places[i]);
        }
        [Fact]
        public void KmlPolyLineRegex()
        {
            MatchCollection coll = KmlLoop.Parse("-2.025399,55.8034,0 -2.0235,55.8029,0 -2.0223,55.80240000000001,0");
            Console.WriteLine(coll.Count + " matches");
            for (int i = 0; i < 10 && i < coll.Count; i++)
                Console.WriteLine(coll[i].Groups["x"].Value + ":" + coll[i].Groups["y"].Value + ":" + coll[i].Groups["z"].Value );
        }
        [Fact]
        public void KmlPolyLineRegexBug()
        {
            MatchCollection coll = KmlLoop.Parse("1,2,3 4,5,6 0,1,2");
            Console.WriteLine(coll.Count + " matches");
            for (int i = 0; i < 10 && i < coll.Count; i++)
                PrintXYZ(coll[i]);
        }

        [Fact]
        public void RegexDoesExponent()
        {
            string input = "-0.0003995174,53.54520000000001,0 4.825540000000001e-007,53.5445,0 0.0003004826,53.5429,0";
            MatchCollection coll = KmlLoop.Parse(input);
            for (int i = 0; i < 10 && i < coll.Count; i++)
                PrintXYZ(coll[i]);
        }
        [Fact]
        public void DoubleParseDoesExponent()
        {
            double x = double.Parse("4.825540000000001e-007");
            Console.WriteLine(x);
        }

        private void PrintXYZ(Match m)
        {
            Console.WriteLine( ":" + m.Value + ":x=" + m.Groups["x"].Value + ":y=" + m.Groups["y"].Value + ":z=" + m.Groups["z"].Value + ":" );

        }
        [Fact]
        public void CommaIsNotADigit()
        {
            Regex r = new Regex( @"\d+" );
            MatchCollection coll = r.Matches( "1,2 3" );
            Assert.True(coll.Count == 3);
        }

        [Fact]
        public void BailiwickOfGuernsey()
        {
            string name = "Bailiwick of Guernsey";
            UKLocationsFromKml u = new UKLocationsFromKml(UkCountriesFile);
            PlaceWithLoops p = u.GetPlace(name);
            Assert.NotNull(p);
            Assert.True(p.MyExtent.MinX < p.MyExtent.MaxX, "X not set");
            Assert.True(p.MyExtent.MinY < p.MyExtent.MaxY, "Y not set");
            List<string> paths = p.MakeSVGPathStrings(500, 400);
            Assert.True(paths.Count > 0, "No paths");
            foreach (string str in paths)
                Console.WriteLine(str);
        }

        [Fact]
        public void France()
        {
            UKLocationsFromKml u = new UKLocationsFromKml(UkCountriesFile);
            PlaceWithLoops p = u.GetPlace("France");
            Assert.Null(p);
        }
        [Fact]
        public void England()
        {
            UKLocationsFromKml u = new UKLocationsFromKml(UkCountriesFile);
            PlaceWithLoops p = u.GetPlace("ENGLAND");
            Assert.NotNull(p);
            Console.WriteLine(p.MyExtent);
            List<string> msgs = p.DebugNearby(-7.0, double.MinValue);
            foreach (string msg in msgs)
                Console.WriteLine(msg);
        }
        [Fact]
        public void GreatBritain()
        {
            UKLocationsFromKml u = new UKLocationsFromKml(UkCountriesFile);
            PlaceWithLoops p;
            p = u.GetPlace("ENGLAND");
            Console.WriteLine(p.Name + " " + p.MyExtent);
            p = u.GetPlace("SCOTLAND");
            Console.WriteLine(p.Name + " " + p.MyExtent);
            p = u.GetPlace("WALES");
            Console.WriteLine(p.Name + " " + p.MyExtent);

            p = u.CombinePlaces("ENGLAND", "SCOTLAND", "WALES");
            Console.WriteLine(p.Name + " " + p.MyExtent);

            List<string> names = new List<string>();
            names.Add("scotland");
            names.Add("wales");
            names.Add("england");
            p = u.CombinePlaces(names);
            Console.WriteLine(p.Name + " " + p.MyExtent);
        }
        [Fact]
        public void PoliceForces()
        {
            UKLocationsFromKml u = new UKLocationsFromKml(PoliceFile);
            foreach (PlaceWithLoops p in u.GetPlaces())
            {
                if (p.Name.Contains("Hertf"))
                    Console.WriteLine(p.Name + " " + p.LoopCount + " loops " + p.GetLoopDesc(0) + " " + p.MyExtent);
            }
            PlaceWithLoops p2 = u.GetPlace("Hertfordshire Constabulary");
            Console.WriteLine(p2.Name + " " + p2.LoopCount + " loops " + p2.GetLoopDesc(0) + " " + p2.MyExtent);
        }

        [Fact]
        public void Kml156737()
        {
            string filename = TestData.FullFilenameInDatasets("Kml156737.kml");
            Assert.True(File.Exists(filename));
            Console.WriteLine(filename);
            UKLocationsFromKml u = new UKLocationsFromKml(filename);
            foreach (string str in u.GetPlacenames())
                Console.WriteLine(str);
        }
        [Fact]
        public void Kml156738()
        {
            string filename = TestData.FullFilenameInDatasets("Kml156738.kml");
            Assert.True(File.Exists(filename));
            Console.WriteLine(filename);
            UKLocationsFromKml u = new UKLocationsFromKml(filename);
            foreach (string str in u.GetPlacenames())
                Console.WriteLine(str);
        }
    }
}
