using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ScLib;
using System.IO;
using System.Text;

namespace Website
{
    public partial class Chartjunk : System.Web.UI.Page
    {
        private string _myscript = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CodeScanner cs = new CodeScanner( Request.PhysicalApplicationPath );
                cs.DoHtmlCssScan( "*.htm", "*.aspx");

                StringBuilder b = new StringBuilder();
                b.AppendLine("<script type='text/javascript'>");
                b.AppendLine("var _total_perc = " + cs.Percentage.ToString("0.00") + ";");
                b.AppendLine("var _data;");
                b.AppendLine("function LoadData() {");
                b.AppendLine("var i = 0;");
                b.AppendLine("_data = new Array();");
                foreach( CodeScannerHtmlCssAspx f in cs.MyFiles )
                    b.AppendLine("_data[i++] = new Array( '" + f.Filename + "', " 
                        + f.Percentage.ToString("0.00") 
                        + ", " + ( f.Head + f.Inline ) + " );");
                b.AppendLine("}");

                b.AppendLine("</script>");
                _myscript = b.ToString();
            }

        }

        protected string MyScript { get { return _myscript; } }
    }
}
