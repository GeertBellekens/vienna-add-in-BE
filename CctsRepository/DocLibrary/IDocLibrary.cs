
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
	/// <summary>
	/// Interface for CCTS/UPCC DOCLibrary.
	/// </summary>
    public partial interface IDocLibrary
    {
		/// <summary>
		/// The DOCLibrary's unique ID.
		/// </summary>
        int Id { get; }
		
		/// <summary>
		/// The DOCLibrary's name.
		/// </summary>
        string Name { get; }

		/// <summary>
		/// The bLibrary containing this DOCLibrary.
		/// </summary>
		IBLibrary BLibrary { get; }

		/// <summary>
		/// The MAs contained in this DOCLibrary.
		/// </summary>
		IEnumerable<IMa> Mas { get; }

		/// <summary>
		/// Retrieves a MA by name.
		/// <param name="name">A MA's name.</param>
		/// <returns>The MA with the given <paramref name="name"/> or <c>null</c> if no such MA is found.</returns>
		/// </summary>
        IMa GetMaByName(string name);

		/// <summary>
		/// Creates a MA based on the given <paramref name="specification"/>.
		/// <param name="specification">A specification for a MA.</param>
		/// <returns>The newly created MA.</returns>
		/// </summary>
		IMa CreateMa(MaSpec specification);

		/// <summary>
		/// Updates a MA to match the given <paramref name="specification"/>.
		/// <param name="ma">A MA.</param>
		/// <param name="specification">A new specification for the given MA.</param>
		/// <returns>The updated MA. Depending on the implementation, this might be the same updated instance or a new instance!</returns>
		/// </summary>
        IMa UpdateMa(IMa ma, MaSpec specification);

		/// <summary>
		/// Removes a MA from this DOCLibrary.
		/// <param name="ma">A MA.</param>
		/// </summary>
        void RemoveMa(IMa ma);

        ///<summary>
        /// Tagged value 'businessTerm'.
        ///</summary>
		IEnumerable<string> BusinessTerms { get; }

        ///<summary>
        /// Tagged value 'copyright'.
        ///</summary>
		IEnumerable<string> Copyrights { get; }

        ///<summary>
        /// Tagged value 'owner'.
        ///</summary>
		IEnumerable<string> Owners { get; }

        ///<summary>
        /// Tagged value 'reference'.
        ///</summary>
		IEnumerable<string> References { get; }

        ///<summary>
        /// Tagged value 'status'.
        ///</summary>
		string Status { get; }

        ///<summary>
        /// Tagged value 'uniqueIdentifier'.
        ///</summary>
		string UniqueIdentifier { get; }

        ///<summary>
        /// Tagged value 'versionIdentifier'.
        ///</summary>
		string VersionIdentifier { get; }

        ///<summary>
        /// Tagged value 'baseURN'.
        ///</summary>
		string BaseURN { get; }

        ///<summary>
        /// Tagged value 'namespacePrefix'.
        ///</summary>
		string NamespacePrefix { get; }
    }
}

