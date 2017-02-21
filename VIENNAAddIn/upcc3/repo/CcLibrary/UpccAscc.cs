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
    internal class UpccAscc : IAscc
    {
        public UpccAscc(IUmlAssociation umlAssociation, IAcc associatingAcc)
        {
            UmlAssociation = umlAssociation;
			AssociatingAcc = associatingAcc;
        }

        public IUmlAssociation UmlAssociation { get; private set; }

        #region IAscc Members

        public int Id
        {
            get { return UmlAssociation.Id; }
        }

        public string Name
        {
            get { return UmlAssociation.Name; }
        }

        public string UpperBound
		{
            get { return UmlAssociation.UpperBound; }
		}
		
        public string LowerBound
		{
            get { return UmlAssociation.LowerBound; }
		}
		
        public bool IsOptional()
        {
            int i;
            return Int32.TryParse(LowerBound, out i) && i == 0;
        }
		
        public IAcc AssociatingAcc { get; private set; }
		
		public IAcc AssociatedAcc
		{
			get
			{
				return new UpccAcc((IUmlClass) UmlAssociation.AssociatedClassifier);
			}
		}

        ///<summary>
        /// Tagged value 'businessTerm'.
        ///</summary>
        public IEnumerable<string> BusinessTerms
        {
            get { return UmlAssociation.GetTaggedValue("businessTerm").SplitValues; }
        }

        ///<summary>
        /// Tagged value 'definition'.
        ///</summary>
        public string Definition
        {
            get { return UmlAssociation.GetTaggedValue("definition").Value; }
        }

        ///<summary>
        /// Tagged value 'dictionaryEntryName'.
        ///</summary>
        public string DictionaryEntryName
        {
            get { return UmlAssociation.GetTaggedValue("dictionaryEntryName").Value; }
        }

        ///<summary>
        /// Tagged value 'languageCode'.
        ///</summary>
        public string LanguageCode
        {
            get { return UmlAssociation.GetTaggedValue("languageCode").Value; }
        }

        ///<summary>
        /// Tagged value 'sequencingKey'.
        ///</summary>
        public string SequencingKey
        {
            get { return UmlAssociation.GetTaggedValue("sequencingKey").Value; }
        }

        ///<summary>
        /// Tagged value 'uniqueIdentifier'.
        ///</summary>
        public string UniqueIdentifier
        {
            get { return UmlAssociation.GetTaggedValue("uniqueIdentifier").Value; }
        }

        ///<summary>
        /// Tagged value 'versionIdentifier'.
        ///</summary>
        public string VersionIdentifier
        {
            get { return UmlAssociation.GetTaggedValue("versionIdentifier").Value; }
        }

        ///<summary>
        /// Tagged value 'usageRule'.
        ///</summary>
        public IEnumerable<string> UsageRules
        {
            get { return UmlAssociation.GetTaggedValue("usageRule").SplitValues; }
        }

		#endregion

        public bool Equals(UpccAscc other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.UmlAssociation, UmlAssociation);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (UpccAscc)) return false;
            return Equals((UpccAscc) obj);
        }

        public override int GetHashCode()
        {
            return (UmlAssociation != null ? UmlAssociation.GetHashCode() : 0);
        }

        public static bool operator ==(UpccAscc left, UpccAscc right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(UpccAscc left, UpccAscc right)
        {
            return !Equals(left, right);
        }
	}
}

