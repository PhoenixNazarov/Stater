using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StaterV.Commands
{
    /// <summary>
    /// Составная команда.
    /// </summary>
    public class CompositeParams : CommandParams
    {
        public CompositeParams()
        {
            Commands = new List<Command>();
        }
        public List<Command> Commands { get; set; }
    }
}
