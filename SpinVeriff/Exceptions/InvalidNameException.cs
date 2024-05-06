using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpinVeriff.Exceptions
{
    class InvalidNameException : Exception
    {
        public InvalidNameException(string message) : base(message)
        {}
    }
}
