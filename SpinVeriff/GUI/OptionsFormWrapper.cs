using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SpinVeriff.GUI
{
    public class OptionsFormWrapper : OptionsWrapper
    {
        private OptionsForm form;

        public override void Init()
        {
            form = new OptionsForm();
            form.Wrapper = this;
            form.CheckDefaults();
        }

        public override OptionsLogic.Result Start()
        {
            var res = form.ShowDialog();
            OptionsLogic.Result result = OptionsLogic.Result.OK;
            switch (res)
            {
                case DialogResult.None:
                    break;
                case DialogResult.OK:
                    break;
                case DialogResult.Cancel:
                    result = OptionsLogic.Result.Cancel;
                    break;
                case DialogResult.Abort:
                    result = OptionsLogic.Result.Cancel;
                    break;
                case DialogResult.Retry:
                    break;
                case DialogResult.Ignore:
                    break;
                case DialogResult.Yes:
                    break;
                case DialogResult.No:
                    result = OptionsLogic.Result.Cancel;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return result;
        }

        public override void DoOk(string formulaLTL, string singleMachineType)
        {
            base.DoOk(formulaLTL, singleMachineType);            
        }

        public override void DoCancel()
        {
        }

        public override void CheckVerifySystem(bool val)
        {
            Owner.CheckVerifySystem(val);
        }

        public override void CheckEntered(bool val)
        {
            Owner.CheckEntered(val);
        }

        public override void EnterObjects()
        {
            Owner.EnterObjects();
        }

        public override void CheckLonely(bool val)
        {
            Owner.CheckLonely(val);
        }

        public override void CheckOnlyOne(bool val)
        {
            Owner.CheckOnlyOne(val);
        }

        public override void AutoCheckVerifySystem(bool val)
        {
            form.SetVeriFySystem(val);
        }

        public override void AutoCheckEntered(bool val)
        {
            form.SetVerifyEntered(val);
        }

        public override void AutoCheckLonely(bool val)
        {
            form.SetVerifyLonely(val);
        }

        public override void AutoCheckOnlyOne(bool val)
        {
            form.SetVerifyOnlyOne(val);
        }

        public override void SetLTL(string ltl)
        {
            form.SetLTL(ltl);
        }
    }
}