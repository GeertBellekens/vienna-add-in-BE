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

namespace VIENNAAddIn.upcc3.repo.CcLibrary
{
    internal class UpccBcc : IBcc
    {
        public UpccBcc(IUmlAttribute umlAttribute, IAcc acc)
        {
            UmlAttribute = umlAttribute;
			Acc = acc;
        }

        public IUmlAttribute UmlAttribute { get; private set; }

        #region IBcc Members

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

        public IAcc Acc { get; private set; }

		public ICdt Cdt
		{
			get
			{
				var type = UmlAttribute.Type;
				return new UpccCdt((IUmlClass) type);
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
        /// Tagged value 'sequencingKey'.
        ///</summary>
        public string SequencingKey
        {
            get { return UmlAttribute.GetTaggedValue("sequencingKey").Value; }
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

        public bool Equals(UpccBcc other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.UmlAttribute, UmlAttribute);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (UpccBcc)) return false;
            return Equals((UpccBcc) obj);
        }

        public override int GetHashCode()
        {
            return (UmlAttribute != null ? UmlAttribute.GetHashCode() : 0);
        }

        public static bool operator ==(UpccBcc left, UpccBcc right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(UpccBcc left, UpccBcc right)
        {
            return !Equals(left, right);
        }
	}
}

