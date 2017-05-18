using System.IO;
using System.Xml.Linq;
using System.Xml.XPath;
using FixedFormPackager.Common.Models;

namespace AssessmentPackageBuilder.Common
{
    public static class TestItem
    {
        public static XElement Construct(AssessmentContent assessmentContent)
        {
            var itemElement = assessmentContent.MainDocument.XPathSelectElement("//item");
            if (itemElement == null)
            {
                throw new IOException("Could not find appropriate element \"item\" in assessment content");
            }

            var uniqueId = $"{itemElement.Attribute("bankkey")?.Value}-{itemElement.Attribute("id")?.Value}";

            return new XElement("testitem",
                new XAttribute("filename",
                    $"item-{uniqueId}.xml"),
                new XAttribute("itemtype", itemElement.Attribute("format")?.Value),
                Identifier.Construct(uniqueId, itemElement.Attribute("version")?.Value));
        }
    }
}