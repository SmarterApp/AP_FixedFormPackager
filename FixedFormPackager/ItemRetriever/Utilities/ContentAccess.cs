using System.IO;
using ItemRetriever.Models;

namespace ItemRetriever.Utilities
{
    public static class ContentAccess
    {
        public static Content RetrieveDocument(string identifier)
        {
            var content = new Content();
            var basePath = PathHelper.RetrievePathForId(identifier);
            if (Directory.Exists(basePath))
            {
                content.MainDocument.Load($"{basePath}/{identifier.ToLower()}.xml");
                content.MetaDocument.Load($"{basePath}/metadata.xml");
            }
            return content;
        }
    }
}