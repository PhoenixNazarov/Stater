using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StaterV.PluginManager
{
    public struct PluginRetVal
    {
        public override string ToString()
        {
            switch (signal)
            {
                case Signal.OK:
                    return "OK!";
                case Signal.Error:
                    return "Error: " + message;
            }
            return "OK!";
        }

        public enum Signal
        {
            OK,
            Error,
        }

        public Signal signal;
        public string message;
        public List<StateMachine.StateMachine> machines;
    }
}
