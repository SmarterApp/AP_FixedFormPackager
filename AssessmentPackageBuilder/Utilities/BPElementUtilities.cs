using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using FixedFormPackager.Common.Extensions;
using FixedFormPackager.Common.Models;
using NLog;

namespace AssessmentPackageBuilder.Utilities
{
    public static class BpElementUtilities
    {
        private const string Pattern = @"^(\w+)-\w*-v(\d):";
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static List<XElement> GetBprefs(string input)
        {
            var barrierCount = input.Count(x => x.Equals('|'));
            var pattern = Pattern;
            for (var i = 0; i <= barrierCount; i++)
            {
                pattern = $"{pattern}(?>(\\w+-*\\w*)+\\|*)+";
            }
            pattern = pattern + "$";
            var matches = Regex.Matches(input, pattern)
                .Cast<Match>().ToList();
            var result = new List<XElement>();
            if (!matches.Any() || !matches.TrueForAll(x => x.Success))
            {
                Logger.LogError(new ErrorReportItem
                {
                    Location = "bpref generation",
                    Severity = LogLevel.Warn
                }, $"Unparsable input {input} passed to bpref generator");
                return result;
            }
            var standard = new XElement("bpref", $"{matches.First().Groups[1]}-{matches.First().Groups[2]}");
            result.Add(standard);
            for (var i = 3; i < matches.First().Groups.Count; i++)
            {
                var bprefBuilder = standard.Value;
                for (var j = 3; j <= i; j++)
                {
                    bprefBuilder = $"{bprefBuilder}|{matches.First().Groups[j]}";
                }
                result.Add(new XElement("bpref", bprefBuilder));
            }
            return result;
        }
    }
}