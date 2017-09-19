
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
    public interface IAcc:ICctsElement
    {
	
        ICcLibrary CcLibrary { get; }

		IAcc IsEquivalentTo { get; }

		IEnumerable<IBcc> Bccs { get; }

		/// <summary>
		/// Creates a(n) BCC based on the given <paramref name="specification"/>.
		/// <param name="specification">A specification for a(n) BCC.</param>
		/// <returns>The newly created BCC.</returns>
		/// </summary>
		IBcc CreateBcc(BccSpec specification);

		/// <summary>
		/// Updates a(n) BCC to match the given <paramref name="specification"/>.
		/// <param name="bcc">A(n) BCC.</param>
		/// <param name="specification">A new specification for the given BCC.</param>
		/// <returns>The updated BCC. Depending on the implementation, this might be the same updated instance or a new instance!</returns>
		/// </summary>
        IBcc UpdateBcc(IBcc bcc, BccSpec specification);

		/// <summary>
		/// Removes a(n) BCC from this ACC.
		/// <param name="bcc">A(n) BCC.</param>
		/// </summary>
        void RemoveBcc(IBcc bcc);

		IEnumerable<IAscc> Asccs { get; }

		/// <summary>
		/// Creates a(n) ASCC based on the given <paramref name="specification"/>.
		/// <param name="specification">A specification for a(n) ASCC.</param>
		/// <returns>The newly created ASCC.</returns>
		/// </summary>
		IAscc CreateAscc(AsccSpec specification);

		/// <summary>
		/// Updates a(n) ASCC to match the given <paramref name="specification"/>.
		/// <param name="ascc">A(n) ASCC.</param>
		/// <param name="specification">A new specification for the given ASCC.</param>
		/// <returns>The updated ASCC. Depending on the implementation, this might be the same updated instance or a new instance!</returns>
		/// </summary>
        IAscc UpdateAscc(IAscc ascc, AsccSpec specification);

		/// <summary>
		/// Removes a(n) ASCC from this ACC.
		/// <param name="ascc">A(n) ASCC.</param>
		/// </summary>
        void RemoveAscc(IAscc ascc);

    }
}

