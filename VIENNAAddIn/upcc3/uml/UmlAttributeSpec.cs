using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.uml
{
    public class UmlAttributeSpec
    {
        public string Stereotype { get; set; }
        public string Name { get; set; }
        public IUmlClassifier Type { get; set; }
        public string UpperBound { get; set; }
        public string LowerBound { get; set; }
        public IEnumerable<UmlTaggedValueSpec> TaggedValues { get; set; }
    }
}