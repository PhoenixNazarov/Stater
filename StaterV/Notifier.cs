using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StaterV
{
    /// <summary>
    /// Реализует паттерн "Наблюдатель".
    /// </summary>
    public class Notifier
    {
        public void Attach(IObserver o)
        {
            observers.Add(o);
        }
        public void Detach(IObserver o)
        {
            observers.Remove(o);
        }

        public void Notify()
        {
            foreach (var o in observers)
            {
                o.Update();
            }
        }

        protected HashSet<IObserver> observers = new HashSet<IObserver>();
    }
}
