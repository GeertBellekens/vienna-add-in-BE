using System;
using CctsRepository.BdtLibrary;

namespace VIENNAAddIn.upcc3.repo.BdtLibrary
{
    internal static partial class BdtSupSpecConverter
    {
        private static string GenerateDictionaryEntryNameDefaultValue(BdtSupSpec bdtSupSpec, string className)
        {
            return className + ". " + bdtSupSpec.Name + ". " + bdtSupSpec.BasicType.Name;
        }

        private static string GenerateUniqueIdentifierDefaultValue(BdtSupSpec bdtSupSpec, string className)
        {
            return Guid.NewGuid().ToString();
        }
    }
}
