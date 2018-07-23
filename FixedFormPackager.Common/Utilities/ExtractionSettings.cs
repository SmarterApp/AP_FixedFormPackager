using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using FixedFormPackager.Common.Models;
using FixedFormPackager.Common.Models.Csv;

namespace FixedFormPackager.Common.Utilities
{
    public static class ExtractionSettings
    {
        public static List<Item> ItemInput { get; set; } = new List<Item>();

        public static List<List<Item>> ItemInputs { get; set; } = new List<List<Item>>();

        public static List<Item> UniqueItems()
        {
            var masterList = ItemInputs.SelectMany(x => x);
            ItemInput =  masterList.GroupBy(x => x.ItemId).Select(i => i.First()).ToList();
            return ItemInput;
        }

        public static Assessment AssessmentInfo { get; set; }

        public static List<AssessmentScoringComputationRule> AssessmentScoring { get; set; } =
            new List<AssessmentScoringComputationRule>();

        public static GitLabInfo GitLabInfo { get; set; }
    }
}