
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

namespace CctsRepository.BdtLibrary
{
    public interface IBdtSup
    {
		int Id { get; }
		
		string Name { get; }
		
        string UpperBound { get; }
		
        string LowerBound { get; }
		
        bool IsOptional();
		
        IBdt Bdt { get; }
		
		BasicType BasicType { get; }

		#region Tagged Values

        ///<summary>
        /// Tagged value 'businessTerm'.
        ///</summary>
		IEnumerable<string> BusinessTerms { get; }

        ///<summary>
        /// Tagged value 'definition'.
        ///</summary>
		string Definition { get; }

        ///<summary>
        /// Tagged value 'dictionaryEntryName'.
        ///</summary>
		string DictionaryEntryName { get; }

        ///<summary>
        /// Tagged value 'enumeration'.
        ///</summary>
		string Enumeration { get; }

        ///<summary>
        /// Tagged value 'fractionDigits'.
        ///</summary>
		string FractionDigits { get; }

        ///<summary>
        /// Tagged value 'languageCode'.
        ///</summary>
		string LanguageCode { get; }

        ///<summary>
        /// Tagged value 'maximumExclusive'.
        ///</summary>
		string MaximumExclusive { get; }

        ///<summary>
        /// Tagged value 'maximumInclusive'.
        ///</summary>
		string MaximumInclusive { get; }

        ///<summary>
        /// Tagged value 'maximumLength'.
        ///</summary>
		string MaximumLength { get; }

        ///<summary>
        /// Tagged value 'minimumExclusive'.
        ///</summary>
		string MinimumExclusive { get; }

        ///<summary>
        /// Tagged value 'minimumInclusive'.
        ///</summary>
		string MinimumInclusive { get; }

        ///<summary>
        /// Tagged value 'minimumLength'.
        ///</summary>
		string MinimumLength { get; }

        ///<summary>
        /// Tagged value 'modificationAllowedIndicator'.
        ///</summary>
		string ModificationAllowedIndicator { get; }

        ///<summary>
        /// Tagged value 'pattern'.
        ///</summary>
		string Pattern { get; }

        ///<summary>
        /// Tagged value 'totalDigits'.
        ///</summary>
		string TotalDigits { get; }

        ///<summary>
        /// Tagged value 'uniqueIdentifier'.
        ///</summary>
		string UniqueIdentifier { get; }

        ///<summary>
        /// Tagged value 'usageRule'.
        ///</summary>
		IEnumerable<string> UsageRules { get; }

        ///<summary>
        /// Tagged value 'versionIdentifier'.
        ///</summary>
		string VersionIdentifier { get; }

		#endregion
    }
}

