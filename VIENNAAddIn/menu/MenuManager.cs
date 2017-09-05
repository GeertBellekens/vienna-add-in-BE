using System;
using System.Collections.Generic;

namespace VIENNAAddIn.menu
{
    ///<summary>
    /// <para>
    /// The MenuManager handles all menu-related EA-Add-In events. The main objective for creating this 
    /// class was to have a way to specify menu structures in a hierarchical and concise way while minimizing 
    /// code duplication. Also, the EA Menu API is a bit complicated (and perhaps even a bit buggy), so that
    /// it is not exactly trivial to handle the events correctly.
    /// </para>
    /// 
    /// <hr/>
    /// 
    /// <para>
    /// <b>The EA Menu API:</b>
    /// </para>
    /// 
    /// <para>
    /// This section briefly describes the EA Menu API so that the knowledge does not get lost. The API consist of 
    /// three events, that invoke the following functions:
    /// </para>
    /// <code>
    /// string[] EA_GetMenuItems(Repository repository, string menuLocation, string menuName);
    /// void     EA_MenuClick   (Repository repository, string menuLocation, string menuName, string menuItem);
    /// void     EA_GetMenuState(Repository repository, string menuLocation, string menuName, string menuItem, ref bool IsEnabled, ref bool IsChecked);
    /// </code>
    /// 
    /// <para>
    /// The first argument always contains a reference to the EA repository.
    /// </para>
    /// <para>
    /// The second argument specifies the menu location, which is one of {"MainMenu", "TreeView", "Diagram"} (see <see cref="MenuLocation"/>).
    /// Now this part seems to be a bit buggy, since the menu location is only correct for the top level element of a menu tree, but this is handled
    /// by the implementation of the MenuManager.
    /// </para>
    /// <para>
    /// The remaining arguments have different meanings for the three events and will be explained later.
    /// </para>
    /// <para>
    /// So the basic question is: How does EA use these events to work with add-in menus? I will try to explain this by listing the steps for displaying
    /// a menu and (possibly) clicking one of the menu items. The steps are always the same and the MenuManager handles the events according to these steps.
    /// 
    /// <list type="number">
    /// <item>
    /// When the user clicks on the "Add-Ins" menu item in the main menu bar or right-clicks a repository element (such as a package or class) elsewhere 
    /// in the user interface, EA invokes EA_GetMenuItems with the correct menu location and an empty menuname. The function must return the top-level menu items
    /// of the add-in (in our case, a single item "VIENNAAddIn").
    /// </item>
    /// <item>
    /// <para>
    /// Then EA invokes the function again for each sub-menu, providing the sub-menu name as an argument 
    /// and expecting the items of the sub-menu as return value. As noted above, the menu location for these invocations is not correct (it seems to always be "Diagram", 
    /// even if the correct location is "MainMenu" or "TreeView"). In this way, EA constructs the entire menu tree at the time the user clicks on the main menu or 
    /// repository element (i.e. it does not wait to build sub-menus only when the user points to them).
    /// </para>
    /// <para>
    /// At some point during this process, EA also invokes EA_GetMenuState for each menu item in order to determine whether the item is enabled and/or checked. Note that
    /// sub-menus cannot be disabled or checked (only click-able items). I have not bothered to find out the exact times when this function is invoked.
    /// </para>
    /// </item>
    /// <item>
    /// If the user dismisses the menu (by clicking elsewhere), no further events are generated.
    /// </item>
    /// <item>
    /// If the user clicks on a menu item (other than a sub-menu), EA invokes EA_MenuClick. Again, the menu location is only correct for the top-level items. The menu name is 
    /// either empty or the name of the sub-menu wherein the menu item resides. The menu item is the name of the clicked item. Note that this implies that the combination of
    /// sub-menu name and menu item name must be unique within a given menu hierarchy, because it is the only information we have to decide which action to execute.
    /// </item>
    /// <item>
    /// The whole process begins again when the user clicks again on the main menu or right-clicks a repository element.
    /// </item>
    /// </list>
    /// </para>
    /// 
    /// <para>
    /// <b>Consequences for the implementation:</b> This behavior has the following consequences for the implementation of the menu manager:
    /// <list type="bullet">
    /// <item>
    /// The first (top-level) invocation of EA_GetMenuItems must be used to select the appropriate menu tree. This selection can depend on the menu location and on arbitrary predicates that
    /// can be defined or information in the repository (e.g. we can define a special menu for right-click context menus for BIE libraries in the tree view). We can recognize that
    /// an invocation is the first invocation, by checking whether the menu name is empty.
    /// </item>
    /// <item>
    /// All other invocations of the menu API operate on the menu tree selected in that first invocation. There is no way around this, since the menu location is not correct for these
    /// subsequent invocations. Also we can improve performance be evaluating the predicates only once for an entire menu tree.
    /// </item>
    /// <item>
    /// The selected menu tree is valid until the next top-level invocation of EA_GetMenuItems.
    /// </item>
    /// </list>
    /// </para>
    /// 
    /// <hr/>
    /// 
    /// <para>
    /// <b>Defining menu structures:</b>
    /// </para>
    /// 
    /// <para>
    /// A menu consists of a <b>menu location</b>, <b>actions</b>, <b>sub-menus</b> and <b>separators</b>.
    /// </para>
    /// 
    /// <para>
    /// <b>Actions</b> can be defined with the <see cref="MenuStringExtensions.OnClick"/> string extension method 
    /// (or with <see cref="MenuAction"/>'s constructor). The following example defines an action
    /// with the name "Open":
    /// 
    /// <code>
    /// var action = "Open".OnClick(OpenFile)
    /// 
    /// ...
    /// 
    /// public void OpenFile(AddInContext context)
    /// {
    ///     // display open file dialog
    /// }
    /// </code>
    /// <br/>
    /// 
    /// As shown in the example, the argument of the OnClick() method is a method (or delegate) that has one argument of 
    /// type <see cref="AddInContext"/>.
    /// </para>
    /// 
    /// <para>
    /// At runtime, an action can be either <b>enabled or disabled</b>. This is determined via
    /// a delegate that is set with the <see cref="MenuAction.Enabled"/> method. In the following example, we make sure
    /// that the "File"/"Close" menu action is only enabled if a file is currently open:
    /// 
    /// <code>
    /// var action = "Close".OnClick(CloseFile).Enabled(IfAFileIsOpen)
    /// 
    /// ...
    /// 
    /// public bool IfAFileIsOpen(AddInContext context)
    /// {
    ///   return openFile != null;
    /// }
    /// </code>
    /// <br/>
    /// 
    /// As shown in the example, the argument of the Enabled() method is a method (or delegate) that has one argument of 
    /// type <see cref="AddInContext"/> and returns a <c>bool</c> (i.e. it is a predicate).
    /// </para>
    /// 
    /// <para>
    /// Note that it is possible to set a default delegate for Enabled(), which will be applied to all menu actions that
    /// where no specific delegate has been set (see default settings below).
    /// </para>
    /// 
    /// <para>
    /// Similar to the enabled/disabled state, an action can be either <b>checked or unchecked</b>. This is 
    /// determined via a delegate that is set with the <see cref="MenuAction.Checked"/> method, as can be seen in the following example:
    /// 
    /// <code>
    /// var action = "Allow multiple open files".OnClick(ToggleMultipleOpenFilesAllowed).Checked(IfMultipleOpenFilesAllowed)
    /// 
    /// ...
    /// 
    /// public void ToggleMultipleOpenFilesAllowed(AddInContext context)
    /// {
    ///   multipleOpenFilesAllowed = !multipleOpenFilesAllowed;
    /// }
    /// 
    /// public bool IfMultipleOpenFilesAllowed(AddInContext context)
    /// {
    ///   return multipleOpenFilesAllowed;
    /// }
    /// </code>
    /// <br/>
    /// 
    /// As shown in the example, the argument of the Checked() method is a method (or delegate) that has one argument of 
    /// type <see cref="AddInContext"/> and returns a <c>bool</c> (i.e. it is a predicate).
    /// </para>
    /// 
    /// <para>
    /// Note that it is possible to set a default delegate for Checked(), which will be applied to all menu actions that
    /// where no specific delegate has been set (see default settings below).
    /// </para>
    /// 
    /// <para>
    /// <b>Sub-Menus</b> are defined using a special syntax (based on overloading the "+" operator). For example, let's define
    /// a "File" sub-menu:
    /// 
    /// <code>
    /// var subMenu = ("File" 
    ///             + "New".OnClick(CreateFile)
    ///             + "Open".OnClick(OpenFile)
    ///             + "Close".OnClick(CloseFile)
    ///             + "Exit".OnClick(Exit)
    ///            );
    /// </code>
    /// <br/>
    /// The first string becomes the sub-menu's name. Menu items are then added with the "+" operator. Note that the parenthesis
    /// could be omitted in this case.
    /// </para>
    /// 
    /// <para>
    /// Sub-menus can also be nested, as shown by the following example (in this case, the nested menu must be 
    /// enclosed in parenthesis):
    /// 
    /// <code>
    /// var subMenu = ("File" 
    ///             + ("New"
    ///                + "File".OnClick(CreateFile)
    ///                + "Project".OnClick(CreateProject)
    ///               )
    ///             + "Open".OnClick(OpenFile)
    ///             + "Close".OnClick(CloseFile)
    ///             + "Exit".OnClick(Exit)
    ///            );
    /// </code>
    /// <br/>
    /// </para>
    /// 
    /// <para>
    /// <b>Separators</b> are added using <see cref="MenuItem.Separator"/>.
    /// 
    /// <code>
    /// var menu = ("File" 
    ///             + "New".OnClick(CreateFile)
    ///             + "Open".OnClick(OpenFile)
    ///             + "Close".OnClick(CloseFile)
    ///             + MenuItem.Separator
    ///             + "Exit".OnClick(Exit)
    ///            );
    /// </code>
    /// </para>
    /// 
    /// <para>
    /// <b>The menu location</b> determines where the menu will be displayed. Possible menu locations are defined by the enum <see cref="MenuLocation"/>. The can, of course, be 
    /// combined using the "|" operator.
    /// <br/>
    /// Again, we use the overloaded "+" operator to define the complete menu. The following examples puts the file menu into the main menu bar:
    /// 
    /// <code>
    /// var menu = MenuLocation.MainMenu
    ///            + ("File" 
    ///               + "New".OnClick(CreateFile)
    ///               + "Open".OnClick(OpenFile)
    ///               + "Close".OnClick(CloseFile)
    ///               + MenuItem.Separator
    ///               + "Exit".OnClick(Exit)
    ///              );
    /// </code>
    /// </para>
    /// 
    /// <para>
    /// <b>Putting it all together:</b>
    /// </para>
    /// 
    /// <para>
    /// Here is an example of a complete "File" menu with actions, nested sub-menus and separators:
    /// 
    /// <code>
    /// var menu = MenuLocation.MainMenu
    ///            + ("&amp;File" 
    ///               + ("&amp;New"
    ///                  + "&amp;File".OnClick(CreateFile)
    ///                  + "&amp;Project".OnClick(CreateProject)
    ///                 )
    ///               + "&amp;Open".OnClick(OpenFile)
    ///               + "&amp;Close".OnClick(CloseFile)
    ///               + MenuItem.Separator
    ///               + "Allow multiple open files".OnClick(ToggleMultipleOpenFilesAllowed).Checked(IfMultipleOpenFilesAllowed)
    ///               + MenuItem.Separator 
    ///               + "&amp;Exit".OnClick(Exit)
    ///              );
    /// </code>
    /// <br/>
    /// The resulting menu structure will look like this (arrows indicate a sub-menu relation):
    /// <code>
    /// File --> New ------------------------> File
    ///          Open                          Project
    ///          Close
    ///          ------
    ///          Allow multiple open files
    ///          ------
    ///          Exit
    /// </code>
    /// </para>
    /// 
    /// <hr/>
    /// 
    /// <para>
    /// <b>Assigning menu structures to contexts:</b>
    /// </para>
    /// 
    /// <para>
    /// The previous section described how menu structures can be defined. However, we want to be able to display 
    /// different menu structures in different contexts. The context in which a menu should be displayed is specified
    /// with the ShowIf method of the Menu class.
    /// 
    /// <code>
    /// var menuManager = new MenuManager();
    /// menuManager.AddMenu(
    ///     (MenuLocation.TreeView | MenuLocation.Diagram)
    ///       + ("BDTLibrary context menu"
    ///          + "Do something specific to BDT libraries".OnClick(DoSomeBDTStuff)
    ///          + "Do something else".OnClick(DoSomethingElse)
    ///         )
    ///     )
    /// ).ShowIf(context => context.SelectedItemIsLibraryOfType(Stereotype.BDTLibrary));
    /// </code>
    /// <br/>
    /// This menu will only be displayed in the context menu of the tree view or diagram and only if a BDTLibrary is currently selected.
    /// </para>
    /// 
    /// <para>
    /// Menu contexts are evaluated in the order in which the menus are added to the menu manager.
    /// </para>
    /// 
    /// <hr/>
    /// 
    /// <para>
    /// <b>Default settings:</b>
    /// </para>
    /// 
    /// <para>
    /// The following default settings must be specified as MenuManager properties:
    /// <list type="bullet">
    /// <item><see cref="DefaultEnabled"/>: A predicate to determine the enabled/disabled state of actions where <see cref="MenuAction.Enabled"/> is not explicitely set.</item>
    /// <item><see cref="DefaultChecked"/>: A predicate to determine the checked state of actions where <see cref="MenuAction.Checked"/> is not explicitely set.</item>
    /// <item><see cref="DefaultMenuItems"/>: A string[] containing the menu items to be displayed when no context matches.</item>
    /// </list>
    /// Example:
    /// <code>
    /// var menuManager = new MenuManager
    ///                   {
    ///                     DefaultMenuItems = new[] {"My Add-In"},
    ///                     DefaultEnabled = Always,
    ///                     DefaultChecked = Never
    ///                   };
    /// 
    /// ...
    /// 
    /// public bool Always(AddInContext context)
    /// {
    ///   return true;
    /// }
    /// 
    /// public bool Never(AddInContext context)
    /// {
    ///   return false;
    /// }
    /// </code>
    /// </para>
    /// 
    /// <hr/>
    /// 
    /// <para>
    /// See <see cref="VIENNAAddIn"/> or MenuManagerTest for real-life menu definitions.
    /// </para>
    ///</summary>
    public class MenuManager
    {
        private readonly MenuAction defaultAction = string.Empty.OnClick(DoNothing).Checked(Never).Enabled(Always);

