using System.Xml.Linq;

namespace AssessmentPackageBuilder.Common
{
    public static class Identifier
    {
        public static XElement Construct(string uniqueId, string version)
        {
            return new XElement("identifier",
                new XAttribute("uniqueid", uniqueId),
                new XAttribute("version", version));
        }
    }
}