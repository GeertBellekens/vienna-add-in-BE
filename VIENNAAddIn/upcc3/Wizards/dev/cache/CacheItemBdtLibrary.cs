// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************

using System.Collections.Generic;
using CctsRepository.BdtLibrary;

namespace VIENNAAddIn.upcc3.Wizards.dev.cache
{
    internal class CacheItemBdtLibrary
    {
        internal IBdtLibrary BdtLibrary { get; set; }

        internal List<IBdt> BdtsInLibrary { get; set; }

        internal CacheItemBdtLibrary(IBdtLibrary library)
        {
            BdtLibrary = library;
        }
    }
}