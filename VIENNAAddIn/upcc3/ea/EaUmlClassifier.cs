using System;
using System.Collections.Generic;
using System.Linq;
using EA;
using VIENNAAddIn.upcc3.uml;
using Attribute=EA.Attribute;

namespace VIENNAAddIn.upcc3.ea
{
    internal class EaUmlClassifier : IUmlClass, IUmlDataType, IUmlEnumeration, IEquatable<EaUmlClassifier>
    {
        private readonly Element eaElement;
        private readonly Repository eaRepository;

        public EaUmlClassifier(Repository eaRepository, Element eaElement)
        {
            this.eaRepository = eaRepository;
            this.eaElement = eaElement;
        }
		
        public bool isEnumeration
        {
        	get
        	{
        		return this.eaElement.Type == "Enumeration"
				||this.eaElement.Type == "Class"        			
        			&& this.Stereotypes.Contains("enumeration");
        	}
        }
        List<IUmlClassifier> _baseClassifiers;
		public IEnumerable<IUmlClassifier> BaseClassifiers 
		{
			get 
			{
				if (_baseClassifiers == null) _baseClassifiers = new List<IUmlClassifier>();
				foreach (var baseClass in this.eaElement.BaseClasses) 
				{
					_baseClassifiers.Add(new EaUmlClassifier(eaRepository,(EA.Element)baseClass));
				}
				return _baseClassifiers;
			}
		}
        #region IUmlClass Members

        public int Id
        {
            get { return eaElement.ElementID; }
        }

        public string GUID
        {
            get { return eaElement.ElementGUID; }
        }

        public string Name
        {
            get { return eaElement.Name; }
        }

        public IUmlPackage Package
        {
            get { return new EaUmlPackage(eaRepository, eaRepository.GetPackageByID(eaElement.PackageID)); }
        }

        public string Stereotype
        {
            get { return eaElement.Stereotype; }
        }

		public string[] Stereotypes 
		{
			get 
			{
				return eaElement.StereotypeEx.Split(new []{","},StringSplitOptions.RemoveEmptyEntries);
			}
		}
        public IUmlTaggedValue GetTaggedValue(string name)
        {
            try
            {
                var eaTaggedValue = eaElement.TaggedValues.GetByName(name) as TaggedValue;
                return eaTaggedValue == null ? (IUmlTaggedValue) new UndefinedTaggedValue(name) : new EaTaggedValue(eaTaggedValue);
            }
            catch (Exception)
            {
                return new UndefinedTaggedValue(name);
            }
        }
        List<IUmlDependency> _Traces;
		public IEnumerable<IUmlDependency> Traces 
		{
			get 
			{
				if(_Traces == null)
				{
					_Traces = new List<IUmlDependency>();
					foreach (Connector eaConnector in eaElement.Connectors)
		            {
		                if (eaConnector.Type == EaConnectorTypes.Abstraction.ToString())
		                {
		                	if (eaConnector.StereotypeEx.Split(',').Contains("trace"))
		                    {
		                		_Traces.Add( new EaUmlDependency(eaRepository, eaConnector));
		                    }
		                }
		            }
				}
				return _Traces;
			}
		}
        public IEnumerable<IUmlDependency> GetDependenciesByStereotype(string stereotype)
        {
            foreach (Connector eaConnector in eaElement.Connectors)
            {
                if (eaConnector.Type == EaConnectorTypes.Dependency.ToString())
                {
                    if (eaConnector.Stereotype == stereotype)
                    {
                        yield return new EaUmlDependency(eaRepository, eaConnector);
                    }
                }
            }
        }
        public IEnumerable<IUmlClass> GetClassesByStereotype(string stereotype)
        {
            foreach (Element element in eaElement.Elements)
            {
                if(element.Stereotype.Equals(stereotype))
                {
                    yield return new EaUmlClassifier(eaRepository, element);
                }
            }
        }

        public IUmlDependency GetFirstDependencyByStereotype(string stereotype)
        {
            var umlDependencies = new List<IUmlDependency>(GetDependenciesByStereotype(stereotype));
            return umlDependencies.Count == 0 ? null : umlDependencies[0];
        }

        public IUmlDependency CreateDependency(UmlDependencySpec spec)
        {
            var eaConnector = (Connector) eaElement.Connectors.AddNew(String.Empty, EaConnectorTypes.Dependency.ToString());
            eaConnector.ClientID = Id;
            eaConnector.Stereotype = spec.Stereotype;
            eaConnector.SupplierID = spec.Target.Id;
            eaConnector.SupplierEnd.Role = spec.Stereotype;
            eaConnector.SupplierEnd.Cardinality = new EaCardinality(spec.LowerBound, spec.UpperBound).ToString();
            eaConnector.Update();
            return new EaUmlDependency(eaRepository, eaConnector);
        }
		
