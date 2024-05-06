using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StaterV.Commands;

namespace StaterV.Attributes
{
    public class TransitionAttrsLogic
    {
        private readonly TransitionAttributesFormWrapper wrapper = new TransitionAttributesFormWrapper();
        public Windows.WindowBase Window { get; set; }
        public Project.ProjectManager PM { get; set; }

        private readonly EditEventWrapper editWrapper = new EditEventWrapper();

        public TransitionAttrsLogic()
        {
            wrapper.Logic = this;
            editWrapper.Logic = this;
        }

        public TransitionAttributes TheAttributes { get; set; }
        public List<StateMachine.StateMachine> Machines { get; set; }

        public ResultDialog Start()
        {
            wrapper.TheAttributes = TheAttributes;
            LoadEvents();
            return wrapper.Start();
        }

        private void LoadEvents()
        {
            List<Event> evtList = new List<Event>();

            evtList.Add(Event.CreateEpsilon());

            //var data = Window.TheWindowData as Windows.StatemachineData;
            //foreach (var @event in data.Events)
            //{
            //    if (!evtList.Contains(@event))
            //    {
            //        evtList.Add(@event);
            //    }
            //}

            if (Window.MyProject != null)
            {
                if (Window.MyProject.Info.Machines != null)
                {
                    //foreach (var machine in Window.MyProject.Info.Machines)
                    if (Machines != null)
                    {
                        /*
                        foreach (var machine in Machines)
                        {
                            //if (machine.Name == Window.DiagramName)
                            //{
                            //    continue;
                            //}
                            foreach (var @event in machine.Events)
                            {
                                if (!evtList.Contains(@event))
                                {
                                    evtList.Add(@event);
                                }
                            }
                        }
                         */

                        foreach (var @event in from machine in Machines from @event in machine.Events 
                                               where !evtList.Contains(@event) select @event)
                        {
                            evtList.Add(@event);
                        }
                    }
                }
            }

            wrapper.ResetEvents(evtList);
        }

        private Event evt = new Event();

        public void ReqNewEvent()
        {
            if (editWrapper.Start() == ResultDialog.OK)
            {
                AddEventParams aeParams = new AddEventParams();
                aeParams.Name = evt.Name;
                aeParams.Comment = evt.Comment;
                aeParams.Window = Window;
                AddEventCommand cmd = new AddEventCommand(aeParams);
                cmd.Execute();

                wrapper.AddEventToList(evt);
            }
        }

        public void EditEvent(Event oldEvent)
        {
            if (oldEvent.Name.Equals(Event.CreateEpsilon().Name))
            {
                return;
            }

            if (editWrapper.Start() == ResultDialog.OK)
            {
                //Save attributes.
                TheAttributes = wrapper.TheAttributes;

                //Execute event edit command.
                EditEventParams p = new EditEventParams();
                p.OldEvent = oldEvent;
                p.NewEvent = evt;
                p.Window = Window;
                EditEventCommand cmd = new EditEventCommand(p);
                cmd.Execute();

                if (wrapper.TheAttributes.TheEvent.Equals(oldEvent))
                {
                    wrapper.TheAttributes.TheEvent = evt;
                }
                wrapper.TheAttributes = TheAttributes;

                LoadEvents();
            }
        }

        public void SetEvent(string _name, string _comment)
        {
            evt.Name = _name;
            evt.Comment = _comment;
        }
    }
}