        private readonly Dictionary<MenuLocation, List<Menu>> menusByLocation = new Dictionary<MenuLocation, List<Menu>>();

        private MenuItem activeMenu;

        private static void DoNothing(AddInContext context)
        {
            // do nothing
        }

        ///<summary>
        /// Returns false.
        ///</summary>
        ///<param name="context"></param>
        ///<returns></returns>
        private static bool Always(AddInContext context)
        {
            return true;
        }

        ///<summary>
        /// Returns false.
        ///</summary>
        ///<param name="context"></param>
        ///<returns></returns>
        private static bool Never(AddInContext context)
        {
            return false;
        }

        public Menu AddMenu(Menu menu)
        {
            foreach (MenuLocation menuLocation in Enum.GetValues(typeof (MenuLocation)))
            {
                if ((menu.MenuLocation & menuLocation) == menuLocation)
                {
                    GetMenus(menuLocation).Add(menu);
                }
            }
            return menu;
        }

        private List<Menu> GetMenus(MenuLocation menuLocation)
        {
            List<Menu> menus;
            if (!menusByLocation.TryGetValue(menuLocation, out menus))
            {
                menus = new List<Menu>();
                menusByLocation[menuLocation] = menus;
            }
            return menus;
        }

        #region EA Menu API

        ///<summary>
        /// Should be called from EA_GetMenuState.
        ///</summary>
        ///<param name="context">The current context.</param>
        ///<param name="isEnabled"></param>
        ///<param name="isChecked"></param>
        ///<param name="menuName"></param>
        ///<param name="menuItem"></param>
        public void GetMenuState(AddInContext context, string menuName, string menuItem, ref bool isEnabled, ref bool isChecked)
        {
            MenuAction menuAction = GetMenuAction(menuName, menuItem);
            isEnabled = menuAction.IsEnabled == null ? DefaultEnabled(context) : menuAction.IsEnabled(context);
            isChecked = menuAction.IsChecked == null ? DefaultChecked(context) : menuAction.IsChecked(context);
        }

