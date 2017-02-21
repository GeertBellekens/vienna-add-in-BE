using System;
using System.Linq;
using CctsRepository.CcLibrary;

namespace VIENNAAddIn.upcc3.import.mapping
{
    public class ComplexElementToAsccMapping : ElementMapping, IEquatable<ComplexElementToAsccMapping>
    {
        public ComplexElementToAsccMapping(SourceItem sourceElement, IAscc targetAscc)
            : base(sourceElement)
        {
            Ascc = targetAscc;
            Acc = Ascc.AssociatingAcc;
        }

        public override string ToString()
        {
            return string.Format("ComplexElementToAsccMapping <SourceItem: {0}, ACC: {1} [{2}]>", SourceItem.Name, Acc.Name, Acc.Id);
        }

        public override string BIEName
        {
            get { return SourceItem.Name + "_" + Ascc.Name; }
        }

        public IAcc Acc { get; private set; }

        public IAscc Ascc { get; private set; }

        public ComplexTypeMapping TargetMapping { get; set; }

        public string ElementName
        {
            get { return SourceItem.Name; }
        }

        public override bool ResolveTypeMapping(SchemaMapping schemaMapping)
        {
            ComplexTypeMapping complexTypeMapping = schemaMapping.GetComplexTypeMapping(SourceItem.XsdType);
            if (!complexTypeMapping.IsMappedToSingleACC)
            {
                throw new MappingError("Complex typed element '" + SourceItem.Path +
                                       "' mapped to ASCC, but the complex type is not mapped to a single ACC: TargetACCs: [" + string.Join(", ", complexTypeMapping.TargetACCs.Select(acc => acc.Name).ToArray()) + "], number of children mapped to ASMAs: " + complexTypeMapping.AsmaMappings.Count() + ".");
            }
            IAcc complexTypeACC = complexTypeMapping.TargetACCs.ElementAt(0);
            if (complexTypeACC.Id != Ascc.AssociatedAcc.Id)
            {
                throw new MappingError("Complex typed element '" + SourceItem.Path +
                                       "' mapped to ASCC with associated ACC other than the target ACC for the complex type.");
            }
            TargetMapping = complexTypeMapping;
            return true;
        }

        public bool Equals(ComplexElementToAsccMapping other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.SourceItem.Name, SourceItem.Name) && Equals(other.Ascc.Id, Ascc.Id) && Equals(other.Acc.Id, Acc.Id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (ComplexElementToAsccMapping)) return false;
            return Equals((ComplexElementToAsccMapping) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = (SourceItem != null ? SourceItem.GetHashCode() : 0);
                result = (result*397) ^ (Ascc != null ? Ascc.GetHashCode() : 0);
                result = (result*397) ^ (Acc != null ? Acc.GetHashCode() : 0);
                return result;
            }
        }

        public static bool operator ==(ComplexElementToAsccMapping left, ComplexElementToAsccMapping right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ComplexElementToAsccMapping left, ComplexElementToAsccMapping right)
        {
            return !Equals(left, right);
        }
   }
}