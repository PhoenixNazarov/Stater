using System;
using System.Collections.Generic;
using StaterV;
using StaterV.PluginManager;
using StaterV.StateMachine;

namespace SLXParser
{
    public class SLXPlugin: ButtonPlugin
    {
        public override PluginRetVal Start(PluginParams pluginParams)
        {
            var result = new PluginRetVal();
            const string path = "./BR_GATES_HDL.slx";

            var parser = new Parser(path);
            var stateflow = parser.Parse();
            var pluginStateflow = new Translator().Convert(stateflow);
            result.machines = new List<StateMachine>();
            result.machines.Add(pluginStateflow);
        }

        public override PluginRetVal SilentStart(PluginParams pluginParams)
        {
            throw new NotImplementedException();
        }
    }
}