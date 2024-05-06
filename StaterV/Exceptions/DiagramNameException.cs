using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StaterV.Exceptions
{
    class DiagramNameException : Exception
    {
        public DiagramNameException(string message) : base(message) {}
    }
}
