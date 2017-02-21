/*******************************************************************************
This file is part of the VIENNAAddIn project

Licensed under GNU General Public License V3 http://gplv3.fsf.org/

For further information on the VIENNAAddIn project please visit 
http://vienna-add-in.googlecode.com
*******************************************************************************/
using System;
using System.Windows.Forms;
using EA;
using VIENNAAddIn.menu;
using VIENNAAddIn.WorkFlow;

namespace VIENNAAddIn.workflow
{
    ///<summary>
    ///</summary>
    public partial class InitialPackageStructureCreator : Form
    {
        private readonly Repository repository;

        ///<summary>
        ///</summary>
        ///<param name="repository"></param>
        public InitialPackageStructureCreator(Repository repository)
        {
            InitializeComponent();
            this.repository = repository;
        }

        ///<summary>
        ///</summary>
        public static void ShowForm(AddInContext context)
        {
            new InitialPackageStructureCreator(context.EARepository).Show();
        }

        /// <summary>
        /// Set the name of all vies to the model name
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void useForAllButton_Click(object sender, EventArgs e)
        {
            //Get the text of the model name field
            String s = modelName.Text;

            if (s == null || s.Equals(""))
            {
                MessageBox.Show("Please specificy a model name first.", "Error");
            }
            else
            {
                nameBCV.Text = s;
                nameBIV.Text = s;
                nameBRV.Text = s;
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        //Create the inital package structure
        private void createButton_Click(object sender, EventArgs e)
        {
            //BRV checked?
            if (checkBRV.Checked)
            {
                //Name for BRV set
                if (nameBRV.Text == null || nameBRV.Text.Equals(""))
                {
                    MessageBox.Show("Please select a name for the Business Requirements View.", "Error");
                    return;
                }
            }

            //Name for BCV set?
            if (nameBCV.Text == null || nameBCV.Text.Equals(""))
            {
                MessageBox.Show("Please select a name for the Business Choreography View.", "Error");
                return;
            }

            //Name for BIV set?
            if (nameBIV.Text == null || nameBIV.Text.Equals(""))
            {
                MessageBox.Show("Please select a name for the Business Information View.", "Error");
                return;
            }

            //No errors - Last Check

            DialogResult dr = MessageBox.Show("This will DELETE ALL CONTENTS from current selected model.\n" +
                                              "Are you sure?", "UMMAddIn Question", MessageBoxButtons.YesNoCancel,
                                              MessageBoxIcon.Warning);
            // if yes proceed
            if (dr.Equals(DialogResult.Yes))
            {
                try
                {
                    var msc = new ModelStructureCreator(repository);
                    msc.create(modelName.Text, nameBRV.Text, nameBCV.Text, nameBIV.Text, checkBRV.Checked);
                    Close();
                    return;
                }
                catch (Exception ex)
                {
                    Close();
                    MessageBox.Show(
                        "An error occured during creating the UMM model structure: " + ex.Message + "\n" +
                        ex.InnerException.StackTrace, "UMMAddIn Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (dr.Equals(DialogResult.Cancel))
            {
                Close();
                return;
            }
            else
            {
                return;
            }
        }
    }
}