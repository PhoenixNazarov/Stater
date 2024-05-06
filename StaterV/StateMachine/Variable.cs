using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StaterV.StateMachine
{
    public abstract class Variable
    {
        public enum TypeList
        {
            Bool,
            Int8,
            Int16,
            Int32,
        }

        public TypeList Type { get; set; }
        public string Name { get; set; }
        public bool Volatile { get; set; }
        public bool Param { get; set; }
        public bool External { get; set; }

        public void SetType(string type)
        {
            switch (type)
            {
                case "bool":
                    Type = TypeList.Bool;
                    break;
                case "int8":
                    Type = TypeList.Int8;
                    break;
                case "int16":
                    Type = TypeList.Int16;
                    break;
                case "int32":
                    Type = TypeList.Int32;
                    break;
                default:
                    throw (new ArgumentException("Wrong type declaration!"));
            }
        }

        public string TypeToString()
        {
            string res;
            switch (Type)
            {
                case TypeList.Bool:
                    res = "bool";
                    break;
                case TypeList.Int8:
                    res = "int8";
                    break;
                case TypeList.Int16:
                    res  = "int16";
                    break;
                case TypeList.Int32:
                    res = "int32";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return res;
        }
    }
}
