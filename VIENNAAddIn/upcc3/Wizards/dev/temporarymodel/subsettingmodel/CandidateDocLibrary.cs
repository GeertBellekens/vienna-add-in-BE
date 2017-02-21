using System.Collections.Generic;
using CctsRepository.DocLibrary;
using VIENNAAddIn.upcc3.Wizards.dev.cache;

namespace VIENNAAddIn.upcc3.Wizards.dev.temporarymodel.subsettingmodel
{
    public class CandidateDocLibrary
    {
        private List<CandidateRootElement> mCandidateRootElements;

        public CandidateDocLibrary(IDocLibrary docLibrary)
        {
            OriginalDocLibrary = docLibrary;
            Selected = false;
            mCandidateRootElements = null;
        }

        public IDocLibrary OriginalDocLibrary { set; get; }
        public bool Selected { set; get; }

        public List<CandidateRootElement> CandidateRootElements
        {
            set { mCandidateRootElements = value; }

            get
            {
                if (mCandidateRootElements == null)
                {
                    mCandidateRootElements =
                        new List<CandidateRootElement>(
                            CcCache.GetInstance().GetMasFromDocLibrary(OriginalDocLibrary.Name).ConvertAll(
                                ma => new CandidateRootElement(ma)));
                }

                return mCandidateRootElements;
            }
        }
    }
}