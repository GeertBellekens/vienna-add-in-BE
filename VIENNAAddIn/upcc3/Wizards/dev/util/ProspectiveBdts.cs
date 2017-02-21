// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.Wizards.dev.util
{
    class ProspectiveBdts
    {
        private static ProspectiveBdts ClassInstance;
        private Dictionary<int, List<string>> mProspectiveBdts;

        private ProspectiveBdts()
        {
            mProspectiveBdts = new Dictionary<int, List<string>>();
        }

        public static ProspectiveBdts GetInstance()
        {
            if (ClassInstance == null)
            {
                ClassInstance = new ProspectiveBdts();
            }

            return ClassInstance;
        }

        public List<string> Bdts(int cdtIdForProspectiveBdts)
        {
            if (mProspectiveBdts.ContainsKey(cdtIdForProspectiveBdts))
            {
                return mProspectiveBdts[cdtIdForProspectiveBdts];
            }

            return new List<string>();
        }

        public void AddBdt(int cdtIdThatNewBDTIsAddedFor, string newBdtName)
        {
            if (!(mProspectiveBdts.ContainsKey(cdtIdThatNewBDTIsAddedFor)))
            {
                mProspectiveBdts.Add(cdtIdThatNewBDTIsAddedFor, new List<string>());
            }
            
            mProspectiveBdts[cdtIdThatNewBDTIsAddedFor].Add(newBdtName);
        }

        public void UpdateBdtName(string originalBdtName, string updatedBdtName)
        {
            Dictionary<int, List<string>> updatedDictionary = new Dictionary<int, List<string>>();

            foreach (KeyValuePair<int, List<string>> keyValuePair in mProspectiveBdts)
            {
                List<string> updatedBdtNames = new List<string>();

                foreach (string bdtName in keyValuePair.Value)
                {
                    if (bdtName.Equals(originalBdtName))
                    {
                        updatedBdtNames.Add(updatedBdtName);
                    }
                    else
                    {
                        updatedBdtNames.Add(bdtName);
                    }
                }

                updatedDictionary.Add(keyValuePair.Key, updatedBdtNames);
            }

            mProspectiveBdts = updatedDictionary;
        }

        public void Clear()
        {
            mProspectiveBdts = new Dictionary<int, List<string>>();
        }
    }
}
