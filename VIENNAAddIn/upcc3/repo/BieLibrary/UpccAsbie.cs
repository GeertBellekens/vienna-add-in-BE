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
    internal class UpccAsbie : IAsbie
    {
        public UpccAsbie(IUmlAssociation umlAssociation, IAbie associatingAbie)
        {
            UmlAssociation = umlAssociation;
			AssociatingAbie = associatingAbie;
        }

        public IUmlAssociation UmlAssociation { get; private set; }

        #region IAsbie Members

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

		public AggregationKind AggregationKind
        {
            get { return UmlAssociation.AggregationKind; }
        }
		
        public IAbie AssociatingAbie { get; private set; }
		
		public IAbie AssociatedAbie
		{
			get
			{
				return new UpccAbie((IUmlClass) UmlAssociation.AssociatedClassifier);
			}
		}

		public IAscc BasedOn
        {
            get
            {
                if (AssociatingAbie == null)
                {
                    return null;
                }
                var acc = AssociatingAbie.BasedOn;
                if (acc == null)
                {
                    return null;
                }
                string nameWithoutQualifiers = Name.Substring(Name.LastIndexOf('_') + 1);
                foreach (var ascc in acc.Asccs)
                {
                    if (nameWithoutQualifiers == ascc.Name)
                    {
                        return ascc;
                    }
                }
                return null;
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

        public bool Equals(UpccAsbie other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.UmlAssociation, UmlAssociation);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (UpccAsbie)) return false;
            return Equals((UpccAsbie) obj);
        }

        public override int GetHashCode()
        {
            return (UmlAssociation != null ? UmlAssociation.GetHashCode() : 0);
        }

        public static bool operator ==(UpccAsbie left, UpccAsbie right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(UpccAsbie left, UpccAsbie right)
        {
            return !Equals(left, right);
        }
	}
}

