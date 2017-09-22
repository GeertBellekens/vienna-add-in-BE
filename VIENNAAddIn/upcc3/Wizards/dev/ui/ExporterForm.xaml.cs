using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Media;
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
using System.Linq;

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
        private string outputDirectory = "";
        private EA.Package selectedPackage;

        public ExporterForm(ICctsRepository cctsRepository,EA.Package selectedPackage)
        {
            cctsR = cctsRepository;
            this.selectedPackage = selectedPackage;
            try
            {
                cache = new Cache();
                cache.LoadBIVs(cctsR, selectedPackage);
            }
            catch (CacheException ce)
            {
                MessageBox.Show(ce.Message, "VIENNA Add-In Information", MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
            }

            InitializeComponent();

            //documentModels.Add("CCTS", panelSettingsCCTS);
            //set the pakage name
            this.selectedPackageTextBox.Text = selectedPackage.Name;

            MirrorDOCsToUI();

        }

        public static void ShowForm(AddInContext context)
        {
        	var exporterForm = new ExporterForm(context.CctsRepository,context.SelectedPackage);
        	var mainWindowHandle = context.GetmainWindowHandle();
        	if (mainWindowHandle != IntPtr.Zero)
        	{
        		WindowInteropHelper wih = new WindowInteropHelper(exporterForm);
        		wih.Owner = context.GetmainWindowHandle();
        	}
        	exporterForm.Show();
        }

        private void MirrorDOCsToUI()
        {
            foreach (var biv in cache.BIVs.Values) 
            {
            	cDOC doc = biv.DOC;
            	if (doc != null)
	            {
	                var newItem = new CheckBox
	                                  {
	                                      Content = doc.Name,
	                                      IsChecked = (doc.State == CheckState.Checked ? true : false)
	                                  };
	                newItem.Tag = doc;
	                documentsListBox.Items.Add(newItem);
	            }
            }
            EnableDisable();
            selectAlldocuments(true);
        }

        private void GatherUserInput()
        {
            outputDirectory = textboxOutputDirectory.DirectoryName;
        }

        private void EnableDisable()
        {
        	var validOutputdirectory = System.IO.Directory.Exists(textboxOutputDirectory.DirectoryName);
            textboxOutputDirectory.IsEnabled = documentsListBox.Items.Count > 0;
            buttonExport.IsEnabled = ! string.IsNullOrEmpty(textboxOutputDirectory.DirectoryName)
            						&& validOutputdirectory;
            textboxOutputDirectory.Foreground = validOutputdirectory  ? Brushes.Red : Brushes.Black;
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
            var generationContexts = new List<GeneratorContext>();
            foreach (var cDoc in this.documentsListBox.Items
                     					.OfType<CheckBox>()
                     					.Where(x => x.IsChecked.HasValue && x.IsChecked.Value)
                     					.Select(y => y.Tag as cDOC))
            {
                if (cDoc != null && cDoc.State == CheckState.Checked)
                {
                	var generationContext = new GeneratorContext(cctsR, cDoc.TargetNamespace, cDoc.BaseUrn, 
                                                             cDoc.TargetNamespacePrefix, false, true,
                                                             outputDirectory, cDoc.BIV.DocL);
	                generationContext.SchemaAdded += HandleSchemaAdded;
	                generationContexts.Add(generationContext);
                }
            }
           	XSDGenerator.GenerateSchemas(generationContexts);
//            else
//            {
//                SubsetExporter.ExportSubset(docl, originalXMLSchema, outputDirectory);
//            }
            textBoxStatus.Text += "\nGenerating XML schemas completed!";
            Cursor = Cursors.Arrow;
        }

        private void HandleSchemaAdded(object sender, SchemaAddedEventArgs e)
        {
            textBoxStatus.Text += "- " + System.IO.Path.GetFileName(e.FileName) + Environment.NewLine;
            textBoxStatus.ScrollToEnd();
        }

        private void textboxOutputDirectory_DirectoryNameChanged(object sender, RoutedEventArgs e)
        {
        	EnableDisable();
        }

        private void selectAlldocuments(bool isChecked)
        {
        	foreach (CheckBox item in documentsListBox.Items) 
			{
				item.IsChecked = isChecked;
				var doc = item.Tag as cDOC;
				if (doc != null) doc.State = isChecked ? CheckState.Checked : CheckState.Unchecked;
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