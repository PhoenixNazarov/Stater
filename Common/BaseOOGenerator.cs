using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using StaterV;
using StaterV.PluginManager;
using StaterV.StateMachine;

namespace Common
{
    public abstract class BaseOOGenerator : ButtonPlugin
    {
        public override PluginRetVal Start(PluginParams pluginParams)
        {
            var machineList = pluginParams.pm.ExportStateMachines();
            foreach (var machine in machineList)
            {
                path = machine.FileName;
                GenerateCode(machine);
            }

            PluginRetVal res = new PluginRetVal();
            res.signal = PluginRetVal.Signal.OK;
            return res;
        }

        protected string path;

        protected const string statesEnumName = "States";
        protected const string transitionsEnumName = "Transitions";
        protected const string eventsEnumName = "Events";
        protected const string stateVariableName = "state";
        protected const string eventArg = "_event";

        protected int indentCount;
        protected void GenerateCode(StaterV.StateMachine.StateMachine machine)
        {
            CreateFile();
            indentCount = 0;

            WriteClassBeginning();

            WriteClassEnding();
        }

        protected StreamWriter sw;
        protected virtual void CreateFile()
        {
            //sw = new StreamWriter();
        }

        protected abstract void WriteClassBeginning();
        protected abstract void WriteClassEnding();

        protected abstract void WriteStatesEnum();
        protected abstract void WriteEventsEnum();
    }
}
