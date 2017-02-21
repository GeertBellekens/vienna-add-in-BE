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
using System.ComponentModel;
using System.Windows.Input;
using CctsRepository;
using CctsRepository.BdtLibrary;
using CctsRepository.CdtLibrary;
using VIENNAAddIn.upcc3.Wizards.dev.binding;
using VIENNAAddIn.upcc3.Wizards.dev.cache;
using VIENNAAddIn.upcc3.Wizards.dev.temporarymodel.bdtmodel.exceptions;
using VIENNAAddIn.upcc3.Wizards.dev.util;
namespace VIENNAAddIn.upcc3.Wizards.dev.temporarymodel.bdtmodel
{
    public class TemporaryBdtModel : TemporaryModel, INotifyPropertyChanged
    {
        #region Backing Fields for Binding Properties
        private List<string> mCandidateBdtLibraryNames;
        private List<string> mCandidateCdtLibraryNames;
        private List<string> mCandidateCdtNames;
        private List<CheckableItem> mCandidateConItems;
        private List<CheckableItem> mCandidateSupItems;
        private string mName;
        private string mPrefix;
        #endregion

        #region Class Fields
        private CcCache ccCache;
        public List<CandidateBdtLibrary> mCandidateBdtLibraries;
        public List<CandidateCdtLibrary> mCandidateCdtLibraries;
        #endregion

        public TemporaryBdtModel(ICctsRepository cctsRepository)
        {
            ccCache = CcCache.GetInstance(cctsRepository);

            mCandidateBdtLibraries = new List<CandidateBdtLibrary>(ccCache.GetBdtLibraries().ConvertAll(bdtl => new CandidateBdtLibrary(bdtl)));
            mCandidateBdtLibraryNames = new List<string>(mCandidateBdtLibraries.ConvertAll(bdtlname => bdtlname.OriginalBdtLibrary.Name));
            mCandidateCdtLibraries = new List<CandidateCdtLibrary>(ccCache.GetCdtLibraries().ConvertAll(cdtl => new CandidateCdtLibrary(cdtl)));
            mCandidateCdtLibraryNames = new List<string>(mCandidateCdtLibraries.ConvertAll(cdtlname => cdtlname.OriginalCdtLibrary.Name));
        }

        #region Binding Properties
        public string Name
        {
            get { return mName; }
            set
            {
                mName = value;
                OnPropertyChanged(BindingPropertyNames.TemporaryBdtModel.Name);
            }
        }
        public string Prefix
        {
            get { return mPrefix; }
            set
            {
                mPrefix = value;
                OnPropertyChanged(BindingPropertyNames.TemporaryBdtModel.Prefix);
            }
        }
        public List<string> CandidateBdtLibraryNames
        {
            set
            {
                mCandidateBdtLibraryNames = value;
                OnPropertyChanged(BindingPropertyNames.TemporaryBdtModel.CandidateBdtLibraryNames);
            }

            get
            {
                return mCandidateBdtLibraryNames;
            }
        }
        public List<string> CandidateCdtLibraryNames
        {
            set
            {
                mCandidateCdtLibraryNames = value;
                OnPropertyChanged(BindingPropertyNames.TemporaryBdtModel.CandidateCdtLibraryNames);
            }

            get
            {
                return mCandidateCdtLibraryNames;
            }
        }
        public List<string> CandidateCdtNames
        {
            set
            {
                mCandidateCdtNames = value;
                OnPropertyChanged(BindingPropertyNames.TemporaryBdtModel.CandidateCdtNames);
            }

            get
            {
                return mCandidateCdtNames;
            }
        }
        public List<CheckableItem> CandidateConItems
        {
            set
            {
                mCandidateConItems = value;
                OnPropertyChanged(BindingPropertyNames.TemporaryBdtModel.CandidateConItems);
            }

            get
            {
                return mCandidateConItems;
            }
        }
        public List<CheckableItem> CandidateSupItems
        {
            set
            {
                mCandidateSupItems = value;
                OnPropertyChanged(BindingPropertyNames.TemporaryBdtModel.CandidateSupItems);
            }

            get
            {
                return mCandidateSupItems;
            }
        }

