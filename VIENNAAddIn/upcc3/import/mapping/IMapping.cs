namespace VIENNAAddIn.upcc3.import.mapping
{
    public interface IMapping
    {
        void TraverseDepthFirst(IMappingVisitor visitor);
        string BIEName { get; }
    }
}