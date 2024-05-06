using System;
using System.Text;

namespace StaterV.Attributes
{
    [Serializable]
    public class Event
    {
        public string Name { get; set; }
        public string Comment { get; set; }
        public string SafeName
        {
            get
            {
                if (Name == null)
                {
                    return null;
                }
                return GetSafeName(Name);
            }
        }

        public static string GetLocalName(string machineName)
        {
            if (machineName.IndexOf('.') == -1)
            {
                return machineName;
            }
            var prefix = machineName.Split('.')[0];
            var localName = machineName.Substring(prefix.Length + 1);
            //return (prefix == machineName) ? machineName : GetSafeName(localName);
            return GetSafeName(localName);
        }

        public void ConvertName()
        {
            Name = GetLocalName(Name);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(Name);
            sb.Append(" (");
            sb.Append(Comment);
            sb.Append(")");
            return sb.ToString();
        }

        public static string GetSafeName(string name)
        {
            string res = name.Replace('.', '_');//.Replace("*", "epsilon");
            if (res.Equals("event"))
            {
                res = "_event";
            }
            return res;
        }

        public static bool operator <(Event lhs, Event rhs)
        {
            if (lhs.Name != null) if (lhs.Name != null)
            {
                if (lhs.Name.CompareTo(rhs.Name) < 0)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool operator >(Event lhs, Event rhs)
        {
            return (!(lhs < rhs)) && (lhs != rhs);
        }

        public static bool operator ==(Event lhs, Event rhs)
        {
            if (ReferenceEquals(lhs, rhs))
            {
                return true;
            }
            if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null))
            {
                return false;
            }

            return lhs.Name == rhs.Name;
        }

        public static bool operator !=(Event lhs, Event rhs)
        {
            return !(lhs == rhs);
        }

        public bool Equals(Event obj)   
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;

            return Name == obj.Name;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(Event)) return false;
            return Equals((Event)obj);
        }

        public static Event CreateEpsilon()
        {
            Event eps = new Event();
            eps.Name = "*";
            eps.Comment = "No event";
            return eps;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Name != null ? Name.GetHashCode() : 0)*397) ^ (Comment != null ? Comment.GetHashCode() : 0);
            }
        }
    }
}
