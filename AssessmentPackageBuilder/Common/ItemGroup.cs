using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using FixedFormPackager.Common.Models.Csv;
using NLog;

namespace AssessmentPackageBuilder.Common
{
    public static class ItemGroup
    {
        //private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static IEnumerable<XElement> Construct(IList<Item> items, string partitionIdentifier, int index)
        {
            return items.GroupBy(x => x.FormPartitionPosition).Select(x => new XElement("itemgroup",
                new XAttribute("formposition", index.ToString()),
                new XAttribute("maxitems", "ALL"),
                new XAttribute("maxresponses", "ALL"),
                IdentifierAndResources(x.ToList(), partitionIdentifier)));
        }

        private static IEnumerable<XElement> IdentifierAndResources(IList<Item> items, string partitionIdentifier)
        {
            var fullItemId = items.First().ItemId.Contains('-') ? items.First().ItemId : $"200-{items.First().ItemId}";
            var bank = fullItemId.Split('-').First();
            var stimuliId = items.First().AssociatedStimuliId.Contains(bank)
                ? items.First().AssociatedStimuliId
                : bank + "-" + items.First().AssociatedStimuliId;
            var result = new List<XElement>();
            if (!string.IsNullOrEmpty(items.First().AssociatedStimuliId))
            {
                result.Add(new XElement("identifier",
                    new XAttribute("uniqueid", $"{partitionIdentifier}:G-{stimuliId}-1"),
                    new XAttribute("name", $"{partitionIdentifier}:G-{stimuliId}-1"),
                    new XAttribute("version", "1")));
                result.Add(new XElement("passageref", stimuliId));

            }
            else
            {
                result.Add(new XElement("identifier",
                    new XAttribute("uniqueid", $"{partitionIdentifier}:I-{fullItemId}"),
                    new XAttribute("name", $"{partitionIdentifier}:I-{fullItemId}"),
                    new XAttribute("version", "1")));
            }
            result.AddRange(items.Select((x, i) => new XElement("groupitem",
                new XAttribute("itemid", x.ItemId.Contains('-') ? x.ItemId : $"200-{x.ItemId}"),
                new XAttribute("formposition", x.FormPosition),
                new XAttribute("groupposition", i +1),
                new XAttribute("adminrequired", "false"),
                new XAttribute("responserequired", "false"),
                new XAttribute("isactive", "true"),
                new XAttribute("isfieldtest", "false"),
                new XAttribute("blockid", "A"))));
            return result;
        }
    }
}