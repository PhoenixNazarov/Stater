using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace StaterV.MachineProperties
{
    public partial class MachinePropertiesForm : Form
    {
        public MachinePropertiesForm()
        {
            InitializeComponent();
        }

        public MPWrapperWinForm Wrapper { get; set; }

        public void SetHeader(string header)
        {
            Text = header;
        }
    }
}
