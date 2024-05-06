using System;
using System.Text;
using System.Windows.Forms;
using System.Collections.Generic;
using StaterV.Commands;

namespace StaterV.Attributes
{
    public partial class TransitionAttributesForm : Form
    {
        public TransitionAttributesForm()
        {
            InitializeComponent();
        }

        public TransitionAttributesFormWrapper Wrapper { get; set; }

        //private EditEventForm editForm = new EditEventForm();
        //private EventStorage eventStorage = EventStorage.GetInstance();

        public TransitionAttributes TheAttributes { get; set; }

        /*
        private void AddNewEvent(Event evt)
        {
            //Exec a command here.
            Commands.AddEventParams aeParams = new AddEventParams();
            aeParams.Name = evt.Name;
            aeParams.Comment = evt.Comment;
            AddEventCommand cmd = new AddEventCommand(aeParams);

            try
            {
                eventStorage.AddEvent(evt);
                //TheAttributes.Events.Add(evt);
                TheAttributes.TheEvent = evt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);   
            }
        }
         * */

        private void ResetComboBox()
        {
            comboBoxEvents.Items.Clear();
            /*
            foreach (var eventUsage in eventStorage.Storage)
            {
                comboBoxEvents.Items.Add(eventUsage.e.Name);

                /*
                if (eventUsage.e.Name == TheAttributes.TheEvent.Name)
                {
                    
                }
                 * * /
            }
            comboBoxEvents.SelectedItem = TheAttributes.TheEvent.Name;
             * */
        }

        private void ResetDataGrid()
        {
            dataGridEvents.Rows.Clear();

            /*
            foreach (var eventUsage in eventStorage.Storage)
            {
                dataGridEvents.Rows.Add(CreateRow(eventUsage.e));
            }
             * */
        }

        private enum GridEventsColumns
        {
            Name,
            Comment,
        }

        private string [] CreateRow(Event evt)
        {
            const int len = 2;
            var ret = new string[len];

            ret[(int) GridEventsColumns.Name] = evt.Name;
            ret[(int) GridEventsColumns.Comment] = evt.Comment;

            return ret;
        }

        private void buttonNewEvent_Click(object sender, EventArgs e)
        {
            Wrapper.ReqNewEvent();
            /*
            editForm.TheEvent = new Event();
            var res = editForm.ShowDialog();
            if (res == DialogResult.OK)
            {
                //Добавить событие в Storage.
                AddNewEvent(editForm.TheEvent);

                //Добавить событие на grid.
                dataGridEvents.Rows.Add(CreateRow(editForm.TheEvent));

                //Добавить событие в combobox
                comboBoxEvents.Items.Add(editForm.TheEvent.Name);
            }
             * */
        }

        private Event ExtractEventFromComboBox()
        {
            var index = comboBoxEvents.SelectedIndex;
            if (index >= 0 && index < localEvents.Count)
            {
                return localEvents[index];
            }
            return null;
            //return eventStorage.Storage[index].e;
        }

        private string [] CreateActionRow(Action action)
        {
            const int len = 2;
            var ret = new string[len];
            ret[0] = action.Name;
            ret[1] = action.Comment;
            return ret;
        }

        private void ResetActions()
        {
            dataGridViewTransActions.Rows.Clear();
            if (TheAttributes == null)
            {
                return;
            }

            if (TheAttributes.Actions == null)
            {
                return;
            }

            foreach (var action in TheAttributes.Actions)
            {
                dataGridViewTransActions.Rows.Add(CreateActionRow(action));
            }
        }

        private List<Action> ExtractActions()
        {
            List<Action> actions = new List<Action>();
            actions.Clear();
            for (int i = 0; i < dataGridViewTransActions.Rows.Count - 1; i++)
            {
                if (dataGridViewTransActions[0, i].Value == null)
                {
                    continue;
                }

                var newAction = new Action();
                newAction.Synchronism = ESynchronism.Asynchronous;
                newAction.Name = dataGridViewTransActions[0, i].Value.ToString();

                newAction.Comment = (dataGridViewTransActions[1, i].Value == null)?
                    "" : dataGridViewTransActions[1, i].Value.ToString();

                actions.Add(newAction);
            }
            return actions;
        }

        private void SaveAttributes()
        {
            //TheAttributes.Name = 
            TheAttributes.TheEvent = ExtractEventFromComboBox();
            //TheAttributes.TheEvent.Name = comboBoxEvents.SelectedItem.ToString();
            TheAttributes.Actions = ExtractActions();
            SaveCode();
            SaveConditions();
        }

        private void SaveConditions()
        {
            TheAttributes.Guard = tbConditions.Text;
        }

        private void LoadConditions()
        {
            tbConditions.Text = TheAttributes.Guard;
        }

        private void SaveCode()
        {
            var code = tbCode.Text.Split('\n');
            TheAttributes.Code = new List<string>(code);
        }

        private void LoadCode()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var line in TheAttributes.Code)
            {
                sb.AppendLine(line);
            }
            tbCode.Text = sb.ToString();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            //Сохранить все атрибуты.
            SaveAttributes();
        }

        private void buttonEditEvent_Click(object sender, EventArgs e)
        {
            //var res = editForm.ShowDialog();
            DoEditEvent();
        }

        private void DoEditEvent()
        {
            Wrapper.EditEvent(ExtractEventFromComboBox());
        }

        private void SelectEventInCombobox()
        {
            if (TheAttributes.TheEvent != null)
            {
                int index = 
                    localEvents.FindIndex
                    (delegate(Event evt) { return evt.Equals(TheAttributes.TheEvent); });

                comboBoxEvents.SelectedIndex = index;
            }
        }

        private void TransitionAttributesForm_Activated(object sender, EventArgs e)
        {
            //ResetComboBox();
            //ResetDataGrid();
        }

        public void AddEventToList(Event evt)
        {
            localEvents.Add(evt);
            dataGridEvents.Rows.Add(CreateRow(evt));
            comboBoxEvents.Items.Add(evt.ToString());
        }

        private List<Event> localEvents = new List<Event>();

        public void ResetEvents(List<Event> evtList)
        {
            localEvents.Clear();
            //localEvents.AddRange(evtList);

            ResetComboBox();
            ResetDataGrid();
            ResetActions();
            LoadCode();
            LoadConditions();

            foreach (var evt in evtList)
            {
                AddEventToList(evt);
            }

            SelectEventInCombobox();
        }
    }
}
