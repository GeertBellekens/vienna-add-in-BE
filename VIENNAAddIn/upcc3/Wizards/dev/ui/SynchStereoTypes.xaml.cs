// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************

using System.ComponentModel;
using System.Windows.Input;
using EA;
using VIENNAAddIn.menu;
using VIENNAAddIn.Settings;

namespace VIENNAAddIn.upcc3.Wizards.dev.ui
{
    public partial class SynchStereoTypes
    {
        private SynchTaggedValues synchTaggedValues;
        private readonly BackgroundWorker bw;
        private readonly Repository repo;

        public SynchStereoTypes(Repository repository)
        {
            InitializeComponent();
            repo = repository;
            bw = new BackgroundWorker { WorkerReportsProgress = true, WorkerSupportsCancellation = false };
            bw.DoWork += bw_DoWork;
            bw.ProgressChanged += bw_ProgressChanged;
            bw.RunWorkerCompleted += bw_RunWorkerCompleted;
        }

        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Close.IsEnabled = true;
            Mouse.OverrideCursor = Cursors.Arrow;
            rtxtStatus.Text = "Done." + "\n" + rtxtStatus.Text;
        }

        private void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            rtxtStatus.Text = "Added tagged value '" + e.UserState + "'." + "\n" + rtxtStatus.Text;
        }

        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            var localSynchTaggedValues = (SynchTaggedValues) e.Argument;
            localSynchTaggedValues.TaggedValueFixed += x => bw.ReportProgress(0, x);
            localSynchTaggedValues.FixTaggedValues();
        }


        public static void ShowForm(AddInContext context)
        {
            new SynchStereoTypes(context.EARepository).ShowDialog();
        }

        private void FixAll_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            FixAll.IsEnabled = false;
            Close.IsEnabled = false;
            rtxtStatus.Text = "Adding missing tagged values..." + "\n" + rtxtStatus.Text;
            Mouse.OverrideCursor = Cursors.Wait;
            synchTaggedValues = new SynchTaggedValues(repo, (bool)removeUnusedTaggedValues.IsChecked);
            bw.RunWorkerAsync(synchTaggedValues);
        }

        private void Close_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Close();
        }
    }
}