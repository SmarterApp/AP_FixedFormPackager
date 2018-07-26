using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.XPath;
using FixedFormPackager.Common.Models.Csv;
using NLog;

namespace ItemRetriever.Utilities
{
    public static class IrtMapper
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static IEnumerable<ItemScoring> RetrieveIrtParameters(string itemId)
        {
            var document = ContentAccess.RetrieveDocument($"Item-{itemId}");
            if (document.MetaDocument == null)
            {
                return new List<ItemScoring>();
            }
            var sXmlNs = new XmlNamespaceManager(new NameTable());
            sXmlNs.AddNamespace("sa", "http://www.smarterapp.org/ns/1/assessment_item_metadata");
            var irt = document.MetaDocument.XPathSelectElements(
                "metadata/sa:smarterAppMetadata/sa:IrtDimension", sXmlNs);
            var measElement = document.MetaDocument
                .XPathSelectElement("metadata/sa:smarterAppMetadata/sa:IrtDimension/sa:IrtModelType", sXmlNs)?.Value;
            if (measElement != null && measElement.Equals("IRT3PLN"))
            {
                measElement = "IRT3PLn";
            }
            Logger.Debug($"thing is {measElement}");
            return  irt.Select(x => new ItemScoring
            {
                //MeasurementModel = x.XPathSelectElement("./sa:IrtModelType", sXmlNs)?.Value,
                MeasurementModel = measElement,
                Dimension = x.XPathSelectElement("./sa:IrtDimensionPurpose", sXmlNs)?.Value,
                ScorePoints = x.XPathSelectElement("./sa:IrtScore", sXmlNs)?.Value,
                Weight = x.XPathSelectElement("./sa:IrtWeight", sXmlNs)?.Value,
                a =
                    x.XPathSelectElement("./sa:IrtParameter[sa:Name/text() = \"a\"]/sa:Value", sXmlNs)?.Value ??
                    string.Empty,
                b =
                    x.XPathSelectElement("./sa:IrtParameter[sa:Name/text() = \"b\"]/sa:Value", sXmlNs)?.Value ??
                    string.Empty,
                b0 =
                    x.XPathSelectElement("./sa:IrtParameter[sa:Name/text() = \"b0\"]/sa:Value", sXmlNs)?.Value ??
                    string.Empty,
                b1 =
                    x.XPathSelectElement("./sa:IrtParameter[sa:Name/text() = \"b1\"]/sa:Value", sXmlNs)?.Value ??
                    string.Empty,
                b2 =
                    x.XPathSelectElement("./sa:IrtParameter[sa:Name/text() = \"b2\"]/sa:Value", sXmlNs)?.Value ??
                    string.Empty,
                b3 =
                    x.XPathSelectElement("./sa:IrtParameter[sa:Name/text() = \"b3\"]/sa:Value", sXmlNs)?.Value ??
                    string.Empty,
                b4 =
                    x.XPathSelectElement("./sa:IrtParameter[sa:Name/text() = \"b4\"]/sa:Value", sXmlNs)?.Value ??
                    string.Empty,
                c =
                    x.XPathSelectElement("./sa:IrtParameter[sa:Name/text() = \"c\"]/sa:Value", sXmlNs)?.Value ??
                    string.Empty
            });
        }
    }
}