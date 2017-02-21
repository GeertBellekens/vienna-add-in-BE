// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************

using System;
using System.Collections.Generic;
using CctsRepository.BieLibrary;
using CctsRepository.CcLibrary;
using VIENNAAddIn.upcc3.Wizards.dev.cache;

namespace VIENNAAddIn.upcc3.Wizards.dev.temporarymodel.abiemodel
{
    public class CandidateAcc
    {
        private IAcc mOriginalAcc;
        private bool mSelected;
        private List<CandidateBcc> mCandidateBccs; 
        private List<CandidateAbie> mCandidateAbies;

        public CandidateAcc(IAcc originalAcc)
        {
            mOriginalAcc = originalAcc;
            mSelected = false;
            mCandidateBccs = null;
            mCandidateAbies = null;
        }

        public IAcc OriginalAcc
        {
            get { return mOriginalAcc; }
            set { mOriginalAcc = value;}
        }

        public bool Selected
        {
            get { return mSelected; }
            set { mSelected = value; }
        }

        public List<CandidateBcc> CandidateBccs
        {
            get
            {
                if (mCandidateBccs == null)
                {                                        
                    mCandidateBccs = new List<CandidateBcc>();                    
                    
                    foreach (IBcc bcc in OriginalAcc.Bccs)
                    {
                        CandidateBcc newCandidateBcc = new CandidateBcc(bcc);                                                

                        mCandidateBccs.Add(newCandidateBcc);
                    }                    
                }
                
                return mCandidateBccs;
            }
        }

        public List<CandidateAbie> CandidateAbies
        {
            set
            {
                mCandidateAbies = value;
            }

            get            
            {  
                if (mCandidateAbies == null)
                {
                    CcCache ccCache = CcCache.GetInstance();
                    mCandidateAbies = new List<CandidateAbie>();

                    foreach (IAscc ascc in mOriginalAcc.Asccs)
                    {                        
                        foreach (IBieLibrary bieLibrary in ccCache.GetBieLibraries())
                        {
                            foreach (IAbie abie in ccCache.GetBiesFromBieLibrary(bieLibrary.Name))
                            {
                                if (abie.BasedOn.Id == ascc.AssociatedAcc.Id)
                                {
                                    AddAbieToCandidateAbies(abie);
                                    AddPotentialAsbieToCandidateAbie(abie.Name, ascc);
                                }
                            }
                        }
                    }
                }

                return mCandidateAbies;
            }
        }

        private void AddPotentialAsbieToCandidateAbie(string abieName, IAscc ascc)
        {
            foreach (CandidateAbie candidateAbie in mCandidateAbies)
            {
                if (candidateAbie.Name.Equals(abieName))
                {
                    if (candidateAbie.PotentialAsbies == null)
                    {
                        candidateAbie.PotentialAsbies = new List<PotentialAsbie>();
                    }

                    candidateAbie.PotentialAsbies.Add(new PotentialAsbie(ascc));

                    break;
                }
            }
        }

        private void AddAbieToCandidateAbies(IAbie newAbie)
        {
            bool newAbieNotYetInCandidateAbies = true;

            foreach (CandidateAbie candidateAbie in mCandidateAbies)
            {
                if (candidateAbie.Name == newAbie.Name)
                {
                    newAbieNotYetInCandidateAbies = false;
                }
            }            

            if (newAbieNotYetInCandidateAbies)
            {
                mCandidateAbies.Add(new CandidateAbie(newAbie));
            }
        }

        public void Clear()
        {
            mCandidateBccs = null;
            mCandidateAbies = null;
        }
    }
}