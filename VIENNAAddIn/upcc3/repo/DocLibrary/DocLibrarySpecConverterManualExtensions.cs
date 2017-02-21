using System;
using CctsRepository.DocLibrary;

namespace VIENNAAddIn.upcc3.repo.DocLibrary
{
    internal static partial class DocLibrarySpecConverter
    {
        private static string GenerateUniqueIdentifierDefaultValue(DocLibrarySpec docLibrarySpec)
        {
            return Guid.NewGuid().ToString();
        }
    }
}