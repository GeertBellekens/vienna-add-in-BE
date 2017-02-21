// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************

using System;
using System.Net;
using System.Windows.Forms;
using CctsRepository;
using EA;
using VIENNAAddIn.menu;
using VIENNAAddIn.Settings;
using VIENNAAddIn.upcc3.Wizards.util;

namespace VIENNAAddIn.upcc3.Wizards
{
    /// <summary>
    /// This class implements the functionality for the UPCC Wizard Form which
    /// allows to create default UPCC models. Basically, the wizard allows the
    /// user to create (a) empty default models which contain only the packages 
    /// according to UPCC and (b) default models which also import the standard
    /// CC libraries. The wizard utilizes the class "ModelCreator" for creating
    /// the default structure. 
    /// </summary>
    public partial class UpccModelWizardForm : Form
    {
        #region Local Class Fields

        private string modelName = "";
        private string primLibraryName = "";
        private string enumLibraryName = "";
        private string cdtLibraryName = "";
        private string ccLibraryName = "";
        private string bdtLibraryName = "";
        private string bieLibraryName = "";
        private string docLibraryName = "";
// ReSharper disable InconsistentNaming
        private const string wizardTitle = "UPCC Model Wizard";
        private const string statusMessage = "Creating a default model named \"{0}\" completed successfully.";
// ReSharper restore InconsistentNaming

        private string cclPath;
        private bool importStandardLibraries;
        
        private readonly Repository repository;
        private readonly ICctsRepository cctsRepository;
        private FileBasedVersionHandler versionHandler;

        #endregion

        ///<summary>
        /// The constructor of the wizard initializes the form. Furthermore, 
        /// the constructor is private since he is called by the method "ShowForm" 
        /// which is part of the same class. 
        ///</summary>
        ///<param name="eaRepository">
        /// Specifies the EA Repository that the wizard operates on. 
        ///</param>
        private UpccModelWizardForm(Repository eaRepository, ICctsRepository cctsRepository)
        {
            InitializeComponent();

            repository = eaRepository;
            this.cctsRepository = cctsRepository;
        }

        ///<summary>
        /// The method is called from the menu manager of the VIENNAAddIn and
        /// creates creates as well as launches a new instance of the wizard. 
        ///</summary>
        ///<param name="context">
        /// TODO: what exactly is the context parameter for?
        ///</param>
        public static void ShowForm(AddInContext context)
        {
            new UpccModelWizardForm(context.EARepository, context.CctsRepository).ShowDialog();
        }

        #region Convenience Methods

        private void CheckCCLibraries(CheckState newState)
        {
            checkboxPRIML.CheckState = newState;
            checkboxENUML.CheckState = newState;
            checkboxCDTL.CheckState = newState;
            checkboxCCL.CheckState = newState;
        }

        private void CheckBIELibraries(CheckState newState)
        {
            checkboxBDTL.CheckState = newState;
            checkboxBIEL.CheckState = newState;
            checkboxDOCL.CheckState = newState;
        }

        private void CheckAllLibraries(CheckState newState)
        {
            CheckCCLibraries(newState);
            CheckBIELibraries(newState);
        }

        private void SetEnabledForCCLibraryTextFields(bool newState)
        {
            textboxPRIMLName.Enabled = newState;
            textboxENUMLName.Enabled = newState;
            textboxCDTLName.Enabled = newState;
            textboxCCLName.Enabled = newState;
        }

        private void SetEnabledForCCLibraryCheckBoxes(bool newState)
        {
            checkboxPRIML.Enabled = newState;
            checkboxENUML.Enabled = newState;
            checkboxCDTL.Enabled = newState;
            checkboxCCL.Enabled = newState;
        }

        private void SetEnabledForBIELibraryCheckBoxes(bool newState)
        {
            checkboxBDTL.Enabled = newState;
            checkboxBIEL.Enabled = newState;
            checkboxDOCL.Enabled = newState;
        }

        private void SetEnabledForBIELibraryTextFields(bool newState)
        {
            textboxBDTLName.Enabled = newState;
            textboxBIELName.Enabled = newState;
            textboxDOCLName.Enabled = newState;
        }

        private void SetEnabledForAllLibraryTextFields(bool newState)
        {
            SetEnabledForCCLibraryTextFields(newState);
            SetEnabledForBIELibraryTextFields(newState);
        }

        private void SetModelDefaultName()
        {
            textboxModelName.Text = "Default Model";
        }

