using System;
using CctsRepository.EnumLibrary;

namespace VIENNAAddIn.upcc3.repo.EnumLibrary
{
    internal static partial class EnumSpecConverter
    {
        private static string GenerateDictionaryEntryNameDefaultValue(EnumSpec enumSpec)
        {
            return enumSpec.Name;
        }

        private static string GenerateUniqueIdentifierDefaultValue(EnumSpec enumSpec)
        {
            return Guid.NewGuid().ToString();
        }
    }
}