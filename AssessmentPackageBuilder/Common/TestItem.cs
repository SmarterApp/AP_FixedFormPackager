using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using AssessmentPackageBuilder.Utilities;
using FixedFormPackager.Common.Models;
using FixedFormPackager.Common.Models.Csv;

namespace AssessmentPackageBuilder.Common
{
    public static class TestItem
    {
        public static XElement Construct(AssessmentContent assessmentContent, Item itemInput)
        {
            var itemElement = assessmentContent.MainDocument.XPathSelectElement("/itemrelease/item");
            var uniqueId = $"{itemElement.Attribute("bankkey")?.Value}-{itemElement.Attribute("id")?.Value}";

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
            if (!string.IsNullOrEmpty(itemInput.AssociatedStimuliId))
            {
                result.Add(new XElement("passageref", itemInput.AssociatedStimuliId));
            }
            result.Add(bprefs);
            result.Add(languages);
            ConstructItemScoringNodes(itemInput);
            return result;
        }

        private static IEnumerable<XElement> ConstructItemScoringNodes(Item itemInput)
        {
            var result = itemInput.GetType().GetProperties()
                .Where(x => Regex.IsMatch(x.Name, @"^.+\d$"))
                .GroupBy(x => x.Name.Last());
            return null;
        }
    }
}