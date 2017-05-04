using System;
using System.IO;
using System.Linq;
using FixedFormPackager.Common.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FixedFormPackager.Test.Integration
{
    [TestClass]
    public class ItemInputTests
    {
        [TestMethod]
        public void CorrectFilePathWithCorrectExtensionAndHeadersShouldSucceed()
        {
            // Arrange
            var filePath = Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.FullName, "Resources",
                "FFP_Sample.csv");

            // Act
            var result = CsvExtractor.ExtractTestItemInput(filePath);

            // Assert
            Assert.IsTrue(result.Any());
            Assert.AreEqual(result.Count, 5);
            Assert.AreEqual(result.First().ItemId, "187-300");
            Assert.AreEqual(result.Last().FormPartitionId, "187-561");
            Assert.IsTrue(result.Count(x => string.IsNullOrEmpty(x.AssociatedStimuliId)) == 1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void BadFilePathShouldFail()
        {
            // Arrange
            const string filePath = "randomfilepath";

            // Act
            CsvExtractor.ExtractTestItemInput(filePath);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CorrectFilePathWithIncorrectExtensionShouldFail()
        {
            // Arrange
            var filePath = Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.FullName, "Resources",
                "test.txt");

            // Act
            CsvExtractor.ExtractTestItemInput(filePath);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CorrectFilePathWithCorrectExtensionAndIncorrectHeadersShouldFail()
        {
            // Arrange
            var filePath = Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.FullName, "Resources",
                "BAD-FFP_Sample.csv");

            // Act
            CsvExtractor.ExtractTestItemInput(filePath);
        }
    }
}