using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using CctsRepository;
using CctsRepository.DocLibrary;
using VIENNAAddIn.menu;
using VIENNAAddIn.upcc3.export.cctsndr;
using VIENNAAddIn.upcc3.export.mapping;
using CheckBox=System.Windows.Controls.CheckBox;
using ComboBox=System.Windows.Controls.ComboBox;
using Cursors=System.Windows.Input.Cursors;
using ListBox=System.Windows.Controls.ListBox;
using MessageBox=System.Windows.Forms.MessageBox;

namespace VIENNAAddIn.upcc3.Wizards.dev.ui
{
    /// <summary>
    /// Interaction logic for ExporterForm.xaml
    /// </summary>
    public partial class ExporterForm
    {
        //private const int MARGIN = 15;
        private readonly Cache cache;
        private readonly ICctsRepository cctsR;
        private readonly Dictionary<string, StackPanel> documentModels = new Dictionary<string, StackPanel>();
        //private int mouseDownPosX;
        private string originalXMLSchema = "";
        private string outputDirectory = "";
        private string selectedBIVName;
        private string selectedModelName;
        private EA.Package selectedPackage;

        public ExporterForm(ICctsRepository cctsRepository,EA.Package selectedPackage)
        {
            cctsR = cctsRepository;
            this.selectedPackage = selectedPackage;
            try
            {
                cache = new Cache();
                //cache.LoadBIVs(cctsR);
                //TODO get only the BIV's of the selected package
                cache.LoadBIVs(cctsR, selectedPackage);
            }
            catch (CacheException ce)
            {
                MessageBox.Show(ce.Message, "VIENNA Add-In Information", MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
            }

            InitializeComponent();

            //documentModels.Add("CCTS", panelSettingsCCTS);
            documentModels.Add("XML Schema", panelSettingsXMLSchema);
            //set the pakage name
            this.selectedPackageTextBox.Text = selectedPackage.Name;

            MirrorDOCsToUI();

        }

        public static void ShowForm(AddInContext context)
        {
            new ExporterForm(context.CctsRepository,context.SelectedPackage).Show();
        }

        private static void SetSafeIndex(ComboBox box, int indexToBeSet)
        {
            if (box.Items.Count > 0)
            {
                box.SelectedIndex = indexToBeSet < box.Items.Count ? indexToBeSet : 0;
            }
        }

        private static void SetSafeIndex(ListBox box, int indexToBeSet)
        {
            if (box.Items.Count > 0)
            {
                box.SelectedIndex = indexToBeSet < box.Items.Count ? indexToBeSet : 0;
            }
        }


        private void MirrorModelsToUI()
        {
            comboboxDocumentModel.Items.Clear();
            foreach (string item in documentModels.Keys)
            {
                comboboxDocumentModel.Items.Add(item);
            }
        }

        private void MirrorDOCsToUI()
        {
            foreach (var biv in cache.BIVs.Values) 
            {
            	try
        		{
            		biv.LoadDOC(cctsR);
            	}
            	catch (CacheException ce)
	            {
	                // TODO: find a way to properly list issues
//	                MessageBox.Show(ce.Message, "VIENNA Add-In Error", MessageBoxButtons.OK,
//	                                MessageBoxIcon.Error);
	            }    
            	cDOC doc = biv.DOC;
            	if (doc != null)
	            {
	                var newItem = new CheckBox
	                                  {
	                                      Content = doc.Name,
	                                      IsChecked = (doc.State == CheckState.Checked ? true : false)
	                                  };
	                documentsListBox.Items.Add(newItem);
	            }
            }
            ResetForm(1);
            selectAlldocuments(true);
        }

        private void MirrorDOCSettingsToUI()
        {
            GatherUserInput();

            cBIV biv = cache.BIVs[selectedBIVName];
            if (biv.DOC != null)
            {
                textboxTagetNamespace.Text = biv.DOC.TargetNamespace;
                textboxPrefix.Text = biv.DOC.TargetNamespacePrefix;
            }
        }

        private void GatherUserInput()
        {
            selectedModelName = comboboxDocumentModel.SelectedIndex >= 0
                                    ? comboboxDocumentModel.SelectedItem.ToString()
                                    : "";
            outputDirectory = textboxOutputDirectory.DirectoryName;
            originalXMLSchema = textboxXMLSchemaOriginalFile.FileName;
        }

        private void ResetForm(int levelOfReset)
        {
            switch (levelOfReset)
            {
                case 0:
                    documentsListBox.IsEnabled = false;
                    textboxTagetNamespace.IsEnabled = false;
                    textboxPrefix.IsEnabled = false;
                    checkboxDocumentationAnnotations.IsEnabled = false;
                    checkboxGenerateCcSchemas.IsEnabled = false;
                    comboboxDocumentModel.IsEnabled = false;
                    textboxOutputDirectory.IsEnabled = false;
                    buttonExport.IsEnabled = false;
                    break;

                case 1:
                    documentsListBox.IsEnabled = true;
                    textboxTagetNamespace.IsEnabled = true;
                    textboxPrefix.IsEnabled = true;
                    checkboxDocumentationAnnotations.IsEnabled = true;
                    checkboxGenerateCcSchemas.IsEnabled = true;
                    comboboxDocumentModel.IsEnabled = true;
                    textboxOutputDirectory.IsEnabled = true;
                    buttonExport.IsEnabled = false;
                    break;

                case 2:
                    buttonExport.IsEnabled = true;
                    break;
            }
        }

        private void VerifyUserInput()
        {
            GatherUserInput();

            if (comboboxDocumentModel.SelectedItem.Equals("CCTS"))
            {
                if (!(String.IsNullOrEmpty(selectedBIVName)) &&
                    //(comboboxDocuments.CheckedItems.Count > 0) &&
                    !(String.IsNullOrEmpty(textboxTagetNamespace.Text)) &&
                    !(String.IsNullOrEmpty(textboxPrefix.Text)) &&
                    !(String.IsNullOrEmpty(selectedModelName)) &&
                    !(String.IsNullOrEmpty(textboxOutputDirectory.DirectoryName)))
                {
                    ResetForm(2);
                }
                else
                {
                    ResetForm(1);
                }
            }
            else
            {
                if (!(String.IsNullOrEmpty(selectedBIVName)) &&
                    !(String.IsNullOrEmpty(selectedModelName)) &&
                    !(String.IsNullOrEmpty(textboxXMLSchemaOriginalFile.FileName)))
                {
                    ResetForm(2);
                }
                else
                {
                    ResetForm(1);
                }
            }
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void buttonExport_Click(object sender, RoutedEventArgs e)
        {
            Cursor = Cursors.Wait;
            textBoxStatus.Text = "Starting to generate XML schemas ...\n\n";
            GatherUserInput();
            cBIV currentBIV = cache.BIVs[selectedBIVName];


            IDocLibrary docl = cctsR.GetDocLibraryById(currentBIV.Id);

            // TODO: xsd generator needs to be adapted - currently all doc libraries are being generated whereas
            // only the ones that are checked should be generated.. 

            //TODO: currently the wizard just takes the input from the text fields whereas the prefix and the
            // target namespace should be (a) stored in the cache and (b) read from there while generation.. 

            if (comboboxDocumentModel.SelectedItem.Equals("CCTS"))
            {
                // TODO: check if path is valid
                cDOC document = currentBIV.DOC;
                if (document != null && document.State == CheckState.Checked)
                {
                }
                string targetNamespace = textboxTagetNamespace.Text;
                string namespacePrefix = textboxPrefix.Text;
                bool annotate = checkboxDocumentationAnnotations.IsChecked == true ? true : false;
                bool allschemas = checkboxGenerateCcSchemas.IsChecked == true ? true : false;
                var generationContext = new GeneratorContext(cctsR, targetNamespace,
                                                             namespacePrefix, annotate, allschemas,
                                                             outputDirectory, docl);
                generationContext.SchemaAdded += HandleSchemaAdded;
                XSDGenerator.GenerateSchemas(generationContext);
            }
            else
            {
                SubsetExporter.ExportSubset(docl, originalXMLSchema, outputDirectory);
            }
            textBoxStatus.Text += "\nGenerating XML schemas completed!";
            Cursor = Cursors.Arrow;
        }

        private void HandleSchemaAdded(object sender, SchemaAddedEventArgs e)
        {
            //textBoxStatus.Text += "Schema generated: file:///" + e.FileName + "\n";
            textBoxStatus.Text += "Generated Schema file:" + e.FileName + "\n";
        }

        private void comboboxDocumentModel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboboxDocumentModel.SelectedItem != null)
            {
                StackPanel tempPanel;
                if (comboboxDocumentModel.SelectedItem.Equals("ebInterface"))
                {
                    generationSettings.Header = "Subsetting Generation Settings";
                }
                else
                {
                    generationSettings.Header = "Generation Settings";
                }
                if (documentModels.TryGetValue((string) comboboxDocumentModel.SelectedItem, out tempPanel))
                {
                    foreach (StackPanel panel in documentModels.Values)
                    {
                        panel.Visibility = Visibility.Collapsed;
                    }
                    tempPanel.Visibility = Visibility.Visible;
                }
                VerifyUserInput();
            }
        }


