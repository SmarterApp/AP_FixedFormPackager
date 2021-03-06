﻿using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using FixedFormPackager.Common.Models.Csv;
using FixedFormPackager.Common.Utilities;
using ItemRetriever.Utilities;

namespace AssessmentPackageBuilder.Common
{
    public static class ItemPool
    {
        public static XElement Construct(IEnumerable<Item> itemInput, string publisher)
        {
            var result = new XElement("itempool");
            result.Add(ExtractionSettings.ItemInput.Select(
                    x => new {Content = ContentAccess.RetrieveDocument($"Item-{x.ItemId}"), x.ItemId})
                .Select(
                    x =>
                        TestItem.Construct(x.Content,
                            ItemStimuliMapper.Map(x.Content, itemInput.First(y => y.ItemId.Equals(x.ItemId))), publisher)));
            result.Add(itemInput
                .Where(x => !string.IsNullOrEmpty(x.AssociatedStimuliId)).Select(x => x.AssociatedStimuliId)
                .Distinct()
                .Select(x => new XElement("passage",
                    new XAttribute("filename", $"stim-{x}.xml"),
                    Identifier.Construct(x, "1"))));
            return result;
        }
    }
}