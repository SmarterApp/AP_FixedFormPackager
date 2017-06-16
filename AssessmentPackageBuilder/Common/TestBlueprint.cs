using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using FixedFormPackager.Common.Models.Csv;

namespace AssessmentPackageBuilder.Common
{
    public static class TestBlueprint
    {
        public static XElement Construct(IEnumerable<Item> items, XElement itemPool, string assessmentId)
        {
            var result = new XElement("testblueprint");
            var bprefs = itemPool.XPathSelectElements("//bpref")
                .GroupBy(x => x.Value)
                .ToDictionary(x => x.Key, x => x.Count())
                .GroupBy(x => items.Select(y => y.SegmentId).Contains(x.Key)).ToList();
            var segments =
                bprefs.First(x => x.Key).Select(x => BpElement.Construct("segment", x.Value.ToString(), x.Key));
            var strands =
                bprefs.FirstOrDefault(x => !x.Key)?
                    .Where(x => !x.Key.Contains('|'))
                    .Select(x => BpElement.Construct("strand", x.Value.ToString(), x.Key));
            var contentLevels = bprefs.FirstOrDefault(x => !x.Key)?
                .Where(x => x.Key.Contains('|'))
                .Select(x => BpElement.Construct("contentlevel", x.Value.ToString(), x.Key));
            result.Add(BpElement.Construct("test", bprefs.First(x => x.Key).Sum(x => x.Value).ToString(), assessmentId));
            result.Add(segments);
            result.Add(strands);
            result.Add(contentLevels);
            return result;
        }
    }
}