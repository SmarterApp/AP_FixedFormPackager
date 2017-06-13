using System.Collections.Generic;

namespace FixedFormPackager.Common.Models.Csv
{
    public class Item
    {
        public string ItemId { get; set; }
        public string AssociatedStimuliId { get; set; } = string.Empty;
        public string FormPartitionId { get; set; }
        public string FormPartitionPosition { get; set; }
        public string FormPosition { get; set; }
        public string SegmentId { get; set; }
        public string SegmentPosition { get; set; }
        public IEnumerable<ItemScoring> ItemScoringInformation { get; set; } = new List<ItemScoring>();
    }
}