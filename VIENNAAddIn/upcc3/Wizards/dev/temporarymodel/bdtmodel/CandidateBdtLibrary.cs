// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************

using CctsRepository.BdtLibrary;

namespace VIENNAAddIn.upcc3.Wizards.dev.temporarymodel.bdtmodel
{
    public class CandidateBdtLibrary
    {
        private IBdtLibrary mOriginalBdtLibrary;
        private bool mSelected;

        public CandidateBdtLibrary(IBdtLibrary bdtLibrary)
        {
            mOriginalBdtLibrary = bdtLibrary;
            mSelected = false;
        }

        public IBdtLibrary OriginalBdtLibrary
        {
            set { mOriginalBdtLibrary = value; }
            get { return mOriginalBdtLibrary; }
        }

        public bool Selected
        {
            set { mSelected = value; }
            get { return mSelected; }
        }
    }
}