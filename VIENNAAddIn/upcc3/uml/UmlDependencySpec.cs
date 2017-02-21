namespace VIENNAAddIn.upcc3.uml
{
    public class UmlDependencySpec
    {
        public string Stereotype { get; set; }
        public IUmlClassifier Target { get; set; }
        public string LowerBound { get; set; }
        public string UpperBound { get; set; }
     }
}