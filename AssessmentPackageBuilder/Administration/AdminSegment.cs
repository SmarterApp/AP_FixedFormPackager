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
                    GetSegmentForms(x.ToList())
                    ));
        }

        
        private static IEnumerable<XElement> GetSegmentForms(IList<Item> items)
        {
            var result = new List<XElement>();
            var segforms = items.GroupBy(i => i.FormPartitionId).Select(j => j.First()).ToList();
            foreach (var segform in segforms)
            {
                result.Add(new XElement("segmentform",
                    new XAttribute("formpartitionid", segform.FormPartitionId)));
            }
            return result;
        }
        

        private static IEnumerable<string> GetBprefsForItemId(XNode itempool, string uniqueId)
        {
            try
            {
                return
                    itempool.XPathSelectElement($"./testitem[identifier/@uniqueid='{uniqueId}']")
                        .XPathSelectElements("./bpref")
                        .Select(x => x.Value);
            }
            catch (Exception) // If there was no metadata file provided with the items packaged, return an empty list
            {
                return new List<string>();
            }
        }
    }
}