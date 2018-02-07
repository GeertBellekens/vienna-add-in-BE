/*******************************************************************************
This file is part of the VIENNAAddIn project

Licensed under GNU General Public License V3 http://gplv3.fsf.org/

For further information on the VIENNAAddIn project please visit 
http://vienna-add-in.googlecode.com
*******************************************************************************/
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Input;
using EA;
using VIENNAAddIn.ErrorReporter;
using VIENNAAddIn.menu;
using VIENNAAddIn.Settings;
using VIENNAAddIn.upcc3.otf;
using VIENNAAddIn.upcc3.Wizards.dev.ui;
using VIENNAAddIn.Utils;
using VIENNAAddIn.validator;
using Cursors=System.Windows.Input.Cursors;
using MenuItem=VIENNAAddIn.menu.MenuItem;
using Stereotype=VIENNAAddIn.upcc3.Stereotype;

namespace VIENNAAddIn
{
    ///<summary>
    ///</summary>
    [Guid("ADFF62A3-BEB5-4f39-9F79-560989B6E48B"),
     ClassInterface(ClassInterfaceType.None),
     ComSourceInterfaces(typeof (VIENNAAddInEvents))]
    public class VIENNAAddIn : VIENNAAddInInterface
    {
        private static Repository Repo;
        private readonly MenuManager menuManager;
        private AddInContext context;
//        private ValidatingCCRepository validatingCCRepository;
        private const bool AllowDeletion = true;
        private const bool ItemNotUpdated = false;

