// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************

namespace VIENNAAddIn.upcc3.Wizards.dev.temporarymodel.subsettingmodel
{
    public class PotentialAsbie
    {
        public PotentialAsbie(string name, bool incoming)
        {
            Name = name;
            Checked = true;
            Incoming = incoming;
        }

        public string Name { get; set; }
        public bool Checked { get; set; }
        public bool Incoming { get; set; }
    }
}