using System.Collections.Generic;

namespace FixedFormPackager.Common.Models.Csv
{
    public class AssessmentScoringParameter
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public IEnumerable<AssessmentScoringParameterProperty> Properties { get; set; }
    }
}