        private void textboxPrefix_TextChanged(object sender, TextChangedEventArgs e)
        {
            // todo: make path check
            cache.BIVs[selectedBIVName].DOC.TargetNamespacePrefix = textboxTagetNamespace.Text;

            VerifyUserInput();
        }

        private void textboxTagetNamespace_TextChanged(object sender, TextChangedEventArgs e)
        {
            // todo: make path check
            cache.BIVs[selectedBIVName].DOC.TargetNamespace = textboxTagetNamespace.Text;

            VerifyUserInput();
        }

        private void textboxOutputDirectory_DirectoryNameChanged(object sender, RoutedEventArgs e)
        {
            // todo: make path check
            cache.BIVs[selectedBIVName].DOC.OutputDirectory = outputDirectory;

            VerifyUserInput();
        }

        private void textboxXMLSchemaOriginalFile_FileNameChanged(object sender, RoutedEventArgs e)
        {
            VerifyUserInput();
        }
        private void selectAlldocuments(bool isSelected)
        {
        	foreach (CheckBox item in documentsListBox.Items) 
			{
				item.IsChecked = isSelected;
			}
        }

		void selectNoneButton_Click(object sender, RoutedEventArgs e)
		{
			selectAlldocuments(false);
		}
		void selectAllButton_Click(object sender, RoutedEventArgs e)
		{
			selectAlldocuments(true);
		}
    }


}