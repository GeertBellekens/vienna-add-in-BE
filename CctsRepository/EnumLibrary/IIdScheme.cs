
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

namespace CctsRepository.EnumLibrary
{
    public interface IIdScheme
    {
		int Id { get; }
		
		string Name { get; }
		
        IEnumLibrary EnumLibrary { get; }

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
        /// Tagged value 'identifierSchemeAgencyIdentifier'.
        ///</summary>
		string IdentifierSchemeAgencyIdentifier { get; }

        ///<summary>
        /// Tagged value 'identifierSchemeAgencyName'.
        ///</summary>
		string IdentifierSchemeAgencyName { get; }

        ///<summary>
        /// Tagged value 'modificationAllowedIndicator'.
        ///</summary>
		string ModificationAllowedIndicator { get; }

        ///<summary>
        /// Tagged value 'pattern'.
        ///</summary>
		string Pattern { get; }

        ///<summary>
        /// Tagged value 'restrictedPrimitive'.
        ///</summary>
		string RestrictedPrimitive { get; }

        ///<summary>
        /// Tagged value 'uniqueIdentifier'.
        ///</summary>
		string UniqueIdentifier { get; }

        ///<summary>
        /// Tagged value 'versionIdentifier'.
        ///</summary>
		string VersionIdentifier { get; }

		#endregion
    }
}

