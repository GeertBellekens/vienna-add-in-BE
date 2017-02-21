// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************

using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using CctsRepository;
using VIENNAAddIn.menu;
using VIENNAAddIn.upcc3.Wizards.dev.temporarymodel.subsettingmodel;
using VIENNAAddIn.upcc3.Wizards.dev.util;

namespace VIENNAAddIn.upcc3.Wizards.dev.ui
{
    public partial class SubSettingWizard
    {
        private readonly BackgroundWorker backgroundworker;
        private readonly ICctsRepository repo;
        private DoWorkEventHandler doWorkEventHandler;

        public SubSettingWizard(ICctsRepository cctsRepository)
        {
            backgroundworker = new BackgroundWorker
                                   {
                                       WorkerReportsProgress = false,
                                       WorkerSupportsCancellation = false
                                   };

            InitializeComponent();
            repo = cctsRepository;
            Model = new TemporarySubSettingModel(repo);
            DataContext = this;
        }

        #region BackgroundWorkers

        private void backgroundworkerDocLibs_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Model = (TemporarySubSettingModel) e.Result;
            backgroundworker.DoWork -= doWorkEventHandler;


            ShowShield(false);
        }

        private static void backgroundworkerDocLibs_DoWork(object sender, DoWorkEventArgs e)
        {
            var tempModel = (TemporarySubSettingModel) e.Argument;
            tempModel.SetSelectedCandidateDocLibrary(tempModel.currentDocLibrary);
            e.Result = tempModel;
        }

        private void backgroundworkerCreateSubset_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Model = (TemporarySubSettingModel)e.Result;
            backgroundworker.DoWork -= doWorkEventHandler;


