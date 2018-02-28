namespace ViennaStandalone
{
    partial class SelectPackageForm
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
            this.browsePackageButton = new System.Windows.Forms.Button();
            this.importPackageTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.GenerateButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // browsePackageButton
            // 
            this.browsePackageButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.browsePackageButton.Location = new System.Drawing.Point(196, 25);
            this.browsePackageButton.Name = "browsePackageButton";
            this.browsePackageButton.Size = new System.Drawing.Size(24, 20);
            this.browsePackageButton.TabIndex = 19;
            this.browsePackageButton.Text = "...";
            this.browsePackageButton.UseVisualStyleBackColor = true;
            this.browsePackageButton.Click += new System.EventHandler(this.browsePackageButton_Click);
            // 
            // importPackageTextBox
            // 
            this.importPackageTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.importPackageTextBox.Location = new System.Drawing.Point(12, 25);
            this.importPackageTextBox.MinimumSize = new System.Drawing.Size(153, 20);
            this.importPackageTextBox.Name = "importPackageTextBox";
            this.importPackageTextBox.ReadOnly = true;
            this.importPackageTextBox.Size = new System.Drawing.Size(178, 20);
            this.importPackageTextBox.TabIndex = 18;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 13);
            this.label1.TabIndex = 20;
            this.label1.Text = "EA Package";
            // 
            // GenerateButton
            // 
            this.GenerateButton.Location = new System.Drawing.Point(147, 51);
            this.GenerateButton.Name = "GenerateButton";
            this.GenerateButton.Size = new System.Drawing.Size(75, 23);
            this.GenerateButton.TabIndex = 21;
            this.GenerateButton.Text = "Generate";
            this.GenerateButton.UseVisualStyleBackColor = true;
            this.GenerateButton.Click += new System.EventHandler(this.GenerateButton_Click);
            // 
            // SelectPackageForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(232, 89);
            this.Controls.Add(this.GenerateButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.browsePackageButton);
            this.Controls.Add(this.importPackageTextBox);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(248, 128);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(248, 128);
            this.Name = "SelectPackageForm";
            this.Text = "Select Package";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button browsePackageButton;
        private System.Windows.Forms.TextBox importPackageTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button GenerateButton;
    }
}