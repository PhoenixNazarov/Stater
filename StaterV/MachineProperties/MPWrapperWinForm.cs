using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StaterV.MachineProperties
{
    public class MPWrapperWinForm
    {
        public MPWrapperWinForm()
        {
            form = new MachinePropertiesForm();
            form.Wrapper = this;
        }

        private MachinePropertiesForm form;
        public MachinePropertiesLogic Logic { get; set; }

        internal void Start()
        {
            var res = form.ShowDialog();
        }

        internal void SetHeader(string header)
        {
            form.SetHeader(header);
        }
    }
}
