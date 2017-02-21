// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************

using System.Collections.Generic;
using CctsRepository.CdtLibrary;

namespace VIENNAAddIn.upcc3.Wizards.dev.cache
{
    internal class CacheItemCdtLibrary
    {
        internal ICdtLibrary CdtLibrary { get; set; }

        internal List<ICdt> CdtsInLibrary { get; set; }

        internal CacheItemCdtLibrary(ICdtLibrary library)
        {
            CdtLibrary = library;
        }
    }
}