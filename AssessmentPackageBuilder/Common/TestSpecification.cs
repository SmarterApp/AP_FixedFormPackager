using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using AssessmentPackageBuilder.Administration;
using AssessmentPackageBuilder.Scoring;
using AssessmentPackageBuilder.Utilities;
using FixedFormPackager.Common.Utilities;
using NLog;

namespace AssessmentPackageBuilder.Common
{
    public static class TestSpecification
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static IEnumerable<XElement> Construct()
        {
            var result = new List<XElement>();
            var scoringRules = ScoringRules.Construct(ExtractionSettings.AssessmentScoring);
            var itemPool = ItemPool.Construct(ExtractionSettings.UniqueItems(), ExtractionSettings.AssessmentInfo.Publisher);
            var testBlueprint = TestBlueprint.Construct(ExtractionSettings.UniqueItems(), itemPool,
                ExtractionSettings.AssessmentInfo.UniqueId);
            var testForms = TestForm.Construct(ExtractionSettings.ItemInputs, itemPool,
                ExtractionSettings.AssessmentInfo).ToList();
            var performanceLevels = PerformanceLevels.Construct(ExtractionSettings.AssessmentInfo);
            var adminSegments = AdminSegment.Construct(itemPool, ExtractionSettings.ItemInput);

            var scoringSpecification = BuildGlobalProperties("scoring");
            scoringSpecification.Add(new XElement("scoring",
                testBlueprint,
                itemPool,
                testForms,
                performanceLevels,
                scoringRules
            ));
            result.Add(scoringSpecification);

            var administrationSpecification = BuildGlobalProperties("administration");
            administrationSpecification.Add(new XElement("administration",
                testBlueprint,
                PoolPropertyUtilities.GeneratePoolPropertyTypes(itemPool),
                PoolPropertyUtilities.GeneratePoolPropertyLanguages(itemPool),
                itemPool,
                testForms,
                adminSegments));
            result.Add(administrationSpecification);

            return result;
        }

        private static XElement BuildGlobalProperties(string purpose)
        {
            return new XElement("testspecification",
                new XAttribute("purpose", purpose),
                new XAttribute("publisher", ExtractionSettings.AssessmentInfo.Publisher),
                new XAttribute("publishdate", DateTime.Now.ToString("MMM dd yyyy h:mmtt")),
                new XAttribute("version", "1.0"),
                new XElement("identifier",
                    new XAttribute("uniqueid", ExtractionSettings.AssessmentInfo.UniqueId),
                    new XAttribute("name",
                        Regex.Matches(ExtractionSettings.AssessmentInfo.UniqueId,
                                @"^(\(" + ExtractionSettings.AssessmentInfo?.Publisher + @"\))(.*)$")
                            .Cast<Match>().FirstOrDefault(x => x.Success)?.Groups.Cast<Group>()
                            .LastOrDefault()?.Value ?? ExtractionSettings.AssessmentInfo.UniqueId),
                    new XAttribute("label",
                        string.IsNullOrEmpty(ExtractionSettings.AssessmentInfo.Label)
                            ? ExtractionSettings.AssessmentInfo.UniqueId
                            : ExtractionSettings.AssessmentInfo.Label),
                    new XAttribute("version", "1")),
                new XElement("property",
                    new XAttribute("name", "subject"),
                    new XAttribute("value", ExtractionSettings.AssessmentInfo.Subject),
                    new XAttribute("label", ExtractionSettings.AssessmentInfo.Subject)),
                new XElement("property",
                    new XAttribute("name", "grade"),
                    new XAttribute("value", ExtractionSettings.AssessmentInfo.Grade),
                    new XAttribute("label", $"grade {ExtractionSettings.AssessmentInfo.Grade}")),
                new XElement("property",
                    new XAttribute("name", "type"),
                    new XAttribute("value", ExtractionSettings.AssessmentInfo.AssessmentType),
                    new XAttribute("label", ExtractionSettings.AssessmentInfo.AssessmentType)));
        }
    }
}