using System;
using CctsRepository.CdtLibrary;

namespace VIENNAAddIn.upcc3.repo.CdtLibrary
{
    internal static partial class CdtConSpecConverter
    {
        private static string GenerateDictionaryEntryNameDefaultValue(CdtConSpec cdtConSpec, string className)
        {
            return className + ". Content";
        }

        private static string GenerateUniqueIdentifierDefaultValue(CdtConSpec cdtConSpec, string className)
        {
            return Guid.NewGuid().ToString();
        }
    }
}
