using System;

namespace VIENNAAddIn.menu
{
    ///<summary>
    /// A menu item that represents an action (as opposed to a sub-menu).
    ///</summary>
    public class MenuAction : MenuItem
    {
        ///<summary>
        ///</summary>
        ///<param name="name"></param>
        ///<param name="action"></param>
        public MenuAction(string name, Action<AddInContext> action) : base(name)
        {
            Execute = action;
        }

        ///<summary>
        /// Executes the action.
        ///</summary>
        public Action<AddInContext> Execute { get; private set; }

        public Predicate<AddInContext> IsChecked { get; private set; }
        public Predicate<AddInContext> IsEnabled { get; private set; }

        /// <summary>
        /// Sets the predicate to determine the action's checked state.
        /// </summary>
        /// <param name="isChecked"></param>
        /// <returns></returns>
        public MenuAction Checked(Predicate<AddInContext> isChecked)
        {
            IsChecked = isChecked;
            return this;
        }

        ///<summary>
        /// Sets the predicate to determine the action's enabled/disabled state.
        ///</summary>
        ///<param name="isEnabled"></param>
        ///<returns></returns>
        public MenuAction Enabled(Predicate<AddInContext> isEnabled)
        {
            IsEnabled = isEnabled;
            return this;
        }
    }
}