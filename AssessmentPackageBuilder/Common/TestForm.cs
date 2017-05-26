using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using AssessmentPackageBuilder.Utilities;
using FixedFormPackager.Common.Models.Csv;

namespace AssessmentPackageBuilder.Common
{
    public static class TestForm
    {
        public static IEnumerable<XElement> Construct(IList<Item> items,
            XElement itemPool, Assessment assessment)
        {
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
                            new XAttribute("value", x)),
                        new XElement("poolproperty",
                            new XAttribute("property", "Language"),
                            new XAttribute("value", x),
                            new XAttribute("itemcount", items.Count().ToString())),
                        PoolPropertyUtilities.GeneratePoolPropertyTypes(itemPool),
                        FormPartition.Construct(items, itemPool, assessment)));
        }
    }
}