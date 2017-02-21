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
using CctsRepository;
using CctsRepository.BdtLibrary;
using CctsRepository.BieLibrary;
using CctsRepository.CcLibrary;
using VIENNAAddIn.upcc3.Wizards.dev.binding;
using VIENNAAddIn.upcc3.Wizards.dev.cache;
using VIENNAAddIn.upcc3.Wizards.dev.temporarymodel.abiemodel.exceptions;
using VIENNAAddIn.upcc3.Wizards.dev.util;

namespace VIENNAAddIn.upcc3.Wizards.dev.temporarymodel.abiemodel
{
    public class TemporaryAbieModel : TemporaryModel, INotifyPropertyChanged
    {        
        #region Backing Fields for Binding Properties

        private string mAbieName;
        private string mAbiePrefix;
        private List<string> mCandidateCcLibraryNames;
        private List<string> mCandidateAccNames;
        private List<string> mCandidateBdtLibraryNames;
        private List<string> mCandidateBieLibraryNames;
        private List<CheckableItem> mCandidateBccItems;
        private List<CheckableItem> mPotentialBbieItems;
        private List<CheckableItem> mPotentialBdtItems;
        private List<CheckableItem> mCandidateAbieItems;
        private List<CheckableItem> mPotentialAsbieItems;
        #endregion
        
        #region Class Fields

        private readonly CcCache ccCache;
        private readonly ProspectiveBdts mProspectiveBdts;
        private List<CandidateCcLibrary> mCandidateCcLibraries;
        private List<CandidateBdtLibrary> mCandidateBdtLibraries;
        private List<CandidateBieLibrary> mCandidateBieLibraries;

        #endregion
        

        public TemporaryAbieModel(ICctsRepository cctsRepository)
        {
            ccCache = CcCache.GetInstance(cctsRepository);
            mProspectiveBdts = ProspectiveBdts.GetInstance();

            mCandidateCcLibraries = new List<CandidateCcLibrary>(ccCache.GetCcLibraries().ConvertAll(ccl => new CandidateCcLibrary(ccl)));
            CandidateCcLibraryNames = new List<string>(mCandidateCcLibraries.ConvertAll(new Converter<CandidateCcLibrary, string>(CandidateCcLibraryToString)));

            mAbiePrefix = "Qualified";

            mCandidateBdtLibraries = new List<CandidateBdtLibrary>(ccCache.GetBdtLibraries().ConvertAll(bdtl => new CandidateBdtLibrary(bdtl)));
            CandidateBdtLibraryNames = new List<string>(mCandidateBdtLibraries.ConvertAll(new Converter<CandidateBdtLibrary, string>(CandidateBdtLibraryToString)));

            mCandidateBieLibraries = new List<CandidateBieLibrary>(ccCache.GetBieLibraries().ConvertAll(biel => new CandidateBieLibrary(biel)));
            CandidateBieLibraryNames = new List<string>(mCandidateBieLibraries.ConvertAll(new Converter<CandidateBieLibrary, string>(CandidateBieLibraryToString)));
        }

