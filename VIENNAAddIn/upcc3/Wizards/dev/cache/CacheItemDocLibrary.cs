// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************

using System.Collections.Generic;
using CctsRepository.CcLibrary;

namespace VIENNAAddIn.upcc3.Wizards.dev.cache
{
    internal class CacheItemCcLibrary
    {
        internal ICcLibrary CcLibrary { get; set; }

        internal List<IAcc> CcsInLibrary { get; set; }

        internal CacheItemCcLibrary(ICcLibrary library)
        {
            CcLibrary = library;
        }
    }
}