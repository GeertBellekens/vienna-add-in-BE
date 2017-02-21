using System;
using System.IO;

namespace VIENNAAddIn.upcc3.import.mapping
{
    public class FunctionComponent : IEquatable<FunctionComponent>
    {
        public string FunctionType { get; set; }
        public InputOutputKey FunctionNameKey { get; set; }
        public InputOutputKey[] InputKeys { get; set; }
        public InputOutputKey[] OutputKeys { get; set; }

        public FunctionComponent(string functionType, InputOutputKey functionNameKey, InputOutputKey[] inputKeys, InputOutputKey[] outputKeys)
        {
            FunctionType = functionType;
            FunctionNameKey = functionNameKey;
            InputKeys = inputKeys;
            OutputKeys = outputKeys;
        }

        public bool Equals(FunctionComponent other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.FunctionType, FunctionType);
//            return Equals(other.FunctionType, FunctionType) && Equals(other.FunctionNameKey, FunctionNameKey) && Equals(other.InputKeys, InputKeys) && Equals(other.OutputKeys, OutputKeys);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (FunctionComponent)) return false;
            return Equals((FunctionComponent) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = (FunctionType != null ? FunctionType.GetHashCode() : 0);
                result = (result*397) ^ (FunctionNameKey != null ? FunctionNameKey.GetHashCode() : 0);
                result = (result*397) ^ (InputKeys != null ? InputKeys.GetHashCode() : 0);
                result = (result*397) ^ (OutputKeys != null ? OutputKeys.GetHashCode() : 0);
                return result;
            }
        }

        public static bool operator ==(FunctionComponent left, FunctionComponent right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(FunctionComponent left, FunctionComponent right)
        {
            return !Equals(left, right);
        }

        public void PrettyPrint(TextWriter writer, string indent)
        {
            writer.WriteLine(indent + "Type: " + FunctionType + ", NameKey: " + FunctionNameKey);
        }
    }
}