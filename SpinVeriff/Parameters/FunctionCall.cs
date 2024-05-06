namespace SpinVeriff.Parameters
{
    class FunctionCall
    {
        public string machine;
        public string function;

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

            return (machine == otherCall.machine) && (function == otherCall.function);
        }

        public static bool operator ==(FunctionCall left, FunctionCall right)
        {
            if (ReferenceEquals(left, right))
            {
                return true;
            }

            if (((object)left == null) || ((object)right == null))
            {
                return false;
            }

            return (left.function == right.function) && (left.machine == right.machine);
        }

        public static bool operator !=(FunctionCall left, FunctionCall right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            string str = machine + ":" + function;
            return str.GetHashCode();
        }
    }
}
