// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************

namespace VIENNAAddIn.upcc3.Wizards.dev.util
{
    public class IOConnectionItem
    {
        public IOConnectionItem(string initText, bool isInbound, bool isChecked)
        {
            if (isInbound)
            {
                Inbound = "Visible";
                Outbound = "Collapsed";
            }
            else
            {
                Inbound = "Collapsed";
                Outbound = "Visible";
            }
            Text = initText;
            Checked = isChecked;
        }

        public string Inbound { get; set; }
        public string Outbound { get; set; }
        public string Text { get; set; }
        public bool Checked { get; set; }
    }
}