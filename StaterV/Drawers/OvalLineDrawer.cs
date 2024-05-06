using System;
using StaterV.Utility;
using StaterV.Widgets;
using StaterV.Windows;

namespace StaterV.Drawers
{
    /// <summary>
    /// Рисует линии в виде части овала. В данный момент реализован только для петель.
    /// </summary>
    [Serializable]
    class OvalLineDrawer : LineDrawer
    {
        const int radiusX = 10;
        const int radiusY = 10;

        public override void Draw()
        {
            Dot s;
            int pos;
            int amount;
            Owner.Window.GetMultiArrowNumber(Owner, out pos, out amount);
            Owner.Start.GetPosition(out s);
            //Owner.Start.GetPosition(Owner, out s, out pos, out amount);
            var sPos = MathSupport.CalcPositionInsideBox(s, pos, amount, Owner.Start.Height, Owner.Start.Width);
            Start = sPos;
            MathSupport.GetLoopBorderPosition(Start, Owner.Start, out s, out boxSide);
            Start = s;
            End = s;

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

            switch (boxSide)
            {
                case SidePieces.Top:
                    center.x = s.x;
                    center.y = s.y - radiusY;
                    break;
                case SidePieces.Right:
                    center.x = s.x + radiusX;
                    center.y = s.y;
                    break;
                case SidePieces.Bottom:
                    center.x = s.x;
                    center.y = s.y + radiusY;
                    break;
                case SidePieces.Left:
                    center.x = s.x - radiusX;
                    center.y = s.y;
                    break;
            }
            Owner.Window.DrawEllipse(color, center.x - radiusX, center.y - radiusY, 2 * radiusX, 2 * radiusY);
        }

        private SidePieces boxSide;

        private Dot center;

        public override double GetAngle()
        {
            if (boxSide == SidePieces.Left || boxSide == SidePieces.Right)
            {
                return 90;
            }
            return 0;
        }

        public override Dot GetEnd()
        {
            return End;
        }

        public override bool IntersectsWith(Dot testing)
        {
            return MathSupport.IsDotCrossesCircle(testing, center, radiusX, 10);
        }

        public override Dot GetCenter()
        {
            //throw new NotImplementedException();
            return center;
        }
    }
}
