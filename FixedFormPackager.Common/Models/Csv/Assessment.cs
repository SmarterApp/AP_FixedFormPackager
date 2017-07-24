namespace FixedFormPackager.Common.Models.Csv
{
    public class Assessment
    {
        public string UniqueId { get; set; }
        public string Publisher { get; set; }
        public string Label { get; set; }
        public string Subject { get; set; }
        public string Grade { get; set; }
        public string AssessmentType { get; set; }
        public string AssessmentSubtype { get; set; }
        public string ScaledLo { get; set; }
        public string ScaledPartition1 { get; set; }
        public string ScaledPartition2 { get; set; }
        public string ScaledPartition3 { get; set; }
        public string ScaledHi { get; set; }
    }
}