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
    internal class UpccCdtCon : ICdtCon
    {
        public UpccCdtCon(IUmlAttribute umlAttribute, ICdt cdt)
        {
            UmlAttribute = umlAttribute;
			Cdt = cdt;
        }

        public IUmlAttribute UmlAttribute { get; private set; }

        #region ICdtCon Members

        public int Id
        {
            get { return UmlAttribute.Id; }
        }

        public string Name
        {
            get { return UmlAttribute.Name; }
        }

        public string UpperBound
		{
            get { return UmlAttribute.UpperBound; }
		}
		
        public string LowerBound
		{
            get { return UmlAttribute.LowerBound; }
		}
		
        public bool IsOptional()
        {
            int i;
            return Int32.TryParse(LowerBound, out i) && i == 0;
        }

        public ICdt Cdt { get; private set; }

		public BasicType BasicType
		{
			get
			{
				var type = UmlAttribute.Type;
                switch (type.Stereotype)
                {
                    case "PRIM":
                    	return new BasicType(new UpccPrim((IUmlDataType) type));
                    case "IDSCHEME":
                    	return new BasicType(new UpccIdScheme((IUmlDataType) type));
                    case "ENUM":
                    	return new BasicType(new UpccEnum((IUmlEnumeration) type));
                    default:
                        return null;
                }
			}
		}

        ///<summary>
        /// Tagged value 'businessTerm'.
        ///</summary>
        public IEnumerable<string> BusinessTerms
        {
            get { return UmlAttribute.GetTaggedValue("businessTerm").SplitValues; }
        }

        ///<summary>
        /// Tagged value 'definition'.
        ///</summary>
        public string Definition
        {
            get { return UmlAttribute.GetTaggedValue("definition").Value; }
        }

        ///<summary>
        /// Tagged value 'dictionaryEntryName'.
        ///</summary>
        public string DictionaryEntryName
        {
            get { return UmlAttribute.GetTaggedValue("dictionaryEntryName").Value; }
        }

        ///<summary>
        /// Tagged value 'languageCode'.
        ///</summary>
        public string LanguageCode
        {
            get { return UmlAttribute.GetTaggedValue("languageCode").Value; }
        }

        ///<summary>
        /// Tagged value 'modificationAllowedIndicator'.
        ///</summary>
        public string ModificationAllowedIndicator
        {
            get { return UmlAttribute.GetTaggedValue("modificationAllowedIndicator").Value; }
        }

        ///<summary>
        /// Tagged value 'uniqueIdentifier'.
        ///</summary>
        public string UniqueIdentifier
        {
            get { return UmlAttribute.GetTaggedValue("uniqueIdentifier").Value; }
        }

        ///<summary>
        /// Tagged value 'versionIdentifier'.
        ///</summary>
        public string VersionIdentifier
        {
            get { return UmlAttribute.GetTaggedValue("versionIdentifier").Value; }
        }

        ///<summary>
        /// Tagged value 'usageRule'.
        ///</summary>
        public IEnumerable<string> UsageRules
        {
            get { return UmlAttribute.GetTaggedValue("usageRule").SplitValues; }
        }

		#endregion

        public bool Equals(UpccCdtCon other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.UmlAttribute, UmlAttribute);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (UpccCdtCon)) return false;
            return Equals((UpccCdtCon) obj);
        }

        public override int GetHashCode()
        {
            return (UmlAttribute != null ? UmlAttribute.GetHashCode() : 0);
        }

        public static bool operator ==(UpccCdtCon left, UpccCdtCon right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(UpccCdtCon left, UpccCdtCon right)
        {
            return !Equals(left, right);
        }
	}
}

