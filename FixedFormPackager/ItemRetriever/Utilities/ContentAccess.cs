using System.IO;
using System.Xml.Linq;
using FixedFormPackager.Common.Models;

namespace ItemRetriever.Utilities
{
    public static class ContentAccess
    {
        public static AssessmentContent RetrieveDocument(string identifier)
        {
            var content = new AssessmentContent();
            var basePath = PathHelper.RetrievePathForId(identifier);
            if (!Directory.Exists(basePath))
            {
                throw new IOException($"Expected content directory at {basePath} does not exist");
            }
            content.MainDocument = XDocument.Load($"{basePath}/{identifier.ToLower()}.xml");
            content.MetaDocument = XDocument.Load($"{basePath}/metadata.xml");
            content.BasePath = basePath;
            return content;
        }
    }
}