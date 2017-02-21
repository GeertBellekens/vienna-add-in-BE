namespace Upcc
{
    public class MetaDependency
    {
        public MetaStereotype Stereotype { get; internal set; }
        public MetaClassifier SourceClassifierType { get; internal set; }
        public MetaClassifier TargetClassifierType { get; internal set; }
    }
}