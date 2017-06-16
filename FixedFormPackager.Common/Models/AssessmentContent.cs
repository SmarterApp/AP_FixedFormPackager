using System.Xml.Linq;

namespace FixedFormPackager.Common.Models
{
    public class AssessmentContent
    {
        public XDocument MainDocument { get; set; }
        public XDocument MetaDocument { get; set; }
        public string BasePath { get; set; }
    }
}