using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using AssessmentPackageBuilder.Utilities;
using FixedFormPackager.Common.Models;
using FixedFormPackager.Common.Models.Csv;

namespace AssessmentPackageBuilder.Common
{
    public static class TestForm
    {
        public static IEnumerable<XElement> Construct(List<List<Item>> items,
            XElement itemPool, Assessment assessment)
        {
            var forms = new List<XElement>();
            foreach (var itemSet in items)
            {
                var language = itemSet.First().Presentation;
                var languageValue = LanguageMapping.getLabel(language);
                forms.Add(
                    new XElement("testform",
                        new XAttribute("length", itemSet.Count().ToString()),
                        new XElement("identifier",
                            new XAttribute("uniqueid", $"{assessment.UniqueId}:Default-{languageValue}"),
                            new XAttribute("name", $"{assessment.UniqueId}:Default-{languageValue}"),
                            new XAttribute("version", "1")),
                        new XElement("property",
                            new XAttribute("name", "language"),
                            new XAttribute("value", languageValue),
                            new XAttribute("label", language)),
                        new XElement("poolproperty",
                            new XAttribute("property", "Language"),
                            new XAttribute("value", languageValue),
                            new XAttribute("label", language),
                            new XAttribute("itemcount", items.Count().ToString())),
                        PoolPropertyUtilities.GeneratePoolPropertyTypes(itemPool),
                        FormPartition.Construct(itemSet, itemPool, assessment))
                );
            }

            return forms;
            /***** orig
            // Each language will get its own test form, but they will be identical
            return itemPool.XPathSelectElements("//poolproperty")
                .Where(x => x.Attribute("property").Value.Equals("Language", StringComparison.OrdinalIgnoreCase))
                .Select(x => x.Attribute("value").Value)
                .Distinct().ToList().Select(x =>
                    new XElement("testform",
                        new XAttribute("length", items.Count().ToString()),
                        new XElement("identifier",
                            new XAttribute("uniqueid", $"{assessment.UniqueId}:Default-{x}"),
                            new XAttribute("name", $"{assessment.UniqueId}:Default-{x}"),
                            new XAttribute("version", "1")),
                        new XElement("property",
                            new XAttribute("name", "language"),
                            new XAttribute("value", x),
                            new XAttribute("label", x)),
                        new XElement("poolproperty",
                            new XAttribute("property", "Language"),
                            new XAttribute("value", x),
                            new XAttribute("label", x),
                            new XAttribute("itemcount", items.Count().ToString())),
                        PoolPropertyUtilities.GeneratePoolPropertyTypes(itemPool),
                        FormPartition.Construct(items, itemPool, assessment)));
        }
        ******/
        }
    }
}