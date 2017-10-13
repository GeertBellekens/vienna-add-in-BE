using System.Collections.Generic;
using System.Linq;
using System;
using CctsRepository;
using VIENNAAddIn.upcc3.repo.EnumLibrary;
using VIENNAAddIn.upcc3.repo.PrimLibrary;
using VIENNAAddIn.upcc3.uml;

namespace VIENNAAddIn.upcc3.repo
{
	/// <summary>
	/// Description of UpccAttribute.
	/// </summary>
	public abstract class UpccAttribute: ICctsAttribute
	{
		public IUmlAttribute UmlAttribute { get; private set; }
		protected UpccAttribute(IUmlAttribute umlAttribute)
		{
			UmlAttribute = umlAttribute;
			this.position = umlAttribute.position * 100;
		}
		      
        public int Id
        {
            get { return UmlAttribute.Id; }
        }

		public int position {get;set;}
		

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

        public abstract ICctsElement Owner {get;}

		public BasicType BasicType
		{
			get
			{
				var type = UmlAttribute.Type;
				if (type != null)
				{
					if (type.Stereotypes.Contains("PRIM")) return new BasicType(new UpccPrim((IUmlDataType) type));
					if (type.Stereotypes.Contains("IDSCHEME")) return new BasicType(new UpccIdScheme((IUmlDataType) type));
					if (type.Stereotypes.Contains("ENUM")) return new BasicType(new UpccEnum((IUmlEnumeration) type));
				}
				return null;
			}
		}
        public bool IsOptional()
        {
            int i;
            return Int32.TryParse(LowerBound, out i) && i == 0;
        }
        UpccAttribute _sourceAttribute;
        public UpccAttribute SourceAttribute
        {
        	get
        	{
        		if (_sourceAttribute == null)
        		{
        			var sourceUmlAttribute = this.UmlAttribute.ReferencedAttributes.FirstOrDefault(x => x.Name == this.Name);
        			if (sourceUmlAttribute != null)
        			{
        				_sourceAttribute = ((UpccElement)this.Owner).CreateAttribute(sourceUmlAttribute) as UpccAttribute;
        			}
        		}
        		return _sourceAttribute;
        	}
        }
        List<ICctsFacet> _allFacets;
        List<ICctsFacet> _facets;
		public IEnumerable<ICctsFacet> Facets 
		{
			get 
			{
				getAllFacets();
				return _facets;
			}
		}

		public IEnumerable<ICctsFacet> AllFacets 
		{
			get 
			{
				getAllFacets();
				return _allFacets;
			}
		}

		private void getAllFacets()
		{
			if (_facets == null && _allFacets == null)
			{
				_facets = new List<ICctsFacet>();
				_allFacets = new List<ICctsFacet>();
				addFacet("fractionDigit");
				addFacet("length");
				addFacet("maxExclusive");
				addFacet("maxInclusive");
				addFacet("maxLength");
				addFacet("minExclusive");
				addFacet("minInclusive");
				addFacet("minLength");
				addFacet("pattern");
				addFacet("totalDigits");
				addFacet("whiteSpace");
			}
		}
		void addFacet (string facetName)
		{
			bool noSource = SourceAttribute == null;
			switch (facetName) 
			{
				
				case "fractionDigit":
					var facetValue = this.FractionDigits;
					if (! string.IsNullOrEmpty(facetValue))
					{
						var facet = new UpccFacet(facetName, facetValue);
						_allFacets.Add(facet);
						if ((noSource || SourceAttribute.FractionDigits != facetValue))
							_facets.Add(facet);
					}
					break;
				case "length": 
					facetValue = this.Length;
					if (! string.IsNullOrEmpty(facetValue))
					{
						var facet = new UpccFacet(facetName, facetValue);
						_allFacets.Add(facet);
						if ((noSource || SourceAttribute.Length != facetValue))
							_facets.Add(facet);
					}
					break;
				case "maxExclusive":
					facetValue = this.MaximumExclusive;
					if (! string.IsNullOrEmpty(facetValue))
					{
						var facet = new UpccFacet(facetName, facetValue);
						_allFacets.Add(facet);
						if ((noSource || SourceAttribute.MaximumExclusive != facetValue))
							_facets.Add(facet);
					}
					break;					
				case "maxInclusive":
					facetValue = this.MaximumInclusive;
					if (! string.IsNullOrEmpty(facetValue))
					{
						var facet = new UpccFacet(facetName, facetValue);
						_allFacets.Add(facet);
						if ((noSource || SourceAttribute.MaximumInclusive != facetValue))
							_facets.Add(facet);
					}
					break;						
				case "maxLength":
					facetValue = this.MaximumLength;
					if (! string.IsNullOrEmpty(facetValue))
					{
						var facet = new UpccFacet(facetName, facetValue);
						_allFacets.Add(facet);
						if ((noSource || SourceAttribute.MaximumLength != facetValue))
							_facets.Add(facet);
					}
					break;						 
				case "minExclusive":
					facetValue = this.MinimumExclusive;
					if (! string.IsNullOrEmpty(facetValue))
					{
						var facet = new UpccFacet(facetName, facetValue);
						_allFacets.Add(facet);
						if ((noSource || SourceAttribute.MinimumExclusive != facetValue))
							_facets.Add(facet);
					}
					break;						
				case "minInclusive":
					facetValue = this.MinimumInclusive;
					if (! string.IsNullOrEmpty(facetValue))
					{
						var facet = new UpccFacet(facetName, facetValue);
						_allFacets.Add(facet);
						if ((noSource || SourceAttribute.MinimumInclusive != facetValue))
							_facets.Add(facet);
					}
					break;						 
				case "minLength":
					facetValue = this.MinimumLength;
					if (! string.IsNullOrEmpty(facetValue))
					{
						var facet = new UpccFacet(facetName, facetValue);
						_allFacets.Add(facet);
						if ((noSource || SourceAttribute.MinimumLength != facetValue))
							_facets.Add(facet);
					}
					break;						 
				case "pattern":
					facetValue = this.Pattern;
					if (! string.IsNullOrEmpty(facetValue))
					{
						var facet = new UpccFacet(facetName, facetValue);
						_allFacets.Add(facet);
						if ((noSource || SourceAttribute.Pattern != facetValue))
							_facets.Add(facet);
					}
					break;						 
				case "totalDigits":
					facetValue = this.TotalDigits;
					if (! string.IsNullOrEmpty(facetValue))
					{
						var facet = new UpccFacet(facetName, facetValue);
						_allFacets.Add(facet);
						if ((noSource || SourceAttribute.TotalDigits != facetValue))
							_facets.Add(facet);
					}
					break;						 
				case "whiteSpace":
					facetValue = this.WhiteSpace;
					if (! string.IsNullOrEmpty(facetValue))
					{
						var facet = new UpccFacet(facetName, facetValue);
						_allFacets.Add(facet);
						if ((noSource || SourceAttribute.WhiteSpace != facetValue))
							_facets.Add(facet);
					}
					break;						 
			}
		}
	
