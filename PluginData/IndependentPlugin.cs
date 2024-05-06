using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PluginData
{
    public abstract class IndependentPlugin
    {
        public abstract IReturn Start(IParams param);
        public abstract bool NeedParams { get; }
    }
}
