using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using FixedFormPackager.Common.Models.Csv;
using FixedFormPackager.Common.Utilities;

namespace AssessmentPackageBuilder.Administration
{
    public static class AdminSegment
    {
        public static IEnumerable<XElement> Construct(XElement itemPool, IList<Item> items)
        {
            return items.GroupBy(x => x.SegmentPosition)
                .Select(x => new XElement("adminsegment",
                    new XAttribute("segmentid", x.First().SegmentId),
                    new XAttribute("position", x.Key),
                    new XAttribute("itemselection", "fixedform"),
                    new XElement("segmentblueprint",
                        x.SelectMany(y => GetBprefsForItemId(itemPool, y.ItemId))
                            .GroupBy(y => y)
                            .Select(y => SegmentBpElement.Construct(y.Key, y.Count().ToString()))),
                    new XElement("itemselector",
                        new XAttribute("type", "fixedform"),
                        new XElement("identifier",
                            new XAttribute("uniqueid", "AIR FIXEDFORM1"),
                            new XAttribute("name", "AIR FIXEDFORM"),
                            new XAttribute("label", "AIR FIXEDFORM"),
                            new XAttribute("version", "1.0")),
                        new XElement("itemselectionparameter",
                            new XAttribute("bpelementid", x.First().SegmentId),
                            new XElement("property",
                                new XAttribute("name", "slope"),
                                new XAttribute("value", "85.8"),
                                new XAttribute("label", "slope")),
                            new XElement("property",
                                new XAttribute("name", "intercept"),
                                new XAttribute("value", "2508.2"),
                                new XAttribute("label", "intercept")))),
                    GetSegmentForms(x.Key)
                    ));
        }

        // Foreach language : foreach formpartitionposition : find unique formpartitionid and make a segmentform
        private static IEnumerable<XElement> GetSegmentForms(string position)
        {
            var result = new List<XElement>();
            var stuffitems = new List<Item>();
            foreach (var lang in ExtractionSettings.ItemInputs)
            {
                stuffitems.AddRange(lang.Select(x => x).Where(item => item.FormPartitionPosition.Equals(position, StringComparison.OrdinalIgnoreCase)));
            }
            var ids = stuffitems.GroupBy(i => i.FormPartitionId).Select(j => j.First()).ToList();
            foreach (var i in ids)
            {
                result.Add(new XElement("segmentform",
                    new XAttribute("formpartitionid", i.FormPartitionId)));
            }
            return result;
        }
        

        private static IEnumerable<string> GetBprefsForItemId(XNode itempool, string uniqueId)
        {
            try
            {
                return
                    itempool.XPathSelectElement($"./testitem[identifier/@uniqueid='{uniqueId}']")
                        .XPathSelectElements("./bpref").Skip(1)
                        .Select(x => x.Value);
            }
            catch (Exception) // If there was no metadata file provided with the items packaged, return an empty list
            {
                return new List<string>();
            }
        }
    }
}