        List<IUmlAttribute> _attributes;
		public IEnumerable<IUmlAttribute> Attributes 
		{
			get
			{
				if(_attributes == null)
				{
					_attributes = new List<IUmlAttribute>();
					foreach (Attribute eaAttribute in eaElement.Attributes)
		            {
						//check if attribute is literal value
						if (EaUmlEnumerationLiteral.isLiteralValue(eaAttribute,this))
							_attributes.Add(new EaUmlEnumerationLiteral(eaRepository, eaAttribute,this));
						else
							_attributes.Add(new EaUmlAttribute(eaRepository, eaAttribute));
		            }
				}
				return _attributes;
			}
		}

        public IEnumerable<IUmlAttribute> GetAttributesByStereotype(string stereotype)
        {
        	return this.Attributes.Where(x => x.Stereotypes.Contains(stereotype));
        }

        public IUmlAttribute GetFirstAttributeByStereotype(string stereotype)
        {
        	return this.Attributes.FirstOrDefault(x => x.Stereotypes.Contains(stereotype));
        }

        public IUmlAttribute CreateAttribute(UmlAttributeSpec spec)
        {
            var attribute = new EaUmlAttribute(eaRepository, (Attribute) eaElement.Attributes.AddNew(spec.Name, spec.Type.Name));
            attribute.Initialize(spec);
            return attribute;
        }

        public IUmlAttribute UpdateAttribute(IUmlAttribute attribute, UmlAttributeSpec spec)
        {
            ((EaUmlAttribute) attribute).Update(spec);
            return attribute;
        }

        public void RemoveAttribute(IUmlAttribute attribute)
        {
            short i = 0;
            Collection eaAttributes = eaElement.Attributes;
            foreach (Attribute eaAttribute in eaAttributes)
            {
                if (eaAttribute.AttributeID == attribute.Id)
                {
                    eaAttributes.Delete(i);
                }
                i++;
            }
            eaAttributes.Refresh();
        }
        List<IUmlAssociation> _associations;
		public IEnumerable<IUmlAssociation> Associations 
		{
			get 
			{
				if (_associations == null)
				{
					_associations = new List<IUmlAssociation>();
					foreach (Connector eaConnector in eaElement.Connectors)
		            {
		                if (eaConnector.Type == EaConnectorTypes.Association.ToString() || eaConnector.Type == EaConnectorTypes.Aggregation.ToString())
		                {
	                        if ((eaConnector.ClientID == Id && eaConnector.ClientEnd.Aggregation != (int) EaAggregationKind.None) ||
	                            (eaConnector.SupplierID == Id && eaConnector.SupplierEnd.Aggregation != (int) EaAggregationKind.None))
	                        {
		                		_associations.Add(new EaUmlAssociation(eaRepository, eaConnector, Id));
	                        }
		                }
		            }
				}
				return _associations;
			}
		}
        public IEnumerable<IUmlAssociation> GetAssociationsByStereotype(string stereotype)
        {
        	return _associations.Where( x => x.Stereotypes.Contains(stereotype));
        }

        public IUmlAssociation CreateAssociation(UmlAssociationSpec spec)
        {
            Connector eaConnector = (Connector) eaElement.Connectors.AddNew(string.Empty, EaConnectorTypes.Aggregation.ToString());
            var association = new EaUmlAssociation(eaRepository, eaConnector, Id);
            association.Initialize(spec);
            return association;
        }

        public IUmlAssociation UpdateAssociation(IUmlAssociation association, UmlAssociationSpec spec)
        {
            ((EaUmlAssociation) association).Update(spec);
            return association;
        }

        public void RemoveAssociation(IUmlAssociation association)
        {
            short i = 0;
            Collection eaConnectors = eaElement.Connectors;
            foreach (Connector eaConnector in eaConnectors)
            {
                if (eaConnector.ConnectorID == association.Id)
                {
                    eaConnectors.Delete(i);
                }
                i++;
            }
            eaConnectors.Refresh();
        }

        #endregion

        #region IUmlEnumeration Members

        public IEnumerable<IUmlEnumerationLiteral> GetEnumerationLiteralsByStereotype(string stereotype)
        {
            foreach (Attribute eaAttribute in eaElement.Attributes)
            {
                if (eaAttribute.Stereotype == stereotype)
                {
                    yield return new EaUmlEnumerationLiteral(eaRepository, eaAttribute,this);
                }
            }
        }

        public IUmlEnumerationLiteral CreateEnumerationLiteral(UmlEnumerationLiteralSpec spec)
        {
            var enumerationLiteral = new EaUmlEnumerationLiteral(eaRepository,(Attribute) eaElement.Attributes.AddNew(spec.Name, "String"),this);
            enumerationLiteral.Initialize(spec);
            return enumerationLiteral;
        }

        public IUmlEnumerationLiteral UpdateEnumerationLiteral(IUmlEnumerationLiteral enumerationLiteral, UmlEnumerationLiteralSpec spec)
        {
            ((EaUmlEnumerationLiteral) enumerationLiteral).Update(spec);
            return enumerationLiteral;
        }

