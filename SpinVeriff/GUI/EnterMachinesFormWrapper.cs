using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SpinVeriff.GUI
{
    public class EnterMachinesFormWrapper : EnterMachinesWrapper
    {
        public EnterMachinesFormWrapper()
        {
            form = new EnterMachinesForm();
            form.Wrapper = this;
        }

        private EnterMachinesForm form;

        #region Interaction with form
        public override void SynchronizeObjects(List<SpinVeriff.Options.ObjectName> objects)
        {
            base.SynchronizeObjects(objects);
            form.SyncText();
        }
        #endregion

        #region Interaction with logic
        public override OptionsLogic.Result Start()
        {
            var r = form.ShowDialog();
            OptionsLogic.Result res = OptionsLogic.Result.OK;
            switch (r)
            {
                case DialogResult.None:
                    break;
                case DialogResult.OK:
                    break;
                case DialogResult.Cancel:
                    res = OptionsLogic.Result.Cancel;
                    break;
                case DialogResult.Abort:
                    res = OptionsLogic.Result.Cancel;
                    break;
                case DialogResult.Retry:
                    break;
                case DialogResult.Ignore:
                    break;
                case DialogResult.Yes:
                    break;
                case DialogResult.No:
                    res = OptionsLogic.Result.Cancel;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return res;
        }
        #endregion
    }
}
