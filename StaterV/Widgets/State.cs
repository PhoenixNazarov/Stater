using System;
using System.Collections.Generic;
using System.Xml;
using StaterV.Attributes;
using StaterV.Windows;

namespace StaterV.Widgets
{
    [Serializable]
    public class State : Shape
    {
        protected State() : base() {}

        public State(WindowBase _window) : base(_window)
        {
            TheAttributes = new StateAttributes();
            TheAttributes.Name = "state" + Window.Widgets.Count;
            TheAttributes.Type = StateType.Common;
        }

        private const float XIndent = 25f;
        private const float YIndent = 5f;
        private const float curvingRadius = 10;
        private int fontSize = 12;

        public enum StateType
        {
            Common,
            Start,
            End,
        }

        protected override void WriteSpecificInfo(XmlWriter writer)
        {
            writer.WriteAttributeString(XMLTypeAttribute, "State");
        }

        protected override void WriteAttributes(XmlWriter writer)
        {
            writer.WriteElementString("name", TheAttributes.Name);
            int type = (int) TheAttributes.Type;
            writer.WriteElementString("type", type.ToString());
            if (IncomingArrows.Count > 0)
            {
                foreach (var arrow in IncomingArrows)
                {
                    writer.WriteStartElement("incoming");
                    writer.WriteAttributeString(XMLIDAttribute, arrow.ID.ToString());
                    writer.WriteEndElement();
                }
            }

            if (OutgoingArrows.Count > 0)
            {
                foreach (var arrow in OutgoingArrows)
                {
                    writer.WriteStartElement("outgoing");
                    writer.WriteAttributeString(XMLIDAttribute, arrow.ID.ToString());
                    writer.WriteEndElement();
                }
            }

            //Nested machines
            WriteNested(writer);

            WriteExecutions(writer);

            WriteEffects(writer);

            WriteActions(writer);
        }

        private void WriteActions(XmlWriter writer)
        {
            if (TheAttributes.EntryActions != null)
            {
                foreach (var act in TheAttributes.EntryActions)
                {
                    writer.WriteStartElement("EntryAction");
                    writer.WriteAttributeString("name", act.Name);
                    writer.WriteAttributeString("synchro", ((int)act.Synchronism).ToString());
                    writer.WriteAttributeString("comment", act.Comment);
                }
            }
        }

        private void WriteEffects(XmlWriter writer)
        {
            if (TheAttributes.EntryEffects != null)
            {
                foreach (var eff in TheAttributes.EntryEffects)
                {
                    writer.WriteStartElement("effect");
                    writer.WriteAttributeString("type", eff.Type);
                    writer.WriteAttributeString("name", eff.Name);
                    writer.WriteAttributeString("event", eff.Event);
                    writer.WriteAttributeString("synchro", ((int)eff.Synchronism).ToString());
                    writer.WriteAttributeString("descritpion", eff.Description);
                    writer.WriteAttributeString("effect_type", ((int)eff.TheEffectType).ToString());
                    writer.WriteEndElement();
                }
            }
        }

        private void WriteExecutions(XmlWriter writer)
        {
            if (TheAttributes.EntryExecutions != null)
            {
                foreach (var exec in TheAttributes.EntryExecutions)
                {
                    writer.WriteStartElement("execution");
                    writer.WriteAttributeString("type", exec.Type);
                    writer.WriteAttributeString("name", exec.Name);
                    writer.WriteEndElement();
                }
            }
        }

        private void WriteNested(XmlWriter writer)
        {
            if (TheAttributes.NestedMachines != null)
            {
                foreach (var machine in TheAttributes.NestedMachines)
                {
                    writer.WriteStartElement("nested");
                    writer.WriteAttributeString("type", machine.Type);
                    writer.WriteAttributeString("name", machine.Name);
                    writer.WriteEndElement();
                }
            }
        }

        /// <summary>
        /// Write the string such as: <state id="0" x="1" y="2" width="60" height="30/>
        /// </summary>
        /// <param name="writer">An XmlWriter which opened the file.</param>
        public override void SaveCoords(XmlWriter writer)
        {
            writer.WriteStartElement("state");
            writer.WriteAttributeString("id", ID.ToString());
            writer.WriteAttributeString("x", X.ToString());
            writer.WriteAttributeString("y", Y.ToString());
            writer.WriteAttributeString("width", Width.ToString());
            writer.WriteAttributeString("height", Height.ToString());
            writer.WriteEndElement();
        }

        public override void Draw()
        {
            DetermineWidth();

            var color = WindowBase.Colors.Foreground;
            switch (TheMode)
            {
                case Mode.Common:
                    color = WindowBase.Colors.Foreground;
                    break;
                case Mode.Active:
                    color = WindowBase.Colors.Active;
                    break;
                case Mode.ShowInfo:
                    color = WindowBase.Colors.ShowInfo;
                    break;
            }
            Window.DrawRoundedRectangle(color, X, Y, curvingRadius, Width, Height);
            Window.DrawLine(color, X, Y + Height/2, X + Width, Y + Height/2);

            switch (TheAttributes.Type)
            {
                case StateType.Common:
                    break;
                case StateType.Start:
                    Window.FillEllipse(color, X+5, Y+5, 10, 10);
                    break;
                case StateType.End:
                    Window.DrawEllipse(color, X + 5, Y + 5, 10, 10);
                    Window.FillEllipse(color, X + 7, Y + 7, 6, 6);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            Window.DrawText(WindowBase.Colors.Foreground, X + XIndent, Y + YIndent, fontSize, TheAttributes.Name);
        }

        public override void DetermineWidth()
        {
            var size = Window.MeasureText(fontSize, TheAttributes.Name);
            if (size.width + XIndent * 2 > Width)
            {
                Width = size.width + XIndent*2;
            }
        }

        public Attributes.StateAttributes TheAttributes { get; set; }

        public override void Insert()
        {
            throw new NotImplementedException();
        }

        public override void Delete()
        {
            throw new NotImplementedException();
        }

        public override bool ShowPropertiesForm()
        {
            return window.ShowStatePropertiesForm(this);
        }
        
    }
}