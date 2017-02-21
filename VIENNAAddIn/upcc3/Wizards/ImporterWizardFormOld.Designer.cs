namespace VIENNAAddIn.upcc3.Wizards
{
    partial class ImporterWizardFormOld
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
            this.label3 = new System.Windows.Forms.Label();
            this.textboxRootSchema = new System.Windows.Forms.TextBox();
            this.buttonBrowseFolders = new System.Windows.Forms.Button();
            this.comboModels = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.richtextStatus = new System.Windows.Forms.RichTextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.buttonImport = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(126, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Select root XML schema:";
            // 
            // textboxRootSchema
            // 
            this.textboxRootSchema.Location = new System.Drawing.Point(207, 17);
            this.textboxRootSchema.Name = "textboxRootSchema";
            this.textboxRootSchema.Size = new System.Drawing.Size(207, 20);
            this.textboxRootSchema.TabIndex = 3;
            // 
            // buttonBrowseFolders
            // 
            this.buttonBrowseFolders.Location = new System.Drawing.Point(420, 17);
            this.buttonBrowseFolders.Name = "buttonBrowseFolders";
            this.buttonBrowseFolders.Size = new System.Drawing.Size(24, 20);
            this.buttonBrowseFolders.TabIndex = 0;
            this.buttonBrowseFolders.Text = "...";
            this.buttonBrowseFolders.UseVisualStyleBackColor = true;
            this.buttonBrowseFolders.Click += new System.EventHandler(this.buttonBrowseFolders_Click);
            // 
            // comboModels
            // 
            this.comboModels.FormattingEnabled = true;
            this.comboModels.Location = new System.Drawing.Point(207, 43);
            this.comboModels.Name = "comboModels";
            this.comboModels.Size = new System.Drawing.Size(237, 21);
            this.comboModels.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 46);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(130, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Choose Document Model:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.progressBar);
            this.groupBox1.Controls.Add(this.richtextStatus);
            this.groupBox1.Location = new System.Drawing.Point(12, 96);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(458, 153);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Status";
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(15, 126);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(429, 13);
            this.progressBar.TabIndex = 1;
            // 
            // richtextStatus
            // 
            this.richtextStatus.Location = new System.Drawing.Point(14, 19);
            this.richtextStatus.Name = "richtextStatus";
            this.richtextStatus.Size = new System.Drawing.Size(430, 99);
            this.richtextStatus.TabIndex = 0;
            this.richtextStatus.Text = "";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.comboModels);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.buttonBrowseFolders);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.textboxRootSchema);
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(458, 78);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Import Settings";
            // 
            // buttonImport
            // 
            this.buttonImport.Location = new System.Drawing.Point(69, 265);
            this.buttonImport.Name = "buttonImport";
            this.buttonImport.Size = new System.Drawing.Size(158, 23);
            this.buttonImport.TabIndex = 3;
            this.buttonImport.Text = "&Import XML Schemas ...";
            this.buttonImport.UseVisualStyleBackColor = true;
            this.buttonImport.Click += new System.EventHandler(this.buttonImport_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(314, 265);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 23);
            this.buttonClose.TabIndex = 0;
            this.buttonClose.Text = "&Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // ImporterWizardForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(481, 298);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.buttonImport);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ImporterWizardForm";
            this.Text = "Import CCTS model from XML schemas";
            this.Load += new System.EventHandler(this.ImporterWizardForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textboxRootSchema;
        private System.Windows.Forms.Button buttonBrowseFolders;
        private System.Windows.Forms.ComboBox comboModels;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.RichTextBox richtextStatus;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button buttonImport;
        private System.Windows.Forms.Button buttonClose;
    }
}