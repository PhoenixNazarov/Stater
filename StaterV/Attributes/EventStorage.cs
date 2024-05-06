using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StaterV.Widgets;

namespace StaterV.Attributes
{
    /// <summary>
    /// Singleton
    /// </summary>
    [Serializable]
    public class EventStorage
    {
        private EventStorage()
        {
            Storage = new List<EventUsage>();
            var epsilon = new Event();
            var epsUsage = new EventUsage();
            epsUsage.e = epsilon;
            epsilon.Name = "*";
            Storage.Add(epsUsage);
        }

        private static EventStorage _theInstance;

        public static EventStorage GetInstance()
        {
            if (_theInstance == null)
            {
                _theInstance = new EventStorage();
            }
            return _theInstance;
        }

        public static void Deserialize(object obj)
        {
            _theInstance = obj as EventStorage;
        }

        [Serializable]
        public struct EventUsage
        {
            public EventUsage(Event evt)
            {
                e = evt;
                users = new List<Transition>(5);
            }

            public Event e;
            public List<Transition> users;
        }

        public List<EventUsage> Storage { get; private set; }

        public void AddEvent(Event evt)
        {
            foreach (var events in Storage)
            {
                if (events.e.Name == evt.Name)
                {
                    //Нельзя иметь события с одинаковыми названиями!
                    throw (new ArgumentException("Event is already exists!"));
                }
            }
            var evtUsage = new EventUsage(evt);
            Storage.Add(evtUsage);
        }
    }
}
