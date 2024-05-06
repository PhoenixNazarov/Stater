using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CSMultyThread.GUI
{
    public partial class OptionsForm : Form
    {
        public OptionsForm()
        {
            InitializeComponent();
        }

        public OptionsWrapper Wrapper { get; set;}

        private void bOK_Click(object sender, EventArgs e)
        {
            if (Wrapper != null)
            {
                Wrapper.RawText = tbExecutions.Text;
                Wrapper.Namespace = tbNamespace.Text;
            }
        }

        internal void GetState()
        {
            tbExecutions.Text = Wrapper.RawText;
            tbNamespace.Text = Wrapper.Namespace;
        }

        private void bCancel_Click(object sender, EventArgs e)
        {

        }
    }
}