        #endregion

        #region INotifyPropertyChanged Implementation

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(Enum fieldName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(fieldName.ToString()));
            }
        }

        #endregion

        #region ConvertAll Methods
        /// 
        /// <param name="potentialCon"></param>
        
        private static CheckableItem PotentialConToCheckableItem(PotentialCon potentialCon)
        {
            return new CheckableItem(potentialCon.Checked, potentialCon.OriginalCdtCon.Name, true, false, Cursors.Arrow);
        }

        /// 
        /// <param name="potentialSup"></param>
        public CheckableItem PotentialSupToCheckableItem(PotentialSup potentialSup)
        {
            return new CheckableItem(potentialSup.Checked, potentialSup.OriginalCdtSup.Name, true, false, Cursors.Arrow);
        }
        #endregion

        /// 
        /// <param name="selectedBdtLibrary"></param>
        public void setSelectedCandidateBdtLibrary(string selectedBdtLibrary)
        {
            foreach (CandidateBdtLibrary candidateBdtLibrary in mCandidateBdtLibraries)
            {
                if(candidateBdtLibrary.OriginalBdtLibrary.Name.Equals(selectedBdtLibrary))
                {
                    candidateBdtLibrary.Selected = true;
                }
                else
                {
                    candidateBdtLibrary.Selected = false;
                }
            }
        }

        /// 
        /// <param name="selectedCdtLibrary"></param>
        public void setSelectedCandidateCdtLibrary(string selectedCdtLibrary)
        {
            foreach (CandidateCdtLibrary candidateCdtLibrary in mCandidateCdtLibraries)
            {
                if(candidateCdtLibrary.OriginalCdtLibrary.Name.Equals(selectedCdtLibrary))
                {
                    candidateCdtLibrary.Selected = true;
                    CandidateCdtNames = new List<string>(candidateCdtLibrary.CandidateCdts.ConvertAll(cdt => cdt.OriginalCdt.Name));
                }
                else
                {
                    candidateCdtLibrary.Selected = false;
                }
            }
        }

        /// 
        /// <param name="selectedCdt"></param>
        public void setSelectedCandidateCdt(string selectedCdt)
        {
            foreach (CandidateCdtLibrary candidateCdtLibrary in mCandidateCdtLibraries)
            {
                if(candidateCdtLibrary.Selected.Equals(true))
                {
                    foreach (CandidateCdt candidateCdt in candidateCdtLibrary.CandidateCdts)
                    {
                        if(candidateCdt.OriginalCdt.Name.Equals(selectedCdt))
                        {
                            candidateCdt.Selected = true;
                            CandidateConItems = new List<CheckableItem>{PotentialConToCheckableItem(candidateCdt.mPotentialCon)};
                            CandidateSupItems = new List<CheckableItem>(candidateCdt.PotentialSups.ConvertAll(new Converter<PotentialSup,CheckableItem>(PotentialSupToCheckableItem)));
                        }
                        else
                        {
                            candidateCdt.Selected = false;
                        }
                    }
                }
            }
        }

        /// 
        /// <param name="checkedValue"></param>
        /// <param name="selectedSup"></param>
        public void setCheckedPotentialSup(bool checkedValue, string selectedSup)
        {
            foreach (CandidateCdtLibrary candidateCdtLibrary in mCandidateCdtLibraries)
            {
                if (candidateCdtLibrary.Selected.Equals(true))
                {
                    foreach (CandidateCdt candidateCdt in candidateCdtLibrary.CandidateCdts)
                    {
                        if (candidateCdt.Selected.Equals(true))
                        {
                            foreach (PotentialSup potentialSup in candidateCdt.mPotentialSups)
                            {
                                if(potentialSup.OriginalCdtSup.Name.Equals(selectedSup))
                                {
                                    potentialSup.Checked = checkedValue;
                                }
                            }
                        }
                    }
                }
            }
        }

        public void setCheckedAllPotentialSups(bool checkedValue)
        {
            foreach (CandidateCdtLibrary candidateCdtLibrary in mCandidateCdtLibraries)
            {
                if (candidateCdtLibrary.Selected.Equals(true))
                {
                    foreach (CandidateCdt candidateCdt in candidateCdtLibrary.CandidateCdts)
                    {
                        if (candidateCdt.Selected.Equals(true))
                        {
                            foreach (PotentialSup potentialSup in candidateCdt.mPotentialSups)
                            {
                                    potentialSup.Checked = checkedValue;
                            }
                            CandidateSupItems = new List<CheckableItem>(candidateCdt.mPotentialSups.ConvertAll(new Converter<PotentialSup, CheckableItem>(PotentialSupToCheckableItem)));
                        }
                    }
                }
            }
        }

        public void CreateBdt()
        {
            foreach (CandidateBdtLibrary candidateBdtLibrary in mCandidateBdtLibraries)
            {
                if(candidateBdtLibrary.Selected)
                {
                    foreach (IBdt bdt in candidateBdtLibrary.OriginalBdtLibrary.Bdts)
                    {
                        if (bdt.Name.Equals(Prefix + Name))
                        {
                            throw new TemporaryBdtModelException(String.Format("The name of the BDT is invalid since another BDT with the same name already exists. An example for a valid BDT name would be \"New{0}\".", Prefix + Name));
                        }
                    }
                    ICdt basedOnCdt = GetBasedOnCdt();
                    BdtSpec bdtSpec = BdtSpec.CloneCdt(basedOnCdt, Prefix+Name);
                    bdtSpec.Sups = GetSelectedSups(bdtSpec);

                    candidateBdtLibrary.OriginalBdtLibrary.CreateBdt(bdtSpec);
                }
            }
        }

        private List<BdtSupSpec> GetSelectedSups(BdtSpec bdtSpec)
        {
            var bdtSupSpecs = new List<BdtSupSpec>();
            foreach (var checkableItem in CandidateSupItems)
            {
                if(checkableItem.Checked)
                {
                    foreach (var bdtSupSpec in bdtSpec.Sups)
                    {
                        if(bdtSupSpec.Name.Equals(checkableItem.Text))
                        {
                            bdtSupSpecs.Add(bdtSupSpec);
                        }
                    }
                }
            }
            return bdtSupSpecs;
        }

        private ICdt GetBasedOnCdt()
        {
            foreach (CandidateCdtLibrary candidateCdtLibrary in mCandidateCdtLibraries)
            {
                if(candidateCdtLibrary.Selected)
                {
                    foreach (CandidateCdt candidateCdt in candidateCdtLibrary.CandidateCdts)
                    {
                        if(candidateCdt.Selected)
                        {
                            return candidateCdt.OriginalCdt;
                        }  
                    }
                }
            }
            return null;
        }

        public void Reset()
        {
            ccCache.Refresh();
            //this has to be used to reset current UI state:
            //mCandidateBdtLibraries = new List<CandidateBdtLibrary>(ccCache.GetBdtLibraries().ConvertAll(bdtl => new CandidateBdtLibrary(bdtl)));
            //CandidateBdtLibraryNames = new List<string>(mCandidateBdtLibraries.ConvertAll(bdtlname => bdtlname.OriginalBdtLibrary.Name));
            //mCandidateCdtLibraries = new List<CandidateCdtLibrary>(ccCache.GetCdtLibraries().ConvertAll(cdtl => new CandidateCdtLibrary(cdtl)));
            //CandidateCdtLibraryNames = new List<string>(mCandidateCdtLibraries.ConvertAll(cdtlname => cdtlname.OriginalCdtLibrary.Name));
            //CandidateCdtNames = new List<string>();
            //CandidateSupItems = new List<CheckableItem>();
            //CandidateConItems = new List<CheckableItem>();
        }
    }
}