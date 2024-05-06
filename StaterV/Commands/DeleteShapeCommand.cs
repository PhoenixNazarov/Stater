using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StaterV.Commands
{
    public class DeleteShapeCommand : Command
    {
        public DeleteShapeCommand(CommandParams aParams): base(aParams)
        {
        }

        public override void Execute()
        {
            if (!IsSubCommand)
            {
                base.Execute();
            }
            theParams.Window.Widgets.Remove(theParams.TheWidget);
        }

        public override void Unexecute()
        {
            theParams.Window.Widgets.Add(theParams.TheWidget);
        }
    }
}