        ///<summary>
        ///</summary>
        public VIENNAAddIn()
        {
            menuManager = new MenuManager
                          {
                              DefaultMenuItems = new[] {AddInSettings.AddInName},
                              DefaultEnabled = IfRepositoryIsUmm2Model,
                              DefaultChecked = Never
                          };

            MenuAction createUPCCStructure =
                "&Create initial UPCC3 model structure".OnClick(UpccModelCreator.ShowForm);
            MenuAction createABIE = "Create new &ABIE".OnClick(AbieEditor.ShowCreateDialog);
            MenuAction createBDT = "Create new BD&T".OnClick(BdtEditor.ShowCreateDialog);            
            MenuItem modifyAbie = "&Modify ABIE".OnClick(AbieEditor.ShowUpdateDialog).Enabled(IfABIEIsSelected);
            MenuAction validate = "&Validate".OnClick(ValidatorForm.ShowValidator);
            MenuItem _____ = MenuItem.Separator;

            menuManager.AddMenu(MenuLocation.MainMenu
                                + (AddInSettings.AddInName
//                                   + "&Set Model as UMM2/UPCC3 Model"
//                                         .OnClick(ToggleUmm2ModelState)
//                                         .Checked(IfRepositoryIsUmm2Model)
//                                         .Enabled(Always)
//                                   + createUPCCStructure
//                                   + "&Create initial UMM 2 model structure".OnClick(UmmInitialPackageStructureCreator.ShowForm)
//                                   + _____
//                                   + "&Validate All - UPCC3".OnClick(ValidatorForm.ShowUpccValidator)
//                                   + "&Validate All - UMM2".OnClick(ValidatorForm.ShowUmmValidator)
//                                   + _____
//                                   + ("Maintenance"
//                                      + "Synchronize &Tagged Values...".OnClick(SynchStereoTypes.ShowForm)
//                                      // TODO: revert this to reenable UML Class Customizer
//                                      //+ "Set Stereo Types...".OnClick(StereoTypeTransformer.ShowForm)
//                                     )
//                                   + ("Wizards"
//                                      + createABIE
//                                      + modifyAbie
//                                      + createBDT
                                      // TODO: revert this to reenable UML Class Customizer
                                      //+ "&Uml Class Customizer".OnClick(UmlClassCustomizer.ShowCreateDialog)
                                      //+ "Generate &XML Schema (old)".OnClick(GeneratorWizardForm.ShowGeneratorWizard)
//                                      + "&Generate XML Schema".OnClick(ExporterForm.ShowForm)
//                                      + "&Import XML Schemas".OnClick(XsdImporterForm.ShowForm)
//                                      + "&XSLT Generator".OnClick(XsltGeneratorForm.ShowForm)
//                                      //+ "Import XML Schemas (old)".OnClick(ImporterWizardFormOld.ShowImporterWizard)
//                                      + _____
//                                      + "&Transformer Wizard".OnClick(TransformerWizard.ShowForm)
//                                      + "&Sub&setting Wizard".OnClick(SubSettingWizard.ShowForm)
//                                      + "&Schema Anal&yzer".OnClick(SchemaAnalyzer.ShowForm)
//                                      + "&WSDL Generator".OnClick(WSDLGenerator.ShowForm)
//                                      )
//                                   + "&Options".OnClick(OptionsForm.ShowForm)
                                   + ("&About " + AddInSettings.AddInName).OnClick(AboutWindow.ShowForm)));
            menuManager.AddMenu((MenuLocation.TreeView | MenuLocation.Diagram)
                                + (AddInSettings.AddInName
                                   + "&Generate XML Schema".OnClick(ExporterForm.ShowForm)))
                .ShowIf(context => context.SelectedPackage != null);
//            //menuManager.AddMenu((MenuLocation.TreeView | MenuLocation.Diagram)
//            //                    + (AddInSettings.AddInName
//            //                       + validate
//                                   + "Import Standard CC Libraries".OnClick(StandardLibraryImporter.ShowForm)))
//                .ShowIf(context => context.SelectedItemIsLibraryOfType(Stereotype.bLibrary));
//            menuManager.AddMenu((MenuLocation.TreeView | MenuLocation.Diagram)
//                                + (AddInSettings.AddInName
//                                   + createABIE
//                                   + validate))
//                .ShowIf(context => context.SelectedItemIsLibraryOfType(Stereotype.BIELibrary));
//            menuManager.AddMenu((MenuLocation.TreeView | MenuLocation.Diagram)
//                                + (AddInSettings.AddInName
//                                   + modifyAbie))
//                .ShowIf(context => context.SelectedItemIsABIE());
//            menuManager.AddMenu((MenuLocation.TreeView | MenuLocation.Diagram)
//                                + (AddInSettings.AddInName
//                                   + createBDT
//                                   + validate))
//                .ShowIf(context => context.SelectedItemIsLibraryOfType(Stereotype.BDTLibrary));
//            menuManager.AddMenu((MenuLocation.TreeView | MenuLocation.Diagram)
//                                + (AddInSettings.AddInName
//                                   + createUPCCStructure))
//                .ShowIf(context => context.SelectedItemIsRootModel());
//            menuManager.AddMenu((MenuLocation.TreeView | MenuLocation.Diagram)
//                                + (AddInSettings.AddInName
//                                   + validate));
        }

        #region VIENNAAddInInterface Members

        /// <summary>
        /// Get menu state
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="menuLocation"></param>
        /// <param name="menuName"></param>
        /// <param name="menuItem"></param>
        /// <param name="isEnabled"></param>
        /// <param name="isChecked"></param>
        public void EA_GetMenuState(Repository repository, string menuLocation, string menuName, string menuItem,
                                    ref bool isEnabled, ref bool isChecked)
        {
            if (Repo == null)
            {
                isEnabled = false;
                isChecked = false;
            }
            else
            {
                menuManager.GetMenuState(context, menuName, menuItem, ref isEnabled, ref isChecked);
            }
        }

        ///<summary>
        ///</summary>
        ///<param name="repository"></param>
        ///<param name="GUID"></param>
        ///<param name="ot"></param>
        ///<returns></returns>
        public bool EA_OnNotifyContextItemModified(Repository repository, string GUID, ObjectType ot)
        {
//            validatingCCRepository.LoadItemByGUID(ot, GUID);
            return true;
        }


