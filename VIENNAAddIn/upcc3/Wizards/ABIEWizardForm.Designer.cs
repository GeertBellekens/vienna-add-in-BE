namespace VIENNAAddIn.upcc3.Wizards
{
    partial class ABIEWizardForm
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
            this.groupboxSettings = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textPrefix = new System.Windows.Forms.TextBox();
            this.tabcontrolACC = new System.Windows.Forms.TabControl();
            this.TabControlPageAttributes = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.buttonAddBBIE = new System.Windows.Forms.Button();
            this.checkedlistboxBBIEs = new System.Windows.Forms.CheckedListBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.checkedlistboxBCCs = new System.Windows.Forms.CheckedListBox();
            this.checkboxAttributes = new System.Windows.Forms.CheckBox();
            this.groupboxBDTs = new System.Windows.Forms.GroupBox();
            this.checkedlistboxBDTs = new System.Windows.Forms.CheckedListBox();
            this.TabControlPageAssociations = new System.Windows.Forms.TabPage();
            this.groupboxASBIEs = new System.Windows.Forms.GroupBox();
            this.checkedlistboxASCCs = new System.Windows.Forms.CheckedListBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBDTLs = new System.Windows.Forms.ComboBox();
            this.comboCCLs = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comboACCs = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textABIEName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBIELs = new System.Windows.Forms.ComboBox();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonGenerate = new System.Windows.Forms.Button();
            this.groupboxStatus = new System.Windows.Forms.GroupBox();
            this.richtextStatus = new System.Windows.Forms.RichTextBox();
            this.groupboxSettings.SuspendLayout();
            this.tabcontrolACC.SuspendLayout();
            this.TabControlPageAttributes.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupboxBDTs.SuspendLayout();
            this.TabControlPageAssociations.SuspendLayout();
            this.groupboxASBIEs.SuspendLayout();
            this.groupboxStatus.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupboxSettings
            // 
            this.groupboxSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupboxSettings.Controls.Add(this.label6);
            this.groupboxSettings.Controls.Add(this.textPrefix);
            this.groupboxSettings.Controls.Add(this.tabcontrolACC);
            this.groupboxSettings.Controls.Add(this.label7);
            this.groupboxSettings.Controls.Add(this.label4);
            this.groupboxSettings.Controls.Add(this.comboBDTLs);
            this.groupboxSettings.Controls.Add(this.comboCCLs);
            this.groupboxSettings.Controls.Add(this.label1);
            this.groupboxSettings.Controls.Add(this.comboACCs);
            this.groupboxSettings.Controls.Add(this.label2);
            this.groupboxSettings.Controls.Add(this.textABIEName);
            this.groupboxSettings.Controls.Add(this.label3);
            this.groupboxSettings.Controls.Add(this.comboBIELs);
            this.groupboxSettings.Location = new System.Drawing.Point(4, 4);
            this.groupboxSettings.Name = "groupboxSettings";
            this.groupboxSettings.Size = new System.Drawing.Size(676, 551);
            this.groupboxSettings.TabIndex = 28;
            this.groupboxSettings.TabStop = false;
            this.groupboxSettings.Text = "Settings";
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 448);
            this.label6.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(187, 13);
            this.label6.TabIndex = 46;
            this.label6.Text = "Prefix used for the generated Artifacts:";
            // 
            // textPrefix
            // 
            this.textPrefix.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textPrefix.Location = new System.Drawing.Point(217, 445);
            this.textPrefix.Name = "textPrefix";
            this.textPrefix.Size = new System.Drawing.Size(450, 20);
            this.textPrefix.TabIndex = 45;
            this.textPrefix.TextChanged += new System.EventHandler(this.textPrefix_TextChanged);
            // 
            // tabcontrolACC
            // 
            this.tabcontrolACC.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabcontrolACC.Controls.Add(this.TabControlPageAttributes);
            this.tabcontrolACC.Controls.Add(this.TabControlPageAssociations);
            this.tabcontrolACC.Location = new System.Drawing.Point(10, 76);
            this.tabcontrolACC.Name = "tabcontrolACC";
            this.tabcontrolACC.SelectedIndex = 0;
            this.tabcontrolACC.Size = new System.Drawing.Size(659, 359);
            this.tabcontrolACC.TabIndex = 44;
            // 
            // TabControlPageAttributes
            // 
            this.TabControlPageAttributes.Controls.Add(this.groupBox2);
            this.TabControlPageAttributes.Controls.Add(this.groupBox3);
            this.TabControlPageAttributes.Controls.Add(this.groupboxBDTs);
            this.TabControlPageAttributes.Location = new System.Drawing.Point(4, 22);
            this.TabControlPageAttributes.Name = "TabControlPageAttributes";
            this.TabControlPageAttributes.Padding = new System.Windows.Forms.Padding(3);
            this.TabControlPageAttributes.Size = new System.Drawing.Size(651, 333);
            this.TabControlPageAttributes.TabIndex = 0;
            this.TabControlPageAttributes.Text = "Attributes";
            this.TabControlPageAttributes.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.buttonAddBBIE);
            this.groupBox2.Controls.Add(this.checkedlistboxBBIEs);
            this.groupBox2.Location = new System.Drawing.Point(216, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(216, 324);
            this.groupBox2.TabIndex = 45;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "BBIEs based on BCCs";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(30, 21);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(92, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "Add another BBIE";
            // 
            // buttonAddBBIE
            // 
            this.buttonAddBBIE.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.buttonAddBBIE.Location = new System.Drawing.Point(10, 18);
            this.buttonAddBBIE.Name = "buttonAddBBIE";
            this.buttonAddBBIE.Size = new System.Drawing.Size(15, 17);
            this.buttonAddBBIE.TabIndex = 29;
            this.buttonAddBBIE.Text = "+";
            this.buttonAddBBIE.UseVisualStyleBackColor = true;
            this.buttonAddBBIE.Click += new System.EventHandler(this.buttonAddBBIE_Click);
            // 
            // checkedlistboxBBIEs
            // 
            this.checkedlistboxBBIEs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.checkedlistboxBBIEs.CheckOnClick = true;
            this.checkedlistboxBBIEs.FormattingEnabled = true;
            this.checkedlistboxBBIEs.Location = new System.Drawing.Point(8, 40);
            this.checkedlistboxBBIEs.Name = "checkedlistboxBBIEs";
            this.checkedlistboxBBIEs.Size = new System.Drawing.Size(200, 274);
            this.checkedlistboxBBIEs.TabIndex = 28;
            this.checkedlistboxBBIEs.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.checkedlistboxBBIEs_ItemCheck);
            this.checkedlistboxBBIEs.DoubleClick += new System.EventHandler(this.checkedlistboxBBIEs_DoubleClick);
            this.checkedlistboxBBIEs.MouseDown += new System.Windows.Forms.MouseEventHandler(this.checkedlistboxBBIEs_MouseDown);
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.checkedlistboxBCCs);
            this.groupBox3.Controls.Add(this.checkboxAttributes);
            this.groupBox3.Location = new System.Drawing.Point(6, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(212, 324);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "BCCs used as BBIEs";
            // 
            // checkedlistboxBCCs
            // 
            this.checkedlistboxBCCs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.checkedlistboxBCCs.CheckOnClick = true;
            this.checkedlistboxBCCs.FormattingEnabled = true;
            this.checkedlistboxBCCs.Location = new System.Drawing.Point(8, 40);
            this.checkedlistboxBCCs.Name = "checkedlistboxBCCs";
            this.checkedlistboxBCCs.Size = new System.Drawing.Size(197, 274);
            this.checkedlistboxBCCs.TabIndex = 27;
            this.checkedlistboxBCCs.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.checkedlistboxBCCs_ItemCheck);
            this.checkedlistboxBCCs.MouseDown += new System.Windows.Forms.MouseEventHandler(this.checkedlistboxBCCs_MouseDown);
            // 
            // checkboxAttributes
            // 
            this.checkboxAttributes.AutoSize = true;
            this.checkboxAttributes.Location = new System.Drawing.Point(11, 17);
            this.checkboxAttributes.Name = "checkboxAttributes";
            this.checkboxAttributes.Size = new System.Drawing.Size(98, 17);
            this.checkboxAttributes.TabIndex = 26;
            this.checkboxAttributes.Text = "Select all BCCs";
            this.checkboxAttributes.UseVisualStyleBackColor = true;
            this.checkboxAttributes.MouseDown += new System.Windows.Forms.MouseEventHandler(this.checkboxAttributes_MouseDown);
            this.checkboxAttributes.CheckedChanged += new System.EventHandler(this.checkboxAttributes_CheckedChanged);
            // 
            // groupboxBDTs
            // 
            this.groupboxBDTs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupboxBDTs.Controls.Add(this.checkedlistboxBDTs);
            this.groupboxBDTs.Location = new System.Drawing.Point(430, 3);
            this.groupboxBDTs.Name = "groupboxBDTs";
            this.groupboxBDTs.Size = new System.Drawing.Size(214, 324);
            this.groupboxBDTs.TabIndex = 3;
            this.groupboxBDTs.TabStop = false;
            this.groupboxBDTs.Text = "BDTs available to typify the BBIEs";
            // 
            // checkedlistboxBDTs
            // 
            this.checkedlistboxBDTs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.checkedlistboxBDTs.CheckOnClick = true;
            this.checkedlistboxBDTs.FormattingEnabled = true;
            this.checkedlistboxBDTs.Location = new System.Drawing.Point(7, 40);
            this.checkedlistboxBDTs.Name = "checkedlistboxBDTs";
            this.checkedlistboxBDTs.Size = new System.Drawing.Size(200, 274);
            this.checkedlistboxBDTs.TabIndex = 29;
            this.checkedlistboxBDTs.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.checkedlistboxBDTs_ItemCheck);
            this.checkedlistboxBDTs.DoubleClick += new System.EventHandler(this.checkedlistboxBDTs_DoubleClick);
            this.checkedlistboxBDTs.MouseDown += new System.Windows.Forms.MouseEventHandler(this.checkedlistboxBDTs_MouseDown);
            // 
            // TabControlPageAssociations
            // 
            this.TabControlPageAssociations.Controls.Add(this.groupboxASBIEs);
            this.TabControlPageAssociations.Location = new System.Drawing.Point(4, 22);
            this.TabControlPageAssociations.Name = "TabControlPageAssociations";
            this.TabControlPageAssociations.Size = new System.Drawing.Size(651, 333);
            this.TabControlPageAssociations.TabIndex = 1;
            this.TabControlPageAssociations.Text = "Associations";
            this.TabControlPageAssociations.UseVisualStyleBackColor = true;
            // 
            // groupboxASBIEs
            // 
            this.groupboxASBIEs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupboxASBIEs.Controls.Add(this.checkedlistboxASCCs);
            this.groupboxASBIEs.Location = new System.Drawing.Point(6, 3);
            this.groupboxASBIEs.Name = "groupboxASBIEs";
            this.groupboxASBIEs.Size = new System.Drawing.Size(638, 322);
            this.groupboxASBIEs.TabIndex = 0;
            this.groupboxASBIEs.TabStop = false;
            this.groupboxASBIEs.Text = "ABIEs available to associate the new ABIE with";
            // 
            // checkedlistboxASCCs
            // 
            this.checkedlistboxASCCs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.checkedlistboxASCCs.CheckOnClick = true;
            this.checkedlistboxASCCs.FormattingEnabled = true;
            this.checkedlistboxASCCs.Location = new System.Drawing.Point(11, 22);
            this.checkedlistboxASCCs.Name = "checkedlistboxASCCs";
            this.checkedlistboxASCCs.Size = new System.Drawing.Size(615, 289);
            this.checkedlistboxASCCs.TabIndex = 0;
            this.checkedlistboxASCCs.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.checkedlistboxASCCs_ItemCheck);
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(7, 500);
            this.label7.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(178, 13);
            this.label7.TabIndex = 43;
            this.label7.Text = "BDT Library used to store the BDTs:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 47);
            this.label4.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(198, 13);
            this.label4.TabIndex = 31;
            this.label4.Text = "Choose ACC used to generate the ABIE:";
            // 
            // comboBDTLs
            // 
            this.comboBDTLs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBDTLs.FormattingEnabled = true;
            this.comboBDTLs.Location = new System.Drawing.Point(217, 497);
            this.comboBDTLs.Name = "comboBDTLs";
            this.comboBDTLs.Size = new System.Drawing.Size(450, 21);
            this.comboBDTLs.TabIndex = 42;
            this.comboBDTLs.SelectedIndexChanged += new System.EventHandler(this.comboBDTLs_SelectedIndexChanged);
            // 
            // comboCCLs
            // 
            this.comboCCLs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboCCLs.FormattingEnabled = true;
            this.comboCCLs.Location = new System.Drawing.Point(217, 18);
            this.comboCCLs.Name = "comboCCLs";
            this.comboCCLs.Size = new System.Drawing.Size(450, 21);
            this.comboCCLs.TabIndex = 28;
            this.comboCCLs.SelectionChangeCommitted += new System.EventHandler(this.comboCCLs_SelectionChangeCommitted);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 527);
            this.label1.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(170, 13);
            this.label1.TabIndex = 41;
            this.label1.Text = "BIE Library used to store the ABIE:";
            // 
            // comboACCs
            // 
            this.comboACCs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboACCs.FormattingEnabled = true;
            this.comboACCs.Location = new System.Drawing.Point(217, 44);
            this.comboACCs.Name = "comboACCs";
            this.comboACCs.Size = new System.Drawing.Size(450, 21);
            this.comboACCs.TabIndex = 29;
            this.comboACCs.SelectionChangeCommitted += new System.EventHandler(this.comboACCs_SelectionChangeCommitted);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 474);
            this.label2.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(146, 13);
            this.label2.TabIndex = 39;
            this.label2.Text = "Name of the generated ABIE:";
            // 
            // textABIEName
            // 
            this.textABIEName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textABIEName.Location = new System.Drawing.Point(217, 471);
            this.textABIEName.Name = "textABIEName";
            this.textABIEName.Size = new System.Drawing.Size(450, 20);
            this.textABIEName.TabIndex = 38;
            this.textABIEName.TextChanged += new System.EventHandler(this.textABIEName_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 21);
            this.label3.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(178, 13);
            this.label3.TabIndex = 30;
            this.label3.Text = "Choose CC Library containing ACCs:";
            // 
            // comboBIELs
            // 
            this.comboBIELs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBIELs.FormattingEnabled = true;
            this.comboBIELs.Location = new System.Drawing.Point(217, 524);
            this.comboBIELs.Name = "comboBIELs";
            this.comboBIELs.Size = new System.Drawing.Size(450, 21);
            this.comboBIELs.TabIndex = 40;
            this.comboBIELs.SelectedIndexChanged += new System.EventHandler(this.comboBIELs_SelectedIndexChanged);
            // 
            // buttonClose
            // 
            this.buttonClose.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.buttonClose.Location = new System.Drawing.Point(399, 645);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(99, 22);
            this.buttonClose.TabIndex = 32;
            this.buttonClose.Text = "C&lose";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonGenerate
            // 
            this.buttonGenerate.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.buttonGenerate.Location = new System.Drawing.Point(182, 645);
            this.buttonGenerate.Name = "buttonGenerate";
            this.buttonGenerate.Size = new System.Drawing.Size(99, 22);
            this.buttonGenerate.TabIndex = 31;
            this.buttonGenerate.Text = "&Generate ABIE ...";
            this.buttonGenerate.UseVisualStyleBackColor = true;
            this.buttonGenerate.Click += new System.EventHandler(this.buttonGenerate_Click);
            // 
            // groupboxStatus
            // 
            this.groupboxStatus.Controls.Add(this.richtextStatus);
            this.groupboxStatus.Location = new System.Drawing.Point(5, 557);
            this.groupboxStatus.Name = "groupboxStatus";
            this.groupboxStatus.Size = new System.Drawing.Size(675, 79);
            this.groupboxStatus.TabIndex = 33;
            this.groupboxStatus.TabStop = false;
            this.groupboxStatus.Text = "Status";
            // 
            // richtextStatus
            // 
            this.richtextStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.richtextStatus.BackColor = System.Drawing.SystemColors.Info;
            this.richtextStatus.Location = new System.Drawing.Point(9, 19);
            this.richtextStatus.Name = "richtextStatus";
            this.richtextStatus.ReadOnly = true;
            this.richtextStatus.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.richtextStatus.Size = new System.Drawing.Size(656, 50);
            this.richtextStatus.TabIndex = 0;
            this.richtextStatus.Text = "";
            this.richtextStatus.BackColorChanged += new System.EventHandler(this.richtextStatus_BackColorChanged);
            this.richtextStatus.TextChanged += new System.EventHandler(this.richtextStatus_TextChanged);
            // 
            // ABIEWizardForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 675);
            this.Controls.Add(this.groupboxStatus);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.buttonGenerate);
            this.Controls.Add(this.groupboxSettings);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(692, 702);
            this.Name = "ABIEWizardForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Aggregated Business Information Entity (ABIE) Wizard";
            this.Load += new System.EventHandler(this.ABIEWizardForm_Load);
            this.SizeChanged += new System.EventHandler(this.ABIEWizardForm_SizeChanged);
            this.groupboxSettings.ResumeLayout(false);
            this.groupboxSettings.PerformLayout();
            this.tabcontrolACC.ResumeLayout(false);
            this.TabControlPageAttributes.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupboxBDTs.ResumeLayout(false);
            this.TabControlPageAssociations.ResumeLayout(false);
            this.groupboxASBIEs.ResumeLayout(false);
            this.groupboxStatus.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupboxSettings;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboCCLs;
        private System.Windows.Forms.ComboBox comboACCs;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox comboBDTLs;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textABIEName;
        private System.Windows.Forms.ComboBox comboBIELs;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button buttonGenerate;
        private System.Windows.Forms.TabControl tabcontrolACC;
        private System.Windows.Forms.TabPage TabControlPageAttributes;
        private System.Windows.Forms.TabPage TabControlPageAssociations;
        private System.Windows.Forms.GroupBox groupboxBDTs;
        private System.Windows.Forms.CheckedListBox checkedlistboxBDTs;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button buttonAddBBIE;
        private System.Windows.Forms.CheckedListBox checkedlistboxBBIEs;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckedListBox checkedlistboxBCCs;
        private System.Windows.Forms.CheckBox checkboxAttributes;
        private System.Windows.Forms.GroupBox groupboxASBIEs;
        private System.Windows.Forms.CheckedListBox checkedlistboxASCCs;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textPrefix;
        private System.Windows.Forms.GroupBox groupboxStatus;
        private System.Windows.Forms.RichTextBox richtextStatus;

    }
}