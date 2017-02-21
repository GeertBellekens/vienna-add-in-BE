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
using System.Collections.Generic;
using VIENNAAddIn.upcc3.uml;

namespace VIENNAAddIn.upcc3.repo.EnumLibrary
{
    internal class UpccIdScheme : IIdScheme
    {
        public UpccIdScheme(IUmlDataType umlDataType)
        {
            UmlDataType = umlDataType;
        }

        public IUmlDataType UmlDataType { get; private set; }

        #region IIdScheme Members

        public int Id
        {
            get { return UmlDataType.Id; }
        }

        public string Name
        {
            get { return UmlDataType.Name; }
        }

		public IEnumLibrary EnumLibrary
        {
            get { return new UpccEnumLibrary(UmlDataType.Package); }
        }

        ///<summary>
        /// Tagged value 'businessTerm'.
        ///</summary>
        public IEnumerable<string> BusinessTerms
        {
            get { return UmlDataType.GetTaggedValue("businessTerm").SplitValues; }
        }

        ///<summary>
        /// Tagged value 'definition'.
        ///</summary>
        public string Definition
        {
            get { return UmlDataType.GetTaggedValue("definition").Value; }
        }

        ///<summary>
        /// Tagged value 'dictionaryEntryName'.
        ///</summary>
        public string DictionaryEntryName
        {
            get { return UmlDataType.GetTaggedValue("dictionaryEntryName").Value; }
        }

        ///<summary>
        /// Tagged value 'identifierSchemeAgencyIdentifier'.
        ///</summary>
        public string IdentifierSchemeAgencyIdentifier
        {
            get { return UmlDataType.GetTaggedValue("identifierSchemeAgencyIdentifier").Value; }
        }

        ///<summary>
        /// Tagged value 'identifierSchemeAgencyName'.
        ///</summary>
        public string IdentifierSchemeAgencyName
        {
            get { return UmlDataType.GetTaggedValue("identifierSchemeAgencyName").Value; }
        }

        ///<summary>
        /// Tagged value 'modificationAllowedIndicator'.
        ///</summary>
        public string ModificationAllowedIndicator
        {
            get { return UmlDataType.GetTaggedValue("modificationAllowedIndicator").Value; }
        }

        ///<summary>
        /// Tagged value 'pattern'.
        ///</summary>
        public string Pattern
        {
            get { return UmlDataType.GetTaggedValue("pattern").Value; }
        }

        ///<summary>
        /// Tagged value 'restrictedPrimitive'.
        ///</summary>
        public string RestrictedPrimitive
        {
            get { return UmlDataType.GetTaggedValue("restrictedPrimitive").Value; }
        }

        ///<summary>
        /// Tagged value 'uniqueIdentifier'.
        ///</summary>
        public string UniqueIdentifier
        {
            get { return UmlDataType.GetTaggedValue("uniqueIdentifier").Value; }
        }

        ///<summary>
        /// Tagged value 'versionIdentifier'.
        ///</summary>
        public string VersionIdentifier
        {
            get { return UmlDataType.GetTaggedValue("versionIdentifier").Value; }
        }

        #endregion

        public bool Equals(UpccIdScheme other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.UmlDataType, UmlDataType);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (UpccIdScheme)) return false;
            return Equals((UpccIdScheme) obj);
        }

        public override int GetHashCode()
        {
            return (UmlDataType != null ? UmlDataType.GetHashCode() : 0);
        }

        public static bool operator ==(UpccIdScheme left, UpccIdScheme right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(UpccIdScheme left, UpccIdScheme right)
        {
            return !Equals(left, right);
        }
	}
}
