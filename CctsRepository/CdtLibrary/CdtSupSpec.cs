
// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************

using System.Collections.Generic;
// ReSharper disable RedundantUsingDirective
using CctsRepository.BdtLibrary;
using CctsRepository.BieLibrary;
using CctsRepository.BLibrary;
using CctsRepository.CcLibrary;
using CctsRepository.CdtLibrary;
using CctsRepository.DocLibrary;
using CctsRepository.EnumLibrary;
using CctsRepository.PrimLibrary;
// ReSharper restore RedundantUsingDirective

namespace CctsRepository.CdtLibrary
{
    public partial class CdtSupSpec
    {
		public string Name { get; set; }
		
        public string UpperBound { get; set; }
		
        public string LowerBound { get; set; }
		
		public BasicType BasicType { get; set; }

		#region Tagged Values

        ///<summary>
        /// Tagged value 'businessTerm'.
        ///</summary>
		public IEnumerable<string> BusinessTerms { get; set; }

        ///<summary>
        /// Tagged value 'definition'.
        ///</summary>
		public string Definition { get; set; }

        ///<summary>
        /// Tagged value 'dictionaryEntryName'.
        ///</summary>
		public string DictionaryEntryName { get; set; }

        ///<summary>
        /// Tagged value 'languageCode'.
        ///</summary>
		public string LanguageCode { get; set; }

        ///<summary>
        /// Tagged value 'modificationAllowedIndicator'.
        ///</summary>
		public string ModificationAllowedIndicator { get; set; }

        ///<summary>
        /// Tagged value 'uniqueIdentifier'.
        ///</summary>
		public string UniqueIdentifier { get; set; }

        ///<summary>
        /// Tagged value 'versionIdentifier'.
        ///</summary>
		public string VersionIdentifier { get; set; }

        ///<summary>
        /// Tagged value 'usageRule'.
        ///</summary>
		public IEnumerable<string> UsageRules { get; set; }

		#endregion

        public static CdtSupSpec CloneCdtSup(ICdtSup cdtSup)
        {
            return new CdtSupSpec
                   {
                   	   Name = cdtSup.Name,
                       UpperBound = cdtSup.UpperBound,
                       LowerBound = cdtSup.LowerBound,
                       BasicType = cdtSup.BasicType,
					   BusinessTerms = new List<string>(cdtSup.BusinessTerms),
					   Definition = cdtSup.Definition,
					   LanguageCode = cdtSup.LanguageCode,
					   ModificationAllowedIndicator = cdtSup.ModificationAllowedIndicator,
					   VersionIdentifier = cdtSup.VersionIdentifier,
					   UsageRules = new List<string>(cdtSup.UsageRules),
                   };
        }
    }
}

