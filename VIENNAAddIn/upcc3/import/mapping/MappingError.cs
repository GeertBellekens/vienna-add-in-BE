using System;

namespace VIENNAAddIn.upcc3.import.mapping
{
    public class MappingError : Exception
    {
        public MappingError(string message) : base(message)
        {
        }
    }
}