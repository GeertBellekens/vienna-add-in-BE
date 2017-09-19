
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
	/// <summary>
	/// Interface for CCTS/UPCC CDTLibrary.
	/// </summary>
    public partial interface ICdtLibrary:ICctsLibrary
    {

		/// <summary>
		/// The bLibrary containing this CDTLibrary.
		/// </summary>
		IBLibrary BLibrary { get; }

		/// <summary>
		/// The CDTs contained in this CDTLibrary.
		/// </summary>
		IEnumerable<ICdt> Cdts { get; }

		/// <summary>
		/// Retrieves a CDT by name.
		/// <param name="name">A CDT's name.</param>
		/// <returns>The CDT with the given <paramref name="name"/> or <c>null</c> if no such CDT is found.</returns>
		/// </summary>
        ICdt GetCdtByName(string name);

		/// <summary>
		/// Creates a CDT based on the given <paramref name="specification"/>.
		/// <param name="specification">A specification for a CDT.</param>
		/// <returns>The newly created CDT.</returns>
		/// </summary>
		ICdt CreateCdt(CdtSpec specification);

		/// <summary>
		/// Updates a CDT to match the given <paramref name="specification"/>.
		/// <param name="cdt">A CDT.</param>
		/// <param name="specification">A new specification for the given CDT.</param>
		/// <returns>The updated CDT. Depending on the implementation, this might be the same updated instance or a new instance!</returns>
		/// </summary>
        ICdt UpdateCdt(ICdt cdt, CdtSpec specification);

		/// <summary>
		/// Removes a CDT from this CDTLibrary.
		/// <param name="cdt">A CDT.</param>
		/// </summary>
        void RemoveCdt(ICdt cdt);

    }
}

