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

namespace VIENNAAddIn.upcc3.repo.DocLibrary
{
    internal partial class UpccDocLibrary : IDocLibrary
    {
        public UpccDocLibrary(IUmlPackage umlPackage)
        {
            UmlPackage = umlPackage;
        }

        public IUmlPackage UmlPackage { get; private set; }

        #region IDocLibrary Members

		/// <summary>
		/// The DOCLibrary's unique ID.
		/// </summary>
        public int Id
        {
            get { return UmlPackage.Id; }
        }

		/// <summary>
		/// The DOCLibrary's name.
		/// </summary>
        public string Name
        {
            get { return UmlPackage.Name; }
        }

		/// <summary>
		/// The bLibrary containing this DOCLibrary.
		/// </summary>
		public IBLibrary BLibrary
        {
            get { return new UpccBLibrary(UmlPackage.Parent); }
        }

		/// <summary>
		/// The MAs contained in this DOCLibrary.
		/// </summary>
		public IEnumerable<IMa> Mas
		{
            get
            {
                foreach (var umlClass in UmlPackage.GetClassesByStereotype("MA"))
                {
                    yield return new UpccMa(umlClass);
                }
            }
		}

		/// <summary>
		/// Retrieves a MA by name.
		/// <param name="name">A MA's name.</param>
		/// <returns>The MA with the given <paramref name="name"/> or <c>null</c> if no such MA is found.</returns>
		/// </summary>
        public IMa GetMaByName(string name)
		{
            foreach (IMa ma in Mas)
            {
                if (ma.Name == name)
                {
                    return ma;
                }
            }
            return null;
		}

		/// <summary>
		/// Creates a MA based on the given <paramref name="specification"/>.
		/// <param name="specification">A specification for a MA.</param>
		/// <returns>The newly created MA.</returns>
		/// </summary>
		public IMa CreateMa(MaSpec specification)
		{
		    return new UpccMa(UmlPackage.CreateClass(MaSpecConverter.Convert(specification)));
		}

		/// <summary>
		/// Updates a MA to match the given <paramref name="specification"/>.
		/// <param name="ma">A MA.</param>
		/// <param name="specification">A new specification for the given MA.</param>
		/// <returns>The updated MA. Depending on the implementation, this might be the same updated instance or a new instance!</returns>
		/// </summary>
        public IMa UpdateMa(IMa ma, MaSpec specification)
		{
		    return new UpccMa(UmlPackage.UpdateClass(((UpccMa) ma).UmlClass, MaSpecConverter.Convert(specification)));
		}

		/// <summary>
		/// Removes a MA from this DOCLibrary.
		/// <param name="ma">A MA.</param>
		/// </summary>
        public void RemoveMa(IMa ma)
		{
            UmlPackage.RemoveClass(((UpccMa) ma).UmlClass);
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

        public bool Equals(UpccDocLibrary other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.UmlPackage, UmlPackage);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (UpccDocLibrary)) return false;
            return Equals((UpccDocLibrary) obj);
        }

        public override int GetHashCode()
        {
            return (UmlPackage != null ? UmlPackage.GetHashCode() : 0);
        }

        public static bool operator ==(UpccDocLibrary left, UpccDocLibrary right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(UpccDocLibrary left, UpccDocLibrary right)
        {
            return !Equals(left, right);
        }
	}
}
