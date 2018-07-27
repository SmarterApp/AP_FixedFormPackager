using System.Xml.Linq;
using AssessmentPackageBuilder.Utilities;

namespace AssessmentPackageBuilder.Common
{
    public static class PoolProperty
    {
        public static XElement Construct(string name, string value)
        {
            return new XElement("poolproperty",
                new XAttribute("property", name),
                new XAttribute("value", PoolPropertyUtilities.CheckBraille(value) ),
                new XAttribute("label", PoolPropertyUtilities.CheckSpanish(value)));
        }

    }

}