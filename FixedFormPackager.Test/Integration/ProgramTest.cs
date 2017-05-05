using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using FixedFormPackager.Test.Models;
using FixedFormPackager.Test.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FixedFormPackager.Test.Integration
{
    [TestClass]
    public class ProgramTest
    {
        private const string OutputName = "FFP_Test";
        // TODO: Swap out this placeholder value with the relative path to the output of the FFP
        private const string InputName = "C:\\Projects\\SmarterBalanced\\Resources\\DELETE\\STS-Limited-Perf-ELA-3.xml";

        [TestInitialize]
        public void Setup()
        {
            // Act
            ProcessUtility.LaunchExecutable(
                $"-i \"{InputName}\" -o \"{OutputName}\" -sv");
        }

        [TestMethod]
        public void TestForPresenceOfSevereErrors()
        {
            // Act
            var result = GetErrorList($"{Directory.GetCurrentDirectory()}\\{OutputName}.errors.csv");

            // Assert
            Assert.IsTrue(!result.Any(x => x.ErrorSeverity.Equals("Severe", StringComparison.OrdinalIgnoreCase)));
        }

        [TestMethod]
        public void TestForPresenceOfDegradedErrors()
        {
            // Act
            var result = GetErrorList($"{Directory.GetCurrentDirectory()}\\{OutputName}.errors.csv");

            // Assert
            Assert.IsTrue(!result.Any(x => x.ErrorSeverity.Equals("Degraded", StringComparison.OrdinalIgnoreCase)));
        }

        [TestMethod]
        public void TestForPresenceOfBenignErrors()
        {
            // Act
            var result = GetErrorList($"{Directory.GetCurrentDirectory()}\\{OutputName}.errors.csv");

            // Assert
            Assert.IsTrue(!result.Any(x => x.ErrorSeverity.Equals("Benign", StringComparison.OrdinalIgnoreCase)));
        }

        private static IEnumerable<TestPackageTabulatorError> GetErrorList(string fileName)
        {
            var csvReader = new CsvReader(new StreamReader(File.OpenRead(fileName)), new CsvConfiguration
            {
                HasHeaderRecord = true
            });
            return csvReader.GetRecords<TestPackageTabulatorError>().ToList();
        }
    }
}