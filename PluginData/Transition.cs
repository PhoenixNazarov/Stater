using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PluginData
{
    public class Transition
    {
        public Transition()
        {
            Event = new Event {Name = "*"};
            Guard = "";

            Actions = new List<Action>();
            Effects = new List<AutomatonEffect>();
            Code = new List<string>();
        }

        public string Name { get; set; }
        public UID ID { get; set; }

        public State Start { get; set; }
        public State End { get; set; }
        public Event Event { get; set; }

        public List<Action> Actions { get; set; }
        public List<AutomatonEffect> Effects { get; set; }
        public List<String> Code { get; set; }
        public string Guard { get; set; }

        public bool GuardExists()
        {
            if (Guard == null)
            {
                return false;
            }

            if (Guard.Trim() == "")
            {
                return false;
            }

            return true;
        }
    }
}
