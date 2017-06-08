using System.Linq;

namespace FixedFormPackager.Common.Utilities
{
    public static class HashGenerator
    {
        public static string Hash(params int[] values)
        {
            return values.Aggregate((x, y) => x * y).ToString("D4");
        }
    }
}