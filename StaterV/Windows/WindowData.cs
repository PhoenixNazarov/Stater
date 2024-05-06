using System;
using System.Collections.Generic;
using StaterV.Attributes;
using StaterV.StateMachine;

namespace StaterV.Windows
{
    [Serializable]
    public class WindowData
    {
        public Project.DiagramType Type;
    }

    [Serializable]
    public class StatemachineData : WindowData
    {
        public StatemachineData()
        {
            Type = Project.DiagramType.StateMachine;
            Events = new List<Event>();
            Variables = new List<Variable>();
            AutoReject = false;
        }

        public List<Variable> Variables { get; set; }

        /// <summary>
        /// We consider that the state machine doesn't have a lot of events.
        /// </summary>
        public List<Attributes.Event> Events { get; set; }

        /// <summary>
        /// True: statemachine will reject if it get unexpected event.
        /// </summary>
        public bool AutoReject { get; set; }

        public bool IsEventPresent(string name)
        {
            foreach (var @event in Events)
            {
                if (@event.Name == name)
                {
                    return true;
                }
            }
            return false;
        }

        #region Static type checkers
        public static bool TryExtractData(WindowBase wnd, out StatemachineData data)
        {
            if (wnd.TheWindowData.GetType() != typeof(StatemachineData))
            {
                data = null;
                return false;
            }

            data = (StatemachineData) wnd.TheWindowData;
            return true;
        }

        public static bool TryGetEvents(WindowBase wnd, out List<Attributes.Event> eventList)
        {
            StatemachineData data;
            if (!TryExtractData(wnd, out data))
            {
                eventList = null;
                return false;
            }
            eventList = data.Events;
            return true;
        }

        public static bool TryAddEvent(WindowBase wnd, Attributes.Event evt)
        {
            StatemachineData data;
            if (!TryExtractData(wnd, out data))
            {
                return false;
            }

            data.Events.Add(evt);
            return true;
        }
        #endregion
    }

    public class ConnectionsData : WindowData
    {
        public ConnectionsData()
        {
            Type = Project.DiagramType.Connections;
        }
    }
}