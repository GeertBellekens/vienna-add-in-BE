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
using System.Windows.Input;
using CctsRepository;
using EA;
using VIENNAAddIn.menu;
using VIENNAAddIn.Settings;
using VIENNAAddIn.upcc3.Wizards.util;

namespace VIENNAAddIn.upcc3.Wizards.dev.ui
{
    /// <summary>
    /// This class implements the functionality for the UPCC Wizard Form which
    /// allows to create default UPCC models. Basically, the wizard allows the
    /// user to create (a) empty default models which contain only the packages 
    /// according to UPCC and (b) default models which also import the standard
    /// CC libraries. The wizard utilizes the class "ModelCreator" for creating
    /// the default structure. 
    /// </summary>
    public partial class UpccModelCreator
    {
        #region Local Class Fields

// ReSharper disable UnaccessedField.Local
        private bool importStandardLibraries;
// ReSharper restore UnaccessedField.Local
        private string modelName = "";
        private string primLibraryName = "";
        private string enumLibraryName = "";
        private string cdtLibraryName = "";
        private string ccLibraryName = "";
        private string bdtLibraryName = "";
        private string bieLibraryName = "";
        private string docLibraryName = "";
        private string cclPath = "";
        private FileBasedVersionHandler versionHandler; 
        private readonly Repository repository;
        private readonly ICctsRepository cctsRepository;

        private const string WizardTitle = "UPCC Model Generator";
        private const string StatusMessage = "Creating a default model named \"{0}\" completed successfully.";

        #endregion

        /// <summary>
        /// The constructor of the wizard initializes the form. Furthermore, 
        /// the constructor is private since he is called by the method "ShowForm" 
        /// which is part of the same class. 
        /// </summary>
        public UpccModelCreator(AddInContext context)
        {
            InitializeComponent();

            repository = context.EARepository;
            cctsRepository = context.CctsRepository;

            InitializeWindow();
        }

        ///<summary>
        /// The method is called from the menu manager of the VIENNAAddIn and
        /// creates creates as well as launches a new instance of the wizard. 
        ///</summary>
        ///<param name="context">
        /// Specifies the context of the VIENNA Add-In containing items such as
        /// the current CC Repository. 
        ///</param>
        public static void ShowForm(AddInContext context)
        {
            new UpccModelCreator(context).ShowDialog();
        }

        #region Convenience Methods

        private void InitializeWindow()
        {
            CheckAllLibraries(true);
            checkboxDefaultValues.IsChecked = true;
            SetEnabledForAllLibraryTextFields(false);
            SetModelDefaultName();
            SetLibraryDefaultNames();
        }

        private void OnStatusChanged(string statusMessage)
        {
            rtxtStatus.Text = statusMessage + "\n" + rtxtStatus.Text;
        }

        private void CheckCCLibraries(bool newState)
        {
            checkboxPRIML.IsChecked = newState;
            checkboxENUML.IsChecked = newState;
            checkboxCDTL.IsChecked = newState;
            checkboxCCL.IsChecked = newState;
        }

        private void CheckBIELibraries(bool newState)
        {
            checkboxBDTL.IsChecked = newState;
            checkboxBIEL.IsChecked = newState;
            checkboxDOCL.IsChecked = newState;
        }

        private void CheckAllLibraries(bool newState)
        {
            CheckCCLibraries(newState);
            CheckBIELibraries(newState);
        }

        private void SetEnabledForCCLibraryTextFields(bool newState)
        {
            textboxPRIMLName.IsEnabled = newState;
            textboxENUMLName.IsEnabled = newState;
            textboxCDTLName.IsEnabled = newState;
            textboxCCLName.IsEnabled = newState;
        }

        private void SetEnabledForCCLibraryCheckBoxes(bool newState)
        {
            checkboxPRIML.IsEnabled = newState;
            checkboxENUML.IsEnabled = newState;
            checkboxCDTL.IsEnabled = newState;
            checkboxCCL.IsEnabled = newState;
        }

