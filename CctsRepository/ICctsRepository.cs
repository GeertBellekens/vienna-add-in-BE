
// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System;
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
using VIENNAAddInUtils;

namespace CctsRepository
{
	public interface ICctsRepository
	{
		#region Libraries
		
		IEnumerable<Object> GetAllLibraries();

		/// <returns>
		/// All BDTLibraries contained in this repository.
		/// </returns>
		IEnumerable<IBdtLibrary> GetBdtLibraries();

		/// <summary>
		/// Retrieves a BDTLibrary by ID.
		/// <param name="id">A BDTLibrary's ID.</param>
		/// <returns>The BDTLibrary with the given <paramref name="id"/> or <c>null</c> if no such BDTLibrary is found.</returns>
		/// </summary>
		IBdtLibrary GetBdtLibraryById(int id);

		/// <summary>
		/// Retrieves a BDTLibrary by <see cref="Path"/>.
		/// <param name="path">A BDTLibrary's <see cref="Path"/>.</param>
		/// <returns>The BDTLibrary with the given <paramref name="path"/> or <c>null</c> if no such BDTLibrary is found.</returns>
		/// </summary>
		IBdtLibrary GetBdtLibraryByPath(Path path);

		/// <returns>
		/// All BIELibraries contained in this repository.
		/// </returns>
		IEnumerable<IBieLibrary> GetBieLibraries();

		/// <summary>
		/// Retrieves a BIELibrary by ID.
		/// <param name="id">A BIELibrary's ID.</param>
		/// <returns>The BIELibrary with the given <paramref name="id"/> or <c>null</c> if no such BIELibrary is found.</returns>
		/// </summary>
		IBieLibrary GetBieLibraryById(int id);

		/// <summary>
		/// Retrieves a BIELibrary by <see cref="Path"/>.
		/// <param name="path">A BIELibrary's <see cref="Path"/>.</param>
		/// <returns>The BIELibrary with the given <paramref name="path"/> or <c>null</c> if no such BIELibrary is found.</returns>
		/// </summary>
		IBieLibrary GetBieLibraryByPath(Path path);

		/// <returns>
		/// All bLibraries contained in this repository.
		/// </returns>
		IEnumerable<IBLibrary> GetBLibraries();

		/// <summary>
		/// Retrieves a bLibrary by ID.
		/// <param name="id">A bLibrary's ID.</param>
		/// <returns>The bLibrary with the given <paramref name="id"/> or <c>null</c> if no such bLibrary is found.</returns>
		/// </summary>
		IBLibrary GetBLibraryById(int id);

		/// <summary>
		/// Retrieves a bLibrary by <see cref="Path"/>.
		/// <param name="path">A bLibrary's <see cref="Path"/>.</param>
		/// <returns>The bLibrary with the given <paramref name="path"/> or <c>null</c> if no such bLibrary is found.</returns>
		/// </summary>
		IBLibrary GetBLibraryByPath(Path path);

		/// <returns>
		/// All CCLibraries contained in this repository.
		/// </returns>
		IEnumerable<ICcLibrary> GetCcLibraries();

		/// <summary>
		/// Retrieves a CCLibrary by ID.
		/// <param name="id">A CCLibrary's ID.</param>
		/// <returns>The CCLibrary with the given <paramref name="id"/> or <c>null</c> if no such CCLibrary is found.</returns>
		/// </summary>
		ICcLibrary GetCcLibraryById(int id);

		/// <summary>
		/// Retrieves a CCLibrary by <see cref="Path"/>.
		/// <param name="path">A CCLibrary's <see cref="Path"/>.</param>
		/// <returns>The CCLibrary with the given <paramref name="path"/> or <c>null</c> if no such CCLibrary is found.</returns>
		/// </summary>
		ICcLibrary GetCcLibraryByPath(Path path);

		/// <returns>
		/// All CDTLibraries contained in this repository.
		/// </returns>
		IEnumerable<ICdtLibrary> GetCdtLibraries();

		/// <summary>
		/// Retrieves a CDTLibrary by ID.
		/// <param name="id">A CDTLibrary's ID.</param>
		/// <returns>The CDTLibrary with the given <paramref name="id"/> or <c>null</c> if no such CDTLibrary is found.</returns>
		/// </summary>
		ICdtLibrary GetCdtLibraryById(int id);

		/// <summary>
		/// Retrieves a CDTLibrary by <see cref="Path"/>.
		/// <param name="path">A CDTLibrary's <see cref="Path"/>.</param>
		/// <returns>The CDTLibrary with the given <paramref name="path"/> or <c>null</c> if no such CDTLibrary is found.</returns>
		/// </summary>
		ICdtLibrary GetCdtLibraryByPath(Path path);

