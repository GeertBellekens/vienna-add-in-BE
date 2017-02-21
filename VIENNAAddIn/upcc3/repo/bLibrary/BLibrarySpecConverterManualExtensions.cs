using System;
using CctsRepository.BLibrary;

namespace VIENNAAddIn.upcc3.repo.BLibrary
{
    internal static partial class BLibrarySpecConverter
    {
        private static string GenerateUniqueIdentifierDefaultValue(BLibrarySpec bLibrarySpec)
        {
            return Guid.NewGuid().ToString();
        }
    }
}