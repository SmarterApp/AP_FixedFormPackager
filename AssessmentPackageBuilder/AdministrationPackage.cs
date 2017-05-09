using System.Xml;
using AssessmentPackageBuilder.Common;
using FixedFormPackager.Common.Models;

namespace AssessmentPackageBuilder
{
    public class AdministrationPackage
    {
        public XmlDocument Assemble()
        {
            var document = new XmlDocument();
            document = TestSpecificationBuilder.Build(document, PackageType.Administration);
            return document;
        }
    }
}