using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using CctsRepository;
using CctsRepository.BLibrary;
using CctsRepository.CcLibrary;
using VIENNAAddIn.menu;
using VIENNAAddIn.upcc3.import.mapping;
using Cursors=System.Windows.Input.Cursors;
using MessageBox=System.Windows.Forms.MessageBox;
using OpenFileDialog=Microsoft.Win32.OpenFileDialog;

namespace VIENNAAddIn.upcc3.Wizards.dev.ui
{
    public partial class XsdImporterForm
    {
        private readonly ICctsRepository cctsRepository;
        private IBLibrary selectedBLibrary;
        private ICcLibrary selectedCcLibrary;

        public XsdImporterForm(ICctsRepository cctsRepository)
        {
            Model = new XsdImporterViewModel();
            DataContext = this;
            this.cctsRepository = cctsRepository;

            InitializeComponent();

            buttonImport.IsEnabled = false;
            Model.CcLibraries = new List<ICcLibrary>(this.cctsRepository.GetCcLibraries());
            if (Model.CcLibraries.Count > 0)
                ccLibraryComboBox.SelectedIndex = 0;
            Model.BLibraries = new List<IBLibrary>(this.cctsRepository.GetBLibraries());
            if (Model.BLibraries.Count > 0)
                bLibraryComboBox.SelectedIndex = 0;

            mappedSchemaFileSelector.FileNameChanged += MappedSchemaFileSelectorFileNameChanged;
            mappedSchemaFileSelector.FileName = " ";
            mappedSchemaFileSelector.FileName = "";
        }

        public XsdImporterViewModel Model { get; set; }

        private void CheckIfInputIsValid()
        {
            buttonImport.IsEnabled = (ccLibraryComboBox.SelectedIndex > -1 &&
                                      bLibraryComboBox.SelectedIndex > -1 &&
                                      mappingFilesListBox.Items.Count > 0 &&
                                      !string.IsNullOrEmpty(mappedSchemaFileSelector.FileName));
        }

        private void MappedSchemaFileSelectorFileNameChanged(object sender, RoutedEventArgs args)
        {
            if (mappedSchemaFileSelector.FileName.Length > 0)
            {
                ButtonSchemaAnalyzer.IsEnabled = true;
                ButtonSchemaAnalyzerImage.Opacity = 1;
            }
            else
            {
                ButtonSchemaAnalyzer.IsEnabled = false;
                ButtonSchemaAnalyzerImage.Opacity = 0.3;
            }
            CheckIfInputIsValid();
        }

        public static void ShowForm(AddInContext context)
        {
            new XsdImporterForm(context.CctsRepository).ShowDialog();
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void buttonImport_Click(object sender, RoutedEventArgs e)
        {
            Cursor = Cursors.Wait;
            buttonImport.IsEnabled = false;

            switch (tabControl1.SelectedIndex)
            {
                case 0: // ebInterface
                    if (!StartImportEbInterface())
                    {
                        Cursor = Cursors.Arrow;
                        buttonImport.IsEnabled = true;
                        return;
                    }
                    break;
                case 1: // CCTS
                    throw new NotImplementedException();
                    //XSDImporter.ImportSchemas(new ImporterContext(cctsRepository, cctsSchemaFileSelector.FileName));
            }

            textboxStatus.Text += "Import completed!\n";
            Cursor = Cursors.Arrow;
        }

        private bool StartImportEbInterface()
        {
            try
            {
                new MappingImporter(Model.MappingFiles, new[] {mappedSchemaFileSelector.FileName}, selectedCcLibrary,
                                    selectedBLibrary, docLibraryNameTextBox.Text, bieLibraryNameTextBox.Text,
                                    bdtLibraryNameTextBox.Text, qualifierTextBox.Text, rootElementNameTextBox.Text,
                                    cctsRepository).ImportMapping();
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("The ebInterface Schema file could not be openend!", Title, MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return false;
            }
            catch (DirectoryNotFoundException)
            {
                MessageBox.Show("The ebInterface Schema file could not be openend!", Title, MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return false;
            }
            catch (MappingError me)
            {
                DialogResult errorResult =
                    MessageBox.Show(
                        "An error occured while mapping the following element:\n" + me.Message +
                        "\nYou can edit the mapping file and click 'Retry' to re-start the import process!", Title,
                        MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                if (errorResult == System.Windows.Forms.DialogResult.Retry)
                {
                    return StartImportEbInterface();
                }
                return false;
            }
            return true;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog
                          {
                              DefaultExt = ".mfd",
                              Filter = "MapForce Mapping files (.mfd)|*.mfd",
                              Multiselect = true
                          };
            if (dlg.ShowDialog() == true)
            {
                var tempList = new List<string>(Model.MappingFiles);
                foreach (string file in dlg.FileNames)
                {
                    bool exists = false;
                    foreach (string item in Model.MappingFiles)
                    {
                        if (item.Equals(file))
                        {
                            exists = true;
                            break;
                        }
                    }
                    if (!exists)
                        tempList.Add(file);
                    else
                        System.Windows.MessageBox.Show("The file '" + file + "' already exists in the list!", Title,
                                                       MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
                Model.MappingFiles = tempList;
            }
            CheckIfInputIsValid();
        }

        private void RemoveSelection_Click(object sender, RoutedEventArgs e)
        {
            if (mappingFilesListBox.SelectedIndex > -1)
            {
                var tempList = new List<string>(Model.MappingFiles);
                tempList.RemoveAt(mappingFilesListBox.SelectedIndex);
                Model.MappingFiles = tempList;
            }
            CheckIfInputIsValid();
        }

        private void ButtonSchemaAnalyzer_Click(object sender, RoutedEventArgs e)
        {
            new SchemaAnalyzer(mappedSchemaFileSelector.FileName, "").ShowDialog();
        }

        private void ccLibraryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ccLibraryComboBox.SelectedIndex > -1)
            {
                selectedCcLibrary = Model.CcLibraries[ccLibraryComboBox.SelectedIndex];
            }
            CheckIfInputIsValid();
        }

        private void bLibraryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (bLibraryComboBox.SelectedIndex > -1)
            {
                selectedBLibrary = Model.BLibraries[bLibraryComboBox.SelectedIndex];
            }
            CheckIfInputIsValid();
        }
    }

    public class XsdImporterViewModel : INotifyPropertyChanged
    {
        private List<IBLibrary> bLibraries;
        private List<ICcLibrary> ccLibraries;
        private List<string> mappingFiles;

        public XsdImporterViewModel()
        {
            mappingFiles = new List<string>();
            ccLibraries = new List<ICcLibrary>();
            bLibraries = new List<IBLibrary>();
        }

        public List<ICcLibrary> CcLibraries
        {
            get { return ccLibraries; }
            set
            {
                ccLibraries = value;
                OnPropertyChanged("CcLibraries");
            }
        }

        public List<IBLibrary> BLibraries
        {
            get { return bLibraries; }
            set
            {
                bLibraries = value;
                OnPropertyChanged("BLibraries");
            }
        }

        public List<string> MappingFiles
        {
            get { return mappingFiles; }
            set
            {
                mappingFiles = value;
                OnPropertyChanged("MappingFiles");
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        private void OnPropertyChanged(string fieldName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(fieldName));
            }
        }
    }
}