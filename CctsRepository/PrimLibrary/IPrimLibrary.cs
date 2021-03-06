
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
	/// <summary>
	/// Interface for CCTS/UPCC PRIMLibrary.
	/// </summary>
	public partial interface IPrimLibrary:ICctsLibrary
    {
		/// <summary>
		/// The bLibrary containing this PRIMLibrary.
		/// </summary>
		IBLibrary BLibrary { get; }

		/// <summary>
		/// The PRIMs contained in this PRIMLibrary.
		/// </summary>
		IEnumerable<IPrim> Prims { get; }

		/// <summary>
		/// Retrieves a PRIM by name.
		/// <param name="name">A PRIM's name.</param>
		/// <returns>The PRIM with the given <paramref name="name"/> or <c>null</c> if no such PRIM is found.</returns>
		/// </summary>
        IPrim GetPrimByName(string name);

		/// <summary>
		/// Creates a PRIM based on the given <paramref name="specification"/>.
		/// <param name="specification">A specification for a PRIM.</param>
		/// <returns>The newly created PRIM.</returns>
		/// </summary>
		IPrim CreatePrim(PrimSpec specification);

		/// <summary>
		/// Updates a PRIM to match the given <paramref name="specification"/>.
		/// <param name="prim">A PRIM.</param>
		/// <param name="specification">A new specification for the given PRIM.</param>
		/// <returns>The updated PRIM. Depending on the implementation, this might be the same updated instance or a new instance!</returns>
		/// </summary>
        IPrim UpdatePrim(IPrim prim, PrimSpec specification);

		/// <summary>
		/// Removes a PRIM from this PRIMLibrary.
		/// <param name="prim">A PRIM.</param>
		/// </summary>
        void RemovePrim(IPrim prim);

    }
}