        private void SetCCLibraryDefaultNames()
        {
            textboxPRIMLName.Text = "PRIMLibrary";
            textboxENUMLName.Text = "ENUMLibrary";
            textboxCDTLName.Text = "CDTLibrary";
            textboxCCLName.Text = "CCLibrary";            
        }

        private void SetBIELibraryDefaultNames()
        {
            textboxBDTLName.Text = "BDTLibrary";
            textboxBIELName.Text = "BIELibrary";
            textboxDOCLName.Text = "DOCLibrary";
        }

        private void SetLibraryDefaultNames()
        {
            SetCCLibraryDefaultNames();
            SetBIELibraryDefaultNames();
        }

        private void GatherUserInput()
        {
            modelName = textboxModelName.Text;

            importStandardLibraries = checkboxImportStandardLibraries.Checked ? true : false;

            primLibraryName = checkboxPRIML.Checked ? textboxPRIMLName.Text : "";
            enumLibraryName = checkboxENUML.Checked ? textboxENUMLName.Text : "";
            cdtLibraryName = checkboxCDTL.Checked ? textboxCDTLName.Text : "";
            ccLibraryName = checkboxCCL.Checked ? textboxCCLName.Text : "";
            bdtLibraryName = checkboxBDTL.Checked ? textboxBDTLName.Text : "";
            bieLibraryName = checkboxBIEL.Checked ? textboxBIELName.Text : "";
            docLibraryName = checkboxDOCL.Checked ? textboxDOCLName.Text : "";
        }

        private void CheckIfInputIsValid()
        {
            GatherUserInput();

            if ((modelName.Equals("")) ||
                (checkboxPRIML.Checked && primLibraryName.Equals("")) ||
                (checkboxENUML.Checked && enumLibraryName.Equals("")) ||
                (checkboxCDTL.Checked && cdtLibraryName.Equals("")) ||
                (checkboxCCL.Checked && ccLibraryName.Equals("")) ||
                (checkboxBDTL.Checked && bdtLibraryName.Equals("")) ||
                (checkboxBIEL.Checked && bieLibraryName.Equals("")) ||
                (checkboxDOCL.Checked && docLibraryName.Equals("")))
            {
                buttonGenerate.Enabled = false;
            }
            else
            {
                buttonGenerate.Enabled = true;
            }
        }

        #endregion

        #region Event Handler Methods

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void OnStatusChanged(string statusMessage)
        {
            rtxtStatus.Text = statusMessage + "\n" + rtxtStatus.Text;
        }

        private void buttonGenerate_Click(object sender, EventArgs e)
        {
            buttonGenerate.Enabled = false;

            GatherUserInput();

            ModelCreator creator = new ModelCreator(repository, cctsRepository);
            creator.StatusChanged += OnStatusChanged;

            if (checkboxImportStandardLibraries.CheckState == CheckState.Checked)
            {
                ResourceDescriptor resourceDescriptor = new ResourceDescriptor(cclPath, cbxMajor.SelectedItem.ToString(), cbxMinor.SelectedItem.ToString());                
                creator.CreateUpccModel(modelName, bdtLibraryName, bieLibraryName, docLibraryName, resourceDescriptor);
            }
            else
            {
                creator.CreateUpccModel(modelName, primLibraryName, enumLibraryName, cdtLibraryName,
                                        ccLibraryName, bdtLibraryName, bieLibraryName, docLibraryName);
            }

            MessageBox.Show(string.Format(statusMessage, modelName), wizardTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);

            buttonGenerate.Enabled = true;
        }

        private void checkboxDefaultValues_CheckedChanged(object sender, EventArgs e)
        {
            UpdateFormState();
        }

        private void checkboxImportStandardLibraries_CheckedChanged(object sender, EventArgs e)
        {
            if (checkboxImportStandardLibraries.CheckState == CheckState.Checked)
            {
                LoadStandardLibraryVersions();
            }
            UpdateFormState();
        }

