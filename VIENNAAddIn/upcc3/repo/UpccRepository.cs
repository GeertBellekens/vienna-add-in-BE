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
using CctsRepository;
using CctsRepository.BdtLibrary;
using CctsRepository.BieLibrary;
using CctsRepository.BLibrary;
using CctsRepository.CcLibrary;
using CctsRepository.CdtLibrary;
using CctsRepository.DocLibrary;
using CctsRepository.EnumLibrary;
using CctsRepository.PrimLibrary;
using VIENNAAddIn.upcc3.repo;
using VIENNAAddIn.upcc3.repo.BdtLibrary;
using VIENNAAddIn.upcc3.repo.BieLibrary;
using VIENNAAddIn.upcc3.repo.BLibrary;
using VIENNAAddIn.upcc3.repo.CcLibrary;
using VIENNAAddIn.upcc3.repo.CdtLibrary;
using VIENNAAddIn.upcc3.repo.DocLibrary;
using VIENNAAddIn.upcc3.repo.EnumLibrary;
using VIENNAAddIn.upcc3.repo.PrimLibrary;
// ReSharper restore RedundantUsingDirective
using VIENNAAddIn.upcc3.uml;
using VIENNAAddInUtils;

namespace VIENNAAddIn.upcc3.repo
{
    public class UpccRepository : ICctsRepository
    {
        public UpccRepository(IUmlRepository umlRepository)
        {
            UmlRepository = umlRepository;
        }
		
		public IUmlRepository UmlRepository { get; private set; }

        #region ICctsRepository Members

		#region Libraries
		
		public IEnumerable<object> GetAllLibraries()
        {
            foreach (IUmlPackage umlPackage in UmlRepository.GetPackagesByStereotype("BDTLibrary", 
			                                                                         "BIELibrary", 
			                                                                         "bLibrary", 
			                                                                         "CCLibrary", 
			                                                                         "CDTLibrary", 
			                                                                         "DOCLibrary",
																					 "e_DocLibrary",			                                                                         
			                                                                         "ENUMLibrary", 
			                                                                         "PRIMLibrary"))
            {
                switch (umlPackage.Stereotype)
                {
                    case "BDTLibrary":
                        yield return new UpccBdtLibrary(umlPackage);
                        break;
                    case "BIELibrary":
                        yield return new UpccBieLibrary(umlPackage);
                        break;
                    case "bLibrary":
                        yield return new UpccBLibrary(umlPackage);
                        break;
                    case "CCLibrary":
                        yield return new UpccCcLibrary(umlPackage);
                        break;
                    case "CDTLibrary":
                        yield return new UpccCdtLibrary(umlPackage);
                        break;
                    case "DOCLibrary":
                    case "e_DocLibrary":
                        yield return new UpccDocLibrary(umlPackage);
                        break;
                    case "ENUMLibrary":
                        yield return new UpccEnumLibrary(umlPackage);
                        break;
                    case "PRIMLibrary":
                        yield return new UpccPrimLibrary(umlPackage);
                        break;
                }
            }
        }

		/// <returns>
		/// All BDTLibraries contained in this repository.
		/// </returns>
        public IEnumerable<IBdtLibrary> GetBdtLibraries()
        {
            foreach (IUmlPackage umlPackage in UmlRepository.GetPackagesByStereotype("BDTLibrary"))
            {
                yield return new UpccBdtLibrary(umlPackage);
            }
        }

		/// <summary>
		/// Retrieves a BDTLibrary by ID.
		/// <param name="id">A BDTLibrary's ID.</param>
		/// <returns>The BDTLibrary with the given <paramref name="id"/> or <c>null</c> if no such BDTLibrary is found.</returns>
		/// </summary>
		public IBdtLibrary GetBdtLibraryById(int id)
		{
            var umlPackage = UmlRepository.GetPackageById(id);
            return umlPackage == null ? null : new UpccBdtLibrary(umlPackage);
		}

