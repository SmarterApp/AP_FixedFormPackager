using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using FixedFormPackager.Test.Models;
using FixedFormPackager.Test.Utilities;
using NUnit.Framework;

namespace FixedFormPackager.Test.Integration
{
    [TestFixture]
    public class ProgramTest
    {
        [SetUp]
        public void Setup()
        {
            // Act
            ProcessUtility.LaunchExecutable(
                $"-i \"{InputName}\" -o \"{OutputName}\" -sv");
        }

        private const string OutputName = "FFP_Test";
        // TODO: Swap out this placeholder value with the relative path to the output of the FFP
        private const string InputName = "C:\\Projects\\SmarterBalanced\\Resources\\DELETE\\STS-Limited-Perf-ELA-3.xml";
        public static readonly string ExecutionDirectory = AppDomain.CurrentDomain.BaseDirectory;

        private static IEnumerable<TestPackageTabulatorError> GetErrorList(string fileName)
        {
            var csvReader = new CsvReader(new StreamReader(File.OpenRead(fileName)), new CsvConfiguration
            {
                HasHeaderRecord = true
            });
            return csvReader.GetRecords<TestPackageTabulatorError>().ToList();
        }

        [Test]
        public void TestForPresenceOfBenignErrors()
        {
            // Act
            var result = GetErrorList($"{ExecutionDirectory}\\{OutputName}.errors.csv");

            // Assert
            Assert.IsTrue(!result.Any(x => x.ErrorSeverity.Equals("Benign", StringComparison.OrdinalIgnoreCase)));
        }

        [Test]
        public void TestForPresenceOfDegradedErrors()
        {
            // Act
            var result = GetErrorList($"{ExecutionDirectory}\\{OutputName}.errors.csv");

            // Assert
            Assert.IsTrue(!result.Any(x => x.ErrorSeverity.Equals("Degraded", StringComparison.OrdinalIgnoreCase)));
        }

        [Test]
        public void TestForPresenceOfSevereErrors()
        {
            // Act
            var result = GetErrorList($"{ExecutionDirectory}\\{OutputName}.errors.csv");

            // Assert
            Assert.IsTrue(!result.Any(x => x.ErrorSeverity.Equals("Severe", StringComparison.OrdinalIgnoreCase)));
        }
    }
}