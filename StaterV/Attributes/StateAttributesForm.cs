using System;
using System.Collections.Generic;
using System.Windows.Forms;
using StaterV.Widgets;

namespace StaterV.Attributes
{
    public partial class StateAttributesForm : Form
    {
        public StateAttributesForm()
        {
            InitializeComponent();
            TheAttributes = new StateAttributes();
        }

        public Windows.WindowBase Window { get; set; }

        public StateAttributes TheAttributes { get; set; }

        public List<string> MachineTypes { get; set; }
        public List<AutomatonExecution> AllExecutions { get; set; }
        public List<StateMachine.StateMachine> Machines { get; set; }

        private ESynchronism GetSynchronism(string name)
        {
            switch (name)
            {
                case "Synchronous":
                    return ESynchronism.Synchronous;
                case "Asynchronous":
                    return ESynchronism.Asynchronous;
                default:
                    throw new ArgumentException("Wrong synchronism name!");
            }
        }

        private enum GridColumns
        {
            Name,
            Comment,
            Synchro,
        }

        private string [] CreateRow(Action a)
        {
            const int len = 3;
            string [] ret = new string[len];

            ret[(int)GridColumns.Name] = a.Name;
            ret[(int) GridColumns.Comment] = a.Comment;
            ret[(int) GridColumns.Synchro] = a.Synchronism.ToString();
            return ret;
        }

        //private string [] CreateRow()

        private void LoadAttributes()
        {
            tbName.Text = TheAttributes.Name;
            tbComment.Text = TheAttributes.Comment;

            cbType.SelectedIndex = (int) TheAttributes.Type;

            dataGridEntry.Rows.Clear();
            foreach (var action in TheAttributes.EntryActions)
            {
                dataGridEntry.Rows.Add(CreateRow(action));
            }
            dataGridExit.Rows.Clear();
            foreach (var action in TheAttributes.ExitActions)
            {
                dataGridExit.Rows.Add(CreateRow(action));
            }

            GetRunningMachines();
            FillEffectsGridView();
            LoadNested();
        }

        private void FillCBNested()
        {
            cbInner.Items.Clear();
            cbInner.Items.AddRange(MachineTypes.ToArray());
            if (Window.TheWindowData.Type == Project.DiagramType.StateMachine)
            {
                cbInner.Items.Remove(Window.DiagramName);
            }
        }



        private void FillEffectsGridView()
        {
            CStateMachine.Items.Clear();
            if (AllExecutions == null)
            {
                return;
            }
            string[] eff = new string[AllExecutions.Count];
            for (int i = 0; i < AllExecutions.Count; i++)
            {
                eff[i] = AllExecutions[i].Type + " " + AllExecutions[i].Name;
            }
            CStateMachine.Items.AddRange(eff);
        }

        private void GetRunningMachines()
        {
            dataGridViewRunMachine.Rows.Clear();
            for (int i = 0; i < TheAttributes.EntryExecutions.Count; i++)
            {
                dataGridViewRunMachine.Rows.Add(CreateRMRow(i));
            }

            //dataGridViewRunMachine[(int)RunmachineCols.Type, i].Value.ToString();
            //dataGridView1.Rows.Add(s1);

        }

        private string [] CreateRMRow(int iMachine)
        {
            const int length = 2;
            var res = new string[length];

            res[(int)RunmachineCols.Type] = TheAttributes.EntryExecutions[iMachine].Type;
            res[(int)RunmachineCols.Name] = TheAttributes.EntryExecutions[iMachine].Name;

            return res;
        }

