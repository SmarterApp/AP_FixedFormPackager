﻿using System.Xml.Linq;

namespace AssessmentPackageBuilder.Common
{
    public static class PoolProperty
    {
        public static XElement Construct(string name, string value)
        {
            return new XElement("poolproperty",
                new XAttribute("property", name),
                new XAttribute("value", value),
                new XAttribute("label", value));
        }
    }
}