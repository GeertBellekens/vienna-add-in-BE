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

namespace VIENNAAddIn.upcc3.Wizards.dev.temporarymodel.subsettingmodel
{
    public class CandidateAbie
    {
        private readonly bool mItemReadOnly;
        private readonly Cursor mItemCursor;
        private readonly bool mItemFocusable;

        public string Name { get; set; }
        public bool Checked { get; set; }
        public bool Selected { get; set; }
        public IAbie OriginalAbie { get; set; }
        public List<CandidateAbie> PotentialAbies { get; set; }
        public List<PotentialBbie> PotentialBbies { get; set; }
        public List<PotentialAsbie> PotentialAsbies { get; set; }

        public CandidateAbie(IAbie originalAbie)
        {
            Name = originalAbie.Name;
            Checked = false;
            OriginalAbie = originalAbie;
            Selected = false;
            PotentialAbies = null;
            PotentialBbies = null;
            PotentialAsbies = null;

            mItemReadOnly = true;
            mItemCursor = Cursors.Arrow;
            mItemFocusable = false;
        }

        public CandidateAbie(IAbie originalAbie, List<CandidateAbie> potenAbies)
        {
            Name = originalAbie.Name;
            Checked = false;
            OriginalAbie = originalAbie;
            Selected = false;
            PotentialAbies = potenAbies;
            PotentialBbies = null;
            PotentialAsbies = null;
            mItemReadOnly = true;
            mItemCursor = Cursors.Arrow;
            mItemFocusable = false;
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