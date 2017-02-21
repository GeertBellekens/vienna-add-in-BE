namespace Upcc
{
    public class MetaSubPackageRelation
    {
        public MetaCardinality ParentPackageCardinality { get; internal set; }
        public MetaPackage ParentPackageType { get; internal set; }
        public string ParentPackageRole { get; internal set; }

        public MetaPackage SubPackageType { get; internal set; }
        public string SubPackageRole { get; internal set; }
    }
}