using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using System.IO;

namespace ScLib
{
    public static class UKLocationsDictionary
    {
        private static Dictionary<UKLocationsFromKml.KmlSet, UKLocationsFromKml> _dict = new Dictionary<UKLocationsFromKml.KmlSet, UKLocationsFromKml>();

        public static UKLocationsFromKml Fetch(UKLocationsFromKml.KmlSet set, string directory)
        {
            if (!_dict.ContainsKey(set))
            {
                _dict.Add(set, new UKLocationsFromKml(set, directory));
                _dict[set].Load();
            }
            return _dict[set];
        }
    }

    public class UKLocationsFromKml
    {
        private string _xmlfilename;
        private List<PlaceWithLoops> _places;

        private XNamespace _MyNs = null;
        public XNamespace MyNs
        {
            get
            {
                return _MyNs;
            }
        }

        public enum KmlSet { UKCountries, Police, Counties, EnglishRegions };

        public UKLocationsFromKml(KmlSet set, string directory)
        {
            _xmlfilename = MakeFullFilenameForSet(set, directory);
            _places = null;
        }

        public UKLocationsFromKml( string filename )
        {
            _xmlfilename = filename;
            _places = null;
        }

        public string MakeFullFilenameForSet(KmlSet set, string directory)
        {
            return Path.Combine(directory, MakeFilenameForSet(set));
        }

        public string MakeFilenameForSet(KmlSet set)
        {
            if (set == KmlSet.UKCountries) return "UKLocationsFromKml.kml";
            if (set == KmlSet.Police) return "UKPolice.kml";
            if (set == KmlSet.Counties) return "kml156738.kml";
            if (set == KmlSet.EnglishRegions) return "kml156737.kml";
            throw new Exception("MakeFilenameForSet does not handle " + set.ToString());
        }

        public void Load()
        {
            _places = new List<PlaceWithLoops>();
            XElement elt = XElement.Load(_xmlfilename);
            _MyNs = elt.Attribute("xmlns").Value;
            var places =
                    from b in elt.Descendants(MyNs + "Document")
                        //.Descendants(MyNs + "Folder")
                        //.Descendants(MyNs + "Folder")
                        //.Descendants(MyNs + "Folder")
                        .Descendants(MyNs + "Placemark")
                    select new PlaceWithLoops(b, MyNs );
            foreach (var place in places)
            {
                if (place.IsNonTrivial() )
                    _places.Add(place);
            }
        }

        public List<PlaceWithLoops> GetPlaces()
        {
            if (_places == null)
                Load();
            return _places;
        }
        public List<string> GetPlacenames()
        {
            List<string> result = new List<string>();
            foreach (PlaceWithLoops p in GetPlaces())
                result.Add(p.Name);
            return result;

        }

        public PlaceWithLoops GetPlace(string name)
        {
            foreach (PlaceWithLoops p in GetPlaces())
                if (String.Compare(p.Name, name, true) == 0 )
                    return p;
            return null;
        }

        public PlaceWithLoops CombinePlaces(IEnumerable<string> names)
        {
            string combined_name = "";
            PlaceWithLoops result = new PlaceWithLoops();
            foreach (string name in names)
            {
                PlaceWithLoops p = GetPlace(name);
                if (p != null)
                {
                    if (combined_name == "")
                        combined_name = name;
                    else
                        combined_name += "/" + name;
                    result.Add(p);
                }
            }
            result.SetName(combined_name);
            return result;
        }
        public PlaceWithLoops CombinePlaces( params string [] names )
        {
            List<string> nlist = new List<string>(names);
            return CombinePlaces(nlist);
        }
    }

    public class Extent
    {
        double _MinX, _MaxX, _MinY, _MaxY;

        public double MinX { get { return _MinX; } }
        public double MaxX { get { return _MaxX; } }
        public double MinY { get { return _MinY; } }
        public double MaxY { get { return _MaxY; } }

        public Extent()
        {
            _MinX = _MaxX = _MinY = _MaxY = double.MinValue;
        }

        public void Inflate(KmlCoord c)
        {
            if (_MinX == double.MinValue || c.X < _MinX) _MinX = c.X;
            if (_MaxX == double.MinValue || c.X > _MaxX) _MaxX = c.X;
            if (_MinY == double.MinValue || c.Y < _MinY) _MinY = c.Y;
            if (_MaxY == double.MinValue || c.Y > _MaxY) _MaxY = c.Y;
        }

