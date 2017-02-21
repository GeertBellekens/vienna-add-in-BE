
// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************

using System.Collections.Generic;
using VIENNAAddInUtils;
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
    public partial class CdtSpec
    {
		public string Name { get; set; }

		public ICdt IsEquivalentTo { get; set; }

		public CdtConSpec Con { get; set; }

		public List<CdtSupSpec> Sups { get; set; }

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

        public static CdtSpec CloneCdt(ICdt cdt)
        {
            return new CdtSpec
                   {
                   	   Name = cdt.Name,
					   IsEquivalentTo = cdt.IsEquivalentTo,
					   Con = CdtConSpec.CloneCdtCon(cdt.Con),
					   Sups = new List<CdtSupSpec>(cdt.Sups.Convert(o => CdtSupSpec.CloneCdtSup(o))),
					   BusinessTerms = new List<string>(cdt.BusinessTerms),
					   Definition = cdt.Definition,
					   LanguageCode = cdt.LanguageCode,
					   VersionIdentifier = cdt.VersionIdentifier,
					   UsageRules = new List<string>(cdt.UsageRules),
                   };
        }
	}
}

