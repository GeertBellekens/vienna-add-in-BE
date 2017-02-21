namespace Upcc
{
    public class MetaAttribute
    {
        public MetaStereotype Stereotype { get; internal set; }

        public MetaClass ContainingClassifierType { get; internal set; }

        public MetaCardinality Cardinality { get; internal set; }
        public string ClassName { get; internal set; }
        public string AttributeName { get; internal set; }
        public MetaClassifier Type { get; internal set; }

        public MetaTaggedValue[] TaggedValues { get; internal set; }

        public bool HasTaggedValues
        {
            get
            {
                return TaggedValues != null && TaggedValues.Length > 0;
            }
        }
    }
}