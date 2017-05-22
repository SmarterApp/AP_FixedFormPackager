namespace FixedFormPackager.Common.Models
{
    public class ItemInput
    {
        public string ItemId { get; set; }
        public string AssociatedStimuliId { get; set; }
        public string FormPartitionId { get; set; }
        public string SegmentId { get; set; }
        public string SegmentPosition { get; set; }
        //MeasurementModel1	ScorePoints1	Dimension1	Weight1	a_1	b_1	b0_1	b1_1	b2_1	b3_1	b4_1	c_1	
        //MeasurementModel2	ScorePoints2	Dimension2	Weight2	a_2	b_2	b0_2	b1_2	b2_2	b3_2	b4_2	c_2

        public string MeasurementModel1 { get; set; }
        public string ScorePoints1 { get; set; }
        public string Dimension1 { get; set; }
        public string Weight1 { get; set; }
        public string a_1 { get; set; }
        public string b_1 { get; set; }
        public string b0_1 { get; set; }
        public string b1_1 { get; set; }
        public string b2_1 { get; set; }
        public string b3_1 { get; set; }
        public string b4_1 { get; set; }
        public string c_1 { get; set; }

        public string MeasurementModel2 { get; set; }
        public string ScorePoints2 { get; set; }
        public string Dimension2 { get; set; }
        public string Weight2 { get; set; }
        public string a_2 { get; set; }
        public string b_2 { get; set; }
        public string b0_2 { get; set; }
        public string b1_2 { get; set; }
        public string b2_2 { get; set; }
        public string b3_2 { get; set; }
        public string b4_2 { get; set; }
        public string c_2 { get; set; }
    }
}