        private void SaveAttributes()
        {
            TheAttributes.Name = tbName.Text;
            TheAttributes.Comment = tbComment.Text;
            TheAttributes.Type = (State.StateType)cbType.SelectedIndex;

            TheAttributes.ExitActions.Clear();
            for (int i = 0; i < dataGridExit.Rows.Count - 1; i++)
            {
                var newAction = new Action();
                newAction.Synchronism = GetSynchronism(
                    dataGridExit[(int)GridColumns.Synchro, i].Value.ToString());

                newAction.Name = dataGridExit[(int)GridColumns.Name, i].Value.ToString();

                var cm = dataGridExit[(int) GridColumns.Comment, i].Value;
                if (cm != null)
                {
                    newAction.Comment = cm.ToString();
                }
                else
                {
                    newAction.Comment = "";
                }

                TheAttributes.ExitActions.Add(newAction);
            }

            TheAttributes.EntryActions.Clear();
            for (int i = 0; i < dataGridEntry.Rows.Count - 1; i++)
            {
                var newAction = new Action();
                newAction.Synchronism = GetSynchronism(
                    dataGridEntry[(int)GridColumns.Synchro, i].Value.ToString());

                var body = dataGridEntry[(int) GridColumns.Name, i].Value;

                if (body != null)
                {
                    newAction.Name = body.ToString();
                }
                else
                {
                    newAction.Name = "";
                }
                var comment = dataGridEntry[(int) GridColumns.Comment, i].Value;
                if (comment != null)
                {
                    newAction.Comment = comment.ToString();
                }

                else
                {
                    newAction.Comment = "";
                }

                TheAttributes.EntryActions.Add(newAction);
            }

            SaveRunningMachines();
            SaveEffects();
            SaveNested();
        }

        private enum RunmachineCols
        {
            Type,
            Name,
        }

        private void SaveRunningMachines()
        {
            //Some old files may be without this property. So it can be null.
            if (TheAttributes.EntryExecutions == null)
            {
                TheAttributes.EntryExecutions = new List<AutomatonExecution>();
            }

            TheAttributes.EntryExecutions.Clear();
            for (int i = 0; i < dataGridViewRunMachine.Rows.Count - 1; i++)
            {
                AutomatonExecution newExecution = new AutomatonExecution();
                newExecution.Type = dataGridViewRunMachine[(int) RunmachineCols.Type, i].Value.ToString();
                newExecution.Name = dataGridViewRunMachine[(int) RunmachineCols.Name, i].Value.ToString();
                TheAttributes.EntryExecutions.Add(newExecution);
            }
        }

        private void FillMachineTypesBox()
        {
            RunMachineType.Items.Clear();
            RunMachineType.Items.AddRange(MachineTypes.ToArray());
        }

        private void SaveEffects()
        {
            TheAttributes.EntryEffects.Clear();
            //TheAttributes.ExitEffects.Clear();

            SaveManualEffects();
        }

        private void SaveNested()
        {
            if (TheAttributes.NestedMachines == null)
            {
                TheAttributes.NestedMachines = new List<NestedMachine>();
            }

            TheAttributes.NestedMachines.Clear();

            for (int i = 0; i < dataGridViewNested.Rows.Count - 1; i++)
            {
                //TheAttributes.NestedMachines.Add(dataGridViewNested[0, i].Value.ToString());
                TheAttributes.NestedMachines.Add(new NestedMachine 
                {Type = dataGridViewNested[0, i].Value.ToString(), Name = dataGridViewNested[1, i].Value.ToString()});
            }
        }

        private void LoadNested()
        {
            dataGridViewNested.Rows.Clear();
            if (TheAttributes.NestedMachines == null)
            {
                return;
            }

            foreach (var machine in TheAttributes.NestedMachines)
            {
                string [] r = new string[2];
                r[0] = machine.Type;
                r[1] = machine.Name;
                dataGridViewNested.Rows.Add(r);
            }
        }

        private enum ManualGridCols
        {
            Type,
            Name,
            Event,
            Count,
        }

