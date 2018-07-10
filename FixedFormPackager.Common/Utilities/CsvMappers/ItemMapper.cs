using System.Linq;
using System.Text.RegularExpressions;
using CsvHelper;
using CsvHelper.Configuration;
using FixedFormPackager.Common.Models.Csv;

namespace FixedFormPackager.Common.Utilities.CsvMappers
{
    public sealed class ItemMapper : CsvClassMap<Item>
    {
        public ItemMapper()
        {
            Map(m => m.ItemId).Name("ItemId").Index(0);
            Map(m => m.FormPartitionId).Name("FormPartitionId").Index(1);
            Map(m => m.FormPartitionPosition).Name("FormPartitionPosition").Index(2);
            Map(m => m.FormPosition).Name("FormPosition").Index(3);
            Map(m => m.SegmentId).Name("SegmentId").Index(4);
            Map(m => m.SegmentPosition).Name("SegmentPosition").Index(5);
            Map(m => m.ItemScoringInformation).ConvertUsing(row =>
                (row as CsvReader)?.FieldHeaders
                .Where(header => Regex.IsMatch(header, @"^.+\d$"))
                .GroupBy(x => x.Last())
                .Select(x => new ItemScoring
                {
                    MeasurementModel =
                        row.GetField(x.FirstOrDefault(y => y.Equals($"MeasurementModel_{x.Key}")) ?? string.Empty),
                    ScorePoints = row.GetField(x.FirstOrDefault(y => y.Equals($"ScorePoints_{x.Key}")) ?? string.Empty),
                    Dimension = row.GetField(x.FirstOrDefault(y => y.Equals($"Dimension_{x.Key}")) ?? string.Empty),
                    Weight = row.GetField(x.FirstOrDefault(y => y.Equals($"Weight_{x.Key}")) ?? string.Empty),
                    a = row.GetField(x.FirstOrDefault(y => y.Equals($"a_{x.Key}")) ?? string.Empty),
                    b = row.GetField(x.FirstOrDefault(y => y.Equals($"b_{x.Key}")) ?? string.Empty),
                    b0 = row.GetField(x.FirstOrDefault(y => y.Equals($"b0_{x.Key}")) ?? string.Empty),
                    b1 = row.GetField(x.FirstOrDefault(y => y.Equals($"b1_{x.Key}")) ?? string.Empty),
                    b2 = row.GetField(x.FirstOrDefault(y => y.Equals($"b2_{x.Key}")) ?? string.Empty),
                    b3 = row.GetField(x.FirstOrDefault(y => y.Equals($"b3_{x.Key}")) ?? string.Empty),
                    b4 = row.GetField(x.FirstOrDefault(y => y.Equals($"b4_{x.Key}")) ?? string.Empty),
                    c = row.GetField(x.FirstOrDefault(y => y.Equals($"c_{x.Key}")) ?? string.Empty)
                }).ToList()
            );
        }
    }
}