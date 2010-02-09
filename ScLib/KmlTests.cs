using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using System.IO;
using System.Text.RegularExpressions;

namespace ScLib
{
    public class KmlTests
    {
        private const string FILENAME = @"C:\Documents and Settings\Jo Clarke\My Documents\Hardwix\Datasets\UKLocationsFromKml.kml";
        //private const string FILENAME = @"C:\Documents and Settings\Jo Clarke\My Documents\Hardwix\Datasets\TestLocations.kml";
        [Fact]
        public void FileExists()
        {
            Assert.True(File.Exists(FILENAME));
        }
        [Fact]
        public void UKLocationsFromKmlLoad()
        {
            UKLocationsFromKml u = new UKLocationsFromKml(FILENAME);
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
            UKLocationsFromKml u = new UKLocationsFromKml(FILENAME);
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
            UKLocationsFromKml u = new UKLocationsFromKml(FILENAME);
            PlaceWithLoops p = u.GetPlace("France");
            Assert.Null(p);
        }
        [Fact]
        public void England()
        {
            UKLocationsFromKml u = new UKLocationsFromKml(FILENAME);
            PlaceWithLoops p = u.GetPlace("ENGLAND");
            Assert.NotNull(p);
            Console.WriteLine(p.MyExtent);
        }
    }
}
