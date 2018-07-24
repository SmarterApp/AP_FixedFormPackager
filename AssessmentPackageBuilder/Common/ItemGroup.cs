using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using FixedFormPackager.Common.Models.Csv;
using FixedFormPackager.Common.Utilities;
using NLog;

namespace AssessmentPackageBuilder.Common
{
    public static class ItemGroup
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static IEnumerable<XElement> Construct(IList<Item> items, string partitionIdentifier, int index)
        {
            var result = new List<XElement>();
            var firstItem = items.First();
            var referenceItem = ExtractionSettings.ItemInput.FirstOrDefault(x => x.ItemId == firstItem.ItemId);

            // this is a G: or group
            if (!string.IsNullOrEmpty(firstItem.AssociatedStimuliId) || !string.IsNullOrEmpty(referenceItem?.AssociatedStimuliId))
            {
                var stimId = !string.IsNullOrEmpty(firstItem.AssociatedStimuliId)
                    ? firstItem.AssociatedStimuliId
                    : referenceItem?.AssociatedStimuliId;
                return items.GroupBy(x => x.FormPartitionPosition).Select(x => new XElement("itemgroup",
                    new XAttribute("formposition", index.ToString()),
                    new XAttribute("maxitems", "ALL"),
                    new XAttribute("maxresponses", "ALL"),
                    IdentifierAndResources(x.ToList(), partitionIdentifier, stimId)));
            }
            else // I: individual groupitem per itemgroup
            {
                foreach (var item in items)
                {
                    var itemList = new List<Item>{item};
                    result.Add(new XElement("itemgroup",
                        new XAttribute("formposition", item.FormPosition),
                        new XAttribute("maxitems", "ALL"),
                        new XAttribute("maxresponses", "ALL"),
                        IdentifierAndResources(itemList, partitionIdentifier, "")));
                }
            }
            return result;
        }

        private static IEnumerable<XElement> IdentifierAndResources(IList<Item> items, string partitionIdentifier, string assocaitaedStimuliId)
        {
            var fullItemId = items.First().ItemId.Contains('-') ? items.First().ItemId : $"200-{items.First().ItemId}";
            var bank = fullItemId.Split('-').First();
            var stimuliId = assocaitaedStimuliId.Contains(bank)
                ? assocaitaedStimuliId
                : bank + "-" + assocaitaedStimuliId;
            var result = new List<XElement>();
            if (!string.IsNullOrEmpty(assocaitaedStimuliId))
            {
                result.Add(new XElement("identifier",
                    new XAttribute("uniqueid", $"{partitionIdentifier}:G-{stimuliId}-1"),
                    new XAttribute("name", $"{partitionIdentifier}:G-{stimuliId}-1"),
                    new XAttribute("version", "1")));
                result.Add(new XElement("passageref", stimuliId));
                result.AddRange(items.Select((x, i) => new XElement("groupitem",
                    new XAttribute("itemid", x.ItemId.Contains('-') ? x.ItemId : $"200-{x.ItemId}"),
                    new XAttribute("formposition", x.FormPosition),
                    new XAttribute("groupposition", i + 1),
                    new XAttribute("adminrequired", "false"),
                    new XAttribute("responserequired", "false"),
                    new XAttribute("isactive", "true"),
                    new XAttribute("isfieldtest", "false"),
                    new XAttribute("blockid", "A"))));
            }
            else
            {
                result.Add(new XElement("identifier",
                    new XAttribute("uniqueid", $"{partitionIdentifier}:I-{fullItemId}"),
                    new XAttribute("name", $"{partitionIdentifier}:I-{fullItemId}"),
                    new XAttribute("version", "1")));
                foreach (var x in items)
                {
                    result.Add(new XElement("groupitem",
                        new XAttribute("itemid", x.ItemId.Contains('-') ? x.ItemId : $"200-{x.ItemId}"),
                        new XAttribute("formposition", x.FormPosition),
                        new XAttribute("groupposition", 1),
                        new XAttribute("adminrequired", "false"),
                        new XAttribute("responserequired", "false"),
                        new XAttribute("isactive", "true"),
                        new XAttribute("isfieldtest", "false"),
                        new XAttribute("blockid", "A")
                        ));
                }
            }
           
            return result;
        }
    }
}