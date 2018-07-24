using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FixedFormPackager.Common.Models
{
    public static class LanguageMapping
    {
        static Dictionary<string, string> _langs = new Dictionary<string, string>
        {
            {"English", "ENU" },
            {"Spanish", "ESN"},
            {"Braille", "BRA" }
        };

        public static string GetLabel(string language)
        {
            if (_langs.TryGetValue(language, out var label))
            {
                return label;
            }
            else
            {
                return null;
            }
        }
    }
}
