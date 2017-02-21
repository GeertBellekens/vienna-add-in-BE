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

namespace VIENNAAddIn.upcc3.repo.BdtLibrary
{
    internal class UpccBdtLibrary : IBdtLibrary
    {
        public UpccBdtLibrary(IUmlPackage umlPackage)
        {
            UmlPackage = umlPackage;
        }

        public IUmlPackage UmlPackage { get; private set; }

        #region IBdtLibrary Members

		/// <summary>
		/// The BDTLibrary's unique ID.
		/// </summary>
        public int Id
        {
            get { return UmlPackage.Id; }
        }

		/// <summary>
		/// The BDTLibrary's name.
		/// </summary>
        public string Name
        {
            get { return UmlPackage.Name; }
        }

		/// <summary>
		/// The bLibrary containing this BDTLibrary.
		/// </summary>
		public IBLibrary BLibrary
        {
            get { return new UpccBLibrary(UmlPackage.Parent); }
        }

		/// <summary>
		/// The BDTs contained in this BDTLibrary.
		/// </summary>
		public IEnumerable<IBdt> Bdts
		{
            get
            {
                foreach (var umlClass in UmlPackage.GetClassesByStereotype("BDT"))
                {
                    yield return new UpccBdt(umlClass);
                }
            }
		}

		/// <summary>
		/// Retrieves a BDT by name.
		/// <param name="name">A BDT's name.</param>
		/// <returns>The BDT with the given <paramref name="name"/> or <c>null</c> if no such BDT is found.</returns>
		/// </summary>
        public IBdt GetBdtByName(string name)
		{
            foreach (IBdt bdt in Bdts)
            {
                if (bdt.Name == name)
                {
                    return bdt;
                }
            }
            return null;
		}

		/// <summary>
		/// Creates a BDT based on the given <paramref name="specification"/>.
		/// <param name="specification">A specification for a BDT.</param>
		/// <returns>The newly created BDT.</returns>
		/// </summary>
		public IBdt CreateBdt(BdtSpec specification)
		{
		    return new UpccBdt(UmlPackage.CreateClass(BdtSpecConverter.Convert(specification)));
		}

		/// <summary>
		/// Updates a BDT to match the given <paramref name="specification"/>.
		/// <param name="bdt">A BDT.</param>
		/// <param name="specification">A new specification for the given BDT.</param>
		/// <returns>The updated BDT. Depending on the implementation, this might be the same updated instance or a new instance!</returns>
		/// </summary>
        public IBdt UpdateBdt(IBdt bdt, BdtSpec specification)
		{
		    return new UpccBdt(UmlPackage.UpdateClass(((UpccBdt) bdt).UmlClass, BdtSpecConverter.Convert(specification)));
		}

		/// <summary>
		/// Removes a BDT from this BDTLibrary.
		/// <param name="bdt">A BDT.</param>
		/// </summary>
        public void RemoveBdt(IBdt bdt)
		{
            UmlPackage.RemoveClass(((UpccBdt) bdt).UmlClass);
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

        public bool Equals(UpccBdtLibrary other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.UmlPackage, UmlPackage);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (UpccBdtLibrary)) return false;
            return Equals((UpccBdtLibrary) obj);
        }

        public override int GetHashCode()
        {
            return (UmlPackage != null ? UmlPackage.GetHashCode() : 0);
        }

        public static bool operator ==(UpccBdtLibrary left, UpccBdtLibrary right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(UpccBdtLibrary left, UpccBdtLibrary right)
        {
            return !Equals(left, right);
        }
	}
}
