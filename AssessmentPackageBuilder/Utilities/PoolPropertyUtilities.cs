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
            return itemPool.XPathSelectElements(".//poolproperty")
                .Where(x => x.Attribute("property").Value.Equals("--ITEMTYPE--"))
                .GroupBy(x => x.Attribute("value").Value)
                .Select(
                    x =>
                        new XElement("poolproperty", new XAttribute("property", "--ITEMTYPE--"),
                            new XAttribute("value", x.Key.ToUpper()), new XAttribute("label", x.Key.ToUpper()),
                            new XAttribute("itemcount", x.Count())));
        }

        public static IEnumerable<XElement> GeneratePoolPropertyLanguages(XNode itemPool)
        {
            return itemPool.XPathSelectElements(".//poolproperty")
                .Where(x => x.Attribute("property").Value.Equals("Language"))
                .GroupBy(x => x.Attribute("value").Value)
                .Select(
                    x =>
                        new XElement("poolproperty", new XAttribute("property", "Language"),
                            new XAttribute("value", _checkBraille(x.Key.ToUpper())), new XAttribute("label", x.Key),
                            new XAttribute("itemcount", x.Count())));
        }

        private static string _checkBraille(string language)
        {
            if (language.Equals("ENU-BRAILLE"))
            {
                return "ENU-Braille";
            }

            return language.ToUpper();
        }
    }
}