namespace VIENNAAddIn.upcc3.import.mapping
{
    public interface IMappingVisitor
    {
        void VisitBeforeChildren(IMapping mapping);
        void VisitAfterChildren(IMapping mapping);
    }
}