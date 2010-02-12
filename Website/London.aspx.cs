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
        private LiTimer _timer;

        protected void Page_Load(object sender, EventArgs e)
        {
            _timer = new LiTimer("Page_Load at " + DateTime.Now.ToString("HH:mm:ss dd/MM/yy"));
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
            _timer.StartSubset("FillDropDowns");
            Mensa.SetConnectionString(ConfigurationManager.ConnectionStrings["datasets"].ToString());
            Mensa m = new Mensa();
            DataSet ds = m.LondonBoroughCrime3OffencesPerBorough("?", "?");
            FillDropDown( ddlCrime, ds.Tables[0], 0, false );
            FillDropDown( ddlYear, ds.Tables[1], 0, true );

            ds = m.LondonArtsEngagement("?");
            FillDropDown(ddlArtType, ds.Tables[0], 0, false);

            FillDropDown(ddlBeggingYear, true, "2008", "2009" );
        }

        private void FillDropDown(DropDownList ddl, bool select_last, params string [] values )
        {
            ddl.Items.Clear();
            foreach (string value in values)
                ddl.Items.Add(value);
            SelectDdlItem(ddl, select_last);
        }
        private void FillDropDown(DropDownList ddl, DataTable dt, int colno, bool select_last )
        {
            ddl.Items.Clear();
            foreach (DataRow dr in dt.Rows)
                ddl.Items.Add(dr[colno].ToString());
            SelectDdlItem(ddl, select_last);

        }
        private void SelectDdlItem( DropDownList ddl, bool select_last )
        {
            if (ddl.Items.Count > 0)
            {
                if (select_last)
                    ddl.SelectedIndex = ddl.Items.Count - 1;
                else
                    ddl.SelectedIndex = 0;
            }
        }

        private string TabSelected
        {
            get
            {
                return this.Request["hdnTabSelected"] ?? "Arts";
            }
        }

        private void MakeScriptNull()
        {
        }
        private void MakeScript()
        {
            _timer.StartSubset("MakeScript Start");
            int width = 1000, height = 500, mapcount = 0;
            StringBuilder b = new StringBuilder();

            //_timer.StartSubset("MakeScript UKLocationsFromKml ctor");
            //UKLocationsFromKml u = new UKLocationsFromKml(UKLocationsFromKml.KmlSet.Counties, this.DataSetDirectory);
            //_timer.StartSubset("MakeScript UKLocationsFromKml Load");
            //u.Load();

            _timer.StartSubset("MakeScript Static Fetch");
            UKLocationsFromKml u = UKLocationsDictionary.Fetch(UKLocationsFromKml.KmlSet.Counties, this.DataSetDirectory);
            _timer.StartSubset("MakeScript after Fetch");

            b.AppendLine("<script type='text/javascript'>");

            b.AppendLine("function SelectCurrentTab()");
            b.AppendLine("{");
            b.AppendLine("SelectTab( '" + TabSelected + "' );");
            b.AppendLine("}");

            _timer.StartSubset("MakeScript GetData");
            Mensa.SetConnectionString(ConfigurationManager.ConnectionStrings["datasets"].ToString());
            // DataTable dt = GetCrimeData();
            // DataTable dt = GetArtsData();
            DataTable dt;
            if( TabSelected == "Crimes" )
                dt = GetCrimeData();
            else if( TabSelected == "Arts" )
                dt = GetArtsData();
            else
                dt = GetBeggingData();

            _timer.StartSubset("MakeScript MakeTable");
            JsMaker.MakeTable(b, "_borough_data", "FillBoroughData", dt);

            b.AppendLine("var _kmlmap = new Array();");

            _timer.StartSubset("MakeScript CombinePlaces");
            PlaceWithLoops combined = u.CombinePlaces(Boroughs);
            for( int i = 0; i < Boroughs.Length; i++ )
            {
                _timer.StartSubset("MakeScript Boroughs Loop");

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
            _timer.StartSubset("MakeScript Last Few Lines");
            b.AppendLine("function LoadKmlMaps() {");
            for (int i = 0; i < mapcount; i++)
                b.AppendLine("LoadKmlMap" + i + "();");
            b.AppendLine("}");
            b.AppendLine("</script>");

            foreach (string timer_result in _timer.GetResults())
                b.AppendLine("<!-- " + timer_result + " -->");

            _myscript = b.ToString();
        }

        private DataTable GetBeggingData()
        {
            Mensa m = new Mensa();
            return m.LondonBoroughBegging(ddlBeggingYear.Text).Tables[0];
        }

        private DataTable GetArtsData()
        {
            Mensa m = new Mensa();
            return m.LondonArtsEngagement(ddlArtType.Text).Tables[0];
        }

        private DataTable GetCrimeData()
        {
            Mensa m = new Mensa();
            DataTable dt_all = m.LondonBoroughCrime3OffencesPerBorough(ddlCrime.Text, ddlYear.Text).Tables[0];
            DataTable dt = dt_all.Clone();
            foreach (DataRow dr in dt_all.Select(MakeBoroughSelect()))
            {
                DataRow dr2 = dt.NewRow();
                for (int i = 0; i < dt.Columns.Count; i++)
                    dr2[i] = dr[i];
                dt.Rows.Add(dr2);
            }
            return dt;
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

