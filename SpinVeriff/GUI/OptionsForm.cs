using System;
using System.Windows.Forms;

namespace SpinVeriff.GUI
{
    public partial class OptionsForm : Form
    {
        public OptionsForm()
        {
            InitializeComponent();
            comboSigleMachine.Items.Add("1");
        }

        public OptionsFormWrapper Wrapper { private get; set; }

        #region From wrapper
        public void CheckDefaults()
        {
            if (cbVerifySystem.Checked)
            {
                Wrapper.CheckVerifySystem(true);
            }
            if (cbEnteredObjects.Checked)
            {
                Wrapper.CheckEntered(true);
            }
            if (cbSingleMachine.Checked)
            {
                Wrapper.CheckOnlyOne(true);
            }
            if (cbLonely.Checked)
            {
                Wrapper.CheckLonely(true);
            }
        }

        public void SetVeriFySystem(bool val)
        {
            cbVerifySystem.Checked = val;
        }
        public void SetVerifyEntered(bool val)
        {
            cbEnteredObjects.Checked = val;
        }
        public void SetVerifyLonely(bool val)
        {
            cbLonely.Checked = val;
        }
        public void SetVerifyOnlyOne(bool val)
        {
            cbSingleMachine.Checked = val;
        }
        #endregion

        private void DoOK()
        {
            string machine = null;
            if (comboSigleMachine.SelectedItem != null)
            {
                machine = comboSigleMachine.SelectedItem.ToString();
            }
            Wrapper.DoOk(tbLTL.Text, machine);
        }

        private void bOK_Click(object sender, EventArgs e)
        {
            DoOK();
        }

        private void bCancel_Click(object sender, EventArgs e)
        {
            Wrapper.DoCancel();
        }

        private void cbVerifySystem_CheckedChanged(object sender, EventArgs e)
        {
            Wrapper.CheckVerifySystem(cbVerifySystem.Checked);
        }

        private void cbEnteredObjects_CheckedChanged(object sender, EventArgs e)
        {
            Wrapper.CheckEntered(cbEnteredObjects.Checked);
        }

        private void bEnterMachines_Click(object sender, EventArgs e)
        {
            Wrapper.EnterObjects();
        }

        private void cbLonely_CheckedChanged(object sender, EventArgs e)
        {
            Wrapper.CheckLonely(cbLonely.Checked);
        }

        private void cbSingleMachine_CheckedChanged(object sender, EventArgs e)
        {
            Wrapper.CheckOnlyOne(cbSingleMachine.Checked);
        }

        public void SetLTL(string ltl)
        {
            tbLTL.Text = ltl;
        }
    }
}
