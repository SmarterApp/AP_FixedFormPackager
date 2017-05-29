using System.Xml.Linq;

namespace AssessmentPackageBuilder.Administration
{
    public static class SegmentBpElement
    {
        public static XElement Construct(string bpelementid, string count)
        {
            return new XElement("segmentbpelement",
                new XAttribute("minopitems", count),
                new XAttribute("maxopitems", count),
                new XAttribute("bpelementid", bpelementid));
        }
    }
}