using System;
using System.Collections.Generic;
using System.Windows.Forms;
using EA;
using VIENNAAddIn.menu;
using VIENNAAddInUtils;

namespace VIENNAAddIn.Settings
{
    public partial class SynchStereotypesForm : Form
    {
        private static SynchStereotypesForm synchStereotypesForm;
        private readonly SynchTaggedValues synchTaggedValues;

        public SynchStereotypesForm(Repository repository)
        {
            InitializeComponent();

            synchTaggedValues = new SynchTaggedValues(repository);
            synchTaggedValues.TaggedValueFixed += OnTaggedValueFixed;
        }

        private void OnTaggedValueFixed(Path taggedValuePath)
        {
            missingList.Items.Add("Added tagged value '" + taggedValuePath + "'.");
        }

        public static void ShowForm(AddInContext context)
        {
            if (synchStereotypesForm == null || synchStereotypesForm.IsDisposed)
            {
                synchStereotypesForm = new SynchStereotypesForm(context.EARepository);
            }
            else
            {
                synchStereotypesForm.missingList.Items.Clear();
                synchStereotypesForm.Select();
                synchStereotypesForm.Focus();
            }
            synchStereotypesForm.CloseButton.Enabled = false;
            synchStereotypesForm.Show();
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void SynchStereotypesForm_Shown(object sender, EventArgs e)
        {
            missingList.Items.Add("Adding missing tagged values.");
            synchTaggedValues.FixTaggedValues();
            missingList.Items.Add("Done.");
            synchStereotypesForm.CloseButton.Enabled = true;
        }
    }
}
