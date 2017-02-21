using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.uml
{
    public class UmlPackageSpec
    {
        public string Stereotype { get; set; }
        public string Name { get; set; }
        public IEnumerable<UmlTaggedValueSpec> TaggedValues { get; set; }
        public UmlDiagramType DiagramType { get; set; }
    }

    public enum UmlDiagramType
    {
        Class,
        Package,
    }
}