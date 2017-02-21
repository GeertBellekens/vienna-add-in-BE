using System;
using CctsRepository.BieLibrary;

namespace VIENNAAddIn.upcc3.repo.BieLibrary
{
    internal static partial class AbieSpecConverter
    {
        private static string GenerateDictionaryEntryNameDefaultValue(AbieSpec abieSpec)
        {
            return abieSpec.Name + ". Details";
        }

        private static string GenerateUniqueIdentifierDefaultValue(AbieSpec abieSpec)
        {
            return Guid.NewGuid().ToString();
        }
    }
}