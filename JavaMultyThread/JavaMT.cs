using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PluginData;


namespace JavaMultyThread
{
    public class JavaMT : IndependentPlugin
    {
        public override IReturn Start(IParams param)
        {
            IReturn res = new IReturn();

            res.Message = "Done!";
            return res;

        }

        public override bool NeedParams
        {
            get { return true; }
        }
    }
}
