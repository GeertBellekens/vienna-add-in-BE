using System;
using System.Collections.Generic;
using EA;
using VIENNAAddIn.upcc3.uml;
using Attribute=EA.Attribute;

namespace VIENNAAddIn.upcc3.ea
{
    internal class EaUmlAttribute : IUmlAttribute, IEquatable<EaUmlAttribute>
    {
        private readonly Attribute eaAttribute;
        private readonly Repository eaRepository;
        protected EaUmlRepository repository { get; set; }

        public EaUmlAttribute(Repository eaRepository, Attribute eaAttribute)
        {
            this.eaRepository = eaRepository;
            this.eaAttribute = eaAttribute;
            this.repository = EaUmlRepository.getRepository(eaRepository);
        }

        #region IUmlAttribute Members

        public int Id
        {
            get { return eaAttribute.AttributeID; }
        }

        public string Name
        {
            get { return eaAttribute.Name; }
        }
       	public string[] Stereotypes 
		{
			get 
			{
				return eaAttribute.StereotypeEx.Split(new []{","},StringSplitOptions.RemoveEmptyEntries);
			}
		}

        public string UpperBound
        {
            get { return eaAttribute.UpperBound; }
        }

        public string LowerBound
        {
            get { return eaAttribute.LowerBound; }
        }

		public int position 
		{
			get 
			{
				return eaAttribute.Pos;
			}
		}
        public IUmlClassifier Type
        {
        	get 
        	{ 
        		if (eaAttribute.ClassifierID > 0)
        			return new EaUmlClassifier(eaRepository, this.repository.GetElementByID(eaAttribute.ClassifierID)); 
        		return null;
        	}
        }

        public IEnumerable<IUmlAttribute> ReferencedAttributes(string tagName)
		{
			List<EaUmlAttribute> foundAttributes = new List<EaUmlAttribute>();
			foreach (var tagGUID in GetTaggedValue(tagName).Value
			         .Split(new [] {","},StringSplitOptions.RemoveEmptyEntries))
			{
				Guid attributeGUID;
				if( Guid.TryParse(tagGUID, out attributeGUID))
				{
					try
					{
						var refAttribute = eaRepository.GetAttributeByGuid(tagGUID);
						if (refAttribute != null)
							foundAttributes.Add(new EaUmlAttribute(eaRepository, refAttribute));
					}
					catch(Exception)
					{
						//do nothing if attribute found found.
					}
				}
			}
			return foundAttributes;
		}
        public IEnumerable<IUmlAssociation> ReferencedAssociations(string tagName)
        {
            var foundConnectors = new List<EaUmlAssociation>();
            foreach (var tagGUID in GetTaggedValue(tagName).Value
                     .Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries))
            {
                Guid connectorGUID;
                if (Guid.TryParse(tagGUID, out connectorGUID))
                {
                    try
                    {
                        var refConnector = eaRepository.GetConnectorByGuid(tagGUID);
                        if (refConnector != null)
                            if (refConnector.Type == EaConnectorTypes.Association.ToString() || refConnector.Type == EaConnectorTypes.Aggregation.ToString())
                            {
                                //determine the "owner"
                                int ownerID = refConnector.SupplierEnd.Aggregation != (int)EaAggregationKind.None ? refConnector.SupplierID : refConnector.ClientID;
                                foundConnectors.Add(new EaUmlAssociation(eaRepository, refConnector, ownerID));
                            }
                    }
                    catch (Exception)
                    {
                        //do nothing if connector not found
                    }
                }
            }
            return foundConnectors;
        }

        public IEnumerable<IUmlTaggedValue> GetTaggedValues()
        {
            foreach (AttributeTag eaAttributeTag in eaAttribute.TaggedValues)
            {
                yield return new EaAttributeTag(eaAttributeTag);
            }
        }

        public IUmlTaggedValue GetTaggedValue(string name)
        {
            try
            {
                var eaAttributeTag = eaAttribute.TaggedValues.GetByName(name) as AttributeTag;
                return eaAttributeTag == null ? (IUmlTaggedValue) new UndefinedTaggedValue(name) : new EaAttributeTag(eaAttributeTag);
            }
            catch (Exception)
            {
                return new UndefinedTaggedValue(name);
            }
        }

        private void CreateTaggedValue(UmlTaggedValueSpec taggedValueSpec)
        {
            var eaAttributeTag = (AttributeTag) eaAttribute.TaggedValues.AddNew(taggedValueSpec.Name, String.Empty);
            eaAttributeTag.Value = taggedValueSpec.Value ?? taggedValueSpec.DefaultValue;
            eaAttributeTag.Update();
        }

        #endregion

        public void Initialize(UmlAttributeSpec spec)
        {
            eaAttribute.Stereotype = spec.Stereotype;
            eaAttribute.ClassifierID = spec.Type.Id;
            eaAttribute.LowerBound = spec.LowerBound;
            eaAttribute.UpperBound = spec.UpperBound;
            eaAttribute.Update();
            foreach (var taggedValueSpec in spec.TaggedValues)
            {
                CreateTaggedValue(taggedValueSpec);
            }
        }

        public void Update(UmlAttributeSpec spec)
        {
            eaAttribute.Name = spec.Name;
            eaAttribute.Stereotype = spec.Stereotype;
            eaAttribute.ClassifierID = spec.Type.Id;
            eaAttribute.LowerBound = spec.LowerBound;
            eaAttribute.UpperBound = spec.UpperBound;
            eaAttribute.Update();
            foreach (var taggedValueSpec in spec.TaggedValues)
            {
                var taggedValue = GetTaggedValue(taggedValueSpec.Name);
                if (taggedValue.IsDefined)
                {
                    taggedValue.Update(taggedValueSpec);
                }
                else
                {
                    CreateTaggedValue(taggedValueSpec);
                }
            }
        }

        public bool Equals(EaUmlAttribute other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.eaAttribute.AttributeID, eaAttribute.AttributeID);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (EaUmlAttribute)) return false;
            return Equals((EaUmlAttribute) obj);
        }

        public override int GetHashCode()
        {
            return (eaAttribute != null ? eaAttribute.AttributeID : 0);
        }

        public static bool operator ==(EaUmlAttribute left, EaUmlAttribute right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(EaUmlAttribute left, EaUmlAttribute right)
        {
            return !Equals(left, right);
        }
    }
}