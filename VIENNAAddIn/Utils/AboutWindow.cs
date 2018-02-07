/*******************************************************************************
This file is part of the VIENNAAddIn project

Licensed under GNU General Public License V3 http://gplv3.fsf.org/

For further information on the VIENNAAddIn project please visit 
http://vienna-add-in.googlecode.com
*******************************************************************************/
using System;
using System.Diagnostics;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using VIENNAAddIn.menu;

namespace VIENNAAddIn.Utils
{
	/// <sUMM2ary>
	/// SUMM2ary description for AboutWindow.
	/// </sUMM2ary>
	internal class AboutWindow : System.Windows.Forms.Form
	{
		/// <sUMM2ary>
		/// Defining the constants for the about-window
		/// </sUMM2ary>
		private String VERSION = typeof(VIENNAAddIn).Assembly.GetName().Name +
			" " + typeof(VIENNAAddIn).Assembly.GetName().Version;
        private const String WEBSITE = "https://bellekens.com/";
        private const String BUG_REPORT = "https://github.com/GeertBellekens/vienna-add-in-BE/issues"; 		
        private const String COPYRIGHT = "Licensed under GNU General Public License v3";
        private const String UMMVERSION = "2.0";


		private System.Windows.Forms.TabControl tabControl;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.Label versionLabel;
		private System.Windows.Forms.Label copyrightLabel;
		private System.Windows.Forms.Button buttonClose;
		private System.Windows.Forms.TextBox versionTextBox;
        private System.Windows.Forms.TextBox copyrightTextBox;
		private System.Windows.Forms.GroupBox VIENNAAddIn;
        private System.Windows.Forms.GroupBox Support;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label UMM2VersionLabel;
		private System.Windows.Forms.TextBox UMM2TextBox;
        private LinkLabel linkLabel2;
        private LinkLabel linkLabel3;
		
		private System.ComponentModel.Container components = null;

		internal AboutWindow()
		{
			
			InitializeComponent();
	

			//initialize the textBoxes with the constants
			initializeTextBoxes();
		}

		internal void initializeTextBoxes()
		{
			this.versionTextBox.Text = VERSION;
            this.copyrightTextBox.Text = COPYRIGHT;
            this.UMM2TextBox.Text = UMMVERSION;
	
		}

