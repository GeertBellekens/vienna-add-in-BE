// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************

using CctsRepository.BieLibrary;

namespace VIENNAAddIn.upcc3.Wizards.dev.temporarymodel.abiemodel
{
    public class CandidateBieLibrary
    {
        private IBieLibrary mOriginalBieLibrary;
        private bool mSelected;

        public CandidateBieLibrary(IBieLibrary bieLibrary)
        {
            mOriginalBieLibrary = bieLibrary;
            mSelected = false;            
        }

        public IBieLibrary OriginalBieLibrary
        {
            set { mOriginalBieLibrary = value; }
            get { return mOriginalBieLibrary; }
        }

        public bool Selected
        {
            set { mSelected = value; }
            get { return mSelected; }
        }
    }
}