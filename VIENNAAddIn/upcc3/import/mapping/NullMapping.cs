using System;

namespace VIENNAAddIn.upcc3.import.mapping
{
    public class NullMapping : ElementMapping, IEquatable<NullMapping>
    {
        public NullMapping() : base(null)
        {
        }

        public override string BIEName
        {
            get { return string.Empty; }
        }

        #region IEquatable<NullMapping> Members

        public bool Equals(NullMapping other)
        {
            return !ReferenceEquals(null, other);
        }

        #endregion

        public override string ToString()
        {
            return "NullMapping";
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (NullMapping)) return false;
            return Equals((NullMapping) obj);
        }

        public override int GetHashCode()
        {
            return 0;
        }

        public static bool operator ==(NullMapping left, NullMapping right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(NullMapping left, NullMapping right)
        {
            return !Equals(left, right);
        }

        public override bool ResolveTypeMapping(SchemaMapping schemaMapping)
        {
            // do nothing
            return false;
        }
    }
}