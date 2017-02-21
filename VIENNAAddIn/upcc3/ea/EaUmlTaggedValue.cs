using System;
using EA;
using VIENNAAddIn.upcc3.uml;

namespace VIENNAAddIn.upcc3.ea
{
    internal class UndefinedTaggedValue : IUmlTaggedValue
    {
        private static readonly string[] emptyStringArray = new string[0];

        public UndefinedTaggedValue(string name)
        {
            Name = name;
        }

        #region IUmlTaggedValue Members

        public bool IsDefined
        {
            get { return false; }
        }

        public string Name { get; private set; }

        public string Value
        {
            get { return string.Empty; }
        }

        public string[] SplitValues
        {
            get { return emptyStringArray; }
        }

        public void Update(UmlTaggedValueSpec spec)
        {
            // do nothing
        }

        #endregion
    }

    internal abstract class EaUmlTaggedValue : IUmlTaggedValue
    {
        #region IUmlTaggedValue Members

        public bool IsDefined
        {
            get { return true; }
        }

        public abstract string Name { get; }

        public abstract string Value { get; }

        public string[] SplitValues
        {
            get { return IsDefined && !string.IsNullOrEmpty(Value) ? Value.Split(MultiPartTaggedValue.ValueSeparator) : new string[0]; }
        }

        public abstract void Update(UmlTaggedValueSpec spec);

        #endregion
    }

    internal class EaTaggedValue : EaUmlTaggedValue
    {
        private readonly TaggedValue eaTaggedValue;

        public EaTaggedValue(TaggedValue eaTaggedValue)
        {
            if (eaTaggedValue == null)
            {
                throw new NullReferenceException("tagged value is null");
            }
            this.eaTaggedValue = eaTaggedValue;
        }

        public override string Name
        {
            get { return eaTaggedValue.Name; }
        }

        public override string Value
        {
            get { return eaTaggedValue.Value; }
        }

        public override void Update(UmlTaggedValueSpec spec)
        {
            eaTaggedValue.Name = spec.Name;
            if (spec.Value != null)
                eaTaggedValue.Value = spec.Value;
            eaTaggedValue.Update();
        }
    }

    internal class EaAttributeTag : EaUmlTaggedValue
    {
        private readonly AttributeTag eaAttributeTag;

        public EaAttributeTag(AttributeTag eaAttributeTag)
        {
            if (eaAttributeTag == null)
            {
                throw new NullReferenceException("attribute tag is null");
            }
            this.eaAttributeTag = eaAttributeTag;
        }

        public override string Name
        {
            get { return eaAttributeTag.Name; }
        }

        public override string Value
        {
            get { return eaAttributeTag.Value; }
        }

        public override void Update(UmlTaggedValueSpec spec)
        {
            eaAttributeTag.Name = spec.Name;
            if (spec.Value != null)
                eaAttributeTag.Value = spec.Value;
            eaAttributeTag.Update();
        }
    }

    internal class EaConnectorTag : EaUmlTaggedValue
    {
        private readonly ConnectorTag eaConnectorTag;

        public EaConnectorTag(ConnectorTag eaConnectorTag)
        {
            if (eaConnectorTag == null)
            {
                throw new NullReferenceException("connector tag is null");
            }
            this.eaConnectorTag = eaConnectorTag;
        }

        public override string Name
        {
            get { return eaConnectorTag.Name; }
        }

        public override string Value
        {
            get { return eaConnectorTag.Value; }
        }

        public override void Update(UmlTaggedValueSpec spec)
        {
            eaConnectorTag.Name = spec.Name;
            if (spec.Value != null)
                eaConnectorTag.Value = spec.Value;
            eaConnectorTag.Update();
        }
    }
}