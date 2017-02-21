using System;
using System.Collections.Generic;
using System.Linq;

namespace VIENNAAddIn.upcc3.import.mapping
{
    public class ComplexTypeToAccMapping : ComplexTypeMapping, IEquatable<ComplexTypeToAccMapping>
    {
        public ComplexTypeToAccMapping(string sourceElementName, string complexTypeName, IEnumerable<ElementMapping> childMappings) : base(sourceElementName,complexTypeName, childMappings)
        {            
        }

        public override string BIEName
        {
            get { return ComplexTypeName + "_" + TargetACCs.First().Name; }
        }

        public override string ToString()
        {
            return string.Format("ComplexTypeToAccMapping <ComplexType: {0}>", ComplexTypeName);
        }

        public bool Equals(ComplexTypeToAccMapping other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            bool b = ChildrenEqual(other);
            bool b1 = Equals(other.ComplexTypeName, ComplexTypeName);
            return b && b1;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(ComplexTypeToAccMapping)) return false;
            return Equals((ComplexTypeToAccMapping)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ComplexTypeName != null ? ComplexTypeName.GetHashCode() : 0;
            }
        }

        public static bool operator ==(ComplexTypeToAccMapping left, ComplexTypeToAccMapping right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ComplexTypeToAccMapping left, ComplexTypeToAccMapping right)
        {
            return !Equals(left, right);
        }
    }
}