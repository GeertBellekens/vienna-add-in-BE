// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************

using System.Collections.Generic;
using System.Windows.Input;
using CctsRepository.BieLibrary;

namespace VIENNAAddIn.upcc3.Wizards.dev.temporarymodel.abiemodel
{
    public class CandidateAbie
    {
        private string mName;
        private bool mChecked;
        private IAbie mOriginalAbie;
        private bool mSelected; 
        private List<PotentialAsbie> mPotentialAsbies;
        private bool mItemReadOnly;
        private Cursor mItemCursor;
        private bool mItemFocusable;

        public CandidateAbie(IAbie originalAbie)
        {
            mName = originalAbie.Name;
            mChecked = false;
            mOriginalAbie = originalAbie;
            mSelected = false;
            mPotentialAsbies = null;

            mItemReadOnly = true;
            mItemCursor = Cursors.Arrow;
            mItemFocusable = false;
        }

        public string Name
        {
            get { return mName; }
            set { mName = value; }
        }

        public bool Checked
        {
            get { return mChecked; }
            set { mChecked = value; }
        }

        public bool Selected
        {
            get { return mSelected; }
            set { mSelected = value; }
        }

        public IAbie OriginalAbie
        {
            get { return mOriginalAbie; }
            set { mOriginalAbie = value; }
        }

        public List<PotentialAsbie> PotentialAsbies
        {
            get { return mPotentialAsbies; }
            set { mPotentialAsbies = value; }
        }

        public bool ItemReadOnly
        {
            get { return mItemReadOnly; }
        }

        public Cursor ItemCursor
        {
            get { return mItemCursor; }
        }

        public bool ItemFocusable
        {
            get { return mItemFocusable; }
        }
    }
}