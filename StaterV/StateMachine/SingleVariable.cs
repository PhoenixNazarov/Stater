using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StaterV.StateMachine
{
    public class SingleVariable : Variable
    {
        public string Value { get; set; }

        public override string ToString()
        {
            string res = "";

            if (External)
            {
                res += "external ";
            }

            if (Param)
            {
                res += "param ";
            }

            if (Volatile)
            {
                res += "volatile ";
            }
            res += TypeToString() + " " + Name + " = " + Value + ";";
            return res;
        }
    }
}
