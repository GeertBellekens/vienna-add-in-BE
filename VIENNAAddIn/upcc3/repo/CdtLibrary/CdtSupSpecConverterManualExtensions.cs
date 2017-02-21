using System;
using CctsRepository.CdtLibrary;

namespace VIENNAAddIn.upcc3.repo.CdtLibrary
{
    internal static partial class CdtSupSpecConverter
    {
        private static string GenerateDictionaryEntryNameDefaultValue(CdtSupSpec cdtSupSpec, string className)
        {
            return className + ". " + cdtSupSpec.Name + ". " + cdtSupSpec.BasicType.Name;
        }

        private static string GenerateUniqueIdentifierDefaultValue(CdtSupSpec cdtSupSpec, string className)
        {
            return Guid.NewGuid().ToString();
        }
    }
}
