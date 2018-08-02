using CctsRepository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VIENNAAddIn.menu;
using VIENNAAddIn.upcc3.ea;
using VIENNAAddIn.upcc3.repo;
using VIENNAAddIn.upcc3.uml;
using System.Linq;
using VIENNAAddIn.upcc3.export.cctsndr;

namespace VIENNAAddIn.upcc3.Wizards.dev.ui
{
    public partial class ExportXSDSchemaForm : Form
    {
        private Cache cache;
        private readonly ICctsRepository cctsR;
        private EA.Package selectedPackage;
        ViennaAddinSettings settings { get; set; }

        public ExportXSDSchemaForm(ICctsRepository cctsRepository, EA.Package selectedPackage, ViennaAddinSettings settings)
        {
            this.settings = settings;
            InitializeComponent();
            cctsR = cctsRepository;
            this.destinationFolderTextBox.Text = settings.lastUsedExportPath;
            this.init(selectedPackage);
        }
        private void init(EA.Package selectedPackage)
        {
            this.selectedPackage = selectedPackage;
            if (cache == null) cache = new Cache();
            if (this.selectedPackage != null)
            {
                cache.LoadBIVs(cctsR, selectedPackage);
                this.selectedPackageTextBox.Text = selectedPackage.Name;
                MirrorDOCsToUI();
            }
            else
            {
                this.selectedPackageTextBox.Clear();
                this.messagesListView.ClearObjects();
            }
        }
        private void MirrorDOCsToUI()
        {
            this.messagesListView.Objects = cache.BIVs.Values;
            this.messagesListView.CheckAll();
            EnableDisable();
        }
        private void EnableDisable()
        {
            this.generateButton.Enabled = System.IO.Directory.Exists(this.destinationFolderTextBox.Text)
                                           && this.messagesListView.CheckedObjects.Count > 0
                                           && !this.GenerateBackgroundWorker.IsBusy;
            this.cancelButton.Enabled = this.GenerateBackgroundWorker.IsBusy;
        }

        public static void ShowForm(AddInContext context)
        {
            //clear the cache
            context.reloadRepository();
            //start
            var exporterForm = new ExportXSDSchemaForm(context.CctsRepository, context.SelectedPackage, context.settings);
            var mainWindow = context.GetMainEAWindow();
            if (mainWindow != null)
            {
                exporterForm.ShowDialog(mainWindow);
            }
            else
            {
                exporterForm.ShowDialog();
            }
        }

        private void selectPackageButton_Click(object sender, EventArgs e)
        {
            var umlPackage = ((UpccRepository)cctsR).UmlRepository.getUserSelectedPackage(this.selectedPackage.PackageGUID) as EaUmlPackage;
            this.init(umlPackage?.eaPackage);
        }

        private void browseDestinationFolderButton_Click(object sender, EventArgs e)
        {
            var ofd1 = new FolderBrowserDialog();
            ofd1.SelectedPath = this.settings.lastUsedExportPath;
            ofd1.ShowDialog();
            this.destinationFolderTextBox.Text = ofd1.SelectedPath;
        }


        private void generateButton_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            //reset status on messages
            this.resetStatus();
            //set status
            this.StatusLabel.Text = "Generating..";
            //save the last used folder
            this.settings.save();
            //actually generate the xsd's
            var generationContexts = new List<GeneratorContext>();
            var rows = this.messagesListView.CheckedObjects.Cast<cBIV>();
            //start the generation process
            this.GenerateBackgroundWorker.RunWorkerAsync(rows);
            this.EnableDisable();
        }

        private void resetStatus()
        {
            foreach (var row in this.messagesListView.Objects.Cast<cBIV>())
            {
                row.status = string.Empty;
                this.messagesListView.RefreshObject(row);
            }
        }

        private void destinationFolderTextBox_TextChanged(object sender, EventArgs e)
        {
            if (System.IO.Directory.Exists(this.destinationFolderTextBox.Text))
                this.settings.lastUsedExportPath = this.destinationFolderTextBox.Text;
            this.EnableDisable();
        }

        private void messagesListView_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            EnableDisable();
        }

        private void messagesListView_FormatRow(object sender, BrightIdeasSoftware.FormatRowEventArgs e)
        {
            var row = e.Model as cBIV;
            switch (row.status)
            {
                case "Finished":
                    e.Item.BackColor = Color.LightGreen;
                    break;
                case "Generating":
                    e.Item.BackColor = Color.LightGoldenrodYellow;
                    break;
                default:
                    e.Item.BackColor = Color.White;
                    break;
            }
        }

        private void GenerateBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var rows = (IEnumerable<cBIV>)e.Argument;
            int i = 0;
            foreach (var row in rows)
            {
                if (GenerateBackgroundWorker.CancellationPending)
                    break;
                //set status
                row.status = "Generating";
                GenerateBackgroundWorker.ReportProgress((i * 100 / rows.Count() * 100) / 100, row);
                var cDoc = row.DOC;
                if (cDoc != null)
                {
                    var generationContext = new GeneratorContext(cctsR, cDoc.TargetNamespace, cDoc.BaseUrn,
                                                             cDoc.TargetNamespacePrefix, false, true,
                                                             this.destinationFolderTextBox.Text, cDoc.BIV.DocL);
                    XSDGenerator.GenerateSchemas(generationContext);
                }
                i++;
                //set status
                row.status = "Finished";
                GenerateBackgroundWorker.ReportProgress((i * 100 / rows.Count() * 100) / 100, row);
            }
        }

        private void GenerateBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                this.StatusLabel.Text = "Error!";
                throw e.Error;
            }
            else if (e.Cancelled)
            {
                this.StatusLabel.Text = "Cancelled!";
            }
            else
            {
                //set status
                this.StatusLabel.Text = "Finished!";
            }
            this.EnableDisable();
            //set cursor back to normal
            Cursor.Current = Cursors.Default;
        }

        private void GenerateBackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            var row = (cBIV)e.UserState;
            this.messagesListView.RefreshObject(row);
            // set overall progress
            this.StatusLabel.Text = $"Generating .. {e.ProgressPercentage} %";
        }

        private void messagesListView_SubItemChecking(object sender, BrightIdeasSoftware.SubItemCheckingEventArgs e)
        {
            this.EnableDisable();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            if (this.GenerateBackgroundWorker.IsBusy)
                this.GenerateBackgroundWorker.CancelAsync();
        }
    }
}
