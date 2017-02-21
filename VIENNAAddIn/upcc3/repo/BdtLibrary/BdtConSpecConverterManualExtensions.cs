using System;
using CctsRepository.BdtLibrary;

namespace VIENNAAddIn.upcc3.repo.BdtLibrary
{
    internal static partial class BdtConSpecConverter
    {
        private static string GenerateDictionaryEntryNameDefaultValue(BdtConSpec bdtConSpec, string className)
        {
            return className + ". Content";
        }

        private static string GenerateUniqueIdentifierDefaultValue(BdtConSpec bdtConSpec, string className)
        {
            return Guid.NewGuid().ToString();
        }
    }
}
