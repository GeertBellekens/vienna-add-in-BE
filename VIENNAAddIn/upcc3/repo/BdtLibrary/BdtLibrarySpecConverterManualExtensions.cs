using System;
using CctsRepository.BdtLibrary;

namespace VIENNAAddIn.upcc3.repo.BdtLibrary
{
    internal static partial class BdtLibrarySpecConverter
    {
        private static string GenerateUniqueIdentifierDefaultValue(BdtLibrarySpec bdtLibrarySpec)
        {
            return Guid.NewGuid().ToString();
        }
    }
}