		public string Length 
        {
            get { return UmlAttribute.GetTaggedValue("length").Value; }
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
        /// Tagged value 'enumeration'.
        ///</summary>
        public string Enumeration
        {
            get { return UmlAttribute.GetTaggedValue("enumeration").Value; }
        }

        ///<summary>
        /// Tagged value 'fractionDigits'.
        ///</summary>
        public string FractionDigits
        {
            get { return UmlAttribute.GetTaggedValue("fractionDigits").Value; }
        }

        ///<summary>
        /// Tagged value 'languageCode'.
        ///</summary>
        public string LanguageCode
        {
            get { return UmlAttribute.GetTaggedValue("languageCode").Value; }
        }

        ///<summary>
        /// Tagged value 'maximumExclusive'.
        ///</summary>
        public string MaximumExclusive
        {
            get { return UmlAttribute.GetTaggedValue("maximumExclusive").Value; }
        }

        ///<summary>
        /// Tagged value 'maximumInclusive'.
        ///</summary>
        public string MaximumInclusive
        {
            get { return UmlAttribute.GetTaggedValue("maximumInclusive").Value; }
        }

        ///<summary>
        /// Tagged value 'maximumLength'.
        ///</summary>
        public string MaximumLength
        {
            get { return UmlAttribute.GetTaggedValue("maximumLength").Value; }
        }

        ///<summary>
        /// Tagged value 'minimumExclusive'.
        ///</summary>
        public string MinimumExclusive
        {
            get { return UmlAttribute.GetTaggedValue("minimumExclusive").Value; }
        }

        ///<summary>
        /// Tagged value 'minimumInclusive'.
        ///</summary>
        public string MinimumInclusive
        {
            get { return UmlAttribute.GetTaggedValue("minimumInclusive").Value; }
        }

        ///<summary>
        /// Tagged value 'minimumLength'.
        ///</summary>
        public string MinimumLength
        {
            get { return UmlAttribute.GetTaggedValue("minimumLength").Value; }
        }

        ///<summary>
        /// Tagged value 'modificationAllowedIndicator'.
        ///</summary>
        public string ModificationAllowedIndicator
        {
            get { return UmlAttribute.GetTaggedValue("modificationAllowedIndicator").Value; }
        }

        ///<summary>
        /// Tagged value 'pattern'.
        ///</summary>
        public string Pattern
        {
            get { return UmlAttribute.GetTaggedValue("pattern").Value; }
        }

        ///<summary>
        /// Tagged value 'totalDigits'.
        ///</summary>
        public string TotalDigits
        {
            get { return UmlAttribute.GetTaggedValue("totalDigits").Value; }
        }

        ///<summary>
        /// Tagged value 'uniqueIdentifier'.
        ///</summary>
        public string UniqueIdentifier
        {
            get { return UmlAttribute.GetTaggedValue("uniqueIdentifier").Value; }
        }

        ///<summary>
        /// Tagged value 'usageRule'.
        ///</summary>
        public IEnumerable<string> UsageRules
        {
            get { return UmlAttribute.GetTaggedValue("usageRule").SplitValues; }
        }

        ///<summary>
        /// Tagged value 'versionIdentifier'.
        ///</summary>
        public string VersionIdentifier
        {
            get { return UmlAttribute.GetTaggedValue("versionIdentifier").Value; }
        }
        ///<summary>
        /// Tagged value 'SequencingKey'.
        ///</summary>
       	public string SequencingKey 
       	{
			get { return UmlAttribute.GetTaggedValue("SequencingKey").Value; }
		}
		///<summary>
        /// Tagged value 'whiteSpace'.
        ///</summary>
		public string WhiteSpace
		{
			get { return UmlAttribute.GetTaggedValue("whiteSpace").Value; }
		}
	}
}
