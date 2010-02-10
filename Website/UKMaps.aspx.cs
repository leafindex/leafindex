using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using ScLib;
using System.Configuration;

namespace Website
{
    public partial class UKMaps : System.Web.UI.Page
    {
        private string _myscript = "";
        protected string MyScript { get { return _myscript; } }

        private static Dictionary<string, UKLocationsFromKml> _dict = null;

        private const string SOURCE_COUNTRIES = "countries";
        private const string SOURCE_POLICE = "police";

        protected void Page_Load(object sender, EventArgs e)
        {
            string name = Request[ "name" ] ?? "England";
            string source = Request["source"] ?? SOURCE_COUNTRIES;
            int width, height;
            int.TryParse(Request["width"] ?? "0", out width);
            int.TryParse(Request["height"] ?? "0", out height);
            if (width <= 0 || width > 1000) width = 1000;
            if (height <= 0 || height > 500) height = 500;

            int mapcount = 0;

            StringBuilder b = new StringBuilder();

            b.AppendLine("<!-- " + GetKmlFileForSource(source) + " -->");
            b.AppendLine("<script type='text/javascript'>");
            b.AppendLine("var _kmlmap = new Array();");

            UKLocationsFromKml u = GetLocations(source);
            List<string> names = new List<string>();
            if (name == "all")
            {
                foreach (string n1 in u.GetPlacenames())
                    if (!names.Contains(n1))
                        names.Add(n1);
            }
            else
            {
                names.Add(name);
                int x = 2;
                while( Request[ "name" + x.ToString() ] != null )
                {
                    names.Add( Request["name" + x.ToString()] );
                    x++;
                }
            }

            PlaceWithLoops combined = u.CombinePlaces(names);
            foreach (string n in names)
            {
                if (n == "")
                    continue;
                PlaceWithLoops p = u.GetPlace(n);
                if (p == null)
                    continue;

                b.AppendLine("function LoadKmlMap" + mapcount + "() {");
                b.AppendLine("var i = 0;");
                b.AppendLine("_kmlmap" + mapcount + " = new Array();");
                b.AppendLine("_kmlmap" + mapcount + "[i++] = '" + p.Name + "';");

                List<string> paths = p.MakeSVGPathStrings(width, height, combined.MyExtent );
                foreach (string str in paths)
                    b.AppendLine("_kmlmap" + mapcount + "[i++] = '" + str + "';");
                b.AppendLine("_kmlmap[ _kmlmap.length ] = _kmlmap" + mapcount + ";");
                b.AppendLine("}");
                mapcount++;
            }
            b.AppendLine("function LoadKmlMaps() {");
            for (int i = 0; i < mapcount; i++)
                b.AppendLine("LoadKmlMap" + i + "();");
            b.AppendLine("}");
            b.AppendLine("</script>");
            _myscript = b.ToString();
        }

        private UKLocationsFromKml GetLocations(string source)
        {
            if (_dict == null)
                _dict = new Dictionary<string, UKLocationsFromKml>();
            if (!_dict.ContainsKey(source))
            {
                UKLocationsFromKml u = new UKLocationsFromKml( GetKmlFileForSource( source ) );
                u.Load();
                _dict.Add( source, u  );
            }
            return _dict[source];
        }

        private string GetKmlFileForSource(string source)
        {
            if (source == SOURCE_COUNTRIES)
                return System.IO.Path.Combine(Request.PhysicalApplicationPath, @"..\DataSets\UKLocationsFromKml.kml" );
            if (source == SOURCE_POLICE)
                return System.IO.Path.Combine(Request.PhysicalApplicationPath, @"..\DataSets\UKPolice.kml");

            return System.IO.Path.Combine(Request.PhysicalApplicationPath, @"..\DataSets\" + source + ".kml" );
        }
    }
}
