using CctsRepository;
using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.uml
{
    public interface IUmlAssociation
    {
        int Id { get; }
        string Name { get; }
        string UpperBound { get; }
        string LowerBound { get; }
        string [] Stereotypes {get;}
        IUmlClassifier AssociatedClassifier { get; }
        AggregationKind AggregationKind { get; }
        IUmlTaggedValue GetTaggedValue(string name);
        IEnumerable<IUmlAssociation> ReferencedAssociations(string tagName);
        IEnumerable<IUmlAttribute> ReferencedAttributes(string tagName);
    }
}