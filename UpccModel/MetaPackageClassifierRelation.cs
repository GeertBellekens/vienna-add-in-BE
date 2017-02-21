namespace Upcc
{
    public class MetaPackageClassifierRelation
    {
        public MetaPackage PackageType { get; internal set; }
        public string PackageRole { get; internal set; }

        public MetaCardinality ClassifierCardinality { get; internal set; }
        public MetaClassifier ClassifierType { get; internal set; }
        public string ClassifierRole { get; internal set; }
    }
}