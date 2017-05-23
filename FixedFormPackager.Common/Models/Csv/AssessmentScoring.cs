﻿using System.Collections.Generic;

namespace FixedFormPackager.Common.Models.Csv
{
    public class AssessmentScoring
    {
        public string Name { get; set; }
        public string ComputationOrder { get; set; }
        public string BpElementId { get; set; }
        public IEnumerable<AssessmentScoringParameter> ScoringParameters { get; set; }
    }
}