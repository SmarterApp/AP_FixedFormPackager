using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using FixedFormPackager.Common.Models.Csv;

namespace AssessmentPackageBuilder.Administration
{
    public static class AdminSegment
    {
        public static IEnumerable<XElement> Construct(XElement itemPool, IList<Item> items)
        {
            var testItems = itemPool.XPathSelectElements("testitem");
            return items.GroupBy(x => x.SegmentPosition)
                .Select(x => new XElement("adminsegment",
                    new XAttribute("segmentid", x.First().SegmentId),
                    new XAttribute("position", x.Key),
                    new XAttribute("itemselection", "fixedform"),
                    // TODO: Investigate strange counts on segmentbpelements
                    new XElement("segmentblueprint",
                        testItems.Where(y => x.Select(z => z.ItemId)
                                .Contains(y.XPathSelectElement("identifier")
                                    .Attribute("uniqueid").Value))
                            .SelectMany(y => y.XPathSelectElements("//bpref"))
                            .Where(y => !items.Any(z => z.SegmentId.Equals(y.Value, StringComparison.OrdinalIgnoreCase)))
                            .GroupBy(y => y.Value)
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
                    new XElement("segmentform",
                        new XAttribute("formpartitionid", x.First().FormPartitionId))));
        }
    }
}