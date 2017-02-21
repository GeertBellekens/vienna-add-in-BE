using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.import.mapping
{
    internal class MappingFunction
    {
        public MappingFunction(IEnumerable<object> targetCcs)
        {
            TargetCcs = new List<object>(targetCcs).ToArray();
        }

        public bool IsSplit
        {
            get { return true; }
        }

        public object[] TargetCcs { get; private set; }
    }
}