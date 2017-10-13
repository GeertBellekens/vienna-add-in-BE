using System;
using System.Collections.Generic;
using EA;
using VIENNAAddIn.upcc3.uml;
using Attribute=EA.Attribute;

namespace VIENNAAddIn.upcc3.ea
{
    internal class EaUmlEnumerationLiteral : IUmlEnumerationLiteral, IEquatable<EaUmlEnumerationLiteral>
    {
        private readonly Attribute eaAttribute;
        private readonly Repository eaRepository;
        public IUmlClassifier Owner {get;private set;}

        public EaUmlEnumerationLiteral(Repository eaRepository, Attribute eaAttribute, IUmlClassifier owner)
        {
            this.eaAttribute = eaAttribute;
            this.eaRepository = eaRepository;
            this.Owner = owner;
        }
		
       	public static bool isLiteralValue(Attribute eaAttribute, EaUmlClassifier owner)
		{
			//if the field StyleEx contains "IsLiteral=1" then it is a literal value
			//of if the StyleEx field is empty and the owner of this attribute is of type Enumeration
			var styleEx = eaAttribute.StyleEx;
			if (styleEx.Contains("IsLiteral=1")) return true;
			if (styleEx.Contains("IsLiteral=0")) return false;
			//no information in the StyleEx, check the owner
			return owner.isEnumeration;
		}
        #region IUmlEnumerationLiteral Members

        public int Id
        {
            get { return eaAttribute.AttributeID; }
        }

        public string Name
        {
            get { return eaAttribute.Name; }
        }

		public int position 
		{
			get 
			{
				return eaAttribute.Pos;
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
       	public string[] Stereotypes 
		{
			get 
			{
				return eaAttribute.StereotypeEx.Split(new []{","},StringSplitOptions.RemoveEmptyEntries);
			}
		}

		public IEnumerable<IUmlAttribute> ReferencedAttributes 
		{
			get 
			{
				return new List<EaUmlAttribute>();
			}
		}

        public IEnumerable<IUmlTaggedValue> GetTaggedValues()
        {
            foreach (AttributeTag eaAttributeTag in eaAttribute.TaggedValues)
            {
                yield return new EaAttributeTag(eaAttributeTag);
            }
        }
		public string UpperBound 
		{
			get {return string.Empty;}
		}
		public string LowerBound 
		{
			get {return string.Empty;}
		}
		public IUmlClassifier Type 		
		{
			get {return null;}
		}
        #endregion

        public void Initialize(UmlEnumerationLiteralSpec spec)
        {
            eaAttribute.Stereotype = spec.Stereotype;
            foreach (UmlTaggedValueSpec taggedValueSpec in spec.TaggedValues)
            {
                CreateTaggedValue(taggedValueSpec);
            }
        }

        private void CreateTaggedValue(UmlTaggedValueSpec taggedValueSpec)
        {
            var eaAttributeTag = (AttributeTag) eaAttribute.TaggedValues.AddNew(taggedValueSpec.Name, String.Empty);
            eaAttributeTag.Value = taggedValueSpec.Value ?? taggedValueSpec.DefaultValue;
            eaAttributeTag.Update();
        }

        public void Update(UmlEnumerationLiteralSpec spec)
        {
            eaAttribute.Name = spec.Name;
            eaAttribute.Stereotype = spec.Stereotype;
            eaAttribute.Update();
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

        public bool Equals(EaUmlEnumerationLiteral other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.eaAttribute.AttributeID, eaAttribute.AttributeID);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (EaUmlEnumerationLiteral)) return false;
            return Equals((EaUmlEnumerationLiteral) obj);
        }

        public override int GetHashCode()
        {
            return (eaAttribute != null ? eaAttribute.AttributeID : 0);
        }

        public static bool operator ==(EaUmlEnumerationLiteral left, EaUmlEnumerationLiteral right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(EaUmlEnumerationLiteral left, EaUmlEnumerationLiteral right)
        {
            return !Equals(left, right);
        }
    }
}