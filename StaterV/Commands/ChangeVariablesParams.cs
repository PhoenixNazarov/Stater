using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StaterV.StateMachine;

namespace StaterV.Commands
{
    class ChangeVariablesParams : CommandParams
    {
        public List<Variable> NewVariables { get; set; }
    }
}
