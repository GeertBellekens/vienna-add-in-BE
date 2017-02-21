// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************

namespace VIENNAAddIn.upcc3.Wizards.dev.binding
{
    public static class BindingPropertyNames
    {
        public enum TemporaryAbieModel
        {
            AbieName,
            AbiePrefix,
            CandidateCcLibraryNames,
            CandidateAccNames,
            CandidateBieLibraryNames,
            CandidateBdtLibraryNames,
            CandidateBccItems,
            PotentialBbieItems,
            PotentialBdtItems,
            CandidateAbieItems,
            PotentialAsbieItems
        }   

        public enum TemporaryBdtModel
        {
            Name,
            Prefix,
            CandidateBdtLibraryNames,
            CandidateCdtLibraryNames,
            CandidateCdtNames,
            CandidateConItems,
            CandidateSupItems,
            CandidateBdtLibraries,
            CandidateCdtLibraries
        }

        public enum TemporarySubSettingModel
        {
            CandidateDocLibraryNames,
            CandidateDocLibraryItems,
            CandidateRootElementNames,
            CandidateRootElementItems,
            PotentialBbieItems,
            PotentialAsbieItems,
            CandidateAbieItems,
            RootElement
        }   
    }    
}
