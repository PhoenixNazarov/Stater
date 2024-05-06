using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PluginData
{
    class CommentVariable : Variable
    {
        public CommentVariable()
        {
            Name = "//";
        }
        public CommentVariable(string n)
        {
            if (n.IndexOf("//") != 0)
            {
                Name = "//" + n;
            } 
            else
            {
                Name = n;
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
