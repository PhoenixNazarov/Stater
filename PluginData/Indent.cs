using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PluginData
{
    public class Indent
    {
        public Indent()
        {
            indentLevel = 0;
        }

        public Indent(int level)
        {
            indentLevel = level;
        }

        private int indentLevel = 0;

        public string Do()
        {
            var res = "";
            for (int i = 0; i < indentLevel; i++)
            {
                res += "\t";
            }
            return res;
        }

        public void Reset()
        {
            indentLevel = 0;
        }

        public static Indent operator ++(Indent i)
        {
            ++i.indentLevel;
            return i;
        }

        public static Indent operator --(Indent i)
        {
            --i.indentLevel;
            return i;
        }

        public override string ToString()
        {
            return "indentLevel = " + indentLevel;
        }
    }
}
