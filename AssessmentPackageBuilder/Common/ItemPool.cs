using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using FixedFormPackager.Common.Models.Csv;
using FixedFormPackager.Common.Utilities;
using ItemRetriever.Utilities;
using NLog;

namespace AssessmentPackageBuilder.Common
{
    public static class ItemPool
    {
        //private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static XElement Construct(IEnumerable<Item> itemInput, string publisher)
        {
            var result = new XElement("itempool");
            result.Add(ExtractionSettings.ItemInput.Select(
                    x => new {Content = ContentAccess.RetrieveDocument($"Item-{x.ItemId}"), x.ItemId})
                .Select(
                    x =>
                        TestItem.Construct(x.Content,
                            ItemStimuliMapper.Map(x.Content, itemInput.First(y => y.ItemId.Equals(x.ItemId))), publisher)));
            var bank = itemInput.First(x => !string.IsNullOrEmpty(x.BankKey)).BankKey;
            result.AddFirst(itemInput
                .Where(x => !string.IsNullOrEmpty(x.AssociatedStimuliId)).Select(x => x.AssociatedStimuliId)
                .Distinct()
                .Select(x => new XElement("passage",
                    new XAttribute("filename", $"stim-{bank}-{x}.xml"),
                    Identifier.Construct($"{bank}-{x}", "1"))));
            return result;
        }
    }
}