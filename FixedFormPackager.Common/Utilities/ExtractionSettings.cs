using System.Collections.Generic;
using FixedFormPackager.Common.Models;

namespace FixedFormPackager.Common.Utilities
{
    public static class ExtractionSettings
    {
        public static List<ItemInput> ItemInput { get; set; }
        public static AssessmentInfo AssessmentInfo { get; set; }
        public static GitLabInfo GitLabInfo { get; set; }
    }
}