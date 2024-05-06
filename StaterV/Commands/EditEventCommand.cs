using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StaterV.Widgets;

namespace StaterV.Commands
{
    public class EditEventCommand : Command
    {
        public EditEventCommand(CommandParams aParams) : base(aParams)
        {
        }

        public override void Execute()
        {
            base.Execute();

            //Change event in the whole project.
            var p = (EditEventParams)theParams;
            ChangeEventInWindow(p.Window, p.OldEvent, p.NewEvent);
            //p.Project.Info.

            //Reload all opened windows.
        }

        public override void Unexecute()
        {
            var p = (EditEventParams)theParams;
            ChangeEventInWindow(p.Window, p.NewEvent, p.OldEvent);
        }

        private void ChangeEventInWindow(Windows.WindowBase window, Attributes.Event old, Attributes.Event newEvent)
        {
            var data = window.TheWindowData as Windows.StatemachineData;

            if (data == null)
            {
                return;
            }

            for (int i = 0; i < data.Events.Count; i++)
            {
                if (data.Events[i].Equals(old))
                {
                    data.Events[i] = newEvent;
                }
            }

            //Change event in each transition.
            foreach (var w in window.Widgets)
            {
                Transition tr = w as Transition;
                if (tr == null)
                {
                    continue;
                }

                if (tr.TheAttributes.TheEvent.Equals(old))
                {
                    tr.TheAttributes.TheEvent = newEvent;
                }
            }
        }
    }
}
