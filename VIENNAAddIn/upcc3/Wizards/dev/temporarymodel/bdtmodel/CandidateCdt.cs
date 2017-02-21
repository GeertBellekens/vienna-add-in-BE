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
    public class CandidateCdt
    {

        private ICdt mOriginalCdt;
        private bool mSelected;
        public PotentialCon mPotentialCon;
        public List<PotentialSup> mPotentialSups;

        public CandidateCdt(ICdt originalCdt)
        {
            mOriginalCdt = originalCdt;
            mSelected = false;
            mPotentialSups = null;
            mPotentialCon = new PotentialCon(originalCdt.Con);
        }

        public ICdt OriginalCdt
        {
            get { return mOriginalCdt; }
            set { mOriginalCdt = value; }
        }
        public bool Selected
        {
            get { return mSelected; }
            set { mSelected = value; }
        }
        public PotentialCon PotentialCon
        {
            get { return mPotentialCon; }
        }
        public List<PotentialSup> PotentialSups
        {
            get
            {
                if (mPotentialSups == null)
                {
                    mPotentialSups = new List<PotentialSup>();
                    foreach (ICdtSup sup in mOriginalCdt.Sups)
                    {
                        mPotentialSups.Add(new PotentialSup(sup));
                    }
                }
                return mPotentialSups;
            }
        }
    }
}