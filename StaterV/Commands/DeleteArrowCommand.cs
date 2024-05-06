using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StaterV.Widgets;

namespace StaterV.Commands
{
    public class DeleteArrowCommand : Command
    {
        public DeleteArrowCommand(CommandParams aParams) : base(aParams)
        {
        }

        private Arrow arr;

        public override void Execute()
        {
            base.Execute();
            arr = (Arrow) theParams.TheWidget;
            arr.Start.OutgoingArrows.Remove(arr);
            arr.End.IncomingArrows.Remove(arr);
            theParams.Window.Widgets.Remove(arr);
        }

        public override void Unexecute()
        {
            arr.Start.OutgoingArrows.Add(arr);
            arr.End.IncomingArrows.Add(arr);
            theParams.Window.Widgets.Add(arr);
        }
    }
}
