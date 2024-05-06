using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StaterV.PluginManager;
using StaterV.Project;

namespace SpinVeriff
{
    public class VeriffParams : PluginParams
    {
        public VeriffParams(ProjectManager _pm) : base(_pm)
        {
        }

        public Options Options { get; set; }
    }
}
