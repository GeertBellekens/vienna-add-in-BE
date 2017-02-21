using System;
using CctsRepository.CdtLibrary;

namespace VIENNAAddIn.upcc3.repo.CdtLibrary
{
    internal static partial class CdtSpecConverter
    {
        private static string GenerateDictionaryEntryNameDefaultValue(CdtSpec cdtSpec)
        {
            return cdtSpec.Name + ". Type";
        }

        private static string GenerateUniqueIdentifierDefaultValue(CdtSpec cdtSpec)
        {
            return Guid.NewGuid().ToString();
        }
    }
}