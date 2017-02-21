using System;
using CctsRepository.CcLibrary;

namespace VIENNAAddIn.upcc3.repo.CcLibrary
{
    internal static partial class BccSpecConverter
    {
        private static string GenerateDictionaryEntryNameDefaultValue(BccSpec bccSpec, string className)
        {
            return className + ". " + bccSpec.Name + ". " + bccSpec.Cdt.Name;
        }

        private static string GenerateUniqueIdentifierDefaultValue(BccSpec bccSpec, string className)
        {
            return Guid.NewGuid().ToString();
        }
    }
}
