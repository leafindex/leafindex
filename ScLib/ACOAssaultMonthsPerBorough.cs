using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace ScLib
{
	public class ACOAssaultMonthsPerBorough
	{
		private List<ACOAssaultMonth> _months = new List<ACOAssaultMonth>();
		private string _errorMessage = "";

		public ACOAssaultMonthsPerBorough(string xmlfilename, string borough)
		{
			try
			{
				var months =
					from m in XDocument.Load(xmlfilename).Descendants("ROW")
					where String.Equals(
						(string)m.Element("District_Name").Value, borough, StringComparison.OrdinalIgnoreCase
						)
					group m
					by (string)m.Element("District_Name")
						into g
						select new
						{
							_1 = g.Sum(m =>
							{
								int r;
								Int32.TryParse(m.Element("Count_12-2007").Value, out r);
								return r;
							}),
							_2 = g.Sum(m =>
							{
								int r;
								Int32.TryParse(m.Element("Count_1-2008").Value, out r);
								return r;
							}),
							_3 = g.Sum(m =>
							{
								int r;
								Int32.TryParse(m.Element("Count_2-2008").Value, out r);
								return r;
							}),
							_4 = g.Sum(m =>
							{
								int r;
								Int32.TryParse(m.Element("Count_3-2008").Value, out r);
								return r;
							}),
							_5 = g.Sum(m =>
							{
								int r;
								Int32.TryParse(m.Element("Count_4-2008").Value, out r);
								return r;
							}),
							_6 = g.Sum(m =>
							{
								int r;
								Int32.TryParse(m.Element("Count_5-2008").Value, out r);
								return r;
							}),
							_7 = g.Sum(m =>
							{
								int r;
								Int32.TryParse(m.Element("Count_6-2008").Value, out r);
								return r;
							}),
							_8 = g.Sum(m =>
							{
								int r;
								Int32.TryParse(m.Element("Count_7-2008").Value, out r);
								return r;
							}),
							_9 = g.Sum(m =>
							{
								int r;
								Int32.TryParse(m.Element("Count_8-2008").Value, out r);
								return r;
							}),
							_10 = g.Sum(m =>
							{
								int r;
								Int32.TryParse(m.Element("Count_9-2008").Value, out r);
								return r;
							}),
							_11 = g.Sum(m =>
							{
								int r;
								Int32.TryParse(m.Element("Count_10-2008").Value, out r);
								return r;
							}),
							_12 = g.Sum(m =>
							{
								int r;
								Int32.TryParse(m.Element("Count_11-2008").Value, out r);
								return r;
							}),
							_13 = g.Sum(m =>
							{
								int r;
								Int32.TryParse(m.Element("Count_12-2008").Value, out r);
								return r;
							}),
							_14 = g.Sum(m =>
							{
								int r;
								Int32.TryParse(m.Element("Count_1-2009").Value, out r);
								return r;
							}),
							_15 = g.Sum(m =>
							{
								int r;
								Int32.TryParse(m.Element("Count_2-2009").Value, out r);
								return r;
							}),
							_16 = g.Sum(m =>
							{
								int r;
								Int32.TryParse(m.Element("Count_3-2009").Value, out r);
								return r;
							}),
							_17 = g.Sum(m =>
							{
								int r;
								Int32.TryParse(m.Element("Count_4-2009").Value, out r);
								return r;
							}),
							_18 = g.Sum(m =>
							{
								int r;
								Int32.TryParse(m.Element("Count_5-2009").Value, out r);
								return r;
							}),
							_19 = g.Sum(m =>
							{
								int r;
								Int32.TryParse(m.Element("Count_6-2009").Value, out r);
								return r;
							}),
							_20 = g.Sum(m =>
							{
								int r;
								Int32.TryParse(m.Element("Count_7-2009").Value, out r);
								return r;
							}),
							_21 = g.Sum(m =>
							{
								int r;
								Int32.TryParse(m.Element("Count_8-2009").Value, out r);
								return r;
							}),
							_22 = g.Sum(m =>
							{
								int r;
								Int32.TryParse(m.Element("Count_9-2009").Value, out r);
								return r;
							}),
							_23 = g.Sum(m =>
							{
								int r;
								Int32.TryParse(m.Element("Count_10-2009").Value, out r);
								return r;
							}),
							_24 = g.Sum(m =>
							{
								int r;
								Int32.TryParse(m.Element("Count_11-2009").Value, out r);
								return r;
							})
						};
				foreach (var month in months)
				{
					_months.Add(new ACOAssaultMonth("December 2007", month._1));
					_months.Add(new ACOAssaultMonth("January 2008", month._2));
					_months.Add(new ACOAssaultMonth("February 2008", month._3));
					_months.Add(new ACOAssaultMonth("March 2008", month._4));
					_months.Add(new ACOAssaultMonth("April 2008", month._5));
					_months.Add(new ACOAssaultMonth("May 2008", month._6));
					_months.Add(new ACOAssaultMonth("June 2008", month._7));
					_months.Add(new ACOAssaultMonth("July 2008", month._8));
					_months.Add(new ACOAssaultMonth("August 2008", month._9));
					_months.Add(new ACOAssaultMonth("September 2008", month._10));
					_months.Add(new ACOAssaultMonth("October 2008", month._11));
					_months.Add(new ACOAssaultMonth("November 2008", month._12));
					_months.Add(new ACOAssaultMonth("December 2008", month._13));
					_months.Add(new ACOAssaultMonth("January 2009", month._14));
					_months.Add(new ACOAssaultMonth("February 2009", month._15));
					_months.Add(new ACOAssaultMonth("March 2009", month._16));
					_months.Add(new ACOAssaultMonth("April 2009", month._17));
					_months.Add(new ACOAssaultMonth("May 2009", month._18));
					_months.Add(new ACOAssaultMonth("June 2009", month._19));
					_months.Add(new ACOAssaultMonth("July 2009", month._20));
					_months.Add(new ACOAssaultMonth("August 2009", month._21));
					_months.Add(new ACOAssaultMonth("September 2009", month._22));
					_months.Add(new ACOAssaultMonth("October 2009", month._23));
					_months.Add(new ACOAssaultMonth("November 2009", month._24));
				}
			}
			catch (Exception ex)
			{
			    _errorMessage = ex.Message;
			    _months.Clear();
			}
		}

		public List<ACOAssaultMonth> Months { get { return _months; } }
		public string ErrorMessage { get { return _errorMessage; } }

	}
}
