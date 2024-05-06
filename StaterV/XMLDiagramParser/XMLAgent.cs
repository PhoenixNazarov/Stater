using System.Xml;

namespace StaterV.XMLDiagramParser
{
    public class XMLAgent
    {
        //private const string prefix = "AParser.";
// ReSharper disable InconsistentNaming
        private const string valueEvent = "value";
// ReSharper restore InconsistentNaming
        private string path;
        private XmlDocument xmlDocument = new XmlDocument();
        private CParser parser = new CParser();

        public XMLAgent()
        {
            parser.Owner = this;
        }

        private void CreateParser()
        {
            parser = new CParser();
            parser.Owner = this;
        }

        public Windows.WindowBase Parse(string _path)
        {
            path = _path;
            xmlDocument.Load(path);
            CreateParser();

            var node = xmlDocument.FirstChild; //<?xml
            while (node.NextSibling != null)
            {
                node = node.NextSibling;
                ParseNode(node);
            }
            //parser.ProcessEventStr("diagram");

            //parser.ProcessEvent();

            //parser.Window.GiveDefaultCoords();
            parser.Window.FileName = path;
            parser.Window.SafeFileName = parser.Window.FileName.Substring(parser.Window.FileName.LastIndexOf('\\') + 1);
            return parser.Window;
        }

        private void ParseNode(XmlNode node)
        {
            parser.ProcessEventStr(node.Name);
            if (node.Attributes != null)
            {
                ParseAttributes(node.Attributes);
            } 
            if (node.Value != null && node.Value.Trim() != "")
            {
                CurValue = node.Value;
                parser.ProcessEventStr(valueEvent);
            }
            if (node.HasChildNodes)
            {
                foreach (XmlNode child in node.ChildNodes)
                {
                    ParseNode(child);
                }
            }
            parser.ProcessEventStr(node.Name);
        }

        private void ParseAttributes(XmlAttributeCollection _collection)
        {
            foreach (XmlAttribute attribute in _collection)
            {
                CurValue = attribute.Value;
                parser.ProcessEventStr(attribute.Name);
            }
        }

        public string CurValue { get; private set; }
    }
}
