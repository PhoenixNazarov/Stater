using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PluginData;

namespace CSMultyThread
{
    public class Options
    {
        public Options()
        {
            WithEvents = true;
            WithStrings = true;
        }

        public string Namespace { get; set; }
        public List<AutomatonExecution> Executions { get; set; }

        public bool ThreadManager { get; set; }
        public bool WithEvents { get; set; }
        public bool WithStrings { get; set; }
        public bool OverWrite { get; set; }
    }
}
