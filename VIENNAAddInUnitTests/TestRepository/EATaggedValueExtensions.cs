using EA;

namespace VIENNAAddInUnitTests.TestRepository
{
    public static class EATaggedValueExtensions
    {
        public static TaggedValue WithValue(this TaggedValue taggedValue, string value)
        {
            taggedValue.Value = value;
            taggedValue.Update();
            return taggedValue;
        }
    }

    public static class EAAttributeTagExtensions
    {
        public static AttributeTag WithValue(this AttributeTag taggedValue, string value)
        {
            taggedValue.Value = value;
            taggedValue.Update();
            return taggedValue;
        }
    }

    public static class EAConnectorTagExtensions
    {
        public static ConnectorTag WithValue(this ConnectorTag taggedValue, string value)
        {
            taggedValue.Value = value;
            taggedValue.Update();
            return taggedValue;
        }
    }
}