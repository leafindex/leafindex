using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using ScLib;
using System.Data;
using System.Configuration;

namespace Website
{
    public partial class London : LiPage
    {
        private string _myscript = "";
        protected string MyScript { get { return _myscript; } }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillDropDowns();
                MakeScript();
            }
            
        }

        protected void RefreshClick(object sender, EventArgs e)
        {
            MakeScript();
        }

        private void FillDropDowns()
        {
            Mensa.SetConnectionString(ConfigurationManager.ConnectionStrings["datasets"].ToString());
            Mensa m = new Mensa();
            DataSet ds = m.LondonBoroughCrime3OffencesPerBorough("?", "?");
            ddlCrime.Items.Clear();
            foreach (DataRow dr in ds.Tables[0].Rows)
                ddlCrime.Items.Add(dr[0].ToString());
            ddlYear.Items.Clear();
            foreach (DataRow dr in ds.Tables[1].Rows)
                ddlYear.Items.Add(dr[0].ToString());
            if (ddlCrime.Items.Count > 0)
                ddlCrime.SelectedIndex = 0;
            if (ddlYear.Items.Count > 0)
                ddlYear.SelectedIndex = ddlYear.Items.Count - 1;
        }

        private void MakeScript()
        {
            int width = 1000, height = 500, mapcount = 0;
            StringBuilder b = new StringBuilder();
            UKLocationsFromKml u = new UKLocationsFromKml(UKLocationsFromKml.KmlSet.Counties, this.DataSetDirectory);

            b.AppendLine("<script type='text/javascript'>");

            Mensa.SetConnectionString(ConfigurationManager.ConnectionStrings["datasets"].ToString());
            Mensa m = new Mensa();
            DataTable dt_all = m.LondonBoroughCrime3OffencesPerBorough(ddlCrime.Text, ddlYear.Text).Tables[0];
            DataTable dt = dt_all.Clone();
            foreach (DataRow dr in dt_all.Select( MakeBoroughSelect() ) )
            {
                DataRow dr2 = dt.NewRow();
                for( int i = 0; i < dt.Columns.Count; i++ )
                    dr2[i] = dr[i];
                dt.Rows.Add( dr2 );
            }
            JsMaker.MakeTable(b, "_borough_data", "FillBoroughData", dt);

            b.AppendLine("var _kmlmap = new Array();");

            PlaceWithLoops combined = u.CombinePlaces(Boroughs);
            for( int i = 0; i < Boroughs.Length; i++ )
            {
                PlaceWithLoops p = u.GetPlace(Boroughs[i]);
                if (p == null)
                    continue;

                b.AppendLine("function LoadKmlMap" + mapcount + "() {");
                b.AppendLine("var i = 0;");
                b.AppendLine("_kmlmap" + mapcount + " = new Array();");
                b.AppendLine("_kmlmap" + mapcount + "[i++] = '" + p.Name +  "';");
                b.AppendLine("_kmlmap" + mapcount + "[i++] = '" + DistrictCodes[i] + "';");

                List<string> paths = p.MakeSVGPathStrings(width, height, combined.MyExtent);
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

        private string MakeBoroughSelect()
        {
            StringBuilder sb = new StringBuilder();
            foreach (string code in DistrictCodes)
            {
                if (sb.Length > 0)
                    sb.Append(" or ");
                sb.Append("[ONS Code] = '" + code + "'");
            }
            return sb.ToString();
        }

        private string[] Boroughs = new string[] { 
                "City of London", "Barking and Dagenham", "Barnet", "Bexley", "Brent", "Bromley", "Camden", "Croydon", 
                "Ealing", "Enfield", "Greenwich", "Hackney", "Hammersmith and Fulham", "Haringey", "Harrow", 
                "Havering", "Hillingdon", "Hounslow", "Islington", "Kensington and Chelsea", 
                "Kingston upon Thames", "Lambeth", "Lewisham", "Merton", "Newham", "Redbridge", 
                "Richmond upon Thames", "Southwark", "Sutton", "Tower Hamlets", "Waltham Forest", 
                "Wandsworth", "City of Westminster"
        };

        private string[] DistrictCodes = new string[] {
            "00AA","00AB","00AC","00AD","00AE","00AF","00AG","00AH",
            "00AJ","00AK","00AL","00AM","00AN","00AP","00AQ",
            "00AR","00AS","00AT","00AU","00AW",
            "00AX","00AY","00AZ","00BA","00BB","00BC",
            "00BD","00BE","00BF","00BG","00BH",
            "00BJ","00BK" 
        };
    }
}

