using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Xunit.Extensions;

namespace ScLib
{
    public class ACOAssaultsByLinqTests
    {
        [Theory]
        [InlineData("Southwark", "Top 5", 5)]
        [InlineData("Southwark", "200 300", 6)]
        [InlineData("City", "bottom 3", 17)]
        public void CheckVarious(string borough, string frequency, int expected )
        {
            ACOAssaultsByLinq finder = new ACOAssaultsByLinq(TestData.XmlFileName);
            //ACOAssaultWard[] wards = finder.Find(borough, frequency);
            //Console.WriteLine(borough + "/" + frequency + " expected " + expected);
            //foreach (ACOAssaultWard w in wards)
            //    Console.WriteLine(w.WardName + " (" + w.DistrictName + ") " + w.Total);

            int actual = finder.Find(borough, frequency).Length;
            Assert.True(expected == actual, String.Format("{0} != {1} for {2} / {3}",
                expected, actual, borough, frequency));
        }
    }
}
