using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StaterV.StateMachine;
using StaterV.Windows;

namespace StaterV.Commands
{
    class ChangeVariablesCommand : Command
    {
        public ChangeVariablesCommand(CommandParams aParams) : base(aParams)
        {}

        private List<Variable> oldVariables = new List<Variable>();

        public override void Execute()
        {
            base.Execute();

            var wData = (StatemachineData) theParams.Window.TheWindowData;
            oldVariables = new List<Variable>(wData.Variables);
            var p = (ChangeVariablesParams) theParams;
            wData.Variables = new List<Variable>(p.NewVariables);
        }

        public override void Unexecute()
        {
            var wData = (StatemachineData)theParams.Window.TheWindowData;
            wData.Variables = new List<Variable>(oldVariables);
        }
    }
}
