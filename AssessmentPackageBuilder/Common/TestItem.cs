using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using AssessmentPackageBuilder.Utilities;
using FixedFormPackager.Common.Extensions;
using FixedFormPackager.Common.Models;
using FixedFormPackager.Common.Models.Csv;
using NLog;

namespace AssessmentPackageBuilder.Common
{
    public static class TestItem
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static XElement Construct(AssessmentContent assessmentContent, Item itemInput, string publisher)
        {
            var itemElement = assessmentContent.MainDocument.XPathSelectElement("/itemrelease/item");
            var uniqueId = $"{itemElement.Attribute("bankkey")?.Value}-{itemElement.Attribute("id")?.Value}";

            var result = new XElement("testitem",
                new XAttribute("filename",
                    $"item-{uniqueId}.xml"),
                new XAttribute("itemtype", itemElement.Attribute("format")?.Value.ToUpper()),
                Identifier.Construct(uniqueId, itemElement.Attribute("version")?.Value));
            var grade = itemElement.XPathSelectElement("//attrib[@attid='itm_att_Grade']/val")?.Value;

            result.Add(new XElement("bpref", itemInput.SegmentId));
            
            if (assessmentContent.MetaDocument != null)
            {
                var sXmlNs = new XmlNamespaceManager(new NameTable());
                sXmlNs.AddNamespace("sa", "http://www.smarterapp.org/ns/1/assessment_item_metadata");
                result.Add(assessmentContent.MetaDocument.XPathSelectElements(
                        "metadata/sa:smarterAppMetadata/sa:StandardPublication/sa:PrimaryStandard", sXmlNs)
                    .Select(x => BpElementUtilities.GetBprefs(x.Value, publisher)).First());
                var calc = assessmentContent.MetaDocument.XPathSelectElement("metadata/sa:smarterAppMetadata/sa:AllowCalculator", sXmlNs)?.Value;
                if (!string.IsNullOrEmpty(calc))
                {
                    result.Add(PoolProperty.Construct("Calculator", calc.Equals("N", StringComparison.OrdinalIgnoreCase) ? "No" : "Yes"));
                }
            }
            if (!string.IsNullOrEmpty(itemInput.AssociatedStimuliId))
            {
                var bank = itemElement.Attribute("bankkey")?.Value;
                var stimuliId = itemInput.AssociatedStimuliId.Contains(bank+"-")
                    ? itemInput.AssociatedStimuliId
                    : bank + "-" + itemInput.AssociatedStimuliId;
                result.Add(new XElement("passageref", stimuliId));
            }
            result.Add(PoolProperty.Construct("--ITEMTYPE--", itemElement.Attribute("format")?.Value));
            result.Add(PoolProperty.Construct("Language", "ENU"));
            if (grade != null)
            {
                result.Add(
                    PoolProperty.Construct("Grade", grade));
            }
            else
            {
                Logger.LogError(new ErrorReportItem
                {
                    Location = $"Test Item Construction: {uniqueId}",
                    Severity = LogLevel.Warn
                }, "Item contains no 'itm_att_Grade' element");
            }
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