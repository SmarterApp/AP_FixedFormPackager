using System;
using System.Linq;
using System.Xml.Linq;

namespace AssessmentPackageBuilder.Common
{
    public static class BpElement
    {
        public static XElement Construct(string elementtype, string count, string uniqueid)
        {
            var result = new XElement("bpelement",
                new XAttribute("elementtype", elementtype),
                new XAttribute("minopitems", count),
                new XAttribute("maxopitems", count),
                new XAttribute("opitemcount", count));
            var identifier = Identifier.Construct(uniqueid, "1");
            if (elementtype.Equals("test", StringComparison.OrdinalIgnoreCase) ||
                elementtype.Equals("segment", StringComparison.OrdinalIgnoreCase))
            {
                result.Add(new XAttribute("minftitems", "1"));
                result.Add(new XAttribute("maxftitems", "1"));
                result.Add(new XAttribute("ftitemcount", "1"));
                identifier.Add(new XAttribute("name", uniqueid));
            }
            else
            {
                identifier.Add(new XAttribute("name",
                    uniqueid.Split('-').Length > 1
                        ? uniqueid.Split('-').Skip(1).Aggregate((x, y) => $"{x}-{y}")
                        : uniqueid));
            }
            if (elementtype.Equals("contentlevel", StringComparison.OrdinalIgnoreCase))
            {
                result.Add(new XAttribute("parentid", identifier.Attribute("uniqueid").Value
                    .Split('|')
                    .Reverse()
                    .Skip(1)
                    .Reverse()
                    .Aggregate((x, y) => $"{x}|{y}")));
            }
            result.Add(identifier);
            return result;
        }
    }
}