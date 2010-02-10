using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScLib
{
    public class LiPage : System.Web.UI.Page
    {
        public string DataSetDirectory
        {
            get
            {
                return System.IO.Path.Combine(Request.PhysicalApplicationPath, @"..\DataSets");
            }
        }
    }
}
