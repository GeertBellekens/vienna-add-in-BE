/*******************************************************************************
This file is part of the VIENNAAddIn project

Licensed under GNU General Public License V3 http://gplv3.fsf.org/

For further information on the VIENNAAddIn project please visit 
http://vienna-add-in.googlecode.com
*******************************************************************************/
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.Office.Interop.Word;

namespace VIENNAAddIn.ErrorReporter
{
    /// <summary>
    /// summary description for ErrorReporterForm.
    /// </summary>
    public class ErrorReporterForm : Form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private Container components;

        private RichTextBox errorMessageBox;

        private Label label;
        private Label label1;
        private Label label2;
        private LinkLabel linkLabel1;
        private String message = "";

        public ErrorReporterForm(String message, int eaVersion)
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            String s = "";

            s += "EA-Version: " + eaVersion + "\n";
            s += "Add-In-Version: " + typeof (VIENNAAddIn).Assembly.GetName().Name + " " +
                 typeof (VIENNAAddIn).Assembly.GetName().Version + "\n";
            s += ".NET version: " + Environment.Version + "\n";
            s += "OS version: " + Environment.OSVersion + "\n";

            String wordversion;
            try
            {
                var myWordApp = new ApplicationClass();
                wordversion = myWordApp.Version;
            }
            catch (Exception)
            {
                wordversion = "Not installed.";
            }

            s += "Word version: " + wordversion;

            message += "\n\n" + s;


            //
            // TODO: Add any constructor code after InitializeComponent call
            //
            if (message != null)
            {
                this.message = message;
                errorMessageBox.AppendText(message);
            }

            Show();
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Send the error email
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Do not send the error email - close the error dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://code.google.com/p/vienna-add-in/issues/list");
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            var resources = new System.ComponentModel.ComponentResourceManager(typeof (ErrorReporterForm));
            this.label = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.errorMessageBox = new System.Windows.Forms.RichTextBox();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label
            // 
            this.label.Location = new System.Drawing.Point(8, 8);
            this.label.Name = "label";
            this.label.Size = new System.Drawing.Size(713, 17);
            this.label.TabIndex = 0;
            this.label.Text = "An unexpected exception occurred during the execution of the AddIn.";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(8, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(713, 32);
            this.label1.TabIndex = 1;
            this.label1.Text = resources.GetString("label1.Text");
            // 
            // errorMessageBox
            // 
            this.errorMessageBox.Location = new System.Drawing.Point(8, 111);
            this.errorMessageBox.Name = "errorMessageBox";
            this.errorMessageBox.Size = new System.Drawing.Size(713, 263);
            this.errorMessageBox.TabIndex = 2;
            this.errorMessageBox.Text = "";
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(8, 85);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(96, 13);
            this.linkLabel1.TabIndex = 3;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Open issue tracker";
            this.linkLabel1.LinkClicked +=
                new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 62);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(387, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Before you can submit an issue you have to login in with a valid Google account.";
            // 
            // ErrorReport
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(733, 386);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.errorMessageBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label);
            this.Name = "ErrorReport";
            this.Text = "Error report";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
    }
}