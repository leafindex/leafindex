using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ScLib
{
    public class TestData
    {
        public static string RootDirectory
        {
            get
            {
                string dllfilename = (new System.Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase)).AbsolutePath;
                FileInfo fi = new FileInfo(dllfilename);
                DirectoryInfo leafindex = fi.Directory.Parent.Parent.Parent;
                return leafindex.FullName;
            }
        }
        public static string XmlFileName
        {
            get
            {
                return FullFilenameInDatasets("ambulance-all-assaults-ward.xml");
            }
        }
        public static string FullFilenameInDatasets(string filename)
        {
            return Path.Combine(RootDirectory, @"DataSets\" + filename);
        }
    }
}
