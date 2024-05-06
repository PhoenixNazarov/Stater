using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StaterV.Commands
{
    class MoveWidgetCommand : Command
    {
        public MoveWidgetCommand(CommandParams aParams) : base(aParams)
        {
        }

        public override void Execute()
        {
            base.Execute();
            widget = theParams.TheWidget;
            widget.X += theParams.Width;
            widget.Y += theParams.Height;
        }

        public override void Unexecute()
        {
            widget = theParams.TheWidget;
            widget.X -= theParams.Width;
            widget.Y -= theParams.Height;
        }
    }
}
