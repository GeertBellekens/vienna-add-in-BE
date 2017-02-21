using System;

namespace VIENNAAddIn.menu
{
    ///<summary>
    ///</summary>
    public static class MenuStringExtensions
    {
        ///<summary>
        /// Create a MenuAction from a string specifying the action's display name and an action to be executed when the item is clicked.
        ///</summary>
        ///<param name="name"></param>
        ///<param name="onClick"></param>
        ///<returns></returns>
        public static MenuAction OnClick(this string name, Action<AddInContext> onClick)
        {
            return new MenuAction(name, onClick);
        }
    }
}