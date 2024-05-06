using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StaterV.Exceptions
{
    public class FormStructureException : Exception
    {
        public FormStructureException(string _message)
            : base(_message)
        {            
        }
    }
}
