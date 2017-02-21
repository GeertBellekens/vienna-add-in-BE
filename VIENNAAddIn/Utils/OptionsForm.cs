/*******************************************************************************
This file is part of the VIENNAAddIn project

Licensed under GNU General Public License V3 http://gplv3.fsf.org/

For further information on the VIENNAAddIn project please visit 
http://vienna-add-in.googlecode.com
*******************************************************************************/
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using EA;
using VIENNAAddIn.menu;

namespace VIENNAAddIn.Utils
{
	/// <sUMM2ary>
	/// SUMM2ary description for OptionsForm.
	/// </sUMM2ary>
	internal class OptionsForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TabControl optionsTab;
		private System.Windows.Forms.TabPage WorksheetsTabPage;
		/// <sUMM2ary>
		/// Required designer variable.
		/// </sUMM2ary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Button cancelButton;
		private EA.Repository repository;
		private System.Windows.Forms.CheckBox dontAskAgainCheckBox;
		private IssueUtil issueUtil;

		internal OptionsForm(EA.Repository repository)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			this.repository = repository;
			this.issueUtil = new IssueUtil(this.repository);
		}

		/// <sUMM2ary>
		/// Clean up any resources being used.
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

		#region Windows Form Designer generated code
		/// <sUMM2ary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </sUMM2ary>
		private void InitializeComponent()
		{
            this.optionsTab = new System.Windows.Forms.TabControl();
            this.WorksheetsTabPage = new System.Windows.Forms.TabPage();
            this.dontAskAgainCheckBox = new System.Windows.Forms.CheckBox();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.optionsTab.SuspendLayout();
            this.WorksheetsTabPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // optionsTab
            // 
            this.optionsTab.Controls.Add(this.WorksheetsTabPage);
            this.optionsTab.Location = new System.Drawing.Point(8, 8);
            this.optionsTab.Name = "optionsTab";
            this.optionsTab.SelectedIndex = 0;
            this.optionsTab.Size = new System.Drawing.Size(432, 136);
            this.optionsTab.TabIndex = 0;
            // 
            // WorksheetsTabPage
            // 
            this.WorksheetsTabPage.Controls.Add(this.dontAskAgainCheckBox);
            this.WorksheetsTabPage.Location = new System.Drawing.Point(4, 22);
            this.WorksheetsTabPage.Name = "WorksheetsTabPage";
            this.WorksheetsTabPage.Size = new System.Drawing.Size(424, 110);
            this.WorksheetsTabPage.TabIndex = 1;
            this.WorksheetsTabPage.Text = "Worksheets";
            // 
            // dontAskAgainCheckBox
            // 
            this.dontAskAgainCheckBox.Location = new System.Drawing.Point(16, 16);
            this.dontAskAgainCheckBox.Name = "dontAskAgainCheckBox";
            this.dontAskAgainCheckBox.Size = new System.Drawing.Size(184, 32);
            this.dontAskAgainCheckBox.TabIndex = 0;
            this.dontAskAgainCheckBox.Text = "Disable asking for worksheet input file every time";
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(118, 152);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(100, 24);
            this.okButton.TabIndex = 1;
            this.okButton.Text = "Ok";
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(230, 152);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(100, 24);
            this.cancelButton.TabIndex = 2;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // OptionsForm
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(448, 181);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.optionsTab);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            this.MaximizeBox = false;
            this.Name = "OptionsForm";
            this.Text = "Options";
            this.Load += new System.EventHandler(this.OptionsForm_Load);
            this.optionsTab.ResumeLayout(false);
            this.WorksheetsTabPage.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		private void OptionsForm_Load(object sender, System.EventArgs e) {
			this.dontAskAgainCheckBox.Checked = issueUtil.GetDontAskForInputWorksheet();
		}

		private void okButton_Click(object sender, System.EventArgs e) {
			// set the parameter with the value of the checkbox
			issueUtil.SetDontAskForInputWorksheet(this.dontAskAgainCheckBox.Checked);

			// close the form
			this.Close();
		}

		private void cancelButton_Click(object sender, System.EventArgs e) {
			this.Close();
		}

	    public static void ShowForm(AddInContext context)
	    {
	        new OptionsForm(context.EARepository).ShowDialog();
	    }
	}
}
