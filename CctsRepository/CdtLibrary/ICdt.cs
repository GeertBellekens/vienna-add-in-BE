
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
    public interface ICdt:ICctsElement
    {
		
        ICdtLibrary CdtLibrary { get; }

		ICdt IsEquivalentTo { get; }

		ICdtCon Con { get; }

		IEnumerable<ICdtSup> Sups { get; }

		/// <summary>
		/// Creates a(n) SUP based on the given <paramref name="specification"/>.
		/// <param name="specification">A specification for a(n) SUP.</param>
		/// <returns>The newly created SUP.</returns>
		/// </summary>
		ICdtSup CreateCdtSup(CdtSupSpec specification);

		/// <summary>
		/// Updates a(n) SUP to match the given <paramref name="specification"/>.
		/// <param name="cdtSup">A(n) SUP.</param>
		/// <param name="specification">A new specification for the given SUP.</param>
		/// <returns>The updated SUP. Depending on the implementation, this might be the same updated instance or a new instance!</returns>
		/// </summary>
        ICdtSup UpdateCdtSup(ICdtSup cdtSup, CdtSupSpec specification);

		/// <summary>
		/// Removes a(n) SUP from this CDT.
		/// <param name="cdtSup">A(n) SUP.</param>
		/// </summary>
        void RemoveCdtSup(ICdtSup cdtSup);

    }
}

