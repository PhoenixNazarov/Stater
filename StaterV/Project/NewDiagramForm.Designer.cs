namespace StaterV.Project
{
    partial class NewDiagramForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewDiagramForm));
            this.rbStateMachine = new System.Windows.Forms.RadioButton();
            this.rbLinkDiagram = new System.Windows.Forms.RadioButton();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.enterDgrNameLabel = new System.Windows.Forms.Label();
            this.diagramNameTextBox = new System.Windows.Forms.TextBox();
            this.chooseDgrTypeLabel = new System.Windows.Forms.Label();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.SuspendLayout();
            // 
            // rbStateMachine
            // 
            this.rbStateMachine.AccessibleDescription = null;
            this.rbStateMachine.AccessibleName = null;
            resources.ApplyResources(this.rbStateMachine, "rbStateMachine");
            this.rbStateMachine.BackgroundImage = null;
            this.rbStateMachine.Font = null;
            this.rbStateMachine.Name = "rbStateMachine";
            this.rbStateMachine.TabStop = true;
            this.rbStateMachine.UseVisualStyleBackColor = true;
            // 
            // rbLinkDiagram
            // 
            this.rbLinkDiagram.AccessibleDescription = null;
            this.rbLinkDiagram.AccessibleName = null;
            resources.ApplyResources(this.rbLinkDiagram, "rbLinkDiagram");
            this.rbLinkDiagram.BackgroundImage = null;
            this.rbLinkDiagram.Font = null;
            this.rbLinkDiagram.Name = "rbLinkDiagram";
            this.rbLinkDiagram.TabStop = true;
            this.rbLinkDiagram.UseVisualStyleBackColor = true;
            // 
            // okButton
            // 
            this.okButton.AccessibleDescription = null;
            this.okButton.AccessibleName = null;
            resources.ApplyResources(this.okButton, "okButton");
            this.okButton.BackgroundImage = null;
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Font = null;
            this.okButton.Name = "okButton";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.AccessibleDescription = null;
            this.cancelButton.AccessibleName = null;
            resources.ApplyResources(this.cancelButton, "cancelButton");
            this.cancelButton.BackgroundImage = null;
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Font = null;
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // enterDgrNameLabel
            // 
            this.enterDgrNameLabel.AccessibleDescription = null;
            this.enterDgrNameLabel.AccessibleName = null;
            resources.ApplyResources(this.enterDgrNameLabel, "enterDgrNameLabel");
            this.enterDgrNameLabel.Font = null;
            this.enterDgrNameLabel.Name = "enterDgrNameLabel";
            // 
            // diagramNameTextBox
            // 
            this.diagramNameTextBox.AccessibleDescription = null;
            this.diagramNameTextBox.AccessibleName = null;
            resources.ApplyResources(this.diagramNameTextBox, "diagramNameTextBox");
            this.diagramNameTextBox.BackgroundImage = null;
            this.diagramNameTextBox.Font = null;
            this.diagramNameTextBox.Name = "diagramNameTextBox";
            // 
            // chooseDgrTypeLabel
            // 
            this.chooseDgrTypeLabel.AccessibleDescription = null;
            this.chooseDgrTypeLabel.AccessibleName = null;
            resources.ApplyResources(this.chooseDgrTypeLabel, "chooseDgrTypeLabel");
            this.chooseDgrTypeLabel.Font = null;
            this.chooseDgrTypeLabel.Name = "chooseDgrTypeLabel";
            // 
            // folderBrowserDialog1
            // 
            resources.ApplyResources(this.folderBrowserDialog1, "folderBrowserDialog1");
            // 
            // NewDiagramForm
            // 
            this.AcceptButton = this.okButton;
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = null;
            this.CancelButton = this.cancelButton;
            this.Controls.Add(this.chooseDgrTypeLabel);
            this.Controls.Add(this.diagramNameTextBox);
            this.Controls.Add(this.enterDgrNameLabel);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.rbLinkDiagram);
            this.Controls.Add(this.rbStateMachine);
            this.Font = null;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = null;
            this.Name = "NewDiagramForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton rbStateMachine;
        private System.Windows.Forms.RadioButton rbLinkDiagram;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label enterDgrNameLabel;
        private System.Windows.Forms.TextBox diagramNameTextBox;
        private System.Windows.Forms.Label chooseDgrTypeLabel;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
    }
}