		/// <summary>
		/// Retrieves a BDTLibrary by <see cref="Path"/>.
		/// <param name="path">A BDTLibrary's <see cref="Path"/>.</param>
		/// <returns>The BDTLibrary with the given <paramref name="path"/> or <c>null</c> if no such BDTLibrary is found.</returns>
		/// </summary>
        public IBdtLibrary GetBdtLibraryByPath(Path path)
		{
            var umlPackage = UmlRepository.GetPackageByPath(path);
            return umlPackage == null ? null : new UpccBdtLibrary(umlPackage);
		}

		/// <returns>
		/// All BIELibraries contained in this repository.
		/// </returns>
        public IEnumerable<IBieLibrary> GetBieLibraries()
        {
            foreach (IUmlPackage umlPackage in UmlRepository.GetPackagesByStereotype("BIELibrary"))
            {
                yield return new UpccBieLibrary(umlPackage);
            }
        }

		/// <summary>
		/// Retrieves a BIELibrary by ID.
		/// <param name="id">A BIELibrary's ID.</param>
		/// <returns>The BIELibrary with the given <paramref name="id"/> or <c>null</c> if no such BIELibrary is found.</returns>
		/// </summary>
		public IBieLibrary GetBieLibraryById(int id)
		{
            var umlPackage = UmlRepository.GetPackageById(id);
            return umlPackage == null ? null : new UpccBieLibrary(umlPackage);
		}

		/// <summary>
		/// Retrieves a BIELibrary by <see cref="Path"/>.
		/// <param name="path">A BIELibrary's <see cref="Path"/>.</param>
		/// <returns>The BIELibrary with the given <paramref name="path"/> or <c>null</c> if no such BIELibrary is found.</returns>
		/// </summary>
        public IBieLibrary GetBieLibraryByPath(Path path)
		{
            var umlPackage = UmlRepository.GetPackageByPath(path);
            return umlPackage == null ? null : new UpccBieLibrary(umlPackage);
		}

		/// <returns>
		/// All bLibraries contained in this repository.
		/// </returns>
        public IEnumerable<IBLibrary> GetBLibraries()
        {
            foreach (IUmlPackage umlPackage in UmlRepository.GetPackagesByStereotype("bLibrary"))
            {
                yield return new UpccBLibrary(umlPackage);
            }
        }

		/// <summary>
		/// Retrieves a bLibrary by ID.
		/// <param name="id">A bLibrary's ID.</param>
		/// <returns>The bLibrary with the given <paramref name="id"/> or <c>null</c> if no such bLibrary is found.</returns>
		/// </summary>
		public IBLibrary GetBLibraryById(int id)
		{
            var umlPackage = UmlRepository.GetPackageById(id);
            return umlPackage == null ? null : new UpccBLibrary(umlPackage);
		}

		/// <summary>
		/// Retrieves a bLibrary by <see cref="Path"/>.
		/// <param name="path">A bLibrary's <see cref="Path"/>.</param>
		/// <returns>The bLibrary with the given <paramref name="path"/> or <c>null</c> if no such bLibrary is found.</returns>
		/// </summary>
        public IBLibrary GetBLibraryByPath(Path path)
		{
            var umlPackage = UmlRepository.GetPackageByPath(path);
            return umlPackage == null ? null : new UpccBLibrary(umlPackage);
		}

		/// <returns>
		/// All CCLibraries contained in this repository.
		/// </returns>
        public IEnumerable<ICcLibrary> GetCcLibraries()
        {
            foreach (IUmlPackage umlPackage in UmlRepository.GetPackagesByStereotype("CCLibrary"))
            {
                yield return new UpccCcLibrary(umlPackage);
            }
        }

		/// <summary>
		/// Retrieves a CCLibrary by ID.
		/// <param name="id">A CCLibrary's ID.</param>
		/// <returns>The CCLibrary with the given <paramref name="id"/> or <c>null</c> if no such CCLibrary is found.</returns>
		/// </summary>
		public ICcLibrary GetCcLibraryById(int id)
		{
            var umlPackage = UmlRepository.GetPackageById(id);
            return umlPackage == null ? null : new UpccCcLibrary(umlPackage);
		}

