using System.Xml;

namespace ItemRetriever.Models
{
    public class Content
    {
        public Content()
        {
            MainDocument = new XmlDocument();
            MetaDocument = new XmlDocument();
        }

        public XmlDocument MainDocument { get; set; }
        public XmlDocument MetaDocument { get; set; }
    }
}