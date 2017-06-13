using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using NLog;

namespace AssessmentPackageBuilder.Utilities
{
    public static class BpElementUtilities
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static List<XElement> GetBprefs(string input, string publisher)
        {
            var result = new List<XElement>();
            if (!input.Contains(":"))
            {
                return result;
            }
            var sections = input.Split(':').LastOrDefault()?.Split('|');
            for (var i = 0; i < sections.Length; i++)
            {
                result.Add(i == 0
                    ? new XElement("bpref", $"{publisher}-{sections[i]}")
                    : new XElement("bpref",
                        $"{publisher}-{sections.Take(result.Count).Aggregate((x, y) => $"{x}|{y}")}|{sections[i]}"));
            }
            return result;
        }
    }
}