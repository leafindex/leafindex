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

        private static UKLocationsFromKml _locations = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            string name = Request[ "name" ] ?? "England";
            int width, height;
            int.TryParse(Request["width"] ?? "0", out width);
            int.TryParse(Request["height"] ?? "0", out height);
            if (width <= 0 || width > 1000) width = 1000;
            if (height <= 0 || height > 500) height = 500;

            StringBuilder b = new StringBuilder();
            b.AppendLine("<script type='text/javascript'>");
            b.AppendLine("var _kmlmap;");
            b.AppendLine("function LoadKmlMap() {");
            b.AppendLine("var i = 0;");
            b.AppendLine("_kmlmap = new Array();");

            string kmlPath = Request.PhysicalApplicationPath + ConfigurationManager.AppSettings["UKLocationsFromKml"];
            UKLocationsFromKml u = GetLocations(kmlPath);
            foreach (PlaceWithLoops p in u.GetPlaces())
            {
                if (p.Name == name)
                {
                    List<string> paths = p.MakeSVGPathStrings(width, height);
                    foreach (string str in paths)
                        b.AppendLine("_kmlmap[i++] = '" + str + "';");
                }
            }
            b.AppendLine("}");

            b.AppendLine("</script>");
            _myscript = b.ToString();
        }

        private static UKLocationsFromKml GetLocations( string kmlPath )
        {
            if (_locations == null)
            {
                _locations = new UKLocationsFromKml( kmlPath );
                _locations.Load();
            }
            return _locations;
        }
    }
}
