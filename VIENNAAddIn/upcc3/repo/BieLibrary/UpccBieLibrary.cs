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

namespace VIENNAAddIn.upcc3.repo.BieLibrary
{
    internal class UpccBieLibrary : IBieLibrary
    {
        public UpccBieLibrary(IUmlPackage umlPackage)
        {
            UmlPackage = umlPackage;
        }

        public IUmlPackage UmlPackage { get; private set; }

        #region IBieLibrary Members

		/// <summary>
		/// The BIELibrary's unique ID.
		/// </summary>
        public int Id
        {
            get { return UmlPackage.Id; }
        }

		/// <summary>
		/// The BIELibrary's name.
		/// </summary>
        public string Name
        {
            get { return UmlPackage.Name; }
        }

		/// <summary>
		/// The bLibrary containing this BIELibrary.
		/// </summary>
		public IBLibrary BLibrary
        {
            get { return new UpccBLibrary(UmlPackage.Parent); }
        }

		/// <summary>
		/// The ABIEs contained in this BIELibrary.
		/// </summary>
		public IEnumerable<IAbie> Abies
		{
            get
            {
                foreach (var umlClass in UmlPackage.GetClassesByStereotype("ABIE"))
                {
                    yield return new UpccAbie(umlClass);
                }
            }
		}

		/// <summary>
		/// Retrieves a ABIE by name.
		/// <param name="name">A ABIE's name.</param>
		/// <returns>The ABIE with the given <paramref name="name"/> or <c>null</c> if no such ABIE is found.</returns>
		/// </summary>
        public IAbie GetAbieByName(string name)
		{
            foreach (IAbie abie in Abies)
            {
                if (abie.Name == name)
                {
                    return abie;
                }
            }
            return null;
		}

		/// <summary>
		/// Creates a ABIE based on the given <paramref name="specification"/>.
		/// <param name="specification">A specification for a ABIE.</param>
		/// <returns>The newly created ABIE.</returns>
		/// </summary>
		public IAbie CreateAbie(AbieSpec specification)
		{
		    return new UpccAbie(UmlPackage.CreateClass(AbieSpecConverter.Convert(specification)));
		}

		/// <summary>
		/// Updates a ABIE to match the given <paramref name="specification"/>.
		/// <param name="abie">A ABIE.</param>
		/// <param name="specification">A new specification for the given ABIE.</param>
		/// <returns>The updated ABIE. Depending on the implementation, this might be the same updated instance or a new instance!</returns>
		/// </summary>
        public IAbie UpdateAbie(IAbie abie, AbieSpec specification)
		{
		    return new UpccAbie(UmlPackage.UpdateClass(((UpccAbie) abie).UmlClass, AbieSpecConverter.Convert(specification)));
		}

		/// <summary>
		/// Removes a ABIE from this BIELibrary.
		/// <param name="abie">A ABIE.</param>
		/// </summary>
        public void RemoveAbie(IAbie abie)
		{
            UmlPackage.RemoveClass(((UpccAbie) abie).UmlClass);
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

        public bool Equals(UpccBieLibrary other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.UmlPackage, UmlPackage);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (UpccBieLibrary)) return false;
            return Equals((UpccBieLibrary) obj);
        }

        public override int GetHashCode()
        {
            return (UmlPackage != null ? UmlPackage.GetHashCode() : 0);
        }

        public static bool operator ==(UpccBieLibrary left, UpccBieLibrary right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(UpccBieLibrary left, UpccBieLibrary right)
        {
            return !Equals(left, right);
        }
	}
}
