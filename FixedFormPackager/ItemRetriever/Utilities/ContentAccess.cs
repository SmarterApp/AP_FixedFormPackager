using System.IO;
using System.Xml.Linq;
using FixedFormPackager.Common.Extensions;
using FixedFormPackager.Common.Models;
using NLog;

namespace ItemRetriever.Utilities
{
    public static class ContentAccess
    {

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public static AssessmentContent RetrieveDocument(string identifier)
        {
            var content = new AssessmentContent();
            var basePath = PathHelper.RetrievePathForId(identifier);
            if (!Directory.Exists(basePath))
            {
                throw new IOException($"Expected content directory at {basePath} does not exist");
            }
            content.MainDocument = XDocument.Load($"{basePath}/{identifier.ToLower()}.xml");
            try
            {
                content.MetaDocument = XDocument.Load($"{basePath}/metadata.xml");
            }
            catch (IOException)
            {
                Logger.LogError(new ErrorReportItem
                {
                    Location = "Retrieve Document metadata.xml",
                    Severity = LogLevel.Warn
                },
                "Retrieved item does not contain a metadata.xml file!");
            }
            content.BasePath = basePath;
            return content;
        }
    }
}