// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using CctsRepository;
using CctsRepository.BieLibrary;
using EA;
using VIENNAAddIn.menu;
using VIENNAAddIn.upcc3.Wizards.dev.temporarymodel.abiemodel;
using VIENNAAddIn.upcc3.Wizards.dev.temporarymodel.abiemodel.exceptions;
using VIENNAAddIn.upcc3.Wizards.dev.util;

namespace VIENNAAddIn.upcc3.Wizards.dev.ui
{
    public partial class UmlClassCustomizer
    {
        private readonly IAbie abieToBeUpdated;
        private readonly int DiagramId;
        private readonly string presetAcc;
        private readonly string presetBieLib;
        private readonly string presetCcLib;
        private readonly Repository Repository;
        private string bbieNameBeforeRename;
        private string bdtNameBeforeRename;

        /// <summary>
        /// This is the default ABIE Editor constructor. It should be used anytime an ABIE is build up from scratch.
        /// </summary>
        /// <param name="cctsRepository">An implementation of the ICctsRepository interface containing the target libraries / Core Components</param>
        public UmlClassCustomizer(ICctsRepository cctsRepository)
        {
            //backgroundWorker = new BackgroundWorker
            //                       {
            //                           WorkerReportsProgress = false,
            //                           WorkerSupportsCancellation = false
            //                       };

            InitializeComponent();
            AbieEditorMode = EditorModes.AbieEditorModes.Create;
            abieToBeUpdated = null;

            Model = new TemporaryAbieModel(cctsRepository);
            DataContext = this;

            SelectDefaultLibraries();

            UpdateFormState();
        }

        /// <summary>
        /// This constructor should only be used when an ACC is dragged into an BIE library.
        /// </summary>
        /// <param name="cclibName">The name of the CC library containing the selected ACC</param>
        /// <param name="accName">The name of the selected ACC</param>
        /// <param name="bieLibName">The name of the target BIE library (drag target)</param>
        /// <param name="diagramID">The ID of the target BIE library diagram (needed for EA workaround)</param>
        /// <param name="repository">The EA repository which contains all previous elements</param>
        public UmlClassCustomizer(string cclibName, string accName, string bieLibName, int diagramID, Repository repository)
        {
            InitializeComponent();
            AbieEditorMode = EditorModes.AbieEditorModes.CreateFromAcc;
            abieToBeUpdated = null;
            presetCcLib = cclibName;
            presetAcc = accName;
            presetBieLib = bieLibName;
            Model = new TemporaryAbieModel(CctsRepositoryFactory.CreateCctsRepository(repository));
            Repository = repository;
            DiagramId = diagramID;
            DataContext = this;

            UpdateFormState();
        }

        /// <summary>
        /// The ABIE Editor in update mode is used to edit an existing ABIE.
        /// Please note that this will load all data available from the target ABIE (including associations) which could lead to timeouts!
        /// </summary>
        /// <param name="cctsRepository">An implementation of the ICctsRepository interface containing the target libraries / Core Components</param>
        /// <param name="idOfAbieToBeUpdated">The element ID of the ABIE to edit.</param>
        public UmlClassCustomizer(ICctsRepository cctsRepository, int idOfAbieToBeUpdated)
        {
            //backgroundWorker = new BackgroundWorker
            //{
            //    WorkerReportsProgress = false,
            //    WorkerSupportsCancellation = false
            //};

            InitializeComponent();
            AbieEditorMode = EditorModes.AbieEditorModes.Update;

            abieToBeUpdated = cctsRepository.GetAbieById(idOfAbieToBeUpdated);

            buttonCreateOrUpdate.Content = "_Update ABIE";

            Model = new TemporaryAbieModel(cctsRepository, abieToBeUpdated);

            DataContext = this;

            SelectDefaultLibraries();

            UpdateFormState();
        }

        public TemporaryAbieModel Model { get; set; }
        private EditorModes.AbieEditorModes AbieEditorMode { get; set; }

