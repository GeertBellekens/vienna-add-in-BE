
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

namespace CctsRepository.BLibrary
{
	/// <summary>
	/// Interface for CCTS/UPCC bLibrary.
	/// </summary>
    public partial interface IBLibrary
    {
		/// <summary>
		/// The bLibrary's unique ID.
		/// </summary>
        int Id { get; }
		
		/// <summary>
		/// The bLibrary's name.
		/// </summary>
        string Name { get; }

		/// <summary>
		/// The bLibrary containing this bLibrary.
		/// </summary>
		IBLibrary Parent { get; }

		/// <summary>
		/// The bLibrarys contained in this bLibrary.
		/// </summary>
		IEnumerable<IBLibrary> GetBLibraries();

		/// <summary>
		/// Retrieves a bLibrary by name.
		/// <param name="name">A bLibrary's name.</param>
		/// <returns>The bLibrary with the given <paramref name="name"/> or <c>null</c> if no such bLibrary is found.</returns>
		/// </summary>
        IBLibrary GetBLibraryByName(string name);

		/// <summary>
		/// Creates a bLibrary based on the given <paramref name="specification"/>.
		/// <param name="specification">A specification for a bLibrary.</param>
		/// <returns>The newly created bLibrary.</returns>
		/// </summary>
		IBLibrary CreateBLibrary(BLibrarySpec specification);

		/// <summary>
		/// Updates a bLibrary to match the given <paramref name="specification"/>.
		/// <param name="bLibrary">A bLibrary.</param>
		/// <param name="specification">A new specification for the given bLibrary.</param>
		/// <returns>The updated bLibrary. Depending on the implementation, this might be the same updated instance or a new instance!</returns>
		/// </summary>
        IBLibrary UpdateBLibrary(IBLibrary bLibrary, BLibrarySpec specification);

		/// <summary>
		/// Removes a bLibrary from this bLibrary.
		/// <param name="bLibrary">A bLibrary.</param>
		/// </summary>
        void RemoveBLibrary(IBLibrary bLibrary);

		/// <summary>
		/// The PRIMLibrarys contained in this bLibrary.
		/// </summary>
		IEnumerable<IPrimLibrary> GetPrimLibraries();

		/// <summary>
		/// Retrieves a PRIMLibrary by name.
		/// <param name="name">A PRIMLibrary's name.</param>
		/// <returns>The PRIMLibrary with the given <paramref name="name"/> or <c>null</c> if no such PRIMLibrary is found.</returns>
		/// </summary>
        IPrimLibrary GetPrimLibraryByName(string name);

		/// <summary>
		/// Creates a PRIMLibrary based on the given <paramref name="specification"/>.
		/// <param name="specification">A specification for a PRIMLibrary.</param>
		/// <returns>The newly created PRIMLibrary.</returns>
		/// </summary>
		IPrimLibrary CreatePrimLibrary(PrimLibrarySpec specification);

		/// <summary>
		/// Updates a PRIMLibrary to match the given <paramref name="specification"/>.
		/// <param name="primLibrary">A PRIMLibrary.</param>
		/// <param name="specification">A new specification for the given PRIMLibrary.</param>
		/// <returns>The updated PRIMLibrary. Depending on the implementation, this might be the same updated instance or a new instance!</returns>
		/// </summary>
        IPrimLibrary UpdatePrimLibrary(IPrimLibrary primLibrary, PrimLibrarySpec specification);

		/// <summary>
		/// Removes a PRIMLibrary from this bLibrary.
		/// <param name="primLibrary">A PRIMLibrary.</param>
		/// </summary>
        void RemovePrimLibrary(IPrimLibrary primLibrary);

		/// <summary>
		/// The ENUMLibrarys contained in this bLibrary.
		/// </summary>
		IEnumerable<IEnumLibrary> GetEnumLibraries();

		/// <summary>
		/// Retrieves a ENUMLibrary by name.
		/// <param name="name">A ENUMLibrary's name.</param>
		/// <returns>The ENUMLibrary with the given <paramref name="name"/> or <c>null</c> if no such ENUMLibrary is found.</returns>
		/// </summary>
        IEnumLibrary GetEnumLibraryByName(string name);

		/// <summary>
		/// Creates a ENUMLibrary based on the given <paramref name="specification"/>.
		/// <param name="specification">A specification for a ENUMLibrary.</param>
		/// <returns>The newly created ENUMLibrary.</returns>
		/// </summary>
		IEnumLibrary CreateEnumLibrary(EnumLibrarySpec specification);

