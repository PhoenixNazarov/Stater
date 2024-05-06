using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpinVeriff.Parameters
{
    class NestedMachineCall
    {
        public string machine;
        public string nested;

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            var otherCall = obj as FunctionCall;
            if (otherCall == null)
            {
                return false;
            }

            return (machine == otherCall.machine) && (nested == otherCall.function);
        }

        public static bool operator ==(NestedMachineCall left, NestedMachineCall right)
        {
            if (ReferenceEquals(left, right))
            {
                return true;
            }

            if (((object)left == null) || ((object)right == null))
            {
                return false;
            }

            return (left.nested == right.nested) && (left.machine == right.machine);
        }

        public static bool operator !=(NestedMachineCall left, NestedMachineCall right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            string str = machine + "|" + nested;
            return str.GetHashCode();
        }
    }
}
