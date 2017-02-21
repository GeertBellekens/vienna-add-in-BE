// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************

using System.Collections.Generic;
using CctsRepository.CdtLibrary;

namespace VIENNAAddIn.upcc3.Wizards.dev.temporarymodel.bdtmodel
{
    public class CandidateCdtLibrary
    {

        private ICdtLibrary mOriginalCdtLibrary;
        private bool mSelected;
        public List<CandidateCdt> mCandidateCdts;

        public CandidateCdtLibrary(ICdtLibrary originalCdtLibrary)
        {
            mOriginalCdtLibrary = originalCdtLibrary;
            mSelected = false;
            mCandidateCdts = null;
        }
        
        public bool Selected
        {
            get { return mSelected; }
            set { mSelected = value; }
        }

        public ICdtLibrary OriginalCdtLibrary
        {
            get { return mOriginalCdtLibrary; }
            set { mOriginalCdtLibrary = value; }
        }

        public List<CandidateCdt> CandidateCdts
        {
            get
            {
                if (mCandidateCdts == null)
                {
                    mCandidateCdts = new List<CandidateCdt>();
                    foreach (ICdt cdt in mOriginalCdtLibrary.Cdts)
                    {
                        mCandidateCdts.Add(new CandidateCdt(cdt));
                    }
                }
                return mCandidateCdts;
            }
        }
    }
}