using System.Collections.Generic;
using System.Linq;
using System;
using CctsRepository;
using VIENNAAddIn.upcc3.uml;

namespace VIENNAAddIn.upcc3.repo
{
	/// <summary>
	/// Description of UpccAssociation.
	/// </summary>
	public abstract class UpccAssociation : ICctsAssociation
	{
		protected ICctsElement _associatingElement;
				
		public UpccAssociation(IUmlAssociation umlAssociation, ICctsElement associatingElement)
        {
            UmlAssociation = umlAssociation;
			_associatingElement = associatingElement;
			//set initial position
			int sequence ;
			if (int.TryParse(this.SequencingKey, out sequence))
			{
				this.position = sequence;
			}
			//zero means not ordered so it should appear last.
			if (position == 0) position = int.MaxValue;
		}
		public IUmlAssociation UmlAssociation { get; private set; }

		public ICctsElement Owner 
		{
			get {return this.AssociatingElement;}
		}

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

		public ICctsElement AssociatingElement 
		{
			get {return _associatingElement;}
		}

		public abstract ICctsElement AssociatedElement {get;}
		
		
		public int position {get;set;}

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

	}
}
