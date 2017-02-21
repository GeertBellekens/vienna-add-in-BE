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
using System;
using System.Collections.Generic;
using VIENNAAddIn.upcc3.uml;

namespace VIENNAAddIn.upcc3.repo.BLibrary
{
    internal class UpccBLibrary : IBLibrary
    {
        public UpccBLibrary(IUmlPackage umlPackage)
        {
            UmlPackage = umlPackage;
        }

        public IUmlPackage UmlPackage { get; private set; }

        #region IBLibrary Members

		/// <summary>
		/// The bLibrary's unique ID.
		/// </summary>
        public int Id
        {
            get { return UmlPackage.Id; }
        }

		/// <summary>
		/// The bLibrary's name.
		/// </summary>
        public string Name
        {
            get { return UmlPackage.Name; }
        }

		/// <summary>
		/// The bLibrary containing this bLibrary.
		/// </summary>
		public IBLibrary Parent
        {
            get { return new UpccBLibrary(UmlPackage.Parent); }
        }

		/// <summary>
		/// The bLibrarys contained in this bLibrary.
		/// </summary>
		public IEnumerable<IBLibrary> GetBLibraries()
		{
            foreach (var package in UmlPackage.GetPackagesByStereotype("bLibrary"))
            {
                yield return new UpccBLibrary(package);
            }
		}

		/// <summary>
		/// Retrieves a bLibrary by name.
		/// <param name="name">A bLibrary's name.</param>
		/// <returns>The bLibrary with the given <paramref name="name"/> or <c>null</c> if no such bLibrary is found.</returns>
		/// </summary>
        public IBLibrary GetBLibraryByName(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				return null;
			}
            foreach (var package in UmlPackage.GetPackagesByStereotype("bLibrary"))
            {
				if (name == package.Name)
				{
                    return new UpccBLibrary(package);
				}
            }
			return null;
		}

		/// <summary>
		/// Creates a bLibrary based on the given <paramref name="specification"/>.
		/// <param name="specification">A specification for a bLibrary.</param>
		/// <returns>The newly created bLibrary.</returns>
		/// </summary>
		public IBLibrary CreateBLibrary(BLibrarySpec specification)
		{
		    return new UpccBLibrary(UmlPackage.CreatePackage(BLibrarySpecConverter.Convert(specification)));
		}

		/// <summary>
		/// Updates a bLibrary to match the given <paramref name="specification"/>.
		/// <param name="bLibrary">A bLibrary.</param>
		/// <param name="specification">A new specification for the given bLibrary.</param>
		/// <returns>The updated bLibrary. Depending on the implementation, this might be the same updated instance or a new instance!</returns>
		/// </summary>
        public IBLibrary UpdateBLibrary(IBLibrary bLibrary, BLibrarySpec specification)
		{
		    return new UpccBLibrary(UmlPackage.UpdatePackage(((UpccBLibrary) bLibrary).UmlPackage, BLibrarySpecConverter.Convert(specification)));
		}

		/// <summary>
		/// Removes a BLibrary from this bLibrary.
		/// <param name="bLibrary">A BLibrary.</param>
		/// </summary>
        public void RemoveBLibrary(IBLibrary bLibrary)
		{
            UmlPackage.RemovePackage(((UpccBLibrary) bLibrary).UmlPackage);
		}

		/// <summary>
		/// The PRIMLibrarys contained in this bLibrary.
		/// </summary>
		public IEnumerable<IPrimLibrary> GetPrimLibraries()
		{
            foreach (var package in UmlPackage.GetPackagesByStereotype("PRIMLibrary"))
            {
                yield return new UpccPrimLibrary(package);
            }
		}

