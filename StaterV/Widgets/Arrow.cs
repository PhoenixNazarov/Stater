using System;
using StaterV.Commands;
using StaterV.Utility;
using StaterV.Windows;

namespace StaterV.Widgets
{
    [Serializable]
    public abstract class Arrow : Widget
    {
        public Arrow(WindowBase _window) : base(_window)
        {
        }

        public override void Draw()
        {
            if (Start == End)
            {
                lineDrawer = new Drawers.OvalLineDrawer();
                lineDrawer.Owner = this;
            }

            if (lineDrawer == null)
            {
                lineDrawer = new Drawers.StraightLineDrawer();
                lineDrawer.Window = Window;
            }
            if (endingDrawer == null)
            {
                endingDrawer = new Drawers.SimpleArrowDrawer();
                endingDrawer.Window = Window;
            }

            lineDrawer.Draw();

            //Рисуем стрелочки
            endingDrawer.Angle = lineDrawer.GetAngle();
            endingDrawer.Coord = lineDrawer.GetEnd();
            endingDrawer.Draw();
        }

        public override bool IntersectsWith(float dotX, float dotY)
        {
            return lineDrawer.IntersectsWith(dotX, dotY);
        }

        public void CalcBounds(out Dot s, out Dot e)
        {
            Dot sPos, ePos;
            int sNum, eNum;
            int sAm, eAm;
            window.GetMultiArrowNumber(this, out sNum, out sAm);
            eNum = sNum;
            eAm = sAm;
            start.GetPosition(out sPos);
            end.GetPosition(out ePos);
            eNum = sNum;
            sPos = Utility.MathSupport.CalcPositionInsideBox(sPos, sNum, sAm, start.Height, start.Width);
            ePos = Utility.MathSupport.CalcPositionInsideBox(ePos, eNum, eAm, end.Height, end.Width);
            start.IntersectLine(sPos, ePos, out s);
            end.IntersectLine(sPos, ePos, out e);
        }

        protected Shape start;
        protected Shape end;

        protected Drawers.LineDrawer lineDrawer;
        protected Drawers.EndingDrawer endingDrawer;

        public Shape Start
        {
            get { return start; }
            set { start = value; }
        }

        public Shape End
        {
            get { return end; }
            set { end = value; }
        }

        public override StaterV.Commands.Command PrepareDeleteCommand()
        {
            CommandParams p = new CommandParams();
            p.Window = Window;
            p.TheWidget = this;
            return new DeleteArrowCommand(p);
        }
    }
}