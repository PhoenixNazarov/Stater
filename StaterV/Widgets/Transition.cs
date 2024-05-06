using System;
using System.Xml;
using StaterV.Attributes;
using StaterV.Drawers;
using StaterV.Windows;
using System.Text;

namespace StaterV.Widgets
{
    [Serializable]
    public class Transition : Arrow
    {
        public Transition(WindowBase _window) : base(_window)
        {

            lineDrawer = new StraightLineDrawer {Owner = this, Window = Window};

            endingDrawer = new SimpleArrowDrawer {Owner = this, Window = Window};

            TheAttributes = new TransitionAttributes();
            //TheAttributes.Name = "transition" + Window.Widgets.Count;
            TheAttributes.Name = "transition" + ID;
            TheAttributes.TheEvent = Event.CreateEpsilon();
        }

        public TransitionAttributes TheAttributes;

        [field: NonSerialized]
        protected TextDrawer eventDrawer = new TextDrawer();

        public void AutoName(int _attempt)
        {
            attempt = _attempt;
            AutoName();
        }

        private int attempt = 0;
        public void AutoName()
        {
            string suffix = "";
            if (attempt > 0)
            {
                suffix = "_" + attempt;
            }
            TheAttributes.Name = "transition" + ID + suffix;
            ++attempt;
        }

        protected override void WriteSpecificInfo(XmlWriter writer)
        {
            writer.WriteAttributeString(XMLTypeAttribute, "Transition");
        }

        protected override void WriteAttributes(XmlWriter writer)
        {
            //Event
            writer.WriteStartElement("event");
            writer.WriteAttributeString("name", TheAttributes.TheEvent.Name);
            writer.WriteAttributeString("comment", TheAttributes.TheEvent.Comment);
            writer.WriteEndElement();

            //Actions
            foreach (var action in TheAttributes.Actions)
            {
                writer.WriteStartElement("action");
                writer.WriteAttributeString("name", action.Name);
                writer.WriteAttributeString("comment", action.Comment);
                writer.WriteAttributeString("synchro", ((int)action.Synchronism).ToString());
                writer.WriteEndElement();
            }

            WriteCode(writer);
            WriteCondition(writer);
        }

        private void WriteCondition(XmlWriter writer)
        {
            writer.WriteStartElement("guard");
            writer.WriteValue(TheAttributes.Guard);
            writer.WriteEndElement();
        }

        private void WriteCode(XmlWriter writer)
        {
            writer.WriteStartElement("code");
            foreach (var line in TheAttributes.Code)
            {
                var wl = line.Trim();
                if (wl != "")
                {
                    writer.WriteValue(wl);
                    writer.WriteValue("\r\n");
                }
            }
            writer.WriteEndElement();
        }

        public override void Draw()
        {
            base.Draw();
            if (eventDrawer == null)
            {
                eventDrawer = new TextDrawer();
            }
            eventDrawer.Owner = this;
            eventDrawer.Center = lineDrawer.GetCenter();
            //TheAttributes.TheEvent.ConvertName();
            string guard = "";
            if (TheAttributes.Guard.Trim() != "")
            {
                guard = "/" + TheAttributes.Guard;
            }
            eventDrawer.Text = "[" + TheAttributes.TheEvent.Name + guard + "]";
            if (TheMode == Mode.ShowInfo)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("\n");
                foreach (var line in TheAttributes.Code)
                {
                    sb.Append(line);
                    sb.Append("\n");
                }
                eventDrawer.Text += sb.ToString();
            }
            eventDrawer.FontSize = 12;
            eventDrawer.Draw();
        }

        public override void Insert()
        {
            throw new NotImplementedException();
        }

        public override void Delete()
        {
            throw new NotImplementedException();
        }

        public override bool IntersectsWith(float rectX, float rectY, float rectH, float rectW)
        {
            return false;
        }

        public override bool ShowPropertiesForm()
        {
            return Window.ShowTransitionPropertiesForm(this);
        }

        public override void SaveCoords(XmlWriter writer)
        {
            //Do nothing
        }
    }
}