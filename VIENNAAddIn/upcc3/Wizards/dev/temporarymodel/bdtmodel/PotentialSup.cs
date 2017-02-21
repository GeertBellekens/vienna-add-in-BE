// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************

using CctsRepository.CdtLibrary;
namespace VIENNAAddIn.upcc3.Wizards.dev.temporarymodel.bdtmodel
{
    public class PotentialSup
    {
        private bool mChecked;
        private ICdtSup mOriginalCdtSup;

        public PotentialSup(ICdtSup originalCdtSup)
        {
            mOriginalCdtSup = originalCdtSup;
            mChecked = false;
        }
        
        public bool Checked
        {
            get{ return mChecked; }
            set{ mChecked = value; }
        }
        public ICdtSup OriginalCdtSup
        {
            get { return mOriginalCdtSup; }
            set { mOriginalCdtSup = value; }
        }
    }
}