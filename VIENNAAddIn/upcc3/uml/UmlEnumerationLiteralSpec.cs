using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.uml
{
    public class UmlEnumerationLiteralSpec
    {
        public string Stereotype { get; set; }
        public string Name { get; set; }
        public IEnumerable<UmlTaggedValueSpec> TaggedValues { get; set; }
    }
}