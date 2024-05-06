using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StaterV.Commands
{
    class RenameEventCommand : Command
    {
        public RenameEventCommand(CommandParams aParams) : base(aParams)
        {
        }

        public override void Unexecute()
        {
            throw new NotImplementedException();
        }
    }
}
