using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace ScLib
{
	public class ACOAssaultBoroughs
	{
		private List<ACOAssaultBorough> _boroughs = new List<ACOAssaultBorough>();
		private string _errorMessage = "";

		public ACOAssaultBoroughs(string xmlfilename)
		{
				XDocument xdoc = XDocument.Load(xmlfilename);

				var boroughs = from b in xdoc.Descendants("ROW")
											 group b
											 by (string)b.Element("District_Name")
											 into g
											 select new
											 {
												borough = g.Key,
												sum = g.Sum(b => {
													int r;
													Int32.TryParse(b.Element("Count_12-2007_to_11-2009").Value, out r);
													return r;
												}),
												count = g.Count()
											 };

				foreach (var borough in boroughs)
				{
					_boroughs.Add(new ACOAssaultBorough(borough.borough, borough.sum, borough.count));
				}
		}
		
		public List<ACOAssaultBorough> Boroughs { get { return _boroughs; } }
		public string ErrorMessage { get { return _errorMessage; } }

	}
}
