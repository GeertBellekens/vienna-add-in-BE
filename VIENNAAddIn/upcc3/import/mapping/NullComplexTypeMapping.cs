using System;

namespace VIENNAAddIn.upcc3.import.mapping
{
    public class NullComplexTypeMapping: ComplexTypeMapping, IEquatable<NullComplexTypeMapping>
    {
        public NullComplexTypeMapping() : base(string.Empty, string.Empty, new ElementMapping[0])
        {
        }

        public override string ToString()
        {
            return "NullMapping";
        }

        public bool Equals(NullComplexTypeMapping other)
        {
            return !ReferenceEquals(null, other);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (NullComplexTypeMapping)) return false;
            return Equals(obj);
        }

        public override int GetHashCode()
        {
            return 0;
        }

        public static bool operator ==(NullComplexTypeMapping left, NullComplexTypeMapping right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(NullComplexTypeMapping left, NullComplexTypeMapping right)
        {
            return !Equals(left, right);
        }

        public override string BIEName
        {
            get { return string.Empty; }
        }
    }
}