using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StaterV.Widgets;
using StaterV.Windows;

namespace StaterV
{
    public class CommandParams
    {
        public Widget TheWidget { set; get; }
        public float X { set; get; }
        public float Y { set; get; }

        public float Width { set; get; }
        public float Height { set; get; }

        public WidgetType Type { set; get; }
        public WindowBase Window { set; get; }
        }
}
