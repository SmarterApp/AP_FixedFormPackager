using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using AssessmentPackageBuilder.Utilities;
using FixedFormPackager.Common.Models;

namespace AssessmentPackageBuilder.Common
{
    public static class TestItem
    {
        public static XElement Construct(AssessmentContent assessmentContent)
        {
            var itemElement = assessmentContent.MainDocument.XPathSelectElement("/itemrelease/item");
            var uniqueId = $"{itemElement.Attribute("bankkey")?.Value}-{itemElement.Attribute("id")?.Value}";

            var test = itemElement.XPathSelectElement("//attrib[@attid='itm_att_Grade']/val");

            var result = new XElement("testitem",
                new XAttribute("filename",
                    $"item-{uniqueId}.xml"),
                new XAttribute("itemtype", itemElement.Attribute("format")?.Value),
                Identifier.Construct(uniqueId, itemElement.Attribute("version")?.Value),
                PoolProperty.Construct("--ITEMTYPE--", itemElement.Attribute("format")?.Value),
                PoolProperty.Construct("Grade",
                    itemElement.XPathSelectElement("//attrib[@attid='itm_att_Grade']/val").Value));
            var sXmlNs = new XmlNamespaceManager(new NameTable());
            sXmlNs.AddNamespace("sa", "http://www.smarterapp.org/ns/1/assessment_item_metadata");
            var bprefs =
                assessmentContent.MetaDocument.XPathSelectElements(
                        "metadata/sa:smarterAppMetadata/sa:StandardPublication/sa:PrimaryStandard", sXmlNs)
                    .Select(x => BpElementUtilities.GetBprefs(x.Value));
            var languages =
                itemElement.XPathSelectElements("//content[@name='language']/@value")
                    .Select(x => PoolProperty.Construct("Language", x.Value));
            result.Add(bprefs);
            result.Add(languages);
            return result;
        }
    }
}