using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StaterV.Windows;

namespace StaterV.Drawers
{
    public class TextDrawer : Drawer
    {
        public Utility.Dot Center { get; set; }
        public float FontSize { get; set; }
        public string Text { get; set; }

        public override void Draw()
        {
            var size = Owner.Window.MeasureText(FontSize, Text);
            Owner.Window.DrawText(WindowBase.Colors.Foreground, Center.x - size.width/2, Center.y - size.height/2, FontSize, Text);
        }
    }
}
