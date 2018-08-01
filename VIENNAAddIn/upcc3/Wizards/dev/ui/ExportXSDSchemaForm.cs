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
            this.destinationFolderTextBox.Text = settings.lastUsedExportPath;
            InitializeComponent();
            cctsR = cctsRepository;
            this.init(selectedPackage);
        }
        private void init(EA.Package selectedPackage)
        {
            this.selectedPackage = selectedPackage;
            try
            {
                if (cache == null) cache = new Cache();
                cache.LoadBIVs(cctsR, selectedPackage);
            }
            catch (CacheException ce)
            {
                MessageBox.Show(ce.Message, "VIENNA Add-In Information", MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
            }
            this.selectedPackageTextBox.Text = selectedPackage.Name;
            MirrorDOCsToUI();
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
                                           && this.messagesListView.CheckedObjects.Count > 0;
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

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void generateButton_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            //save the last used folder
            this.settings.save();
            //actually generate the xsd's
            var generationContexts = new List<GeneratorContext>();
            foreach (var cBiv in this.messagesListView.Objects.Cast<cBIV>())
            {
                var cDoc = cBiv.DOC;
                if (cDoc != null)
                {
                    var generationContext = new GeneratorContext(cctsR, cDoc.TargetNamespace, cDoc.BaseUrn,
                                                             cDoc.TargetNamespacePrefix, false, true,
                                                             this.destinationFolderTextBox.Text, cDoc.BIV.DocL);
                    XSDGenerator.GenerateSchemas(generationContext);
                    //change the color of the item //TODO: find the correct item
                    this.messagesListView.Items[0].BackColor = Color.Green;
                }
            }
            Cursor.Current = Cursors.Default;
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
    }
}
