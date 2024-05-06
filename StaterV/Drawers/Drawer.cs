using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StaterV.Widgets;
using StaterV.Windows;

namespace StaterV.Drawers
{
    /// <summary>
    /// Базовый класс для отрисовки.
    /// </summary>
    [Serializable]
    public abstract class Drawer
    {
        public abstract void Draw();

        protected Arrow owner;

        public Arrow Owner
        {
            get { return owner; }
            set { owner = value; }
        }

        protected WindowBase window;

        public WindowBase Window
        {
            get { return window; }
            set { window = value; }
        }

    }
}