        public TemporaryAbieModel(ICctsRepository cctsRepository, IAbie abieToBeUpdated)
        {
            ccCache = CcCache.GetInstance(cctsRepository);
            mProspectiveBdts = ProspectiveBdts.GetInstance();

            // Populate the model with the appropriate BIE library which contains the ABIE to be updated.
            foreach (IBieLibrary bieLibrary in ccCache.GetBieLibraries())
            {
                if ((bieLibrary.GetAbieByName(abieToBeUpdated.Name)) != null)
                {
                    List<IBieLibrary> bieLibraries = new List<IBieLibrary> {bieLibrary};
                    mCandidateBieLibraries = new List<CandidateBieLibrary>(bieLibraries.ConvertAll(biel => new CandidateBieLibrary(biel)));
                    mCandidateBieLibraries[0].Selected = true;
                }
            }

            // Populate the model with the appropriate CC library which contains the ACC that the ABIE to be updated is based on.
            foreach (ICcLibrary ccLibrary in ccCache.GetCcLibraries())
            {
                foreach (IAcc acc in ccLibrary.Accs)
                {
                    if (abieToBeUpdated.BasedOn.Id == acc.Id)                    
                    {
                        // Populate the model with the appropriate CC Library which contains the ACC that the ABIE (to be updated) is based on.
                        List<ICcLibrary> targetCcLibraries = new List<ICcLibrary>() { ccLibrary };
                        mCandidateCcLibraries = new List<CandidateCcLibrary>(targetCcLibraries.ConvertAll(ccl => new CandidateCcLibrary(ccl)));
                        mCandidateCcLibraries[0].Selected = true;

                        // Populate the model with the appropriate ACC that the ABIE (to be updated) is based on.
                        List<CandidateAcc> candidateAccs = new List<CandidateAcc>() { new CandidateAcc(acc) };
                        mCandidateCcLibraries[0].CandidateAccs = candidateAccs;
                        mCandidateCcLibraries[0].CandidateAccs[0].Selected = true;

                        #region Populate BCCs, BBIEs, and BDTs
                        // Populating the model with the available BCCs for the ABIE is automatically
                        // triggered by accessing the property "CandidateBccs". However, it is necessary
                        // to populate the model with the BBIEs which already exist in the ABIE to be 
                        // updated.                        
                        foreach (CandidateBcc candidateBcc in candidateAccs[0].CandidateBccs)
                        {
                            List<PotentialBbie> potentialBbies = new List<PotentialBbie>();

                            foreach (IBbie bbie in abieToBeUpdated.Bbies)
                            {
                                // We need to match the BBIEs which belong to the particular BCC which 
                                // is currently being processed. To do so, we need to extract the name
                                // of the BCC, that the BBIE is based on, from the name of the BBIE. The
                                // "_" used to separate qualifiers from names allows us to extract the 
                                // original name of the BCC. If none is found then we can assume that the
                                // name of the BBIE is the same as the BCC that it is based on. Having
                                // the name of the BCC at hand we can then compare it to the currently 
                                // processed BCC. The name is then used to perform the matching. 
                                string nameOfTheBccThatTheBbieIsBasedOn = "";
                                int indexOfQualifierSeparator = bbie.Name.LastIndexOf('_');

                                if (indexOfQualifierSeparator == -1)
                                {
                                    nameOfTheBccThatTheBbieIsBasedOn = bbie.Name;
                                }
                                else
                                {
                                    nameOfTheBccThatTheBbieIsBasedOn = bbie.Name.Substring(indexOfQualifierSeparator + 1);
                                }
                                
                                if (nameOfTheBccThatTheBbieIsBasedOn.Equals(candidateBcc.OriginalBcc.Name))
                                {
                                    candidateBcc.Checked = true;
                                    
                                    PotentialBbie potentialBbie = new PotentialBbie(bbie.Name, candidateBcc.OriginalBcc.Cdt);
                                    potentialBbie.Checked = true;

                                    // Furthermore, it is necessary to add the appropriate list of 
                                    // BDTs available to typify the current BBIE. Also, among the list
                                    // of available BDTs we need to select the BDT which is currently
                                    // used to typify the BDT. 
                                    List<PotentialBdt> potentialBdts = new List<PotentialBdt>();

                                    foreach (IBdtLibrary bdtLibrary in ccCache.GetBdtLibraries())
                                    {
                                        foreach (IBdt bdt in bdtLibrary.Bdts)
                                        {
                                            if (bdt.BasedOn.Id == bbie.Bdt.BasedOn.Id)
                                            {
                                                PotentialBdt potentialBdt = new PotentialBdt(bdt);

                                                if (bdt.Id == bbie.Bdt.Id)
                                                {
                                                    potentialBdt.Checked = true;
                                                }

                                                potentialBdts.Add(potentialBdt);
                                            }
                                        }
                                    }
                                    
                                    potentialBbie.PotentialBdts = potentialBdts;

                                    potentialBbies.Add(potentialBbie);
                                }
                            }

                            if (potentialBbies.Count > 0)
                            {
                                candidateBcc.PotentialBbies = potentialBbies;
                            }
                        }
                        #endregion

                        #region Populate ASBIEs

                        List<CandidateAbie> candidateAbies = new List<CandidateAbie>();
                        HashSet<string> abiesAlreadyInCandidateAbies = new HashSet<string>();

                        foreach (IAscc ascc in acc.Asccs)
                        {
                            IAcc associatedAcc = ascc.AssociatedAcc;

                            foreach (IBieLibrary bieLibrary in ccCache.GetBieLibraries())
                            {
                                foreach (IAbie abie in bieLibrary.Abies)
                                {
                                    if (abie.BasedOn.Id == associatedAcc.Id)
                                    {
                                        CandidateAbie candidateAbie = new CandidateAbie(abie) {PotentialAsbies = new List<PotentialAsbie>()};

                                        if (!abiesAlreadyInCandidateAbies.Contains(abie.Name))
                                        {
                                            candidateAbies.Add(candidateAbie);
                                            abiesAlreadyInCandidateAbies.Add(abie.Name);
                                        }
                                        else
                                        {
                                            foreach (CandidateAbie existingCandidateAbie in candidateAbies)
                                            {
                                                if (existingCandidateAbie.Name == abie.Name)
                                                {
                                                    candidateAbie = existingCandidateAbie;
                                                    break;
                                                }
                                            }
                                        }


                                        
                                        PotentialAsbie potentialAsbie = new PotentialAsbie(ascc);

                                        foreach (IAsbie asbie in abieToBeUpdated.Asbies)
                                        {
                                            if (asbie.Name == ascc.Name)
                                            {
                                                potentialAsbie.Checked = true;
                                                candidateAbie.Checked = true;
                                            }
                                        }
                                        candidateAbie.PotentialAsbies.Add(potentialAsbie);
                                        
                                        break;
                                    }
                                }
                            }
                        }

                        mCandidateCcLibraries[0].CandidateAccs[0].CandidateAbies = candidateAbies;

                        #endregion
                    }
                }
            }

            mCandidateBdtLibraries = new List<CandidateBdtLibrary>(ccCache.GetBdtLibraries().ConvertAll(bdtl => new CandidateBdtLibrary(bdtl)));

            CandidateCcLibraryNames = new List<string>(mCandidateCcLibraries.ConvertAll(new Converter<CandidateCcLibrary, string>(CandidateCcLibraryToString)));            
            CandidateBdtLibraryNames = new List<string>(mCandidateBdtLibraries.ConvertAll(new Converter<CandidateBdtLibrary, string>(CandidateBdtLibraryToString)));
            CandidateBieLibraryNames = new List<string>(mCandidateBieLibraries.ConvertAll(new Converter<CandidateBieLibrary, string>(CandidateBieLibraryToString)));

            // Populate the textfields containing the name of the ABIE as well as the prefix of the ABIE
            int indexOfAbieQualifierSeparator = abieToBeUpdated.Name.LastIndexOf('_');

            if (indexOfAbieQualifierSeparator == -1)
            {
                mAbiePrefix = "";
                mAbieName = abieToBeUpdated.Name;
            }
            else
            {
                mAbiePrefix = abieToBeUpdated.Name.Substring(0, indexOfAbieQualifierSeparator);
                mAbieName = abieToBeUpdated.Name;
            }


        }

