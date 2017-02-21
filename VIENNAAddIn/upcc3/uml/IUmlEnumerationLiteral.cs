namespace VIENNAAddIn.upcc3.uml
{
    public interface IUmlEnumerationLiteral
    {
        int Id { get; }
        string Name { get; }
        IUmlTaggedValue GetTaggedValue(string name);
    }
}