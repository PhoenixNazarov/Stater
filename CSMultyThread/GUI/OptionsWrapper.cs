using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSMultyThread.GUI
{
    public class OptionsWrapper
    {
        public OptionsWrapper()
        {
            form = new OptionsForm();
            form.Wrapper = this;
        }

        public OptionsLogic Logic { get; set; }
        private OptionsForm form;

        public string RawText { get; set;}
        public string Namespace { get; set; }

        internal OptionsLogic.Result Start()
        {
            form.GetState();
            var res = form.ShowDialog();
            switch (res)
            {
                case System.Windows.Forms.DialogResult.OK:
                    Logic.RawText = RawText;
                    Logic.Options.Namespace = Namespace;
                    return OptionsLogic.Result.OK;
                default:
                    return OptionsLogic.Result.Cancel;
            }
        }
    }
}
