using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace StaterV.Variables
{
    public class EditVariablesWrapper
    {
        public EditVariablesWrapper()
        {
            form.Wrapper = this;
        }

        public EditVariablesLogic Logic { get; set; }
        private EditVariablesForm form = new EditVariablesForm();

        public string Variables { get; set; }

        #region From logic

        public ResultDialog Start()
        {
            form.SetVariables(Variables);
            var res = form.ShowDialog();
            ResultDialog ret;
            switch (res)
            {
                case DialogResult.None:
                    ret = ResultDialog.Cancel;
                    break;
                case DialogResult.OK:
                    ret = ResultDialog.OK;
                    break;
                case DialogResult.Cancel:
                    ret = ResultDialog.Cancel;
                    break;
                case DialogResult.Abort:
                    ret = ResultDialog.Cancel;
                    break;
                case DialogResult.Retry:
                    ret = ResultDialog.Cancel;
                    break;
                case DialogResult.Ignore:
                    ret = ResultDialog.Cancel;
                    break;
                case DialogResult.Yes:
                    ret = ResultDialog.OK;
                    break;
                case DialogResult.No:
                    ret = ResultDialog.Cancel;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return ret;
        }

        public ResultDialog ReportError(string text)
        {
            var res = form.ReportError(text);
            ResultDialog ret = ResultDialog.OK;
            switch (res)
            {
                case DialogResult.OK:
                    ret = ResultDialog.OK;
                    break;
                case DialogResult.Cancel:
                    ret = ResultDialog.Cancel;
                    break;
            }
            return ret;
        }

        #endregion

        #region From form

        #endregion
    }
}
