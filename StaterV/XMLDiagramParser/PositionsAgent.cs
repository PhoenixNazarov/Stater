using System;
using System.Xml;
using PositionsLoader;

namespace StaterV.XMLDiagramParser
{
    class PositionsAgent
    {
        private readonly XmlDocument xmlDocument = new XmlDocument();
        private CPosLoader parser;
        public string CurValue { get; private set; }
        public Windows.WindowBase Window { get; private set; }
// ReSharper disable InconsistentNaming
        private const string valueEvent = "value";
// ReSharper restore InconsistentNaming

        public void LoadPositions(Windows.WindowBase wnd)
        {
            Window = wnd;
            string path = wnd.FileName + ".positions";
            try
            {
                xmlDocument.Load(path);
            }
            //catch (System.IO.FileNotFoundException)
            catch (Exception)
            {
                return;
            }
            CreateParser();

            var node = xmlDocument.FirstChild; //<?xml
            while (node.NextSibling != null)
            {
                node = node.NextSibling;
                ParseNode(node);
            }
            wnd.CoordsAreSet = true;
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

        private void CreateParser()
        {
            parser = new CPosLoader {Owner = this};
        }
    }
}
