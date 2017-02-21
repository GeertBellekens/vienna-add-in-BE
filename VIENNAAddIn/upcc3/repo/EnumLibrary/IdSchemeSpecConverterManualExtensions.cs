using System;
using CctsRepository.EnumLibrary;

namespace VIENNAAddIn.upcc3.repo.EnumLibrary
{
    internal static partial class IdSchemeSpecConverter
    {
        private static string GenerateDictionaryEntryNameDefaultValue(IdSchemeSpec idSchemeSpec)
        {
            return idSchemeSpec.Name;
        }

        private static string GenerateUniqueIdentifierDefaultValue(IdSchemeSpec idSchemeSpec)
        {
            return Guid.NewGuid().ToString();
        }
    }
}