        public static void ShowCreateDialog(AddInContext context)
        {
            new UmlClassCustomizer(context.CctsRepository).ShowDialog();
        }

        public static void ShowUpdateDialog(AddInContext context)
        {
            new UmlClassCustomizer(context.CctsRepository, ((Element) context.SelectedItem).ElementID).ShowDialog();
        }

        private void comboboxAccs_Loaded(object sender, RoutedEventArgs e)
        {
            if (AbieEditorMode == EditorModes.AbieEditorModes.Update)
            {
                SelectFirstAcc();
            }
            if (AbieEditorMode == EditorModes.AbieEditorModes.CreateFromAcc)
            {
                comboboxAccs.SelectedIndex = comboboxAccs.Items.IndexOf(presetAcc);
                //Model.SetSelectedCandidateAcc(presetAcc);
            }
        }

        private void comboboxCcLibraries_Loaded(object sender, RoutedEventArgs e)
        {
            if (AbieEditorMode == EditorModes.AbieEditorModes.CreateFromAcc)
            {
                comboboxCcLibraries.SelectedIndex = comboboxCcLibraries.Items.IndexOf(presetCcLib);
                //Model.SetSelectedCandidateCcLibrary(presetCcLib);
            }
        }

        private void comboboxBieLibraries_Loaded(object sender, RoutedEventArgs e)
        {
            if (AbieEditorMode == EditorModes.AbieEditorModes.CreateFromAcc)
            {
                comboboxBieLibraries.SelectedIndex = comboboxBieLibraries.Items.IndexOf(presetBieLib);
                //Model.SetSelectedCandidateBieLibrary(presetBieLib);
            }
        }

        private void comboboxBdtLibraries_Loaded(object sender, RoutedEventArgs e)
        {
            if (AbieEditorMode == EditorModes.AbieEditorModes.CreateFromAcc)
            {
                comboboxBdtLibraries.SelectedIndex = 0;
            }
        }

        #region Methods for handling Control Events

        // ------------------------------------------------------------------------------------
        // Event handler: ComboBox CC Libraries
        private void comboboxCcLibraries_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Model.SetSelectedCandidateCcLibrary(comboboxCcLibraries.SelectedItem.ToString());

            UpdateFormState();
        }

        // ------------------------------------------------------------------------------------
        // Event handler: ComboBox ACCs
        private void comboboxAccs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (AbieEditorMode == EditorModes.AbieEditorModes.Create)
            //{
            //    ShowShield(true);

            //    //Model.SetSelectedCandidateAcc(comboboxAccs.SelectedItem.ToString());
            //    Model.AbieName = comboboxAccs.SelectedItem.ToString();

            //    // STEP 1: Beim Background Work werden die Eventhandler registriert                 // TODO: document
            //    // STEP 2 - grafik einblenden
            //    // STEP 3 - thread starten,  fix codiert - loest den DoWork Event aus
            //    backgroundWorker.DoWork += backgroundworkerAcc_DoWork;
            //    backgroundWorker.RunWorkerCompleted += backgroundworkerAcc_RunWorkerCompleted;

            //    backgroundWorker.RunWorkerAsync(Model);
            //}

            //UpdateFormState();

            //SetSelectedItemForBccListBox();
            //SetSelectedItemForAbieListBox();

            //MessageBox.Show("selection changed von accs");


            // ---------------------------------------------------------------
            // ALTER CODE OHNE ZAHNRAD
            // ---------------------------------------------------------------
            Model.SetSelectedCandidateAcc(comboboxAccs.SelectedItem.ToString());

            if (AbieEditorMode == EditorModes.AbieEditorModes.Create ||
                AbieEditorMode == EditorModes.AbieEditorModes.CreateFromAcc)
            {
                // The value of Model.AbiePrefix was already set in the constructor of the 
                // TemporaryAbieModel. For more information refer to the constructor
                // of the TemporaryAbieModel.
                Model.AbieName = Model.AbiePrefix + "_" + comboboxAccs.SelectedItem;
            }

