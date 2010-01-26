using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ScLib
{
    public class TestData
    {
        public static string XmlFileName
        {
            get
            {
                string dllfilename = (new System.Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase)).AbsolutePath;
                FileInfo fi = new FileInfo(dllfilename);
                DirectoryInfo leafindex = fi.Directory.Parent.Parent.Parent;
                return Path.Combine(leafindex.FullName, @"DataSets\ambulance-all-assaults-ward.xml");

            }
        }
    }
}
