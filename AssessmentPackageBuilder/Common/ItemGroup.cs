using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using FixedFormPackager.Common.Models.Csv;

namespace AssessmentPackageBuilder.Common
{
    public static class ItemGroup
    {
        public static IEnumerable<XElement> Construct(IList<Item> items, string partitionIdentifier)
        {
            return items.GroupBy(x => x.FormPartitionPosition).Select(x => new XElement("itemgroup",
                new XAttribute("formposition", "1"),
                new XAttribute("maxitems", "ALL"),
                new XAttribute("maxresponses", "ALL"),
                IdentifierAndResources(x.ToList(), partitionIdentifier)));
        }

        private static IEnumerable<XElement> IdentifierAndResources(IList<Item> items, string partitionIdentifier)
        {
            var fullItemId = items.First().ItemId.Contains('-') ? items.First().ItemId : $"187-{items.First().ItemId}";
            var result = new List<XElement>();
            if (!string.IsNullOrEmpty(items.First().AssociatedStimuliId))
            {
                result.Add(new XElement("passageref", items.First().AssociatedStimuliId));
                result.Add(new XElement("identifier",
                    new XAttribute("uniqueid", $"{partitionIdentifier}:G-{items.First().AssociatedStimuliId}"),
                    new XAttribute("name", $"{partitionIdentifier}:G-{items.First().AssociatedStimuliId}"),
                    new XAttribute("version", "1")));
            }
            else
            {
                result.Add(new XElement("identifier",
                    new XAttribute("uniqueid", $"{partitionIdentifier}:I-{fullItemId}"),
                    new XAttribute("name", $"{partitionIdentifier}:I-{fullItemId}"),
                    new XAttribute("version", "1")));
            }
            result.AddRange(items.Select(x => new XElement("groupitem",
                new XAttribute("itemid", x.ItemId.Contains('-') ? x.ItemId : $"187-{x.ItemId}"),
                new XAttribute("formposition", x.FormPosition),
                new XAttribute("groupposition", x.FormPosition),
                new XAttribute("adminrequired", "false"),
                new XAttribute("responserequired", "false"),
                new XAttribute("isactive", "true"),
                new XAttribute("isfieldtest", "false"),
                new XAttribute("blockid", "A"))));
            return result;
        }
    }
}