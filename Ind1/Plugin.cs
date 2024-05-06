using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PluginData;

namespace Ind1
{
    public class Plugin : IndependentPlugin
    {
        public override IReturn Start(IParams param)
        {
            IReturn res = new IReturn();
            res.Message = "Simple plugin!";
            return res;
        }

        public override bool NeedParams
        {
            get { return false; }
        }
    }
}
