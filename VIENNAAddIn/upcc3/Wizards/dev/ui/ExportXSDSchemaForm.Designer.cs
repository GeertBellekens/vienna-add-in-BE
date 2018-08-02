using BrightIdeasSoftware;

namespace VIENNAAddIn.upcc3.Wizards.dev.ui
{
    partial class ExportXSDSchemaForm
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
            this.messagesListView = new BrightIdeasSoftware.ObjectListView();
            this.nameColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.selectedPackageTextBox = new System.Windows.Forms.TextBox();
            this.selectPackageButton = new System.Windows.Forms.Button();
            this.selectedPackageLabel = new System.Windows.Forms.Label();
            this.generateButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.destinationFolderLabel = new System.Windows.Forms.Label();
            this.browseDestinationFolderButton = new System.Windows.Forms.Button();
            this.destinationFolderTextBox = new System.Windows.Forms.TextBox();
            this.StatusLabel = new System.Windows.Forms.Label();
            this.GenerateBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.closeButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.messagesListView)).BeginInit();
            this.SuspendLayout();
            // 
            // messagesListView
            // 
            this.messagesListView.AllColumns.Add(this.nameColumn);
            this.messagesListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.messagesListView.CellEditUseWholeCell = false;
            this.messagesListView.CheckBoxes = true;
            this.messagesListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.nameColumn});
            this.messagesListView.Cursor = System.Windows.Forms.Cursors.Default;
            this.messagesListView.FullRowSelect = true;
            this.messagesListView.Location = new System.Drawing.Point(12, 97);
            this.messagesListView.Name = "messagesListView";
            this.messagesListView.ShowGroups = false;
            this.messagesListView.Size = new System.Drawing.Size(409, 264);
            this.messagesListView.TabIndex = 0;
            this.messagesListView.UseCompatibleStateImageBehavior = false;
            this.messagesListView.UseFilterIndicator = true;
            this.messagesListView.UseFiltering = true;
            this.messagesListView.View = System.Windows.Forms.View.Details;
            this.messagesListView.SubItemChecking += new System.EventHandler<BrightIdeasSoftware.SubItemCheckingEventArgs>(this.messagesListView_SubItemChecking);
            this.messagesListView.FormatRow += new System.EventHandler<BrightIdeasSoftware.FormatRowEventArgs>(this.messagesListView_FormatRow);
            this.messagesListView.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.messagesListView_ItemChecked);
            // 
            // nameColumn
            // 
            this.nameColumn.AspectName = "Name";
            this.nameColumn.FillsFreeSpace = true;
            this.nameColumn.HeaderCheckBox = true;
            this.nameColumn.HeaderCheckState = System.Windows.Forms.CheckState.Checked;
            this.nameColumn.Text = "Message";
            this.nameColumn.Width = 219;
            // 
            // selectedPackageTextBox
            // 
            this.selectedPackageTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.selectedPackageTextBox.Location = new System.Drawing.Point(12, 27);
            this.selectedPackageTextBox.Name = "selectedPackageTextBox";
            this.selectedPackageTextBox.ReadOnly = true;
            this.selectedPackageTextBox.Size = new System.Drawing.Size(378, 20);
            this.selectedPackageTextBox.TabIndex = 1;
            // 
            // selectPackageButton
            // 
            this.selectPackageButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.selectPackageButton.Location = new System.Drawing.Point(396, 25);
            this.selectPackageButton.Name = "selectPackageButton";
            this.selectPackageButton.Size = new System.Drawing.Size(25, 23);
            this.selectPackageButton.TabIndex = 2;
            this.selectPackageButton.Text = "...";
            this.selectPackageButton.UseVisualStyleBackColor = true;
            this.selectPackageButton.Click += new System.EventHandler(this.selectPackageButton_Click);
            // 
            // selectedPackageLabel
            // 
            this.selectedPackageLabel.AutoSize = true;
            this.selectedPackageLabel.Location = new System.Drawing.Point(16, 9);
            this.selectedPackageLabel.Name = "selectedPackageLabel";
            this.selectedPackageLabel.Size = new System.Drawing.Size(95, 13);
            this.selectedPackageLabel.TabIndex = 3;
            this.selectedPackageLabel.Text = "Selected Package";
            // 
            // generateButton
            // 
            this.generateButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.generateButton.Location = new System.Drawing.Point(184, 367);
            this.generateButton.Name = "generateButton";
            this.generateButton.Size = new System.Drawing.Size(75, 23);
            this.generateButton.TabIndex = 4;
            this.generateButton.Text = "Generate";
            this.generateButton.UseVisualStyleBackColor = true;
            this.generateButton.Click += new System.EventHandler(this.generateButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.Location = new System.Drawing.Point(265, 367);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 4;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // destinationFolderLabel
            // 
            this.destinationFolderLabel.AutoSize = true;
            this.destinationFolderLabel.Location = new System.Drawing.Point(16, 52);
            this.destinationFolderLabel.Name = "destinationFolderLabel";
            this.destinationFolderLabel.Size = new System.Drawing.Size(92, 13);
            this.destinationFolderLabel.TabIndex = 7;
            this.destinationFolderLabel.Text = "Destination Folder";
            // 
            // browseDestinationFolderButton
            // 
            this.browseDestinationFolderButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.browseDestinationFolderButton.Location = new System.Drawing.Point(396, 68);
            this.browseDestinationFolderButton.Name = "browseDestinationFolderButton";
            this.browseDestinationFolderButton.Size = new System.Drawing.Size(25, 23);
            this.browseDestinationFolderButton.TabIndex = 6;
            this.browseDestinationFolderButton.Text = "...";
            this.browseDestinationFolderButton.UseVisualStyleBackColor = true;
            this.browseDestinationFolderButton.Click += new System.EventHandler(this.browseDestinationFolderButton_Click);
            // 
            // destinationFolderTextBox
            // 
            this.destinationFolderTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.destinationFolderTextBox.Location = new System.Drawing.Point(12, 70);
            this.destinationFolderTextBox.Name = "destinationFolderTextBox";
            this.destinationFolderTextBox.Size = new System.Drawing.Size(378, 20);
            this.destinationFolderTextBox.TabIndex = 5;
            this.destinationFolderTextBox.TextChanged += new System.EventHandler(this.destinationFolderTextBox_TextChanged);
            // 
            // StatusLabel
            // 
            this.StatusLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.StatusLabel.AutoSize = true;
            this.StatusLabel.Location = new System.Drawing.Point(22, 372);
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(0, 13);
            this.StatusLabel.TabIndex = 8;
            // 
            // GenerateBackgroundWorker
            // 
            this.GenerateBackgroundWorker.WorkerReportsProgress = true;
            this.GenerateBackgroundWorker.WorkerSupportsCancellation = true;
            this.GenerateBackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.GenerateBackgroundWorker_DoWork);
            this.GenerateBackgroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.GenerateBackgroundWorker_ProgressChanged);
            this.GenerateBackgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.GenerateBackgroundWorker_RunWorkerCompleted);
            // 
            // closeButton
            // 
            this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.closeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.closeButton.Location = new System.Drawing.Point(346, 367);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(75, 23);
            this.closeButton.TabIndex = 9;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = true;
            // 
            // ExportXSDSchemaForm
            // 
            this.AcceptButton = this.generateButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.closeButton;
            this.ClientSize = new System.Drawing.Size(433, 402);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.StatusLabel);
            this.Controls.Add(this.destinationFolderLabel);
            this.Controls.Add(this.browseDestinationFolderButton);
            this.Controls.Add(this.destinationFolderTextBox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.generateButton);
            this.Controls.Add(this.selectedPackageLabel);
            this.Controls.Add(this.selectPackageButton);
            this.Controls.Add(this.selectedPackageTextBox);
            this.Controls.Add(this.messagesListView);
            this.MinimumSize = new System.Drawing.Size(449, 441);
            this.Name = "ExportXSDSchemaForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Generate XSD\'s";
            ((System.ComponentModel.ISupportInitialize)(this.messagesListView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ObjectListView messagesListView;
        private System.Windows.Forms.TextBox selectedPackageTextBox;
        private System.Windows.Forms.Button selectPackageButton;
        private System.Windows.Forms.Label selectedPackageLabel;
        private OLVColumn nameColumn;
        private System.Windows.Forms.Button generateButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label destinationFolderLabel;
        private System.Windows.Forms.Button browseDestinationFolderButton;
        private System.Windows.Forms.TextBox destinationFolderTextBox;
        private System.Windows.Forms.Label StatusLabel;
        private System.ComponentModel.BackgroundWorker GenerateBackgroundWorker;
        private System.Windows.Forms.Button closeButton;
    }
}