
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

namespace CctsRepository.PrimLibrary
{
	public interface IPrim:ICctsElement
    {
	
        IPrimLibrary PrimLibrary { get; }

		IPrim IsEquivalentTo { get; }
		
		/// <summary>
		/// the XSD type name that can be used to represent this primitive type
		/// </summary>
		string xsdType {get;}

		#region Tagged Values

        ///<summary>
        /// Tagged value 'businessTerm'.
        ///</summary>
		IEnumerable<string> BusinessTerms { get; }

        ///<summary>
        /// Tagged value 'dictionaryEntryName'.
        ///</summary>
		string DictionaryEntryName { get; }

        ///<summary>
        /// Tagged value 'fractionDigits'.
        ///</summary>
		string FractionDigits { get; }

        ///<summary>
        /// Tagged value 'length'.
        ///</summary>
		string Length { get; }

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
        /// Tagged value 'pattern'.
        ///</summary>
		string Pattern { get; }

        ///<summary>
        /// Tagged value 'totalDigits'.
        ///</summary>
		string TotalDigits { get; }

        ///<summary>
        /// Tagged value 'whiteSpace'.
        ///</summary>
		string WhiteSpace { get; }

		#endregion
    }
}

