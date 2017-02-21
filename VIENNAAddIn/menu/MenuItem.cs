using System;
using System.Collections.Generic;

namespace VIENNAAddIn.menu
{
    ///<summary>
    ///</summary>
    public class MenuItem
    {
        public readonly static MenuItem Separator = new MenuItem("-");
        public virtual string Name { get; private set; }

        protected MenuItem(string name)
        {
            Name = name;
        }

        public static Menu operator +(MenuLocation lhs, MenuItem rhs)
        {
            var menu = new Menu(lhs);
            menu.AddItem(rhs);
            return menu;
        }

        public static SubMenu operator +(string lhs, MenuItem rhs)
        {
            return new SubMenu(lhs).AddItem(rhs);
        }

        public static List<MenuItem> operator +(MenuItem lhs, MenuItem rhs)
        {
            return new List<MenuItem> {lhs, rhs};
        }

        public static List<MenuItem> operator +(List<MenuItem> lhs, MenuItem rhs)
        {
            return new List<MenuItem>(lhs) {rhs};
        }

        public virtual string[] GetMenuItems(string menuName)
        {
            return null;
        }

        public virtual MenuAction GetMenuAction(string menuName, string menuItem)
        {
            return null;
        }
    }

    public class Menu:SubMenu
    {
        public Menu(MenuLocation menuLocation):base(null)
        {
            MenuLocation = menuLocation;
        }

        public MenuLocation MenuLocation { get; private set; }
        private Predicate<AddInContext> predicate;

        public void ShowIf(Predicate<AddInContext> predicate)
        {
            this.predicate = this.predicate.And(predicate);
        }

        public bool Matches(AddInContext context)
        {
            return predicate != null ? predicate(context) : true;
        }
    }

    public class SubMenu:MenuItem
    {
        public SubMenu(string name) : base(name)
        {
        }

        public override string Name
        {
            get { return string.IsNullOrEmpty(base.Name) ? string.Empty : "-" + base.Name; }
        }

        private string[] menuItems;
        private readonly List<MenuItem> items = new List<MenuItem>();

        public List<MenuItem> Items
        {
            get { return items; }
        }

        public SubMenu AddItem(MenuItem item)
        {
            items.Add(item);
            return this;
        }

        public static SubMenu operator +(SubMenu lhs, MenuItem rhs)
        {
            return lhs.AddItem(rhs);
        }

        public override string[] GetMenuItems(string menuName)
        {
            if (Name.Equals(menuName))
            {
                if (menuItems == null)
                {
                    menuItems = items.ConvertAll(item => item.Name).ToArray();
                }
                return menuItems;
            }
            foreach (MenuItem item in items)
            {
                var subMenuItems = item.GetMenuItems(menuName);
                if (subMenuItems != null)
                {
                    return subMenuItems;
                }
            }
            return null;
        }

        public override MenuAction GetMenuAction(string menuName, string menuItem)
        {
            if (Name.Equals(menuName))
            {
                foreach (MenuItem item in items)
                {
                    if (item is MenuAction && item.Name.Equals(menuItem))
                    {
                        return (MenuAction) item;
                    }
                }
            }
            foreach (MenuItem item in items)
            {
                if (item is SubMenu)
                {
                    var menuAction = item.GetMenuAction(menuName, menuItem);
                    if (menuAction != null)
                    {
                        return menuAction;
                    }
                }
            }
            return null;
        }
    }
}