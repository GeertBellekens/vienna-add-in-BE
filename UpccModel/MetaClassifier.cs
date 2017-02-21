namespace Upcc
{
    public abstract class MetaClassifier
    {
        public string Name { get; internal set; }
        public MetaStereotype Stereotype { get; internal set; }
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