        private void SetEnabledForBIELibraryCheckBoxes(bool newState)
        {
            checkboxBDTL.IsEnabled = newState;
            checkboxBIEL.IsEnabled = newState;
            checkboxDOCL.IsEnabled = newState;
        }

        private void SetEnabledForBIELibraryTextFields(bool newState)
        {
            textboxBDTLName.IsEnabled = newState;
            textboxBIELName.IsEnabled = newState;
            textboxDOCLName.IsEnabled = newState;
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

        private void UpdateFormState()
        {
            if (checkboxDefaultValues.IsChecked == true)
            {
                if (checkboxImportStandardLibraries.IsChecked == true)
                {
                    SetEnabledForAllLibraryTextFields(false);

                    SetEnabledForCCLibraryCheckBoxes(false);
                    SetEnabledForBIELibraryCheckBoxes(true);

                    SetEnabledForStandardCCControls(true);

                    SetLibraryDefaultNames();

                    CheckCCLibraries(true);
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
                if (checkboxImportStandardLibraries.IsChecked == true)
                {
                    SetEnabledForCCLibraryTextFields(false);
                    SetEnabledForBIELibraryTextFields(true);

                    SetEnabledForCCLibraryCheckBoxes(false);
                    SetEnabledForBIELibraryCheckBoxes(true);

                    SetEnabledForStandardCCControls(true);

                    SetCCLibraryDefaultNames();

                    CheckCCLibraries(true);
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
            cbxMajor.IsEnabled = newState;
            cbxMinor.IsEnabled = newState;
            txtComment.IsEnabled = newState;
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
                    versionHandler.RetrieveAvailableVersions();
                }
                catch (WebException)
                {
                    cclPath = AddInSettings.HomeDirectory + "upcc3\\resources\\ccl\\";
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
        }

        private void GatherUserInput()
        {
            modelName = textboxModelName.Text;

            importStandardLibraries = (bool) checkboxImportStandardLibraries.IsChecked ? true : false;

            primLibraryName = (bool) checkboxPRIML.IsChecked ? textboxPRIMLName.Text : "";
            enumLibraryName = (bool) checkboxENUML.IsChecked ? textboxENUMLName.Text : "";
            cdtLibraryName = (bool) checkboxCDTL.IsChecked ? textboxCDTLName.Text : "";
            ccLibraryName = (bool) checkboxCCL.IsChecked ? textboxCCLName.Text : "";
            bdtLibraryName = (bool) checkboxBDTL.IsChecked ? textboxBDTLName.Text : "";
            bieLibraryName = (bool) checkboxBIEL.IsChecked ? textboxBIELName.Text : "";
            docLibraryName = (bool) checkboxDOCL.IsChecked ? textboxDOCLName.Text : "";
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
                txtComment.Text = versionHandler.GetComment(cbxMajor.SelectedItem.ToString(), cbxMinor.SelectedItem.ToString());
        }

        private void CheckIfInputIsValid()
        {
            GatherUserInput();

            if ((modelName.Equals("")) ||
                ((bool) checkboxPRIML.IsChecked && primLibraryName.Equals("")) ||
                ((bool) checkboxENUML.IsChecked && enumLibraryName.Equals("")) ||
                ((bool) checkboxCDTL.IsChecked && cdtLibraryName.Equals("")) ||
                ((bool) checkboxCCL.IsChecked && ccLibraryName.Equals("")) ||
                ((bool) checkboxBDTL.IsChecked && bdtLibraryName.Equals("")) ||
                ((bool) checkboxBIEL.IsChecked && bieLibraryName.Equals("")) ||
                ((bool) checkboxDOCL.IsChecked && docLibraryName.Equals("")))
            {
                buttonGenerate.IsEnabled = false;
            }
            else
            {
                buttonGenerate.IsEnabled = true;
            }
        }

        #endregion

        #region Event Handler Methods

        private void textboxModelName_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            CheckIfInputIsValid();
        }

        private void textboxPRIMLName_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            CheckIfInputIsValid();
        }

        private void textboxENUMLName_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            CheckIfInputIsValid();
        }

        private void textboxCDTLName_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            CheckIfInputIsValid();
        }

