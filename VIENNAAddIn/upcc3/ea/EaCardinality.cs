using System;

namespace VIENNAAddIn.upcc3.ea
{
    internal class EaCardinality
    {
        public EaCardinality(string lowerBound, string upperBound)
        {
            LowerBound = string.IsNullOrEmpty(lowerBound) ? "1" : lowerBound;
            UpperBound = string.IsNullOrEmpty(upperBound) ? lowerBound : upperBound;
        }

        public EaCardinality(string cardinality)
        {
            string[] parts = cardinality.Split(new[] {'.'}, StringSplitOptions.RemoveEmptyEntries);
            switch (parts.Length)
            {
                case 1:
                    LowerBound = (parts[0] == "*" ? "0" : parts[0]);
                    UpperBound = parts[0];
                    break;
                case 2:
                    LowerBound = parts[0];
                    UpperBound = parts[1];
                    break;
                default:
                    LowerBound = "1";
                    UpperBound = "1";
                    break;
            }
        }

        public string LowerBound { get; private set; }
        public string UpperBound { get; private set; }

        public override string ToString()
        {
            if (LowerBound == UpperBound)
            {
                return LowerBound;
            }
            return string.Format("{0}..{1}", LowerBound, UpperBound);
        }
    }
}