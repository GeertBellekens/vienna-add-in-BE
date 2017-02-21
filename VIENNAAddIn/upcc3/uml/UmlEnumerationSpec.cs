using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.uml
{
    public class UmlEnumerationSpec : UmlDataTypeSpec
    {
        public IEnumerable<UmlEnumerationLiteralSpec> EnumerationLiterals { get; set; }
    }
}