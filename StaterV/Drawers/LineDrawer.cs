using System;
using StaterV.Utility;

namespace StaterV.Drawers
{
    [Serializable]
    /// <summary>
    /// Рисует линию от стрелки.
    /// </summary>
    public abstract class LineDrawer : Drawer
    {
        public abstract double GetAngle();

        public abstract Dot GetEnd();

        public bool IntersectsWith(float dotX, float dotY)
        {
            Dot testing = new Dot();
            testing.x = dotX;
            testing.y = dotY;
            return IntersectsWith(testing);
        }
        public abstract bool IntersectsWith(Dot testing);
        public abstract Utility.Dot GetCenter();

        //List<
        //Сдесь должен быть список точек, через которые проходит линия.

        /*
        public Shape StartBox { get; set; }
        public Shape EndBox { get; set; }
         * */

        //TODO: Убрать это.
        public Dot Start { get; set; }
        public Dot End { get; set; }
    }
}
