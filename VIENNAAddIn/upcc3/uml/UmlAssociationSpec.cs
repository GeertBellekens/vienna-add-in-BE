using System;
using System.Collections.Generic;
using CctsRepository;

namespace VIENNAAddIn.upcc3.uml
{
    public class UmlAssociationSpec
    {
        public string Stereotype { get; set; }
        public string Name { get; set; }
        public string UpperBound { get; set; }
        public string LowerBound { get; set; }
        public AggregationKind AggregationKind { get; set; }
        public IEnumerable<UmlTaggedValueSpec> TaggedValues { get; set; }
        public IUmlClassifier AssociatingClassifier { get; set; }
        public IUmlClassifier AssociatedClassifier { get; set; }
    }
}