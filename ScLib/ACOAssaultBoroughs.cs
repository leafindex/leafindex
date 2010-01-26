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
			try
			{
				var boroughs =
					from b in XDocument.Load(xmlfilename).Descendants("ROW")
					orderby (string)b.Element("District_Name")
					group b
					by (string)b.Element("District_Name")
						into g
						select new
						{
							borough = g.Key,
							sum = g.Sum(b =>
							{
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
			catch (Exception ex)
			{
				_errorMessage = ex.Message;
				_boroughs.Clear();
			}
		}
		
		public List<ACOAssaultBorough> Boroughs { get { return _boroughs; } }
		public string ErrorMessage { get { return _errorMessage; } }

	}
}
