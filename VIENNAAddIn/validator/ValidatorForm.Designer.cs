namespace VIENNAAddIn.validator
{
    partial class ValidatorForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components;

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
            this.components = new System.ComponentModel.Container();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.startButton = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.validationMessagesListView = new System.Windows.Forms.ListView();
            this.errorLevelColumn = new System.Windows.Forms.ColumnHeader();
            this.sourceColumn = new System.Windows.Forms.ColumnHeader();
            this.messageColumn = new System.Windows.Forms.ColumnHeader();
            this.hiddenColumn = new System.Windows.Forms.ColumnHeader();
            this.levelSelector = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.errorLink = new System.Windows.Forms.LinkLabel();
            this.detailsBox = new System.Windows.Forms.RichTextBox();
            this.statusBar = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.progressTimer = new System.Windows.Forms.Timer(this.components);
            this.bworker = new System.ComponentModel.BackgroundWorker();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.statusBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.startButton);
            this.groupBox1.Controls.Add(this.progressBar);
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(691, 55);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Progress";
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(604, 19);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 23);
            this.startButton.TabIndex = 1;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(12, 19);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(586, 23);
            this.progressBar.Step = 5;
            this.progressBar.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.validationMessagesListView);
            this.groupBox2.Controls.Add(this.levelSelector);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(13, 75);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(691, 215);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Messages";
            // 
            // validationMessagesListView
            // 
            this.validationMessagesListView.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.validationMessagesListView.CausesValidation = false;
            this.validationMessagesListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.errorLevelColumn,
            this.sourceColumn,
            this.messageColumn,
            this.hiddenColumn});
            this.validationMessagesListView.FullRowSelect = true;
            this.validationMessagesListView.Location = new System.Drawing.Point(12, 44);
            this.validationMessagesListView.MultiSelect = false;
            this.validationMessagesListView.Name = "validationMessagesListView";
            this.validationMessagesListView.Size = new System.Drawing.Size(667, 165);
            this.validationMessagesListView.TabIndex = 15;
            this.validationMessagesListView.UseCompatibleStateImageBehavior = false;
            this.validationMessagesListView.View = System.Windows.Forms.View.Details;
            this.validationMessagesListView.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.validationMessagesListView_ItemSelectionChanged_1);
            // 
            // errorLevelColumn
            // 
            this.errorLevelColumn.Text = "Level";
            this.errorLevelColumn.Width = 49;
            // 
            // sourceColumn
            // 
            this.sourceColumn.Text = "View";
            this.sourceColumn.Width = 53;
            // 
            // messageColumn
            // 
            this.messageColumn.Text = "Message";
            this.messageColumn.Width = 580;
            // 
            // hiddenColumn
            // 
            this.hiddenColumn.Width = 0;
            // 
            // levelSelector
            // 
            this.levelSelector.FormattingEnabled = true;
            this.levelSelector.Items.AddRange(new object[] {
            "ALL",
            "INFO",
            "WARN",
            "ERROR"});
            this.levelSelector.Location = new System.Drawing.Point(55, 17);
            this.levelSelector.Name = "levelSelector";
            this.levelSelector.Size = new System.Drawing.Size(121, 21);
            this.levelSelector.TabIndex = 1;
            this.levelSelector.SelectedIndexChanged += new System.EventHandler(this.levelSelector_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Show:";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.errorLink);
            this.groupBox3.Controls.Add(this.detailsBox);
            this.groupBox3.Location = new System.Drawing.Point(13, 297);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(691, 144);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Details";
            // 
            // errorLink
            // 
            this.errorLink.AutoSize = true;
            this.errorLink.Location = new System.Drawing.Point(15, 123);
            this.errorLink.Name = "errorLink";
            this.errorLink.Size = new System.Drawing.Size(100, 13);
            this.errorLink.TabIndex = 1;
            this.errorLink.TabStop = true;
            this.errorLink.Text = "Show error in model";
            this.errorLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.errorLink_LinkClicked);
            this.errorLink.MouseClick += new System.Windows.Forms.MouseEventHandler(this.errorLink_MouseClick);
            // 
            // detailsBox
            // 
            this.detailsBox.Location = new System.Drawing.Point(12, 20);
            this.detailsBox.Name = "detailsBox";
            this.detailsBox.Size = new System.Drawing.Size(667, 96);
            this.detailsBox.TabIndex = 0;
            this.detailsBox.Text = "";
            // 
            // statusBar
            // 
            this.statusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel});
            this.statusBar.Location = new System.Drawing.Point(0, 449);
            this.statusBar.Name = "statusBar";
            this.statusBar.Size = new System.Drawing.Size(717, 22);
            this.statusBar.TabIndex = 3;
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // progressTimer
            // 
            this.progressTimer.Interval = 400;
            // 
            // bworker
            // 
            this.bworker.WorkerReportsProgress = true;
            this.bworker.WorkerSupportsCancellation = true;
            this.bworker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bworker_DoWork);
            this.bworker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bworker_RunWorkerCompleted);
            this.bworker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bworker_ProgressChanged);
            // 
            // ValidatorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(717, 471);
            this.Controls.Add(this.statusBar);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "ValidatorForm";
            this.Text = "Validator";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.statusBar.ResumeLayout(false);
            this.statusBar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox levelSelector;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.LinkLabel errorLink;
        private System.Windows.Forms.RichTextBox detailsBox;
        private System.Windows.Forms.ListView validationMessagesListView;
        private System.Windows.Forms.ColumnHeader errorLevelColumn;
        private System.Windows.Forms.ColumnHeader sourceColumn;
        private System.Windows.Forms.ColumnHeader messageColumn;
        private System.Windows.Forms.ColumnHeader hiddenColumn;
        private System.Windows.Forms.StatusStrip statusBar;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.Timer progressTimer;
        private System.ComponentModel.BackgroundWorker bworker;


    }
}