using System.Collections.Generic;

namespace FixedFormPackager.Common.Models.Csv
{
    public class AssessmentScoringComputationRule
    {
        public string Name { get; set; }
        public string ComputationOrder { get; set; }
        public string BpElementId { get; set; }

        public IEnumerable<AssessmentScoringParameter> ScoringParameters { get; set; } =
            new List<AssessmentScoringParameter>();
    }
}