		/// <sUMM2ary>
		/// 
		/// </sUMM2ary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Vom Windows Form-Designer generierter Code
		/// <sUMM2ary>
		/// 
		/// </sUMM2ary>
		private void InitializeComponent()
		{
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.Support = new System.Windows.Forms.GroupBox();
            this.linkLabel3 = new System.Windows.Forms.LinkLabel();
            this.linkLabel2 = new System.Windows.Forms.LinkLabel();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.VIENNAAddIn = new System.Windows.Forms.GroupBox();
            this.UMM2TextBox = new System.Windows.Forms.TextBox();
            this.UMM2VersionLabel = new System.Windows.Forms.Label();
            this.versionLabel = new System.Windows.Forms.Label();
            this.versionTextBox = new System.Windows.Forms.TextBox();
            this.copyrightLabel = new System.Windows.Forms.Label();
            this.copyrightTextBox = new System.Windows.Forms.TextBox();
            this.buttonClose = new System.Windows.Forms.Button();
            this.tabControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.Support.SuspendLayout();
            this.VIENNAAddIn.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPage1);
            this.tabControl.Location = new System.Drawing.Point(18, 13);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(466, 265);
            this.tabControl.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.Support);
            this.tabPage1.Controls.Add(this.VIENNAAddIn);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(458, 239);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "VIENNAAddIn";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // Support
            // 
            this.Support.Controls.Add(this.linkLabel3);
            this.Support.Controls.Add(this.linkLabel2);
            this.Support.Controls.Add(this.label2);
            this.Support.Controls.Add(this.label1);
            this.Support.Location = new System.Drawing.Point(16, 136);
            this.Support.Name = "Support";
            this.Support.Size = new System.Drawing.Size(429, 89);
            this.Support.TabIndex = 13;
            this.Support.TabStop = false;
            this.Support.Text = "Support";
            // 
            // linkLabel3
            // 
            this.linkLabel3.AutoSize = true;
            this.linkLabel3.Location = new System.Drawing.Point(112, 48);
            this.linkLabel3.Name = "linkLabel3";
            this.linkLabel3.Size = new System.Drawing.Size(291, 13);
            this.linkLabel3.TabIndex = 21;
            this.linkLabel3.TabStop = true;
            this.linkLabel3.Text = "https://github.com/GeertBellekens/vienna-add-in-BE/issues";
            this.linkLabel3.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel3_LinkClicked);
            // 
            // linkLabel2
            // 
            this.linkLabel2.AutoSize = true;
            this.linkLabel2.Location = new System.Drawing.Point(112, 24);
            this.linkLabel2.Name = "linkLabel2";
            this.linkLabel2.Size = new System.Drawing.Size(115, 13);
            this.linkLabel2.TabIndex = 20;
            this.linkLabel2.TabStop = true;
            this.linkLabel2.Text = "https://bellekens.com/";
            this.linkLabel2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel2_LinkClicked);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(16, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 24);
            this.label2.TabIndex = 16;
            this.label2.Text = "Bug-Report";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(16, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 24);
            this.label1.TabIndex = 12;
            this.label1.Text = "Website";
            // 
            // VIENNAAddIn
            // 
            this.VIENNAAddIn.Controls.Add(this.UMM2TextBox);
            this.VIENNAAddIn.Controls.Add(this.UMM2VersionLabel);
            this.VIENNAAddIn.Controls.Add(this.versionLabel);
            this.VIENNAAddIn.Controls.Add(this.versionTextBox);
            this.VIENNAAddIn.Controls.Add(this.copyrightLabel);
            this.VIENNAAddIn.Controls.Add(this.copyrightTextBox);
            this.VIENNAAddIn.Location = new System.Drawing.Point(16, 8);
            this.VIENNAAddIn.Name = "VIENNAAddIn";
            this.VIENNAAddIn.Size = new System.Drawing.Size(429, 110);
            this.VIENNAAddIn.TabIndex = 12;
            this.VIENNAAddIn.TabStop = false;
            this.VIENNAAddIn.Text = "VIENNAAddIn";
            // 
            // UMM2TextBox
            // 
            this.UMM2TextBox.Location = new System.Drawing.Point(112, 48);
            this.UMM2TextBox.Name = "UMM2TextBox";
            this.UMM2TextBox.ReadOnly = true;
            this.UMM2TextBox.Size = new System.Drawing.Size(251, 21);
            this.UMM2TextBox.TabIndex = 5;
            // 
            // UMM2VersionLabel
            // 
            this.UMM2VersionLabel.Location = new System.Drawing.Point(16, 48);
            this.UMM2VersionLabel.Name = "UMM2VersionLabel";
            this.UMM2VersionLabel.Size = new System.Drawing.Size(96, 24);
            this.UMM2VersionLabel.TabIndex = 4;
            this.UMM2VersionLabel.Text = "UMM-Version";
            // 
            // versionLabel
            // 
            this.versionLabel.Location = new System.Drawing.Point(16, 24);
            this.versionLabel.Name = "versionLabel";
            this.versionLabel.Size = new System.Drawing.Size(96, 24);
            this.versionLabel.TabIndex = 0;
            this.versionLabel.Text = "Version";
            // 
            // versionTextBox
            // 
            this.versionTextBox.Location = new System.Drawing.Point(112, 24);
            this.versionTextBox.Name = "versionTextBox";
            this.versionTextBox.ReadOnly = true;
            this.versionTextBox.Size = new System.Drawing.Size(251, 21);
            this.versionTextBox.TabIndex = 2;
            // 
            // copyrightLabel
            // 
            this.copyrightLabel.Location = new System.Drawing.Point(16, 72);
            this.copyrightLabel.Name = "copyrightLabel";
            this.copyrightLabel.Size = new System.Drawing.Size(96, 32);
            this.copyrightLabel.TabIndex = 1;
            this.copyrightLabel.Text = "Copyright";
            // 
            // copyrightTextBox
            // 
            this.copyrightTextBox.Location = new System.Drawing.Point(112, 72);
            this.copyrightTextBox.Multiline = true;
            this.copyrightTextBox.Name = "copyrightTextBox";
            this.copyrightTextBox.ReadOnly = true;
            this.copyrightTextBox.Size = new System.Drawing.Size(251, 21);
            this.copyrightTextBox.TabIndex = 3;
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(202, 293);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(100, 24);
            this.buttonClose.TabIndex = 1;
            this.buttonClose.Text = "Close";
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // AboutWindow
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
            this.ClientSize = new System.Drawing.Size(506, 324);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.tabControl);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "AboutWindow";
            this.Text = "About...";
            this.tabControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.Support.ResumeLayout(false);
            this.Support.PerformLayout();
            this.VIENNAAddIn.ResumeLayout(false);
            this.VIENNAAddIn.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion

		private void buttonClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void websiteLabel_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			e.Link.Visited = true;
			Process.Start(WEBSITE);
		}

		private void bugReportLabel_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			e.Link.Visited = true;
			Process.Start(BUG_REPORT);
		}
        


        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://bellekens.com/");
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            Process.Start("https://github.com/GeertBellekens/vienna-add-in-BE/issues");
        }

	    public static void ShowForm(AddInContext context)
	    {
	        new AboutWindow().Show();
	    }
	}
}
