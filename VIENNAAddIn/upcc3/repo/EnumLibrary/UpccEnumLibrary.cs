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

namespace VIENNAAddIn.upcc3.repo.EnumLibrary
{
    internal class UpccEnumLibrary : IEnumLibrary
    {
        public UpccEnumLibrary(IUmlPackage umlPackage)
        {
            UmlPackage = umlPackage;
        }

        public IUmlPackage UmlPackage { get; private set; }

        #region IEnumLibrary Members

		/// <summary>
		/// The ENUMLibrary's unique ID.
		/// </summary>
        public int Id
        {
            get { return UmlPackage.Id; }
        }

		/// <summary>
		/// The ENUMLibrary's name.
		/// </summary>
        public string Name
        {
            get { return UmlPackage.Name; }
        }

		/// <summary>
		/// The bLibrary containing this ENUMLibrary.
		/// </summary>
		public IBLibrary BLibrary
        {
            get { return new UpccBLibrary(UmlPackage.Parent); }
        }

		/// <summary>
		/// The ENUMs contained in this ENUMLibrary.
		/// </summary>
		public IEnumerable<IEnum> Enums
		{
            get
            {
                foreach (var umlEnumeration in UmlPackage.GetEnumerationsByStereotype("ENUM"))
                {
                    yield return new UpccEnum(umlEnumeration);
                }
            }
		}

		/// <summary>
		/// Retrieves a ENUM by name.
		/// <param name="name">A ENUM's name.</param>
		/// <returns>The ENUM with the given <paramref name="name"/> or <c>null</c> if no such ENUM is found.</returns>
		/// </summary>
        public IEnum GetEnumByName(string name)
		{
            foreach (IEnum @enum in Enums)
            {
                if (@enum.Name == name)
                {
                    return @enum;
                }
            }
            return null;
		}

		/// <summary>
		/// Creates a ENUM based on the given <paramref name="specification"/>.
		/// <param name="specification">A specification for a ENUM.</param>
		/// <returns>The newly created ENUM.</returns>
		/// </summary>
		public IEnum CreateEnum(EnumSpec specification)
		{
		    return new UpccEnum(UmlPackage.CreateEnumeration(EnumSpecConverter.Convert(specification)));
		}

		/// <summary>
		/// Updates a ENUM to match the given <paramref name="specification"/>.
		/// <param name="@enum">A ENUM.</param>
		/// <param name="specification">A new specification for the given ENUM.</param>
		/// <returns>The updated ENUM. Depending on the implementation, this might be the same updated instance or a new instance!</returns>
		/// </summary>
        public IEnum UpdateEnum(IEnum @enum, EnumSpec specification)
		{
		    return new UpccEnum(UmlPackage.UpdateEnumeration(((UpccEnum) @enum).UmlEnumeration, EnumSpecConverter.Convert(specification)));
		}

		/// <summary>
		/// Removes a ENUM from this ENUMLibrary.
		/// <param name="@enum">A ENUM.</param>
		/// </summary>
        public void RemoveEnum(IEnum @enum)
		{
            UmlPackage.RemoveEnumeration(((UpccEnum) @enum).UmlEnumeration);
		}

		/// <summary>
		/// The IDSCHEMEs contained in this ENUMLibrary.
		/// </summary>
		public IEnumerable<IIdScheme> IdSchemes
		{
            get
            {
                foreach (var umlDataType in UmlPackage.GetDataTypesByStereotype("IDSCHEME"))
                {
                    yield return new UpccIdScheme(umlDataType);
                }
            }
		}

		/// <summary>
		/// Retrieves a IDSCHEME by name.
		/// <param name="name">A IDSCHEME's name.</param>
		/// <returns>The IDSCHEME with the given <paramref name="name"/> or <c>null</c> if no such IDSCHEME is found.</returns>
		/// </summary>
        public IIdScheme GetIdSchemeByName(string name)
		{
            foreach (IIdScheme idScheme in IdSchemes)
            {
                if (idScheme.Name == name)
                {
                    return idScheme;
                }
            }
            return null;
		}

		/// <summary>
		/// Creates a IDSCHEME based on the given <paramref name="specification"/>.
		/// <param name="specification">A specification for a IDSCHEME.</param>
		/// <returns>The newly created IDSCHEME.</returns>
		/// </summary>
		public IIdScheme CreateIdScheme(IdSchemeSpec specification)
		{
		    return new UpccIdScheme(UmlPackage.CreateDataType(IdSchemeSpecConverter.Convert(specification)));
		}

		/// <summary>
		/// Updates a IDSCHEME to match the given <paramref name="specification"/>.
		/// <param name="idScheme">A IDSCHEME.</param>
		/// <param name="specification">A new specification for the given IDSCHEME.</param>
		/// <returns>The updated IDSCHEME. Depending on the implementation, this might be the same updated instance or a new instance!</returns>
		/// </summary>
        public IIdScheme UpdateIdScheme(IIdScheme idScheme, IdSchemeSpec specification)
		{
		    return new UpccIdScheme(UmlPackage.UpdateDataType(((UpccIdScheme) idScheme).UmlDataType, IdSchemeSpecConverter.Convert(specification)));
		}

		/// <summary>
		/// Removes a IDSCHEME from this ENUMLibrary.
		/// <param name="idScheme">A IDSCHEME.</param>
		/// </summary>
        public void RemoveIdScheme(IIdScheme idScheme)
		{
            UmlPackage.RemoveDataType(((UpccIdScheme) idScheme).UmlDataType);
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

        public bool Equals(UpccEnumLibrary other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.UmlPackage, UmlPackage);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (UpccEnumLibrary)) return false;
            return Equals((UpccEnumLibrary) obj);
        }

        public override int GetHashCode()
        {
            return (UmlPackage != null ? UmlPackage.GetHashCode() : 0);
        }

        public static bool operator ==(UpccEnumLibrary left, UpccEnumLibrary right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(UpccEnumLibrary left, UpccEnumLibrary right)
        {
            return !Equals(left, right);
        }
	}
}