        public void Inflate(KmlLoop kpl)
        {
            Inflate(kpl.MyExtent);
        }
        public void Inflate(Extent e)
        {
            if (e.IsSet)
            {
                if (_MinX == double.MinValue || e.MinX < _MinX) _MinX = e.MinX;
                if (_MaxX == double.MinValue || e.MaxX > _MaxX) _MaxX = e.MaxX;
                if (_MinY == double.MinValue || e.MinY < _MinY) _MinY = e.MinY;
                if (_MaxY == double.MinValue || e.MaxY > _MaxY) _MaxY = e.MaxY;
            }
        }
        public override string ToString()
        {
            if (IsSet)
                return String.Format("{0},{1} to {2},{3}",
                    _MinX.ToString("0.0000"), _MinY.ToString("0.0000"),
                    _MaxX.ToString("0.0000"), _MaxY.ToString("0.0000"));
            else
                return "Empty";
        }
        public bool IsSet { get { return MinX != double.MinValue; } }

        public double CalculateScaleFactor(int width, int height)
        {
            if( !IsSet )
                return 1.0;
            double result1 = width / (_MaxX - _MinX);
            double result2 = height / ( (_MaxY - _MinY) * KmlLoop.LATITUDE_53_MULTIPLIER );
            return result1 < result2 ? result1 : result2;
        }
    }

    public class PlaceWithLoops
    {
        string _name;
        Extent _extent;
        public Extent MyExtent { get { return _extent; } }

        List<KmlLoop> _loops = new List<KmlLoop>();

        public PlaceWithLoops()
        {
            _extent = new Extent();
            _name = "";
        }
        public PlaceWithLoops(XElement elt, XNamespace ns )
        {
            _extent = new Extent();
            _name = elt.Element( ns + "name").Value;

            var coords =
                from b in elt.Descendants(ns + "coordinates")
                select new { str = b.Value };
            foreach (var c in coords)
            {
                KmlLoop kpl = new KmlLoop(c.str);
                if (kpl.CoordCount > 0)
                {
                    _loops.Add(kpl);
                    _extent.Inflate(kpl);
                }
            }
        }
        public string Name { get { return _name; } }
        public void SetName(string value)
        {
            _name = value;
        }
        public int LoopCount { get { return _loops.Count; } }

        public void Add(PlaceWithLoops p)
        {
            _extent.Inflate(p.MyExtent);
            foreach (KmlLoop kl in p._loops)
                _loops.Add(kl);
        }

        public string GetLoopDesc(int i)
        {
            if (i >= 0 && i < _loops.Count)
                return _loops[i].CoordCount + " coords e.g." + _loops[i].GetCoord(0).ToString();
            return "";
        }

        public List<string> MakeSVGPathStrings(int width, int height)
        {
            return MakeSVGPathStrings(width, height, MyExtent);
        }
        public List<string> MakeSVGPathStrings(int width, int height, Extent e)
        {
            List<string> result = new List<string>();
            double scale_factor = e.CalculateScaleFactor(width, height);
            foreach (KmlLoop kl in _loops)
                result.Add(kl.MakeSVGPathString(scale_factor, e));
            return result;
        }

        public bool IsNonTrivial()
        {
            if (LoopCount == 0)
                return false;
            foreach (KmlLoop kl in _loops)
                if (kl.CoordCount > 1)
                    return true;
            return false;
        }

        public List<string> DebugNearby(double x, double y)
        {
            List<string> result = new List<string>();
            foreach (KmlLoop kl in _loops)
                result.AddRange( kl.DebugNearby(x, y) );
            return result;
        }
    }

    public class KmlLoop
    {
        private static Regex r_coord = null;
        private List<KmlCoord> _list;
        public int CoordCount { get { return _list.Count; } }
        Extent _extent;
        public Extent MyExtent { get { return _extent; } }

        public KmlLoop(string coords)
	    {
            _list = new List<KmlCoord>();
            _extent = new Extent();
            foreach (Match m in Parse(coords))
            {
                KmlCoord c = new KmlCoord(m.Groups["x"].Value, m.Groups["y"].Value, m.Groups["z"].Value, m.Value );
                _list.Add(c);
                _extent.Inflate(c);
            }
	    }

