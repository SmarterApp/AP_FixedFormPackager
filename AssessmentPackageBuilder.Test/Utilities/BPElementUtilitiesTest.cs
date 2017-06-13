using System;
using AssessmentPackageBuilder.Utilities;
using NUnit.Framework;

namespace AssessmentPackageBuilder.Test.Utilities
{
    [TestFixture]
    public class BpElementUtilitiesTest
    {

        [Test]
        public void GetBprefsBadReturnsEmptyList()
        {
            // Arrange
            const string input = "wormhole";
            const string publisher = "SBAC_PT";

            // Act
            var result = BpElementUtilities.GetBprefs(input, publisher);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsEmpty(result);
        }

        [Test]
        public void GetBprefsGoodStandardNoPipesLongWordReturnsValidGroups()
        {
            // Arrange
            const string input = "SBAC-SH-v1:SH-Undesignated";
            const string publisher = "SBAC_PT";

            // Act
            var result = BpElementUtilities.GetBprefs(input, publisher);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
            Assert.AreEqual(result.Count, 1);
            Assert.AreEqual(result[0].Name.LocalName, "bpref");
            Assert.IsTrue(result[0].Value.Equals("SBAC_PT-SH-Undesignated", StringComparison.OrdinalIgnoreCase));
        }

        [Test]
        public void GetBprefsGoodStandardNoPipesReturnsValidGroups()
        {
            // Arrange
            const string input = "SBAC-ELA-v1:2-W";
            const string publisher = "SBAC_PT";

            // Act
            var result = BpElementUtilities.GetBprefs(input, publisher);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
            Assert.AreEqual(result.Count, 1);
            Assert.AreEqual(result[0].Name.LocalName, "bpref");
            Assert.IsTrue(result[0].Value.Equals("SBAC_PT-2-W", StringComparison.OrdinalIgnoreCase));
        }

        [Test]
        public void GetBprefsGoodStandardReturnsValidGroups()
        {
            // Arrange
            const string input = "SBAC-ELA-v1:2-W|1-3";
            const string publisher = "SBAC_PT";

            // Act
            var result = BpElementUtilities.GetBprefs(input, publisher);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
            Assert.AreEqual(result.Count, 2);
            Assert.AreEqual(result[0].Name.LocalName, "bpref");
            Assert.IsTrue(result[0].Value.Equals("SBAC_PT-2-W", StringComparison.OrdinalIgnoreCase));
            Assert.IsTrue(result[1].Value.Equals("SBAC_PT-2-W|1-3", StringComparison.OrdinalIgnoreCase));
        }

        [Test]
        public void GetBprefsGoodStandardWithSinglesReturnsValidGroups()
        {
            // Arrange
            const string input = "SBAC-ELA-v1:2-W|1-3|G|6|R2D2|L-5";
            const string publisher = "SBAC_PT";

            // Act
            var result = BpElementUtilities.GetBprefs(input, publisher);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
            Assert.AreEqual(result.Count, 6);
            Assert.IsTrue(result[0].Value.Equals("SBAC_PT-2-W", StringComparison.OrdinalIgnoreCase));
            Assert.IsTrue(result[1].Value.Equals("SBAC_PT-2-W|1-3", StringComparison.OrdinalIgnoreCase));
            Assert.IsTrue(result[2].Value.Equals("SBAC_PT-2-W|1-3|G", StringComparison.OrdinalIgnoreCase));
            Assert.IsTrue(result[3].Value.Equals("SBAC_PT-2-W|1-3|G|6", StringComparison.OrdinalIgnoreCase));
            Assert.IsTrue(result[4].Value.Equals("SBAC_PT-2-W|1-3|G|6|R2D2", StringComparison.OrdinalIgnoreCase));
            Assert.IsTrue(result[5].Value.Equals("SBAC_PT-2-W|1-3|G|6|R2D2|L-5", StringComparison.OrdinalIgnoreCase));
        }

        [Test]
        public void GetBprefsGoodStandardWithDotsReturnsValidGroups()
        {
            // Arrange
            const string input = "SBAC-ELA-v1:1-LT|7-3|3.L.5a";
            const string publisher = "SBAC_PT";

            // Act
            var result = BpElementUtilities.GetBprefs(input, publisher);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
            Assert.AreEqual(result.Count, 3);
            Assert.IsTrue(result[0].Value.Equals("SBAC_PT-1-LT", StringComparison.OrdinalIgnoreCase));
            Assert.IsTrue(result[1].Value.Equals("SBAC_PT-1-LT|7-3", StringComparison.OrdinalIgnoreCase));
            Assert.IsTrue(result[2].Value.Equals("SBAC_PT-1-LT|7-3|3.L.5a", StringComparison.OrdinalIgnoreCase));
        }
    }
}