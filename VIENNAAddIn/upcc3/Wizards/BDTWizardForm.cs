using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using CctsRepository;
using CctsRepository.BdtLibrary;
using CctsRepository.CdtLibrary;
using VIENNAAddIn.menu;

namespace VIENNAAddIn.upcc3.Wizards
{
    ///<summary>
    ///</summary>
    public partial class BDTWizardForm : Form
    {
        private ICctsRepository repository;
        private Cache cache;
        private string selectedCDTLName;
        private string selectedCDTName;
        private string selectedBDTLName;
        private string selectedSUPName;
        private Label errorMessageBDTName;
        private bool userHasClicked = false;

        private const string CAPTION_ERROR_WINDOW = "BDT Wizard Error";
        private const string CAPTION_INFO_WINDOW = "BDT Wizard";
        private const string DEFAULT_PREFIX = "My";
        
        #region Constructor
        ///<summary>
        ///</summary>
        ///<param name="eaRepository"></param>
        public BDTWizardForm(ICctsRepository cctsRepository)
        {
            InitializeComponent();

            repository = cctsRepository;

            cache = new Cache();

            try
            {
                cache.LoadCDTLs(repository);
                cache.LoadBDTLsAndTheirBDTs(repository);
            }
            catch (CacheException ce)
            {
                InformativeMessage(ce.Message);
                ResetForm(0);
            }
            catch (Exception e)
            {
                CriticalErrorMessage(e.ToString());
                ResetForm(0);
            }
        }
        #endregion

        #region Event Handler Methods

        private void BDTWizardForm_Load(object sender, EventArgs e)
        {
            ResetForm(0);

            MirrorCDTLsToUI();
            MirrorBDTLsToUI();

            SetSafeIndex(comboBDTLs, 0);
            //comboBDTLs.SelectedIndex = 0;

            errorMessageBDTName = new Label
            {
                Size = new Size(100, 17),
                Location = new Point(0, 0),
                BorderStyle = BorderStyle.FixedSingle,
                ForeColor = Color.Black,
                BackColor = Color.Yellow
            };

            groupboxSettings.Controls.Add(errorMessageBDTName);
            errorMessageBDTName.Hide();

            textBDTPrefix.Text = DEFAULT_PREFIX;

            ResetForm(1);
        }

        private void comboCDTLs_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                GatherUserInput();

