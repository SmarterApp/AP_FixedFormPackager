﻿using System.Xml.Linq;

namespace AssessmentPackageBuilder.Administration
{
    public static class SegmentBpElement
    {
        public static XElement Construct(string uniqueid, string count)
        {
            return new XElement("segmentbpelement",
                new XAttribute("minopitems", count),
                new XAttribute("maxopitems", count),
                new XAttribute("bpelementid", uniqueid));
        }
    }
}