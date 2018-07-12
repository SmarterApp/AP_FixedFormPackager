using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Remoting.Messaging;
using System.Text.RegularExpressions;
using CsvHelper;
using CsvHelper.Configuration;
using FixedFormPackager.Common.Models.Csv;
//using NLog;

namespace FixedFormPackager.Common.Utilities.CsvMappers
{
    public sealed class AssessmentScoringMapper : CsvClassMap<AssessmentScoringComputationRule>
    {
        //private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public AssessmentScoringMapper()
        {
            Map(m => m.Name).Name("Name").Index(0);
            Map(m => m.ComputationOrder).Name("ComputationOrder").Index(1);
            Map(m => m.BpElementId).Name("BpElementId").Index(2);
            Map(m => m.ScoringParameters).ConvertUsing(row =>
            {
                var groupedHeaders = (row as CsvReader)?.FieldHeaders
                    .Where(header => Regex.IsMatch(header, @"^.+\d$"))
                    .GroupBy(x => x.Split('_').Skip(1).First());
                List<AssessmentScoringParameter> scoringParameters = new List<AssessmentScoringParameter>();
                foreach (var headerGroup in groupedHeaders)
                {
                    List<AssessmentScoringComputationRuleParameterProperty> props =
                        new List<AssessmentScoringComputationRuleParameterProperty>();
                    List<AssessmentScoringComputationRuleParameterValue> comps =
                        new List<AssessmentScoringComputationRuleParameterValue>();
                    var type = row.GetField($"ParameterType_{headerGroup.Key}") ?? string.Empty;
                    var name = row.GetField($"ParameterName_{headerGroup.Key}") ?? string.Empty;
                    var position = headerGroup.Key;
                    for (var i = 1; i <= 4; i++)
                    {
                        if (row.TryGetField($"ParameterPropertyName_{position}_{i}", out string whatever))
                        {
                            props.Add(new AssessmentScoringComputationRuleParameterProperty
                            {
                                Name = row.GetField($"ParameterPropertyName_{position}_{i}"),
                                Value = row.GetField($"ParameterPropertyValue_{position}_{i}")
                            });
                        }
                        else
                        {
                            break;
                        }
                    }
                    for (var i = 1; i <= 4; i++)
                    {
                        if (row.TryGetField($"ParameterPropertyName_{position}_{i}", out string whatever))
                        {
                            comps.Add(new AssessmentScoringComputationRuleParameterValue
                            {
                                Index = row.GetField($"ParameterComputationRuleIndex_{position}_{i}"),
                                Value = row.GetField($"ParameterComputationRuleValue_{position}_{i}")
                            });
                        }
                        else
                        {
                            break;
                        }
                    }
                    /*
                    foreach (var header in headerGroup)
                    {
                        Logger.Debug($"header={header} \t\t value={row.GetField(header)}");
                    }
                    */

                    scoringParameters.Add(new AssessmentScoringParameter
                    {
                        Name = name,
                        Type = type,
                        Position = position,
                        Properties = props,
                        ComputationValues = comps
                    });
                }
                return scoringParameters;
            });
        }
    }
}
