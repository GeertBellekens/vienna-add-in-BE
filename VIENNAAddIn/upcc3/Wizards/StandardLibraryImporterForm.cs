using System;
using System.Net;
using System.Windows.Forms;
using EA;
using VIENNAAddIn.menu;
using VIENNAAddIn.Settings;
using VIENNAAddIn.upcc3.Wizards.util;

namespace VIENNAAddIn.upcc3.Wizards
{
    public partial class StandardLibraryImporterForm : Form
    {
        private readonly Repository eaRepository;
        FileBasedVersionHandler versionHandler;
        private string cclPath;

        public static void ShowForm(AddInContext context)
        {
            new StandardLibraryImporterForm(context.EARepository).ShowDialog();
        }

        private StandardLibraryImporterForm(Repository repository)
        {
            eaRepository = repository;
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void StandardLibraryImporterForm_Load(object sender, EventArgs e)
        {
            cbxMajor.DropDownStyle = ComboBoxStyle.DropDownList;
            cbxMinor.DropDownStyle = ComboBoxStyle.DropDownList;

            try
            {
                cclPath = "http://www.umm-dev.org/xmi/";
                versionHandler = new FileBasedVersionHandler(new RemoteVersionsFile(cclPath + "ccl_versions.txt"));
            }
            catch (WebException)
            {
                cclPath = AddInSettings.HomeDirectory + "upcc3\\resources\\ccl\\";
                versionHandler = new FileBasedVersionHandler(new LocalVersionsFile(cclPath + "ccl_verions.txt"));
            }

                versionHandler.RetrieveAvailableVersions();

                foreach (string majorVersion in versionHandler.GetMajorVersions())
                {
                    cbxMajor.Items.Add(majorVersion);
                }

                cbxMajor.SelectedIndex = cbxMajor.Items.Count - 1;
                
                PopulateCbxMinor();

        }

        private void cbxMajor_SelectionChangeCommitted(object sender, EventArgs e)
        {
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

        private void cbxMinor_SelectionChangeCommitted(object sender, EventArgs e)
        {
            PopulateTxtComment();
        }

        private void PopulateTxtComment()
        {
            txtComment.Text = versionHandler.GetComment(cbxMajor.SelectedItem.ToString(), cbxMinor.SelectedItem.ToString());
        }

        private void btnImport_Click(object sender, EventArgs e)
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

            if (dialogResult == DialogResult.Yes)
            {
                Cursor.Current = Cursors.WaitCursor;
                btnClose.Enabled = false;
                btnImport.Enabled = false;

                string bLibraryGuid = eaRepository.GetTreeSelectedPackage().Element.ElementGUID;
                Package bLibrary = eaRepository.GetPackageByGuid(bLibraryGuid);

                ResourceDescriptor resourceDescriptor = new ResourceDescriptor(cclPath, cbxMajor.SelectedItem.ToString(), cbxMinor.SelectedItem.ToString());                

                LibraryImporter importer = new LibraryImporter(eaRepository, resourceDescriptor);
                importer.StatusChanged += OnStatusChanged;
                importer.ImportStandardCcLibraries(bLibrary);

                btnImport.Enabled = true;
                btnClose.Enabled = true;
                Cursor.Current = Cursors.Default;
            }
        }

        private void OnStatusChanged(string statusMessage)
        {
            rtxtStatus.Text = statusMessage + "\n" + rtxtStatus.Text;
        }
    }
}
