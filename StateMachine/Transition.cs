using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StateMachine
{
    public class Transition
    {
        public UID ID { get; set; }

        public State Start { get; set; }
        public State End { get; set; }
    }
}
