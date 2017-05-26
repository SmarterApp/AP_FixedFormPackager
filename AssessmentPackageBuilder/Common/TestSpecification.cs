using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using AssessmentPackageBuilder.Scoring;
using FixedFormPackager.Common.Utilities;

namespace AssessmentPackageBuilder.Common
{
    public static class TestSpecification
    {
        public static IEnumerable<XElement> Construct()
        {
            var result = new List<XElement>();
            var scoringRules = ScoringRules.Construct(ExtractionSettings.AssessmentScoring);
            var itemPool = ItemPool.Construct(ExtractionSettings.ItemInput);
            var testBlueprint = TestBlueprint.Construct(ExtractionSettings.ItemInput, itemPool,
                ExtractionSettings.AssessmentInfo.UniqueId);
            var testForms = TestForm.Construct(ExtractionSettings.ItemInput, itemPool,
                ExtractionSettings.AssessmentInfo).ToList();
            var performanceLevels = PerformanceLevels.Construct(ExtractionSettings.AssessmentInfo);

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

            return result;
        }

        private static XElement BuildGlobalProperties(string purpose)
        {
            return new XElement("testspecification",
                new XAttribute("purpose", purpose),
                new XAttribute("publisher", ExtractionSettings.AssessmentInfo.Publisher),
                new XAttribute("publishdate", DateTime.Now.ToString("MMM dd yyyy h:mm tt")),
                new XAttribute("version", "1.0"),
                new XElement("identifier",
                    new XAttribute("uniqueid", ExtractionSettings.AssessmentInfo.UniqueId),
                    new XAttribute("name",
                        Regex.Matches(ExtractionSettings.AssessmentInfo.UniqueId,
                                @"^(\(" + ExtractionSettings.AssessmentInfo?.Publisher + @"\)).*$")
                            .Cast<Match>().FirstOrDefault(x => x.Success)?.Groups.Cast<Group>()
                            .LastOrDefault()?.Value ?? ExtractionSettings.AssessmentInfo.UniqueId),
                    new XAttribute("label", ExtractionSettings.AssessmentInfo.UniqueId),
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