        public void RemoveEnumerationLiteral(IUmlEnumerationLiteral enumerationLiteral)
        {
            short i = 0;
            Collection eaAttributes = eaElement.Attributes;
            foreach (Attribute eaAttribute in eaAttributes)
            {
                if (eaAttribute.AttributeID == enumerationLiteral.Id)
                {
                    eaAttributes.Delete(i);
                }
                i++;
            }
            eaAttributes.Refresh();
        }

        #endregion

        private void CreateTaggedValue(UmlTaggedValueSpec taggedValueSpec)
        {
            var eaTaggedValue = (TaggedValue) eaElement.TaggedValues.AddNew(taggedValueSpec.Name, String.Empty);
            eaTaggedValue.Value = taggedValueSpec.Value ?? taggedValueSpec.DefaultValue;
            eaTaggedValue.Update();
        }

        public void Update(UmlClassifierSpec spec)
        {
            eaElement.Name = spec.Name;
            eaElement.Stereotype = spec.Stereotype;
            eaElement.Update();

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
            eaElement.TaggedValues.Refresh();

            for (var i = (short) (eaElement.Attributes.Count - 1); i >= 0; i--)
            {
                eaElement.Attributes.Delete(i);
            }
            eaElement.Attributes.Refresh();
            if (spec.Attributes != null)
            {
                foreach (UmlAttributeSpec attributeSpec in spec.Attributes)
                {
                    CreateAttribute(attributeSpec);
                }
            }
            if (spec is UmlEnumerationSpec)
            {
                var enumSpec = (UmlEnumerationSpec) spec;
                if (enumSpec.EnumerationLiterals != null)
                {
                    foreach (UmlEnumerationLiteralSpec enumerationLiteralSpec in enumSpec.EnumerationLiterals)
                    {
                        CreateEnumerationLiteral(enumerationLiteralSpec);
                    }
                }
            }
            eaElement.Attributes.Refresh();

            for (var i = (short) (eaElement.Connectors.Count - 1); i >= 0; i--)
            {
                if (DeleteConnectorOnUpdate((Connector) eaElement.Connectors.GetAt(i)))
                {
                    eaElement.Connectors.Delete(i);
                }
            }
            if (spec.Associations != null)
            {
                foreach (UmlAssociationSpec associationSpec in spec.Associations)
                {
                    CreateAssociation(associationSpec);
                }
            }
            if (spec.Dependencies != null)
            {
                foreach (UmlDependencySpec dependencySpec in spec.Dependencies)
                {
                    CreateDependency(dependencySpec);
                }
            }
            eaElement.Connectors.Refresh();
        }

        private bool DeleteConnectorOnUpdate(Connector eaConnector)
        {
            if (eaConnector.Type == EaConnectorTypes.Dependency.ToString())
            {
                if (eaConnector.ClientID == Id)
                {
                    return true;
                }
            }
            if (eaConnector.Type == EaConnectorTypes.Aggregation.ToString())
            {
                if (eaConnector.ClientID == Id)
                {
                    return eaConnector.ClientEnd.Aggregation != (int) EaAggregationKind.None;
                }
                if (eaConnector.SupplierID == Id)
                {
                    return eaConnector.SupplierEnd.Aggregation != (int) EaAggregationKind.None;
                }
            }
            return false;
        }

        public void Initialize(UmlClassifierSpec spec)
        {
            eaElement.Stereotype = spec.Stereotype;
            eaElement.Update();

            if (spec.TaggedValues != null)
            {
                foreach (UmlTaggedValueSpec taggedValueSpec in spec.TaggedValues)
                {
                    CreateTaggedValue(taggedValueSpec);
                }
                eaElement.TaggedValues.Refresh();
            }

            if (spec.Attributes != null)
            {
                foreach (UmlAttributeSpec attributeSpec in spec.Attributes)
                {
                    CreateAttribute(attributeSpec);
                }
            }
            if (spec is UmlEnumerationSpec)
            {
                var enumSpec = (UmlEnumerationSpec)spec;
                if (enumSpec.EnumerationLiterals != null)
                {
                    foreach (UmlEnumerationLiteralSpec enumerationLiteralSpec in enumSpec.EnumerationLiterals)
                    {
                        CreateEnumerationLiteral(enumerationLiteralSpec);
                    }
                }
            }
            eaElement.Attributes.Refresh();

            if (spec.Associations != null)
            {
                foreach (UmlAssociationSpec associationSpec in spec.Associations)
                {
                    CreateAssociation(associationSpec);
                }
            }
            if (spec.Dependencies != null)
            {
                foreach (UmlDependencySpec dependencySpec in spec.Dependencies)
                {
                    CreateDependency(dependencySpec);
                }
            }
            eaElement.Connectors.Refresh();
        }

        public bool Equals(EaUmlClassifier other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.eaElement.ElementID, eaElement.ElementID);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (EaUmlClassifier)) return false;
            return Equals((EaUmlClassifier) obj);
        }

        public override int GetHashCode()
        {
            return (eaElement != null ? eaElement.ElementID : 0);
        }

        public static bool operator ==(EaUmlClassifier left, EaUmlClassifier right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(EaUmlClassifier left, EaUmlClassifier right)
        {
            return !Equals(left, right);
        }
    }
}