		/// <summary>
		/// Retrieves a CCLibrary by <see cref="Path"/>.
		/// <param name="path">A CCLibrary's <see cref="Path"/>.</param>
		/// <returns>The CCLibrary with the given <paramref name="path"/> or <c>null</c> if no such CCLibrary is found.</returns>
		/// </summary>
        public ICcLibrary GetCcLibraryByPath(Path path)
		{
            var umlPackage = UmlRepository.GetPackageByPath(path);
            return umlPackage == null ? null : new UpccCcLibrary(umlPackage);
		}

		/// <returns>
		/// All CDTLibraries contained in this repository.
		/// </returns>
        public IEnumerable<ICdtLibrary> GetCdtLibraries()
        {
            foreach (IUmlPackage umlPackage in UmlRepository.GetPackagesByStereotype("CDTLibrary"))
            {
                yield return new UpccCdtLibrary(umlPackage);
            }
        }

		/// <summary>
		/// Retrieves a CDTLibrary by ID.
		/// <param name="id">A CDTLibrary's ID.</param>
		/// <returns>The CDTLibrary with the given <paramref name="id"/> or <c>null</c> if no such CDTLibrary is found.</returns>
		/// </summary>
		public ICdtLibrary GetCdtLibraryById(int id)
		{
            var umlPackage = UmlRepository.GetPackageById(id);
            return umlPackage == null ? null : new UpccCdtLibrary(umlPackage);
		}

		/// <summary>
		/// Retrieves a CDTLibrary by <see cref="Path"/>.
		/// <param name="path">A CDTLibrary's <see cref="Path"/>.</param>
		/// <returns>The CDTLibrary with the given <paramref name="path"/> or <c>null</c> if no such CDTLibrary is found.</returns>
		/// </summary>
        public ICdtLibrary GetCdtLibraryByPath(Path path)
		{
            var umlPackage = UmlRepository.GetPackageByPath(path);
            return umlPackage == null ? null : new UpccCdtLibrary(umlPackage);
		}

		/// <returns>
		/// All DOCLibraries contained in this repository.
		/// </returns>
        public IEnumerable<IDocLibrary> GetDocLibraries()
        {
            foreach (IUmlPackage umlPackage in UmlRepository.GetPackagesByStereotype("DOCLibrary","e_DocLibrary"))
            {
                yield return new UpccDocLibrary(umlPackage);
            }
        }

		/// <summary>
		/// Retrieves a DOCLibrary by ID.
		/// <param name="id">A DOCLibrary's ID.</param>
		/// <returns>The DOCLibrary with the given <paramref name="id"/> or <c>null</c> if no such DOCLibrary is found.</returns>
		/// </summary>
		public IDocLibrary GetDocLibraryById(int id)
		{
            var umlPackage = UmlRepository.GetPackageById(id);
            return umlPackage == null ? null : new UpccDocLibrary(umlPackage);
		}

		/// <summary>
		/// Retrieves a DOCLibrary by <see cref="Path"/>.
		/// <param name="path">A DOCLibrary's <see cref="Path"/>.</param>
		/// <returns>The DOCLibrary with the given <paramref name="path"/> or <c>null</c> if no such DOCLibrary is found.</returns>
		/// </summary>
        public IDocLibrary GetDocLibraryByPath(Path path)
		{
            var umlPackage = UmlRepository.GetPackageByPath(path);
            return umlPackage == null ? null : new UpccDocLibrary(umlPackage);
		}

		/// <returns>
		/// All ENUMLibraries contained in this repository.
		/// </returns>
        public IEnumerable<IEnumLibrary> GetEnumLibraries()
        {
            foreach (IUmlPackage umlPackage in UmlRepository.GetPackagesByStereotype("ENUMLibrary"))
            {
                yield return new UpccEnumLibrary(umlPackage);
            }
        }

