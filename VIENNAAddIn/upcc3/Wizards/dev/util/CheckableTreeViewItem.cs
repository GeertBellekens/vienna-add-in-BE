// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************

using System.Collections.ObjectModel;

namespace VIENNAAddIn.upcc3.Wizards.dev.util
{
    public class CheckableTreeViewItem
    {
        public bool Checked { get; set; }
        public bool IsExpanded { get; set; }
        public string Text { get; set; }
        public string visibility { get; set; }
        public CheckableTreeViewItem Parent { get; set; }
        public ObservableCollection<CheckableTreeViewItem> Children { get; set; }
        public int Id { get; set; }

        public string BindId
        {
            get { return Id.ToString(); }
            set { BindId = value; }
        }
        public CheckableTreeViewItem(bool initChecked, string initText, int id)
        {
            Id = id;
            Checked = initChecked;
            Text = initText;
            visibility = "Visible";
        }
        public CheckableTreeViewItem(string initText, int id)
        {
            Id = id;
            Text = initText;
            visibility = "Collapsed";
        }

        public CheckableTreeViewItem(bool initChecked, string initText, CheckableTreeViewItem parent, ObservableCollection<CheckableTreeViewItem> children, int id)
        {
            Id = id;
            Checked = initChecked;
            Text = initText;
            Parent = parent;
            Children = children;
            visibility = "Visible";
        }
        public CheckableTreeViewItem(bool initChecked, string initText, CheckableTreeViewItem parent, int id)
        {
            Id = id;
            Checked = initChecked;
            Text = initText;
            Parent = parent;
            visibility = "Visible";
        }
        public CheckableTreeViewItem(string initText, CheckableTreeViewItem parent, ObservableCollection<CheckableTreeViewItem> children, int id)
        {
            Id = id;
            Text = initText;
            Children = children;
            Parent = parent;
            visibility = "Collapsed";
        }
        public CheckableTreeViewItem(string initText, CheckableTreeViewItem parent, int id)
        {
            Id = id;
            Text = initText;
            Parent = parent;
            visibility = "Collapsed";
        }
    }
}