        private void UpdateFormState()
        {
            if (checkboxDefaultValues.CheckState == CheckState.Checked)
            {
                if (checkboxImportStandardLibraries.CheckState == CheckState.Checked)
                {
                    SetEnabledForAllLibraryTextFields(false);

                    SetEnabledForCCLibraryCheckBoxes(false);
                    SetEnabledForBIELibraryCheckBoxes(true);

                    SetEnabledForStandardCCControls(true);

                    SetLibraryDefaultNames();

                    CheckCCLibraries(CheckState.Checked);
                }
                else
                {
                    SetEnabledForAllLibraryTextFields(false);

                    SetEnabledForAllLibraryCheckBoxes(true);

                    SetEnabledForStandardCCControls(false);

                    SetLibraryDefaultNames();
                }
            }
            else
            {
                if (checkboxImportStandardLibraries.CheckState == CheckState.Checked)
                {
                    SetEnabledForCCLibraryTextFields(false);
                    SetEnabledForBIELibraryTextFields(true);

                    SetEnabledForCCLibraryCheckBoxes(false);
                    SetEnabledForBIELibraryCheckBoxes(true);

                    SetEnabledForStandardCCControls(true);

                    SetCCLibraryDefaultNames();

                    CheckCCLibraries(CheckState.Checked);
                }
                else
                {
                    SetEnabledForAllLibraryTextFields(true);

                    SetEnabledForAllLibraryCheckBoxes(true);

                    SetEnabledForStandardCCControls(false);
                }
            }
        }

        private void SetEnabledForStandardCCControls(bool newState)
        {
            cbxMajor.Enabled = newState;
            cbxMinor.Enabled = newState;
            txtComment.Enabled = newState;
        }

        private void SetEnabledForAllLibraryCheckBoxes(bool newState)
        {
            SetEnabledForCCLibraryCheckBoxes(newState);
            SetEnabledForBIELibraryCheckBoxes(newState);
        }

        private void LoadStandardLibraryVersions()
        {
            if (versionHandler == null)
            {
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
            txtComment.Text = versionHandler.GetComment(cbxMajor.SelectedItem.ToString(), cbxMinor.SelectedItem.ToString());
        }

        private void UPCCModelWizardForm_Load(object sender, EventArgs e)
        {
            cbxMajor.DropDownStyle = ComboBoxStyle.DropDownList;
            cbxMinor.DropDownStyle = ComboBoxStyle.DropDownList;
            
            CheckAllLibraries(CheckState.Checked);
            checkboxDefaultValues.CheckState = CheckState.Checked;            
            SetEnabledForAllLibraryTextFields(false);
            SetModelDefaultName();
            SetLibraryDefaultNames();
        }

        private void textboxModelName_TextChanged(object sender, EventArgs e)
        {
            CheckIfInputIsValid();
        }

        private void textboxPRIMLName_TextChanged(object sender, EventArgs e)
        {
            CheckIfInputIsValid();
        }

        private void textboxENUMLName_TextChanged(object sender, EventArgs e)
        {
            CheckIfInputIsValid();
        }

        private void textboxCDTLName_TextChanged(object sender, EventArgs e)
        {
            CheckIfInputIsValid();
        }

        private void textboxCCLName_TextChanged(object sender, EventArgs e)
        {
            CheckIfInputIsValid();
        }

        private void textboxBDTLName_TextChanged(object sender, EventArgs e)
        {
            CheckIfInputIsValid();
        }

        private void textboxBIELName_TextChanged(object sender, EventArgs e)
        {
            CheckIfInputIsValid();
        }

        private void textboxDOCLName_TextChanged(object sender, EventArgs e)
        {
            CheckIfInputIsValid();
        }

        private void checkboxPRIML_CheckedChanged(object sender, EventArgs e)
        {
            CheckIfInputIsValid();
        }

        private void checkboxENUML_CheckedChanged(object sender, EventArgs e)
        {
            CheckIfInputIsValid();
        }

        private void checkboxCDTL_CheckedChanged(object sender, EventArgs e)
        {
            CheckIfInputIsValid();
        }

        private void checkboxCCL_CheckedChanged(object sender, EventArgs e)
        {
            CheckIfInputIsValid();
        }

        private void checkboxBDTL_CheckedChanged(object sender, EventArgs e)
        {
            CheckIfInputIsValid();
        }

        private void checkboxBIEL_CheckedChanged(object sender, EventArgs e)
        {
            CheckIfInputIsValid();
        }

        private void checkboxDOCL_CheckedChanged(object sender, EventArgs e)
        {
            CheckIfInputIsValid();
        }

        private void cbxMajor_SelectionChangeCommitted(object sender, EventArgs e)
        {
            PopulateCbxMinor();
        }

        private void cbxMinor_SelectionChangeCommitted(object sender, EventArgs e)
        {
            PopulateTxtComment();
        }

        #endregion Event Handler Methods
    }
}