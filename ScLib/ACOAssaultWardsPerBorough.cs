using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace ScLib
{
	public class ACOAssaultWardsPerBorough
	{
		private List<ACOAssaultWard1> _wards = new List<ACOAssaultWard1>();
		private string _errorMessage = "";

		public ACOAssaultWardsPerBorough(string xmlfilename, string borough)
		{
			try
			{
				int a;
				var wards =
					from w in XDocument.Load(xmlfilename).Descendants("ROW")
					where String.Equals(
						(string)w.Element("District_Name").Value, borough, StringComparison.OrdinalIgnoreCase
						)
					orderby (string)w.Element("Ward_Name")
					select new
					{
						ward = (string)w.Element("Ward_Name"),
						assaults = (int?)(
							Int32.TryParse(w.Element("Count_12-2007_to_11-2009").Value, out a)
								? a : (int?)null
							)
					};
				foreach (var ward in wards)
				{
					_wards.Add(new ACOAssaultWard1(ward.ward, ward.assaults));
				}
			}
			catch (Exception ex)
			{
				_errorMessage = ex.Message;
				_wards.Clear();
			}
		}

		public List<ACOAssaultWard1> Wards { get { return _wards; } }
		public string ErrorMessage { get { return _errorMessage; } }

	}
}
