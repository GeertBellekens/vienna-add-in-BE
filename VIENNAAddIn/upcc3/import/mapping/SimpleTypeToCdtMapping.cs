using System;
using System.Collections.Generic;
using CctsRepository.CdtLibrary;

namespace VIENNAAddIn.upcc3.import.mapping
{
    public class SimpleTypeToCdtMapping : AbstractMapping
    {
        public SimpleTypeToCdtMapping(string simpleTypeName, ICdt targetCdt)
        {
            SimpleTypeName = simpleTypeName;
            TargetCDT = targetCdt;
        }

        public ICdt TargetCDT { get; private set; }

        public string SimpleTypeName { get; private set; }

        public override string ToString()
        {
            return string.Format("SimpleTypeToCdtMapping <SimpleType: {0}>", SimpleTypeName);
        }

        public bool Equals(SimpleTypeToCdtMapping other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.TargetCDT.Id, TargetCDT.Id) && Equals(other.SimpleTypeName, SimpleTypeName);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (SimpleTypeToCdtMapping)) return false;
            return Equals((SimpleTypeToCdtMapping) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((TargetCDT != null ? TargetCDT.GetHashCode() : 0)*397) ^ (SimpleTypeName != null ? SimpleTypeName.GetHashCode() : 0);
            }
        }

        public override string BIEName
        {
            get { return SimpleTypeName + "_" + TargetCDT.Name; }
        }

        public override IEnumerable<ElementMapping> Children
        {
            get { yield break; }
        }
    } 
}