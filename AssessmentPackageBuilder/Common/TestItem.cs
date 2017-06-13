using System.Collections.Generic;
using System.Linq;
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
        public static XElement Construct(AssessmentContent assessmentContent, Item itemInput, string publisher)
        {
            var itemElement = assessmentContent.MainDocument.XPathSelectElement("/itemrelease/item");
            var uniqueId = $"{itemElement.Attribute("bankkey")?.Value}-{itemElement.Attribute("id")?.Value}";

            var result = new XElement("testitem",
                new XAttribute("filename",
                    $"item-{uniqueId}.xml"),
                new XAttribute("itemtype", itemElement.Attribute("format")?.Value),
                Identifier.Construct(uniqueId, itemElement.Attribute("version")?.Value),
                PoolProperty.Construct("--ITEMTYPE--", itemElement.Attribute("format")?.Value),
                PoolProperty.Construct("Language", "ENU"),
                PoolProperty.Construct("Grade",
                    itemElement.XPathSelectElement("//attrib[@attid='itm_att_Grade']/val").Value));
            var sXmlNs = new XmlNamespaceManager(new NameTable());
            sXmlNs.AddNamespace("sa", "http://www.smarterapp.org/ns/1/assessment_item_metadata");
            if (!string.IsNullOrEmpty(itemInput.AssociatedStimuliId))
            {
                result.Add(new XElement("passageref", itemInput.AssociatedStimuliId));
            }
            result.Add(assessmentContent.MetaDocument.XPathSelectElements(
                    "metadata/sa:smarterAppMetadata/sa:StandardPublication/sa:PrimaryStandard", sXmlNs)
                .Select(x => BpElementUtilities.GetBprefs(x.Value, publisher)));
            result.Add(new XElement("bpref", itemInput.SegmentId));
            result.Add(itemElement.XPathSelectElements("//content[@name='language']/@value")
                .Select(x => PoolProperty.Construct("Language", x.Value)));
            result.Add(ConstructItemScoringNodes(itemInput));
            return result;
        }

        private static IEnumerable<XElement> ConstructItemScoringNodes(Item itemInput)
        {
            var result = new List<XElement>();
            foreach (var item in itemInput.ItemScoringInformation.Where(x => !string.IsNullOrEmpty(x.MeasurementModel)))
            {
                var itemScoreDimension = new XElement("itemscoredimension",
                    new XAttribute("measurementmodel", item.MeasurementModel),
                    new XAttribute("scorepoints", item.ScorePoints),
                    new XAttribute("weight", item.Weight));
                if (!string.IsNullOrEmpty(item.Dimension))
                {
                    itemScoreDimension.Add(new XAttribute("dimension", item.Dimension));
                }
                if (!string.IsNullOrEmpty(item.a))
                {
                    itemScoreDimension.Add(ConstructItemScoreParameter("a", item.a));
                }
                if (!string.IsNullOrEmpty(item.b))
                {
                    itemScoreDimension.Add(ConstructItemScoreParameter("b", item.b));
                }
                if (!string.IsNullOrEmpty(item.b0))
                {
                    itemScoreDimension.Add(ConstructItemScoreParameter("b0", item.b0));
                }
                if (!string.IsNullOrEmpty(item.b1))
                {
                    itemScoreDimension.Add(ConstructItemScoreParameter("b1", item.b1));
                }
                if (!string.IsNullOrEmpty(item.b2))
                {
                    itemScoreDimension.Add(ConstructItemScoreParameter("b2", item.b2));
                }
                if (!string.IsNullOrEmpty(item.b3))
                {
                    itemScoreDimension.Add(ConstructItemScoreParameter("b3", item.b3));
                }
                if (!string.IsNullOrEmpty(item.b4))
                {
                    itemScoreDimension.Add(ConstructItemScoreParameter("b4", item.b4));
                }
                if (!string.IsNullOrEmpty(item.c))
                {
                    itemScoreDimension.Add(ConstructItemScoreParameter("c", item.c));
                }
                result.Add(itemScoreDimension);
            }
            return result;
        }

        private static XElement ConstructItemScoreParameter(string parameter, string value)
        {
            return new XElement("itemscoreparameter",
                new XAttribute("measurementparameter", parameter),
                new XAttribute("value", value));
        }
    }
}