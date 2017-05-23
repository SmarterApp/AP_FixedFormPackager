using System.Collections.Generic;

namespace FixedFormPackager.Common.Models
{
    public class ItemContentMetadata
    {
        public string Identifier { get; set; }
        public string Subject { get; set; }
        public string Version { get; set; }
        public string IntendedGrade { get; set; }
        public string DepthOfKnowledge { get; set; }
        public string DifficultyCategory { get; set; }
        public List<string> Languages { get; set; }
        public string ItemType { get; set; }
        public string ScoringEngine { get; set; }
        public List<string> Standards { get; set; }
        public List<ItemScoringInformation> ItemScoringInformation { get; set; }
    }
}