		/// <summary>
		/// Updates a ENUMLibrary to match the given <paramref name="specification"/>.
		/// <param name="enumLibrary">A ENUMLibrary.</param>
		/// <param name="specification">A new specification for the given ENUMLibrary.</param>
		/// <returns>The updated ENUMLibrary. Depending on the implementation, this might be the same updated instance or a new instance!</returns>
		/// </summary>
        IEnumLibrary UpdateEnumLibrary(IEnumLibrary enumLibrary, EnumLibrarySpec specification);

		/// <summary>
		/// Removes a ENUMLibrary from this bLibrary.
		/// <param name="enumLibrary">A ENUMLibrary.</param>
		/// </summary>
        void RemoveEnumLibrary(IEnumLibrary enumLibrary);

		/// <summary>
		/// The CDTLibrarys contained in this bLibrary.
		/// </summary>
		IEnumerable<ICdtLibrary> GetCdtLibraries();

		/// <summary>
		/// Retrieves a CDTLibrary by name.
		/// <param name="name">A CDTLibrary's name.</param>
		/// <returns>The CDTLibrary with the given <paramref name="name"/> or <c>null</c> if no such CDTLibrary is found.</returns>
		/// </summary>
        ICdtLibrary GetCdtLibraryByName(string name);

		/// <summary>
		/// Creates a CDTLibrary based on the given <paramref name="specification"/>.
		/// <param name="specification">A specification for a CDTLibrary.</param>
		/// <returns>The newly created CDTLibrary.</returns>
		/// </summary>
		ICdtLibrary CreateCdtLibrary(CdtLibrarySpec specification);

		/// <summary>
		/// Updates a CDTLibrary to match the given <paramref name="specification"/>.
		/// <param name="cdtLibrary">A CDTLibrary.</param>
		/// <param name="specification">A new specification for the given CDTLibrary.</param>
		/// <returns>The updated CDTLibrary. Depending on the implementation, this might be the same updated instance or a new instance!</returns>
		/// </summary>
        ICdtLibrary UpdateCdtLibrary(ICdtLibrary cdtLibrary, CdtLibrarySpec specification);

		/// <summary>
		/// Removes a CDTLibrary from this bLibrary.
		/// <param name="cdtLibrary">A CDTLibrary.</param>
		/// </summary>
        void RemoveCdtLibrary(ICdtLibrary cdtLibrary);

		/// <summary>
		/// The CCLibrarys contained in this bLibrary.
		/// </summary>
		IEnumerable<ICcLibrary> GetCcLibraries();

		/// <summary>
		/// Retrieves a CCLibrary by name.
		/// <param name="name">A CCLibrary's name.</param>
		/// <returns>The CCLibrary with the given <paramref name="name"/> or <c>null</c> if no such CCLibrary is found.</returns>
		/// </summary>
        ICcLibrary GetCcLibraryByName(string name);

		/// <summary>
		/// Creates a CCLibrary based on the given <paramref name="specification"/>.
		/// <param name="specification">A specification for a CCLibrary.</param>
		/// <returns>The newly created CCLibrary.</returns>
		/// </summary>
		ICcLibrary CreateCcLibrary(CcLibrarySpec specification);

		/// <summary>
		/// Updates a CCLibrary to match the given <paramref name="specification"/>.
		/// <param name="ccLibrary">A CCLibrary.</param>
		/// <param name="specification">A new specification for the given CCLibrary.</param>
		/// <returns>The updated CCLibrary. Depending on the implementation, this might be the same updated instance or a new instance!</returns>
		/// </summary>
        ICcLibrary UpdateCcLibrary(ICcLibrary ccLibrary, CcLibrarySpec specification);

		/// <summary>
		/// Removes a CCLibrary from this bLibrary.
		/// <param name="ccLibrary">A CCLibrary.</param>
		/// </summary>
        void RemoveCcLibrary(ICcLibrary ccLibrary);

		/// <summary>
		/// The BDTLibrarys contained in this bLibrary.
		/// </summary>
		IEnumerable<IBdtLibrary> GetBdtLibraries();