            UpdateFormState();

            SetSelectedItemForBccListBox();
            SetSelectedItemForAbieListBox();
        }

        //private static void backgroundworkerAcc_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    // STEP 5: den prozess starten, im e steht der input parameter
        //    // STEP 5a: dem e.Result das Ergebnis zuweisen
        //    // STEP 6: wenn completed, loest er RunWorkerCompleted aus

        //    TemporaryAbieModel tempAbieModel = (TemporaryAbieModel)e.Argument;

        //    tempAbieModel.SetSelectedCandidateAcc(tempAbieModel.AbieName); // TODO: der name sollte der vom ACC sein
        //    //tempAbieModel.SetSelectedCandidateAcc("Address");
        //    e.Result = tempAbieModel;
        //}

        //private void backgroundworkerAcc_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    // INFO: der RunWorkerCompleted laeuft nun wieder im Kontext des GUIs, also des aufrufenden threads

        //    // STEP 7: result verarbeiten
        //    // STEP 8: den DoWork Event wieder entfernen
        //    // STEP 9: wir sind fertig - grafik entfernen - gui einblenden
        //    // STEP 10: es geht normal weiter

        //    Model = (TemporaryAbieModel)e.Result;

        //    backgroundWorker.DoWork -= backgroundworkerAcc_DoWork;

        //    ShowShield(false);
        //}

        // ------------------------------------------------------------------------------------
        // Event handler: Checkbox BCCs
        private void checkboxBccs_Checked(object sender, RoutedEventArgs e)
        {
            string selectedItemText = ((CheckableItem) listboxBccs.SelectedItem).Text;

            Model.SetCheckedForAllCandidateBccs((bool) ((CheckBox) sender).IsChecked);

            listboxBccs.SelectedItem = GetSelectedCheckableItemforListbox(listboxBccs, selectedItemText);

            UpdateFormState();


            // CODE MIT ZAHNRAD
            // => von cp disabled
            // TODO: wenn bug gefixed - auch fuer bccs wieder enablen
            //Model.temporaryCheckstate = (bool) ((CheckBox) sender).IsChecked;
            //doWorkEventHandler = backgroundworkerBccs_DoWork;
            //backgroundWorker.DoWork += doWorkEventHandler;
            //backgroundWorker.RunWorkerCompleted += backgroundworkerBccs_RunWorkerCompleted;                       
            //ShowShield(true);
            //backgroundWorker.RunWorkerAsync(Model);
        }

        //private static void backgroundworkerBccs_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    var tempModel = (TemporaryAbieModel)e.Argument;
        //    tempModel.SetCheckedForAllCandidateBccs(tempModel.temporaryCheckstate);
        //    e.Result = tempModel;
        //}

        //private void backgroundworkerBccs_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    Model = (TemporaryAbieModel) e.Result;
        //    backgroundWorker.DoWork -= doWorkEventHandler;

        //    string selectedItemText = ((CheckableItem) listboxBccs.SelectedItem).Text;
        //    Model.SetCheckedForAllCandidateBccs(Model.temporaryCheckstate);

        //    SetSelectedItemForBccListBox();
        //    listboxBccs.SelectedItem = GetSelectedCheckableItemforListbox(listboxBccs, selectedItemText);

        //    UpdateFormState();

        //    ShowShield(false);
        //}

        // ------------------------------------------------------------------------------------
        // Event handler: ListBox BCCs
        private void listboxBccs_ItemCheckBoxChecked(object sender, RoutedEventArgs e)
        {
            var checkableItem = (CheckableItem) listboxBccs.SelectedItem;
            Model.SetSelectedAndCheckedCandidateBcc(checkableItem.Text, checkableItem.Checked);

            // The following code only keeps the UI in sync with the TemporaryAbieModel since 
            // clicking the CheckBox only triggers an update in the TemporaryAbieModel but does
            // not select the current item in the ListBox.
            listboxBccs.SelectedItem = GetSelectedCheckableItemforListbox(listboxBccs, (CheckBox) sender);

            SetSelectedItemForBbieListBox();

            UpdateFormState();
        }

        private void listboxBccs_ItemSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listboxBccs.SelectedItem != null)
            {
                var checkableItem = (CheckableItem) listboxBccs.SelectedItem;
                Model.SetSelectedAndCheckedCandidateBcc(checkableItem.Text, checkableItem.Checked);

                SetSelectedItemForBbieListBox();
            }
        }


        // ------------------------------------------------------------------------------------
        // Event handler: Button BBIE
        private void buttonAddBbie_Click(object sender, RoutedEventArgs e)
        {
            int selectedIndex = listboxBbies.SelectedIndex;

            Model.AddPotentialBbie();

            listboxBbies.SelectedIndex = selectedIndex;
        }

        // ------------------------------------------------------------------------------------
        // Event handler: ListBox BBIEs
        private void listboxBbies_ItemCheckBoxChecked(object sender, RoutedEventArgs e)
        {
            var checkableItem = (CheckableItem) listboxBbies.SelectedItem;
            Model.SetSelectedAndCheckedPotentialBbie(checkableItem.Text, checkableItem.Checked);

            // The following code only keeps the UI in sync with the TemporaryAbieModel since 
            // clicking the CheckBox only triggers an update in the TemporaryAbieModel but does
            // not select the current item in the ListBox.
            listboxBbies.SelectedItem = GetSelectedCheckableItemforListbox(listboxBbies, (CheckBox) sender);

            SetSelectedItemForBdtListBox();

            UpdateFormState();
        }

        private void listboxBbies_ItemSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listboxBbies.SelectedItem != null)
            {
                var checkableItem = (CheckableItem) listboxBbies.SelectedItem;

                Model.SetSelectedAndCheckedPotentialBbie(checkableItem.Text, checkableItem.Checked);

                SetSelectedItemForBdtListBox();
            }
        }

        private void listboxBbies_ItemTextBoxGotMouseCapture(object sender, MouseEventArgs e)
        {
            // The listbox of the BBIEs contains a set of CheckableItems. Each CheckableItem
            // consists of a CheckBox as well as a TextBox. However, in case the user clicks
            // on the text in the TextBox the GotMouseCapture event is triggered but the item
            // in the listbox is not selected causing the rename of a BBIE to fail. Therefore,
            // we need to use the following workaround which ensures that the item in the 
            // ListBox having the focus is also selected. 
            listboxBbies.SelectedItem = GetSelectedCheckableItemforListbox(listboxBbies, (TextBox) sender);

            var checkableItem = (CheckableItem) listboxBbies.SelectedItem;

            // We need to store the original name of the BBIE before it is changed as part of a
            // rename process. The reason for doing so is that in case the BBIE is renamed to 
            // match the name of an existing BBIE it is necessary to display the original name
            // of the BBIE. 
            bbieNameBeforeRename = checkableItem.Text;

            Model.SetSelectedAndCheckedPotentialBbie(checkableItem.Text, checkableItem.Checked);

            SetSelectedItemForBdtListBox();
        }


        private void listboxBbies_ItemTextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            var checkableItem = (CheckableItem) listboxBbies.SelectedItem;

            try
            {
                if (checkableItem.Text != bbieNameBeforeRename)
                {
                    Model.UpdateBbieName(checkableItem.Text);
                }
            }
            catch (TemporaryAbieModelException tame)
            {
                ShowWarningMessage(tame.Message);

                checkableItem.Text = bbieNameBeforeRename;
                ((TextBox) sender).Text = bbieNameBeforeRename;
            }
        }

        // ------------------------------------------------------------------------------------
        // Event handler: Button BDT
        private void buttonAddBdt_Click(object sender, RoutedEventArgs e)
        {
            int selectedIndex = listboxBdts.SelectedIndex;

            Model.AddPotentialBdt();

            listboxBdts.SelectedIndex = selectedIndex;
        }

        // ------------------------------------------------------------------------------------
        // Event handler: ListBox BDTs
        private void listboxBdts_ItemCheckBoxChecked(object sender, RoutedEventArgs e)
        {
            listboxBdts.SelectedItem = GetSelectedCheckableItemforListbox(listboxBdts, (CheckBox) sender);

            var checkableItem = (CheckableItem) listboxBdts.SelectedItem;
            Model.SetSelectedAndCheckedPotentialBdt(checkableItem.Text, checkableItem.Checked);

            SetSelectedItemForBdtListBox();

            UpdateFormState();
        }

        private void listboxBdts_ItemSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listboxBdts.SelectedItem != null)
            {
                var checkableItem = (CheckableItem) listboxBdts.SelectedItem;
                Model.SetSelectedAndCheckedPotentialBdt(checkableItem.Text, null);
            }
        }

        private void listboxBdts_ItemTextBoxGotMouseCapture(object sender, MouseEventArgs e)
        {
            // The listbox of the BDT contains a set of CheckableItems. Each CheckableItem
            // consists of a CheckBox as well as a TextBox. However, in case the user clicks
            // on the text in the TextBox the GotMouseCapture event is triggered but the item
            // in the listbox is not selected causing the rename of a BBIE to fail. Therefore,
            // we need to use the following workaround which ensures that the item in the 
            // ListBox having the focus is also selected. 
            listboxBdts.SelectedItem = GetSelectedCheckableItemforListbox(listboxBdts, (TextBox) sender);

            var checkableItem = (CheckableItem) listboxBdts.SelectedItem;

            // We need to store the original name of the BDT before it is changed as part of a
            // rename process. The reason for doing so is that in case the BDT is renamed to 
            // match the name of an existing BDT it is necessary to display the original name
            // of the BDT. 
            bdtNameBeforeRename = checkableItem.Text;

            Model.SetSelectedAndCheckedPotentialBdt(checkableItem.Text, null);

            listboxBdts.SelectedItem = GetSelectedCheckableItemforListbox(listboxBdts, (TextBox) sender);
        }

        private void listboxBdts_ItemTextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            var checkableItem = ((CheckableItem) listboxBdts.SelectedItem);

            try
            {
                if (checkableItem.Text != bdtNameBeforeRename)
                {
                    Model.UpdateBdtName(checkableItem.Text);
                }
            }
            catch (TemporaryAbieModelException tame)
            {
                ShowWarningMessage(tame.Message);

                checkableItem.Text = bdtNameBeforeRename;
                ((TextBox) sender).Text = bdtNameBeforeRename;
            }
        }


        // ------------------------------------------------------------------------------------
        // Event handler: ListBox ABIEs
        private void listboxAbies_ItemCheckBoxChecked(object sender, RoutedEventArgs e)
        {
            var checkableItem = (CheckableItem) listboxAbies.SelectedItem;
            Model.SetSelectedAndCheckedCandidateAbie(checkableItem.Text, checkableItem.Checked);

            // The following code only keeps the UI in sync with the TemporaryAbieModel since 
            // clicking the CheckBox only triggers an update in the TemporaryAbieModel but does
            // not select the current item in the ListBox.
            listboxAbies.SelectedItem = GetSelectedCheckableItemforListbox(listboxAbies, (CheckBox) sender);

            SetSelectedItemForAsbieListBox();
        }

        private void listboxAbies_ItemSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listboxAbies.SelectedItem != null)
            {
                var checkableItem = (CheckableItem) listboxAbies.SelectedItem;
                Model.SetSelectedAndCheckedCandidateAbie(checkableItem.Text, checkableItem.Checked);

                SetSelectedItemForAsbieListBox();
            }
            else
            {
                Model.SetNoSelectedCandidateAbie();
            }
        }


        // ------------------------------------------------------------------------------------
        // Event handler: ListBox ASBIEs
        private void listboxAsbies_ItemCheckBoxChecked(object sender, RoutedEventArgs e)
        {
            var checkableItem = (CheckableItem) listboxAsbies.SelectedItem;
            Model.SetSelectedAndCheckedPotentialAsbie(checkableItem.Text, checkableItem.Checked);

            listboxAsbies.SelectedItem = GetSelectedCheckableItemforListbox(listboxAsbies, (CheckBox) sender);
        }

        private void listboxAsbies_ItemSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listboxAsbies.SelectedItem != null)
            {
                var checkableItem = (CheckableItem) listboxAsbies.SelectedItem;

                Model.SetSelectedAndCheckedPotentialAsbie(checkableItem.Text, checkableItem.Checked);
            }
        }


        // ------------------------------------------------------------------------------------
        // Event handler: TextBox ABIE Prefix
        private void textboxAbiePrefix_TextChanged(object sender, TextChangedEventArgs e)
        {
            Model.AbiePrefix = textboxPrefix.Text;

            if (string.IsNullOrEmpty(Model.AbiePrefix))
            {
                textboxAbieName.Text = Model.AbieName;
            }
            else
            {
                if (!string.IsNullOrEmpty(Model.AbieName))
                {
                    int indexOfQualifierSeparator = Model.AbieName.IndexOf('_');

                    if (indexOfQualifierSeparator != -1)
                    {
                        string newAbieName = Model.AbiePrefix + "_" +
                                             Model.AbieName.Substring(indexOfQualifierSeparator + 1);
                        Model.AbieName = newAbieName;
                    }
                }
            }
        }

        // ------------------------------------------------------------------------------------
        // Event handler: TextBox ABIE Name
        private void textboxAbieName_TextChanged(object sender, TextChangedEventArgs e)
        {
            Model.AbieName = textboxAbieName.Text;
        }

        // ------------------------------------------------------------------------------------
        // Event handler: ComboBox BDT Libraries
        private void comboboxBdtLibraries_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Model.SetSelectedCandidateBdtLibrary(comboboxBdtLibraries.SelectedItem.ToString());

            UpdateFormState();
        }

        // ------------------------------------------------------------------------------------
        // Event handler: ComboBox BIE Libraries
        private void comboboxBieLibraries_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Model.SetSelectedCandidateBieLibrary(comboboxBieLibraries.SelectedItem.ToString());

            UpdateFormState();
        }

        // ------------------------------------------------------------------------------------
        // Event handler: Button Create / Button Update
        private void buttonCreateOrUpdate_Click(object sender, RoutedEventArgs e)
        {
            // Disable the "Create Button"/"Update Button" while the ABIE is created
            buttonCreateOrUpdate.IsEnabled = false;

            if (AbieEditorMode == EditorModes.AbieEditorModes.Create)
            {
                try
                {
                    Model.CreateAbie();
                    ShowInformativeMessage(String.Format("A new ABIE named \"{0}\" was created successfully.",
                                                         Model.AbieName));
                }
                catch (TemporaryAbieModelException tame)
                {
                    ShowWarningMessage(tame.Message);
                }
            }
            else if (AbieEditorMode == EditorModes.AbieEditorModes.CreateFromAcc)
            {
                try
                {
                    Model.CreateAbie();
                    //Due to a bug in Enterprise Architect we need to close and reopen the current diagram in order to display the actual changes!
                    Repository.CloseDiagram(DiagramId);
                    Repository.OpenDiagram(DiagramId);
                    //To improve usability we close the wizard and show the changes in the current diagram rather than displaying an info message.
                    Close();
                    //ShowInformativeMessage(String.Format("A new ABIE named \"{0}\" was created successfully.",
                    //                                    Model.AbieName));
                }
                catch (TemporaryAbieModelException tame)
                {
                    ShowWarningMessage(tame.Message);
                }
            }
            else if (AbieEditorMode == EditorModes.AbieEditorModes.Update)
            {
                try
                {
                    Model.UpdateAbie(abieToBeUpdated);
                    ShowInformativeMessage(String.Format("The ABIE named \"{0}\" was updated successfully.",
                                                         Model.AbieName));
                }
                catch (TemporaryAbieModelException tame)
                {
                    ShowWarningMessage(tame.Message);
                }
            }

            // After the ABIE is created the "Create Button"/"Update Button" is enabled again. 
            buttonCreateOrUpdate.IsEnabled = true;
        }

        // ------------------------------------------------------------------------------------
        // Event handler: Button Close
        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        #endregion

        #region Methods for controlling the Form State

        private void UpdateFormState()
        {
            if (AbieEditorMode == EditorModes.AbieEditorModes.Create)
            {
                if (comboboxCcLibraries.SelectedItem != null)
                {
                    SetEnabledForAccComboBox(true);

                    if (comboboxAccs.SelectedItem != null)
                    {
                        SetEnabledForAttributeAndAssoicationTabs(true);
                        SetEnabledForAbieProperties(true);

                        if ((comboboxBieLibraries.SelectedItem != null) && (comboboxBdtLibraries.SelectedItem != null) &&
                            (Model.ContainsValidConfiguration()))
                        {
                            SetEnabledForCreateButton(true);
                        }
                        else
                        {
                            SetEnabledForCreateButton(false);
                        }
                    }
                }
                else
                {
                    SetEnabledForAccComboBox(false);
                    SetEnabledForAttributeAndAssoicationTabs(false);
                    SetEnabledForAbieProperties(false);
                    SetEnabledForCreateButton(false);
                }
            }
            else if (AbieEditorMode == EditorModes.AbieEditorModes.Update)
            {
            }
            else if (AbieEditorMode == EditorModes.AbieEditorModes.CreateFromAcc)
            {
                SetEnabledForAccComboBox(false);
                comboboxCcLibraries.IsEnabled = false;
                comboboxBieLibraries.IsEnabled = false;
            }
        }

        private void SetEnabledForAccComboBox(bool enabledState)
        {
            comboboxAccs.IsEnabled = enabledState;
        }

        private void SetEnabledForAttributeAndAssoicationTabs(bool enabledState)
        {
            tabAttributes.IsEnabled = enabledState;
            checkboxBccs.IsEnabled = enabledState;
            listboxBccs.IsEnabled = enabledState;
            buttonAddBbie.IsEnabled = enabledState;
            listboxBbies.IsEnabled = enabledState;
            listboxBdts.IsEnabled = enabledState;
            buttonAddBdt.IsEnabled = enabledState;

            tabAssociations.IsEnabled = enabledState;
            listboxAbies.IsEnabled = enabledState;
            listboxAsbies.IsEnabled = enabledState;
        }

        private void SetEnabledForAbieProperties(bool enabledState)
        {
            textboxPrefix.IsEnabled = enabledState;
            textboxAbieName.IsEnabled = enabledState;
        }

        private void SetEnabledForCreateButton(bool enabledState)
        {
            buttonCreateOrUpdate.IsEnabled = enabledState;
        }

        #endregion

        #region Methods for extending Control Behaviour

        private void ForceTextboxToLoseFocus(object sender, KeyEventArgs e)
        {
            var textBox = (TextBox) sender;
            var siblingOfTextBox = (CheckBox) ((StackPanel) textBox.Parent).Children[0];

            switch (e.Key)
            {
                case Key.Return:
                    siblingOfTextBox.Focus();
                    break;

                case Key.Escape:
                    textBox.Undo();
                    siblingOfTextBox.Focus();
                    break;
            }
        }

        private static CheckableItem GetSelectedCheckableItemforListbox(ListBox listBox, string selectedItemText)
        {
            foreach (CheckableItem checkableItem in listBox.Items)
            {
                if (checkableItem.Text.Equals(selectedItemText))
                {
                    return checkableItem;
                }
            }

            return null;
        }

        private static CheckableItem GetSelectedCheckableItemforListbox(ListBox listBox, CheckBox checkBox)
        {
            var parent = (StackPanel) checkBox.Parent;
            string selectedItemText = "";

            foreach (UIElement child in parent.Children)
            {
                if (child.GetType() == typeof (TextBox))
                {
                    selectedItemText = ((TextBox) child).Text;
                }
            }

            return GetSelectedCheckableItemforListbox(listBox, selectedItemText);
        }

        private static CheckableItem GetSelectedCheckableItemforListbox(ListBox listBox, TextBox textBox)
        {
            string selectedItemText = textBox.Text;

            return GetSelectedCheckableItemforListbox(listBox, selectedItemText);
        }

        #endregion

        #region Methods for supporting the User Interaction

        private static void ShowWarningMessage(string message)
        {
            MessageBox.Show(message, "ABIE Editor", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private static void ShowInformativeMessage(string message)
        {
            MessageBox.Show(message, "ABIE Editor", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void SelectDefaultLibraries()
        {
            comboboxCcLibraries.SelectedIndex = 0;
            comboboxBdtLibraries.SelectedIndex = 0;
            comboboxBieLibraries.SelectedIndex = 0;
        }

        private void SelectFirstAcc()
        {
            comboboxAccs.SelectedIndex = 0;
        }

        private void SetSelectedItemForBccListBox()
        {
            int index = 0;

            foreach (CheckableItem item in listboxBccs.Items)
            {
                if (item.Checked)
                {
                    break;
                }

                index++;
            }

            if (listboxBccs.Items.Count == index)
            {
                index = 0;
            }

            if (index < listboxBccs.Items.Count)
            {
                listboxBccs.SelectedItem = listboxBccs.Items[index];
            }
        }


        private void SetSelectedItemForBbieListBox()
        {
            int index = 0;

            foreach (CheckableItem item in listboxBbies.Items)
            {
                if (item.Checked)
                {
                    break;
                }

                index++;
            }

            if (listboxBbies.Items.Count == index)
            {
                index = 0;
            }

            if (index < listboxBbies.Items.Count)
            {
                listboxBbies.SelectedItem = listboxBbies.Items[index];
            }
        }


        private void SetSelectedItemForBdtListBox()
        {
            int index = 0;

            foreach (CheckableItem item in listboxBdts.Items)
            {
                if (item.Checked)
                {
                    break;
                }

                index++;
            }

            if (listboxBdts.Items.Count == index)
            {
                index = 0;
            }

            if (index < listboxBdts.Items.Count)
            {
                listboxBdts.SelectedItem = listboxBdts.Items[index];
            }
        }

        private void SetSelectedItemForAbieListBox()
        {
            int index = 0;

            foreach (CheckableItem item in listboxAbies.Items)
            {
                if (item.Checked)
                {
                    break;
                }

                index++;
            }

            if (listboxAbies.Items.Count == index)
            {
                index = 0;
            }

            if (index < listboxAbies.Items.Count)
            {
                listboxAbies.SelectedItem = listboxAbies.Items[index];
            }
        }

        private void SetSelectedItemForAsbieListBox()
        {
            int index = 0;

            foreach (CheckableItem item in listboxAsbies.Items)
            {
                if (item.Checked)
                {
                    break;
                }

                index++;
            }

            if (listboxAsbies.Items.Count == index)
            {
                index = 0;
            }

            if (index < listboxAsbies.Items.Count)
            {
                listboxAsbies.SelectedItem = listboxAsbies.Items[index];
            }
        }

        private void ShowShield(bool visibilityState)
        {
            if (visibilityState)
            {
                Mouse.OverrideCursor = Cursors.Wait;
                shield.Visibility = Visibility.Visible;
                popupGenerating.Visibility = Visibility.Visible;
                var sbdRotation = (Storyboard) FindResource("sbdRotation");
                sbdRotation.Begin(this);
            }
            else
            {
                shield.Visibility = Visibility.Collapsed;
                var sbdRotation = (Storyboard) FindResource("sbdRotation");
                sbdRotation.Stop();
                popupGenerating.Visibility = Visibility.Collapsed;
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        #endregion
    }
}