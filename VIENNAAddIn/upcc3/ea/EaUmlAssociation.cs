using System;
using System.Collections.Generic;
using CctsRepository;
using EA;
using VIENNAAddIn.upcc3.uml;

namespace VIENNAAddIn.upcc3.ea
{
    internal class EaUmlAssociation : IUmlAssociation, IEquatable<EaUmlAssociation>
    {
        private readonly int associatingElementId;
        private readonly Connector eaConnector;
        private readonly Repository eaRepository;

        public EaUmlAssociation(Repository eaRepository, Connector eaConnector, int associatingElementId)
        {
            this.eaRepository = eaRepository;
            this.eaConnector = eaConnector;
            this.associatingElementId = associatingElementId;
        }

        private ConnectorEnd AssociatingConnectorEnd
        {
            get { return eaConnector.ClientID == associatingElementId ? eaConnector.ClientEnd : eaConnector.SupplierEnd; }
        }

        private ConnectorEnd AssociatedConnectorEnd
        {
            get { return eaConnector.ClientID == associatingElementId ? eaConnector.SupplierEnd : eaConnector.ClientEnd; }
        }

        private int AssociatedElementId
        {
            get { return eaConnector.ClientID == associatingElementId ? eaConnector.SupplierID : eaConnector.ClientID; }
        }

        private EaCardinality Cardinality
        {
            get { return new EaCardinality(AssociatedConnectorEnd.Cardinality); }
        }

        #region IUmlAssociation Members

        public int Id
        {
            get { return eaConnector.ConnectorID; }
        }

        public string Name
        {
            get { return AssociatedConnectorEnd.Role; }
        }

        public string UpperBound
        {
            get { return Cardinality.UpperBound; }
        }

        public string LowerBound
        {
            get { return Cardinality.LowerBound; }
        }

        public string[] Stereotypes
        {
            get
            {
                return eaConnector.StereotypeEx.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            }
        }
        public IUmlClassifier AssociatedClassifier
        {
            get { return new EaUmlClassifier(eaRepository, EaUmlRepository.getRepository(eaRepository).GetElementByID(AssociatedElementId)); }
        }

        public AggregationKind AggregationKind
        {
            get
            {
                int value = AssociatingConnectorEnd.Aggregation;
                if (Enum.IsDefined(typeof(AggregationKind), value))
                {
                    return (AggregationKind)Enum.ToObject(typeof(AggregationKind), value);
                }
                return AggregationKind.Composite;
            }
        }

        public IUmlTaggedValue GetTaggedValue(string name)
        {
            try
            {
                var eaConnectorTag = eaConnector.TaggedValues.GetByName(name) as ConnectorTag;
                return eaConnectorTag == null ? (IUmlTaggedValue)new UndefinedTaggedValue(name) : new EaConnectorTag(eaConnectorTag);
            }
            catch (Exception)
            {
                return new UndefinedTaggedValue(name);
            }
        }

        #endregion

        private void CreateTaggedValue(UmlTaggedValueSpec taggedValueSpec)
        {
            var eaConnectorTag = (ConnectorTag)eaConnector.TaggedValues.AddNew(taggedValueSpec.Name, String.Empty);
            eaConnectorTag.Value = taggedValueSpec.Value ?? taggedValueSpec.DefaultValue;
            eaConnectorTag.Update();
        }

        public void Initialize(UmlAssociationSpec spec)
        {
            eaConnector.Stereotype = spec.Stereotype;
            eaConnector.ClientID = associatingElementId;
            eaConnector.ClientEnd.Aggregation = (int)spec.AggregationKind;
            eaConnector.SupplierID = spec.AssociatedClassifier.Id;
            eaConnector.SupplierEnd.Role = spec.Name;
            eaConnector.SupplierEnd.Cardinality = new EaCardinality(spec.LowerBound, spec.UpperBound).ToString();
            eaConnector.Update();

            if (spec.TaggedValues != null)
            {
                foreach (UmlTaggedValueSpec taggedValueSpec in spec.TaggedValues)
                {
                    CreateTaggedValue(taggedValueSpec);
                }
            }
        }

        public void Update(UmlAssociationSpec spec)
        {
            eaConnector.Stereotype = spec.Stereotype;
            eaConnector.ClientID = associatingElementId;
            eaConnector.ClientEnd.Aggregation = (int)spec.AggregationKind;
            eaConnector.SupplierID = spec.AssociatedClassifier.Id;
            eaConnector.SupplierEnd.Role = spec.Name;
            eaConnector.SupplierEnd.Cardinality = new EaCardinality(spec.LowerBound, spec.UpperBound).ToString();
            eaConnector.Update();
            foreach (UmlTaggedValueSpec taggedValueSpec in spec.TaggedValues)
            {
                IUmlTaggedValue taggedValue = GetTaggedValue(taggedValueSpec.Name);
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

        public bool Equals(EaUmlAssociation other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.eaConnector.ConnectorID == eaConnector.ConnectorID;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(EaUmlAssociation)) return false;
            return Equals((EaUmlAssociation)obj);
        }

        public override int GetHashCode()
        {
            return (eaConnector != null ? eaConnector.ConnectorID : 0);
        }

        public IEnumerable<IUmlAttribute> ReferencedAttributes(string tagName)
        {
            List<EaUmlAttribute> foundAttributes = new List<EaUmlAttribute>();
            foreach (var tagGUID in GetTaggedValue(tagName).Value
                     .Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries))
            {
                Guid attributeGUID;
                if (Guid.TryParse(tagGUID, out attributeGUID))
                {
                    try
                    {
                        var refAttribute = eaRepository.GetAttributeByGuid(tagGUID);
                        if (refAttribute != null)
                            foundAttributes.Add(new EaUmlAttribute(eaRepository, refAttribute));
                    }
                    catch (Exception)
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

        public static bool operator ==(EaUmlAssociation left, EaUmlAssociation right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(EaUmlAssociation left, EaUmlAssociation right)
        {
            return !Equals(left, right);
        }
    }
}