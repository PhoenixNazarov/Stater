using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StaterV.Exceptions
{
    public class InvalidDiagramException : Exception
    {
        public InvalidDiagramException(string message) : base(message) { }
    }
}
