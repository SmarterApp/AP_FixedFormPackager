using System.Collections.Generic;
using FixedFormPackager.Common.Models;
using FixedFormPackager.Common.Models.Csv;

namespace FixedFormPackager.Common.Utilities
{
    public static class ExtractionSettings
    {
        public static List<Item> ItemInput { get; set; } = new List<Item>();
        public static Assessment AssessmentInfo { get; set; }

        public static List<AssessmentScoringComputationRule> AssessmentScoring { get; set; } =
            new List<AssessmentScoringComputationRule>();

        public static GitLabInfo GitLabInfo { get; set; }
    }
}