            ShowShield(false);
        }

        private static void backgroundworkerCreateSubset_DoWork(object sender, DoWorkEventArgs e)
        {
            var tempModel = (TemporarySubSettingModel)e.Argument;
            tempModel.createSubSet();
            e.Result = tempModel;
        }

        private void ShowShield(bool shown)
        {
            if (shown)
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

        public TemporarySubSettingModel Model { get; set; }

        public static void ShowCreateDialog(AddInContext context)
        {
            new SubSettingWizard(context.CctsRepository).ShowDialog();
        }

        #region Methods for handling Control Events

        // ------------------------------------------------------------------------------------
        // Event handler: ComboBox Doc Libraries
        private void comboboxDocLibraries_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Model.currentDocLibrary = comboboxDocLibraries.SelectedItem.ToString();
            doWorkEventHandler = backgroundworkerDocLibs_DoWork;
            backgroundworker.DoWork += doWorkEventHandler;
            backgroundworker.RunWorkerCompleted += backgroundworkerDocLibs_RunWorkerCompleted;
            ShowShield(true);

            backgroundworker.RunWorkerAsync(Model);
        }

        // ------------------------------------------------------------------------------------
        // Event handler: ComboBox RootElements
        //private void comboboxRootElement_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (comboboxRootElement.SelectedItem != null)
        //    {
        //        Model.SetSelectedCandidateRootElement(comboboxRootElement.SelectedItem.ToString());
        //        foreach (CheckableTreeViewItem checkableTreeViewItem in Model.CandidateAbieItems)
        //        {
        //            Console.WriteLine(checkableTreeViewItem.Text);
        //        }
        //    }
        //}

        private void treeviewAbies_ItemCheckBoxChecked(object sender, RoutedEventArgs e)
        {
            var checkableTreeViewItem = GetSelectedCheckableTreeViewItemforTreeView(treeviewAbies,
                                                                                                      (CheckBox) sender);
            Model.SetCheckedCandidateAbie(checkableTreeViewItem);
        }

        private void treeviewAbies_ItemSelectionChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (treeviewAbies.SelectedItem != null)
            {
                var checkableTreeViewItem = (CheckableTreeViewItem) treeviewAbies.SelectedItem;
                Model.SetSelectedCandidateAbie(checkableTreeViewItem);
            }
        }


        private void treeviewAbies_Expanded(object sender, RoutedEventArgs e)
        {
            var treeviewItem = (TreeViewItem) e.OriginalSource;
            var checkabletreeviewItem = (CheckableTreeViewItem) treeviewItem.Header;
            if (checkabletreeviewItem != null)
            {
                checkabletreeviewItem.IsExpanded = treeviewItem.IsExpanded;
            }
        }

        // Event handler: ListBox BBIEs
        private void listboxBbies_ItemCheckBoxChecked(object sender, RoutedEventArgs e)
        {
            var checkbox = (CheckBox) e.OriginalSource;

            var checkableItem = GetSelectedCheckableItemforListbox(listboxBbies,checkbox);
            Model.SetCheckedPotentialBbie(checkableItem);
        }

        private void listboxBbies_ItemSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        // Event handler: ListBox BDTs
        private void listboxAsbies_ItemCheckBoxChecked(object sender, RoutedEventArgs e)
        {
            var checkbox = (CheckBox)e.OriginalSource;

            var ioConnectionItem = GetSelectedIOConnectionItemforListbox(listboxAsbies, checkbox);
            Model.SetCheckedPotentialAsbie(ioConnectionItem);
        }

        private void listboxAsbies_ItemSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //nothing to do!
        }


        private static CheckableItem GetSelectedCheckableItemforListbox(ListBox listBox, CheckBox checkBox)
        {
            var parent = (StackPanel)checkBox.Parent;
            var selectedItemText = "";

            foreach (UIElement child in parent.Children)
            {
                if (child.GetType() == typeof(TextBox))
                {
                    selectedItemText = ((TextBox)child).Text;
                }
            }

            return GetSelectedCheckableItemforListbox(listBox, selectedItemText);
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

        private static IOConnectionItem GetSelectedIOConnectionItemforListbox(ListBox listBox, string selectedItemText)
        {
            foreach (IOConnectionItem ioConnectionItem in listBox.Items)
            {
                if (ioConnectionItem.Text.Equals(selectedItemText))
                {
                    return ioConnectionItem;
                }
            }

            return null;
        }

        private static IOConnectionItem GetSelectedIOConnectionItemforListbox(ListBox listBox, CheckBox checkBox)
        {
            var parent = (StackPanel)checkBox.Parent;
            var selectedItemText = "";

            foreach (UIElement child in parent.Children)
            {
                if (child.GetType() == typeof(TextBox))
                {
                    selectedItemText = ((TextBox)child).Text;
                }
            }

            return GetSelectedIOConnectionItemforListbox(listBox, selectedItemText);
        }

        private static CheckableTreeViewItem GetSelectedCheckableTreeViewItemforTreeView(TreeView treeView,
                                                                                         CheckBox checkBox)
        {
            var parent = (StackPanel) checkBox.Parent;
            var selectedId = 0;
            if (parent.Children != null)
            {
                var textBlock2 = (TextBlock) parent.Children[2];
                selectedId = int.Parse(textBlock2.Text);
            }

            //foreach (UIElement child in parent.Children)
            //{
            //    if (child.GetType() == typeof (TextBlock))
            //    {
            //        selectedItemText = ((TextBlock) child).Text;
            //    }
            //}
            return GetSelectedCheckableTreeViewItemForTreeView(treeView, selectedId);
        }

        private static CheckableTreeViewItem GetSelectedCheckableTreeViewItemForTreeView(TreeView treeView,
                                                                                         int selectedId)
        {
            foreach (CheckableTreeViewItem checkableTreeViewItem in treeView.Items)
            {
                if (checkableTreeViewItem.Id.Equals(selectedId))
                {
                    return checkableTreeViewItem;
                }
                if (checkableTreeViewItem.Children == null) continue;
                var tempItem =
                    GetSelectedCheckableTreeViewItemForTreeViewItems(checkableTreeViewItem.Children,
                                                                     selectedId);
                if (tempItem != null)
                {
                    return tempItem;
                }
            }
            return null;
        }

        private static CheckableTreeViewItem GetSelectedCheckableTreeViewItemForTreeViewItems(
            IEnumerable<CheckableTreeViewItem> listToSearch, int selectedId)
        {
            foreach (var checkableTreeViewItem in listToSearch)
            {
                if (checkableTreeViewItem.Id.Equals(selectedId))
                {
                    return checkableTreeViewItem;
                }
                if (checkableTreeViewItem.Children == null) continue;
                var tempItem =
                    GetSelectedCheckableTreeViewItemForTreeViewItems(checkableTreeViewItem.Children,
                                                                     selectedId);
                if (tempItem != null)
                {
                    return tempItem;
                }
            }
            return null;
        }

        // ------------------------------------------------------------------------------------
        // Event handler: Button Create / Button Update
        private void buttonCreateSubSet_Click(object sender, RoutedEventArgs e)
        {
            if (schematronCheckBox.IsChecked == true && (namespacePrefix.Text.Equals(string.Empty) || targetNamespace.Text.Equals(string.Empty) || textboxOutputDirectory.DirectoryName==null))
            {
                MessageBox.Show("The definition of a Namespace Prefix, Target Namespace and Output Directory are required to generate Schematron Rules!","Warning!");
            }
            else
            {
                Model.namespacePrefix = namespacePrefix.Text;
                Model.targetNamespace = targetNamespace.Text;
                Model.outputDirectory = textboxOutputDirectory.DirectoryName;
                doWorkEventHandler = backgroundworkerCreateSubset_DoWork;
                backgroundworker.DoWork += doWorkEventHandler;
                backgroundworker.RunWorkerCompleted += backgroundworkerCreateSubset_RunWorkerCompleted;
                ShowShield(true);

                backgroundworker.RunWorkerAsync(Model);
            }
        }

        // ------------------------------------------------------------------------------------
        // Event handler: Button Close
        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        #endregion

        public static void ShowForm(AddInContext context)
        {
            new SubSettingWizard(context.CctsRepository).Show();
        }
    }
}