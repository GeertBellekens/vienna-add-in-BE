using System;
using System.Collections.Generic;
using CctsRepository.CcLibrary;
using VIENNAAddIn.upcc3.uml;
using VIENNAAddIn.Utils;

namespace VIENNAAddIn.upcc3.repo.CcLibrary
{
    internal static partial class AccSpecConverter
    {
        private static string GenerateDictionaryEntryNameDefaultValue(AccSpec accSpec)
        {
            return accSpec.Name + ". Details";
        }

        private static string GenerateUniqueIdentifierDefaultValue(AccSpec accSpec)
        {
            return Guid.NewGuid().ToString();
        }
    }
}