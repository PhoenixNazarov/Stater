using System;
using StaterV.Utility;
using StaterV.Widgets;
using StaterV.Windows;

namespace StaterV.Drawers
{
    [Serializable]
    class StraightLineDrawer : LineDrawer
    {
        public override void Draw()
        {
            Dot s;
            Dot e;
            Owner.CalcBounds(out s, out e);

            Start = s;
            End = e;

            var color = WindowBase.Colors.Foreground;

            switch (owner.TheMode)
            {
                case Widget.Mode.Common:
                    color = WindowBase.Colors.Foreground;
                    break;
                case Widget.Mode.Active:
                    color = WindowBase.Colors.Active;
                    break;
                case Widget.Mode.ShowInfo:
                    color = WindowBase.Colors.ShowInfo;
                    break;
            }

            Owner.Window.DrawLine(color, Start.x, Start.y, End.x, End.y);
        }

        public override double GetAngle()
        {
            return MathSupport.GetLineAngle(Start, End);
        }

        public override Dot GetEnd()
        {
            return End;
        }

        public override bool IntersectsWith(Dot testing)
        {
            //TODO: Придумать алгоритм, достигающий погрешность в 3 пиксела.
            return MathSupport.IsDotCrossesSegLine(testing, Start, End, 12);
        }

        public override Dot GetCenter()
        {
            var res = new Dot();
            res.x = (Start.x + End.x)/2;
            res.y = (Start.y + End.y) / 2;
            return res;
        }
    }
}
