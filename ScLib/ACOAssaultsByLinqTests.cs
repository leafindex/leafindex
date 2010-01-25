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
        public const string FILENAME = @"C:\CSharp\LeafIndex\DataSets\ambulance-all-assaults-ward.xml";

        [Theory]
        [InlineData("Southwark", "Top 5", 5)]
        [InlineData("City", "bottom 3", 17)]
        public void CheckVarious(string borough, string frequency, int expected )
        {
            ACOAssaultFinder finder = new ACOAssaultFinder(FILENAME);
            int actual = finder.Find(borough, frequency).Length;
            Assert.True(expected == actual, String.Format("{0} != {1} for {2} / {3}",
                expected, actual, borough, frequency));

        }
    }
}