        public static MatchCollection Parse(string input )
        {
            if (r_coord == null)
                r_coord = new Regex(@"(?<x>[+-e\d\.]+),(?<y>[+-e\d\.]+),(?<z>[+-e\d\.]+)");
                //r_coord = new Regex(@"(?<x>[+-]{0,1}\d+(\.\d+){0,1})\s*\,\s*(?<y>[+-]{0,1}\d+(\.\d+){0,1})\s*\,\s*(?<z>[+-]{0,1}\d+(\.\d+){0,1})");
            return r_coord.Matches(input);
        }
        public KmlCoord GetCoord(int i)
        {
            if (i >= 0 && i < _list.Count)
                return _list[i];
            return null;
        }

        public string MakeSVGPathString(double scale_factor, Extent extent)
        {
            StringBuilder result = new StringBuilder();
            string str, last_added = String.Empty;
            foreach (KmlCoord kc in _list)
            {
                str = MakeSVGPoint(scale_factor, extent, kc.X, kc.Y);
                if (str != last_added)
                {
                    if (result.Length == 0)
                        result.Append("M ");
                    else
                        result.Append(" L ");
                    result.Append(str);
                    last_added = str;
                }
            }
            if (_list.Count > 0)
            {
                KmlCoord kc = _list[0];
                str = MakeSVGPoint(scale_factor, extent, kc.X, kc.Y);
                if (str != last_added)
                {
                    result.Append(str);
                    last_added = str;
                }
            }
            return result.ToString();
        }

        public const double LATITUDE_53_MULTIPLIER = 1.65759415;

        private string MakeSVGPoint(double scale_factor, Extent extent, double x, double y)
        {
            return Scale(scale_factor, x, extent.MinX, extent.MaxX) + " "
                + ScaleInvert(scale_factor, y * LATITUDE_53_MULTIPLIER,
                    extent.MinY * LATITUDE_53_MULTIPLIER, extent.MaxY * LATITUDE_53_MULTIPLIER);
        }
        private int ScaleInvert(double scale_factor, double value, double minvalue, double maxvalue)
        {
            return (int)(scale_factor * (maxvalue - value) + 0.5);
        }
        private int Scale(double scale_factor, double value, double minvalue, double maxvalue)
        {
            return (int)(scale_factor * (value - minvalue) + 0.5);
        }

        public List<string> DebugNearby(double x, double y)
        {
            List<string> result = new List<string>();
            for( int i = 0; i < _list.Count; i++ )
            {
                KmlCoord kc = _list[i];
                if (kc.Nearby(x, y))
                {
                    if (i > 0)
                        result.Add(String.Format("{0}) {1}", i - 1, _list[i-1].ToString()));
                    result.Add(String.Format("{0}) {1}", i, kc.ToString()));
                    if( i + 1 < _list.Count )
                        result.Add(String.Format("{0}) {1}", i + 1, _list[i + 1].ToString()));
                }
            }
            return result;
        }
    }
    public class KmlCoord
    {
        private double _x, _y, _z;
        private string _msg;
        public KmlCoord(string x, string y, string z, string msg )
        {
            try
            {
                _x = double.Parse(x);
                _y = double.Parse(y);
                _z = double.Parse(z);
                _msg = msg;
            }
            catch
            {
                throw new Exception("Did not like :" + x + ":" + y + ":" + z + ":" );
            }
        }
        public double X { get { return _x; } }
        public double Y { get { return _y; } }
        public double Z { get { return _z; } }
        public override string ToString()
        {
            return _x.ToString("0.0000") + "," + _y.ToString("0.0000") + " " + _msg;
        }

        private const double TOLERANCE = 0.0001;
        public bool Nearby(double x, double y)
        {
            double diff;
            if (x == double.MinValue && y == double.MinValue)
                return false;

            if (x != double.MinValue)
            {
                diff = X - x;
                if (diff < 0 - TOLERANCE || diff > TOLERANCE)
                    return false;
            }
            if (y != double.MinValue)
            {
                diff = Y - y;
                if (diff < 0 - TOLERANCE || diff > TOLERANCE)
                    return false;
            }
            return true;
        }
    }
}
