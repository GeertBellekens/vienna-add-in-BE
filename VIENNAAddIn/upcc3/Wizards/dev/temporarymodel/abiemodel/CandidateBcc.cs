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
using CctsRepository.CcLibrary;

namespace VIENNAAddIn.upcc3.Wizards.dev.temporarymodel.abiemodel
{
    public class CandidateBcc
    {
        private bool mChecked;
        private IBcc mOriginalBcc;
        private bool mSelected;
        private List<PotentialBbie> mPotentialBbies;
        private bool mItemReadOnly;
        private Cursor mItemCursor;
        private bool mItemFocusable;

        public CandidateBcc(IBcc originalBcc)
        {
            mChecked = false;
            mOriginalBcc = originalBcc;
            mSelected = false;
            mPotentialBbies = null;

            mItemReadOnly = true;
            mItemCursor = Cursors.Arrow;
            mItemFocusable = false;
        }

        public bool Checked
        {
            get { return mChecked; }
            set { mChecked = value; }
        }

        public IBcc OriginalBcc
        {
            get { return mOriginalBcc; }
            set { mOriginalBcc = value; }
        }

        public bool Selected
        {
            get { return mSelected; }
            set { mSelected = value; }
        }

        public List<PotentialBbie> PotentialBbies
        {
            get
            {
                if (mPotentialBbies == null)
                {
                    mPotentialBbies = new List<PotentialBbie> ();
                    AddPotentialBbie();
                }

                return mPotentialBbies;
            }

            set { mPotentialBbies = value; }
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

        public void AddPotentialBbie()
        {
            // The names for the BBIEs generated should follow the following pattern. If no BBIE is
            // available, then the default name for the BBIE generated is the same as the name of
            // the BCC that the BBIE is based on (Option 1). If such a BBIE already exists, then the 
            // name of the new BBIE should be the name of the BCC it is based on prefixed with the 
            // text "Qualifier", a successing number, as well as an underscore to separate the 
            // prefix from the name stemming from the BCC (Option 2). 
            //
            // Example for a list of BBIEs based on the BCC "Age" 
            //    BBIE 1: "Age"                   (i.e. Option 1)
            //    BBIE 2: "Qualifier1_Age"        (i.e. Option 2)
            //    BBIE 3: "Qualifier2_Age"        (i.e. Option 2)
            //    ...

            string newBbieName = OriginalBcc.Name;

            // Generating a new BBIE Name according to Option 1
            if (!BbieWithTheSameNameExists(newBbieName))
            {
                PotentialBbie newPotentialBbie = new PotentialBbie(newBbieName, OriginalBcc.Cdt);
                mPotentialBbies.Add(newPotentialBbie);
            }
            else
            {
                // Generating a new BBIE Name according to Option 2
                for (int i = 1; i != -1; i++)
                {
                    newBbieName = "Qualifier" + i + "_" + OriginalBcc.Name;

                    if (!BbieWithTheSameNameExists(newBbieName))
                    {
                        PotentialBbie newPotentialBbie = new PotentialBbie(newBbieName, OriginalBcc.Cdt);
                        mPotentialBbies.Add(newPotentialBbie);

                        break;
                    }
                }
            }
        }

        private bool BbieWithTheSameNameExists(string newBbieName)
        {
            bool foundBbieWithTheSameName = false;

            foreach (PotentialBbie potentialBbie in mPotentialBbies)
            {
                if (potentialBbie.Name.Equals(newBbieName))
                {
                    foundBbieWithTheSameName = true;
                }

            }
            return foundBbieWithTheSameName;
        }
    }
}