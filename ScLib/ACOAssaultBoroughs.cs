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


        public static List<ACOAssaultBorough> ReadInclMonths(string xmlfilename)
        {
            List<ACOAssaultBorough> results = new List<ACOAssaultBorough>();
            try
            {
                var wards = from b in XDocument.Load(xmlfilename).Descendants("ROW")
                            orderby (string)b.Element("District_Name")
                            select new ACOAssaultWard(b);
                foreach (ACOAssaultWard w in wards)
                {
                    if (results.Count == 0)
                        results.Add(new ACOAssaultBorough(w.DistrictName, 0, 0));
                    else if (results[results.Count - 1].Borough != w.DistrictName)
                        results.Add(new ACOAssaultBorough(w.DistrictName, 0, 0));
                    ACOAssaultBorough boro = results[results.Count - 1];
                    boro.IncrementAllMonths(w);
                }
                return results;
            }
            catch
            {
                results.Clear();
                return results;
            }
        }
        public static List<ACOAssaultBorough> ReadInclMonthsClunky(string xmlfilename)
        {
            List<ACOAssaultBorough> results = new List<ACOAssaultBorough>();
            try
            {
                var wards = from b in XDocument.Load(xmlfilename).Descendants("ROW")
                            orderby (string)b.Element("District_Name")
                            select new
                            {
                                borough = b.Element("District_Name").Value,
                                m0 = b.Element( "Count_12-2007" ).Value,
                                m1 = b.Element("Count_1-2008").Value,
                                m2 = b.Element("Count_2-2008").Value,
                                m3 = b.Element("Count_3-2008").Value,
                                m4 = b.Element("Count_4-2008").Value,
                                m5 = b.Element("Count_5-2008").Value,
                                m6 = b.Element("Count_6-2008").Value,
                            };
                foreach (var w in wards)
                {
                    if( results.Count == 0 )
                        results.Add( new ACOAssaultBorough( w.borough, 0, 0 ) );
                    else if( results[results.Count - 1 ].Borough != w.borough )
                        results.Add( new ACOAssaultBorough( w.borough, 0, 0 ) );
                    ACOAssaultBorough boro = results[results.Count - 1];
                    boro.IncrementMonth(0, ACOAssaultsByLinq.MyIntParse(w.m0));
                    boro.IncrementMonth(1, ACOAssaultsByLinq.MyIntParse(w.m1));
                    boro.IncrementMonth(2, ACOAssaultsByLinq.MyIntParse(w.m2));
                    boro.IncrementMonth(3, ACOAssaultsByLinq.MyIntParse(w.m3));
                    boro.IncrementMonth(4, ACOAssaultsByLinq.MyIntParse(w.m4));
                    boro.IncrementMonth(5, ACOAssaultsByLinq.MyIntParse(w.m5));
                    boro.IncrementMonth(6, ACOAssaultsByLinq.MyIntParse(w.m6));

                }


                return results;
            }
            catch
            {
                results.Clear();
                return results;
            }
        }
    }
}
