namespace SpinVeriff.GUI
{
    partial class OptionsForm
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.bOK = new System.Windows.Forms.Button();
            this.bCancel = new System.Windows.Forms.Button();
            this.tbLTL = new System.Windows.Forms.TextBox();
            this.comboSigleMachine = new System.Windows.Forms.ComboBox();
            this.lTitle = new System.Windows.Forms.Label();
            this.lFormula = new System.Windows.Forms.Label();
            this.cbVerifySystem = new System.Windows.Forms.CheckBox();
            this.cbLonely = new System.Windows.Forms.CheckBox();
            this.cbSingleMachine = new System.Windows.Forms.CheckBox();
            this.cbEnteredObjects = new System.Windows.Forms.CheckBox();
            this.bEnterMachines = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30.625F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 69.375F));
            this.tableLayoutPanel1.Controls.Add(this.bOK, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.bCancel, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.tbLTL, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.comboSigleMachine, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.lTitle, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lFormula, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.cbVerifySystem, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.cbLonely, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.cbSingleMachine, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.cbEnteredObjects, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.bEnterMachines, 1, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 7;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.34568F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.92593F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.34568F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.34568F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.34568F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.34568F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.34568F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(480, 265);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // bOK
            // 
            this.bOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.bOK.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bOK.Location = new System.Drawing.Point(3, 231);
            this.bOK.Name = "bOK";
            this.bOK.Size = new System.Drawing.Size(141, 31);
            this.bOK.TabIndex = 4;
            this.bOK.Text = "OK";
            this.bOK.UseVisualStyleBackColor = true;
            this.bOK.Click += new System.EventHandler(this.bOK_Click);
            // 
            // bCancel
            // 
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bCancel.Location = new System.Drawing.Point(150, 231);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(327, 31);
            this.bCancel.TabIndex = 5;
            this.bCancel.Text = "Cancel";
            this.bCancel.UseVisualStyleBackColor = true;
            this.bCancel.Click += new System.EventHandler(this.bCancel_Click);
            // 
            // tbLTL
            // 
            this.tbLTL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbLTL.Location = new System.Drawing.Point(150, 35);
            this.tbLTL.Multiline = true;
            this.tbLTL.Name = "tbLTL";
            this.tbLTL.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbLTL.Size = new System.Drawing.Size(327, 62);
            this.tbLTL.TabIndex = 6;
            // 
            // comboSigleMachine
            // 
            this.comboSigleMachine.FormattingEnabled = true;
            this.comboSigleMachine.Location = new System.Drawing.Point(150, 199);
            this.comboSigleMachine.Name = "comboSigleMachine";
            this.comboSigleMachine.Size = new System.Drawing.Size(121, 21);
            this.comboSigleMachine.TabIndex = 7;
            // 
            // lTitle
            // 
            this.lTitle.AutoSize = true;
            this.lTitle.Location = new System.Drawing.Point(3, 0);
            this.lTitle.Name = "lTitle";
            this.lTitle.Size = new System.Drawing.Size(49, 13);
            this.lTitle.TabIndex = 8;
            this.lTitle.Text = "              ";
            // 
            // lFormula
            // 
            this.lFormula.AutoSize = true;
            this.lFormula.Location = new System.Drawing.Point(3, 32);
            this.lFormula.Name = "lFormula";
            this.lFormula.Size = new System.Drawing.Size(69, 13);
            this.lFormula.TabIndex = 9;
            this.lFormula.Text = "LTL formulae";
            // 
            // cbVerifySystem
            // 
            this.cbVerifySystem.AutoSize = true;
            this.cbVerifySystem.Enabled = false;
            this.cbVerifySystem.Location = new System.Drawing.Point(3, 103);
            this.cbVerifySystem.Name = "cbVerifySystem";
            this.cbVerifySystem.Size = new System.Drawing.Size(89, 17);
            this.cbVerifySystem.TabIndex = 0;
            this.cbVerifySystem.Text = "Verify System";
            this.cbVerifySystem.UseVisualStyleBackColor = true;
            this.cbVerifySystem.CheckedChanged += new System.EventHandler(this.cbVerifySystem_CheckedChanged);
            // 
            // cbLonely
            // 
            this.cbLonely.AutoSize = true;
            this.cbLonely.Enabled = false;
            this.cbLonely.Location = new System.Drawing.Point(3, 167);
            this.cbLonely.Name = "cbLonely";
            this.cbLonely.Size = new System.Drawing.Size(130, 17);
            this.cbLonely.TabIndex = 2;
            this.cbLonely.Text = "Verify lonely machines";
            this.cbLonely.UseVisualStyleBackColor = true;
            this.cbLonely.CheckedChanged += new System.EventHandler(this.cbLonely_CheckedChanged);
            // 
            // cbSingleMachine
            // 
            this.cbSingleMachine.AutoSize = true;
            this.cbSingleMachine.Enabled = false;
            this.cbSingleMachine.Location = new System.Drawing.Point(3, 199);
            this.cbSingleMachine.Name = "cbSingleMachine";
            this.cbSingleMachine.Size = new System.Drawing.Size(125, 17);
            this.cbSingleMachine.TabIndex = 3;
            this.cbSingleMachine.Text = "Verify single machine";
            this.cbSingleMachine.UseVisualStyleBackColor = true;
            this.cbSingleMachine.CheckedChanged += new System.EventHandler(this.cbSingleMachine_CheckedChanged);
            // 
            // cbEnteredObjects
            // 
            this.cbEnteredObjects.AutoSize = true;
            this.cbEnteredObjects.Checked = true;
            this.cbEnteredObjects.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbEnteredObjects.Location = new System.Drawing.Point(3, 135);
            this.cbEnteredObjects.Name = "cbEnteredObjects";
            this.cbEnteredObjects.Size = new System.Drawing.Size(139, 17);
            this.cbEnteredObjects.TabIndex = 1;
            this.cbEnteredObjects.Text = "Verify entered machines";
            this.cbEnteredObjects.UseVisualStyleBackColor = true;
            this.cbEnteredObjects.CheckedChanged += new System.EventHandler(this.cbEnteredObjects_CheckedChanged);
            // 
            // bEnterMachines
            // 
            this.bEnterMachines.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bEnterMachines.Location = new System.Drawing.Point(150, 135);
            this.bEnterMachines.Name = "bEnterMachines";
            this.bEnterMachines.Size = new System.Drawing.Size(327, 26);
            this.bEnterMachines.TabIndex = 10;
            this.bEnterMachines.Text = "Enter machines";
            this.bEnterMachines.UseVisualStyleBackColor = true;
            this.bEnterMachines.Click += new System.EventHandler(this.bEnterMachines_Click);
            // 
            // OptionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(480, 265);
            this.ControlBox = false;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "OptionsForm";
            this.ShowInTaskbar = false;
            this.Text = "Verification options";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.CheckBox cbVerifySystem;
        private System.Windows.Forms.CheckBox cbEnteredObjects;
        private System.Windows.Forms.CheckBox cbLonely;
        private System.Windows.Forms.CheckBox cbSingleMachine;
        private System.Windows.Forms.Button bOK;
        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.TextBox tbLTL;
        private System.Windows.Forms.ComboBox comboSigleMachine;
        private System.Windows.Forms.Label lTitle;
        private System.Windows.Forms.Label lFormula;
        private System.Windows.Forms.Button bEnterMachines;
    }
}