        ///<summary>
        /// Should be called from EA_GetMenuItems.
        ///</summary>
        ///<param name="context"></param>
        ///<param name="menuName"></param>
        ///<returns></returns>
        public string[] GetMenuItems(AddInContext context, string menuName)
        {
//            if (IsInitialInvocation(menuName))
//            {
            activeMenu = null;
            foreach (Menu menu in GetMenus(context.MenuLocation))
            {
                if (menu.Matches(context))
                {
                    activeMenu = menu;
                    break;
                }
            }
//            }
            if (activeMenu == null)
            {
                return DefaultMenuItems;
            }
            return activeMenu.GetMenuItems(menuName ?? string.Empty) ?? DefaultMenuItems;
        }

        private static bool IsInitialInvocation(string menuName)
        {
            return string.IsNullOrEmpty(menuName);
        }

        ///<summary>
        /// Should be called from EA_MenuClick.
        ///</summary>
        ///<param name="context"></param>
        ///<param name="menuName"></param>
        ///<param name="menuItem"></param>
        public void MenuClick(AddInContext context, string menuName, string menuItem)
        {
            GetMenuAction(menuName, menuItem).Execute(context);
        }

        private MenuAction GetMenuAction(string menuName, string menuItem)
        {
            if (activeMenu == null)
            {
                return defaultAction;
            }
            return activeMenu.GetMenuAction(menuName, menuItem) ?? defaultAction;
        }

        #endregion

        #region Default settings

        ///<summary>
        /// The default menu items to be displayed if no menu is defined for the current context.
        ///</summary>
        public string[] DefaultMenuItems { get; set; }

        ///<summary>
        /// The default predicate to determine the enabled/disabled state of menu items that have no explicit predicate set.
        ///</summary>
        public Predicate<AddInContext> DefaultEnabled { get; set; }

        ///<summary>
        /// The default predicate to determine the checked state of menu items that have no explicit predicate set.
        ///</summary>
        public Predicate<AddInContext> DefaultChecked { get; set; }

        #endregion
    }
}