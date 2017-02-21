/*******************************************************************************
This file is part of the VIENNAAddIn project

Licensed under GNU General Public License V3 http://gplv3.fsf.org/

For further information on the VIENNAAddIn project please visit 
http://vienna-add-in.googlecode.com
*******************************************************************************/
using System;
using System.Windows.Forms;

namespace VIENNAAddIn.Utils
{
    /// <sUMM2ary>
    /// SUMM2ary description for VIENNAAddInLoggerWindow.
    /// </sUMM2ary>
    internal class UMMAddInLoggerWindow : Form
    {
        private RichTextBox logWindow;

        /// <sUMM2ary>
        /// Required designer variable.
        /// </sUMM2ary>
        //private System.ComponentModel.Container components = null;
        internal UMMAddInLoggerWindow()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }


        /// <sUMM2ary>
        /// Append String s to the log Window
        /// </sUMM2ary>
        /// <param name="l"></param>
        internal void appendLogMessage(LogMessage l)
        {
            if (IsDisposed)
                Activate();

            String s = "";
            s += l.level + " " + l.dateTime + "\n" + l.message + "\n";
            logWindow.AppendText(s);
        }

        public void appendLogMessageCCTS(LogMessageCCTS l)
        {
            if (IsDisposed)
                Activate();

            String s = "";
            s += l.level + " " + l.dateTime + "\n" + l.message + "\n";
            logWindow.AppendText(s);
        }


        /// <sUMM2ary>
        /// Because the LoggerWindow is present as long as EA is running,
        /// we do not need to implement a Dispose function
        /// Just set the LoggerWindow in invisible instead.
        /// </sUMM2ary>
        protected override void Dispose(bool disposing)
        {
            Visible = false;
            Update();
        }

        private void logWindow_TextChanged(object sender, EventArgs e)
        {
        }

        #region Windows Form Designer generated code

        /// <sUMM2ary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </sUMM2ary>
        private void InitializeComponent()
        {
            this.logWindow = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // logWindow
            // 
            this.logWindow.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.logWindow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logWindow.Location = new System.Drawing.Point(0, 0);
            this.logWindow.Name = "logWindow";
            this.logWindow.ReadOnly = true;
            this.logWindow.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.logWindow.Size = new System.Drawing.Size(576, 125);
            this.logWindow.TabIndex = 0;
            this.logWindow.Text = "";
            this.logWindow.TextChanged += new System.EventHandler(this.logWindow_TextChanged);
            // 
            // UMMAddInLoggerWindow
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
            this.ClientSize = new System.Drawing.Size(576, 125);
            this.Controls.Add(this.logWindow);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular,
                                                System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.Name = "UMMAddInLoggerWindow";
            this.Text = "Log messages";
            this.ResumeLayout(false);
        }

        #endregion
    }
}