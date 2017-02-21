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
    public class PotentialCon
    {
        private bool mChecked;
        private ICdtCon mOriginalCdtCon;

        public PotentialCon(ICdtCon originalCdtCon)
        {
            mOriginalCdtCon = originalCdtCon;
            mChecked = true;
        }

        public bool Checked
        {
            get { return mChecked; }
            set { mChecked = value; }
        }
        public ICdtCon OriginalCdtCon
        {
            get { return mOriginalCdtCon; }
            set { mOriginalCdtCon = value; }
        }
    }
}