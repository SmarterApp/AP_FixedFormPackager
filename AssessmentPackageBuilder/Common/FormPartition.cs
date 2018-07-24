using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using FixedFormPackager.Common.Models.Csv;
using NLog;

namespace AssessmentPackageBuilder.Common
{
    public static class FormPartition
    {
        //private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static IEnumerable<XElement> Construct(IList<Item> items,
            XElement itemPool, Assessment assessment)
        {
            var formPartions = items.GroupBy(x => x.FormPartitionId);
            var formPartitionElements = new List<XElement>();
            foreach (var formpartition in formPartions)
            {
                var itemgroups = formpartition.GroupBy(x => x.FormPartitionPosition).ToList();
                var formpartitionelement = new XElement("formpartition",
                    new XElement("identifier",
                        new XAttribute("uniqueid", itemgroups.First().First().FormPartitionId),
                        new XAttribute("version", "1"),
                        new XAttribute("name", ParsePartitionName(assessment, formpartition.Key))));
                var index = 1;
                foreach (var group in itemgroups)
                {
                    formpartitionelement.Add(ItemGroup.Construct(group.ToList(), group.First().FormPartitionId, index++));
                }
                formPartitionElements.Add(formpartitionelement);
            }           
            return formPartitionElements;
        }

        private static string ParsePartitionName(Assessment assessment, string position)
        {
            var uniqueIdParsing =
                Regex.Matches(assessment.UniqueId, @"^.*\d{1,2}-(\w+)-.*(\d{4}-\d{4}).*$")
                    .Cast<Match>().LastOrDefault();
            var year = uniqueIdParsing?.Groups.Cast<Group>().LastOrDefault()?
                           .Value?.Substring(2, 2) ?? Regex.Matches(assessment.UniqueId, @"^.*(\d{4}-\d{4}).*$")
                           .Cast<Match>().LastOrDefault()?.Groups.Cast<Group>()
                           .LastOrDefault()?.Value?.Substring(2, 2) ?? new DateTime().Year.ToString();
            var description = uniqueIdParsing?.Captures.Cast<Capture>().Skip(1).FirstOrDefault()?.Value ?? "FFP";
            return
                $"{assessment.AssessmentSubtype}::{assessment.Subject}G{assessment.Grade}::Perf::S{position}::SP{year}::{description}";
        }
    }
}