		/// <summary>
		/// Retrieves a ENUMLibrary by ID.
		/// <param name="id">A ENUMLibrary's ID.</param>
		/// <returns>The ENUMLibrary with the given <paramref name="id"/> or <c>null</c> if no such ENUMLibrary is found.</returns>
		/// </summary>
		public IEnumLibrary GetEnumLibraryById(int id)
		{
            var umlPackage = UmlRepository.GetPackageById(id);
            return umlPackage == null ? null : new UpccEnumLibrary(umlPackage);
		}

		/// <summary>
		/// Retrieves a ENUMLibrary by <see cref="Path"/>.
		/// <param name="path">A ENUMLibrary's <see cref="Path"/>.</param>
		/// <returns>The ENUMLibrary with the given <paramref name="path"/> or <c>null</c> if no such ENUMLibrary is found.</returns>
		/// </summary>
        public IEnumLibrary GetEnumLibraryByPath(Path path)
		{
            var umlPackage = UmlRepository.GetPackageByPath(path);
            return umlPackage == null ? null : new UpccEnumLibrary(umlPackage);
		}

		/// <returns>
		/// All PRIMLibraries contained in this repository.
		/// </returns>
        public IEnumerable<IPrimLibrary> GetPrimLibraries()
        {
            foreach (IUmlPackage umlPackage in UmlRepository.GetPackagesByStereotype("PRIMLibrary"))
            {
                yield return new UpccPrimLibrary(umlPackage);
            }
        }

		/// <summary>
		/// Retrieves a PRIMLibrary by ID.
		/// <param name="id">A PRIMLibrary's ID.</param>
		/// <returns>The PRIMLibrary with the given <paramref name="id"/> or <c>null</c> if no such PRIMLibrary is found.</returns>
		/// </summary>
		public IPrimLibrary GetPrimLibraryById(int id)
		{
            var umlPackage = UmlRepository.GetPackageById(id);
            return umlPackage == null ? null : new UpccPrimLibrary(umlPackage);
		}

		/// <summary>
		/// Retrieves a PRIMLibrary by <see cref="Path"/>.
		/// <param name="path">A PRIMLibrary's <see cref="Path"/>.</param>
		/// <returns>The PRIMLibrary with the given <paramref name="path"/> or <c>null</c> if no such PRIMLibrary is found.</returns>
		/// </summary>
        public IPrimLibrary GetPrimLibraryByPath(Path path)
		{
            var umlPackage = UmlRepository.GetPackageByPath(path);
            return umlPackage == null ? null : new UpccPrimLibrary(umlPackage);
		}

		#endregion
		
		#region Elements

		/// <summary>
		/// Retrieves a IDSCHEME by ID.
		/// <param name="id">A IDSCHEME's ID.</param>
		/// <returns>The IDSCHEME with the given <paramref name="id"/> or <c>null</c> if no such IDSCHEME is found.</returns>
		/// </summary>
        public IIdScheme GetIdSchemeById(int id)
		{
            var umlClassifier = UmlRepository.GetDataTypeById(id);
            return umlClassifier == null ? null : new UpccIdScheme(umlClassifier);
		}

		/// <summary>
		/// Retrieves a IDSCHEME by <see cref="Path"/>.
		/// <param name="path">A IDSCHEME's <see cref="Path"/>.</param>
		/// <returns>The IDSCHEME with the given <paramref name="path"/> or <c>null</c> if no such IDSCHEME is found.</returns>
		/// </summary>
		public IIdScheme GetIdSchemeByPath(Path path)
		{
            var umlClassifier = UmlRepository.GetDataTypeByPath(path);
            return umlClassifier == null ? null : new UpccIdScheme(umlClassifier);
		}

		/// <summary>
		/// Retrieves a PRIM by ID.
		/// <param name="id">A PRIM's ID.</param>
		/// <returns>The PRIM with the given <paramref name="id"/> or <c>null</c> if no such PRIM is found.</returns>
		/// </summary>
        public IPrim GetPrimById(int id)
		{
            var umlClassifier = UmlRepository.GetDataTypeById(id);
            return umlClassifier == null ? null : new UpccPrim(umlClassifier);
		}

