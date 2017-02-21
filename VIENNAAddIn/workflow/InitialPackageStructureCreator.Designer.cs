namespace VIENNAAddIn.workflow
{
    partial class InitialPackageStructureCreator
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InitialPackageStructureCreator));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.modelName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.useForAllButton = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.nameBIV = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.nameBCV = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.nameBRV = new System.Windows.Forms.TextBox();
            this.checkBRV = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.createButton = new System.Windows.Forms.Button();
            this.closeButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.modelName);
            this.groupBox1.Location = new System.Drawing.Point(12, 92);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(396, 49);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Name of the model";
            // 
            // modelName
            // 
            this.modelName.Location = new System.Drawing.Point(19, 20);
            this.modelName.Name = "modelName";
            this.modelName.Size = new System.Drawing.Size(360, 20);
            this.modelName.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(355, 65);
            this.label1.TabIndex = 0;
            this.label1.Text = resources.GetString("label1.Text");
            // 
            // useForAllButton
            // 
            this.useForAllButton.Location = new System.Drawing.Point(16, 148);
            this.useForAllButton.Name = "useForAllButton";
            this.useForAllButton.Size = new System.Drawing.Size(195, 23);
            this.useForAllButton.TabIndex = 2;
            this.useForAllButton.Text = "Use Model Name for all Views";
            this.useForAllButton.UseVisualStyleBackColor = true;
            this.useForAllButton.Click += new System.EventHandler(this.useForAllButton_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.nameBIV);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.nameBCV);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.nameBRV);
            this.groupBox2.Controls.Add(this.checkBRV);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(12, 189);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(396, 184);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Views of the model";
            // 
            // nameBIV
            // 
            this.nameBIV.Location = new System.Drawing.Point(19, 146);
            this.nameBIV.Name = "nameBIV";
            this.nameBIV.Size = new System.Drawing.Size(360, 20);
            this.nameBIV.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 129);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(130, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Business Information View";
            // 
            // nameBCV
            // 
            this.nameBCV.Location = new System.Drawing.Point(19, 95);
            this.nameBCV.Name = "nameBCV";
            this.nameBCV.Size = new System.Drawing.Size(360, 20);
            this.nameBCV.TabIndex = 4;
            this.nameBCV.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 79);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(144, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Business Choreography View";
            // 
            // nameBRV
            // 
            this.nameBRV.Location = new System.Drawing.Point(19, 46);
            this.nameBRV.Name = "nameBRV";
            this.nameBRV.Size = new System.Drawing.Size(360, 20);
            this.nameBRV.TabIndex = 2;
            // 
            // checkBRV
            // 
            this.checkBRV.AutoSize = true;
            this.checkBRV.Location = new System.Drawing.Point(222, 20);
            this.checkBRV.Name = "checkBRV";
            this.checkBRV.Size = new System.Drawing.Size(15, 14);
            this.checkBRV.TabIndex = 1;
            this.checkBRV.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(189, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Business Requirements View (optional)";
            // 
            // createButton
            // 
            this.createButton.Location = new System.Drawing.Point(16, 380);
            this.createButton.Name = "createButton";
            this.createButton.Size = new System.Drawing.Size(75, 23);
            this.createButton.TabIndex = 4;
            this.createButton.Text = "Create";
            this.createButton.UseVisualStyleBackColor = true;
            this.createButton.Click += new System.EventHandler(this.createButton_Click);
            // 
            // closeButton
            // 
            this.closeButton.Location = new System.Drawing.Point(98, 380);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(75, 23);
            this.closeButton.TabIndex = 5;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // InitialPackageStructureCreator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(420, 416);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.createButton);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.useForAllButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox1);
            this.Name = "InitialPackageStructureCreator";
            this.Text = "Create inital UMM2 package structure";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox modelName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button useForAllButton;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox nameBIV;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox nameBCV;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox nameBRV;
        private System.Windows.Forms.CheckBox checkBRV;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button createButton;
        private System.Windows.Forms.Button closeButton;
    }
}