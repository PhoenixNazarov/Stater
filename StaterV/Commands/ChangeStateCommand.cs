using System.Collections.Generic;
using StaterV.Attributes;
using StaterV.Widgets;

namespace StaterV.Commands
{
    class ChangeStateCommand : Command
    {
        public ChangeStateCommand(CommandParams aParams) : base(aParams)
        {
        }

        private Attributes.StateAttributes oldAttributes = new StateAttributes();

        private static int CompareExecutions(AutomatonExecution lhs, AutomatonExecution rhs)
        {
            var res = lhs.Type.CompareTo(rhs.Type);
            if (res != 0)
            {
                return res;
            }

            return lhs.Name.CompareTo(rhs.Name);
        }

        private void FindDiff(List<AutomatonExecution> oldOne, List<AutomatonExecution> newOne,
                                out List<AutomatonExecution> added, out List<AutomatonExecution> removed)
        {
            added = new List<AutomatonExecution>();
            removed = new List<AutomatonExecution>();
            List<AutomatonExecution> oldSorted = new List<AutomatonExecution>(oldOne);
            oldSorted.Sort(CompareExecutions);

            List<AutomatonExecution> newSorted = new List<AutomatonExecution>(newOne);
            newSorted.Sort(CompareExecutions);

            int iOld = 0, iNew = 0;
            while (true)
            {
                if (iOld >= oldOne.Count)
                {
                    added.AddRange(newSorted.GetRange(iNew, newSorted.Count - iNew));
                    break;
                }

                if (iNew >= newOne.Count)
                {
                    removed.AddRange(oldSorted.GetRange(iOld, oldSorted.Count - iOld));
                    break;
                }

                var cmp = CompareExecutions(oldSorted[iOld], newSorted[iNew]);

                if (cmp == 0)
                {
                    iOld++;
                    iNew++;
                }
                else if (cmp < 0)
                {
                    removed.Add(oldSorted[iOld]);
                    iOld++;
                }
                else 
                {
                    added.Add(newSorted[iNew]);
                    iNew++;
                }
            }
        }

        public override void Execute()
        {
            base.Execute();
            var state = (State)(theParams.TheWidget);

            List<AutomatonExecution> toAdd;
            List<AutomatonExecution> toRemove;

            var p = (ChangeStateParams)theParams;

            if (state.TheAttributes.EntryExecutions == null)
            {
                state.TheAttributes.EntryExecutions = new List<AutomatonExecution>();
            }

            FindDiff(state.TheAttributes.EntryExecutions, p.NewAttributes.EntryExecutions,
                out toAdd, out toRemove);

            oldAttributes = state.TheAttributes.Clone() as StateAttributes;
            state.TheAttributes = p.NewAttributes.Clone() as StateAttributes;

            p.Project.AddMachineObjects(toAdd);
            p.Project.RemoveMachineObjects(toRemove);
        }


        public override void Unexecute()
        {
            var state = (State)(theParams.TheWidget);

            List<AutomatonExecution> toAdd;
            List<AutomatonExecution> toRemove;

            var p = (ChangeStateParams)theParams;

            if (oldAttributes.EntryExecutions == null)
            {
                oldAttributes.EntryExecutions = new List<AutomatonExecution>();
            }

            FindDiff(state.TheAttributes.EntryExecutions, oldAttributes.EntryExecutions,
                out toAdd, out toRemove);

            state.TheAttributes = oldAttributes.Clone() as StateAttributes;

            p.Project.AddMachineObjects(toAdd);
            p.Project.RemoveMachineObjects(toRemove);
        }
    }
}