        #region Binding Properties

        public string AbieName
        {
            get { return mAbieName; }
            set
            {
                mAbieName = value;
                OnPropertyChanged(BindingPropertyNames.TemporaryAbieModel.AbieName);                
            }
        }

        public string AbiePrefix
        {
            get { return mAbiePrefix; }
            set
            {
                mAbiePrefix = value;
                OnPropertyChanged(BindingPropertyNames.TemporaryAbieModel.AbiePrefix);
            }
        }

        public List<string> CandidateCcLibraryNames
        {
            set
            {
                mCandidateCcLibraryNames = value;
                OnPropertyChanged(BindingPropertyNames.TemporaryAbieModel.CandidateCcLibraryNames);
            }

            get
            {
                return mCandidateCcLibraryNames;
            }
        }

        public List<string> CandidateAccNames
        {
            set
            {
                mCandidateAccNames = value;
                OnPropertyChanged(BindingPropertyNames.TemporaryAbieModel.CandidateAccNames);
            }

            get
            {
                return mCandidateAccNames;
            }
        }

        public List<string> CandidateBdtLibraryNames
        {
            set
            {
                mCandidateBdtLibraryNames = value;
                OnPropertyChanged(BindingPropertyNames.TemporaryAbieModel.CandidateBdtLibraryNames);
            }

            get
            {
                return mCandidateBdtLibraryNames;                
            }
        }

        public List<string> CandidateBieLibraryNames
        {
            set
            {
                mCandidateBieLibraryNames = value;
                OnPropertyChanged(BindingPropertyNames.TemporaryAbieModel.CandidateBieLibraryNames);
            }

            get
            {
                return mCandidateBieLibraryNames;
            }
        }

        public List<CheckableItem> CandidateBccItems
        {
            set
            {
                mCandidateBccItems = value;
                OnPropertyChanged(BindingPropertyNames.TemporaryAbieModel.CandidateBccItems);
            }

            get
            {
                return mCandidateBccItems;
            }
        }

        public List<CheckableItem> PotentialBbieItems
        {
            set
            {
                mPotentialBbieItems = value;
                OnPropertyChanged(BindingPropertyNames.TemporaryAbieModel.PotentialBbieItems);
            }

            get
            {
                return mPotentialBbieItems;
            }
        }

        public List<CheckableItem> PotentialBdtItems
        {
            set
            {
                mPotentialBdtItems = value;
                OnPropertyChanged(BindingPropertyNames.TemporaryAbieModel.PotentialBdtItems);
            }

            get
            {
                return mPotentialBdtItems;
            }
        }

        public List<CheckableItem> CandidateAbieItems
        {
            set
            {
                mCandidateAbieItems = value;
                OnPropertyChanged(BindingPropertyNames.TemporaryAbieModel.CandidateAbieItems);
            }

            get
            {
                return mCandidateAbieItems;
            }
        }

        public List<CheckableItem> PotentialAsbieItems
        {
            set
            {
                mPotentialAsbieItems = value;
                OnPropertyChanged(BindingPropertyNames.TemporaryAbieModel.PotentialAsbieItems);
            }

            get
            {
                return mPotentialAsbieItems;
            }
        }

        #endregion

      
        #region ConvertAll Methods

        private static string CandidateCcLibraryToString(CandidateCcLibrary candidateCcLibrary)
        {
            return candidateCcLibrary.OriginalCcLibrary.Name;
        }

        private static string CandidateAccToString(CandidateAcc candidateAcc)
        {
            return candidateAcc.OriginalAcc.Name;
        }

        private static CheckableItem CandidateBccToCheckableItem(CandidateBcc candidateBcc)
        {
            return new CheckableItem(candidateBcc.Checked, candidateBcc.OriginalBcc.Name, candidateBcc.ItemReadOnly, candidateBcc.ItemFocusable, candidateBcc.ItemCursor);
        }

        private static CheckableItem PotentialBbieToCheckableText(PotentialBbie potentialBbie)
        {
            return new CheckableItem(potentialBbie.Checked, potentialBbie.Name, potentialBbie.ItemReadOnly, potentialBbie.ItemFocusable, potentialBbie.ItemCursor);
        }

        private static CheckableItem PotentialBdtToTestItem(PotentialBdt potentialBdt)
        {            
            return new CheckableItem(potentialBdt.Checked, potentialBdt.Name, potentialBdt.ItemReadOnly, potentialBdt.ItemFocusable, potentialBdt.ItemCursor);
        }

        private static CheckableItem CandidateAbieToCheckableItem(CandidateAbie candidateAbie)
        {
            return new CheckableItem(candidateAbie.Checked, candidateAbie.OriginalAbie.Name, candidateAbie.ItemReadOnly, candidateAbie.ItemFocusable, candidateAbie.ItemCursor);
        }

        private static CheckableItem PotentialAsbieToCheckableItem(PotentialAsbie potentialAsbie)
        {
            return new CheckableItem(potentialAsbie.Checked, potentialAsbie.Name, potentialAsbie.ItemReadOnly, potentialAsbie.ItemFocusable, potentialAsbie.ItemCursor);
        }

        private static string CandidateBdtLibraryToString(CandidateBdtLibrary candidateBdtLibrary)
        {
            return candidateBdtLibrary.OriginalBdtLibrary.Name;
        }

