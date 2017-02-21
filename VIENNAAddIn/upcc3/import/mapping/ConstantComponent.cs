using System;
using System.IO;

namespace VIENNAAddIn.upcc3.import.mapping
{
    /// <summary>
    /// Represents a MapForce mapping element of the form:
    /// <component name="constant" library="core" uid="17" kind="2">
    /// </summary>
    public class ConstantComponent : IEquatable<ConstantComponent>
    {
        public void PrettyPrint(TextWriter writer, string indent)
        {
            writer.WriteLine(indent + "Constant: " + Value);
        }

        public string Value { get; private set; }
        public InputOutputKey OutputKey { get; private set; }

        public ConstantComponent(string value, InputOutputKey outputKey)
        {
            Value = value;
            OutputKey = outputKey;
        }

        public static bool operator ==(ConstantComponent left, ConstantComponent right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ConstantComponent left, ConstantComponent right)
        {
            return !Equals(left, right);
        }

        public bool Equals(ConstantComponent other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Value, Value);// && Equals(other.OutputKey, OutputKey);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (ConstantComponent)) return false;
            return Equals((ConstantComponent) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Value != null ? Value.GetHashCode() : 0)*397) ^ (OutputKey != null ? OutputKey.GetHashCode() : 0);
            }
        }
    }
}