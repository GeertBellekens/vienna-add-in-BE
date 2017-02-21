using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.uml
{
    public abstract class UmlClassifierSpec
    {
        public string Stereotype { get; set; }
        public string Name { get; set; }
        public IEnumerable<UmlDependencySpec> Dependencies { get; set; }
        public IEnumerable<UmlTaggedValueSpec> TaggedValues { get; set; }
        public IEnumerable<UmlAttributeSpec> Attributes { get; set; }
        public IEnumerable<UmlAssociationSpec> Associations { get; set; }
    }
}