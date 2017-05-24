using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using FixedFormPackager.Common.Extensions;
using FixedFormPackager.Common.Models;
using FixedFormPackager.Common.Models.Csv;
using FixedFormPackager.Common.Utilities.CsvMappers;
using NLog;

namespace FixedFormPackager.Common.Utilities
{
    public static class CsvExtractor
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static IList<T> Extract<T>(string fileName)
        {
            try
            {
                ValidateFile(fileName);
                var csvReader = new CsvReader(new StreamReader(File.OpenRead(fileName)), new CsvConfiguration
                {
                    HasHeaderRecord = true
                });

                // The mappers are necessary for these types to support mapping from flat CSV files to hierarchal objects
                if (typeof(T) == typeof(Item))
                {
                    Logger.Debug($"File {fileName} being processed as an Item using the ItemInputMapper");
                    csvReader.Configuration.RegisterClassMap(new ItemInputMapper());
                }
                else if (typeof(T) == typeof(AssessmentScoringComputationRule))
                {
                    Logger.Debug($"File {fileName} being processed as an AssessmentScoringComputationRule using the AssessmentScoringMapper");
                    csvReader.Configuration.RegisterClassMap(new AssessmentScoringMapper());
                }
                else
                {
                    Logger.Debug($"File {fileName} being processed as a {typeof(T).FullName} using the Generic 1 <-> 1 Mapper");
                    // Items will be mapped to an object where CSV header row == object property name. Mismatches will throw
                    var itemInputProperties = typeof(T).GetProperties().Select(x => x.Name).ToList();
                    csvReader.ReadHeader();
                    var csvInputHeaders = csvReader.FieldHeaders;
                    if (csvInputHeaders.Except(itemInputProperties).Any() ||
                        itemInputProperties.Except(csvInputHeaders).Any())
                    {
                        throw new ArgumentException(
                            $"FATAL - Input file {fileName} headers: [{csvInputHeaders.Aggregate((x, y) => $"{x},{y}")}] " +
                            $"do not match ItemInput required properties: [{itemInputProperties.Aggregate((x, y) => $"{x},{y}")}]");
                    }
                }
                return csvReader.GetRecords<T>().ToList();
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