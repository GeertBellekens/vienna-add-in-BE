using System.Collections.Generic;
using CctsRepository.DocLibrary;

namespace VIENNAAddIn.upcc3.Wizards.dev.temporarymodel.subsettingmodel
{
    public class CandidateRootElement
    {
        public CandidateRootElement(IMa ma)
        {
            OriginalMa = ma;
            Selected = false;
            CandidateAbies = null;
        }

        public IMa OriginalMa { set; get; }

        public bool Selected { set; get; }

        public List<CandidateAbie> CandidateAbies { set; get; }
    }
}