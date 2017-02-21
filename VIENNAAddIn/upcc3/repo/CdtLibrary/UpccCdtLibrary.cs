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

namespace VIENNAAddIn.upcc3.repo.CdtLibrary
{
    internal class UpccCdtLibrary : ICdtLibrary
    {
        public UpccCdtLibrary(IUmlPackage umlPackage)
        {
            UmlPackage = umlPackage;
        }

        public IUmlPackage UmlPackage { get; private set; }

        #region ICdtLibrary Members

		/// <summary>
		/// The CDTLibrary's unique ID.
		/// </summary>
        public int Id
        {
            get { return UmlPackage.Id; }
        }

		/// <summary>
		/// The CDTLibrary's name.
		/// </summary>
        public string Name
        {
            get { return UmlPackage.Name; }
        }

		/// <summary>
		/// The bLibrary containing this CDTLibrary.
		/// </summary>
		public IBLibrary BLibrary
        {
            get { return new UpccBLibrary(UmlPackage.Parent); }
        }

		/// <summary>
		/// The CDTs contained in this CDTLibrary.
		/// </summary>
		public IEnumerable<ICdt> Cdts
		{
            get
            {
                foreach (var umlClass in UmlPackage.GetClassesByStereotype("CDT"))
                {
                    yield return new UpccCdt(umlClass);
                }
            }
		}

		/// <summary>
		/// Retrieves a CDT by name.
		/// <param name="name">A CDT's name.</param>
		/// <returns>The CDT with the given <paramref name="name"/> or <c>null</c> if no such CDT is found.</returns>
		/// </summary>
        public ICdt GetCdtByName(string name)
		{
            foreach (ICdt cdt in Cdts)
            {
                if (cdt.Name == name)
                {
                    return cdt;
                }
            }
            return null;
		}

		/// <summary>
		/// Creates a CDT based on the given <paramref name="specification"/>.
		/// <param name="specification">A specification for a CDT.</param>
		/// <returns>The newly created CDT.</returns>
		/// </summary>
		public ICdt CreateCdt(CdtSpec specification)
		{
		    return new UpccCdt(UmlPackage.CreateClass(CdtSpecConverter.Convert(specification)));
		}

		/// <summary>
		/// Updates a CDT to match the given <paramref name="specification"/>.
		/// <param name="cdt">A CDT.</param>
		/// <param name="specification">A new specification for the given CDT.</param>
		/// <returns>The updated CDT. Depending on the implementation, this might be the same updated instance or a new instance!</returns>
		/// </summary>
        public ICdt UpdateCdt(ICdt cdt, CdtSpec specification)
		{
		    return new UpccCdt(UmlPackage.UpdateClass(((UpccCdt) cdt).UmlClass, CdtSpecConverter.Convert(specification)));
		}

		/// <summary>
		/// Removes a CDT from this CDTLibrary.
		/// <param name="cdt">A CDT.</param>
		/// </summary>
        public void RemoveCdt(ICdt cdt)
		{
            UmlPackage.RemoveClass(((UpccCdt) cdt).UmlClass);
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

        ///<summary>
        /// Tagged value 'baseURN'.
        ///</summary>
        public string BaseURN
        {
            get { return UmlPackage.GetTaggedValue("baseURN").Value; }
        }

        ///<summary>
        /// Tagged value 'namespacePrefix'.
        ///</summary>
        public string NamespacePrefix
        {
            get { return UmlPackage.GetTaggedValue("namespacePrefix").Value; }
        }

        #endregion

        public bool Equals(UpccCdtLibrary other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.UmlPackage, UmlPackage);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (UpccCdtLibrary)) return false;
            return Equals((UpccCdtLibrary) obj);
        }

        public override int GetHashCode()
        {
            return (UmlPackage != null ? UmlPackage.GetHashCode() : 0);
        }

        public static bool operator ==(UpccCdtLibrary left, UpccCdtLibrary right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(UpccCdtLibrary left, UpccCdtLibrary right)
        {
            return !Equals(left, right);
        }
	}
}
