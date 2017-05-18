using System.Xml.Linq;
using AssessmentPackageBuilder.Common;
using FixedFormPackager.Common.Models;

namespace AssessmentPackageBuilder
{
    public class AdministrationPackage
    {
        public XDocument Assemble()
        {
            var document = new XDocument();
            document = TestSpecificationBuilder.Construct(document, PackageType.Administration);
            return document;
        }
    }
}