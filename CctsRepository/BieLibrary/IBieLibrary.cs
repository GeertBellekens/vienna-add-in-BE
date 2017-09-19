
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
	/// <summary>
	/// Interface for CCTS/UPCC BIELibrary.
	/// </summary>
	public partial interface IBieLibrary:ICctsLibrary
    {

		/// <summary>
		/// The bLibrary containing this BIELibrary.
		/// </summary>
		IBLibrary BLibrary { get; }

		/// <summary>
		/// The ABIEs contained in this BIELibrary.
		/// </summary>
		IEnumerable<IAbie> Abies { get; }

		/// <summary>
		/// Retrieves a ABIE by name.
		/// <param name="name">A ABIE's name.</param>
		/// <returns>The ABIE with the given <paramref name="name"/> or <c>null</c> if no such ABIE is found.</returns>
		/// </summary>
        IAbie GetAbieByName(string name);

		/// <summary>
		/// Creates a ABIE based on the given <paramref name="specification"/>.
		/// <param name="specification">A specification for a ABIE.</param>
		/// <returns>The newly created ABIE.</returns>
		/// </summary>
		IAbie CreateAbie(AbieSpec specification);

		/// <summary>
		/// Updates a ABIE to match the given <paramref name="specification"/>.
		/// <param name="abie">A ABIE.</param>
		/// <param name="specification">A new specification for the given ABIE.</param>
		/// <returns>The updated ABIE. Depending on the implementation, this might be the same updated instance or a new instance!</returns>
		/// </summary>
        IAbie UpdateAbie(IAbie abie, AbieSpec specification);

		/// <summary>
		/// Removes a ABIE from this BIELibrary.
		/// <param name="abie">A ABIE.</param>
		/// </summary>
        void RemoveAbie(IAbie abie);

        
    }
}