		/// <summary>
		/// Retrieves a PRIMLibrary by name.
		/// <param name="name">A PRIMLibrary's name.</param>
		/// <returns>The PRIMLibrary with the given <paramref name="name"/> or <c>null</c> if no such PRIMLibrary is found.</returns>
		/// </summary>
        public IPrimLibrary GetPrimLibraryByName(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				return null;
			}
            foreach (var package in UmlPackage.GetPackagesByStereotype("PRIMLibrary"))
            {
				if (name == package.Name)
				{
                    return new UpccPrimLibrary(package);
				}
            }
			return null;
		}

		/// <summary>
		/// Creates a PRIMLibrary based on the given <paramref name="specification"/>.
		/// <param name="specification">A specification for a PRIMLibrary.</param>
		/// <returns>The newly created PRIMLibrary.</returns>
		/// </summary>
		public IPrimLibrary CreatePrimLibrary(PrimLibrarySpec specification)
		{
		    return new UpccPrimLibrary(UmlPackage.CreatePackage(PrimLibrarySpecConverter.Convert(specification)));
		}

		/// <summary>
		/// Updates a PRIMLibrary to match the given <paramref name="specification"/>.
		/// <param name="primLibrary">A PRIMLibrary.</param>
		/// <param name="specification">A new specification for the given PRIMLibrary.</param>
		/// <returns>The updated PRIMLibrary. Depending on the implementation, this might be the same updated instance or a new instance!</returns>
		/// </summary>
        public IPrimLibrary UpdatePrimLibrary(IPrimLibrary primLibrary, PrimLibrarySpec specification)
		{
		    return new UpccPrimLibrary(UmlPackage.UpdatePackage(((UpccPrimLibrary) primLibrary).UmlPackage, PrimLibrarySpecConverter.Convert(specification)));
		}

		/// <summary>
		/// Removes a PrimLibrary from this bLibrary.
		/// <param name="primLibrary">A PrimLibrary.</param>
		/// </summary>
        public void RemovePrimLibrary(IPrimLibrary primLibrary)
		{
            UmlPackage.RemovePackage(((UpccPrimLibrary) primLibrary).UmlPackage);
		}

		/// <summary>
		/// The ENUMLibrarys contained in this bLibrary.
		/// </summary>
		public IEnumerable<IEnumLibrary> GetEnumLibraries()
		{
            foreach (var package in UmlPackage.GetPackagesByStereotype("ENUMLibrary"))
            {
                yield return new UpccEnumLibrary(package);
            }
		}

		/// <summary>
		/// Retrieves a ENUMLibrary by name.
		/// <param name="name">A ENUMLibrary's name.</param>
		/// <returns>The ENUMLibrary with the given <paramref name="name"/> or <c>null</c> if no such ENUMLibrary is found.</returns>
		/// </summary>
        public IEnumLibrary GetEnumLibraryByName(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				return null;
			}
            foreach (var package in UmlPackage.GetPackagesByStereotype("ENUMLibrary"))
            {
				if (name == package.Name)
				{
                    return new UpccEnumLibrary(package);
				}
            }
			return null;
		}

		/// <summary>
		/// Creates a ENUMLibrary based on the given <paramref name="specification"/>.
		/// <param name="specification">A specification for a ENUMLibrary.</param>
		/// <returns>The newly created ENUMLibrary.</returns>
		/// </summary>
		public IEnumLibrary CreateEnumLibrary(EnumLibrarySpec specification)
		{
		    return new UpccEnumLibrary(UmlPackage.CreatePackage(EnumLibrarySpecConverter.Convert(specification)));
		}

		/// <summary>
		/// Updates a ENUMLibrary to match the given <paramref name="specification"/>.
		/// <param name="enumLibrary">A ENUMLibrary.</param>
		/// <param name="specification">A new specification for the given ENUMLibrary.</param>
		/// <returns>The updated ENUMLibrary. Depending on the implementation, this might be the same updated instance or a new instance!</returns>
		/// </summary>
        public IEnumLibrary UpdateEnumLibrary(IEnumLibrary enumLibrary, EnumLibrarySpec specification)
		{
		    return new UpccEnumLibrary(UmlPackage.UpdatePackage(((UpccEnumLibrary) enumLibrary).UmlPackage, EnumLibrarySpecConverter.Convert(specification)));
		}

		/// <summary>
		/// Removes a EnumLibrary from this bLibrary.
		/// <param name="enumLibrary">A EnumLibrary.</param>
		/// </summary>
        public void RemoveEnumLibrary(IEnumLibrary enumLibrary)
		{
            UmlPackage.RemovePackage(((UpccEnumLibrary) enumLibrary).UmlPackage);
		}

		/// <summary>
		/// The CDTLibrarys contained in this bLibrary.
		/// </summary>
		public IEnumerable<ICdtLibrary> GetCdtLibraries()
		{
            foreach (var package in UmlPackage.GetPackagesByStereotype("CDTLibrary"))
            {
                yield return new UpccCdtLibrary(package);
            }
		}

		/// <summary>
		/// Retrieves a CDTLibrary by name.
		/// <param name="name">A CDTLibrary's name.</param>
		/// <returns>The CDTLibrary with the given <paramref name="name"/> or <c>null</c> if no such CDTLibrary is found.</returns>
		/// </summary>
        public ICdtLibrary GetCdtLibraryByName(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				return null;
			}
            foreach (var package in UmlPackage.GetPackagesByStereotype("CDTLibrary"))
            {
				if (name == package.Name)
				{
                    return new UpccCdtLibrary(package);
				}
            }
			return null;
		}

		/// <summary>
		/// Creates a CDTLibrary based on the given <paramref name="specification"/>.
		/// <param name="specification">A specification for a CDTLibrary.</param>
		/// <returns>The newly created CDTLibrary.</returns>
		/// </summary>
		public ICdtLibrary CreateCdtLibrary(CdtLibrarySpec specification)
		{
		    return new UpccCdtLibrary(UmlPackage.CreatePackage(CdtLibrarySpecConverter.Convert(specification)));
		}

		/// <summary>
		/// Updates a CDTLibrary to match the given <paramref name="specification"/>.
		/// <param name="cdtLibrary">A CDTLibrary.</param>
		/// <param name="specification">A new specification for the given CDTLibrary.</param>
		/// <returns>The updated CDTLibrary. Depending on the implementation, this might be the same updated instance or a new instance!</returns>
		/// </summary>
        public ICdtLibrary UpdateCdtLibrary(ICdtLibrary cdtLibrary, CdtLibrarySpec specification)
		{
		    return new UpccCdtLibrary(UmlPackage.UpdatePackage(((UpccCdtLibrary) cdtLibrary).UmlPackage, CdtLibrarySpecConverter.Convert(specification)));
		}

		/// <summary>
		/// Removes a CdtLibrary from this bLibrary.
		/// <param name="cdtLibrary">A CdtLibrary.</param>
		/// </summary>
        public void RemoveCdtLibrary(ICdtLibrary cdtLibrary)
		{
            UmlPackage.RemovePackage(((UpccCdtLibrary) cdtLibrary).UmlPackage);
		}

		/// <summary>
		/// The CCLibrarys contained in this bLibrary.
		/// </summary>
		public IEnumerable<ICcLibrary> GetCcLibraries()
		{
            foreach (var package in UmlPackage.GetPackagesByStereotype("CCLibrary"))
            {
                yield return new UpccCcLibrary(package);
            }
		}

		/// <summary>
		/// Retrieves a CCLibrary by name.
		/// <param name="name">A CCLibrary's name.</param>
		/// <returns>The CCLibrary with the given <paramref name="name"/> or <c>null</c> if no such CCLibrary is found.</returns>
		/// </summary>
        public ICcLibrary GetCcLibraryByName(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				return null;
			}
            foreach (var package in UmlPackage.GetPackagesByStereotype("CCLibrary"))
            {
				if (name == package.Name)
				{
                    return new UpccCcLibrary(package);
				}
            }
			return null;
		}

		/// <summary>
		/// Creates a CCLibrary based on the given <paramref name="specification"/>.
		/// <param name="specification">A specification for a CCLibrary.</param>
		/// <returns>The newly created CCLibrary.</returns>
		/// </summary>
		public ICcLibrary CreateCcLibrary(CcLibrarySpec specification)
		{
		    return new UpccCcLibrary(UmlPackage.CreatePackage(CcLibrarySpecConverter.Convert(specification)));
		}

		/// <summary>
		/// Updates a CCLibrary to match the given <paramref name="specification"/>.
		/// <param name="ccLibrary">A CCLibrary.</param>
		/// <param name="specification">A new specification for the given CCLibrary.</param>
		/// <returns>The updated CCLibrary. Depending on the implementation, this might be the same updated instance or a new instance!</returns>
		/// </summary>
        public ICcLibrary UpdateCcLibrary(ICcLibrary ccLibrary, CcLibrarySpec specification)
		{
		    return new UpccCcLibrary(UmlPackage.UpdatePackage(((UpccCcLibrary) ccLibrary).UmlPackage, CcLibrarySpecConverter.Convert(specification)));
		}

		/// <summary>
		/// Removes a CcLibrary from this bLibrary.
		/// <param name="ccLibrary">A CcLibrary.</param>
		/// </summary>
        public void RemoveCcLibrary(ICcLibrary ccLibrary)
		{
            UmlPackage.RemovePackage(((UpccCcLibrary) ccLibrary).UmlPackage);
		}

		/// <summary>
		/// The BDTLibrarys contained in this bLibrary.
		/// </summary>
		public IEnumerable<IBdtLibrary> GetBdtLibraries()
		{
            foreach (var package in UmlPackage.GetPackagesByStereotype("BDTLibrary"))
            {
                yield return new UpccBdtLibrary(package);
            }
		}

		/// <summary>
		/// Retrieves a BDTLibrary by name.
		/// <param name="name">A BDTLibrary's name.</param>
		/// <returns>The BDTLibrary with the given <paramref name="name"/> or <c>null</c> if no such BDTLibrary is found.</returns>
		/// </summary>
        public IBdtLibrary GetBdtLibraryByName(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				return null;
			}
            foreach (var package in UmlPackage.GetPackagesByStereotype("BDTLibrary"))
            {
				if (name == package.Name)
				{
                    return new UpccBdtLibrary(package);
				}
            }
			return null;
		}

		/// <summary>
		/// Creates a BDTLibrary based on the given <paramref name="specification"/>.
		/// <param name="specification">A specification for a BDTLibrary.</param>
		/// <returns>The newly created BDTLibrary.</returns>
		/// </summary>
		public IBdtLibrary CreateBdtLibrary(BdtLibrarySpec specification)
		{
		    return new UpccBdtLibrary(UmlPackage.CreatePackage(BdtLibrarySpecConverter.Convert(specification)));
		}

		/// <summary>
		/// Updates a BDTLibrary to match the given <paramref name="specification"/>.
		/// <param name="bdtLibrary">A BDTLibrary.</param>
		/// <param name="specification">A new specification for the given BDTLibrary.</param>
		/// <returns>The updated BDTLibrary. Depending on the implementation, this might be the same updated instance or a new instance!</returns>
		/// </summary>
        public IBdtLibrary UpdateBdtLibrary(IBdtLibrary bdtLibrary, BdtLibrarySpec specification)
		{
		    return new UpccBdtLibrary(UmlPackage.UpdatePackage(((UpccBdtLibrary) bdtLibrary).UmlPackage, BdtLibrarySpecConverter.Convert(specification)));
		}

		/// <summary>
		/// Removes a BdtLibrary from this bLibrary.
		/// <param name="bdtLibrary">A BdtLibrary.</param>
		/// </summary>
        public void RemoveBdtLibrary(IBdtLibrary bdtLibrary)
		{
            UmlPackage.RemovePackage(((UpccBdtLibrary) bdtLibrary).UmlPackage);
		}

		/// <summary>
		/// The BIELibrarys contained in this bLibrary.
		/// </summary>
		public IEnumerable<IBieLibrary> GetBieLibraries()
		{
            foreach (var package in UmlPackage.GetPackagesByStereotype("BIELibrary"))
            {
                yield return new UpccBieLibrary(package);
            }
		}

		/// <summary>
		/// Retrieves a BIELibrary by name.
		/// <param name="name">A BIELibrary's name.</param>
		/// <returns>The BIELibrary with the given <paramref name="name"/> or <c>null</c> if no such BIELibrary is found.</returns>
		/// </summary>
        public IBieLibrary GetBieLibraryByName(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				return null;
			}
            foreach (var package in UmlPackage.GetPackagesByStereotype("BIELibrary"))
            {
				if (name == package.Name)
				{
                    return new UpccBieLibrary(package);
				}
            }
			return null;
		}

		/// <summary>
		/// Creates a BIELibrary based on the given <paramref name="specification"/>.
		/// <param name="specification">A specification for a BIELibrary.</param>
		/// <returns>The newly created BIELibrary.</returns>
		/// </summary>
		public IBieLibrary CreateBieLibrary(BieLibrarySpec specification)
		{
		    return new UpccBieLibrary(UmlPackage.CreatePackage(BieLibrarySpecConverter.Convert(specification)));
		}

		/// <summary>
		/// Updates a BIELibrary to match the given <paramref name="specification"/>.
		/// <param name="bieLibrary">A BIELibrary.</param>
		/// <param name="specification">A new specification for the given BIELibrary.</param>
		/// <returns>The updated BIELibrary. Depending on the implementation, this might be the same updated instance or a new instance!</returns>
		/// </summary>
        public IBieLibrary UpdateBieLibrary(IBieLibrary bieLibrary, BieLibrarySpec specification)
		{
		    return new UpccBieLibrary(UmlPackage.UpdatePackage(((UpccBieLibrary) bieLibrary).UmlPackage, BieLibrarySpecConverter.Convert(specification)));
		}

		/// <summary>
		/// Removes a BieLibrary from this bLibrary.
		/// <param name="bieLibrary">A BieLibrary.</param>
		/// </summary>
        public void RemoveBieLibrary(IBieLibrary bieLibrary)
		{
            UmlPackage.RemovePackage(((UpccBieLibrary) bieLibrary).UmlPackage);
		}

		/// <summary>
		/// The DOCLibrarys contained in this bLibrary.
		/// </summary>
		public IEnumerable<IDocLibrary> GetDocLibraries()
		{
            foreach (var package in UmlPackage.GetPackagesByStereotype("DOCLibrary"))
            {
                yield return new UpccDocLibrary(package);
            }
		}

		/// <summary>
		/// Retrieves a DOCLibrary by name.
		/// <param name="name">A DOCLibrary's name.</param>
		/// <returns>The DOCLibrary with the given <paramref name="name"/> or <c>null</c> if no such DOCLibrary is found.</returns>
		/// </summary>
        public IDocLibrary GetDocLibraryByName(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				return null;
			}
            foreach (var package in UmlPackage.GetPackagesByStereotype("DOCLibrary"))
            {
				if (name == package.Name)
				{
                    return new UpccDocLibrary(package);
				}
            }
			return null;
		}

		/// <summary>
		/// Creates a DOCLibrary based on the given <paramref name="specification"/>.
		/// <param name="specification">A specification for a DOCLibrary.</param>
		/// <returns>The newly created DOCLibrary.</returns>
		/// </summary>
		public IDocLibrary CreateDocLibrary(DocLibrarySpec specification)
		{
		    return new UpccDocLibrary(UmlPackage.CreatePackage(DocLibrarySpecConverter.Convert(specification)));
		}

		/// <summary>
		/// Updates a DOCLibrary to match the given <paramref name="specification"/>.
		/// <param name="docLibrary">A DOCLibrary.</param>
		/// <param name="specification">A new specification for the given DOCLibrary.</param>
		/// <returns>The updated DOCLibrary. Depending on the implementation, this might be the same updated instance or a new instance!</returns>
		/// </summary>
        public IDocLibrary UpdateDocLibrary(IDocLibrary docLibrary, DocLibrarySpec specification)
		{
		    return new UpccDocLibrary(UmlPackage.UpdatePackage(((UpccDocLibrary) docLibrary).UmlPackage, DocLibrarySpecConverter.Convert(specification)));
		}

		/// <summary>
		/// Removes a DocLibrary from this bLibrary.
		/// <param name="docLibrary">A DocLibrary.</param>
		/// </summary>
        public void RemoveDocLibrary(IDocLibrary docLibrary)
		{
            UmlPackage.RemovePackage(((UpccDocLibrary) docLibrary).UmlPackage);
		}

        ///<summary>
        /// Tagged value 'businessTerm'.
        ///</summary>
        public IEnumerable<string> BusinessTerms
        {
            get { return UmlPackage.GetTaggedValue("businessTerm").SplitValues; }
        }

        ///<summary>
        /// Tagged value 'copyright'.
        ///</summary>
        public IEnumerable<string> Copyrights
        {
            get { return UmlPackage.GetTaggedValue("copyright").SplitValues; }
        }

        ///<summary>
        /// Tagged value 'owner'.
        ///</summary>
        public IEnumerable<string> Owners
        {
            get { return UmlPackage.GetTaggedValue("owner").SplitValues; }
        }

        ///<summary>
        /// Tagged value 'reference'.
        ///</summary>
        public IEnumerable<string> References
        {
            get { return UmlPackage.GetTaggedValue("reference").SplitValues; }
        }

        ///<summary>
        /// Tagged value 'status'.
        ///</summary>
        public string Status
        {
            get { return UmlPackage.GetTaggedValue("status").Value; }
        }

        ///<summary>
        /// Tagged value 'uniqueIdentifier'.
        ///</summary>
        public string UniqueIdentifier
        {
            get { return UmlPackage.GetTaggedValue("uniqueIdentifier").Value; }
        }

        ///<summary>
        /// Tagged value 'versionIdentifier'.
        ///</summary>
        public string VersionIdentifier
        {
            get { return UmlPackage.GetTaggedValue("versionIdentifier").Value; }
        }

        #endregion

        public bool Equals(UpccBLibrary other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.UmlPackage, UmlPackage);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (UpccBLibrary)) return false;
            return Equals((UpccBLibrary) obj);
        }

        public override int GetHashCode()
        {
            return (UmlPackage != null ? UmlPackage.GetHashCode() : 0);
        }

        public static bool operator ==(UpccBLibrary left, UpccBLibrary right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(UpccBLibrary left, UpccBLibrary right)
        {
            return !Equals(left, right);
        }
	}
}
