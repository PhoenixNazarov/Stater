using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PluginData
{
    public class Action
    {
        public string Name { get; set; }

        public Synchronism Synchronism { get; set; }

        public static bool operator ==(Action left, Action right)
        {
            if (ReferenceEquals(left, right))
            {
                return true;
            }

            if (((object)left == null) || ((object)right == null))
            {
                return false;
            }

            return left.Name == right.Name;
        }

        public static bool operator !=(Action left, Action right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            var other = obj as Action;
            if (other == null)
            {
                return false;
            }

            return this == other;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public override string ToString()
        {
            return Name + ": " + Synchronism;
        }
    }

    public class ActionComparer : IEqualityComparer<Action>
    {
        public bool Equals(Action x, Action y)
        {
            return x == y;
        }

        public int GetHashCode(Action obj)
        {
            return obj.GetHashCode();
        }
    }
}
