using CctsRepository;

namespace VIENNAAddIn.upcc3.uml
{
    public interface IUmlAssociation
    {
        int Id { get; }
        string Name { get; }
        string UpperBound { get; }
        string LowerBound { get; }
        IUmlClassifier AssociatedClassifier { get; }
        AggregationKind AggregationKind { get; }
        IUmlTaggedValue GetTaggedValue(string name);
    }
}