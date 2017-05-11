using System;
using System.IO;

namespace ItemRetriever.Utilities
{
    public static class PathHelper
    {
        public static string RetrievePathForId(string identifier)
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                identifier.Contains("Item") ? "Items" : "Stimuli", identifier);
        }
    }
}