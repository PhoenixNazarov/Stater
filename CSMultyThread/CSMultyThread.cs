using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSMultyThread.GUI;
using PluginData;

namespace CSMultyThread
{
    public class CSMultyThread : IndependentPlugin
    {
        public override IReturn Start(IParams param)
        {
            IReturn res = new IReturn();

            OptionsLogic logic = new OptionsLogic();
            logic.Start(param.WorkDirectory);

            CodeGenerator gen = new CodeGenerator(param);
            gen.AddedExecutions = logic.Executions;
            //gen.Namespace = logic.Namespace;
            gen.Options = logic.Options;
            gen.Generate();

            res.Message = "Y";
            return res;
        }

        public override bool NeedParams
        {
            get { return true; }
        }
    }
}