        private static string CandidateBieLibraryToString(CandidateBieLibrary candidateBieLibrary)
        {
            return candidateBieLibrary.OriginalBieLibrary.Name;
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

        
        public void SetSelectedCandidateCcLibrary(string selectedCcLibrary)
        {
            foreach (CandidateCcLibrary candidateCCLibrary in mCandidateCcLibraries)
            {
                if (candidateCCLibrary.OriginalCcLibrary.Name.Equals(selectedCcLibrary))
                {
                    candidateCCLibrary.Selected = true;

                    CandidateAccNames = new List<string>(candidateCCLibrary.CandidateAccs.ConvertAll(new Converter<CandidateAcc, string>(CandidateAccToString)));                        
                }
                else
                {
                    candidateCCLibrary.Selected = false;
                }
            }
        }	        

        public void SetSelectedCandidateAcc(string selectedAcc)
        {
            foreach (CandidateCcLibrary candidateCcLibrary in mCandidateCcLibraries)
            {
                if (candidateCcLibrary.Selected)
                {
                    foreach (CandidateAcc candidateAcc in candidateCcLibrary.CandidateAccs)
                    {
                        if (candidateAcc.OriginalAcc.Name.Equals(selectedAcc))
                        {
                            candidateAcc.Selected = true;

                            CandidateBccItems = new List<CheckableItem>(candidateAcc.CandidateBccs.ConvertAll(new Converter<CandidateBcc, CheckableItem>(CandidateBccToCheckableItem)));
                            CandidateAbieItems = new List<CheckableItem>(candidateAcc.CandidateAbies.ConvertAll(new Converter<CandidateAbie, CheckableItem>(CandidateAbieToCheckableItem)));
                        }
                        else
                        {
                            candidateAcc.Selected = false;
                        }
                    }
                }
            }             
        }

        public void SetDefaultChecked(CandidateBcc candidateBcc)
        {
            int index = 0;
            int indexCheckedBbie = -1;

            foreach (PotentialBbie potentialBbie in candidateBcc.PotentialBbies)
            {
                if (potentialBbie.Checked)
                {
                    indexCheckedBbie = index;
                    break;
                }
                
                index++;
            }

            if (indexCheckedBbie == -1)
            {
                indexCheckedBbie = 0;
                candidateBcc.PotentialBbies[indexCheckedBbie].Checked = true;
            }
                        
            SetDefaultChecked(candidateBcc.PotentialBbies[indexCheckedBbie]);            
        }

        public void SetDefaultChecked(PotentialBbie potentialBbie)
        {
            int index = 0;
            int indexCheckedBdt = -1;

            foreach (PotentialBdt potentialBdt in potentialBbie.PotentialBdts)
            {
                if (potentialBdt.Checked)
                {
                    indexCheckedBdt = index;
                    break;
                }

                index++;
            }

            if (indexCheckedBdt == -1)
            {
                indexCheckedBdt = 0;
                potentialBbie.PotentialBdts[indexCheckedBdt].Checked = true;                
            }
        }

        public void SetDefaultChecked(CandidateAbie candidateAbie)
        {
            int index = 0;
            int indexCheckedAsbie = -1;

            foreach (PotentialAsbie potentialAsbie in candidateAbie.PotentialAsbies)
            {
                if (potentialAsbie.Checked)                
                {
                    indexCheckedAsbie = index;
                    break;
                }

                index++;
            }

            if (indexCheckedAsbie == -1)
            {
                indexCheckedAsbie = 0;
                candidateAbie.PotentialAsbies[indexCheckedAsbie].Checked = true;                
            }
        }

        public void SetSelectedAndCheckedCandidateBcc(string selectedBcc, bool checkedValue)
        {
            foreach (CandidateCcLibrary candidateCcLibrary in mCandidateCcLibraries)
            {
                if (candidateCcLibrary.Selected)
                {
                    foreach (CandidateAcc candidateAcc in candidateCcLibrary.CandidateAccs)
                    {
                        if (candidateAcc.Selected)
                        {
                            foreach (CandidateBcc candidateBcc in candidateAcc.CandidateBccs)
                            {
                                if (candidateBcc.OriginalBcc.Name.Equals(selectedBcc))
                                {
                                    candidateBcc.Selected = true;

                                    candidateBcc.Checked = checkedValue;
                                    
                                    if (checkedValue)
                                    {
                                        SetDefaultChecked(candidateBcc);   
                                    }
                                    
                                    PotentialBbieItems = new List<CheckableItem>(candidateBcc.PotentialBbies.ConvertAll(new Converter<PotentialBbie, CheckableItem>(PotentialBbieToCheckableText)));
                                }
                                else
                                {
                                    candidateBcc.Selected = false;
                                }
                            }
                        }
                    }
                }
            }
        }

        public void SetCheckedForAllCandidateBccs(bool checkedValue)
        {
            foreach (CandidateCcLibrary candidateCcLibrary in mCandidateCcLibraries)
            {
                if (candidateCcLibrary.Selected)
                {
                    foreach (CandidateAcc candidateAcc in candidateCcLibrary.CandidateAccs)
                    {
                        if (candidateAcc.Selected)
                        {
                            foreach (CandidateBcc candidateBcc in candidateAcc.CandidateBccs)
                            {
                                candidateBcc.Checked = checkedValue;
                                foreach (PotentialBbie potentialBbie in candidateBcc.PotentialBbies)
                                {
                                    potentialBbie.Checked = checkedValue;
                                    
                                    //only check first Bdt!
                                    potentialBbie.PotentialBdts[0].Checked = checkedValue;
                                    //foreach (PotentialBdt potentialBdt in potentialBbie.PotentialBdts)
                                    //{
                                    //    potentialBdt.Checked = checkedValue;
                                    //}
                                }
                            }

                            CandidateBccItems = new List<CheckableItem>(candidateAcc.CandidateBccs.ConvertAll(new Converter<CandidateBcc, CheckableItem>(CandidateBccToCheckableItem)));
                        }
                    }
                }
            }
        }

        public void SetSelectedAndCheckedPotentialBbie(string selectedBbie, bool checkedValue)
        {
            foreach (CandidateCcLibrary candidateCcLibrary in mCandidateCcLibraries)
            {
                if (candidateCcLibrary.Selected)
                {
                    foreach (CandidateAcc candidateAcc in candidateCcLibrary.CandidateAccs)
                    {
                        if (candidateAcc.Selected)
                        {
                            foreach (CandidateBcc candidateBcc in candidateAcc.CandidateBccs)
                            {
                                if (candidateBcc.Selected)
                                {
                                    foreach (PotentialBbie potentialBbie in candidateBcc.PotentialBbies)
                                    {
                                        if (potentialBbie.Name.Equals(selectedBbie))
                                        {
                                            potentialBbie.Selected = true;

                                            potentialBbie.Checked = checkedValue;
                                            
                                            if (checkedValue)
                                            {
                                                SetDefaultChecked(potentialBbie); 
                                            }

                                            PotentialBdtItems = new List<CheckableItem>(potentialBbie.PotentialBdts.ConvertAll(new Converter<PotentialBdt, CheckableItem>(PotentialBdtToTestItem)));
                                        }
                                        else
                                        {
                                            potentialBbie.Selected = false;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public void SetSelectedAndCheckedPotentialBdt(string selectedBdt, bool? checkedValue)
        {
            foreach (CandidateCcLibrary candidateCcLibrary in mCandidateCcLibraries)
            {
                if (candidateCcLibrary.Selected)
                {
                    foreach (CandidateAcc candidateAcc in candidateCcLibrary.CandidateAccs)
                    {
                        if (candidateAcc.Selected)
                        {
                            foreach (CandidateBcc candidateBcc in candidateAcc.CandidateBccs)
                            {
                                if (candidateBcc.Selected)
                                {
                                    foreach (PotentialBbie potentialBbie in candidateBcc.PotentialBbies)
                                    {
                                        if (potentialBbie.Selected)
                                        {
                                            foreach (PotentialBdt potentialBdt in potentialBbie.PotentialBdts)
                                            {
                                                if (potentialBdt.Name.Equals(selectedBdt))
                                                {
                                                    potentialBdt.Selected = true;

                                                    if (checkedValue.HasValue)
                                                    {
                                                        potentialBdt.Checked = checkedValue.Value;    
                                                    }                                                    
                                                }
                                                else
                                                {
                                                    potentialBdt.Selected = false;

                                                    if ((checkedValue.HasValue) && (checkedValue.Value))
                                                    {
                                                        potentialBdt.Checked = false;
                                                    }

                                                    potentialBdt.Checked = false;
                                                }
                                            }

                                            if (checkedValue.HasValue)
                                            {
                                                PotentialBdtItems = new List<CheckableItem>(potentialBbie.PotentialBdts.ConvertAll(new Converter<PotentialBdt, CheckableItem>(PotentialBdtToTestItem)));      
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public void SetSelectedAndCheckedCandidateAbie(string selectedAbie, bool checkedValue)
        {
            foreach (CandidateCcLibrary candidateCcLibrary in mCandidateCcLibraries)
            {
                if (candidateCcLibrary.Selected)
                {
                    foreach (CandidateAcc candidateAcc in candidateCcLibrary.CandidateAccs)
                    {
                        if (candidateAcc.Selected)
                        {
                            foreach (CandidateAbie candidateAbie in candidateAcc.CandidateAbies)
                            {
                                if (candidateAbie.OriginalAbie.Name.Equals(selectedAbie))
                                {
                                    candidateAbie.Selected = true;

                                    candidateAbie.Checked = checkedValue;

                                    if (checkedValue)
                                    {
                                        SetDefaultChecked(candidateAbie);
                                    }
                                    
                                    PotentialAsbieItems = new List<CheckableItem>(candidateAbie.PotentialAsbies.ConvertAll(new Converter<PotentialAsbie, CheckableItem>(PotentialAsbieToCheckableItem)));
                                }
                                else
                                {
                                    candidateAbie.Selected = false;
                                }
                            }
                        }
                    }
                }
            }
        }

        public void SetNoSelectedCandidateAbie()
        {
            CandidateAbieItems = new List<CheckableItem>();
            PotentialAsbieItems = new List<CheckableItem>();            
        }

        public void SetSelectedAndCheckedPotentialAsbie(string selectedAsbie, bool checkedValue)
        {
            foreach (CandidateCcLibrary candidateCcLibrary in mCandidateCcLibraries)
            {
                if (candidateCcLibrary.Selected)
                {
                    foreach (CandidateAcc candidateAcc in candidateCcLibrary.CandidateAccs)
                    {
                        if (candidateAcc.Selected)
                        {
                            foreach (CandidateAbie candidateAbie in candidateAcc.CandidateAbies)
                            {
                                foreach (PotentialAsbie potentialAsbie in candidateAbie.PotentialAsbies)
                                {
                                    if (potentialAsbie.Name.Equals(selectedAsbie))
                                    {
                                        potentialAsbie.Selected = true;

                                        potentialAsbie.Checked = checkedValue;
                                    }
                                    else
                                    {
                                        potentialAsbie.Selected = false;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public void SetSelectedCandidateBdtLibrary(string selectedBdtLibrary)
        {
            foreach (CandidateBdtLibrary candidateBdtLibrary in mCandidateBdtLibraries)
            {
                if (candidateBdtLibrary.OriginalBdtLibrary.Name.Equals(selectedBdtLibrary))
                {
                    candidateBdtLibrary.Selected = true;
                }
                else
                {
                    candidateBdtLibrary.Selected = false;
                }
            }
        }

        public void SetSelectedCandidateBieLibrary(string selectedBieLibrary)
        {
            foreach (CandidateBieLibrary candidateBieLibrary in mCandidateBieLibraries)
            {
                if (candidateBieLibrary.OriginalBieLibrary.Name.Equals(selectedBieLibrary))
                {
                    candidateBieLibrary.Selected = true;
                }
                else
                {
                    candidateBieLibrary.Selected = false;
                }
            }
        }

        public void AddPotentialBbie()
        {
            foreach (CandidateCcLibrary candidateCcLibrary in mCandidateCcLibraries)
            {
                if (candidateCcLibrary.Selected)
                {
                    foreach (CandidateAcc candidateAcc in candidateCcLibrary.CandidateAccs)
                    {
                        if (candidateAcc.Selected)
                        {
                            foreach (CandidateBcc candidateBcc in candidateAcc.CandidateBccs)
                            {
                                if (candidateBcc.Selected)
                                {
                                    candidateBcc.AddPotentialBbie();                                    

                                    PotentialBbieItems = new List<CheckableItem>(candidateBcc.PotentialBbies.ConvertAll(new Converter<PotentialBbie, CheckableItem>(PotentialBbieToCheckableText)));
                                    
                                    return;
                                }
                            }
                        }
                    }
                }
            }            
        }

        public void UpdateBbieName(string updatedBbieName)
        {
            foreach (CandidateCcLibrary candidateCcLibrary in mCandidateCcLibraries)
            {
                if (candidateCcLibrary.Selected)
                {
                    foreach (CandidateAcc candidateAcc in candidateCcLibrary.CandidateAccs)
                    {
                        if (candidateAcc.Selected)
                        {
                            foreach (CandidateBcc candidateBcc in candidateAcc.CandidateBccs)
                            {
                                if (candidateBcc.Selected)
                                {
                                    if (updatedBbieName.EndsWith(candidateBcc.OriginalBcc.Name))
                                    {
                                        foreach (PotentialBbie potentialBbie in candidateBcc.PotentialBbies)
                                        {
                                            if (!(potentialBbie.Selected) && (potentialBbie.Name.Equals(updatedBbieName)))
                                            {
                                                throw new TemporaryAbieModelException(String.Format("The name of the BBIE is invalid since another BBIE with the same name already exists. An example for a valid BBIE name would be \"New{0}\".", updatedBbieName));                                                
                                            }
                                        }

                                        foreach (PotentialBbie potentialBbie in candidateBcc.PotentialBbies)
                                        {
                                            if (potentialBbie.Selected)
                                            {
                                                potentialBbie.Name = updatedBbieName;
                                                return;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        throw new TemporaryAbieModelException(String.Format("The name of the BBIE is invalid since it must end with the name of the BCC that it is based on. An example for a valid BBIE name would be \"New{0}\".", candidateBcc.OriginalBcc.Name));
                                    }                                    
                                }
                            }
                        }
                    }
                }
            }            
        }

        public void UpdateBdtName(string updatedBdtName)
        {
            foreach (CandidateCcLibrary candidateCcLibrary in mCandidateCcLibraries)
            {
                if (candidateCcLibrary.Selected)
                {
                    foreach (CandidateAcc candidateAcc in candidateCcLibrary.CandidateAccs)
                    {
                        if (candidateAcc.Selected)
                        {
                            string originalBdtName = "";

                            foreach (CandidateBcc candidateBcc in candidateAcc.CandidateBccs)
                            {
                                if (candidateBcc.Selected)
                                {
                                    foreach (PotentialBbie potentialBbie in candidateBcc.PotentialBbies)
                                    {
                                        if (potentialBbie.Selected)
                                        {
                                            foreach (PotentialBdt potentialBdt in potentialBbie.PotentialBdts)
                                            {
                                                if (!(potentialBdt.Selected) && (potentialBdt.Name.Equals(updatedBdtName)))
                                                {
                                                    throw new TemporaryAbieModelException(String.Format("The name of the BDT is invalid since another BDT with the same name already exists. An example for a valid BDT name would be \"New{0}\".", updatedBdtName));
                                                }
                                            }

                                            foreach (PotentialBdt potentialBdt in potentialBbie.PotentialBdts)
                                            {
                                                if (potentialBdt.Selected)
                                                {
                                                    originalBdtName = potentialBdt.Name;

                                                    potentialBdt.Name = updatedBdtName;
                                                }
                                            }                                            
                                        }
                                    }                                    
                                }
                            }

                            foreach (CandidateBcc candidateBcc in candidateAcc.CandidateBccs)
                            {
                                foreach (PotentialBbie potentialBbie in candidateBcc.PotentialBbies)
                                {
                                    foreach (PotentialBdt potentialBdt in potentialBbie.PotentialBdts)
                                    {
                                        if (potentialBdt.Name.Equals(originalBdtName))
                                        {
                                            potentialBdt.Name = updatedBdtName;
                                        }
                                    }
                                }
                            }

                            mProspectiveBdts.UpdateBdtName(originalBdtName, updatedBdtName);
                        }
                    }
                }
            }
        }

        public void AddPotentialBdt()
        {
            string newBdtName = "";            
            int cdtIdThatNewBdtIsAddedFor = 0;

            foreach (CandidateCcLibrary candidateCcLibrary in mCandidateCcLibraries)
            {
                if (candidateCcLibrary.Selected)
                {
                    foreach (CandidateAcc candidateAcc in candidateCcLibrary.CandidateAccs)
                    {
                        if (candidateAcc.Selected)
                        {
                            foreach (CandidateBcc candidateBcc in candidateAcc.CandidateBccs)
                            {
                                if (candidateBcc.Selected)
                                {
                                    cdtIdThatNewBdtIsAddedFor = candidateBcc.OriginalBcc.Cdt.Id;

                                    foreach (PotentialBbie potentialBbie in candidateBcc.PotentialBbies)
                                    {
                                        if (potentialBbie.Selected)
                                        {
                                            newBdtName = potentialBbie.AddPotentialBdt();
                                           
                                            PotentialBdtItems = new List<CheckableItem>(potentialBbie.PotentialBdts.ConvertAll(new Converter<PotentialBdt, CheckableItem>(PotentialBdtToTestItem)));
                                        }
                                    }
                                    
                                    foreach (PotentialBbie potentialBbie in candidateBcc.PotentialBbies)
                                    {
                                        if (!potentialBbie.Selected)
                                        {
                                            potentialBbie.AddPotentialBdt(newBdtName);                                            
                                        }
                                    }

                                    ProspectiveBdts.GetInstance().AddBdt(cdtIdThatNewBdtIsAddedFor, newBdtName);
                                }
                            }
                            
                            // Propagate the new BDT to all other BCCs (i.e. BBIEs) of the selected ACC.
                            foreach (CandidateBcc candidateBcc in candidateAcc.CandidateBccs)
                            {
                                if ((candidateBcc.OriginalBcc.Cdt.Id == cdtIdThatNewBdtIsAddedFor) && (!candidateBcc.Selected))
                                {
                                    foreach (PotentialBbie potentialBbie in candidateBcc.PotentialBbies)
                                    {
                                       potentialBbie.AddPotentialBdt(newBdtName);                                            
                                    }
                                }
                            }                            
                        }
                    }
                }
            }               
        }

        public bool ContainsValidConfiguration()
        {
            foreach (CandidateCcLibrary candidateCcLibrary in mCandidateCcLibraries)
            {
                if (candidateCcLibrary.Selected)
                {
                    foreach (CandidateAcc candidateAcc in candidateCcLibrary.CandidateAccs)
                    {
                        if (candidateAcc.Selected)
                        {
                            foreach (CandidateBcc candidateBcc in candidateAcc.CandidateBccs)
                            {
                                if (candidateBcc.Checked)
                                {
                                    foreach (PotentialBbie potentialBbie in candidateBcc.PotentialBbies)
                                    {
                                        if (potentialBbie.Checked)
                                        {
                                            foreach (PotentialBdt potentialBdt in potentialBbie.PotentialBdts)
                                            {
                                                if (potentialBdt.Checked)
                                                {
                                                    return true;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return false;
        }

        public void CreateAbie()
        {
            foreach (CandidateCcLibrary candidateCcLibrary in mCandidateCcLibraries)
            {
                if (candidateCcLibrary.Selected)
                {
                    foreach (CandidateAcc candidateAcc in candidateCcLibrary.CandidateAccs)
                    {
                        if (candidateAcc.Selected)
                        {
                            CreateBdts(candidateAcc);
                            CreateAbie(candidateAcc);

                            ccCache.Refresh();
                            mProspectiveBdts.Clear();
                        }
                        else
                        {
                            candidateAcc.Clear();
                        }
                    }
                }
            }            
        }

        public void UpdateAbie(IAbie abieToBeUpdated)
        {
            foreach (CandidateCcLibrary candidateCcLibrary in mCandidateCcLibraries)
            {
                if (candidateCcLibrary.Selected)
                {
                    foreach (CandidateAcc candidateAcc in candidateCcLibrary.CandidateAccs)
                    {
                        if (candidateAcc.Selected)
                        {
                            CreateBdts(candidateAcc);
                            UpdateAbie(abieToBeUpdated, candidateAcc);

                            ccCache.Refresh();
                            mProspectiveBdts.Clear();
                        }
                        else
                        {
                            candidateAcc.Clear();
                        }
                    }
                }
            }  
        }

        private void UpdateAbie(IAbie abieToBeUpdated, CandidateAcc candidateAcc)
        {
            List<BbieSpec> bbieSpecs = CumulateBbieSpecs(candidateAcc);
            List<AsbieSpec> asbieSpecs = CumulateAsbieSpecs(candidateAcc);            
            AbieSpec abieSpec = CumulateAbieSpec(candidateAcc);

            // The qualifier of the ABIE, if there is one, is already contained in the AbieName. 
            abieSpec.Name = AbieName;
            abieSpec.Bbies = bbieSpecs;
            abieSpec.Asbies = asbieSpecs;

            UpdateAbieInBieLibrary(abieSpec, abieToBeUpdated);  
        }

        private void UpdateAbieInBieLibrary(AbieSpec abieSpec, IAbie abieToBeUpdated)
        {
            foreach (CandidateBieLibrary candidateBieLibrary in mCandidateBieLibraries)
            {
                if (candidateBieLibrary.Selected)
                {
                    candidateBieLibrary.OriginalBieLibrary.UpdateAbie(abieToBeUpdated, abieSpec);
                }
            }
        }

        private void CreateBdts(CandidateAcc candidateAcc)
        {
            foreach (CandidateBcc candidateBcc in candidateAcc.CandidateBccs)
            {
                if (candidateBcc.Checked)
                {
                    foreach (PotentialBbie potentialBbie in candidateBcc.PotentialBbies)
                    {
                        if (potentialBbie.Checked)
                        {
                            foreach (PotentialBdt potentialBdt in potentialBbie.PotentialBdts)
                            {
                                if ((potentialBdt.Checked) && (potentialBdt.OriginalBdt == null))
                                {                                    
                                    BdtSpec bdtSpec = BdtSpec.CloneCdt(candidateBcc.OriginalBcc.Cdt, potentialBdt.Name);

                                    IBdt bdt = CreateBdtInBdtLibrary(bdtSpec);

                                    PropagateNewBdtInModel(bdt);
                                }
                            }
                        }
                    }
                }
            }
        }

        private IBdt CreateBdtInBdtLibrary(BdtSpec bdtSpec)
        {
            foreach (CandidateBdtLibrary candidateBdtLibrary in mCandidateBdtLibraries)
            {
                if (candidateBdtLibrary.Selected)
                {
                    return candidateBdtLibrary.OriginalBdtLibrary.CreateBdt(bdtSpec);
                }
            }

            return null;
        }

        private void PropagateNewBdtInModel(IBdt newBdt)
        {
            foreach (CandidateCcLibrary candidateCcLibrary in mCandidateCcLibraries)
            {
                if (candidateCcLibrary.Selected)
                {
                    foreach (CandidateAcc candidateAcc in candidateCcLibrary.CandidateAccs)
                    {
                        if (candidateAcc.Selected)
                        {
                            foreach (CandidateBcc candidateBcc in candidateAcc.CandidateBccs)
                            {
                                foreach (PotentialBbie potentialBbie in candidateBcc.PotentialBbies)
                                {
                                    foreach (PotentialBdt potentialBdt in potentialBbie.PotentialBdts)
                                    {
                                        if ((potentialBdt.Name == newBdt.Name) && (potentialBdt.OriginalBdt == null))
                                        {
                                            potentialBdt.OriginalBdt = newBdt;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }  
        }

        private void CreateAbie(CandidateAcc candidateAcc)
        {
            List<BbieSpec> bbieSpecs = CumulateBbieSpecs(candidateAcc);
            List<AsbieSpec> asbieSpecs = CumulateAsbieSpecs(candidateAcc);
            AbieSpec abieSpec = CumulateAbieSpec(candidateAcc);

            // The qualifier of the ABIE, if there is one, is already contained in the AbieName. 
            abieSpec.Name = AbieName;
            
            abieSpec.Bbies = bbieSpecs;
            abieSpec.Asbies = asbieSpecs;

            CreateAbieInBieLibrary(abieSpec);
        }

        private void CreateAbieInBieLibrary(AbieSpec abieSpec)
        {
            foreach (CandidateBieLibrary candidateBieLibrary in mCandidateBieLibraries)
            {
                if (candidateBieLibrary.Selected)
                {
                    foreach (IAbie abie in ccCache.GetBiesFromBieLibrary(candidateBieLibrary.OriginalBieLibrary.Name))
                    {
                        if (abie.Name.Equals(abieSpec.Name))
                        {
                            throw new TemporaryAbieModelException(
                                String.Format(
                                    "An ABIE named \"{0}\" already exists in the BIE Library currently selected. Choose a different name for the ABIE or select a different BIE Library which does not contain a ABIE with the same name either.",
                                    abieSpec.Name));
                        }
                    }

                    candidateBieLibrary.OriginalBieLibrary.CreateAbie(abieSpec);
                }
            }                        
        }

        private static List<AsbieSpec> CumulateAsbieSpecs(CandidateAcc candidateAcc)
        {
            List<AsbieSpec> asbieSpecs = new List<AsbieSpec>();

            foreach (CandidateAbie candidateAbie in candidateAcc.CandidateAbies)
            {
                if (candidateAbie.Checked)
                {
                    foreach (PotentialAsbie potentialAsbie in candidateAbie.PotentialAsbies)
                    {
                        if (potentialAsbie.Checked)
                        {
                            asbieSpecs.Add(AsbieSpec.CloneAscc(potentialAsbie.BasedOn, potentialAsbie.Name, candidateAbie.OriginalAbie));
                        }
                    }                    
                }
            }

            return asbieSpecs;
        }

        private static List<BbieSpec> CumulateBbieSpecs(CandidateAcc candidateAcc)
        {
            List<BbieSpec> bbieSpecs = new List<BbieSpec>();

            foreach (CandidateBcc candidateBcc in candidateAcc.CandidateBccs)
            {
                if (candidateBcc.Checked)
                {
                    foreach (PotentialBbie potentialBbie in candidateBcc.PotentialBbies)
                    {
                        if (potentialBbie.Checked)
                        {
                            IBdt bdtTypifingTheBbie = null;

                            foreach (PotentialBdt potentialBdt in potentialBbie.PotentialBdts)
                            {
                                if (potentialBdt.Checked)
                                {
                                    bdtTypifingTheBbie = potentialBdt.OriginalBdt;
                                    break;
                                }
                            }

                            BbieSpec bbieSpec = BbieSpec.CloneBcc(candidateBcc.OriginalBcc, bdtTypifingTheBbie);
                            bbieSpec.Name = potentialBbie.Name;
                            bbieSpecs.Add(bbieSpec);
                        }
                    }
                }
            }

            return bbieSpecs;
        }

        private static AbieSpec CumulateAbieSpec(CandidateAcc candidateAcc)
        {
            AbieSpec abieSpec = new AbieSpec
            {
                Definition = candidateAcc.OriginalAcc.Definition,
                LanguageCode = candidateAcc.OriginalAcc.LanguageCode,
                BusinessTerms = candidateAcc.OriginalAcc.BusinessTerms,
                UsageRules = candidateAcc.OriginalAcc.UsageRules,
                BasedOn = candidateAcc.OriginalAcc,
            };

            return abieSpec;
        }
    }
}