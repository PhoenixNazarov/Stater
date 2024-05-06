using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StaterV.Attributes;
using StaterV.Widgets;
using XMLDiagramParser;
using Action=StaterV.Attributes.Action;

namespace StaterV.XMLDiagramParser
{
    class CAttributes : AAttributes
    {
        public CWidget Owner { get; set; }

        private Widgets.Transition curTransition;
        private int curNested;
        //private Attributes.Action curAction;
        private int curAction;
        private int curExecution;
        private int curEffect;

        private void CreateTransition(bool direction)
        {
            Int64 id = Int64.Parse(Owner.Owner.Owner.CurValue);

            //Определяем, куда вставлять переход.
            var s = (Widgets.State)Owner.Owner.CurWidget;
            var container = (direction) ? s.OutgoingArrows : s.IncomingArrows;

            var foundTrans = FindTransition(id);
            if (foundTrans != null)
            {
                switch (direction)
                {
                    case true: //start
                        if (foundTrans.Start != null)
                        {
                            throw (new ArgumentException("Outgoing transition with ID == " + id + " already exists!"));
                        }
                        foundTrans.Start = s;
                        break;
                    case false: //end
                        if (foundTrans.End != null)
                        {
                            throw (new ArgumentException("Incoming transition with ID == " + id + " already exists!"));
                        }
                        foundTrans.End = s;
                        break;
                }
                container.Add(foundTrans);
                return;
            }
            foreach (var arrow in container)
            {
                if (arrow.ID == id)
                {
                    //error!
                    throw (new ArgumentException("ID == " + id + " already exists!"));
                }
            }

            var t = new Widgets.Transition(Owner.Owner.Window);
            t.ID = id;
            container.Add(t);
            curTransition = t;
            if (direction)
            {
                t.Start = s;
            }
            else
            {
                t.End = s;
            }
            Owner.Owner.Window.AddWidget(t);
        }

        private Widgets.Transition FindTransition (Int64 id)
        {
            foreach (var widget in Owner.Owner.Window.Widgets)
            {
                var t = widget as Widgets.Transition;
                if (t == null)
                {
                    continue;
                }

                if (t.ID == id)
                {
                    return t;
                }
            }
            return null;
        }

        private Widgets.State FindState(Int64 id)
        {
            return Owner.Owner.Window.Widgets.OfType<State>().FirstOrDefault(s => s.ID == id);
        }

        /// <summary>
        ///
        /// </summary>
        public override void CreateAction()
        {
            var newAction = new Action();
            newAction.Name = "Enter_Name";
            newAction.Synchronism = Attributes.ESynchronism.Synchronous;
            var t = FindTransition(Owner.Owner.CurID);
            curAction = t.TheAttributes.Actions.Count;
            t.TheAttributes.Actions.Add(newAction);
        }

        /// <summary>
        ///
        /// </summary>
        public override void CreateExecution()
        {
            var newExecution = new AutomatonExecution();

            var w = Owner.Owner.CurWidget;
            var state = w as StaterV.Widgets.State;
            if (state != null)
            {
                curExecution = state.TheAttributes.EntryExecutions.Count;
                state.TheAttributes.EntryExecutions.Add(newExecution);
            }
        }

        /// <summary>
        ///
        /// </summary>
        public override void CreateEffect()
        {
            var newEffect = new AutomatonEffect();

            var state = Owner.Owner.CurWidget as Widgets.State;
            if (state != null)
            {
                curEffect = state.TheAttributes.EntryEffects.Count;
                state.TheAttributes.EntryEffects.Add(newEffect);
            }
        }

        /// <summary>
        ///
        /// </summary>
        public override void SetName()
        {
            ((Widgets.State) Owner.Owner.CurWidget).TheAttributes.Name = Owner.Owner.Owner.CurValue;
        }

        /// <summary>
        ///
        /// </summary>
        public override void SetType()
        {
            Widgets.State w = ((Widgets.State) Owner.Owner.CurWidget);
            w.TheAttributes.Type =
                (Widgets.State.StateType) int.Parse(Owner.Owner.Owner.CurValue);

            if (w.TheAttributes.Type == Widgets.State.StateType.Start)
            {
                //Owner.Owner.Window.sta
            }
        }

        /// <summary>
        ///
        /// </summary>
        public override void CreateOutTransition()
        {
            CreateTransition(true);
        }

        /// <summary>
        ///
        /// </summary>
        public override void CreateInTransition()
        {
            CreateTransition(false);
        }

        /// <summary>
        ///
        /// </summary>
        public override void SetEventName()
        {
            FindTransition(Owner.Owner.CurID).TheAttributes.TheEvent.Name = Owner.Owner.Owner.CurValue;
        }

        /// <summary>
        ///
        /// </summary>
        public override void SetEventComment()
        {
            FindTransition(Owner.Owner.CurID).TheAttributes.TheEvent.Comment = Owner.Owner.Owner.CurValue;
        }

        /// <summary>
        ///
        /// </summary>
        public override void SetActionSynchro()
        {
            var t = FindTransition(Owner.Owner.CurID);
            t.TheAttributes.Actions[curAction].Synchronism
                = (Attributes.ESynchronism) (int.Parse(Owner.Owner.Owner.CurValue));
        }

        /// <summary>
        ///
        /// </summary>
        public override void SetActionComment()
        {
            var t = FindTransition(Owner.Owner.CurID);
            t.TheAttributes.Actions[curAction].Comment = Owner.Owner.Owner.CurValue;
        }

