/*******************************************************************************
This file is part of the VIENNAAddIn project

Licensed under GNU General Public License V3 http://gplv3.fsf.org/

For further information on the VIENNAAddIn project please visit 
http://vienna-add-in.googlecode.com
*******************************************************************************/
using System;
using System.Windows;
using EA;
using VIENNAAddIn.menu;
using VIENNAAddIn.WorkFlow;

namespace VIENNAAddIn.upcc3.Wizards.dev.ui
{
    /// <summary>
    /// Interaction logic for UmmInitialPackageStructureCreator.xaml
    /// </summary>
    public partial class UmmInitialPackageStructureCreator
    {
        private readonly Repository repository;

        public UmmInitialPackageStructureCreator(Repository repository)
        {
            InitializeComponent();
            this.repository = repository;
        }

        public static void ShowForm(AddInContext context)
        {
            new UmmInitialPackageStructureCreator(context.EARepository).Show();
        }


        private void checkBox1_Checked(object sender, RoutedEventArgs e)
        {
            if ((bool)checkBox1.IsChecked)
            {
                var text = modelName.Text;
                if (text == null || text.Equals(""))
                {
                    MessageBox.Show("Please specificy a model name first.", "Error");
                }
                businessChoreographyView.Text = text;
                businessInformationView.Text = text;
                businessRequirementsView.Text = text;
            }
            else
            {
                businessChoreographyView.Text = "";
                businessInformationView.Text = "";
                businessRequirementsView.Text = "";
            }
        }

        private void Button_Close(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_Create(object sender, RoutedEventArgs e)
        {
            if ((bool) BusinessRequirementsViewCheckbox.IsChecked)
            {
                //Name for BRV set
                if (businessRequirementsView.Text == null || businessRequirementsView.Text.Equals(""))
                {
                    MessageBox.Show("Please select a name for the Business Requirements View.", "Error");
                    return;
                }
            }

            //Name for BCV set?
            if (businessChoreographyView.Text == null || businessChoreographyView.Text.Equals(""))
            {
                MessageBox.Show("Please select a name for the Business Choreography View.", "Error");
                return;
            }

            //Name for BIV set?
            if (businessInformationView.Text == null || businessInformationView.Text.Equals(""))
            {
                MessageBox.Show("Please select a name for the Business Information View.", "Error");
                return;
            }

            //No errors - Last Check


            var rsltMessageBox =
                MessageBox.Show("This will DELETE ALL CONTENTS from current selected model.\n" +
                                "Are you sure?", "UMMAddIn Question", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            // Process message box results
            switch (rsltMessageBox)
            {
                case MessageBoxResult.Yes:

                    // User pressed Yes button
                    try
                    {
                        var msc = new ModelStructureCreator(repository);
                        msc.create(modelName.Text, businessRequirementsView.Text, businessChoreographyView.Text,
                                   businessInformationView.Text, (bool) BusinessRequirementsViewCheckbox.IsChecked);
                        Close();
                        return;
                    }
                    catch (Exception ex)
                    {
                        Close();
                        MessageBox.Show(
                            "An error occured during creating the UMM model structure: " + ex.Message + "\n" +
                            ex.InnerException.StackTrace, "UMMAddIn Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    break;

                case MessageBoxResult.No:

                    // User pressed No button
                    Close();
                    break;
            }
        }
    }
}