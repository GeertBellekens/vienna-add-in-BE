// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************

using System;
using System.ComponentModel;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using EA;
using VIENNAAddIn.menu;
using VIENNAAddIn.upcc3.Wizards.util;
using Cursors=System.Windows.Input.Cursors;
using MessageBox=System.Windows.Forms.MessageBox;

namespace VIENNAAddIn.upcc3.Wizards.dev.ui
{
    public partial class StandardLibraryImporter
    {
        private readonly Repository eaRepository;
        private string cclPath;
        private FileBasedVersionHandler versionHandler;
        private readonly BackgroundWorker bw;

        public StandardLibraryImporter(Repository eaRepository)
        {
            this.eaRepository = eaRepository;
            InitializeComponent();
            bw = new BackgroundWorker {WorkerReportsProgress = true, WorkerSupportsCancellation = false};
            bw.DoWork += bw_DoWork;
            bw.ProgressChanged += bw_ProgressChanged;
            bw.RunWorkerCompleted += bw_RunWorkerCompleted;

            WindowLoaded();
        }

        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            var importer = (LibraryImporter) e.Argument;
            importer.StatusChanged += x => bw.ReportProgress(0,x);
            importer.ImportStandardCcLibraries();
        }
        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            buttonClose.IsEnabled = true;
            Mouse.OverrideCursor = Cursors.Arrow;
        }
        private void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            rtxtStatus.Text = (string)e.UserState + "\n" + rtxtStatus.Text;
        }


        public static void ShowForm(AddInContext context)
        {
            new StandardLibraryImporter(context.EARepository).ShowDialog();
        }

        private void WindowLoaded()
        {
            try
            {
                cclPath = "http://www.umm-dev.org/xmi/";
                versionHandler = new FileBasedVersionHandler(new RemoteVersionsFile(cclPath + "ccl_versions.txt"));
                versionHandler.RetrieveAvailableVersions();
            }
            catch (WebException)
            {
                //changed to users folder in order to resolve issue 70
                cclPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\ViennaAddIn\\upcc3\\resources\\ccl\\";
                versionHandler = new FileBasedVersionHandler(new LocalVersionsFile(cclPath + "ccl_versions.txt"));
                versionHandler.RetrieveAvailableVersions();
            }

            foreach (string majorVersion in versionHandler.GetMajorVersions())
            {
                cbxMajor.Items.Add(majorVersion);
            }

            cbxMajor.SelectedIndex = cbxMajor.Items.Count - 1;

            PopulateCbxMinor();
        }

        private void PopulateCbxMinor()
        {
            cbxMinor.Items.Clear();

            foreach (string minorVersion in versionHandler.GetMinorVersions(cbxMajor.SelectedItem.ToString()))
            {
                cbxMinor.Items.Add(minorVersion);
            }

            cbxMinor.SelectedIndex = cbxMinor.Items.Count - 1;
            PopulateTxtComment();
        }

        private void PopulateTxtComment()
        {
            if (cbxMajor.SelectedItem != null && cbxMinor.SelectedItem != null)
                txtComment.Text = versionHandler.GetComment(cbxMajor.SelectedItem.ToString(),
                                                            cbxMinor.SelectedItem.ToString());
        }

        private void cbxMinor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PopulateTxtComment();
        }

        private void cbxMajor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PopulateCbxMinor();
        }

        private void buttonImport_Click(object sender, RoutedEventArgs e)
        {
            const string warnMessage = "Importing the standard CC libraries will overwrite all existing:\n\n"
                                       + "    - ENUM libraries named \"ENUMLibrary\",\n"
                                       + "    - PRIM libraries named \"PRIMLibrary\",\n"
                                       + "    - CDT libraries named \"CDTLibrary \", and \n"
                                       + "    - CC libraries named \"CCLibrary\"\n\n"
                                       + "Are you sure you want to proceed?";
            const string caption = "VIENNA Add-In Warning";

            DialogResult dialogResult = MessageBox.Show(warnMessage, caption, MessageBoxButtons.YesNo,
                                                        MessageBoxIcon.Exclamation);

            if (dialogResult == System.Windows.Forms.DialogResult.Yes)
            {
                Mouse.OverrideCursor = Cursors.Wait;
                buttonClose.IsEnabled = false;
                buttonImport.IsEnabled = false;

                string bLibraryGuid = eaRepository.GetTreeSelectedPackage().Element.ElementGUID;
                Package bLibrary = eaRepository.GetPackageByGuid(bLibraryGuid);

                var resourceDescriptor = new ResourceDescriptor(cclPath, cbxMajor.SelectedItem.ToString(),
                                                                cbxMinor.SelectedItem.ToString());

                var importer = new LibraryImporter(eaRepository, resourceDescriptor) {bLibrary = bLibrary};
                bw.RunWorkerAsync(importer);
            }
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}