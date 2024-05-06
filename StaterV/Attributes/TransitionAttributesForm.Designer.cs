namespace StaterV.Attributes
{
    partial class TransitionAttributesForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TransitionAttributesForm));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.tcTransitionAttributes = new System.Windows.Forms.TabControl();
            this.tpEvent = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.dataGridEvents = new System.Windows.Forms.DataGridView();
            this.dgEventsName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgEventsComment = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonNewEvent = new System.Windows.Forms.Button();
            this.buttonEditEvent = new System.Windows.Forms.Button();
            this.buttonDeleteEvent = new System.Windows.Forms.Button();
            this.labelChooseEvent = new System.Windows.Forms.Label();
            this.comboBoxEvents = new System.Windows.Forms.ComboBox();
            this.tpActions = new System.Windows.Forms.TabPage();
            this.tableLayoutActions = new System.Windows.Forms.TableLayoutPanel();
            this.dataGridViewTransActions = new System.Windows.Forms.DataGridView();
            this.TransAction = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TransDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tpEffects = new System.Windows.Forms.TabPage();
            this.tpConditions = new System.Windows.Forms.TabPage();
            this.tbConditions = new System.Windows.Forms.TextBox();
            this.tpCode = new System.Windows.Forms.TabPage();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.tbCode = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tcTransitionAttributes.SuspendLayout();
            this.tpEvent.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridEvents)).BeginInit();
            this.tableLayoutPanel4.SuspendLayout();
            this.tpActions.SuspendLayout();
            this.tableLayoutActions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTransActions)).BeginInit();
            this.tpConditions.SuspendLayout();
            this.tpCode.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tcTransitionAttributes, 0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // tableLayoutPanel2
            // 
            resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
            this.tableLayoutPanel2.Controls.Add(this.buttonOK, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.buttonCancel, 1, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            // 
            // buttonOK
            // 
            resources.ApplyResources(this.buttonOK, "buttonOK");
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            resources.ApplyResources(this.buttonCancel, "buttonCancel");
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // tcTransitionAttributes
            // 
            resources.ApplyResources(this.tcTransitionAttributes, "tcTransitionAttributes");
            this.tcTransitionAttributes.Controls.Add(this.tpEvent);
            this.tcTransitionAttributes.Controls.Add(this.tpActions);
            this.tcTransitionAttributes.Controls.Add(this.tpEffects);
            this.tcTransitionAttributes.Controls.Add(this.tpConditions);
            this.tcTransitionAttributes.Controls.Add(this.tpCode);
            this.tcTransitionAttributes.Name = "tcTransitionAttributes";
            this.tcTransitionAttributes.SelectedIndex = 0;
            // 
            // tpEvent
            // 
            resources.ApplyResources(this.tpEvent, "tpEvent");
            this.tpEvent.Controls.Add(this.tableLayoutPanel3);
            this.tpEvent.Name = "tpEvent";
            this.tpEvent.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel3
            // 
            resources.ApplyResources(this.tableLayoutPanel3, "tableLayoutPanel3");
            this.tableLayoutPanel3.Controls.Add(this.dataGridEvents, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel4, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.labelChooseEvent, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.comboBoxEvents, 1, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            // 
            // dataGridEvents
            // 
            resources.ApplyResources(this.dataGridEvents, "dataGridEvents");
            this.dataGridEvents.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridEvents.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgEventsName,
            this.dgEventsComment});
            this.dataGridEvents.Name = "dataGridEvents";
            // 
            // dgEventsName
            // 
            this.dgEventsName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            resources.ApplyResources(this.dgEventsName, "dgEventsName");
            this.dgEventsName.Name = "dgEventsName";
            // 
            // dgEventsComment
            // 
            this.dgEventsComment.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            resources.ApplyResources(this.dgEventsComment, "dgEventsComment");
            this.dgEventsComment.Name = "dgEventsComment";
            // 
            // tableLayoutPanel4
            // 
            resources.ApplyResources(this.tableLayoutPanel4, "tableLayoutPanel4");
            this.tableLayoutPanel4.Controls.Add(this.buttonNewEvent, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.buttonEditEvent, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.buttonDeleteEvent, 0, 2);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            // 
            // buttonNewEvent
            // 
            resources.ApplyResources(this.buttonNewEvent, "buttonNewEvent");
            this.buttonNewEvent.Name = "buttonNewEvent";
            this.buttonNewEvent.UseVisualStyleBackColor = true;
            this.buttonNewEvent.Click += new System.EventHandler(this.buttonNewEvent_Click);
            // 
            // buttonEditEvent
            // 
            resources.ApplyResources(this.buttonEditEvent, "buttonEditEvent");
            this.buttonEditEvent.Name = "buttonEditEvent";
            this.buttonEditEvent.UseVisualStyleBackColor = true;
            this.buttonEditEvent.Click += new System.EventHandler(this.buttonEditEvent_Click);
            // 
            // buttonDeleteEvent
            // 
            resources.ApplyResources(this.buttonDeleteEvent, "buttonDeleteEvent");
            this.buttonDeleteEvent.Name = "buttonDeleteEvent";
            this.buttonDeleteEvent.UseVisualStyleBackColor = true;
            // 
            // labelChooseEvent
            // 
            resources.ApplyResources(this.labelChooseEvent, "labelChooseEvent");
            this.labelChooseEvent.Name = "labelChooseEvent";
            // 
            // comboBoxEvents
            // 
            resources.ApplyResources(this.comboBoxEvents, "comboBoxEvents");
            this.comboBoxEvents.FormattingEnabled = true;
            this.comboBoxEvents.Name = "comboBoxEvents";
            // 
            // tpActions
            // 
            resources.ApplyResources(this.tpActions, "tpActions");
            this.tpActions.Controls.Add(this.tableLayoutActions);
            this.tpActions.Name = "tpActions";
            this.tpActions.UseVisualStyleBackColor = true;
            // 
            // tableLayoutActions
            // 
            resources.ApplyResources(this.tableLayoutActions, "tableLayoutActions");
            this.tableLayoutActions.Controls.Add(this.dataGridViewTransActions, 0, 1);
            this.tableLayoutActions.Name = "tableLayoutActions";
            // 
            // dataGridViewTransActions
            // 
            resources.ApplyResources(this.dataGridViewTransActions, "dataGridViewTransActions");
            this.dataGridViewTransActions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewTransActions.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.TransAction,
            this.TransDescription});
            this.dataGridViewTransActions.Name = "dataGridViewTransActions";
            // 
            // TransAction
            // 
            this.TransAction.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            resources.ApplyResources(this.TransAction, "TransAction");
            this.TransAction.Name = "TransAction";
            // 
            // TransDescription
            // 
            this.TransDescription.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            resources.ApplyResources(this.TransDescription, "TransDescription");
            this.TransDescription.Name = "TransDescription";
            // 
            // tpEffects
            // 
            resources.ApplyResources(this.tpEffects, "tpEffects");
            this.tpEffects.Name = "tpEffects";
            this.tpEffects.UseVisualStyleBackColor = true;
            // 
            // tpConditions
            // 
            resources.ApplyResources(this.tpConditions, "tpConditions");
            this.tpConditions.Controls.Add(this.tbConditions);
            this.tpConditions.Name = "tpConditions";
            this.tpConditions.UseVisualStyleBackColor = true;
            // 
            // tbConditions
            // 
            resources.ApplyResources(this.tbConditions, "tbConditions");
            this.tbConditions.Name = "tbConditions";
            // 
            // tpCode
            // 
            resources.ApplyResources(this.tpCode, "tpCode");
            this.tpCode.Controls.Add(this.richTextBox1);
            this.tpCode.Controls.Add(this.tbCode);
            this.tpCode.Name = "tpCode";
            this.tpCode.UseVisualStyleBackColor = true;
            // 
            // richTextBox1
            // 
            resources.ApplyResources(this.richTextBox1, "richTextBox1");
            this.richTextBox1.DetectUrls = false;
            this.richTextBox1.Name = "richTextBox1";
            // 
            // tbCode
            // 
            resources.ApplyResources(this.tbCode, "tbCode");
            this.tbCode.Name = "tbCode";
            // 
            // TransitionAttributesForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "TransitionAttributesForm";
            this.Activated += new System.EventHandler(this.TransitionAttributesForm_Activated);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tcTransitionAttributes.ResumeLayout(false);
            this.tpEvent.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridEvents)).EndInit();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tpActions.ResumeLayout(false);
            this.tableLayoutActions.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTransActions)).EndInit();
            this.tpConditions.ResumeLayout(false);
            this.tpConditions.PerformLayout();
            this.tpCode.ResumeLayout(false);
            this.tpCode.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.TabControl tcTransitionAttributes;
        private System.Windows.Forms.TabPage tpEvent;
        private System.Windows.Forms.TabPage tpActions;
        private System.Windows.Forms.TabPage tpEffects;
        private System.Windows.Forms.TabPage tpConditions;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.ComboBox comboBoxEvents;
        private System.Windows.Forms.Label labelChooseEvent;
        private System.Windows.Forms.DataGridView dataGridEvents;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Button buttonNewEvent;
        private System.Windows.Forms.Button buttonEditEvent;
        private System.Windows.Forms.Button buttonDeleteEvent;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgEventsName;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgEventsComment;
        private System.Windows.Forms.TableLayoutPanel tableLayoutActions;
        private System.Windows.Forms.DataGridView dataGridViewTransActions;
        private System.Windows.Forms.DataGridViewTextBoxColumn TransAction;
        private System.Windows.Forms.DataGridViewTextBoxColumn TransDescription;
        private System.Windows.Forms.TabPage tpCode;
        private System.Windows.Forms.TextBox tbCode;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.TextBox tbConditions;

    }
}