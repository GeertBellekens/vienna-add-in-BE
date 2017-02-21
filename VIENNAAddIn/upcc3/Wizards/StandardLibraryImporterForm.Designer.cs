namespace VIENNAAddIn.upcc3.Wizards
{
    partial class StandardLibraryImporterForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cbxMajor = new System.Windows.Forms.ComboBox();
            this.cbxMinor = new System.Windows.Forms.ComboBox();
            this.txtComment = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnImport = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.rtxtStatus = new System.Windows.Forms.RichTextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Major";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(209, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Minor";
            // 
            // cbxMajor
            // 
            this.cbxMajor.FormattingEnabled = true;
            this.cbxMajor.Location = new System.Drawing.Point(46, 21);
            this.cbxMajor.Name = "cbxMajor";
            this.cbxMajor.Size = new System.Drawing.Size(157, 21);
            this.cbxMajor.Sorted = true;
            this.cbxMajor.TabIndex = 3;
            this.cbxMajor.SelectionChangeCommitted += new System.EventHandler(this.cbxMajor_SelectionChangeCommitted);
            // 
            // cbxMinor
            // 
            this.cbxMinor.FormattingEnabled = true;
            this.cbxMinor.Location = new System.Drawing.Point(249, 21);
            this.cbxMinor.Name = "cbxMinor";
            this.cbxMinor.Size = new System.Drawing.Size(55, 21);
            this.cbxMinor.Sorted = true;
            this.cbxMinor.TabIndex = 4;
            this.cbxMinor.SelectionChangeCommitted += new System.EventHandler(this.cbxMinor_SelectionChangeCommitted);
            // 
            // txtComment
            // 
            this.txtComment.BackColor = System.Drawing.SystemColors.Info;
            this.txtComment.Location = new System.Drawing.Point(10, 18);
            this.txtComment.Multiline = true;
            this.txtComment.Name = "txtComment";
            this.txtComment.ReadOnly = true;
            this.txtComment.Size = new System.Drawing.Size(293, 58);
            this.txtComment.TabIndex = 5;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtComment);
            this.groupBox1.Location = new System.Drawing.Point(12, 71);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(314, 88);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Comment";
            // 
            // btnImport
            // 
            this.btnImport.Location = new System.Drawing.Point(55, 312);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(75, 23);
            this.btnImport.TabIndex = 7;
            this.btnImport.Text = "&Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(210, 312);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 8;
            this.btnClose.Text = "&Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.cbxMajor);
            this.groupBox2.Controls.Add(this.cbxMinor);
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(313, 53);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Core Component Library Version";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.rtxtStatus);
            this.groupBox3.Location = new System.Drawing.Point(12, 163);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(314, 133);
            this.groupBox3.TabIndex = 10;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Status";
            // 
            // rtxtStatus
            // 
            this.rtxtStatus.BackColor = System.Drawing.SystemColors.Control;
            this.rtxtStatus.Location = new System.Drawing.Point(10, 18);
            this.rtxtStatus.Name = "rtxtStatus";
            this.rtxtStatus.Size = new System.Drawing.Size(293, 103);
            this.rtxtStatus.TabIndex = 0;
            this.rtxtStatus.Text = "";
            this.rtxtStatus.WordWrap = false;
            // 
            // StandardLibraryImporterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(338, 349);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnImport);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "StandardLibraryImporterForm";
            this.Text = "Standard Core Component Library Importer";
            this.Load += new System.EventHandler(this.StandardLibraryImporterForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbxMajor;
        private System.Windows.Forms.ComboBox cbxMinor;
        private System.Windows.Forms.TextBox txtComment;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RichTextBox rtxtStatus;
    }
}