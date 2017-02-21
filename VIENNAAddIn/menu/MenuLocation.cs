using System;

namespace VIENNAAddIn.menu
{
    ///<summary>
    /// Enum for EA menu locations.
    ///</summary>
    [Flags]
    public enum MenuLocation
    {
        Undefined = 0,
        MainMenu = 1,
        TreeView = 2,
        Diagram = 4,
    }

    public static class MenuLocationExtensions
    {
        public static bool IsContextMenu(this MenuLocation menuLocation)
        {
            return ((MenuLocation.TreeView | MenuLocation.Diagram) & menuLocation) == menuLocation;
        }
    }
}