using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using FixedFormPackager.Common.Extensions;
using FixedFormPackager.Common.Models;
using NLog;

namespace FixedFormPackager.Common.Utilities
{
    public static class CsvExtractor
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static IList<ItemInput> ExtractTestItemInput(string fileName)
        {
            try
            {
                ValidateFile(fileName);
                var csvReader = new CsvReader(new StreamReader(File.OpenRead(fileName)), new CsvConfiguration
                {
                    HasHeaderRecord = true
                });
                var itemInputProperties = typeof(ItemInput).GetProperties().Select(x => x.Name).ToList();
                csvReader.ReadHeader();
                var csvInputHeaders = csvReader.FieldHeaders;
                if (csvInputHeaders.Except(itemInputProperties).Any() ||
                    itemInputProperties.Except(csvInputHeaders).Any())
                {
                    throw new ArgumentException(
                        $"FATAL - Input file {fileName} headers: [{csvInputHeaders.Aggregate((x, y) => $"{x},{y}")}] " +
                        $"do not match ItemInput required properties: [{itemInputProperties.Aggregate((x, y) => $"{x},{y}")}]");
                }
                return csvReader.GetRecords<ItemInput>().ToList();
            }
            catch (Exception ex)
            {
                Logger.LogError(new ErrorReportItem
                {
                    Location = "CSV Extraction",
                    Severity = LogLevel.Fatal
                }, ex.Message);
                throw;
            }
        }

        private static void ValidateFile(string fileName)
        {
            if (!File.Exists(fileName))
            {
                throw new ArgumentException($"FATAL - Input file {fileName} could not be found.");
            }
            var extension = Path.GetExtension(fileName);
            if (extension == null || !extension.Equals(".csv"))
            {
                throw new ArgumentException(
                    $"FATAL - Input file {fileName} is not in an acceptable format. Allowed formats are [.csv]");
            }
        }
    }
}