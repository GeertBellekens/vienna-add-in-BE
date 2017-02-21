
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

namespace CctsRepository.PrimLibrary
{
    public partial class PrimSpec
    {
		public string Name { get; set; }

		public IPrim IsEquivalentTo { get; set; }

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
        /// Tagged value 'fractionDigits'.
        ///</summary>
		public string FractionDigits { get; set; }

        ///<summary>
        /// Tagged value 'languageCode'.
        ///</summary>
		public string LanguageCode { get; set; }

        ///<summary>
        /// Tagged value 'length'.
        ///</summary>
		public string Length { get; set; }

        ///<summary>
        /// Tagged value 'maximumExclusive'.
        ///</summary>
		public string MaximumExclusive { get; set; }

        ///<summary>
        /// Tagged value 'maximumInclusive'.
        ///</summary>
		public string MaximumInclusive { get; set; }

        ///<summary>
        /// Tagged value 'maximumLength'.
        ///</summary>
		public string MaximumLength { get; set; }

        ///<summary>
        /// Tagged value 'minimumExclusive'.
        ///</summary>
		public string MinimumExclusive { get; set; }

        ///<summary>
        /// Tagged value 'minimumInclusive'.
        ///</summary>
		public string MinimumInclusive { get; set; }

        ///<summary>
        /// Tagged value 'minimumLength'.
        ///</summary>
		public string MinimumLength { get; set; }

        ///<summary>
        /// Tagged value 'pattern'.
        ///</summary>
		public string Pattern { get; set; }

        ///<summary>
        /// Tagged value 'totalDigits'.
        ///</summary>
		public string TotalDigits { get; set; }

        ///<summary>
        /// Tagged value 'uniqueIdentifier'.
        ///</summary>
		public string UniqueIdentifier { get; set; }

        ///<summary>
        /// Tagged value 'versionIdentifier'.
        ///</summary>
		public string VersionIdentifier { get; set; }

        ///<summary>
        /// Tagged value 'whiteSpace'.
        ///</summary>
		public string WhiteSpace { get; set; }

		#endregion

        public static PrimSpec ClonePrim(IPrim prim)
        {
            return new PrimSpec
                   {
                   	   Name = prim.Name,
					   IsEquivalentTo = prim.IsEquivalentTo,
					   BusinessTerms = new List<string>(prim.BusinessTerms),
					   Definition = prim.Definition,
					   FractionDigits = prim.FractionDigits,
					   LanguageCode = prim.LanguageCode,
					   Length = prim.Length,
					   MaximumExclusive = prim.MaximumExclusive,
					   MaximumInclusive = prim.MaximumInclusive,
					   MaximumLength = prim.MaximumLength,
					   MinimumExclusive = prim.MinimumExclusive,
					   MinimumInclusive = prim.MinimumInclusive,
					   MinimumLength = prim.MinimumLength,
					   Pattern = prim.Pattern,
					   TotalDigits = prim.TotalDigits,
					   VersionIdentifier = prim.VersionIdentifier,
					   WhiteSpace = prim.WhiteSpace,
                   };
        }
	}
}