                if (cache.PathIsValid(CacheConstants.PATH_CDTs, new[] { selectedCDTLName }))
                {
                    cache.CDTLs[selectedCDTLName].LoadCDTs(repository);

                    MirrorCDTsToUI();

                    ResetForm(2);
                }

            }
            catch (CacheException ce)
            {
                InformativeMessage(ce.Message);
                ResetForm(0);
            }            
        }

        private void GatherUserInput()
        {
            selectedCDTLName = comboCDTLs.SelectedIndex >= 0 ? comboCDTLs.SelectedItem.ToString() : "";
            selectedCDTName = comboCDTs.SelectedIndex >= 0 ? comboCDTs.SelectedItem.ToString() : "";
            selectedBDTLName = comboBDTLs.SelectedIndex >= 0 ? comboBDTLs.SelectedItem.ToString() : "";                     
            selectedSUPName = checkedlistboxSUPs.SelectedIndex >= 0 ? checkedlistboxSUPs.SelectedItem.ToString() : "";
        }

        private void MirrorCDTLsToUI()
        {
            comboCDTLs.Items.Clear();

            foreach (cCDTLibrary cdtl in cache.CDTLs.Values)
            {
                comboCDTLs.Items.Add(cdtl.Name);
            }
        }

        private void MirrorCDTsToUI()
        {
            //GatherUserInput();
            
            comboCDTs.Items.Clear();

            foreach (cCDT cdt in cache.CDTLs[selectedCDTLName].CDTs.Values)
            {
                comboCDTs.Items.Add(cdt.Name);
            }
        }

        private void MirrorBDTLsToUI()
        {
            comboBDTLs.Items.Clear();

            foreach (cBDTLibrary bdtl in cache.BDTLs.Values)
            {
                comboBDTLs.Items.Add(bdtl.Name);
            }
        }

        private void MirrorAttributesToUI()
        {
            GatherUserInput();

            checkedlistboxCON.Items.Clear();
            checkedlistboxSUPs.Items.Clear();
            
            if (cache.PathIsValid(CacheConstants.PATH_CDTs, new[] {selectedCDTLName, selectedCDTName}))
            {
                // Set the checkbox Status whether all CON and SUPs are selected or not
                // bug: currently disabled
                //userHasClicked = false;
                //checkboxAttributes.CheckState = cache.CDTLs[selectedCDTLName].CDTs[selectedCDTName].AllSUPs;

                // Write the CON attribute of the CDT to the User Interface.
                cCDT currentCDT = cache.CDTLs[selectedCDTLName].CDTs[selectedCDTName];
                checkedlistboxCON.Items.Add(currentCDT.CON.Name, currentCDT.CON.State);

                // Write all SUP attributes of the CDT to the User Interface.
                foreach (cSUP sup in currentCDT.SUPs.Values)
                {
                    checkedlistboxSUPs.Items.Add(sup.Name, sup.State);
                }                                
            }
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void comboCDTs_SelectionChangeCommitted(object sender, EventArgs e)
        {
            GatherUserInput();

            try
            {
                if (cache.PathIsValid(CacheConstants.PATH_CDTs, new[] {selectedCDTLName, selectedCDTName}))
                {
                    cache.CDTLs[selectedCDTLName].CDTs[selectedCDTName].LoadCONAndSUPs(repository);
                }

                MirrorAttributesToUI();

                ResetForm(3);

                textBDTName.Text = textBDTPrefix.Text + selectedCDTName;
            }
            catch (CacheException ce)
            {
                InformativeMessage(ce.Message);
            }
        }

        private void checkboxAttributes_CheckedChanged(object sender, EventArgs e)
        {
            GatherUserInput();

            if (cache.PathIsValid(CacheConstants.PATH_CDTs, new[] {selectedCDTLName, selectedCDTName}))
            {
                // Determine the status of the checkbox since all items in the checked 
                // listbox will be assigned the new value.
                CheckState newState = CheckState.Unchecked;                               

                if (checkboxAttributes.Checked)
                {
                    newState = CheckState.Checked;
                }

                // Unluckily, the current event CheckedChanged is also triggered
                // if the status of the checkbox is set programmatically. Therefore, we
                // introduced a boolean variable named userHasClicked which is only set to 
                // true in case the user clicked the checkbox. The userHasClicked Variable
                // is therefore set to true by the event MouseDown of the checkbox control 
                // checkboxAttributes. 
                if (userHasClicked)
                {
                    foreach (KeyValuePair<string, cSUP> sup in cache.CDTLs[selectedCDTLName].CDTs[selectedCDTName].SUPs)
                    {
                        sup.Value.State = newState;
                    }
                }

                // Furthermore, the new check state of the checkbox needs to be stored
                // in the cache.
                // todo: bug - currently disabled
                //cache.CDTLs[selectedCDTLName].CDTs[selectedCDTName].AllSUPs = newState;

                MirrorAttributesToUI();
            }
        }

        private void buttonGenerateBDT_Click(object sender, EventArgs e)
        {
            GatherUserInput();

            if ((cache.PathIsValid(CacheConstants.PATH_CDTs, new[] {selectedCDTLName, selectedCDTName})) &&
                (cache.PathIsValid(CacheConstants.PATH_BDTLs, new[] {selectedBDTLName})))
            {
                ICdt cdt = repository.GetCdtById(cache.CDTLs[selectedCDTLName].CDTs[selectedCDTName].Id);
                IBdtLibrary bdtl = repository.GetBdtLibraryById(cache.BDTLs[selectedBDTLName].Id);

                BdtSpec bdtSpec = BdtSpec.CloneCdt(cdt, textBDTName.Text);

                var sups = new List<BdtSupSpec>(bdtSpec.Sups);
                foreach (cSUP sup in cache.CDTLs[selectedCDTLName].CDTs[selectedCDTName].SUPs.Values)
                {
                    if (sup.State == CheckState.Unchecked)
                    {
                        var name = sup.Name;
                        sups.RemoveAll(s => s.Name == name);
                    }
                }
                bdtSpec.Sups = sups;

                IBdt newBDT = bdtl.CreateBdt(bdtSpec);

                cache.BDTLs[selectedBDTLName].BDTs.Add(newBDT.Name, new cBDT(newBDT.Name, newBDT.Id, newBDT.BasedOn.Id, CheckState.Unchecked));

                textBDTName.Text = "";
                textBDTName.Text = newBDT.Name;
            }            
        }

        private void textBDTName_TextChanged(object sender, EventArgs e)
        {
            GatherUserInput();

            if (cache.PathIsValid(CacheConstants.PATH_BDTLs, new[]{selectedBDTLName}))
            {
                if (cache.BDTLs[selectedBDTLName].BDTs.ContainsKey(textBDTName.Text))
                {
                    errorMessageBDTName.Location = new Point(textBDTName.Location.X + textBDTName.Width - 105, textBDTName.Location.Y);
                    errorMessageBDTName.Text = "BDT named \"" + textBDTName.Text + "\" alreay exists!";
                    errorMessageBDTName.BringToFront();
                    errorMessageBDTName.Show();
                    ResetForm(3);
                }
                else
                {
                    errorMessageBDTName.Hide();
                    ResetForm(4);
                }
            }

            if (string.IsNullOrEmpty(textBDTName.Text))
            {
                ResetForm(3);
            }
        }

        private void checkedlistboxSUPs_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            GatherUserInput();

            if (cache.PathIsValid(CacheConstants.PATH_CDTs, new[] { selectedCDTLName, selectedCDTName, selectedSUPName}))
            {
                cache.CDTLs[selectedCDTLName].CDTs[selectedCDTName].SUPs[selectedSUPName].State = e.NewValue;

                if (e.NewValue == CheckState.Unchecked)
                {
                    userHasClicked = false;
                    checkboxAttributes.CheckState = CheckState.Unchecked;

                    // bug - currently disabled
                    //// store in the cache
                    //cache.CDTLs[selectedCDTLName].CDTs[selectedCDTName].AllSUPs = e.NewValue;                    
                }                
            }
        }

        #endregion

        #region Convenience Methods

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

        private static void CriticalErrorMessage(string errorMessage)
        {
            MessageBox.Show(errorMessage, CAPTION_ERROR_WINDOW, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private static void InformativeMessage(string infoMessage)
        {
            MessageBox.Show(infoMessage, CAPTION_INFO_WINDOW, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ResetForm(int level)
        {
            switch (level)
            {
                case 0:
                    comboCDTLs.DropDownStyle = ComboBoxStyle.DropDownList;
                    comboCDTs.DropDownStyle = ComboBoxStyle.DropDownList;
                    comboBDTLs.DropDownStyle = ComboBoxStyle.DropDownList;

                    tabcontrolAttributes.Enabled = false;
                    textBDTPrefix.Enabled = false;
                    textBDTName.Enabled = false;
                    comboCDTLs.Enabled = false;
                    comboCDTs.Enabled = false;
                    comboBDTLs.Enabled = false;
                    buttonGenerateBDT.Enabled = false;
                    break;

                case 1:
                    comboCDTLs.Enabled = true;
                    break;

                case 2:
                    comboCDTs.Enabled = true;
                    break;

                case 3:
                    tabcontrolAttributes.Enabled = true;
                    textBDTPrefix.Enabled = true;
                    textBDTName.Enabled = true;
                    comboBDTLs.Enabled = true;
                    buttonGenerateBDT.Enabled = false;
                    break;

                case 4:
                    buttonGenerateBDT.Enabled = true;
                    break;
            }
        }

        #endregion

        private void BDTWizardForm_SizeChanged(object sender, EventArgs e)
        {
            errorMessageBDTName.Location = new Point(textBDTName.Location.X + textBDTName.Width - 105, textBDTName.Location.Y);
        }

        public static void ShowBDTWizard(AddInContext context)
        {
            new BDTWizardForm(context.CctsRepository).Show();
        }

        private void checkboxAttributes_MouseDown(object sender, MouseEventArgs e)
        {
            userHasClicked = true;
        }
    }
}
