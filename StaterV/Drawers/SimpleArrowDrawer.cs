using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StaterV.Utility;
using StaterV.Windows;

namespace StaterV.Drawers
{
    [Serializable]
    class SimpleArrowDrawer : EndingDrawer
    {
        public override void Draw()
        {
            Dot s1;
            float length = 10;
            MathSupport.CreateStartSegLineAngle(Angle - 30, length, Coord, out s1);
            Owner.Window.DrawLine(WindowBase.Colors.Foreground, s1.x, s1.y, Coord.x, Coord.y);
            MathSupport.CreateStartSegLineAngle(Angle + 30, length, Coord, out s1);
            Owner.Window.DrawLine(WindowBase.Colors.Foreground, s1.x, s1.y, Coord.x, Coord.y);
            
        }
    }
}
