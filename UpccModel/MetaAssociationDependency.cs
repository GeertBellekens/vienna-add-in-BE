namespace Upcc
{
    public class MetaAssociationDependency
    {
        public MetaStereotype Stereotype { get; internal set; }
        public MetaAssociation SourceAssociation { get; internal set; }
        public MetaAssociation TargetAssociation { get; internal set; }
    }
}