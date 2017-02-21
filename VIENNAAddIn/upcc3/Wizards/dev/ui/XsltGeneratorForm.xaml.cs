using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Schema;
using CctsRepository;
using CctsRepository.BLibrary;
using CctsRepository.CcLibrary;
using VIENNAAddIn.menu;
using VIENNAAddIn.upcc3.import.mapping;
using VIENNAAddIn.upcc3.Wizards.dev.util;
using Cursors = System.Windows.Input.Cursors;
using MessageBox = System.Windows.Forms.MessageBox;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace VIENNAAddIn.upcc3.Wizards.dev.ui
{
    public partial class XsltGeneratorForm
    {
        private readonly ICctsRepository CctsRepository;
        private IBLibrary SelectedBLibrary;
        private ICcLibrary SelectedCcLibrary;

        public XsltGeneratorForm(ICctsRepository cctsRepository)
        {
            Model = new XsltGeneratorViewModel();
            DataContext = this;
            CctsRepository = cctsRepository;

            InitializeComponent();

            buttonImport.IsEnabled = false;
            Model.CcLibraries = new List<ICcLibrary>(CctsRepository.GetCcLibraries());
            if (Model.CcLibraries.Count > 0)
                ccLibraryComboBox.SelectedIndex = 0;
            Model.BLibraries = new List<IBLibrary>(CctsRepository.GetBLibraries());
            if (Model.BLibraries.Count > 0)
                bLibraryComboBox.SelectedIndex = 0;

            mappedSchemaFileSelector.FileNameChanged += MappedSchemaFileSelectorFileNameChanged;
            mappedSchemaFileSelector.FileName = " ";
            mappedSchemaFileSelector.FileName = "";
        }

        public XsltGeneratorViewModel Model { get; set; }

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
            new XsltGeneratorForm(context.CctsRepository).ShowDialog();
        }

        private void ButtonCloseClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ButtonImportClick(object sender, RoutedEventArgs e)
        {
            Cursor = Cursors.Wait;
            buttonImport.IsEnabled = false;
            if (!StartImportSchema())
            {
                Cursor = Cursors.Arrow;
                buttonImport.IsEnabled = true;
                return;
            }
            textboxStatus.Text += "Import completed!\n";
            Cursor = Cursors.Arrow;
        }

        private bool StartImportSchema()
        {
            var importer = new MappingImporter(Model.MappingFiles, new[] {mappedSchemaFileSelector.FileName},
                                               SelectedCcLibrary,
                                               SelectedBLibrary, docLibraryNameTextBox.Text,
                                               bieLibraryNameTextBox.Text,
                                               bdtLibraryNameTextBox.Text, qualifierTextBox.Text,
                                               rootElementNameTextBox.Text,
                                               CctsRepository);
            try
            {
                importer.ImportMapping();
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show(@"The Schema file could not be openend!", Title, MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return false;
            }
            catch (DirectoryNotFoundException)
            {
                MessageBox.Show(@"The Schema file could not be openend!", Title, MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return false;
            }
            catch (MappingError me)
            {
                DialogResult errorResult =
                    MessageBox.Show(
                        @"An error occured while mapping the following element:" + me.Message +
                        @"You can edit the mapping file and click 'Retry' to re-start the import process!", Title,
                        MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                if (errorResult == System.Windows.Forms.DialogResult.Retry)
                {
                    return StartImportSchema();
                }
                return false;
            }
            SchemaMapping schemaMapping = importer.mappings;
            XmlSchema schema = XmlSchema.Read(XmlReader.Create(mappedSchemaFileSelector.FileName), null);
            var xsltLogic = new XsltGenerator(schema.TargetNamespace, schemaMapping);
            XmlDocument outputDocToCCl = xsltLogic.SetUpXsltToCClDocument();
            var outputDocToSource = xsltLogic.SetUpXsltToSourceDocument();
            outputDocToSource.Save(Path.Combine(@outputFolder.DirectoryName,"mappingToSource.xslt"));
            outputDocToCCl.Save(Path.Combine(@outputFolder.DirectoryName,"mappingToCCl.xslt"));
            return true;
        }

        

        private void AddClick(object sender, RoutedEventArgs e)
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

        private void RemoveSelectionClick(object sender, RoutedEventArgs e)
        {
            if (mappingFilesListBox.SelectedIndex > -1)
            {
                var tempList = new List<string>(Model.MappingFiles);
                tempList.RemoveAt(mappingFilesListBox.SelectedIndex);
                Model.MappingFiles = tempList;
            }
            CheckIfInputIsValid();
        }

        private void ButtonSchemaAnalyzerClick(object sender, RoutedEventArgs e)
        {
            new SchemaAnalyzer(mappedSchemaFileSelector.FileName, "").ShowDialog();
        }

        private void CcLibraryComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ccLibraryComboBox.SelectedIndex > -1)
            {
                SelectedCcLibrary = Model.CcLibraries[ccLibraryComboBox.SelectedIndex];
            }
            CheckIfInputIsValid();
        }

        private void BLibraryComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (bLibraryComboBox.SelectedIndex > -1)
            {
                SelectedBLibrary = Model.BLibraries[bLibraryComboBox.SelectedIndex];
            }
            CheckIfInputIsValid();
        }

        private void ImportSchemaChecked(object sender, RoutedEventArgs e)
        {
            var checkBox = (System.Windows.Controls.CheckBox) e.Source;
            
            if(checkBox.IsChecked.GetValueOrDefault())
            {
                preferenceScrollViewer.Visibility = Visibility.Visible;
                Height = 500;
            }
            else
            {
                preferenceScrollViewer.Visibility = Visibility.Hidden;
                Height = 365;
            }
        }
    }

    public class XsltGeneratorViewModel : INotifyPropertyChanged
    {
        private List<IBLibrary> bLibraries;
        private List<ICcLibrary> ccLibraries;
        private List<string> mappingFiles;

        public XsltGeneratorViewModel()
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