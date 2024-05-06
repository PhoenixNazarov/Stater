using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace StaterV.Project
{
    public partial class NewProjectForm : Form
    {
        public NewProjectForm()
        {
            InitializeComponent();
        }

        public string ProjectName
        {
            get { return projectName; }
            private set { projectName = value; }
        }

        public string ProjectLocation
        {
            get { return projectLocation; }
            set { projectLocation = value; }
        }

        private void NewProjectForm_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Файл проекта - @projectName.stp
        /// </summary>
        private string projectName = ""; 
        private string projectLocation;

        private void OKButton_Click(object sender, EventArgs e)
        {
            ProjectName = projectNameBox.Text;
            ProjectLocation = locationTextBox.Text;
            Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void browseButton_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowNewFolderButton = true;
            var res = folderBrowserDialog1.ShowDialog();
            if (res == DialogResult.OK)
            {
                locationTextBox.Text = folderBrowserDialog1.SelectedPath;
            }
        }
    }
}
