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
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using CctsRepository;
using CctsRepository.BieLibrary;
using CctsRepository.DocLibrary;
using VIENNAAddIn.menu;
using VIENNAAddIn.upcc3.export.mapping;
using VIENNAAddIn.upcc3.export.transformer;
using VIENNAAddInWpfUserControls;

namespace VIENNAAddIn.upcc3.Wizards.dev.ui
{
    /// <summary>
    /// Interaction logic for Transform.xaml
    /// </summary>
    public partial class TransformerWizard
    {
        private readonly BackgroundWorker backgroundworkerGenerate;
        private readonly BackgroundWorker backgroundworkerInitialize;
        private readonly ICctsRepository cctsR;
        private bool directoryselectorTargetFolderOpen;
        private bool fileselectorXsdDocumentOpen;

        private IBieLibrary selectedSourceBieLibrary;
        private IDocLibrary selectedSourceDocLibrary;
        private IBieLibrary selectedTargetBieLibrary;
        private IDocLibrary selectedTargetDocLibrary;
        private string targetFolder = "";
        private ProjectBrowserContent treeContent;
        private string xsdFilename = "";


        public TransformerWizard(ICctsRepository cctsRepository)
        {
            cctsR = cctsRepository;
            InitializeComponent();

            backgroundworkerInitialize = new BackgroundWorker
                                             {
                                                 WorkerReportsProgress = false,
                                                 WorkerSupportsCancellation = false
                                             };
            backgroundworkerInitialize.DoWork += backgroundworkerInitialize_DoWork;
            backgroundworkerInitialize.RunWorkerCompleted += backgroundworkerInitialize_RunWorkerCompleted;
            if (!backgroundworkerInitialize.IsBusy)
            {
                backgroundworkerInitialize.RunWorkerAsync();
            }

            UpdateUI();
            backgroundworkerGenerate = new BackgroundWorker();
        }

        public static void ShowForm(AddInContext context)
        {
            new TransformerWizard(context.CctsRepository).Show();
        }

        private void backgroundworkerInitialize_DoWork(object sender, DoWorkEventArgs e)
        {
            treeContent = new ProjectBrowserContent(cctsR);
        }

