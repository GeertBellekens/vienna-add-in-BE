using System;
using CctsRepository.CcLibrary;

namespace VIENNAAddIn.upcc3.repo.CcLibrary
{
    internal static partial class AsccSpecConverter
    {
        private static string GenerateDictionaryEntryNameDefaultValue(AsccSpec asccSpec, string associatingClassName)
        {
            return associatingClassName + ". " + asccSpec.Name + ". " + asccSpec.AssociatedAcc.Name;
        }

        private static string GenerateUniqueIdentifierDefaultValue(AsccSpec asccSpec, string associatingClassName)
        {
            return Guid.NewGuid().ToString();
        }
    }
}