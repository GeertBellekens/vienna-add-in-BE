using System;
using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.uml
{
    public class UmlTaggedValueSpec : IEquatable<UmlTaggedValueSpec>
    {
        public UmlTaggedValueSpec(string name, string value)
        {
            Name = name;
            Value = value;
            DefaultValue = string.Empty;
        }

        public UmlTaggedValueSpec(string name, IEnumerable<string> values) : this(name, MultiPartTaggedValue.Merge(values))
        {
        }

        public string Name { get; set; }
        public string Value { get; set; }
        public string DefaultValue { get; set; }

        #region IEquatable<UmlTaggedValueSpec> Members

        public bool Equals(UmlTaggedValueSpec other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Name, Name) && Equals(other.Value, Value);
        }

        #endregion

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (UmlTaggedValueSpec)) return false;
            return Equals((UmlTaggedValueSpec) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Name != null ? Name.GetHashCode() : 0)*397) ^ (Value != null ? Value.GetHashCode() : 0);
            }
        }

        public static bool operator ==(UmlTaggedValueSpec left, UmlTaggedValueSpec right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(UmlTaggedValueSpec left, UmlTaggedValueSpec right)
        {
            return !Equals(left, right);
        }
    }
}