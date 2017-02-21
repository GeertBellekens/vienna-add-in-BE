namespace VIENNAAddIn.upcc3.Wizards
{
    partial class GeneratorWizardForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.richtextStatus = new System.Windows.Forms.RichTextBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.checkboxAllschemas = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textOutputDirectory = new System.Windows.Forms.TextBox();
            this.buttonBrowseFolders = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.comboModels = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.textPrefixTargetNS = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.checkedlistboxDOCs = new System.Windows.Forms.CheckedListBox();
            this.textTargetNS = new System.Windows.Forms.TextBox();
            this.checkboxAnnotations = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonGenerate = new System.Windows.Forms.Button();
            this.label16 = new System.Windows.Forms.Label();
            this.comboBIVs = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.progressBar);
            this.groupBox1.Controls.Add(this.richtextStatus);
            this.groupBox1.Location = new System.Drawing.Point(9, 342);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(536, 152);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Status";
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(14, 124);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(507, 13);
            this.progressBar.TabIndex = 1;
            // 
            // richtextStatus
            // 
            this.richtextStatus.Location = new System.Drawing.Point(14, 19);
            this.richtextStatus.Name = "richtextStatus";
            this.richtextStatus.Size = new System.Drawing.Size(507, 99);
            this.richtextStatus.TabIndex = 0;
            this.richtextStatus.Text = "";
            this.richtextStatus.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.richtextStatus_LinkClicked);
            this.richtextStatus.TextChanged += new System.EventHandler(this.richtextStatus_TextChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.checkboxAllschemas);
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.textOutputDirectory);
            this.groupBox4.Controls.Add(this.buttonBrowseFolders);
            this.groupBox4.Controls.Add(this.label10);
            this.groupBox4.Controls.Add(this.comboModels);
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Controls.Add(this.label9);
            this.groupBox4.Controls.Add(this.textPrefixTargetNS);
            this.groupBox4.Controls.Add(this.label7);
            this.groupBox4.Controls.Add(this.checkedlistboxDOCs);
            this.groupBox4.Controls.Add(this.textTargetNS);
            this.groupBox4.Controls.Add(this.checkboxAnnotations);
            this.groupBox4.Controls.Add(this.label1);
            this.groupBox4.Location = new System.Drawing.Point(9, 38);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(536, 298);
            this.groupBox4.TabIndex = 11;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Generation Settings";
            // 
            // checkboxAllschemas
            // 
            this.checkboxAllschemas.AutoSize = true;
            this.checkboxAllschemas.Checked = true;
            this.checkboxAllschemas.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkboxAllschemas.Location = new System.Drawing.Point(201, 214);
            this.checkboxAllschemas.Name = "checkboxAllschemas";
            this.checkboxAllschemas.Size = new System.Drawing.Size(15, 14);
            this.checkboxAllschemas.TabIndex = 42;
            this.checkboxAllschemas.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 266);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(181, 13);
            this.label3.TabIndex = 44;
            this.label3.Text = "Select XML schema output directory:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(11, 212);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(180, 13);
            this.label4.TabIndex = 43;
            this.label4.Text = "Generate Core Component Schemas";
            // 
            // textOutputDirectory
            // 
            this.textOutputDirectory.Location = new System.Drawing.Point(201, 262);
            this.textOutputDirectory.Name = "textOutputDirectory";
            this.textOutputDirectory.Size = new System.Drawing.Size(282, 20);
            this.textOutputDirectory.TabIndex = 43;
            this.textOutputDirectory.TextChanged += new System.EventHandler(this.textOutputDirectory_TextChanged);
            // 
            // buttonBrowseFolders
            // 
            this.buttonBrowseFolders.Location = new System.Drawing.Point(489, 262);
            this.buttonBrowseFolders.Name = "buttonBrowseFolders";
            this.buttonBrowseFolders.Size = new System.Drawing.Size(32, 21);
            this.buttonBrowseFolders.TabIndex = 42;
            this.buttonBrowseFolders.Text = "...";
            this.buttonBrowseFolders.UseVisualStyleBackColor = true;
            this.buttonBrowseFolders.Click += new System.EventHandler(this.buttonBrowseFolders_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(11, 165);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(179, 13);
            this.label10.TabIndex = 38;
            this.label10.Text = "Define Prefix for Target Namespace:";
            // 
            // comboModels
            // 
            this.comboModels.FormattingEnabled = true;
            this.comboModels.Location = new System.Drawing.Point(201, 235);
            this.comboModels.Name = "comboModels";
            this.comboModels.Size = new System.Drawing.Size(320, 21);
            this.comboModels.TabIndex = 40;
            this.comboModels.SelectionChangeCommitted += new System.EventHandler(this.comboModels_SelectionChangeCommitted);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 238);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(130, 13);
            this.label2.TabIndex = 32;
            this.label2.Text = "Choose Document Model:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(11, 139);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(135, 13);
            this.label9.TabIndex = 36;
            this.label9.Text = "Define Target Namespace:";
            // 
            // textPrefixTargetNS
            // 
            this.textPrefixTargetNS.Location = new System.Drawing.Point(201, 162);
            this.textPrefixTargetNS.Name = "textPrefixTargetNS";
            this.textPrefixTargetNS.Size = new System.Drawing.Size(320, 20);
            this.textPrefixTargetNS.TabIndex = 37;
            this.textPrefixTargetNS.TextChanged += new System.EventHandler(this.textPrefixTargetNS_TextChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(11, 21);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(157, 13);
            this.label7.TabIndex = 31;
            this.label7.Text = "Check Documents to Generate:";
            // 
            // checkedlistboxDOCs
            // 
            this.checkedlistboxDOCs.CheckOnClick = true;
            this.checkedlistboxDOCs.FormattingEnabled = true;
            this.checkedlistboxDOCs.Location = new System.Drawing.Point(201, 21);
            this.checkedlistboxDOCs.Name = "checkedlistboxDOCs";
            this.checkedlistboxDOCs.Size = new System.Drawing.Size(320, 109);
            this.checkedlistboxDOCs.TabIndex = 28;
            this.checkedlistboxDOCs.SelectedIndexChanged += new System.EventHandler(this.checkedlistboxDOCs_SelectedIndexChanged);
            this.checkedlistboxDOCs.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.checkedlistboxDOCs_ItemCheck);
            this.checkedlistboxDOCs.MouseDown += new System.Windows.Forms.MouseEventHandler(this.checkedlistboxDOCs_MouseDown);
            // 
            // textTargetNS
            // 
            this.textTargetNS.Location = new System.Drawing.Point(201, 136);
            this.textTargetNS.Name = "textTargetNS";
            this.textTargetNS.Size = new System.Drawing.Size(320, 20);
            this.textTargetNS.TabIndex = 35;
            this.textTargetNS.TextChanged += new System.EventHandler(this.textTargetNS_TextChanged);
            // 
            // checkboxAnnotations
            // 
            this.checkboxAnnotations.AutoSize = true;
            this.checkboxAnnotations.Checked = true;
            this.checkboxAnnotations.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkboxAnnotations.Location = new System.Drawing.Point(201, 191);
            this.checkboxAnnotations.Name = "checkboxAnnotations";
            this.checkboxAnnotations.Size = new System.Drawing.Size(15, 14);
            this.checkboxAnnotations.TabIndex = 33;
            this.checkboxAnnotations.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 189);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(177, 13);
            this.label1.TabIndex = 34;
            this.label1.Text = "Enable Documentation Annotations:";
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(369, 510);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 23);
            this.buttonClose.TabIndex = 3;
            this.buttonClose.Text = "C&lose";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonGenerate
            // 
            this.buttonGenerate.Location = new System.Drawing.Point(105, 510);
            this.buttonGenerate.Name = "buttonGenerate";
            this.buttonGenerate.Size = new System.Drawing.Size(149, 23);
            this.buttonGenerate.TabIndex = 2;
            this.buttonGenerate.Text = "&Generate XML Schema...";
            this.buttonGenerate.UseVisualStyleBackColor = true;
            this.buttonGenerate.Click += new System.EventHandler(this.buttonGenerate_Click);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(12, 15);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(166, 13);
            this.label16.TabIndex = 30;
            this.label16.Text = "Select Business Information View:";
            // 
            // comboBIVs
            // 
            this.comboBIVs.FormattingEnabled = true;
            this.comboBIVs.Location = new System.Drawing.Point(210, 12);
            this.comboBIVs.Name = "comboBIVs";
            this.comboBIVs.Size = new System.Drawing.Size(335, 21);
            this.comboBIVs.TabIndex = 41;
            this.comboBIVs.SelectionChangeCommitted += new System.EventHandler(this.comboBIVs_SelectionChangeCommitted);
            // 
            // GeneratorWizardForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(554, 543);
            this.Controls.Add(this.comboBIVs);
            this.Controls.Add(this.buttonGenerate);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GeneratorWizardForm";
            this.Text = "GeneratorWizardForm";
            this.Load += new System.EventHandler(this.GeneratorWizardForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RichTextBox richtextStatus;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textTargetNS;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkboxAnnotations;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckedListBox checkedlistboxDOCs;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button buttonGenerate;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox textPrefixTargetNS;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboModels;
        private System.Windows.Forms.ComboBox comboBIVs;
        private System.Windows.Forms.Button buttonBrowseFolders;
        private System.Windows.Forms.TextBox textOutputDirectory;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.CheckBox checkboxAllschemas;
        private System.Windows.Forms.Label label4;
    }
}