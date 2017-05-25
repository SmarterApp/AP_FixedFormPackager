using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using FixedFormPackager.Common.Models.Csv;

namespace AssessmentPackageBuilder.Scoring
{
    public static class ScoringRules
    {
        public static XElement Construct(IEnumerable<AssessmentScoringComputationRule> computationRules)
        {
            var result = new XElement("scoringrules");
            result.Add(computationRules.Select(x => new XElement("computationrule",
                new XAttribute("computationorder", x.ComputationOrder ?? string.Empty),
                new XAttribute("bpelementid", x.BpElementId ?? string.Empty),
                new XElement("identifier",
                    new XAttribute("name", x.Name ?? string.Empty),
                    new XAttribute("label", x.Name ?? string.Empty),
                    new XAttribute("version", "1.0"),
                    new XAttribute("uniqueid", Guid.NewGuid().ToString())),
                x.ScoringParameters.Where(y => !string.IsNullOrEmpty(y.Type))
                    .Select(y => new XElement("computationruleparameter",
                        new XAttribute("position", y.Position ?? string.Empty),
                        new XAttribute("parametertype", y.Type ?? string.Empty),
                        new XElement("identifier",
                            new XAttribute("name", y.Name ?? string.Empty),
                            new XAttribute("version", "1.0"),
                            new XAttribute("uniqueid", Guid.NewGuid().ToString())),
                        y.Properties.Where(z => !string.IsNullOrEmpty(z.Name) && !string.IsNullOrEmpty(z.Value))
                            .Select(z => new XElement("property",
                                new XAttribute("name", z.Name),
                                new XAttribute("value", z.Value))),
                        y.ComputationValues.Where(z => !string.IsNullOrEmpty(z.Value))
                            .Select(z => new XElement("computationruleparametervalue",
                                new XAttribute("index", z.Index ?? string.Empty),
                                new XAttribute("value", z.Value))))))));
            return result;
        }
    }
}