using System.Linq;

namespace FixedFormPackager.Common.Utilities
{
    public static class GenerationHash
    {
        public static string GenerateUniqueHash(params string[] values)
        {
            return values.Aggregate((x, y) => $"{x.GetHashCode() * y.GetHashCode()}").Substring(0,5);
        }
    }
}
