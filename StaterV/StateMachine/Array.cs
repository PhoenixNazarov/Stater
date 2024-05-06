using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StaterV.StateMachine
{
    public class Array : Variable
    {
        public int NElements { get; set; }

        //Default value == 0.

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
            res += TypeToString() + " " + Name + " [" + NElements + "];";
            return res;
        }
    }
}
