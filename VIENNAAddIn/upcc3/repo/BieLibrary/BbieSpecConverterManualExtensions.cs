using System;
using CctsRepository.BieLibrary;

namespace VIENNAAddIn.upcc3.repo.BieLibrary
{
    internal static partial class BbieSpecConverter
    {
        private static string GenerateDictionaryEntryNameDefaultValue(BbieSpec bbieSpec, string className)
        {
            return className + ". " + bbieSpec.Name + ". " + bbieSpec.Bdt.Name;
        }

        private static string GenerateUniqueIdentifierDefaultValue(BbieSpec bbieSpec, string className)
        {
            return Guid.NewGuid().ToString();
        }
    }
}
