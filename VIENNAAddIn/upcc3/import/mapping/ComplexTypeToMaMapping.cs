using System;
using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.import.mapping
{
    public class ComplexTypeToMaMapping : ComplexTypeMapping, IEquatable<ComplexTypeToMaMapping>
    {
        public ComplexTypeToMaMapping(string sourceElementName, string complexTypeName, IEnumerable<ElementMapping> childMappings) : base(sourceElementName, complexTypeName, childMappings)
        {
        }

        public override string BIEName
        {
            get { return ComplexTypeName; }
        }

        public override string ToString()
        {
            return string.Format("ComplexTypeToMaMapping <ComplexType: {0}>", ComplexTypeName);
        }

        public bool Equals(ComplexTypeToMaMapping other)
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
            if (obj.GetType() != typeof(ComplexTypeToMaMapping)) return false;
            return Equals((ComplexTypeToMaMapping)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ComplexTypeName != null ? ComplexTypeName.GetHashCode() : 0;
            }
        }

        public static bool operator ==(ComplexTypeToMaMapping left, ComplexTypeToMaMapping right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ComplexTypeToMaMapping left, ComplexTypeToMaMapping right)
        {
            return !Equals(left, right);
        }
    }
}