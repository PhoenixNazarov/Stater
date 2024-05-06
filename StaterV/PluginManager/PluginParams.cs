using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StaterV.PluginManager
{
    public class PluginParams
    {
        public PluginParams(Project.ProjectManager _pm)
        {
            pm = _pm;
        }

        public readonly Project.ProjectManager pm;
    }
}
