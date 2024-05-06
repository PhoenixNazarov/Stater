using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SpinVeriff.GUI
{
    public partial class EnterMachinesForm : Form
    {
        public EnterMachinesForm()
        {
            InitializeComponent();
        }

        public EnterMachinesFormWrapper Wrapper { get; set; }

        private void bOK_Click(object sender, EventArgs e)
        {
            Wrapper.EnteredText = richTextBoxMachines.Text;
        }

        public void SyncText()
        {
            richTextBoxMachines.Text = Wrapper.EnteredText;
        }
    }
}
