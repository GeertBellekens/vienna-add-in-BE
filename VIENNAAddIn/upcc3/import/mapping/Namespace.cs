using System;

namespace VIENNAAddIn.upcc3.import.mapping
{
    public class Namespace : IEquatable<Namespace>
    {
        public Namespace(string id)
        {
            ID = id;
        }

        public string ID { get; private set; }

        #region IEquatable<Namespace> Members

        public bool Equals(Namespace other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.ID, ID);
        }

        #endregion

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(Namespace)) return false;
            return Equals((Namespace)obj);
        }

        public override int GetHashCode()
        {
            return (ID != null ? ID.GetHashCode() : 0);
        }
    }
}