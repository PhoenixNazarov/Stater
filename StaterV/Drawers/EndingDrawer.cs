using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StaterV.Drawers
{
    /// <summary>
    /// Рисует наконечник стрелки.
    /// </summary>
    [Serializable]
    public abstract class EndingDrawer : Drawer
    {
        public Utility.Dot Coord;
        public double Angle { get; set; }
    }
}
