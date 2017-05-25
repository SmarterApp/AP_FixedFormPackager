using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using FixedFormPackager.Common.Models.Csv;
using FixedFormPackager.Common.Utilities;
using ItemRetriever.Utilities;

namespace AssessmentPackageBuilder.Common
{
    public static class ItemPool
    {
        public static XElement Construct(IEnumerable<Item> itemInput)
        {
            var result = new XElement("itempool");
            result.Add(ExtractionSettings.ItemInput.Select(
                    x => new {Content = ContentAccess.RetrieveDocument($"Item-{x.ItemId}"), x.ItemId})
                .Select(
                    x =>
                        TestItem.Construct(x.Content,
                            itemInput.First(y => y.ItemId.Equals(x.ItemId)))));
            result.Add(itemInput
                .Where(x => !string.IsNullOrEmpty(x.AssociatedStimuliId))
                .Distinct()
                .Select(x => new XElement("passage",
                    new XAttribute("filename", $"stim-{x.AssociatedStimuliId}.xml"),
                    Identifier.Construct(x.AssociatedStimuliId, "1"))));
            return result;
        }
    }
}