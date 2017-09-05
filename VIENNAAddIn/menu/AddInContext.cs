using System;
using CctsRepository;
using EA;
using VIENNAAddIn.upcc3;
using Stereotype=VIENNAAddIn.upcc3.Stereotype;

namespace VIENNAAddIn.menu
{
    ///<summary>
    /// Contains the current context of the Add-In. Note that only a subset of the properties is set, depending on which API method that created the context.
    ///</summary>
    public class AddInContext
    {
        public AddInContext(Repository eaRepository, string menuLocation)
        {
            EARepository = eaRepository;

            MenuLocation = (MenuLocation) Enum.Parse(typeof (MenuLocation), menuLocation);

            if (MenuLocation == MenuLocation.TreeView)
            {
                // Workaround to fix problem in Enterprise Architect:
                // The "EA_OnContextItemChanged" method is not invoked in case the user
                // selects a model in the tree view which causes SelectedItem
                // to contain an invalid value. Therefore we override the values of the variables whenever the 
                // user selects a package in the tree view. 
                SelectedItem = EARepository.GetTreeSelectedObject();
            }
            else
            {
                // Cannot simply use EARepository.GetContextObject(), because this method always returns the TreeSelectedObject!
                // (which probably is a bug)
                object contextObject;
                EARepository.GetContextItem(out contextObject);
                SelectedItem = contextObject;
            }
        }
			
        ///<summary>
        /// The EA repository.
        ///</summary>
        public Repository EARepository { get; private set; }

        ///<summary>
        /// The current menu location.
        ///</summary>
        public MenuLocation MenuLocation { get; private set; }

        ///<summary>
        /// A CctsRepository wrapped around the EA repository.
        ///</summary>
        public ICctsRepository CctsRepository
        {
            get { return CctsRepositoryFactory.CreateCctsRepository(EARepository); }
        }

        ///<summary>
        /// The currently selected item.
        ///</summary>
        public object SelectedItem { get; private set; }
        public Package SelectedPackage
        {
        	get
        	{
        		return SelectedItem as Package;
        	}
        }

        public bool SelectedItemIsLibraryOfType(string stereotype)
        {
            return SelectedPackage != null && SelectedPackage.Element != null && SelectedPackage.Element.Stereotype == stereotype;
        }

        public bool SelectedItemIsABIE()
        {
            return (SelectedItem as Element) != null && (SelectedItem as Element).Stereotype == Stereotype.ABIE;
        }
        
        public bool SelectedItemIsDocLibrary()
        {
        	return SelectedPackage != null && SelectedPackage.Element != null && Stereotype.IsDocLibraryStereotype(SelectedPackage.Element.Stereotype);
        }

        public bool SelectedItemIsRootModel()
        {
            return SelectedPackage != null && SelectedPackage.ParentID == 0;
        }
    }
}