		/// <returns>
		/// All DOCLibraries contained in this repository.
		/// </returns>
		IEnumerable<IDocLibrary> GetDocLibraries();
		
		/// <summary>
		/// All DOCLibraries contained in the package with the given packageID
		/// </summary>
		/// <param name="packageID">the packageID to start looking from</param>
		/// <returns>all doclibraries in the package tree with the given ID</returns>
		IEnumerable<IDocLibrary> GetDocLibraries(int packageID);
        
		/// <summary>
		/// Retrieves a DOCLibrary by ID.
		/// <param name="id">A DOCLibrary's ID.</param>
		/// <returns>The DOCLibrary with the given <paramref name="id"/> or <c>null</c> if no such DOCLibrary is found.</returns>
		/// </summary>
		IDocLibrary GetDocLibraryById(int id);

		/// <summary>
		/// Retrieves a DOCLibrary by <see cref="Path"/>.
		/// <param name="path">A DOCLibrary's <see cref="Path"/>.</param>
		/// <returns>The DOCLibrary with the given <paramref name="path"/> or <c>null</c> if no such DOCLibrary is found.</returns>
		/// </summary>
		IDocLibrary GetDocLibraryByPath(Path path);

		/// <returns>
		/// All ENUMLibraries contained in this repository.
		/// </returns>
		IEnumerable<IEnumLibrary> GetEnumLibraries();

		/// <summary>
		/// Retrieves a ENUMLibrary by ID.
		/// <param name="id">A ENUMLibrary's ID.</param>
		/// <returns>The ENUMLibrary with the given <paramref name="id"/> or <c>null</c> if no such ENUMLibrary is found.</returns>
		/// </summary>
		IEnumLibrary GetEnumLibraryById(int id);

		/// <summary>
		/// Retrieves a ENUMLibrary by <see cref="Path"/>.
		/// <param name="path">A ENUMLibrary's <see cref="Path"/>.</param>
		/// <returns>The ENUMLibrary with the given <paramref name="path"/> or <c>null</c> if no such ENUMLibrary is found.</returns>
		/// </summary>
		IEnumLibrary GetEnumLibraryByPath(Path path);

		/// <returns>
		/// All PRIMLibraries contained in this repository.
		/// </returns>
		IEnumerable<IPrimLibrary> GetPrimLibraries();

		/// <summary>
		/// Retrieves a PRIMLibrary by ID.
		/// <param name="id">A PRIMLibrary's ID.</param>
		/// <returns>The PRIMLibrary with the given <paramref name="id"/> or <c>null</c> if no such PRIMLibrary is found.</returns>
		/// </summary>
		IPrimLibrary GetPrimLibraryById(int id);

		/// <summary>
		/// Retrieves a PRIMLibrary by <see cref="Path"/>.
		/// <param name="path">A PRIMLibrary's <see cref="Path"/>.</param>
		/// <returns>The PRIMLibrary with the given <paramref name="path"/> or <c>null</c> if no such PRIMLibrary is found.</returns>
		/// </summary>
		IPrimLibrary GetPrimLibraryByPath(Path path);

		#endregion
		#region Elements

		/// <summary>
		/// Retrieves a IDSCHEME by ID.
		/// <param name="id">A IDSCHEME's ID.</param>
		/// <returns>The IDSCHEME with the given <paramref name="id"/> or <c>null</c> if no such IDSCHEME is found.</returns>
		/// </summary>
		IIdScheme GetIdSchemeById(int id);

		/// <summary>
		/// Retrieves a IDSCHEME by <see cref="Path"/>.
		/// <param name="path">A IDSCHEME's <see cref="Path"/>.</param>
		/// <returns>The IDSCHEME with the given <paramref name="path"/> or <c>null</c> if no such IDSCHEME is found.</returns>
		/// </summary>
		IIdScheme GetIdSchemeByPath(Path path);

		/// <summary>
		/// Retrieves a PRIM by ID.
		/// <param name="id">A PRIM's ID.</param>
		/// <returns>The PRIM with the given <paramref name="id"/> or <c>null</c> if no such PRIM is found.</returns>
		/// </summary>
		IPrim GetPrimById(int id);

		/// <summary>
		/// Retrieves a PRIM by <see cref="Path"/>.
		/// <param name="path">A PRIM's <see cref="Path"/>.</param>
		/// <returns>The PRIM with the given <paramref name="path"/> or <c>null</c> if no such PRIM is found.</returns>
		/// </summary>
		IPrim GetPrimByPath(Path path);

