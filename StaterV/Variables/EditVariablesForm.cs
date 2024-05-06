using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace StaterV.Variables
{
    public partial class EditVariablesForm : Form
    {
        public EditVariablesForm()
        {
            InitializeComponent();
        }

        public EditVariablesWrapper Wrapper { get; set; }

        private string variables;

        public void SetVariables(string vS)
        {
            variables = vS;
            tbVariables.Text = variables;
        }

        public DialogResult ReportError(string text)
        {
            return MessageBox.Show(text, "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
        }

        private void bOK_Click(object sender, EventArgs e)
        {
            variables = tbVariables.Text;
            Wrapper.Variables = variables;
        }
    }
}
