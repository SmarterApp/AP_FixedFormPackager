using System.Linq;
using System.Text.RegularExpressions;
using CsvHelper;
using CsvHelper.Configuration;
using FixedFormPackager.Common.Models.Csv;

namespace FixedFormPackager.Common.Utilities.CsvMappers
{
    public sealed class AssessmentScoringMapper : CsvClassMap<AssessmentScoringComputationRule>
    {
        public AssessmentScoringMapper()
        {
            Map(m => m.Name).Name("Name").Index(0);
            Map(m => m.ComputationOrder).Name("ComputationOrder").Index(1);
            Map(m => m.BpElementId).Name("BpElementId").Index(2);
            Map(m => m.ScoringParameters).ConvertUsing(row =>
                (row as CsvReader)?.FieldHeaders
                .Where(header => Regex.IsMatch(header, @"^.+\d$"))
                .GroupBy(x => x.Split('_').Skip(1).First())
                .Select(x => new AssessmentScoringParameter
                {
                    Type = row.GetField(x.FirstOrDefault(y => y.Equals($"ParameterType_{x.Key}")) ?? string.Empty),
                    Name = row.GetField(x.FirstOrDefault(y => y.Equals($"ParameterName_{x.Key}")) ?? string.Empty),
                    Position = x.Key,
                    Properties =
                        x.GroupBy(y => y.Split('_').Last())
                            .Select(y => new AssessmentScoringComputationRuleParameterProperty
                            {
                                Name =
                                    row.GetField(
                                        x.FirstOrDefault(z => z.Equals($"ParameterPropertyName_{x.Key}_{y.Key}")) ??
                                        string.Empty),
                                Value =
                                    row.GetField(
                                        x.FirstOrDefault(z => z.Equals($"ParameterPropertyValue_{x.Key}_{y.Key}")) ??
                                        string.Empty)
                            }),
                    ComputationValues =
                        x.GroupBy(y => y.Split('_').Last())
                            .Select(y => new AssessmentScoringComputationRuleParameterValue
                            {
                                Index =
                                    row.GetField(
                                        x.FirstOrDefault(z => z.Equals($"ParameterComputationRuleIndex_{x.Key}_{y.Key}")) ??
                                        string.Empty),
                                Value =
                                    row.GetField(
                                        x.FirstOrDefault(z => z.Equals($"ParameterComputationRuleValue_{x.Key}_{y.Key}")) ??
                                        string.Empty)
                            })
                }).ToList()
            );
        }
    }
}