		/// <summary>
		/// Retrieves a PRIM by <see cref="Path"/>.
		/// <param name="path">A PRIM's <see cref="Path"/>.</param>
		/// <returns>The PRIM with the given <paramref name="path"/> or <c>null</c> if no such PRIM is found.</returns>
		/// </summary>
		public IPrim GetPrimByPath(Path path)
		{
            var umlClassifier = UmlRepository.GetDataTypeByPath(path);
            return umlClassifier == null ? null : new UpccPrim(umlClassifier);
		}

		/// <summary>
		/// Retrieves a ENUM by ID.
		/// <param name="id">A ENUM's ID.</param>
		/// <returns>The ENUM with the given <paramref name="id"/> or <c>null</c> if no such ENUM is found.</returns>
		/// </summary>
        public IEnum GetEnumById(int id)
		{
            var umlClassifier = UmlRepository.GetEnumerationById(id);
            return umlClassifier == null ? null : new UpccEnum(umlClassifier);
		}

		/// <summary>
		/// Retrieves a ENUM by <see cref="Path"/>.
		/// <param name="path">A ENUM's <see cref="Path"/>.</param>
		/// <returns>The ENUM with the given <paramref name="path"/> or <c>null</c> if no such ENUM is found.</returns>
		/// </summary>
		public IEnum GetEnumByPath(Path path)
		{
            var umlClassifier = UmlRepository.GetEnumerationByPath(path);
            return umlClassifier == null ? null : new UpccEnum(umlClassifier);
		}

		/// <summary>
		/// Retrieves a ABIE by ID.
		/// <param name="id">A ABIE's ID.</param>
		/// <returns>The ABIE with the given <paramref name="id"/> or <c>null</c> if no such ABIE is found.</returns>
		/// </summary>
        public IAbie GetAbieById(int id)
		{
            var umlClassifier = UmlRepository.GetClassById(id);
            return umlClassifier == null ? null : new UpccAbie(umlClassifier);
		}

		/// <summary>
		/// Retrieves a ABIE by <see cref="Path"/>.
		/// <param name="path">A ABIE's <see cref="Path"/>.</param>
		/// <returns>The ABIE with the given <paramref name="path"/> or <c>null</c> if no such ABIE is found.</returns>
		/// </summary>
		public IAbie GetAbieByPath(Path path)
		{
            var umlClassifier = UmlRepository.GetClassByPath(path);
            return umlClassifier == null ? null : new UpccAbie(umlClassifier);
		}

		/// <summary>
		/// Retrieves a ACC by ID.
		/// <param name="id">A ACC's ID.</param>
		/// <returns>The ACC with the given <paramref name="id"/> or <c>null</c> if no such ACC is found.</returns>
		/// </summary>
        public IAcc GetAccById(int id)
		{
            var umlClassifier = UmlRepository.GetClassById(id);
            return umlClassifier == null ? null : new UpccAcc(umlClassifier);
		}

		/// <summary>
		/// Retrieves a ACC by <see cref="Path"/>.
		/// <param name="path">A ACC's <see cref="Path"/>.</param>
		/// <returns>The ACC with the given <paramref name="path"/> or <c>null</c> if no such ACC is found.</returns>
		/// </summary>
		public IAcc GetAccByPath(Path path)
		{
            var umlClassifier = UmlRepository.GetClassByPath(path);
            return umlClassifier == null ? null : new UpccAcc(umlClassifier);
		}

		/// <summary>
		/// Retrieves a BDT by ID.
		/// <param name="id">A BDT's ID.</param>
		/// <returns>The BDT with the given <paramref name="id"/> or <c>null</c> if no such BDT is found.</returns>
		/// </summary>
        public IBdt GetBdtById(int id)
		{
            var umlClassifier = UmlRepository.GetClassById(id);
            return umlClassifier == null ? null : new UpccBdt(umlClassifier);
		}

