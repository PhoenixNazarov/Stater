using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StaterV.Attributes
{
    [Serializable]
    public class TransitionAttributes : Attributes
    {
        public TransitionAttributes() : base()
        {
            //Events = new List<Event>();
            TheEvent = new Event();
            Actions = new List<Action>();
            Effects = new List<AutomatonEffect>();
            Code = new List<string>();
            Guard = "";
        }

        //public List<Event> Events { get; set; }
        public Event TheEvent { get; set; }

        public List<Action> Actions { get; set; }
        public List<AutomatonEffect> Effects { get; set; }

        public List<string> Code { get; set; } 

        /// <summary>
        /// На будущее.
        /// </summary>
        public string Guard { get; set; }
        //public Condition Guard { get; set; }

        public override object Clone()
        {
            var res = new TransitionAttributes();
            res = MemberwiseClone() as TransitionAttributes;
            return res;
        }
    }
}
