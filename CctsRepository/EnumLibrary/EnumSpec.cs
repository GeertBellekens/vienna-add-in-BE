
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

namespace CctsRepository.EnumLibrary
{
    public partial class EnumSpec
    {
		public string Name { get; set; }

		public IEnum IsEquivalentTo { get; set; }

		public List<CodelistEntrySpec> CodelistEntries { get; set; }

		#region Tagged Values

        ///<summary>
        /// Tagged value 'businessTerm'.
        ///</summary>
		public IEnumerable<string> BusinessTerms { get; set; }

        ///<summary>
        /// Tagged value 'codeListAgencyIdentifier'.
        ///</summary>
		public string CodeListAgencyIdentifier { get; set; }

        ///<summary>
        /// Tagged value 'codeListAgencyName'.
        ///</summary>
		public string CodeListAgencyName { get; set; }

        ///<summary>
        /// Tagged value 'codeListIdentifier'.
        ///</summary>
		public string CodeListIdentifier { get; set; }

        ///<summary>
        /// Tagged value 'codeListName'.
        ///</summary>
		public string CodeListName { get; set; }

        ///<summary>
        /// Tagged value 'dictionaryEntryName'.
        ///</summary>
		public string DictionaryEntryName { get; set; }

        ///<summary>
        /// Tagged value 'definition'.
        ///</summary>
		public string Definition { get; set; }

        ///<summary>
        /// Tagged value 'enumerationURI'.
        ///</summary>
		public string EnumerationURI { get; set; }

        ///<summary>
        /// Tagged value 'languageCode'.
        ///</summary>
		public string LanguageCode { get; set; }

        ///<summary>
        /// Tagged value 'modificationAllowedIndicator'.
        ///</summary>
		public string ModificationAllowedIndicator { get; set; }

        ///<summary>
        /// Tagged value 'restrictedPrimitive'.
        ///</summary>
		public string RestrictedPrimitive { get; set; }

        ///<summary>
        /// Tagged value 'status'.
        ///</summary>
		public string Status { get; set; }

        ///<summary>
        /// Tagged value 'uniqueIdentifier'.
        ///</summary>
		public string UniqueIdentifier { get; set; }

        ///<summary>
        /// Tagged value 'versionIdentifier'.
        ///</summary>
		public string VersionIdentifier { get; set; }

		#endregion

        public static EnumSpec CloneEnum(IEnum @enum)
        {
            return new EnumSpec
                   {
                   	   Name = @enum.Name,
					   IsEquivalentTo = @enum.IsEquivalentTo,
					   CodelistEntries = new List<CodelistEntrySpec>(@enum.CodelistEntries.Convert(o => CodelistEntrySpec.CloneCodelistEntry(o))),
					   BusinessTerms = new List<string>(@enum.BusinessTerms),
					   CodeListAgencyIdentifier = @enum.CodeListAgencyIdentifier,
					   CodeListAgencyName = @enum.CodeListAgencyName,
					   CodeListIdentifier = @enum.CodeListIdentifier,
					   CodeListName = @enum.CodeListName,
					   Definition = @enum.Definition,
					   EnumerationURI = @enum.EnumerationURI,
					   LanguageCode = @enum.LanguageCode,
					   ModificationAllowedIndicator = @enum.ModificationAllowedIndicator,
					   RestrictedPrimitive = @enum.RestrictedPrimitive,
					   Status = @enum.Status,
					   VersionIdentifier = @enum.VersionIdentifier,
                   };
        }
	}
}

