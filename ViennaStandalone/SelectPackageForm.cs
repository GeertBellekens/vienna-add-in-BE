using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VIENNAAddIn.menu;
using VIENNAAddIn.upcc3.Wizards.dev.ui;

namespace ViennaStandalone
{
    public partial class SelectPackageForm : Form
    {
        public SelectPackageForm()
        {
            InitializeComponent();


        }
        private void enableDisable()
        {
            bool modelConnected = this.repo != null;
            this.browsePackageButton.Enabled = modelConnected;
            this.GenerateButton.Enabled = modelConnected && this.browsedPackage != null; ;
        }
        private EA.Package browsedPackage { get; set; }
        private EA.Repository _repo;
        private EA.Repository repo
        {
            get
            {
                if (_repo == null)
                {
                    _repo = this.GetRepository();
                }
                return _repo;
            }
        }
        private void browsePackageButton_Click(object sender, EventArgs e)
        {
            this.browsedPackage = null;
            string includeString = "IncludedTypes=Package;";
            var selectedPackage = repo.GetTreeSelectedPackage();
            if (selectedPackage != null) includeString += "Selection=" + selectedPackage.PackageGUID;
            var packageElementID = this.repo.InvokeConstructPicker(includeString);
            if (packageElementID > 0 )
            {
                //get element
                var packageElement = repo.GetElementByID(packageElementID);
                this.browsedPackage = repo.GetPackageByGuid(packageElement?.ElementGUID);
            }
            this.importPackageTextBox.Text = browsedPackage?.Name;
            //make sure the package is also selected in the project browser
            if (this.browsedPackage != null ) repo.ShowInProjectView(this.browsedPackage);
            enableDisable();
        }

        private void GenerateButton_Click(object sender, EventArgs e)
        {
            var addinContext = new AddInContext(this.repo, "TreeView");
            ExporterForm.ShowForm(addinContext); 
        }
        private EA.Repository GetRepository()
        {
            try
            {
                //get the EA model
                object obj = Marshal.GetActiveObject("EA.App");
                EA.App eaApp = obj as EA.App;
                return eaApp.Repository;
            }
            catch (Exception)
            {
                //do nothing
                return null;
            }
        }
    }
}
