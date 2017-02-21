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
using CctsRepository.BdtLibrary;
using CctsRepository.CdtLibrary;
using VIENNAAddIn.upcc3.Wizards.dev.cache;
using VIENNAAddIn.upcc3.Wizards.dev.util;

namespace VIENNAAddIn.upcc3.Wizards.dev.temporarymodel.abiemodel
{
    public class PotentialBbie
    {
        private string mName;
        private bool mChecked;
        private bool mSelected;
        private ICdt mCdtUsedInBcc;
        private List<PotentialBdt> mPotentialBdts;
        private bool mItemReadOnly;
        private Cursor mItemCursor;
        private bool mItemFocusable;

        public PotentialBbie(string bbieName, ICdt cdtOfTheBccWhichTheBbieIsBasedOn)
        {
            mName = bbieName;
            mCdtUsedInBcc = cdtOfTheBccWhichTheBbieIsBasedOn;
            mChecked = false;
            mSelected = false;
            mPotentialBdts = null;
            mItemReadOnly = false;
            mItemCursor = Cursors.IBeam;
            mItemFocusable = true;
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

        public List<PotentialBdt> PotentialBdts
        {
            get
            {
                // In case the list of potential BDTs fur the currently selected BBIE is empty it needs
                // to be populated with the appropriate list of BDTs. The source of the potential BDTs 
                // is twofold. First (Source 1), BDTs are retrieved from the BDT libraries of the CC 
                // Repository. Second (Source 2), BDTs are retrieved from the list of prospective BDTs, 
                // which are those BDTs which have been added by the user through the user interface.
                if (mPotentialBdts == null)
                {
                    // Retrieve BDTs from Source 1 (BDTs from the BDT Libraries)
                    CcCache ccCache = CcCache.GetInstance();

                    mPotentialBdts = new List<PotentialBdt>();

                    foreach (IBdtLibrary bdtLibrary in ccCache.GetBdtLibraries())
                    {
                        foreach (IBdt bdt in ccCache.GetBdtsFromBdtLibrary(bdtLibrary.Name))
                        {
                            if (bdt.BasedOn.Id == mCdtUsedInBcc.Id)
                            {
                                mPotentialBdts.Add(new PotentialBdt(bdt));
                            }
                        }
                    }

                    // Retrieve BDTs from Source 2 (prospective BDTs added by the user through the user interface)
                    foreach (string prospectiveBdtName in ProspectiveBdts.GetInstance().Bdts(mCdtUsedInBcc.Id))
                    {
                        AddPotentialBdt(prospectiveBdtName);
                    }                    

                    // However, in case no BDTs are available, neither from the BDT libraries nor from
                    // the list of prospective BDTs, then a prospective BDT is generated per default. 
                    if (mPotentialBdts.Count == 0)
                    {
                        string newBdtName = AddPotentialBdt();                        
                        ProspectiveBdts.GetInstance().AddBdt(mCdtUsedInBcc.Id, newBdtName);
                    }
                }

                return mPotentialBdts;
            }

            set { mPotentialBdts = value; }
        }

        public string AddPotentialBdt()
        {
            // The names for the BDTs generated should follow the following pattern. If no BDT is
            // available, then the default name for the BDT generated is the same as the name of
            // the CDT that the BDT is based on (Option 1). If such a BDT already exists, then the 
            // name of the new BDT should be the name of the CDT it is based on prefixed with the 
            // text "Qualifier", a successing number, as well as an underscore to separate the 
            // prefix from the name stemming from the CDT (Option 2). 
            //
            // Example for a list of BDTs based on the CDT "Text" 
            //    BDT 1: "Text"                   (i.e. Option 1)
            //    BDT 2: "Qualifier1_Text"        (i.e. Option 2)
            //    BDT 3: "Qualifier2_Text"        (i.e. Option 2)
            //    ...

            string newBdtName = mCdtUsedInBcc.Name;

            // Generating a new BDT Name according to Option 1
            if (!BdtWithTheSameNameExists(newBdtName))
            {
                AddPotentialBdt(newBdtName);
            }
            else
            {
                // Generating a new BDT Name according to Option 2
                for (int i = 1; i != -1; i++)
                {
                    newBdtName = "Qualifier" + i + "_" + mCdtUsedInBcc.Name;

                    if (!BdtWithTheSameNameExists(newBdtName))
                    {
                        AddPotentialBdt(newBdtName);

                        break;
                    }
                }

            }
            
            return newBdtName;
        }

        private bool BdtWithTheSameNameExists(string newBdtName)
        {
            bool foundBdtWithTheSameName = false;
            
            foreach (PotentialBdt potentialBdt in PotentialBdts)
            {
                if (potentialBdt.Name.Equals(newBdtName))
                {
                    foundBdtWithTheSameName = true;
                }
            }

            return foundBdtWithTheSameName;
        }


        public void AddPotentialBdt(string newBdtName)
        {
            bool bdtAlreadyAddedByPotentialBdtsSetter = false;

            foreach (PotentialBdt potentialBdt in PotentialBdts)
            {
                if (potentialBdt.Name.Equals(newBdtName))
                {
                    bdtAlreadyAddedByPotentialBdtsSetter = true;
                }
            }
            
            if (!bdtAlreadyAddedByPotentialBdtsSetter)
            {
                PotentialBdts.Add(new PotentialBdt(newBdtName));   
            }            
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