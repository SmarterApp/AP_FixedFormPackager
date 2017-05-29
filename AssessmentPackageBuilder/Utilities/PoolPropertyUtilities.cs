using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;

namespace AssessmentPackageBuilder.Utilities
{
    public static class PoolPropertyUtilities
    {
        public static IEnumerable<XElement> GeneratePoolPropertyTypes(XNode itemPool)
        {
            return itemPool.XPathSelectElements("//poolproperty")
                .Where(x => x.Attribute("property").Value.Equals("--ITEMTYPE"))
                .GroupBy(x => x.Attribute("value").Value)
                .Select(
                    x =>
                        new XElement("poolproperty", new XAttribute("property", "--ITEMTYPE--"),
                            new XAttribute("value", x.Key), new XAttribute("label", x.Key),
                            new XAttribute("itemcount", x.Count())));
        }

        public static IEnumerable<XElement> GeneratePoolPropertyLanguages(XNode itemPool)
        {
            return itemPool.XPathSelectElements("//poolproperty")
                .Where(x => x.Attribute("property").Value.Equals("Language"))
                .GroupBy(x => x.Attribute("value").Value)
                .Select(
                    x =>
                        new XElement("poolproperty", new XAttribute("property", "Language"),
                            new XAttribute("value", x.Key), new XAttribute("label", x.Key),
                            new XAttribute("itemcount", x.Count())));
        }
    }
}