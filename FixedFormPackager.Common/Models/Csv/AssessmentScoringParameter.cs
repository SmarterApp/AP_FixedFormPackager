using System.Collections.Generic;

namespace FixedFormPackager.Common.Models.Csv
{
    public class AssessmentScoringParameter
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }

        public IEnumerable<AssessmentScoringComputationRuleParameterProperty> Properties { get; set; } =
            new List<AssessmentScoringComputationRuleParameterProperty>();

        public IEnumerable<AssessmentScoringComputationRuleParameterValue> ComputationValues { get; set; } =
            new List<AssessmentScoringComputationRuleParameterValue>();
    }
}