		/// <summary>
		/// Retrieves a BDTLibrary by name.
		/// <param name="name">A BDTLibrary's name.</param>
		/// <returns>The BDTLibrary with the given <paramref name="name"/> or <c>null</c> if no such BDTLibrary is found.</returns>
		/// </summary>
        IBdtLibrary GetBdtLibraryByName(string name);

		/// <summary>
		/// Creates a BDTLibrary based on the given <paramref name="specification"/>.
		/// <param name="specification">A specification for a BDTLibrary.</param>
		/// <returns>The newly created BDTLibrary.</returns>
		/// </summary>
		IBdtLibrary CreateBdtLibrary(BdtLibrarySpec specification);

		/// <summary>
		/// Updates a BDTLibrary to match the given <paramref name="specification"/>.
		/// <param name="bdtLibrary">A BDTLibrary.</param>
		/// <param name="specification">A new specification for the given BDTLibrary.</param>
		/// <returns>The updated BDTLibrary. Depending on the implementation, this might be the same updated instance or a new instance!</returns>
		/// </summary>
        IBdtLibrary UpdateBdtLibrary(IBdtLibrary bdtLibrary, BdtLibrarySpec specification);

		/// <summary>
		/// Removes a BDTLibrary from this bLibrary.
		/// <param name="bdtLibrary">A BDTLibrary.</param>
		/// </summary>
        void RemoveBdtLibrary(IBdtLibrary bdtLibrary);

		/// <summary>
		/// The BIELibrarys contained in this bLibrary.
		/// </summary>
		IEnumerable<IBieLibrary> GetBieLibraries();

		/// <summary>
		/// Retrieves a BIELibrary by name.
		/// <param name="name">A BIELibrary's name.</param>
		/// <returns>The BIELibrary with the given <paramref name="name"/> or <c>null</c> if no such BIELibrary is found.</returns>
		/// </summary>
        IBieLibrary GetBieLibraryByName(string name);

		/// <summary>
		/// Creates a BIELibrary based on the given <paramref name="specification"/>.
		/// <param name="specification">A specification for a BIELibrary.</param>
		/// <returns>The newly created BIELibrary.</returns>
		/// </summary>
		IBieLibrary CreateBieLibrary(BieLibrarySpec specification);

		/// <summary>
		/// Updates a BIELibrary to match the given <paramref name="specification"/>.
		/// <param name="bieLibrary">A BIELibrary.</param>
		/// <param name="specification">A new specification for the given BIELibrary.</param>
		/// <returns>The updated BIELibrary. Depending on the implementation, this might be the same updated instance or a new instance!</returns>
		/// </summary>
        IBieLibrary UpdateBieLibrary(IBieLibrary bieLibrary, BieLibrarySpec specification);

		/// <summary>
		/// Removes a BIELibrary from this bLibrary.
		/// <param name="bieLibrary">A BIELibrary.</param>
		/// </summary>
        void RemoveBieLibrary(IBieLibrary bieLibrary);

		/// <summary>
		/// The DOCLibrarys contained in this bLibrary.
		/// </summary>
		IEnumerable<IDocLibrary> GetDocLibraries();

		/// <summary>
		/// Retrieves a DOCLibrary by name.
		/// <param name="name">A DOCLibrary's name.</param>
		/// <returns>The DOCLibrary with the given <paramref name="name"/> or <c>null</c> if no such DOCLibrary is found.</returns>
		/// </summary>
        IDocLibrary GetDocLibraryByName(string name);

		/// <summary>
		/// Creates a DOCLibrary based on the given <paramref name="specification"/>.
		/// <param name="specification">A specification for a DOCLibrary.</param>
		/// <returns>The newly created DOCLibrary.</returns>
		/// </summary>
		IDocLibrary CreateDocLibrary(DocLibrarySpec specification);

		/// <summary>
		/// Updates a DOCLibrary to match the given <paramref name="specification"/>.
		/// <param name="docLibrary">A DOCLibrary.</param>
		/// <param name="specification">A new specification for the given DOCLibrary.</param>
		/// <returns>The updated DOCLibrary. Depending on the implementation, this might be the same updated instance or a new instance!</returns>
		/// </summary>
        IDocLibrary UpdateDocLibrary(IDocLibrary docLibrary, DocLibrarySpec specification);

		/// <summary>
		/// Removes a DOCLibrary from this bLibrary.
		/// <param name="docLibrary">A DOCLibrary.</param>
		/// </summary>
        void RemoveDocLibrary(IDocLibrary docLibrary);

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
    }
}

