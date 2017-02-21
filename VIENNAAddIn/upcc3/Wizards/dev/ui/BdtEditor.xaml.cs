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
using CctsRepository;
using EA;
using VIENNAAddIn.menu;
using VIENNAAddIn.upcc3.Wizards.dev.temporarymodel.bdtmodel;
using VIENNAAddIn.upcc3.Wizards.dev.temporarymodel.bdtmodel.exceptions;
using VIENNAAddIn.upcc3.Wizards.dev.util;

namespace VIENNAAddIn.upcc3.Wizards.dev.ui
{
    public partial class BdtEditor
    {
        private bool checkstateCallFromSup;
        private readonly Repository Repository;
        private readonly string CdtLibraryName;
        private readonly string CdtName;
        private readonly string BdtLibraryName;
        private readonly int DiagramId;
        private readonly string EditorMode;

        public BdtEditor(ICctsRepository cctsRepository)
        {
            InitializeComponent();
            Model = new TemporaryBdtModel(cctsRepository);
            DataContext = this;
            EditorMode = "create";
            comboboxBdtLibraries.SelectedIndex = 0;
            comboboxCdtLibraries.SelectedIndex = 0;
        }

        public BdtEditor(string cdtLibrary, string cdtName, string bdtLibrary, int diagramId, Repository repository)
        {
            InitializeComponent();
            EditorMode = "createFromCDT";
            CdtLibraryName = cdtLibrary;
            CdtName = cdtName;
            BdtLibraryName = bdtLibrary;
            DiagramId = diagramId;
            Repository = repository;
            Model = new TemporaryBdtModel(CctsRepositoryFactory.CreateCctsRepository(repository));
            DataContext = this;
            comboboxCdts.IsEnabled = false;
            comboboxCdtLibraries.IsEnabled = false;
            comboboxBdtLibraries.IsEnabled = false;
        }

        public TemporaryBdtModel Model { get; set; }

        public static void ShowCreateDialog(AddInContext context)
        {
            new BdtEditor(context.CctsRepository).ShowDialog();
        }

        private void buttonCreate_Click(object sender, RoutedEventArgs e)
        {
            buttonCreate.IsEnabled = false;

            if (EditorMode.Equals("create"))
            {
                try
                {
                    Model.CreateBdt();
                    ShowInformativeMessage(String.Format("A new BDT named \"{0}\" was created successfully.", Model.Name));
                    Model.Reset();
                    UpdateLayout();
                }
                catch (TemporaryBdtModelException tbme)
                {
                    ShowWarningMessage(tbme.Message);
                }
            UpdateFormState();
            }
            else if (EditorMode.Equals("createFromCDT"))
            {
                Model.CreateBdt();
                Repository.CloseDiagram(DiagramId);
                Repository.OpenDiagram(DiagramId);
                Close();
            }
        }

        #region Methods for supporting the User Interaction

        private static void ShowWarningMessage(string message)
        {
            MessageBox.Show(message, "BDT Editor", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private static void ShowInformativeMessage(string message)
        {
            MessageBox.Show(message, "BDT Editor", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        #endregion

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void comboboxCdtLibraries_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(comboboxCdtLibraries.SelectedItem!=null)
            Model.setSelectedCandidateCdtLibrary(comboboxCdtLibraries.SelectedItem.ToString());
        }

        private void comboboxCdts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboboxCdts.SelectedItem != null)
            {
                Model.setSelectedCandidateCdt(comboboxCdts.SelectedItem.ToString());
                textboxBdtName.Text = comboboxCdts.SelectedItem.ToString();
                UpdateFormState();
            }
        }

        private void comboboxBdtLibraries_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(comboboxBdtLibraries.SelectedItem!=null)
            Model.setSelectedCandidateBdtLibrary(comboboxBdtLibraries.SelectedItem.ToString());
        }

        private void textboxBdtName_TextChanged(object sender, TextChangedEventArgs e)
        {
            Model.Name = textboxBdtName.Text;
            UpdateFormState();
        }

        private void textboxBdtPrefix_TextChanged(object sender, TextChangedEventArgs e)
        {
            Model.Prefix = textboxBdtPrefix.Text;
            UpdateFormState();
        }

        private void checkboxSups_Checked(object sender, RoutedEventArgs e)
        {
            var checkBox = (CheckBox) sender;
            if (!checkstateCallFromSup)
            {
                Model.setCheckedAllPotentialSups((bool)checkBox.IsChecked);
            }
            else
            {
                checkstateCallFromSup = false;
            }
        }

        private void listboxSups_ItemCheckBoxChecked(object sender, RoutedEventArgs e)
        {
            var checkbox = (CheckBox) sender;
            var stackPanel = (StackPanel) checkbox.Parent;
            var textbox = (TextBox) stackPanel.Children[1];
            Model.setCheckedPotentialSup((bool) checkbox.IsChecked, textbox.Text);
            foreach (CheckableItem checkableItem in listboxSups.Items)
            {
                if(checkableItem.Text.Equals(textbox.Text))
                {
                    listboxSups.SelectedItem = checkableItem;
                }
            }
            if(!(bool) checkbox.IsChecked && (bool) checkboxSups.IsChecked)
            {
                checkstateCallFromSup = true;
                checkboxSups.IsChecked = false;
            }
        }

        private void UpdateFormState()
        {
            if(comboboxCdtLibraries.SelectedItem!=null && comboboxCdts.SelectedItem!=null)
            {
                tabControl.IsEnabled = true;
                textboxBdtPrefix.IsEnabled = true;
                textboxBdtName.IsEnabled = true;
                    if (comboboxBdtLibraries.SelectedItem != null && !textboxBdtName.Text.Equals("") && !textboxBdtName.Text.Equals(" "))
                    {
                        buttonCreate.IsEnabled = true;
                    }
                    else
                    {
                        buttonCreate.IsEnabled = false;
                    }
            }
            else
            {
                textboxBdtName.IsEnabled = false;
                buttonCreate.IsEnabled = false;
                tabControl.IsEnabled = true;
                textboxBdtPrefix.IsEnabled = true;
            }
        }

        private void comboboxCdtLibraries_Loaded(object sender, RoutedEventArgs e)
        {
            if(EditorMode == "createFromCDT")
            {
                comboboxCdtLibraries.SelectedIndex = comboboxCdtLibraries.Items.IndexOf(CdtLibraryName);
            }
        }

        private void comboboxCdts_Loaded(object sender, RoutedEventArgs e)
        {
            if (EditorMode == "createFromCDT")
            {
                comboboxCdts.SelectedIndex = comboboxCdts.Items.IndexOf(CdtName);
            }
        }

        private void comboboxBdtLibraries_Loaded(object sender, RoutedEventArgs e)
        {
            if (EditorMode == "createFromCDT")
            {
                comboboxBdtLibraries.SelectedIndex = comboboxBdtLibraries.Items.IndexOf(BdtLibraryName);
            }
        }
    }
}