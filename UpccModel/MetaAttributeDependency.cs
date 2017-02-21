namespace Upcc
{
    public class MetaAttributeDependency
    {
        public MetaStereotype Stereotype { get; internal set; }
        public MetaAttribute SourceAttribute { get; internal set; }
        public MetaAttribute TargetAttribute { get; internal set; }
    }
}