using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using PluginData;

namespace FBDK.FBParser
{
    public class Agent
    {
        private XmlDocument xmlDocument = new XmlDocument();
        private AParser parser = new AParser();
        public string CurValue { get; private set; }
        public string FileName { get; set; }
        public PluginData.StateMachine FSM
        {
            get { return parser.Fsm; }
        }

        /// <summary>
        /// Parse condition from the transition of ECC.
        /// </summary>
        /// <param name="cond">Source condition</param>
        /// <param name="e">result event</param>
        /// <param name="guard">result guard</param>
        /// <returns>true if parse is successful, false otherwise</returns>
        public bool ParseCondition(string cond, out Event e, out string guard)
        {
            bool res = true;
            int and = cond.IndexOf('&');

            if (and == -1)
            {
                //If cond.trim is in the set of events
                var es = FSM.Events.Where(evt => evt.Name == cond.Trim()).AsParallel();
                
                //e = cond.trim
                e = es.FirstOrDefault();
                guard = "";
                res = true;

                //else
                if (e == null)
                {
                    //guard = cond
                    e = new Event();
                    res = ParseGuard(cond, out guard);
                }
                return res;
            }

            e = new Event {Name = cond.Substring(0, and).Trim()};
            res = ParseGuard(cond.Substring(and + 1), out guard);
            return res;
        }

        public bool ParseGuard(string source, out string res)
        {
            bool success = true;
            res = source.Trim();
            res = res.Replace("NOT", "!");
            res = res.Replace("AND", "&&");
            res = res.Replace("OR", "||");
            return success;
        }

        public void Parse(string path)
        {
            CurValue = "";
            parser.Agent = this;
            var settings = new XmlReaderSettings();
            settings.DtdProcessing = DtdProcessing.Ignore;
            settings.XmlResolver = null;
            xmlDocument = new XmlDocument();
            XmlReader xr = XmlReader.Create(path, settings);
            xmlDocument.Load(xr);

            var node = xmlDocument.FirstChild; //<?xml
            while (node.NextSibling != null)
            {
                node = node.NextSibling;
                ParseNode(node);
            }

            parser.Fsm.Type = FileName.Replace('.', '_');
            parser.Fsm.Name = "_" + parser.Fsm.Type;
        }

        private void ParseNode(XmlNode node)
        {
            parser.ProcessEvent(node.Name);
            if (node.Attributes != null)
            {
                ParseAttributes(node.Attributes);
            }
            if (node.Value != null && node.Value.Trim() != "")
            {
                CurValue = node.Value;
                parser.ProcessEvent(node.Value);
            }
            if (node.HasChildNodes)
            {
                foreach (XmlNode child in node.ChildNodes)
                {
                    ParseNode(child);
                }
            }
            parser.ProcessEvent(node.Name);
        }
        private void ParseAttributes(XmlAttributeCollection _collection)
        {
            foreach (XmlAttribute attribute in _collection)
            {
                CurValue = attribute.Value;
                parser.ProcessEvent(attribute.Name);
            }
        }

        public void TestFBTypeComment()
        {
            if (CurValue == "Basic Function Block Type")
            {
                parser.ProcessEvent(Events.Basic);
            }
            else if (CurValue == "Composite Function Block Type")
            {
                parser.ProcessEvent(Events.Composite);
            }
        }

        public void TestInitComment()
        {
            if (CurValue.IndexOf("Initial State") != -1)
            {
                parser.ProcessEvent(Events.InitialState);
            }
        }
    }
}
