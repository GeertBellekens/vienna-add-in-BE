using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.import.mapping
{
    public abstract class AbstractMapping : IMapping
    {
        public void TraverseDepthFirst(IMappingVisitor visitor)
        {
            visitor.VisitBeforeChildren(this);
            foreach (var child in Children)
            {
                child.TraverseDepthFirst(visitor);
            }
            visitor.VisitAfterChildren(this);
        }

        public abstract string BIEName { get; }

        public abstract IEnumerable<ElementMapping> Children { get; }
    }
}