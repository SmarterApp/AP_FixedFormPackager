using System.Xml.Linq;

namespace FixedFormPackager.Common.Models
{
    public class AssessmentContent
    {
        public AssessmentContent()
        {
            MainDocument = new XDocument();
            MetaDocument = new XDocument();
        }

        public XDocument MainDocument { get; set; }
        public XDocument MetaDocument { get; set; }
        public string BasePath { get; set; }
    }
}