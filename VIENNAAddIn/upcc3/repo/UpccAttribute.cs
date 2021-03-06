﻿using System.Collections.Generic;
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
    public abstract class UpccAttribute : ICctsAttribute
    {
        public IUmlAttribute UmlAttribute { get; private set; }
        protected UpccAttribute(IUmlAttribute umlAttribute)
        {
            UmlAttribute = umlAttribute;


        }

        public int Id
        {
            get { return UmlAttribute.Id; }
        }
        private int? _position;
        public int position
        {
            get
            {
                if (!_position.HasValue)
                {
                    this._position = getPosition();
                }
                return _position.Value;
            }
            set
            {
                this._position = value;
            }
        }
        protected virtual int getPosition()
        {
            int attributePosition;
            return int.TryParse(this.SequencingKey, out attributePosition) ? attributePosition : this.UmlAttribute.position;
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

        public abstract ICctsElement Owner { get; }


        public BasicType BasicType
        {
            get
            {
                var type = UmlAttribute.Type;
                if (type != null)
                {
                    if (type.Stereotypes.Contains("PRIM")) return new BasicType(new UpccPrim((IUmlDataType)type));
                    if (type.Stereotypes.Contains("IDSCHEME")) return new BasicType(new UpccIdScheme((IUmlDataType)type));
                    if (type.Stereotypes.Contains("ENUM")) return new BasicType(new UpccEnum((IUmlEnumeration)type));
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
                    var sourceUmlAttribute = this.UmlAttribute.ReferencedAttributes("sourceAttribute").FirstOrDefault();
                    if (sourceUmlAttribute != null)
                    {
                        _sourceAttribute = ((UpccElement)this.Owner).CreateAttribute(sourceUmlAttribute) as UpccAttribute;
                    }
                }
                return _sourceAttribute;
            }
        }
        List<ICctsProperty> _otherPropertiesInChoice;
        public IEnumerable<ICctsProperty> otherPropertiesInChoice
        {
            get
            {
                if (_otherPropertiesInChoice == null)
                {
                    _otherPropertiesInChoice = new List<ICctsProperty>();
                    foreach (var otherUmlAttribute in this.UmlAttribute.ReferencedAttributes("Choice"))
                    {
                        var otherAttribute = this.Owner.Attributes.FirstOrDefault(x => x.Id == otherUmlAttribute.Id);
                        if (otherAttribute != null)
                        {
                            _otherPropertiesInChoice.Add(otherAttribute);
                        }
                    }
                    foreach (var otherUMLassociation in this.UmlAttribute.ReferencedAssociations("Choice"))
                    {
                        var otherAssociation = this.Owner.Associations.FirstOrDefault(x => x.Id == otherUMLassociation.Id);
                        if (otherAssociation != null)
                        {
                            _otherPropertiesInChoice.Add(otherAssociation);
                        }
                    }
                }
                return _otherPropertiesInChoice;
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
                addFacet("enumeration");
            }
        }
        void addFacet(string facetName)
        {
            bool noSource = SourceAttribute == null;
            switch (facetName)
            {

                case "fractionDigit":
                    var facetValue = this.FractionDigits;
                    if (!string.IsNullOrEmpty(facetValue))
                    {
                        var facet = new UpccFacet(facetName, facetValue);
                        _allFacets.Add(facet);
                        //if ((noSource || SourceAttribute.FractionDigits != facetValue))
                        _facets.Add(facet);
                    }
                    break;
                case "length":
                    facetValue = this.Length;
                    if (!string.IsNullOrEmpty(facetValue))
                    {
                        var facet = new UpccFacet(facetName, facetValue);
                        _allFacets.Add(facet);
                        //if ((noSource || SourceAttribute.Length != facetValue))
                        _facets.Add(facet);
                    }
                    break;
                case "maxExclusive":
                    facetValue = this.MaximumExclusive;
                    if (!string.IsNullOrEmpty(facetValue))
                    {
                        var facet = new UpccFacet(facetName, facetValue);
                        _allFacets.Add(facet);
                        //if ((noSource || SourceAttribute.MaximumExclusive != facetValue))
                        _facets.Add(facet);
                    }
                    break;
                case "maxInclusive":
                    facetValue = this.MaximumInclusive;
                    if (!string.IsNullOrEmpty(facetValue))
                    {
                        var facet = new UpccFacet(facetName, facetValue);
                        _allFacets.Add(facet);
                        //if ((noSource || SourceAttribute.MaximumInclusive != facetValue))
                        _facets.Add(facet);
                    }
                    break;
                case "maxLength":
                    facetValue = this.MaximumLength;
                    if (!string.IsNullOrEmpty(facetValue))
                    {
                        var facet = new UpccFacet(facetName, facetValue);
                        _allFacets.Add(facet);
                        //if ((noSource || SourceAttribute.MaximumLength != facetValue))
                        _facets.Add(facet);
                    }
                    break;
                case "minExclusive":
                    facetValue = this.MinimumExclusive;
                    if (!string.IsNullOrEmpty(facetValue))
                    {
                        var facet = new UpccFacet(facetName, facetValue);
                        _allFacets.Add(facet);
                        //if ((noSource || SourceAttribute.MinimumExclusive != facetValue))
                        _facets.Add(facet);
                    }
                    break;
                case "minInclusive":
                    facetValue = this.MinimumInclusive;
                    if (!string.IsNullOrEmpty(facetValue))
                    {
                        var facet = new UpccFacet(facetName, facetValue);
                        _allFacets.Add(facet);
                        //if ((noSource || SourceAttribute.MinimumInclusive != facetValue))
                        _facets.Add(facet);
                    }
                    break;
                case "minLength":
                    facetValue = this.MinimumLength;
                    if (!string.IsNullOrEmpty(facetValue))
                    {
                        var facet = new UpccFacet(facetName, facetValue);
                        _allFacets.Add(facet);
                        //if ((noSource || SourceAttribute.MinimumLength != facetValue))
                        _facets.Add(facet);
                    }
                    break;
                case "pattern":
                    facetValue = this.Pattern;
                    if (!string.IsNullOrEmpty(facetValue))
                    {
                        var facet = new UpccFacet(facetName, facetValue);
                        _allFacets.Add(facet);
                        //if ((noSource || SourceAttribute.Pattern != facetValue))
                        _facets.Add(facet);
                    }
                    break;
                case "totalDigits":
                    facetValue = this.TotalDigits;
                    if (!string.IsNullOrEmpty(facetValue))
                    {
                        var facet = new UpccFacet(facetName, facetValue);
                        _allFacets.Add(facet);
                        //if ((noSource || SourceAttribute.TotalDigits != facetValue))
                        _facets.Add(facet);
                    }
                    break;
                case "whiteSpace":
                    facetValue = this.WhiteSpace;
                    if (!string.IsNullOrEmpty(facetValue))
                    {
                        var facet = new UpccFacet(facetName, facetValue);
                        _allFacets.Add(facet);
                        //if ((noSource || SourceAttribute.WhiteSpace != facetValue))
                        _facets.Add(facet);
                    }
                    break;
                case "enumeration":
                    facetValue = this.Enumeration;
                    if (!string.IsNullOrEmpty(facetValue))
                    {
                        var facet = new UpccFacet(facetName, facetValue);
                        _allFacets.Add(facet);
                        //if ((noSource || SourceAttribute.WhiteSpace != facetValue))
                        _facets.Add(facet);
                    }
                    break;
            }
        }

        public string Length
        {
            get { return getFacetTaggedValue("length").Value; }
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
            get { return getFacetTaggedValue("enumeration").Value; }
        }

        ///<summary>
        /// Tagged value 'fractionDigits'.
        ///</summary>
        public string FractionDigits
        {
            get { return getFacetTaggedValue("fractionDigits").Value; }
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
            get { return getFacetTaggedValue("maxExclusive").Value; }
        }

        ///<summary>
        /// Tagged value 'maximumInclusive'.
        ///</summary>
        public string MaximumInclusive
        {
            get { return getFacetTaggedValue("maxInclusive").Value; }
        }

        ///<summary>
        /// Tagged value 'maximumLength'.
        ///</summary>
        public string MaximumLength
        {
            get { return getFacetTaggedValue("maxLength").Value; }
        }

        ///<summary>
        /// Tagged value 'minimumExclusive'.
        ///</summary>
        public string MinimumExclusive
        {
            get { return getFacetTaggedValue("minExclusive").Value; }
        }

        ///<summary>
        /// Tagged value 'minimumInclusive'.
        ///</summary>
        public string MinimumInclusive
        {
            get { return getFacetTaggedValue("minInclusive").Value; }
        }

        ///<summary>
        /// Tagged value 'minimumLength'.
        ///</summary>
        public string MinimumLength
        {
            get { return getFacetTaggedValue("minLength").Value; }
        }

        public IUmlTaggedValue getFacetTaggedValue(string tagname)
        {
            //first check if there is an overridden version
            var overriddenTagName = "override_" + tagname;
            var overriddentag = UmlAttribute.GetTaggedValue(overriddenTagName);
            return overriddentag.IsDefined ?
                    overriddentag :
                    UmlAttribute.GetTaggedValue(tagname);
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
            get { return getFacetTaggedValue("pattern").Value; }
        }

        ///<summary>
        /// Tagged value 'totalDigits'.
        ///</summary>
        public string TotalDigits
        {
            get { return getFacetTaggedValue("totalDigits").Value; }
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
            get { return UmlAttribute.GetTaggedValue("sequencingKey").Value; }
        }
        ///<summary>
        /// Tagged value 'whiteSpace'.
        ///</summary>
        public string WhiteSpace
        {
            get { return getFacetTaggedValue("whiteSpace").Value; }
        }
    }
}
