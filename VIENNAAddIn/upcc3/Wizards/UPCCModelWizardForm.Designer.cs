namespace VIENNAAddIn.upcc3.Wizards
{
    partial class UpccModelWizardForm
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
            this.buttonGenerate = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.checkboxDOCL = new System.Windows.Forms.CheckBox();
            this.checkboxPRIML = new System.Windows.Forms.CheckBox();
            this.checkboxENUML = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.checkboxBIEL = new System.Windows.Forms.CheckBox();
            this.checkboxCDTL = new System.Windows.Forms.CheckBox();
            this.textboxBDTLName = new System.Windows.Forms.TextBox();
            this.checkboxCCL = new System.Windows.Forms.CheckBox();
            this.textboxCCLName = new System.Windows.Forms.TextBox();
            this.textboxPRIMLName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textboxCDTLName = new System.Windows.Forms.TextBox();
            this.textboxENUMLName = new System.Windows.Forms.TextBox();
            this.textboxBIELName = new System.Windows.Forms.TextBox();
            this.textboxDOCLName = new System.Windows.Forms.TextBox();
            this.checkboxBDTL = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.checkboxDefaultValues = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.cbxMajor = new System.Windows.Forms.ComboBox();
            this.cbxMinor = new System.Windows.Forms.ComboBox();
            this.txtComment = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textboxModelName = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.checkboxImportStandardLibraries = new System.Windows.Forms.CheckBox();
            this.label13 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.rtxtStatus = new System.Windows.Forms.RichTextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonGenerate
            // 
            this.buttonGenerate.Location = new System.Drawing.Point(74, 626);
            this.buttonGenerate.Name = "buttonGenerate";
            this.buttonGenerate.Size = new System.Drawing.Size(176, 23);
            this.buttonGenerate.TabIndex = 10;
            this.buttonGenerate.Text = "&Generate Default Model...";
            this.buttonGenerate.UseVisualStyleBackColor = true;
            this.buttonGenerate.Click += new System.EventHandler(this.buttonGenerate_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(305, 625);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(62, 23);
            this.buttonClose.TabIndex = 11;
            this.buttonClose.Text = "C&lose";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.groupBox4);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.checkboxImportStandardLibraries);
            this.groupBox1.Controls.Add(this.checkboxDefaultValues);
            this.groupBox1.Location = new System.Drawing.Point(11, 43);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(419, 434);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Libraries within the Business Library";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.checkboxDOCL);
            this.groupBox4.Controls.Add(this.checkboxPRIML);
            this.groupBox4.Controls.Add(this.checkboxENUML);
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Controls.Add(this.checkboxBIEL);
            this.groupBox4.Controls.Add(this.checkboxCDTL);
            this.groupBox4.Controls.Add(this.textboxBDTLName);
            this.groupBox4.Controls.Add(this.checkboxCCL);
            this.groupBox4.Controls.Add(this.textboxCCLName);
            this.groupBox4.Controls.Add(this.textboxPRIMLName);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.label7);
            this.groupBox4.Controls.Add(this.label8);
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Controls.Add(this.textboxCDTLName);
            this.groupBox4.Controls.Add(this.textboxENUMLName);
            this.groupBox4.Controls.Add(this.textboxBIELName);
            this.groupBox4.Controls.Add(this.textboxDOCLName);
            this.groupBox4.Controls.Add(this.checkboxBDTL);
            this.groupBox4.Location = new System.Drawing.Point(30, 42);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(376, 194);
            this.groupBox4.TabIndex = 21;
            this.groupBox4.TabStop = false;
            // 
            // checkboxDOCL
            // 
            this.checkboxDOCL.AutoSize = true;
            this.checkboxDOCL.Location = new System.Drawing.Point(9, 169);
            this.checkboxDOCL.Name = "checkboxDOCL";
            this.checkboxDOCL.Size = new System.Drawing.Size(15, 14);
            this.checkboxDOCL.TabIndex = 10;
            this.checkboxDOCL.UseVisualStyleBackColor = true;
            this.checkboxDOCL.CheckedChanged += new System.EventHandler(this.checkboxDOCL_CheckedChanged);
            // 
            // checkboxPRIML
            // 
            this.checkboxPRIML.AutoSize = true;
            this.checkboxPRIML.Location = new System.Drawing.Point(9, 14);
            this.checkboxPRIML.Name = "checkboxPRIML";
            this.checkboxPRIML.Size = new System.Drawing.Size(15, 14);
            this.checkboxPRIML.TabIndex = 0;
            this.checkboxPRIML.UseVisualStyleBackColor = true;
            this.checkboxPRIML.CheckedChanged += new System.EventHandler(this.checkboxPRIML_CheckedChanged);
            // 
            // checkboxENUML
            // 
            this.checkboxENUML.AutoSize = true;
            this.checkboxENUML.Location = new System.Drawing.Point(9, 40);
            this.checkboxENUML.Name = "checkboxENUML";
            this.checkboxENUML.Size = new System.Drawing.Size(15, 14);
            this.checkboxENUML.TabIndex = 15;
            this.checkboxENUML.UseVisualStyleBackColor = true;
            this.checkboxENUML.CheckedChanged += new System.EventHandler(this.checkboxENUML_CheckedChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(25, 66);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(66, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "CDT Library:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(25, 170);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "DOC Library:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(25, 14);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "PRIM Library:";
            // 
            // checkboxBIEL
            // 
            this.checkboxBIEL.AutoSize = true;
            this.checkboxBIEL.Location = new System.Drawing.Point(9, 144);
            this.checkboxBIEL.Name = "checkboxBIEL";
            this.checkboxBIEL.Size = new System.Drawing.Size(15, 14);
            this.checkboxBIEL.TabIndex = 11;
            this.checkboxBIEL.UseVisualStyleBackColor = true;
            this.checkboxBIEL.CheckedChanged += new System.EventHandler(this.checkboxBIEL_CheckedChanged);
            // 
            // checkboxCDTL
            // 
            this.checkboxCDTL.AutoSize = true;
            this.checkboxCDTL.Location = new System.Drawing.Point(9, 66);
            this.checkboxCDTL.Name = "checkboxCDTL";
            this.checkboxCDTL.Size = new System.Drawing.Size(15, 14);
            this.checkboxCDTL.TabIndex = 14;
            this.checkboxCDTL.UseVisualStyleBackColor = true;
            this.checkboxCDTL.CheckedChanged += new System.EventHandler(this.checkboxCDTL_CheckedChanged);
            // 
            // textboxBDTLName
            // 
            this.textboxBDTLName.Enabled = false;
            this.textboxBDTLName.Location = new System.Drawing.Point(105, 115);
            this.textboxBDTLName.Name = "textboxBDTLName";
            this.textboxBDTLName.Size = new System.Drawing.Size(263, 20);
            this.textboxBDTLName.TabIndex = 13;
            this.textboxBDTLName.TextChanged += new System.EventHandler(this.textboxBDTLName_TextChanged);
            // 
            // checkboxCCL
            // 
            this.checkboxCCL.AutoSize = true;
            this.checkboxCCL.Location = new System.Drawing.Point(9, 92);
            this.checkboxCCL.Name = "checkboxCCL";
            this.checkboxCCL.Size = new System.Drawing.Size(15, 14);
            this.checkboxCCL.TabIndex = 13;
            this.checkboxCCL.UseVisualStyleBackColor = true;
            this.checkboxCCL.CheckedChanged += new System.EventHandler(this.checkboxCCL_CheckedChanged);
            // 
            // textboxCCLName
            // 
            this.textboxCCLName.Enabled = false;
            this.textboxCCLName.Location = new System.Drawing.Point(105, 89);
            this.textboxCCLName.Name = "textboxCCLName";
            this.textboxCCLName.Size = new System.Drawing.Size(263, 20);
            this.textboxCCLName.TabIndex = 14;
            this.textboxCCLName.TextChanged += new System.EventHandler(this.textboxCCLName_TextChanged);
            // 
            // textboxPRIMLName
            // 
            this.textboxPRIMLName.Enabled = false;
            this.textboxPRIMLName.Location = new System.Drawing.Point(105, 11);
            this.textboxPRIMLName.Name = "textboxPRIMLName";
            this.textboxPRIMLName.Size = new System.Drawing.Size(263, 20);
            this.textboxPRIMLName.TabIndex = 3;
            this.textboxPRIMLName.TextChanged += new System.EventHandler(this.textboxPRIMLName_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(25, 40);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(76, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "ENUM Library:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(25, 92);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(58, 13);
            this.label7.TabIndex = 6;
            this.label7.Text = "CC Library:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(25, 144);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(61, 13);
            this.label8.TabIndex = 7;
            this.label8.Text = "BIE Library:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(25, 118);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "BDT Library:";
            // 
            // textboxCDTLName
            // 
            this.textboxCDTLName.Enabled = false;
            this.textboxCDTLName.Location = new System.Drawing.Point(105, 63);
            this.textboxCDTLName.Name = "textboxCDTLName";
            this.textboxCDTLName.Size = new System.Drawing.Size(263, 20);
            this.textboxCDTLName.TabIndex = 15;
            this.textboxCDTLName.TextChanged += new System.EventHandler(this.textboxCDTLName_TextChanged);
            // 
            // textboxENUMLName
            // 
            this.textboxENUMLName.Enabled = false;
            this.textboxENUMLName.Location = new System.Drawing.Point(105, 37);
            this.textboxENUMLName.Name = "textboxENUMLName";
            this.textboxENUMLName.Size = new System.Drawing.Size(263, 20);
            this.textboxENUMLName.TabIndex = 10;
            this.textboxENUMLName.TextChanged += new System.EventHandler(this.textboxENUMLName_TextChanged);
            // 
            // textboxBIELName
            // 
            this.textboxBIELName.Enabled = false;
            this.textboxBIELName.Location = new System.Drawing.Point(105, 141);
            this.textboxBIELName.Name = "textboxBIELName";
            this.textboxBIELName.Size = new System.Drawing.Size(263, 20);
            this.textboxBIELName.TabIndex = 12;
            this.textboxBIELName.TextChanged += new System.EventHandler(this.textboxBIELName_TextChanged);
            // 
            // textboxDOCLName
            // 
            this.textboxDOCLName.Enabled = false;
            this.textboxDOCLName.Location = new System.Drawing.Point(105, 167);
            this.textboxDOCLName.Name = "textboxDOCLName";
            this.textboxDOCLName.Size = new System.Drawing.Size(263, 20);
            this.textboxDOCLName.TabIndex = 11;
            this.textboxDOCLName.TextChanged += new System.EventHandler(this.textboxDOCLName_TextChanged);
            // 
            // checkboxBDTL
            // 
            this.checkboxBDTL.AutoSize = true;
            this.checkboxBDTL.Location = new System.Drawing.Point(9, 118);
            this.checkboxBDTL.Name = "checkboxBDTL";
            this.checkboxBDTL.Size = new System.Drawing.Size(15, 14);
            this.checkboxBDTL.TabIndex = 12;
            this.checkboxBDTL.UseVisualStyleBackColor = true;
            this.checkboxBDTL.CheckedChanged += new System.EventHandler(this.checkboxBDTL_CheckedChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(27, 22);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(353, 13);
            this.label9.TabIndex = 16;
            this.label9.Text = "Use default names for generated Libraries (e.g. name of the PRIM Library)";
            // 
            // checkboxDefaultValues
            // 
            this.checkboxDefaultValues.AutoSize = true;
            this.checkboxDefaultValues.Location = new System.Drawing.Point(11, 22);
            this.checkboxDefaultValues.Name = "checkboxDefaultValues";
            this.checkboxDefaultValues.Size = new System.Drawing.Size(15, 14);
            this.checkboxDefaultValues.TabIndex = 13;
            this.checkboxDefaultValues.UseVisualStyleBackColor = true;
            this.checkboxDefaultValues.CheckedChanged += new System.EventHandler(this.checkboxDefaultValues_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtComment);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.cbxMajor);
            this.groupBox2.Controls.Add(this.cbxMinor);
            this.groupBox2.Location = new System.Drawing.Point(30, 271);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(376, 150);
            this.groupBox2.TabIndex = 20;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Core Component Library Version";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(7, 24);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(33, 13);
            this.label11.TabIndex = 0;
            this.label11.Text = "Major";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(283, 24);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(33, 13);
            this.label12.TabIndex = 1;
            this.label12.Text = "Minor";
            // 
            // cbxMajor
            // 
            this.cbxMajor.FormattingEnabled = true;
            this.cbxMajor.Location = new System.Drawing.Point(46, 21);
            this.cbxMajor.Name = "cbxMajor";
            this.cbxMajor.Size = new System.Drawing.Size(213, 21);
            this.cbxMajor.Sorted = true;
            this.cbxMajor.TabIndex = 3;
            this.cbxMajor.SelectionChangeCommitted += new System.EventHandler(this.cbxMajor_SelectionChangeCommitted);
            // 
            // cbxMinor
            // 
            this.cbxMinor.FormattingEnabled = true;
            this.cbxMinor.Location = new System.Drawing.Point(321, 21);
            this.cbxMinor.Name = "cbxMinor";
            this.cbxMinor.Size = new System.Drawing.Size(47, 21);
            this.cbxMinor.Sorted = true;
            this.cbxMinor.TabIndex = 4;
            this.cbxMinor.SelectionChangeCommitted += new System.EventHandler(this.cbxMinor_SelectionChangeCommitted);
            // 
            // txtComment
            // 
            this.txtComment.BackColor = System.Drawing.SystemColors.Info;
            this.txtComment.Location = new System.Drawing.Point(9, 66);
            this.txtComment.Multiline = true;
            this.txtComment.Name = "txtComment";
            this.txtComment.ReadOnly = true;
            this.txtComment.Size = new System.Drawing.Size(359, 74);
            this.txtComment.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Model Name:";
            // 
            // textboxModelName
            // 
            this.textboxModelName.Location = new System.Drawing.Point(85, 10);
            this.textboxModelName.Name = "textboxModelName";
            this.textboxModelName.Size = new System.Drawing.Size(345, 20);
            this.textboxModelName.TabIndex = 9;
            this.textboxModelName.TextChanged += new System.EventHandler(this.textboxModelName_TextChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(27, 250);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(359, 13);
            this.label10.TabIndex = 18;
            this.label10.Text = "Import standard CC Librares (e.g. PRIM Library containing standard PRIMs)";
            // 
            // checkboxImportStandardLibraries
            // 
            this.checkboxImportStandardLibraries.AutoSize = true;
            this.checkboxImportStandardLibraries.Location = new System.Drawing.Point(11, 250);
            this.checkboxImportStandardLibraries.Name = "checkboxImportStandardLibraries";
            this.checkboxImportStandardLibraries.Size = new System.Drawing.Size(15, 14);
            this.checkboxImportStandardLibraries.TabIndex = 17;
            this.checkboxImportStandardLibraries.UseVisualStyleBackColor = true;
            this.checkboxImportStandardLibraries.CheckedChanged += new System.EventHandler(this.checkboxImportStandardLibraries_CheckedChanged);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(7, 48);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(51, 13);
            this.label13.TabIndex = 12;
            this.label13.Text = "Comment";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.rtxtStatus);
            this.groupBox3.Location = new System.Drawing.Point(11, 480);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(419, 133);
            this.groupBox3.TabIndex = 12;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Status";
            // 
            // rtxtStatus
            // 
            this.rtxtStatus.BackColor = System.Drawing.SystemColors.Control;
            this.rtxtStatus.Location = new System.Drawing.Point(10, 18);
            this.rtxtStatus.Name = "rtxtStatus";
            this.rtxtStatus.Size = new System.Drawing.Size(395, 103);
            this.rtxtStatus.TabIndex = 0;
            this.rtxtStatus.Text = "";
            this.rtxtStatus.WordWrap = false;
            // 
            // UpccModelWizardForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(440, 657);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.textboxModelName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.buttonGenerate);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UpccModelWizardForm";
            this.Text = "Generate default UPCC Model Wizard";
            this.Load += new System.EventHandler(this.UPCCModelWizardForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonGenerate;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox checkboxDOCL;
        private System.Windows.Forms.CheckBox checkboxDefaultValues;
        private System.Windows.Forms.CheckBox checkboxENUML;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox checkboxBIEL;
        private System.Windows.Forms.TextBox textboxBDTLName;
        private System.Windows.Forms.TextBox textboxCCLName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textboxCDTLName;
        private System.Windows.Forms.TextBox textboxBIELName;
        private System.Windows.Forms.CheckBox checkboxBDTL;
        private System.Windows.Forms.TextBox textboxDOCLName;
        private System.Windows.Forms.TextBox textboxENUMLName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textboxPRIMLName;
        private System.Windows.Forms.CheckBox checkboxCCL;
        private System.Windows.Forms.CheckBox checkboxCDTL;
        private System.Windows.Forms.CheckBox checkboxPRIML;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textboxModelName;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox checkboxImportStandardLibraries;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox cbxMajor;
        private System.Windows.Forms.ComboBox cbxMinor;
        private System.Windows.Forms.TextBox txtComment;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RichTextBox rtxtStatus;
    }
}