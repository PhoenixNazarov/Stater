using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace StaterV.Attributes
{
    public class EditEventWrapper
    {
        public TransitionAttrsLogic Logic { get; set; }
        private EditEventForm form;

        public EditEventWrapper()
        {
            form = new EditEventForm();
            form.Wrapper = this;
        }

        #region From logic
        public ResultDialog Start()
        {
            var res = form.ShowDialog();
            ResultDialog result = ResultDialog.OK;
            switch (res)
            {
                case DialogResult.None:
                    break;
                case DialogResult.OK:
                    break;
                case DialogResult.Cancel:
                    result = ResultDialog.Cancel;
                    break;
                case DialogResult.Abort:
                    result = ResultDialog.Cancel;
                    break;
                case DialogResult.Retry:
                    break;
                case DialogResult.Ignore:
                    break;
                case DialogResult.Yes:
                    break;
                case DialogResult.No:
                    result = ResultDialog.Cancel;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return result;
        }
        #endregion

        #region From form

        public void SetEvent(string name, string comment)
        {
            Logic.SetEvent(name, comment);
        }
        #endregion
    }
}
