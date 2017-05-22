using System.Collections.Generic;

namespace FixedFormPackager.Common.Models
{
    public class ItemScoringInformation
    {
        public string ScoringModel { get; set; }
        public string Score { get; set; }
        public string Weight { get; set; }
        public Dictionary<string, string> ScoreParameters { get; set; } = new Dictionary<string, string>();
    }
}