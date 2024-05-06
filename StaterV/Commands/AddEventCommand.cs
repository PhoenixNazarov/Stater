using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StaterV.Commands
{
    public class AddEventCommand : Command
    {
        public AddEventCommand(CommandParams aParams) : base(aParams)
        {
        }

        public override void Execute()
        {
            base.Execute();
            var evt = new Attributes.Event();
            var p = (AddEventParams) theParams;
            evt.Name = p.Name;
            evt.Comment = p.Comment;
            var data = p.Window.TheWindowData as Windows.StatemachineData;

            if (data == null)
            {
                //TODO: report an error!
            }

            data.Events.Add(evt);
        }

        public override void Unexecute()
        {
            throw new NotImplementedException();
        }
    }
}
