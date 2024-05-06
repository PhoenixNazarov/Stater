using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PluginData
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
        public UID ID { get; set; }
        public bool Param { get; set; }
        public bool Volatile { get; set; }
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
    }
}
