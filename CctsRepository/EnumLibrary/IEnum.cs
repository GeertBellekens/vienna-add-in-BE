
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
	public interface IEnum:ICctsElement
    {
		
        IEnumLibrary EnumLibrary { get; }

		IEnum IsEquivalentTo { get; }

		IEnumerable<ICodelistEntry> CodelistEntries { get; }
		
		bool IsAssembled{get;}

		/// <summary>
		/// Creates a(n) CodelistEntry based on the given <paramref name="specification"/>.
		/// <param name="specification">A specification for a(n) CodelistEntry.</param>
		/// <returns>The newly created CodelistEntry.</returns>
		/// </summary>
		ICodelistEntry CreateCodelistEntry(CodelistEntrySpec specification);

		/// <summary>
		/// Updates a(n) CodelistEntry to match the given <paramref name="specification"/>.
		/// <param name="codelistEntry">A(n) CodelistEntry.</param>
		/// <param name="specification">A new specification for the given CodelistEntry.</param>
		/// <returns>The updated CodelistEntry. Depending on the implementation, this might be the same updated instance or a new instance!</returns>
		/// </summary>
        ICodelistEntry UpdateCodelistEntry(ICodelistEntry codelistEntry, CodelistEntrySpec specification);

		/// <summary>
		/// Removes a(n) CodelistEntry from this ENUM.
		/// <param name="codelistEntry">A(n) CodelistEntry.</param>
		/// </summary>
        void RemoveCodelistEntry(ICodelistEntry codelistEntry);

		#region Tagged Values

        ///<summary>
        /// Tagged value 'codeListAgencyIdentifier'.
        ///</summary>
		string CodeListAgencyIdentifier { get; }

        ///<summary>
        /// Tagged value 'codeListAgencyName'.
        ///</summary>
		string CodeListAgencyName { get; }

        ///<summary>
        /// Tagged value 'codeListIdentifier'.
        ///</summary>
		string CodeListIdentifier { get; }

        ///<summary>
        /// Tagged value 'codeListName'.
        ///</summary>
		string CodeListName { get; }

        ///<summary>
        /// Tagged value 'dictionaryEntryName'.
        ///</summary>
		string DictionaryEntryName { get; }

        ///<summary>
        /// Tagged value 'enumerationURI'.
        ///</summary>
		string EnumerationURI { get; }

        ///<summary>
        /// Tagged value 'modificationAllowedIndicator'.
        ///</summary>
		string ModificationAllowedIndicator { get; }

        ///<summary>
        /// Tagged value 'restrictedPrimitive'.
        ///</summary>
		string RestrictedPrimitive { get; }

        ///<summary>
        /// Tagged value 'status'.
        ///</summary>
		string Status { get; }

		#endregion
    }
}

