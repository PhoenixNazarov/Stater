using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace StaterV.Attributes
{
    public class TransitionAttributesFormWrapper
    {
        private TransitionAttributesForm form;
        public TransitionAttrsLogic Logic { get; set; }

        public TransitionAttributesFormWrapper()
        {
            form = new TransitionAttributesForm();
            form.Wrapper = this;
        }

        public ResultDialog Start()
        {
            ResultDialog res = ResultDialog.OK;
            var result = form.ShowDialog();

            switch (result)
            {
                case DialogResult.None:
                    break;
                case DialogResult.OK:
                    break;
                case DialogResult.Cancel:
                    res = ResultDialog.Cancel;
                    break;
                case DialogResult.Abort:
                    res = ResultDialog.Cancel;
                    break;
                case DialogResult.Retry:
                    break;
                case DialogResult.Ignore:
                    break;
                case DialogResult.Yes:
                    break;
                case DialogResult.No:
                    res = ResultDialog.Cancel;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return res;
        }

        #region To form

        public void AddEventToList(Event evt)
        {
            form.AddEventToList(evt);
        }

        public void ResetEvents(List<Event> evtList)
        {
            form.ResetEvents(evtList);
        }

        public TransitionAttributes TheAttributes
        {
            get { return form.TheAttributes; }
            set { form.TheAttributes = value; }
        }
        #endregion

        #region From form

        public void ReqNewEvent()
        {
            Logic.ReqNewEvent();
        }

        public void EditEvent(Event @event)
        {
            Logic.EditEvent(@event);
        }
        #endregion

    }
}
