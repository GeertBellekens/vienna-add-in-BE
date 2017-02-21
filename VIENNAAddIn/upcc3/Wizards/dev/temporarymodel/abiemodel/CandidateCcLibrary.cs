using System.Collections.Generic;
using CctsRepository.CcLibrary;
using VIENNAAddIn.upcc3.Wizards.dev.cache;

namespace VIENNAAddIn.upcc3.Wizards.dev.temporarymodel.abiemodel
{
    public class CandidateCcLibrary
    {
        private ICcLibrary mOriginalCcLibrary;
        private bool mSelected;
        private List<CandidateAcc> mCandidateAccs;

        public CandidateCcLibrary(ICcLibrary ccLibrary)
        {
            mOriginalCcLibrary = ccLibrary;
            mSelected = false;
            mCandidateAccs = null;
        }

        public ICcLibrary OriginalCcLibrary
        {
            set { mOriginalCcLibrary = value; }
            get { return mOriginalCcLibrary; }
        }

        public bool Selected
        {
            set { mSelected = value; }
            get { return mSelected; }
        }

        public List<CandidateAcc> CandidateAccs
        {
            set
            {
                mCandidateAccs = value;    
            }

            get
            {
                if (mCandidateAccs == null)
                {
                    mCandidateAccs = new List<CandidateAcc>(CcCache.GetInstance().GetCcsFromCcLibrary(OriginalCcLibrary.Name).ConvertAll(acc => new CandidateAcc(acc)));
                }

                return mCandidateAccs;
            }
        }
    }
}