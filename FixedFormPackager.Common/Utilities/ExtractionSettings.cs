using System.Collections.Generic;
using FixedFormPackager.Common.Models;
using FixedFormPackager.Common.Models.Csv;

namespace FixedFormPackager.Common.Utilities
{
    public static class ExtractionSettings
    {
        public static List<Item> ItemInput { get; set; } = new List<Item>();
        public static Assessment AssessmentInfo { get; set; }
        public static List<AssessmentScoringParameter> AssessmentScoring { get; set; } = new List<AssessmentScoringParameter>();
        public static GitLabInfo GitLabInfo { get; set; }
    }
}