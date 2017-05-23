using System.Collections.Generic;
using FixedFormPackager.Common.Models;
using FixedFormPackager.Common.Models.Csv;

namespace FixedFormPackager.Common.Utilities
{
    public static class ExtractionSettings
    {
        public static List<Item> ItemInput { get; set; }
        public static Assessment AssessmentInfo { get; set; }
        public static GitLabInfo GitLabInfo { get; set; }
    }
}