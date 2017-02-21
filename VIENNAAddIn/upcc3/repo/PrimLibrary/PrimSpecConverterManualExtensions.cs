using System;
using CctsRepository.PrimLibrary;

namespace VIENNAAddIn.upcc3.repo.PrimLibrary
{
    internal static partial class PrimSpecConverter
    {
        private static string GenerateDictionaryEntryNameDefaultValue(PrimSpec primSpec)
        {
            return primSpec.Name;
        }

        private static string GenerateUniqueIdentifierDefaultValue(PrimSpec primSpec)
        {
            return Guid.NewGuid().ToString();
        }
    }
}