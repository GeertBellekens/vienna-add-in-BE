using System.Runtime.InteropServices;
using EA;

namespace VIENNAAddIn
{
    ///<summary>
    ///</summary>
    [Guid("AC600C85-5BFE-45d5-9D5C-EEE1B5BE852B")]
    public interface VIENNAAddInInterface
    {

        /// <summary>
        /// Disconnect
        /// </summary>
        void EA_Disconnect(Repository repository);

        /// <summary>
        /// Open File
        /// </summary>
        /// <param name="repository"></param>
        void EA_FileOpen(Repository repository);

        /// <summary>
        /// Get Menu-Items
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="menuLocation"></param>
        /// <param name="menuName"></param>
        /// <returns></returns>
        string[] EA_GetMenuItems(Repository repository, string menuLocation, string menuName);

        /// <summary>
        /// Menu Click
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="menuLocation"></param>
        /// <param name="menuName"></param>
        /// <param name="menuItem"></param>
        void EA_MenuClick(Repository repository, string menuLocation, string menuName, string menuItem);

        /// <summary>
        /// Menu State
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="menuLocation"></param>
        /// <param name="menuname"></param>
        /// <param name="menuitem"></param>
        /// <param name="IsEnabled"></param>
        /// <param name="IsChecked"></param>
        void EA_GetMenuState(Repository repository, string menuLocation, string menuname, string menuitem,
                             ref bool IsEnabled, ref bool IsChecked);

        bool EA_OnNotifyContextItemModified(EA.Repository repository, string GUID, EA.ObjectType ot);
        void EA_OnContextItemChanged(EA.Repository repository, string GUID, EA.ObjectType ot);
//        bool EA_OnContextItemDoubleClicked(EA.Repository repository, string GUID, EA.ObjectType ot);

        void EA_OnOutputItemClicked(Repository repository, string tabName, string text, int id);
        void EA_OnOutputItemDoubleClicked(Repository repository, string tabName, string text, int id);
        
        bool EA_OnPreNewDiagramObject(Repository repository, EventProperties info);
        bool EA_OnPostNewElement(Repository repository, EventProperties info);
        bool EA_OnPostNewPackage(Repository repository, EventProperties info);

        bool EA_OnPreDeleteElement(Repository repository, EventProperties info);
        bool EA_OnPreDeletePackage(Repository repository, EventProperties info);
    }
}