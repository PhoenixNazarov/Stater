using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PluginData
{
    public class IParams
    {
        public List<StateMachine> Machines { get; set; }

        /// <summary>
        /// Список автоматов, которые будут запущены внешней средой.
        /// </summary>
        public List<AutomatonExecution> StartExecutions { get; set; }

        public string WorkDirectory { get; set; }

    }
}
