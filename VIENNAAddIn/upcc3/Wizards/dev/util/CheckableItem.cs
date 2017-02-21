// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************

using System.Windows.Input;

namespace VIENNAAddIn.upcc3.Wizards.dev.util
{
    public class CheckableItem
    {
        public bool Checked { get; set; }
        public string Text { get; set; }
        public bool ItemReadOnly { get; set; }
        public bool ItemFocusable { get; set; }
        public Cursor ItemCursor { get; set; }

        public CheckableItem(bool initChecked, string initText, bool initReadOnly, bool initFocusable, Cursor initCursor)
        {
            Checked = initChecked;
            Text = initText;
            ItemReadOnly = initReadOnly;
            ItemFocusable = initFocusable;
            ItemCursor = initCursor;
        }
    }
}