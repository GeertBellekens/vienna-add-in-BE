
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

namespace CctsRepository.BieLibrary
{
    public interface IAbie
    {
		int Id { get; }
		
		string Name { get; }
		
        IBieLibrary BieLibrary { get; }

		IAbie IsEquivalentTo { get; }

		IAcc BasedOn { get; }

		IEnumerable<IBbie> Bbies { get; }

		/// <summary>
		/// Creates a(n) BBIE based on the given <paramref name="specification"/>.
		/// <param name="specification">A specification for a(n) BBIE.</param>
		/// <returns>The newly created BBIE.</returns>
		/// </summary>
		IBbie CreateBbie(BbieSpec specification);

		/// <summary>
		/// Updates a(n) BBIE to match the given <paramref name="specification"/>.
		/// <param name="bbie">A(n) BBIE.</param>
		/// <param name="specification">A new specification for the given BBIE.</param>
		/// <returns>The updated BBIE. Depending on the implementation, this might be the same updated instance or a new instance!</returns>
		/// </summary>
        IBbie UpdateBbie(IBbie bbie, BbieSpec specification);

		/// <summary>
		/// Removes a(n) BBIE from this ABIE.
		/// <param name="bbie">A(n) BBIE.</param>
		/// </summary>
        void RemoveBbie(IBbie bbie);

		IEnumerable<IAsbie> Asbies { get; }

		/// <summary>
		/// Creates a(n) ASBIE based on the given <paramref name="specification"/>.
		/// <param name="specification">A specification for a(n) ASBIE.</param>
		/// <returns>The newly created ASBIE.</returns>
		/// </summary>
		IAsbie CreateAsbie(AsbieSpec specification);

		/// <summary>
		/// Updates a(n) ASBIE to match the given <paramref name="specification"/>.
		/// <param name="asbie">A(n) ASBIE.</param>
		/// <param name="specification">A new specification for the given ASBIE.</param>
		/// <returns>The updated ASBIE. Depending on the implementation, this might be the same updated instance or a new instance!</returns>
		/// </summary>
        IAsbie UpdateAsbie(IAsbie asbie, AsbieSpec specification);

		/// <summary>
		/// Removes a(n) ASBIE from this ABIE.
		/// <param name="asbie">A(n) ASBIE.</param>
		/// </summary>
        void RemoveAsbie(IAsbie asbie);

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
        /// Tagged value 'languageCode'.
        ///</summary>
		string LanguageCode { get; }

        ///<summary>
        /// Tagged value 'uniqueIdentifier'.
        ///</summary>
		string UniqueIdentifier { get; }

        ///<summary>
        /// Tagged value 'versionIdentifier'.
        ///</summary>
		string VersionIdentifier { get; }

        ///<summary>
        /// Tagged value 'usageRule'.
        ///</summary>
		IEnumerable<string> UsageRules { get; }

		#endregion
    }
}

