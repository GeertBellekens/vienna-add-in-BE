
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

namespace CctsRepository.BdtLibrary
{
	/// <summary>
	/// Interface for CCTS/UPCC BDTLibrary.
	/// </summary>
    public partial interface IBdtLibrary
    {
		/// <summary>
		/// The BDTLibrary's unique ID.
		/// </summary>
        int Id { get; }
		
		/// <summary>
		/// The BDTLibrary's name.
		/// </summary>
        string Name { get; }

		/// <summary>
		/// The bLibrary containing this BDTLibrary.
		/// </summary>
		IBLibrary BLibrary { get; }

		/// <summary>
		/// The BDTs contained in this BDTLibrary.
		/// </summary>
		IEnumerable<IBdt> Bdts { get; }

		/// <summary>
		/// Retrieves a BDT by name.
		/// <param name="name">A BDT's name.</param>
		/// <returns>The BDT with the given <paramref name="name"/> or <c>null</c> if no such BDT is found.</returns>
		/// </summary>
        IBdt GetBdtByName(string name);

		/// <summary>
		/// Creates a BDT based on the given <paramref name="specification"/>.
		/// <param name="specification">A specification for a BDT.</param>
		/// <returns>The newly created BDT.</returns>
		/// </summary>
		IBdt CreateBdt(BdtSpec specification);

		/// <summary>
		/// Updates a BDT to match the given <paramref name="specification"/>.
		/// <param name="bdt">A BDT.</param>
		/// <param name="specification">A new specification for the given BDT.</param>
		/// <returns>The updated BDT. Depending on the implementation, this might be the same updated instance or a new instance!</returns>
		/// </summary>
        IBdt UpdateBdt(IBdt bdt, BdtSpec specification);

		/// <summary>
		/// Removes a BDT from this BDTLibrary.
		/// <param name="bdt">A BDT.</param>
		/// </summary>
        void RemoveBdt(IBdt bdt);

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