		/// <summary>
		/// Retrieves a ENUM by ID.
		/// <param name="id">A ENUM's ID.</param>
		/// <returns>The ENUM with the given <paramref name="id"/> or <c>null</c> if no such ENUM is found.</returns>
		/// </summary>
		IEnum GetEnumById(int id);

		/// <summary>
		/// Retrieves a ENUM by <see cref="Path"/>.
		/// <param name="path">A ENUM's <see cref="Path"/>.</param>
		/// <returns>The ENUM with the given <paramref name="path"/> or <c>null</c> if no such ENUM is found.</returns>
		/// </summary>
		IEnum GetEnumByPath(Path path);

		/// <summary>
		/// Retrieves a ABIE by ID.
		/// <param name="id">A ABIE's ID.</param>
		/// <returns>The ABIE with the given <paramref name="id"/> or <c>null</c> if no such ABIE is found.</returns>
		/// </summary>
		IAbie GetAbieById(int id);

		/// <summary>
		/// Retrieves a ABIE by <see cref="Path"/>.
		/// <param name="path">A ABIE's <see cref="Path"/>.</param>
		/// <returns>The ABIE with the given <paramref name="path"/> or <c>null</c> if no such ABIE is found.</returns>
		/// </summary>
		IAbie GetAbieByPath(Path path);

		/// <summary>
		/// Retrieves a ACC by ID.
		/// <param name="id">A ACC's ID.</param>
		/// <returns>The ACC with the given <paramref name="id"/> or <c>null</c> if no such ACC is found.</returns>
		/// </summary>
		IAcc GetAccById(int id);

		/// <summary>
		/// Retrieves a ACC by <see cref="Path"/>.
		/// <param name="path">A ACC's <see cref="Path"/>.</param>
		/// <returns>The ACC with the given <paramref name="path"/> or <c>null</c> if no such ACC is found.</returns>
		/// </summary>
		IAcc GetAccByPath(Path path);

		/// <summary>
		/// Retrieves a BDT by ID.
		/// <param name="id">A BDT's ID.</param>
		/// <returns>The BDT with the given <paramref name="id"/> or <c>null</c> if no such BDT is found.</returns>
		/// </summary>
		IBdt GetBdtById(int id);

		/// <summary>
		/// Retrieves a BDT by <see cref="Path"/>.
		/// <param name="path">A BDT's <see cref="Path"/>.</param>
		/// <returns>The BDT with the given <paramref name="path"/> or <c>null</c> if no such BDT is found.</returns>
		/// </summary>
		IBdt GetBdtByPath(Path path);

		/// <summary>
		/// Retrieves a CDT by ID.
		/// <param name="id">A CDT's ID.</param>
		/// <returns>The CDT with the given <paramref name="id"/> or <c>null</c> if no such CDT is found.</returns>
		/// </summary>
		ICdt GetCdtById(int id);

		/// <summary>
		/// Retrieves a CDT by <see cref="Path"/>.
		/// <param name="path">A CDT's <see cref="Path"/>.</param>
		/// <returns>The CDT with the given <paramref name="path"/> or <c>null</c> if no such CDT is found.</returns>
		/// </summary>
		ICdt GetCdtByPath(Path path);

		/// <summary>
		/// Retrieves a MA by ID.
		/// <param name="id">A MA's ID.</param>
		/// <returns>The MA with the given <paramref name="id"/> or <c>null</c> if no such MA is found.</returns>
		/// </summary>
		IMa GetMaById(int id);

		/// <summary>
		/// Retrieves a MA by <see cref="Path"/>.
		/// <param name="path">A MA's <see cref="Path"/>.</param>
		/// <returns>The MA with the given <paramref name="path"/> or <c>null</c> if no such MA is found.</returns>
		/// </summary>
		IMa GetMaByPath(Path path);

		#endregion

		/// <summary>
		/// Root locations are places in the repository where root-level bLibraries can be created.
		/// </summary>
		/// <returns>The root locations currently available in this repository.</returns>
		IEnumerable<Path> GetRootLocations();

		/// <summary>
		/// Creates a bLibrary in the given <paramref name="rootLocation"/>.
		/// </summary>
		/// <param name="rootLocation">A root location (<see cref="GetRootLocations"/>).</param>
		/// <param name="specification">A specification for a bLibrary.</param>
		/// <returns>The newly created bLibrary.</returns>
		/// <returns></returns>
		IBLibrary CreateRootBLibrary(Path rootLocation, BLibrarySpec specification);
	}
}