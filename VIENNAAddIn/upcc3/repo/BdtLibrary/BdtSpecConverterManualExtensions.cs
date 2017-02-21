using System;
using CctsRepository.BdtLibrary;

namespace VIENNAAddIn.upcc3.repo.BdtLibrary
{
    internal static partial class BdtSpecConverter
    {
        private static string GenerateDictionaryEntryNameDefaultValue(BdtSpec bdtSpec)
        {
            return bdtSpec.Name + ". Type";
        }

        private static string GenerateUniqueIdentifierDefaultValue(BdtSpec bdtSpec)
        {
            return Guid.NewGuid().ToString();
        }
    }
}