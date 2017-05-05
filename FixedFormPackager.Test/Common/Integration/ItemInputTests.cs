using System;
using System.IO;
using System.Linq;
using FixedFormPackager.Common.Utilities;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace FixedFormPackager.Test.Common.Integration
{
    [TestFixture]
    public class ItemInputTests
    {
        public static readonly string ResourcesDirectory =
            Path.Combine(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.FullName, "Resources");

        [Test]
        public void BadFilePathShouldFail()
        {
            // Arrange
            const string filePath = "randomfilepath";

            //Act
            ActualValueDelegate<object> testDelegate = () => CsvExtractor.ExtractItemInput(filePath);

            //Assert
            Assert.That(testDelegate, Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void CorrectFilePathWithCorrectExtensionAndHeadersShouldSucceed()
        {
            // Arrange
            var filePath = Path.Combine(ResourcesDirectory, "FFP_Sample.csv");

            // Act
            var result = CsvExtractor.ExtractItemInput(filePath);

            // Assert
            Assert.IsTrue(result.Any());
            Assert.AreEqual(result.Count, 5);
            Assert.AreEqual(result.First().ItemId, "187-300");
            Assert.AreEqual(result.Last().FormPartitionId, "187-561");
            Assert.IsTrue(result.Count(x => string.IsNullOrEmpty(x.AssociatedStimuliId)) == 1);
        }

        [Test]
        public void CorrectFilePathWithCorrectExtensionAndIncorrectHeadersShouldFail()
        {
            // Arrange
            var filePath = Path.Combine(ResourcesDirectory, "BAD-FFP_Sample.csv");

            //Act
            ActualValueDelegate<object> testDelegate = () => CsvExtractor.ExtractItemInput(filePath);

            //Assert
            Assert.That(testDelegate, Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void CorrectFilePathWithIncorrectExtensionShouldFail()
        {
            // Arrange
            var filePath = Path.Combine(ResourcesDirectory, "test.txt");

            //Act
            ActualValueDelegate<object> testDelegate = () => CsvExtractor.ExtractItemInput(filePath);

            //Assert
            Assert.That(testDelegate, Throws.TypeOf<ArgumentException>());
        }
    }
}