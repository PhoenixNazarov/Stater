using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml;
using StaterV.Commands;
using StaterV.Windows;

namespace StaterV.Widgets
{
    [Serializable]
    public abstract class Widget : IObserver
    {
        protected Widget() {}

        public Widget(WindowBase _window)
        {
            window = _window;
            ID = window.GetUniqueID();
        }

        public Int64 ID { get; set; }

        public const string XMLName = "widget";
        public const string XMLIDAttribute = "id";
        public const string XMLTypeAttribute = "type";
        //protected abstract void WriteInfo(XmlWriter writer);
        protected abstract void WriteSpecificInfo(XmlWriter writer);
        protected abstract void WriteAttributes(XmlWriter writer);

        public void ToXML(XmlWriter writer)
        {
            writer.WriteStartElement(XMLName);
            writer.WriteAttributeString(XMLIDAttribute, ID.ToString());
            
            WriteSpecificInfo(writer);

            writer.WriteStartElement("attributes");
            WriteAttributes(writer);
            writer.WriteEndElement();

            writer.WriteEndElement();
        }

        [field: NonSerialized]
        protected WindowBase window;

        public enum Mode
        {
            Common,
            Active,
            ShowInfo, 
        }

        protected Mode theMode;

        private float x;
        protected float y;
        private float width = 30;
        private float height = 60;

        public float X
        {
            get { return x; }
            set { x = value; }
        }
        public float Y
        {
            get { return y; }
            set { y = value; }
        }

        public float Width
        {
            get { return width; }
            set { width = value; }
        }

        public float Height
        {
            get { return height; }
            set { height = value; }
        }

        public Mode TheMode
        {
            get { return theMode; }
            set { theMode = value; }
        }

        //[property:NonSerialized]
        public WindowBase Window
        {
            get { return window; }
            set { window = value; }
        }

        public abstract void Draw();
        public abstract void Insert();
        public abstract void Delete();
        public abstract bool IntersectsWith(float dotX, float dotY);
        public abstract bool IntersectsWith(float rectX, float rectY, float rectH, float rectW);

        public abstract bool ShowPropertiesForm();
        public abstract Command PrepareDeleteCommand();


        [field: NonSerialized]
        protected DeleteCommand deleteWidget;

        public void Update()
        {
            TheMode = Mode.Common;
        }

        public abstract void SaveCoords(XmlWriter writer);
    }
}