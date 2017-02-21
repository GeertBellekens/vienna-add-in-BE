// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************

using System.Windows.Input;
using CctsRepository.BdtLibrary;

namespace VIENNAAddIn.upcc3.Wizards.dev.temporarymodel.abiemodel
{
    public class PotentialBdt
    {
        private string mName;
        private bool mChecked;
        private bool mSelected;
        private IBdt mOriginalBDT;
        private bool mItemReadOnly;
        private Cursor mItemCursor;
        private bool mItemFocusable;

        public PotentialBdt(string newBdtName)
        {
            mName = newBdtName;
            mChecked = false;
            mOriginalBDT = null;

            mItemReadOnly = false;
            mItemCursor = Cursors.IBeam;
            mItemFocusable = true;
        }

        public PotentialBdt(IBdt originalBdt)
        {
            mName = originalBdt.Name;
            mChecked = false;
            mOriginalBDT = originalBdt;

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

        public IBdt OriginalBdt
        {
            get { return mOriginalBDT; }
            set { mOriginalBDT = value; }
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