        /// <summary>
        ///
        /// </summary>
        public override void SetActionName()
        {
            var t = FindTransition(Owner.Owner.CurID);
            t.TheAttributes.Actions[curAction].Name = Owner.Owner.Owner.CurValue;
        }

        /// <summary>
        ///
        /// </summary>
        public override void SetNestedName()
        {
            var attr = ((Widgets.State)Owner.Owner.CurWidget).TheAttributes;

            attr.NestedMachines[curNested].Name = Owner.Owner.Owner.CurValue;
        }

        /// <summary>
        ///
        /// </summary>
        public override void SetNestedType()
        {
            var attr = ((Widgets.State)Owner.Owner.CurWidget).TheAttributes;

            attr.NestedMachines[curNested].Type = Owner.Owner.Owner.CurValue;
        }

        /// <summary>
        ///
        /// </summary>
        public override void SetExecutionType()
        {
            var state = Owner.Owner.CurWidget as StaterV.Widgets.State;
            if (state != null)
            {
                state.TheAttributes.EntryExecutions[curExecution].Type = Owner.Owner.Owner.CurValue;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public override void SetExecutionName()
        {
            var state = Owner.Owner.CurWidget as Widgets.State;
            if (state != null)
            {
                state.TheAttributes.EntryExecutions[curExecution].Name = Owner.Owner.Owner.CurValue;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public override void SetEffectType()
        {
            var state = Owner.Owner.CurWidget as Widgets.State;
            if (state != null)
            {
                state.TheAttributes.EntryEffects[curEffect].Type = Owner.Owner.Owner.CurValue;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public override void SetEffectName()
        {
            var state = Owner.Owner.CurWidget as Widgets.State;
            if (state != null)
            {
                state.TheAttributes.EntryEffects[curEffect].Name = Owner.Owner.Owner.CurValue;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public override void SetEffectEvent()
        {
            var state = Owner.Owner.CurWidget as Widgets.State;
            if (state != null)
            {
                state.TheAttributes.EntryEffects[curEffect].Event = Owner.Owner.Owner.CurValue;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public override void SetEffectSynchro()
        {
            var state = Owner.Owner.CurWidget as Widgets.State;
            if (state != null)
            {
                state.TheAttributes.EntryEffects[curEffect].Synchronism = 
                    (ESynchronism)int.Parse(Owner.Owner.Owner.CurValue);
            }
        }

        /// <summary>
        ///
        /// </summary>
        public override void SetEffectDescription()
        {
            var state = Owner.Owner.CurWidget as Widgets.State;
            if (state != null)
            {
                state.TheAttributes.EntryEffects[curEffect].Description = Owner.Owner.Owner.CurValue;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public override void SetEffectET()
        {
            var state = Owner.Owner.CurWidget as Widgets.State;
            if (state != null)
            {
                state.TheAttributes.EntryEffects[curEffect].TheEffectType = 
                    (AutomatonEffect.EffectType)int.Parse(Owner.Owner.Owner.CurValue);
            }
        }

        public override void SetCode()
        {
            FindTransition(Owner.Owner.CurID).TheAttributes.Code.AddRange(Owner.Owner.Owner.CurValue.Split('\n'));
        }

        public override void CreateGuard()
        {
            FindTransition(Owner.Owner.CurID).TheAttributes.Guard = Owner.Owner.Owner.CurValue;
        }

        /// <summary>
        ///
        /// </summary>
        public override void CreateNestedMachine()
        {
            /*
            var attr = ((Widgets.State) Owner.Owner.CurWidget).TheAttributes;
            if (attr.NestedMachines == null)
            {
                attr.NestedMachines = new List<Attributes.NestedMachine>();
            }
            attr.NestedMachines.Add(Owner.Owner.Owner.CurValue);
             * */
            var newNested = new Attributes.NestedMachine();
            newNested.Name = "Enter_Name";
            newNested.Type = "Enter_Type";
            var attr = ((Widgets.State)Owner.Owner.CurWidget).TheAttributes;
            if (attr.NestedMachines == null)
            {
                attr.NestedMachines = new List<Attributes.NestedMachine>();
            }
            curNested = attr.NestedMachines.Count;
            attr.NestedMachines.Add(newNested);
        }

        public override void CreateEntryAction()
        {
            var newAction = new Action();
            newAction.Name = "Enter_Name";
            newAction.Synchronism = Attributes.ESynchronism.Synchronous;
            var s = FindState(Owner.Owner.CurID);
            curAction = s.TheAttributes.EntryActions.Count;
            s.TheAttributes.EntryActions.Add(newAction);
        }

        public override void SetEntryActionName()
        {
            var s = FindState(Owner.Owner.CurID);
            s.TheAttributes.EntryActions[curAction].Name = Owner.Owner.Owner.CurValue;
        }

        public override void SetEntryActionComment()
        {
            var s = FindState(Owner.Owner.CurID);
            s.TheAttributes.EntryActions[curAction].Comment = Owner.Owner.Owner.CurValue;
        }

        public override void SetEntryActionSynchro()
        {
            var s = FindState(Owner.Owner.CurID);
            s.TheAttributes.EntryActions[curAction].Synchronism
                = (Attributes.ESynchronism)(int.Parse(Owner.Owner.Owner.CurValue));
        }
    }
}
