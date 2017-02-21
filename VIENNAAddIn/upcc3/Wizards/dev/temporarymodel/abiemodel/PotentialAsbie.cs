// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************

using System.Windows.Input;
using CctsRepository.CcLibrary;

namespace VIENNAAddIn.upcc3.Wizards.dev.temporarymodel.abiemodel
{
    public class PotentialAsbie
    {        
        private string mName;
        private bool mChecked;
        private bool mSelected;
        private IAscc mBasedOn;
        private bool mItemReadOnly;
        private Cursor mItemCursor;
        private bool mItemFocusable;

        public PotentialAsbie(IAscc originalAscc)
        {
            mName = originalAscc.Name;
            mChecked = false;
            mBasedOn = originalAscc;

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

        public IAscc BasedOn
        {
            get { return mBasedOn; }
            set { mBasedOn = value; }
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