using System;
using CctsRepository.CcLibrary;

namespace VIENNAAddIn.upcc3.repo.CcLibrary
{
    internal static partial class CcLibrarySpecConverter
    {
        private static string GenerateUniqueIdentifierDefaultValue(CcLibrarySpec ccLibrarySpec)
        {
            return Guid.NewGuid().ToString();
        }
    }
}