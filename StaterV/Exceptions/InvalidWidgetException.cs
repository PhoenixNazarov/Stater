﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StaterV.Exceptions
{
    class InvalidWidgetException : Exception
    {
        public InvalidWidgetException(string message) : base(message) { }
    }
}
