using System.Xml.Linq;
using FixedFormPackager.Common.Models.Csv;

namespace AssessmentPackageBuilder.Scoring
{
    public static class PerformanceLevels
    {
        public static XElement Construct(Assessment assessment)
        {
            return new XElement("performancelevels",
                new XElement("performancelevel",
                    new XAttribute("bpelementid", assessment.UniqueId),
                    new XAttribute("plevel", "1"),
                    new XAttribute("scaledlo", assessment.ScaledLo),
                    new XAttribute("scaledhi", assessment.ScaledPartition1)),
                new XElement("performancelevel",
                    new XAttribute("bpelementid", assessment.UniqueId),
                    new XAttribute("plevel", "2"),
                    new XAttribute("scaledlo", assessment.ScaledPartition1),
                    new XAttribute("scaledhi", assessment.ScaledPartition2)),
                new XElement("performancelevel",
                    new XAttribute("bpelementid", assessment.UniqueId),
                    new XAttribute("plevel", "3"),
                    new XAttribute("scaledlo", assessment.ScaledPartition2),
                    new XAttribute("scaledhi", assessment.ScaledPartition3)),
                new XElement("performancelevel",
                    new XAttribute("bpelementid", assessment.UniqueId),
                    new XAttribute("plevel", "4"),
                    new XAttribute("scaledlo", assessment.ScaledPartition3),
                    new XAttribute("scaledhi", assessment.ScaledHi)));
        }
    }
}