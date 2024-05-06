using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace StaterV.Attributes
{
    public partial class EditEventForm : Form
    {
        public EditEventWrapper Wrapper { get; set; }
        public EditEventForm()
        {
            InitializeComponent();
        }

        public Event TheEvent { get; set; }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            Wrapper.SetEvent(textBoxName.Text, textBoxComment.Text);
            //Event.Name = textBoxName.Text;
            //Event.Comment = textBoxComment.Text;
        }
    }
}
