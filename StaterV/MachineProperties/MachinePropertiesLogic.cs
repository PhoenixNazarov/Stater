using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StaterV.MachineProperties
{
    public class MachinePropertiesLogic
    {
        public MachinePropertiesLogic(PluginData.StateMachine sm)
        {
            machine = sm;
            wrapper = new MPWrapperWinForm();
            wrapper.Logic = this;
            SetHeader();
        }

        private MPWrapperWinForm wrapper;
        private PluginData.StateMachine machine;

        public void Start()
        {
            wrapper.Start();
        }

        private void SetHeader()
        {
            wrapper.SetHeader(machine.Name + " properties");
        }
    }
}
