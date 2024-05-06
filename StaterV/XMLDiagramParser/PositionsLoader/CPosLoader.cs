using System;
using StaterV.XMLDiagramParser;

namespace PositionsLoader
{
    class CPosLoader : APosLoader
    {
        public PositionsAgent Owner { get; set; }
        //private StaterV.Widgets.State state;

        private long curID = -1;
        private float curX = -1;
        private float curY = -1;
        private float curWidth = 80;
        private float curHeight = 60;

        /// <summary>
        ///
        /// </summary>
        public override void WritePositions()
        {
            int ind = FindStateById(curID);
            if (ind != -1)
            {
                if (curX != -1)
                {
                    Owner.Window.Widgets[ind].X = curX;
                }
                if (curY != -1)
                {
                    Owner.Window.Widgets[ind].Y = curY;
                }                
                Owner.Window.Widgets[ind].Width = curWidth;
                Owner.Window.Widgets[ind].Height = curHeight;
            }

            curID = -1;
            curX = -1;
            curY = -1;
        }

        /// <summary>
        ///
        /// </summary>
        public override void SetHeight()
        {
            curHeight = float.Parse(Owner.CurValue);
        }

        /// <summary>
        ///
        /// </summary>
        public override void SetWidth()
        {
            curWidth = float.Parse(Owner.CurValue);
        }

        /// <summary>
        ///
        /// </summary>
        public override void SetY()
        {
            curY = float.Parse(Owner.CurValue);
        }

        /// <summary>
        ///
        /// </summary>
        public override void SetX()
        {
            curX = float.Parse(Owner.CurValue);
        }

        /// <summary>
        ///
        /// </summary>
        public override void SetCurState()
        {
            //state = FindStateById(long.Parse(Owner.CurValue));
            curID = long.Parse(Owner.CurValue);
        }

        private int FindStateById(long id)
        {
            //foreach (var widget in Owner.Window.Widgets)
            for (int i = 0; i < Owner.Window.Widgets.Count; ++i )
            {
                if (Owner.Window.Widgets[i].ID == id)
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
