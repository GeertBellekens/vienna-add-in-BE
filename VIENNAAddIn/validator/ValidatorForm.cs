/*******************************************************************************
This file is part of the VIENNAAddIn project

Licensed under GNU General Public License V3 http://gplv3.fsf.org/

For further information on the VIENNAAddIn project please visit 
http://vienna-add-in.googlecode.com
*******************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using EA;
using VIENNAAddIn.common.logging;
using VIENNAAddIn.ErrorReporter;
using VIENNAAddIn.menu;
using Attribute=EA.Attribute;

namespace VIENNAAddIn.validator
{
    public partial class ValidatorForm : Form
    {
        private static ValidatorForm form;

        //The repository upon which the validator validates
        private readonly Repository repository;

        private String scope;

        //The Validationmessages
        private List<ValidationMessage> validationMessages = new List<ValidationMessage>();

        public ValidatorForm(Repository repository, String scope)
        {
            InitializeComponent();

            this.scope = scope;
            this.repository = repository;

            setStatusText("Selected validation scope: " + getScopeInStatusBar(repository, scope) +
                          " Press Start to invoke a validation run.");

            // initialize progress bar
            progressBar.Maximum = 100;
            progressBar.Minimum = 1;
            progressBar.Value = 1;
            progressBar.Step = 1;

            // initialize timer ...
            progressTimer.Tick += progressTimer_Tick;
        }

        public static void ShowValidator(AddInContext context)
        {
            ShowForm(context, null);
        }

        public static void ShowUmmValidator(AddInContext context)
        {
            ShowForm(context, "ROOT_UMM");
        }

        public static void ShowUpccValidator(AddInContext context)
        {
            ShowForm(context, "ROOT_UPCC");
        }

        private static void ShowForm(AddInContext context, string scope)
        {
            if (String.IsNullOrEmpty(scope))
            {
                scope = DetermineValidationScope(context.EARepository, context.MenuLocation);
            }
            if (scope == "")
            {
                //TO DO - add additional routines here which i.e. try to determine
                //a UPCC validation scope
                MessageBox.Show("Unable to determine a validator for the selected diagram, element or package.");
            }
            else
            {
                if (form == null || form.IsDisposed)
                {
                    form = new ValidatorForm(context.EARepository, scope);
                    form.Show();
                }
                else
                {
                    form.resetValidatorForm(scope);
                    form.Select();
                    form.Focus();
                    form.Show();
                }
            }
        }

        /// <summary>
        /// Depending on the area, where the user clicks "Validate", a different scope of the model
        /// is validated.
        /// For instance, a validation can be restricted to a certain view, or a certain diagram
        /// 
        /// 
        /// Comment by pl:
        /// Clearly the method could have been optimized in terms of eliminating re-occuring code.
        /// However re-occuring code was left for the sake of lucidity.
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="menulocation"></param>
        /// <returns></returns>
        private static String DetermineValidationScope(Repository repository, MenuLocation menulocation)
        {
            String s = "";

            if (menulocation == MenuLocation.TreeView)
            {
                //Get the element in the tree view which was clicked
                Object obj;
                ObjectType otype = repository.GetTreeSelectedItem(out obj);

                //Now otype could be determined - show an error
                if (!Enum.IsDefined(typeof (ObjectType), otype))
                {
                    //Should not occur
                    const string error = "Unable to determine object type of element.";
                    MessageBox.Show(error, "Error");
                }
                    //The user clicked on a package - try to determine the stereotype
                else if (otype == ObjectType.otPackage)
                {
                    var p = (Package) obj;
                    //If the package has no superpackage, it must be the very top package
                    //-> if the very top package is clicked, ALL will be validated
                    bool hasParent = false;
                    try
                    {
                        int dummy = p.ParentID;
                        if (dummy != 0)
                            hasParent = true;
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine("Error checking for superpackage (" + e.Message + ")");
                    }

                    if (!hasParent)
                    {
                        int rootModelPackageID = ((Package) (repository.Models.GetAt(0))).PackageID;
                        s = "" + rootModelPackageID;
                    }
                    else
                    {
                        s = "" + p.PackageID;
                    }
                }
                    //In the treeview apart from a package the user can click on 
                    // an element, a diagram, an attribute or a method
                    //All of these cases are handled here
                else
                {
                    int packageID = 0;

                    if (otype == ObjectType.otElement)
                        packageID = ((Element) obj).PackageID;
                    else if (otype == ObjectType.otDiagram)
                        packageID = ((Diagram) obj).PackageID;
                    else if (otype == ObjectType.otAttribute)
                    {
                        var att = (Attribute) obj;
                        //Get the element that this attribute is part of
                        Element el = repository.GetElementByID(att.ParentID);
                        //Get the package, where this element is located in
                        packageID = el.PackageID;
                    }
                    else if (otype == ObjectType.otMethod)
                    {
                        var meth = (Method) obj;
                        //Get the the element, that this attribute is part of
                        Element el = repository.GetElementByID(meth.ParentID);
                        //Get the package, where this element is located in
                        packageID = el.PackageID;
                    }
                    //Get the package					 
                    Package p = repository.GetPackageByID(packageID);

                    s = "" + p.PackageID;
                }
            }
                //If the users clicks into a diagram we must determine to which package
                //the diagram belongs
            else if (menulocation == MenuLocation.Diagram)
            {
                int packageID = 0;
                try
                {
                    Object obj;
                    ObjectType o = repository.GetContextItem(out obj);
                    if (o == ObjectType.otDiagram)
                        packageID = ((Diagram) obj).PackageID;
                    else if (o == ObjectType.otElement)
                        packageID = ((Element) obj).PackageID;
                }
                catch (Exception e)
                {
                    Debug.Write("Exception while determining Menulocation (" + e.Message + ")");
                }

                if (packageID != 0)
                {
                    //To which package does this diagram belong?
                    Package p = repository.GetPackageByID(packageID);

                    s = "" + p.PackageID;
                }
            }

            return s;
        }

        /// <summary>
        /// Perform a Progress Bar step
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void progressTimer_Tick(object sender, EventArgs e)
        {
            progressBar.PerformStep();
        }

        /// <summary>
        /// Called as soon as the background worker responsible for the validation of the model
        /// has finished
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bworker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // First, handle the case where an exception was thrown.
            // If so, the exception is returned via the Result field
            if (e.Result != null)
            {
                var execption = (Exception) e.Result;

                new ErrorReporterForm(execption.Message + "\n" + execption.StackTrace, repository.LibraryVersion);

                progressBar.Value = 1;
            }
            else if (e.Cancelled)
            {
                // Next, handle the case where the user canceled 
                // the operation.
                // Note that due to a race condition in 
                // the DoWork event handler, the Cancelled
                // flag may not have been set, even though
                // CancelAsync was called.
            }
            else
            {
                progressTimer.Stop();
                progressBar.Value = progressBar.Maximum;

                //Calculate the elapsed time
                //                DateTime endTime = DateTime.Now;
                //double elapsedTime = (endTime.ToFileTime() - startTime.ToFileTime()) / (double)TimeSpan.TicksPerSecond;

                //Set the status message
                setStatusText("Validation complete.");
            }

            // Enable the Start button.
            startButton.Enabled = true;
        }

        /// <summary>
        /// DoWork method of the Background worker - responsible for carrying out the processing time intense
        /// operation of validation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bworker_DoWork(object sender, DoWorkEventArgs e)
        {
            //Deactiveate Start Button
            setStatusText("Validation started. Please wait...");

            validationMessages.Clear();

            var validationContext = new ValidationContext(repository);
            validationContext.ValidationMessageAdded += HandleValidationMessageAdded;
            try
            {
                var validator = new Validator();
                validator.validate(validationContext, scope);
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message, LogType.ERROR);
                // stop worker ...
                bworker.CancelAsync();
                //Set the Excpetion as Result
                e.Result = ex;

                progressTimer.Stop();
            }
        }

        /// <summary>
        /// Handle Message Added Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandleValidationMessageAdded(object sender, ValidationMessageAddedEventArgs e)
        {
            bworker.ReportProgress(0, e);
        }

        /// <summary>
        /// Handle the newly added Validationmessage
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bworker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            var vma = (ValidationMessageAddedEventArgs) e.UserState;

            validationMessages.AddRange(vma.Messages);
            fillListView();
        }

        /// <summary>
        /// Reset the validatorForm
        /// </summary>
        public void resetValidatorForm(String newScope)
        {
            setStatusText("Selected validation scope: " + getScopeInStatusBar(repository, scope));
            scope = newScope;

            //Delete the old validation messages
            foreach (ListViewItem item in validationMessagesListView.Items)
            {
                validationMessagesListView.Items.Remove(item);
            }
            //Reset the progress bar
            progressBar.Value = progressBar.Minimum;
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            //If a top-down validation of the whole model is initialized, the intervall must be higher
            progressTimer.Interval = scope == "ROOT" ? 800 : 400;

            //Set back the list view
            //Delete the old validation messages
            foreach (ListViewItem item in validationMessagesListView.Items)
            {
                validationMessagesListView.Items.Remove(item);
            }
            //Reset the progress bar
            progressBar.Value = progressBar.Minimum;
            //Reset the message detail box
            detailsBox.Clear();

            startButton.Enabled = false;
            progressTimer.Start();

            if (bworker.IsBusy)
                bworker.CancelAsync();

            bworker.RunWorkerAsync(validationMessages);
        }

        /// <summary>
        /// Fill the ListView with the Validationmessagedetails
        /// </summary>
        private void fillListView()
        {
            validationMessagesListView.BeginUpdate();

            //Delete the old validation messages
            foreach (ListViewItem item in validationMessagesListView.Items)
            {
                validationMessagesListView.Items.Remove(item);
            }

            //Iterate over the validation messages generated by the different validatiors
            //and add the message to the grid
            String selectedLevel = "";
            if (levelSelector.SelectedItem != null)
                selectedLevel = levelSelector.SelectedItem.ToString();

            if (validationMessages != null && validationMessages.Count != 0)
            {
                foreach (ValidationMessage message in validationMessages)
                {
                    if (selectedLevel == "" || selectedLevel.ToUpper() == "ALL")
                    {
                        addValidationMessageToList(message);
                    }
                    else if (selectedLevel.ToUpper() == "INFO")
                    {
                        if (message.ErrorLevel == ValidationMessage.errorLevelTypes.INFO)
                            addValidationMessageToList(message);
                    }
                    else if (selectedLevel.ToUpper() == "WARN")
                    {
                        if (message.ErrorLevel == ValidationMessage.errorLevelTypes.WARN)
                            addValidationMessageToList(message);
                    }
                    else if (selectedLevel.ToUpper() == "ERROR")
                    {
                        if (message.ErrorLevel == ValidationMessage.errorLevelTypes.ERROR)
                            addValidationMessageToList(message);
                    }
                }
            }
            else
            {
                //No validation messages returned - the validation was successful
                var successMessage = new ValidationMessage("No errors found.", "No errors were found in your model", "-",
                                                           ValidationMessage.errorLevelTypes.INFO, 0);
                if (validationMessages == null)
                    validationMessages = new List<ValidationMessage>();

                validationMessages.Add(successMessage);
                addValidationMessageToList(successMessage);
            }

            validationMessagesListView.EndUpdate();
        }

        /// <summary>
        /// Add the validationmessage to the validation message listview
        /// </summary>
        /// <param name="message"></param>
        private void addValidationMessageToList(ValidationMessage message)
        {
            //Depending on the warning level get the appropriate color
            Color c = getColor(message.ErrorLevel);

            String msg = message.Message;
            String affectedView = message.AffectedView;
            int msgID = message.MessageID;

            var normalFont = new Font("Tahoma", 8);

            //Add a new ListViewItem with the appropriate format
            //Add the error level
            var item = new ListViewItem(new[] {message.ErrorLevel.ToString()}, 0, Color.Black, c, normalFont)
                           {UseItemStyleForSubItems = false};
            //Add the affectedView
            item.SubItems.Add(affectedView, Color.Black, c, normalFont);
            //Add the message
            item.SubItems.Add(msg, Color.Black, c, normalFont);
            //Add the messageID for later retrieval of information in the detail box
            item.SubItems.Add(msgID.ToString(), Color.Black, c, normalFont);

            validationMessagesListView.Items.Add(item);
        }

        /// <summary>
        /// According to the errorlevel this method returns the backgroundcolor for the 
        /// ListView
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        private static Color getColor(ValidationMessage.errorLevelTypes level)
        {
            Color c = Color.White;

            if (level == ValidationMessage.errorLevelTypes.ERROR)
                c = Color.FromArgb(255, 139, 147);
            else if (level == ValidationMessage.errorLevelTypes.WARN)
                c = Color.FromArgb(255, 255, 204);
            else if (level == ValidationMessage.errorLevelTypes.INFO)
                c = Color.FromArgb(152, 251, 152);

            return c;
        }

        /// <summary>
        /// Shows only the validation messages according to the selected level
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void levelSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Clear the message detail box
            detailsBox.Clear();
            //Refill the list view
            fillListView();
        }

        private void errorLink_MouseClick(object sender, MouseEventArgs e)
        {
        }

        /// <summary>
        /// Set the text of the status bar
        /// </summary>
        /// <param name="s"></param>
        private void setStatusText(String s)
        {
            statusBar.Items["statusLabel"].Text = s;
        }

        private void validationMessagesListView_ItemSelectionChanged_1(object sender,
                                                                       ListViewItemSelectionChangedEventArgs e)
        {
            //Get the message id from the list
            //Through the message id, we can set the text of the detail message
            try
            {
               var s = validationMessagesListView.SelectedItems[0].SubItems[3].Text;

                if (s != null)
                {
                    foreach (ValidationMessage vmsg in validationMessages)
                    {
                        if (vmsg.MessageID.ToString() == s)
                        {
                            detailsBox.Clear();
                            setStatusText("Message selected.");
                            if (string.IsNullOrEmpty(vmsg.MessageDetail))
                            {
                                detailsBox.AppendText("No message detail.");
                            }
                            else
                            {
                                detailsBox.AppendText(vmsg.MessageDetail);
                            }
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        /// <summary>
        /// Return the full package name which is scheduled as root element for the next validation run
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        private static String getScopeInStatusBar(Repository repository, String scope)
        {
            string s;

            if (string.IsNullOrEmpty(scope))
            {
                s = "No scope determined.";
            }
            else if (scope.Equals("ROOT_UMM"))
            {
                s = "Entire UMM model.";
            }
            else if (scope.Equals("ROOT_UCC"))
            {
                s = "Entire UPCC model.";
            }
            else
            {
                try
                {
                    int packageID = Int32.Parse(scope.Trim());
                    Package p = repository.GetPackageByID(packageID);
                    s = "<<" + p.Element.Stereotype + ">> ";
                    s += p.Name;
                }
                catch (Exception)
                {
                    s = "Illegal scope detected. Please restart valdiator.";
                }
            }

            return s;
        }

        private void errorLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            String messageID = "";
            int packageID = 0;
            //Get the message ID of the errorneous element
            try
            {
                messageID = validationMessagesListView.SelectedItems[0].SubItems[3].Text;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }

            if (messageID != null && validationMessages != null)
            {
                foreach (ValidationMessage vmsg in validationMessages)
                {
                    //Get the ID of the erroneous element
                    if (vmsg.MessageID.ToString() == messageID)
                    {
                        packageID = vmsg.AffectedPackageID;
                        break;
                    }
                }

                if (packageID != 0)
                {
                    try
                    {
                        repository.ShowInProjectView(repository.GetPackageByID(packageID));
                        setStatusText("Package containing the error is highlighted in the project explorer.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.StackTrace);
                    }
                }
                else
                {
                    setStatusText("There is no erroneous element associated with this message.");
                }
            }
            else
            {
                setStatusText("There is no erroneous element associated with this message.");
            }
        }
    }
}