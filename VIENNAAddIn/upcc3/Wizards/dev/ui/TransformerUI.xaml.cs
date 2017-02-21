using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using CctsRepository;
using CctsRepository.BieLibrary;
using CctsRepository.DocLibrary;
using VIENNAAddIn.menu;
using VIENNAAddIn.upcc3.export.mapping;
using VIENNAAddIn.upcc3.export.transformer;
using ComboBox=System.Windows.Controls.ComboBox;

namespace VIENNAAddIn.upcc3.Wizards.dev.ui
{
    /// <summary>
    /// Interaction logic for Transform.xaml
    /// </summary>
    public partial class TransformerUI : Window
    {
        private readonly ICctsRepository cctsR;
        private string outputDirectory = "";
        private string targetXSDFilename = "";
        private string selectedSourceDocLibrary = "";
        private string selectedTargetDocLibrary = "";
        private string selectedSourceBieLibrary = "";
        private string selectedTargetBieLibrary = "";


        public TransformerUI(ICctsRepository cctsRepository)
        {
            cctsR = cctsRepository;
            InitializeComponent();
            MirrorLibsToUI();
        }

        private void MirrorLibsToUI()
        {
            var allLibraries = cctsR.GetAllLibraries();
            foreach (var library in allLibraries)
            {
                if (library is IDocLibrary)
                {
                    var actualLibrary = (IDocLibrary)library;
                    comboBoxSourceDocLibrary.Items.Add(actualLibrary.Name);
                    comboBoxTargetDocLibrary.Items.Add(actualLibrary.Name);
                }
                else if (library is IBieLibrary)
                {
                    var actualLibrary = (IBieLibrary)library;
                    comboBoxSourceBIELibrary.Items.Add(actualLibrary.Name);
                    comboBoxTargetBIELibrary.Items.Add(actualLibrary.Name);
                }
            }
        }

        public static void ShowForm(AddInContext context)
        {
            new TransformerUI(context.CctsRepository).Show();
        }

        private static void SetSafeIndex(ComboBox box, int indexToBeSet)
        {
            if (box.Items.Count > 0)
            {
                if (indexToBeSet < box.Items.Count)
                {
                    box.SelectedIndex = indexToBeSet;
                }
                else
                {
                    box.SelectedIndex = 0;
                }
            }
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void comboboxSourceDocLibrary_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedSourceDocLibrary = e.AddedItems[0].ToString();
        }

        private void comboBoxTargetDocLibrary_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           selectedTargetDocLibrary = e.AddedItems[0].ToString();
            
        }

        private void targetXSD_FileNameChanged(object sender, RoutedEventArgs e)
        {
            targetXSDFilename = targetXSD.FileName;
        }

        private void comboBoxSourceBIELibrary_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedSourceBieLibrary = e.AddedItems[0].ToString();
        }

        private void comboBoxTargetBIELibrary_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedTargetBieLibrary = e.AddedItems[0].ToString();
        }

        private void OutputDirectory_DirectoryNameChanged(object sender, RoutedEventArgs e)
        {
            outputDirectory = OutputDirectory.DirectoryName;
        }

        private void buttonTransform_Click(object sender, RoutedEventArgs e)
        {
            innerCanvas.Visibility = Visibility.Visible;
            innerCanvas.Dispatcher.Invoke(DispatcherPriority.Render, new Action(delegate { }));

            Mouse.OverrideCursor = Cursors.Wait;

            IDocLibrary sourceDocLibrary = null;
            IDocLibrary targetDocLibrary = null;
            IBieLibrary sourceBieLibrary = null;
            IBieLibrary targetBieLibrary = null;
            foreach (var library in cctsR.GetAllLibraries())
            {
                if (library is IDocLibrary)
                {
                    var doclibrary = (IDocLibrary) library;
                    if (doclibrary.Name.Equals(selectedSourceDocLibrary))
                    {
                        sourceDocLibrary = doclibrary;
                    }
                    else if (doclibrary.Name.Equals(selectedTargetDocLibrary))
                    {
                        targetDocLibrary = doclibrary;
                    }
                }
                else if (library is IBieLibrary)
                {
                    var bieLibrary = (IBieLibrary) library;
                    if (bieLibrary.Name.Equals(selectedSourceBieLibrary))
                    {
                        sourceBieLibrary = bieLibrary;
                    }
                    else if (bieLibrary.Name.Equals(selectedTargetBieLibrary))
                    {
                        targetBieLibrary = bieLibrary;
                    }
                }
            }
            Transformer.Transform(sourceBieLibrary,targetBieLibrary,sourceDocLibrary,targetDocLibrary);
            SubsetExporter.ExportSubset(targetDocLibrary, targetXSDFilename, outputDirectory);

            innerCanvas.Visibility = Visibility.Collapsed;
            Mouse.OverrideCursor = Cursors.Arrow;
        }
    }
}