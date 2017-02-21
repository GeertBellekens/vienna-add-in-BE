
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

namespace CctsRepository.DocLibrary
{
    public interface IMa
    {
		int Id { get; }
		
		string Name { get; }
		
        IDocLibrary DocLibrary { get; }

		IEnumerable<IAsma> Asmas { get; }

		/// <summary>
		/// Creates a(n) ASMA based on the given <paramref name="specification"/>.
		/// <param name="specification">A specification for a(n) ASMA.</param>
		/// <returns>The newly created ASMA.</returns>
		/// </summary>
		IAsma CreateAsma(AsmaSpec specification);

		/// <summary>
		/// Updates a(n) ASMA to match the given <paramref name="specification"/>.
		/// <param name="asma">A(n) ASMA.</param>
		/// <param name="specification">A new specification for the given ASMA.</param>
		/// <returns>The updated ASMA. Depending on the implementation, this might be the same updated instance or a new instance!</returns>
		/// </summary>
        IAsma UpdateAsma(IAsma asma, AsmaSpec specification);

		/// <summary>
		/// Removes a(n) ASMA from this MA.
		/// <param name="asma">A(n) ASMA.</param>
		/// </summary>
        void RemoveAsma(IAsma asma);
    }
}

