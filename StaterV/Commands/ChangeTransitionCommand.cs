using System;
using StaterV.Widgets;

namespace StaterV.Commands
{
    class ChangeTransitionCommand : Command
    {
        public ChangeTransitionCommand(CommandParams aParams) : base(aParams)
        {
        }

        private Attributes.TransitionAttributes oldAttributes = new Attributes.TransitionAttributes();

        public override void Execute()
        {
            base.Execute();
            var transition = (Transition)theParams.TheWidget;

            var p = theParams as ChangeTransitionParams;
            oldAttributes = transition.TheAttributes.Clone() as Attributes.TransitionAttributes;
            transition.TheAttributes = p.NewAttributes.Clone() as Attributes.TransitionAttributes;
        }

        public override void Unexecute()
        {
            var transition = (Transition) theParams.TheWidget;
            transition.TheAttributes = oldAttributes.Clone() as Attributes.TransitionAttributes;
        }
    }
}
