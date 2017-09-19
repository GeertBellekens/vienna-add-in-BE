
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

namespace CctsRepository.CcLibrary
{
	/// <summary>
	/// Interface for CCTS/UPCC CCLibrary.
	/// </summary>
	public partial interface ICcLibrary:ICctsLibrary
    {

		/// <summary>
		/// The bLibrary containing this CCLibrary.
		/// </summary>
		IBLibrary BLibrary { get; }

		/// <summary>
		/// The ACCs contained in this CCLibrary.
		/// </summary>
		IEnumerable<IAcc> Accs { get; }

		/// <summary>
		/// Retrieves a ACC by name.
		/// <param name="name">A ACC's name.</param>
		/// <returns>The ACC with the given <paramref name="name"/> or <c>null</c> if no such ACC is found.</returns>
		/// </summary>
        IAcc GetAccByName(string name);

		/// <summary>
		/// Creates a ACC based on the given <paramref name="specification"/>.
		/// <param name="specification">A specification for a ACC.</param>
		/// <returns>The newly created ACC.</returns>
		/// </summary>
		IAcc CreateAcc(AccSpec specification);

		/// <summary>
		/// Updates a ACC to match the given <paramref name="specification"/>.
		/// <param name="acc">A ACC.</param>
		/// <param name="specification">A new specification for the given ACC.</param>
		/// <returns>The updated ACC. Depending on the implementation, this might be the same updated instance or a new instance!</returns>
		/// </summary>
        IAcc UpdateAcc(IAcc acc, AccSpec specification);

		/// <summary>
		/// Removes a ACC from this CCLibrary.
		/// <param name="acc">A ACC.</param>
		/// </summary>
        void RemoveAcc(IAcc acc);

    }
}

