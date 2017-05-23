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

            // Act
            var result = BpElementUtilities.GetBprefs(input);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsEmpty(result);
            ;
        }

        [Test]
        public void GetBprefsGoodStandardNoPipesLongWordReturnsValidGroups()
        {
            // Arrange
            const string input = "SBAC-SH-v1:SH-Undesignated";

            // Act
            var result = BpElementUtilities.GetBprefs(input);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
            Assert.AreEqual(result.Count, 2);
            Assert.AreEqual(result[0].Name.LocalName, "bpref");
            Assert.IsTrue(result[0].Value.Equals("SBAC-1", StringComparison.OrdinalIgnoreCase));
            Assert.IsTrue(result[1].Value.Equals("SBAC-1|SH-Undesignated", StringComparison.OrdinalIgnoreCase));
        }

        [Test]
        public void GetBprefsGoodStandardNoPipesReturnsValidGroups()
        {
            // Arrange
            const string input = "SBAC-ELA-v1:2-W";

            // Act
            var result = BpElementUtilities.GetBprefs(input);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
            Assert.AreEqual(result.Count, 2);
            Assert.AreEqual(result[0].Name.LocalName, "bpref");
            Assert.IsTrue(result[0].Value.Equals("SBAC-1", StringComparison.OrdinalIgnoreCase));
            Assert.IsTrue(result[1].Value.Equals("SBAC-1|2-W", StringComparison.OrdinalIgnoreCase));
        }

        [Test]
        public void GetBprefsGoodStandardReturnsValidGroups()
        {
            // Arrange
            const string input = "SBAC-ELA-v1:2-W|1-3";

            // Act
            var result = BpElementUtilities.GetBprefs(input);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
            Assert.AreEqual(result.Count, 3);
            Assert.AreEqual(result[0].Name.LocalName, "bpref");
            Assert.IsTrue(result[0].Value.Equals("SBAC-1", StringComparison.OrdinalIgnoreCase));
            Assert.IsTrue(result[1].Value.Equals("SBAC-1|2-W", StringComparison.OrdinalIgnoreCase));
            Assert.IsTrue(result[2].Value.Equals("SBAC-1|2-W|1-3", StringComparison.OrdinalIgnoreCase));
        }

        [Test]
        public void GetBprefsGoodStandardWithSinglesReturnsValidGroups()
        {
            // Arrange
            const string input = "SBAC-ELA-v1:2-W|1-3|G|6|R2D2|L-5";

            // Act
            var result = BpElementUtilities.GetBprefs(input);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
            Assert.AreEqual(result.Count, 7);
            Assert.IsTrue(result[0].Value.Equals("SBAC-1", StringComparison.OrdinalIgnoreCase));
            Assert.IsTrue(result[1].Value.Equals("SBAC-1|2-W", StringComparison.OrdinalIgnoreCase));
            Assert.IsTrue(result[2].Value.Equals("SBAC-1|2-W|1-3", StringComparison.OrdinalIgnoreCase));
            Assert.IsTrue(result[3].Value.Equals("SBAC-1|2-W|1-3|G", StringComparison.OrdinalIgnoreCase));
            Assert.IsTrue(result[4].Value.Equals("SBAC-1|2-W|1-3|G|6", StringComparison.OrdinalIgnoreCase));
            Assert.IsTrue(result[5].Value.Equals("SBAC-1|2-W|1-3|G|6|R2D2", StringComparison.OrdinalIgnoreCase));
            Assert.IsTrue(result[6].Value.Equals("SBAC-1|2-W|1-3|G|6|R2D2|L-5", StringComparison.OrdinalIgnoreCase));
        }
    }
}