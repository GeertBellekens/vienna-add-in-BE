
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

namespace CctsRepository.CcLibrary
{
    public partial class AccSpec
    {
		public string Name { get; set; }

		public IAcc IsEquivalentTo { get; set; }

		public List<BccSpec> Bccs { get; set; }

		public List<AsccSpec> Asccs { get; set; }

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

        public static AccSpec CloneAcc(IAcc acc)
        {
            return new AccSpec
                   {
                   	   Name = acc.Name,
					   IsEquivalentTo = acc.IsEquivalentTo,
					   Bccs = new List<BccSpec>(acc.Bccs.Convert(o => BccSpec.CloneBcc(o))),
					   Asccs = new List<AsccSpec>(acc.Asccs.Convert(o => AsccSpec.CloneAscc(o))),
					   BusinessTerms = new List<string>(acc.BusinessTerms),
					   Definition = acc.Definition,
					   LanguageCode = acc.LanguageCode,
					   VersionIdentifier = acc.VersionIdentifier,
					   UsageRules = new List<string>(acc.UsageRules),
                   };
        }
	}
}

