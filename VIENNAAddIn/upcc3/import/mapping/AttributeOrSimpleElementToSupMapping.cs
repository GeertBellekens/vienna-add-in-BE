using System;
using CctsRepository.CdtLibrary;

namespace VIENNAAddIn.upcc3.import.mapping
{
    public class AttributeOrSimpleElementToSupMapping : ElementMapping, IEquatable<AttributeOrSimpleElementToSupMapping>
    {
        public AttributeOrSimpleElementToSupMapping(SourceItem sourceElement, ICdtSup targetSup)
            : base(sourceElement)
        {
            Sup = targetSup;
            Cdt = Sup.Cdt;
        }
        
        public string ElementName
        {
            get { return SourceItem.Name; }
        }

        public override string BIEName
        {
            get { return ElementName + "_" + Sup.Name; }
        }

        public ICdtSup Sup { get; private set; }

        public override string ToString()
        {
            return string.Format("SUPMapping <SourceItem: {0}, CDT: {1} [{2}]>", SourceItem.Name, Cdt.Name, Cdt.Id);
        }

        public ICdt Cdt { get; private set; }

        public bool Equals(AttributeOrSimpleElementToSupMapping other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.SourceItem.Name, SourceItem.Name) && Equals(other.Sup.Id, Sup.Id) && Equals(other.Cdt.Id, Cdt.Id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (AttributeOrSimpleElementToSupMapping)) return false;
            return Equals((AttributeOrSimpleElementToSupMapping) obj);
        }

        public override int GetHashCode()
        {
            return (Cdt != null ? Cdt.GetHashCode() : 0);
        }

        public static bool operator ==(AttributeOrSimpleElementToSupMapping left, AttributeOrSimpleElementToSupMapping right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(AttributeOrSimpleElementToSupMapping left, AttributeOrSimpleElementToSupMapping right)
        {
            return !Equals(left, right);
        }

        public override bool ResolveTypeMapping(SchemaMapping schemaMapping)
        {
            // do nothing
            return true;
        }
    }
}