        /// <summary>
        /// Disconnect
        /// </summary>
        public void EA_Disconnect(Repository repository)
        {
            repository.CloseAddins();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        /// <summary>
        /// EA File Open
        /// </summary>
        /// <param name="repository"></param>
        public void EA_FileOpen(Repository repository)
        {
            Repo = repository;
            Repo.EnableCache = true;
            repository.CreateOutputTab(AddInSettings.AddInName);
//            validatingCCRepository = new ValidatingCCRepository(repository);
//            validatingCCRepository.ValidationIssuesUpdated += ValidationIssuesUpdated;
//            validatingCCRepository.LoadRepositoryContent();
        }

        private static void ValidationIssuesUpdated(IEnumerable<ValidationIssue> validationIssues)
        {
            Repo.ClearOutput(AddInSettings.AddInName);
            foreach (var issue in validationIssues)
            {
                Repo.WriteOutput(AddInSettings.AddInName, "ERROR: " + issue.ConstraintViolation.Message, issue.Id);
            }
            Repo.EnsureOutputVisible(AddInSettings.AddInName);
        }

        /// <summary>
        /// EA - get menu items
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="menuLocation"></param>
        /// <param name="menuName"></param>
        /// <returns></returns>
        public string[] EA_GetMenuItems(Repository repository, string menuLocation, string menuName)
        {
        	
            try
            {
                if (string.IsNullOrEmpty(menuName)
            	    || (this.context != null 
            	        && !this.context.MenuLocation.ToString().Equals(menuLocation)))
                {
                    // this is the first (top-level) invocation of this method for the current mouse click
                    context = new AddInContext(repository, menuLocation);
                }
                return menuManager.GetMenuItems(context, menuName);
            }
            catch (Exception e)
            {
                new ErrorReporterForm(e + "\n" + e.Message + "\n" + e.StackTrace, Repo.LibraryVersion);
                if (menuLocation == AddInSettings.AddInName)
                {
                    return new string[0];
                }
                return new[] {AddInSettings.AddInName};
            }
        }

        /// <summary>
        /// EA menu click
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="menuLocation"></param>
        /// <param name="menuName"></param>
        /// <param name="menuItem"></param>
        public void EA_MenuClick(Repository repository, string menuLocation, string menuName, string menuItem)
        {
            try
            {
                menuManager.MenuClick(context, menuName, menuItem);
            }
            catch (Exception e)
            {
                new ErrorReporterForm(e.Message + "\n" + e.StackTrace, Repo.LibraryVersion);
            }
        }

        ///<summary>
        ///</summary>
        ///<param name="repository"></param>
        ///<param name="GUID"></param>
        ///<param name="ot"></param>
        ///<returns></returns>
        public void EA_OnContextItemChanged(Repository repository, string GUID, ObjectType ot)
        {
        }

        ///<summary>
        ///</summary>
        ///<param name="repository"></param>
        ///<param name="tabName"></param>
        ///<param name="text"></param>
        ///<param name="id"></param>
        public void EA_OnOutputItemClicked(Repository repository, string tabName, string text, int id)
        {
//            if (tabName == AddInSettings.AddInName)
//            {
//                var issue = validatingCCRepository.GetValidationIssue(id);
//                if (issue != null)
//                {
//                    var itemId = issue.ConstraintViolation.OffendingItemId;
//                    object item;
//                    if (itemId.Type == ItemId.ItemType.Package)
//                    {
//                        item = repository.GetPackageByID(itemId.Value);
//                    }
//                    else
//                    {
//                        item = repository.GetElementByID(itemId.Value);
//                    }
//                    repository.ShowInProjectView(item);
//                }
//            }
        }

        ///<summary>
        ///</summary>
        ///<param name="repository"></param>
        ///<param name="tabName"></param>
        ///<param name="text"></param>
        ///<param name="id"></param>
        public void EA_OnOutputItemDoubleClicked(Repository repository, string tabName, string text, int id)
        {
        }

        public bool EA_OnPreNewDiagramObject(Repository repository, EventProperties info)
        {
//            var elementId = 0;
//            var diagramId = 0;
//            Element element;
//            foreach (EventProperty eventProperty in info)
//            {
//                    if (eventProperty.Name == "ID")
//                    {
//                        elementId = int.Parse(eventProperty.Value.ToString());
//
//                    }
//                    if (eventProperty.Name == "DiagramID")
//                    {
//                        diagramId = int.Parse(eventProperty.Value.ToString());
//                    }
//            }
//
//            try
//            {
//            element = repository.GetElementByID(elementId);
//            }
//                catch(Exception)
//                {
//                    //if we don't get any ID the new Diagram Object seems to be created from the toolbox, that's ok so we just allow it
//                    return true;
//                }
//            var diagram = repository.GetDiagramByID(diagramId);
//            var diagramLibrary = repository.GetPackageByID(diagram.PackageID);
//            var elementLibrary = repository.GetPackageByID(element.PackageID);
//            if (element.Stereotype == "ACC" && diagramLibrary.StereotypeEx =="BIELibrary")
//            {
//                new AbieEditor(elementLibrary.Name, element.Name, diagramLibrary.Name, diagramId, repository).ShowDialog();
//                return false;
//            }
//            if(element.Stereotype == "CDT" && diagramLibrary.StereotypeEx =="BDTLibrary")
//            {
//                new BdtEditor(elementLibrary.Name, element.Name, diagramLibrary.Name, diagramId, repository).ShowDialog();
//                return false;
//            }
            return true;
        }

        ///<summary>
        ///</summary>
        ///<param name="repository"></param>
        ///<param name="info"></param>
        ///<returns></returns>
        public bool EA_OnPostNewElement(Repository repository, EventProperties info)
        {
//            foreach (EventProperty eventProperty in info)
//            {
//                if (eventProperty.Name == "ElementID")
//                {
//                    int elementId = int.Parse(eventProperty.Value.ToString());
//                    validatingCCRepository.LoadElementByID(elementId);
//                    return ItemNotUpdated;
//                }
//            }
            return ItemNotUpdated;
        }

        ///<summary>
        ///</summary>
        ///<param name="repository"></param>
        ///<param name="info"></param>
        ///<returns></returns>
        public bool EA_OnPostNewPackage(Repository repository, EventProperties info)
        {
//            foreach (EventProperty eventProperty in info)
//            {
//                if (eventProperty.Name == "PackageID")
//                {
//                    int packageId = int.Parse(eventProperty.Value.ToString());
//                    validatingCCRepository.LoadPackageByID(packageId);
//                    return ItemNotUpdated;
//                }
//            }
            return ItemNotUpdated;
        }

        public bool EA_OnPreDeleteElement(Repository repository, EventProperties info)
        {
//            foreach (EventProperty eventProperty in info)
//            {
//                if (eventProperty.Name == "ElementID")
//                {
//                    int elementId = int.Parse(eventProperty.Value.ToString());
//                    validatingCCRepository.ItemDeleted(ItemId.ForElement(elementId));
//                    return AllowDeletion;
//                }
//            }
            return AllowDeletion;
        }

        public bool EA_OnPreDeletePackage(Repository repository, EventProperties info)
        {
//            foreach (EventProperty eventProperty in info)
//            {
//                if (eventProperty.Name == "PackageID")
//                {
//                    int packageId = int.Parse(eventProperty.Value.ToString());
//                    validatingCCRepository.ItemDeleted(ItemId.ForPackage(packageId));
//                    return AllowDeletion;
//                }
//            }
            return AllowDeletion;
        }

        #endregion

        #region AddInContext Predicates

        private static bool IfABIEIsSelected(AddInContext context)
        {
            return context.SelectedItemIsABIE();
        }

        private static bool IfRepositoryIsUmm2Model(AddInContext context)
        {
            return context.EARepository.IsUmm2Model();
        }

        private static bool Always(AddInContext context)
        {
            return true;
        }

        private static bool Never(AddInContext context)
        {
            return false;
        }

        #endregion

        private static void ToggleUmm2ModelState(AddInContext context)
        {
            context.EARepository.ToggleUmm2ModelState();
        }
    }
}