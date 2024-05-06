using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PluginData
{
    public class UID
    {
        public UID()
        {}

        public UID(Int64 val)
        {
            Value = val;
        }

        public Int64 Value { get; set; }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