        private void backgroundworkerInitialize_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart) delegate
                                                                                {
                                                                                    treeSourceBie.AllowOnlyOneType =
                                                                                        "BieLibrary";
                                                                                    treeSourceBie.Initialize(treeContent);
                                                                                    treeSourceDoc.AllowOnlyOneType =
                                                                                        "DocLibrary";
                                                                                    treeSourceDoc.Initialize(treeContent);
                                                                                    treeTargetBie.AllowOnlyOneType =
                                                                                        "BieLibrary";
                                                                                    treeTargetBie.Initialize(treeContent);
                                                                                    treeTargetDoc.AllowOnlyOneType =
                                                                                        "DocLibrary";
                                                                                    treeTargetDoc.Initialize(treeContent);
                                                                                });
        }

        private void UpdateUI()
        {
            if (targetFolder.Length > 0)
            {
                buttonTargetFolder.Tag = "True";
                buttonTargetFolder.ToolTip = "Specified output directory:\n" + targetFolder +
                                             "\n(Click here or drag & drop directory to change)";
            }
            else
            {
                buttonTargetFolder.Tag = "False";
                buttonTargetFolder.ToolTip = "Click here or drag & drop directory to specify output directory";
            }

            if (xsdFilename.Length > 0)
            {
                buttonXsdDocument.Tag = "True";
                buttonXsdDocument.ToolTip = "Specified target model schema:\n" + xsdFilename +
                                            "\n(Click here or drag & drop XSD file to change)";
            }
            else
            {
                buttonXsdDocument.Tag = "False";
                buttonXsdDocument.ToolTip = "Click here or drag & drop XSD file to specify target model schema";
            }

            if (selectedSourceBieLibrary != null && selectedSourceDocLibrary != null)
            {
                buttonSourceModel.Tag = "True";
                buttonSourceModel.ToolTip = "Specified source BIE Library:\n" + selectedSourceBieLibrary.Name + "\n" +
                                            "Specified source DOC Library:\n" + selectedSourceDocLibrary.Name + "\n" +
                                            "(Click here to change)";
            }
            else
            {
                buttonSourceModel.Tag = "False";
                buttonSourceModel.ToolTip = "Click here to change source BIE and DOC Library";
            }

            buttonSourceBie.ToolTip = selectedSourceBieLibrary == null ? "Click to specify source BIE Library" : "Click to change the specified source BIE Library";
            buttonSourceDoc.ToolTip = selectedSourceDocLibrary == null ? "Click to specify source DOC Library" : "Click to change the specified source DOC Library";

            if (selectedTargetBieLibrary != null && selectedTargetDocLibrary != null)
            {
                buttonTargetModel.Tag = "True";
                buttonTargetModel.ToolTip = "Specified target BIE Library:\n" + selectedTargetBieLibrary.Name + "\n" +
                                            "Specified target DOC Library:\n" + selectedTargetDocLibrary.Name + "\n" +
                                            "(Click here to change)";
            }
            else
            {
                buttonTargetModel.Tag = "False";
                buttonTargetModel.ToolTip = "Click here to change target BIE and DOC Library";
            }

            buttonTargetBie.ToolTip = selectedTargetBieLibrary == null ? "Click to specify target BIE Library" : "Click to change the specified target BIE Library";
            buttonTargetDoc.ToolTip = selectedTargetDocLibrary == null ? "Click to specify target DOC Library" : "Click to change the specified target DOC Library";

            if (targetFolder.Length > 0 && xsdFilename.Length > 0 && selectedSourceBieLibrary != null &&
                selectedSourceDocLibrary != null && selectedTargetBieLibrary != null && selectedTargetDocLibrary != null)
            {
                buttonGenerate.IsEnabled = true;
            }
            else
            {
                buttonGenerate.IsEnabled = false;
            }
        }

        private void ShowOrHidePopupSourceModel()
        {
            Thread.Sleep(250);
            Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                                   (ThreadStart)
                                   delegate
                                       {
                                           popupSourceModel.IsOpen =
                                               !(selectedSourceBieLibrary != null && selectedSourceDocLibrary != null);
                                       });
        }

        private void ShowOrHidePopupTargetModel()
        {
            Thread.Sleep(250);
            Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                                   (ThreadStart)
                                   delegate
                                       {
                                           popupTargetModel.IsOpen =
                                               !(selectedTargetBieLibrary != null && selectedTargetDocLibrary != null);
                                       });
        }

        private void HideShield()
        {
            Thread.Sleep(251);
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart) delegate
                                                                                {
                                                                                    if (!popupXsdDocument.IsOpen &&
                                                                                        !popupTargetFolder.IsOpen &&
                                                                                        !popupSourceModel.IsOpen &&
                                                                                        !popupTargetModel.IsOpen)
                                                                                    {
                                                                                        shield.Visibility =
                                                                                            Visibility.Collapsed;
                                                                                    }
                                                                                });
        }

        private void ShowShield(bool shown)
        {
            if (shown)
            {
                shield.Visibility = Visibility.Visible;
            }
            else
            {
                var thread = new Thread(HideShield);
                thread.Start();
            }
        }

        private void buttonSourceModel_Click(object sender, RoutedEventArgs e)
        {
            popupSourceModel.IsOpen = true;
        }

        private void buttonTargetModel_Click(object sender, RoutedEventArgs e)
        {
            popupTargetModel.IsOpen = true;
        }

        private void buttonXsdDocument_Click(object sender, RoutedEventArgs e)
        {
            popupXsdDocument.IsOpen = true;
        }

        private void buttonTargetFolder_Click(object sender, RoutedEventArgs e)
        {
            popupTargetFolder.IsOpen = true;
        }

        private void fileselectorXsdDocument_FileNameChanged(object sender, RoutedEventArgs e)
        {
            xsdFilename = fileselectorXsdDocument.FileName;
            UpdateUI();
        }

        private void popupXsdDocument_Opened(object sender, EventArgs e)
        {
            ShowShield(true);
        }

        private void popupXsdDocument_Closed(object sender, EventArgs e)
        {
            if (!fileselectorXsdDocumentOpen)
            {
                ShowShield(false);
            }
        }

        private void directoryselectorTargetFolder_DirectoryNameChanged(object sender, RoutedEventArgs e)
        {
            targetFolder = directoryselectorTargetFolder.DirectoryName;
            UpdateUI();
        }

        private void popupTargetFolder_Opened(object sender, EventArgs e)
        {
            ShowShield(true);
        }

        private void popupTargetFolder_Closed(object sender, EventArgs e)
        {
            if (!directoryselectorTargetFolderOpen)
            {
                ShowShield(false);
            }
        }

        private void buttonXsdDocument_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var droppedFilePaths = e.Data.GetData(DataFormats.FileDrop, true) as string[];
                if (droppedFilePaths != null)
                {
                    if (droppedFilePaths.Length == 1)
                    {
                        xsdFilename = droppedFilePaths[0];
                        fileselectorXsdDocument.FileName = xsdFilename;
                        UpdateUI();
                    }
                }
            }
        }

        private void buttonTargetFolder_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var droppedFilePaths = e.Data.GetData(DataFormats.FileDrop, true) as string[];
                if (droppedFilePaths != null)
                {
                    if (droppedFilePaths.Length == 1)
                    {
                        if (Directory.Exists(droppedFilePaths[0]))
                        {
                            targetFolder = droppedFilePaths[0];
                            directoryselectorTargetFolder.DirectoryName = targetFolder;
                            UpdateUI();
                        }
                    }
                }
            }
        }

        private void buttonTargetBie_Click(object sender, RoutedEventArgs e)
        {
            popupTargetBie.IsOpen = !popupTargetBie.IsOpen;
        }

        private void buttonTargetDoc_Click(object sender, RoutedEventArgs e)
        {
            popupTargetDoc.IsOpen = !popupTargetDoc.IsOpen;
        }

        private void popupTargetModel_Opened(object sender, EventArgs e)
        {
            ShowShield(true);
        }

        private void popupTargetModel_Closed(object sender, EventArgs e)
        {
            ShowShield(false);
        }

        private void treeTargetBie_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (treeTargetBie.SelectedItem != null)
            {
                var item = (TreeViewItem) treeTargetBie.SelectedItem;
                if (item.Tag is IBieLibrary)
                {
                    var lib = (IBieLibrary) item.Tag;
                    selectedTargetBieLibrary = lib;
                    buttonTargetBie.Content = lib.Name;
                    UpdateUI();
                    popupTargetBie.IsOpen = false;
                }
            }
        }

        private void treeTargetDoc_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (treeTargetDoc.SelectedItem != null)
            {
                var item = (TreeViewItem) treeTargetDoc.SelectedItem;
                if (item.Tag is IDocLibrary)
                {
                    var lib = (IDocLibrary) item.Tag;
                    selectedTargetDocLibrary = lib;
                    buttonTargetDoc.Content = lib.Name;
                    UpdateUI();
                    popupTargetDoc.IsOpen = false;
                }
            }
        }

        private void buttonSourceBie_Click(object sender, RoutedEventArgs e)
        {
            popupSourceBie.IsOpen = !popupSourceBie.IsOpen;
        }

        private void buttonSourceDoc_Click(object sender, RoutedEventArgs e)
        {
            popupSourceDoc.IsOpen = !popupSourceDoc.IsOpen;
        }

        private void popupSourceModel_Opened(object sender, EventArgs e)
        {
            ShowShield(true);
        }

        private void popupSourceModel_Closed(object sender, EventArgs e)
        {
            ShowShield(false);
        }

        private void treeSourceBie_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (treeSourceBie.SelectedItem != null)
            {
                var item = (TreeViewItem) treeSourceBie.SelectedItem;
                if (item.Tag is IBieLibrary)
                {
                    var lib = (IBieLibrary) item.Tag;
                    selectedSourceBieLibrary = lib;
                    buttonSourceBie.Content = lib.Name;
                    UpdateUI();
                    popupSourceBie.IsOpen = false;
                    popupSourceModel.IsOpen = true;
                }
            }
        }

        private void treeSourceDoc_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (treeSourceDoc.SelectedItem != null)
            {
                var item = (TreeViewItem) treeSourceDoc.SelectedItem;
                if (item.Tag is IDocLibrary)
                {
                    var lib = (IDocLibrary) item.Tag;
                    selectedSourceDocLibrary = lib;
                    buttonSourceDoc.Content = lib.Name;
                    UpdateUI();
                    popupSourceDoc.IsOpen = false;
                    popupSourceModel.IsOpen = true;
                }
            }
        }

        private void buttonGenerate_Click(object sender, RoutedEventArgs e)
        {
            Transform();
            buttonXsdDocument.IsEnabled = false;
            buttonTargetModel.IsEnabled = false;
            buttonSourceModel.IsEnabled = false;
            buttonTargetFolder.IsEnabled = false;
            buttonGenerate.IsEnabled = false;
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            selectedSourceBieLibrary = null;
            selectedSourceDocLibrary = null;
            selectedTargetBieLibrary = null;
            selectedTargetDocLibrary = null;
            xsdFilename = "";
            targetFolder = "";
            buttonXsdDocument.IsEnabled = true;
            buttonTargetModel.IsEnabled = true;
            buttonSourceModel.IsEnabled = true;
            buttonTargetFolder.IsEnabled = true;
            popupFinished.Visibility = Visibility.Collapsed;
            ShowShield(false);
            UpdateUI();
            //this.Close();
        }

        private void buttonOpenFile_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(targetFolder);
        }

        private void popupSourceBieOrDoc_Closed(object sender, EventArgs e)
        {
            var thread = new Thread(ShowOrHidePopupSourceModel);
            thread.Start();
        }

        private void popupTargetBieOrDoc_Closed(object sender, EventArgs e)
        {
            var thread = new Thread(ShowOrHidePopupTargetModel);
            thread.Start();
        }

        private void directoryselectorTargetFolder_BeforeDialogOpened(object sender, RoutedEventArgs e)
        {
            directoryselectorTargetFolderOpen = true;
        }

        private void directoryselectorTargetFolder_AfterDialogClosed(object sender, RoutedEventArgs e)
        {
            if (directoryselectorTargetFolderOpen)
            {
                ShowShield(false);
                directoryselectorTargetFolderOpen = false;
            }
        }

        private void fileselectorXsdDocument_BeforeDialogOpened(object sender, RoutedEventArgs e)
        {
            fileselectorXsdDocumentOpen = true;
        }

        private void fileselectorXsdDocument_AfterDialogClosed(object sender, RoutedEventArgs e)
        {
            if (fileselectorXsdDocumentOpen)
            {
                ShowShield(false);
                fileselectorXsdDocumentOpen = false;
            }
        }

        private void backgroundworkerGenerate_DoWork(object sender, DoWorkEventArgs e)
        {
            Transformer.Transform(selectedSourceBieLibrary, selectedTargetBieLibrary, selectedSourceDocLibrary,
                                  selectedTargetDocLibrary);
            SubsetExporter.ExportSubset(selectedTargetDocLibrary, xsdFilename,
                                        (targetFolder.EndsWith("\\") || targetFolder.EndsWith("/")
                                             ? targetFolder
                                             : targetFolder + "\\"));
        }

        private void backgroundworkerGenerate_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if ((e.Cancelled))
            {
                var sbdRotation = (Storyboard) FindResource("sbdRotation");
                sbdRotation.Stop();
                popupGenerating.Visibility = Visibility.Collapsed;
                ShowShield(false);
            }
            else
            {
                popupGenerating.Visibility = Visibility.Collapsed;
                popupFinished.Visibility = Visibility.Visible;
            }
        }

        private void Transform()
        {
            ShowShield(true);
            popupGenerating.Visibility = Visibility.Visible;
            var sbdRotation = (Storyboard) FindResource("sbdRotation");
            sbdRotation.Begin(this);

            backgroundworkerGenerate.WorkerReportsProgress = false;
            backgroundworkerGenerate.WorkerSupportsCancellation = false;
            backgroundworkerGenerate.DoWork += backgroundworkerGenerate_DoWork;
            backgroundworkerGenerate.RunWorkerCompleted += backgroundworkerGenerate_RunWorkerCompleted;
            if (!backgroundworkerGenerate.IsBusy)
            {
                backgroundworkerGenerate.RunWorkerAsync();
            }
        }
    }
}