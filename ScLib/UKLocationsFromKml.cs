using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Text.RegularExpressions;

namespace ScLib
{
    public class UKLocationsFromKml
    {
        private string _xmlfilename;
        private List<PlaceWithLoops> _places;
        public UKLocationsFromKml( string filename )
        {
            _xmlfilename = filename;
            _places = null;
        }

        public void Load()
        {
            _places = new List<PlaceWithLoops>();
            XDocument doc = XDocument.Load(_xmlfilename);
            var places =
                    from b in doc.Descendants("Document")
                        .Descendants("Folder")
                        .Descendants("Folder")
                        .Descendants("Folder")
                        .Descendants("Placemark")
                    select new PlaceWithLoops(b);
            foreach (var place in places)
            {
                if (place.LoopCount > 0)
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
            if (kpl.MyExtent.IsSet)
            {
                if (_MinX == double.MinValue || kpl.MyExtent.MinX < _MinX) _MinX = kpl.MyExtent.MinX;
                if (_MaxX == double.MinValue || kpl.MyExtent.MaxX > _MaxX) _MaxX = kpl.MyExtent.MaxX;
                if (_MinY == double.MinValue || kpl.MyExtent.MinY < _MinY) _MinY = kpl.MyExtent.MinY;
                if (_MaxY == double.MinValue || kpl.MyExtent.MaxY > _MaxY) _MaxY = kpl.MyExtent.MaxY;
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
            double result2 = height / (_MaxY - _MinY);
            return result1 < result2 ? result1 : result2;
        }
    }

    public class PlaceWithLoops
    {
        string _name;
        Extent _extent;
        public Extent MyExtent { get { return _extent; } }

        List<KmlLoop> _loops = new List<KmlLoop>();
        public PlaceWithLoops(XElement elt)
        {
            _extent = new Extent();
            _name = elt.Element("name").Value;
            List<XElement> coords = new List<XElement>();
            coords.AddRange( elt
                .Descendants( "MultiGeometry" )
                .Descendants( "Polygon" )
                .Descendants( "outerBoundaryIs" )
                .Descendants( "LinearRing" )
                .Descendants( "coordinates" ) );

            foreach (XElement c in coords)
            {
                KmlLoop kpl = new KmlLoop(c.Value);
                if (kpl.CoordCount > 0)
                {
                    _loops.Add(kpl);
                    _extent.Inflate(kpl);
                }
            }
        }
        public string Name { get { return _name; } }
        public int LoopCount { get { return _loops.Count; } }

        public string GetLoopDesc(int i)
        {
            if (i >= 0 && i < _loops.Count)
                return _loops[i].CoordCount + " coords e.g." + _loops[i].GetCoord(0).ToString();
            return "";
        }
        public List<string> MakeSVGPathStrings(int width, int height)
        {
            List<string> result = new List<string>();
            double scale_factor = MyExtent.CalculateScaleFactor(width, height);
            foreach (KmlLoop kl in _loops)
                result.Add(kl.MakeSVGPathString(scale_factor, this.MyExtent));
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
                KmlCoord c = new KmlCoord(m.Groups["x"].Value, m.Groups["y"].Value, m.Groups["z"].Value);
                _list.Add(c);
                _extent.Inflate(c);
                if (c.X < -6.0)
                {
                    Console.WriteLine(c.ToString() + " <- " + m.Value );
                }
            }
	    }

        public static MatchCollection Parse(string input )
        {
            if (r_coord == null)
                r_coord = new Regex(@"(?<x>[+-]{0,1}\d+(\.\d+){0,1})\s*\,\s*(?<y>[+-]{0,1}\d+(\.\d+){0,1})\s*\,\s*(?<z>[+-]{0,1}\d+(\.\d+){0,1})");
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

        private string MakeSVGPoint(double scale_factor, Extent extent, double x, double y)
        {
            return Scale(scale_factor, x, extent.MinX, extent.MaxX) + " "
                + ScaleInvert(scale_factor, y, extent.MinY, extent.MaxY);
        }
        private int ScaleInvert(double scale_factor, double value, double minvalue, double maxvalue)
        {
            return (int)(scale_factor * (maxvalue - value) + 0.5);
        }
        private int Scale(double scale_factor, double value, double minvalue, double maxvalue)
        {
            return (int)(scale_factor * (value - minvalue) + 0.5);
        }
    }
    public class KmlCoord
    {
        private double _x, _y, _z;
        public KmlCoord(string x, string y, string z)
        {
            try
            {
                _x = double.Parse(x);
                _y = double.Parse(y);
                _z = double.Parse(z);
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
            return _x.ToString("0.0000") + "," + _y.ToString("0.0000");
        }
    }
}