        private void SaveManualEffects()
        {
            for (int i = 0; i < dataGridViewManualEffects.Rows.Count - 1; i++)
            {
                AutomatonEffect newEffect = new AutomatonEffect();
                newEffect.TheEffectType = AutomatonEffect.EffectType.Manual;

                object cellValue;
                cellValue = dataGridViewManualEffects[(int) ManualGridCols.Type, i].Value;
                if (cellValue != null)
                {
                    newEffect.Type = cellValue.ToString();
                }

                cellValue = dataGridViewManualEffects[(int) ManualGridCols.Name, i].Value;
                if (cellValue != null)
                {
                    newEffect.Name = cellValue.ToString();
                }

                cellValue = dataGridViewManualEffects[(int) ManualGridCols.Event, i].Value;
                if (cellValue != null)
                {
                    newEffect.Event = cellValue.ToString();
                }

                newEffect.Description = "";
                newEffect.Synchronism = ESynchronism.Asynchronous;

                TheAttributes.EntryEffects.Add(newEffect);
            }
        }

        private void ChargeData()
        {
            var typeCol = dataGridViewEffects.Columns[0] as DataGridViewComboBoxColumn;

            if (typeCol == null)
            {
                //Жопа.
                throw (new Exceptions.FormStructureException(""));
            }

            FillMachineTypesBox();

            FillManualEffects();

            typeCol.Items.Clear();
        }

        private void FillManualEffects()
        {
            dataGridViewManualEffects.Rows.Clear();

            foreach (var effect in TheAttributes.EntryEffects)
            {
                if (effect.TheEffectType == AutomatonEffect.EffectType.Manual)
                {
                    dataGridViewManualEffects.Rows.Add(CreateManualEffectRow(effect));
                }
            }
        }

        private string [] CreateManualEffectRow(AutomatonEffect effect)
        {
            var res = new string[(int)ManualGridCols.Count];
            res[(int) ManualGridCols.Name] = effect.Name;
            res[(int) ManualGridCols.Type] = effect.Type;
            res[(int) ManualGridCols.Event] = effect.Event;
            return res;
        }

        private enum GridEffectsCols
        {
            Name,
            Event,
        }

        private void HandleValueChange(int row, int column)
        {
            if (column == (int)GridEffectsCols.Name)
            {
                SetEventList(row);
            }
        }

        private void SetEventList(int row)
        {
            if (row > dataGridViewEffects.Rows.Count || row < 0)
            {
                return;
            }

            if ((int) GridEffectsCols.Name > dataGridViewEffects.Columns.Count)
            {
                return;
            }

            var v = dataGridViewEffects[(int) GridEffectsCols.Name, row].Value;
            if (v == null)
            {
                return;
            }

            string val = v.ToString();
            var splitted = val.Split(' ');
            var type = splitted[0];

            StateMachine.StateMachine machine = FindMachineByType(type);

            CEvent.Items.Clear();

            if (machine != null)
            {
                List<string> events = new List<string>(machine.Events.Count);
                foreach (var evt in machine.Events)
                {
                    //events.Add(evt.Name);
                    CEvent.Items.Add(evt);
                }
            }

            CEvent.Items.AddRange();
        }

        private StateMachine.StateMachine FindMachineByType(string type)
        {
            foreach (var machine in Machines)
            {
                if (machine.Name == type)
                {
                    return machine;
                }
            }

            return null;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            //Записать все изменения.
            SaveAttributes();
        }

        private void StateAttributesForm_Activated(object sender, EventArgs e)
        {
            ChargeData();
            LoadAttributes();
            FillCBNested();
        }

        private void dataGridViewEffects_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            HandleValueChange(e.RowIndex, e.ColumnIndex);
        }

        private void dataGridViewEffects_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            HandleValueChange(e.RowIndex, e.ColumnIndex);
        }

        private void dataGridViewEffects_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            HandleValueChange(e.RowIndex, e.ColumnIndex);
        }

        private void dataGridViewEffects_CellValuePushed(object sender, DataGridViewCellValueEventArgs e)
        {
            HandleValueChange(e.RowIndex, e.ColumnIndex);
        }

        private void dataGridViewNested_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show(this, "Error :(", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