		/// <summary>
		/// Retrieves a BDT by <see cref="Path"/>.
		/// <param name="path">A BDT's <see cref="Path"/>.</param>
		/// <returns>The BDT with the given <paramref name="path"/> or <c>null</c> if no such BDT is found.</returns>
		/// </summary>
		public IBdt GetBdtByPath(Path path)
		{
            var umlClassifier = UmlRepository.GetClassByPath(path);
            return umlClassifier == null ? null : new UpccBdt(umlClassifier);
		}

		/// <summary>
		/// Retrieves a CDT by ID.
		/// <param name="id">A CDT's ID.</param>
		/// <returns>The CDT with the given <paramref name="id"/> or <c>null</c> if no such CDT is found.</returns>
		/// </summary>
        public ICdt GetCdtById(int id)
		{
            var umlClassifier = UmlRepository.GetClassById(id);
            return umlClassifier == null ? null : new UpccCdt(umlClassifier);
		}

		/// <summary>
		/// Retrieves a CDT by <see cref="Path"/>.
		/// <param name="path">A CDT's <see cref="Path"/>.</param>
		/// <returns>The CDT with the given <paramref name="path"/> or <c>null</c> if no such CDT is found.</returns>
		/// </summary>
		public ICdt GetCdtByPath(Path path)
		{
            var umlClassifier = UmlRepository.GetClassByPath(path);
            return umlClassifier == null ? null : new UpccCdt(umlClassifier);
		}

		/// <summary>
		/// Retrieves a MA by ID.
		/// <param name="id">A MA's ID.</param>
		/// <returns>The MA with the given <paramref name="id"/> or <c>null</c> if no such MA is found.</returns>
		/// </summary>
        public IMa GetMaById(int id)
		{
            var umlClassifier = UmlRepository.GetClassById(id);
            return umlClassifier == null ? null : new UpccMa(umlClassifier);
		}

		/// <summary>
		/// Retrieves a MA by <see cref="Path"/>.
		/// <param name="path">A MA's <see cref="Path"/>.</param>
		/// <returns>The MA with the given <paramref name="path"/> or <c>null</c> if no such MA is found.</returns>
		/// </summary>
		public IMa GetMaByPath(Path path)
		{
            var umlClassifier = UmlRepository.GetClassByPath(path);
            return umlClassifier == null ? null : new UpccMa(umlClassifier);
		}

		#endregion

        /// <summary>
        /// Root locations are places in the repository where root-level bLibraries can be created.
        /// </summary>
        /// <returns>The root locations currently available in this repository.</returns>
        public IEnumerable<Path> GetRootLocations()
        {
            return UmlRepository.GetRootLocations();
        }

        /// <summary>
        /// Creates a bLibrary in the given <paramref name="rootLocation"/>.
        /// </summary>
        /// <param name="rootLocation">A root location (<see cref="GetRootLocations"/>).</param>
        /// <param name="specification">A specification for a bLibrary.</param>
        /// <returns>The newly created bLibrary.</returns>
        /// <returns></returns>
        public IBLibrary CreateRootBLibrary(Path rootLocation, BLibrarySpec specification)
        {
            var umlPackage = UmlRepository.CreateRootPackage(rootLocation, BLibrarySpecConverter.Convert(specification));
            return umlPackage == null ? null : new UpccBLibrary(umlPackage);
        }

        #endregion
        public IEnumerable<IDocLibrary> GetDocLibraries(int packageID)
		{
        	var parentPackage = UmlRepository.GetPackageById(packageID);
        	List<IDocLibrary> foundDocLibraries = new List<IDocLibrary>();
        	if (parentPackage !=null)
        	{
	        	//return empty list if package not found
	        	var packages =  UmlRepository.GetSubPackagesByStereotype(parentPackage,true,Stereotype.DocLibraryStereotypes.ToArray());
	        	foreach (var package in packages) 
	        	{
	        		foundDocLibraries.Add(new UpccDocLibrary(package));
	        	}
        	}
			return foundDocLibraries;
		}

        public void getUserSelectedPackage()
        {
            this.UmlRepository.getUserSelectedPackage();
        }
    }
}