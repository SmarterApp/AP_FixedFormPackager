using System.Xml.XPath;
using FixedFormPackager.Common.Models;
using FixedFormPackager.Common.Models.Csv;

namespace FixedFormPackager.Common.Utilities
{
    public static class ItemStimuliMapper
    {
        public static Item Map(AssessmentContent assessmentContent, Item item)
        {
            item.AssociatedStimuliId =
                assessmentContent.MainDocument.XPathSelectElement("./itemrelease/item/associatedpassage")?.Value ??
                string.Empty;
            return item;
        }
    }
}