        private void textboxCCLName_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            CheckIfInputIsValid();
        }

        private void textboxBDTLName_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            CheckIfInputIsValid();
        }

        private void textboxBIELName_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            CheckIfInputIsValid();
        }

        private void textboxDOCLName_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            CheckIfInputIsValid();
        }

        private void checkboxDefaultValues_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            UpdateFormState();
        }

        private void checkboxPRIML_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            CheckIfInputIsValid();
        }

        private void checkboxENUML_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            CheckIfInputIsValid();
        }

        private void checkboxCDTL_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            CheckIfInputIsValid();
        }

        private void checkboxCCL_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            CheckIfInputIsValid();
        }

        private void checkboxBDTL_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            CheckIfInputIsValid();
        }

        private void checkboxBIEL_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            CheckIfInputIsValid();
        }

        private void checkboxDOCL_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            CheckIfInputIsValid();
        }

        private void checkboxImportStandardLibraries_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            if (checkboxImportStandardLibraries.IsChecked == true)
            {
                LoadStandardLibraryVersions();
            }
            UpdateFormState();
        }

        private void cbxMinor_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            PopulateTxtComment();
        }

        private void cbxMajor_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            PopulateCbxMinor();
        }

        private void buttonGenerate_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            buttonClose.IsEnabled = false;
            buttonGenerate.IsEnabled = false;

            GatherUserInput();

            ModelCreator creator = new ModelCreator(repository, cctsRepository);
            creator.StatusChanged += OnStatusChanged;

            if (checkboxImportStandardLibraries.IsChecked == true)
            {
                ResourceDescriptor resourceDescriptor = new ResourceDescriptor(cclPath, cbxMajor.SelectedItem.ToString(), cbxMinor.SelectedItem.ToString());
                creator.CreateUpccModel(modelName, bdtLibraryName, bieLibraryName, docLibraryName, resourceDescriptor);
            }
            else
            {
                creator.CreateUpccModel(modelName, primLibraryName, enumLibraryName, cdtLibraryName,
                                        ccLibraryName, bdtLibraryName, bieLibraryName, docLibraryName);
            }

            MessageBox.Show(string.Format(StatusMessage, modelName), WizardTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);

            buttonClose.IsEnabled = true;
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
        }

        private void buttonClose_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Close();
        }

        private void checkboxPRIML_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            CheckIfInputIsValid();
        }

        private void checkboxENUML_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            CheckIfInputIsValid();
        }

        private void checkboxCDTL_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            CheckIfInputIsValid();
        }

        private void checkboxCCL_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            CheckIfInputIsValid();
        }

        private void checkboxBDTL_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            CheckIfInputIsValid();
        }

        private void checkboxBIEL_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            CheckIfInputIsValid();
        }

        private void checkboxDOCL_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            CheckIfInputIsValid();
        }

        private void checkboxImportStandardLibraries_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            if (checkboxImportStandardLibraries.IsChecked == true)
            {
                LoadStandardLibraryVersions();
            }

            UpdateFormState();
        }

        private void checkboxDefaultValues_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            UpdateFormState();
        }

// ReSharper disable InconsistentNaming
        private void checkboxDefaultValues_CheckedChanged(object sender, EventArgs e)
// ReSharper restore InconsistentNaming
        {
            UpdateFormState();
        }

// ReSharper disable InconsistentNaming
        private void checkboxImportStandardLibraries_CheckedChanged(object sender, EventArgs e)
// ReSharper restore InconsistentNaming
        {
            if (checkboxImportStandardLibraries.IsChecked == true)
            {
                LoadStandardLibraryVersions();
            }

            UpdateFormState();
        }

        #endregion Event Handler Methods
    }
}