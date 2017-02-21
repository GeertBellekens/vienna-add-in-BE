// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************

using System.Collections.Generic;
using CctsRepository.BieLibrary;

namespace VIENNAAddIn.upcc3.Wizards.dev.cache
{
    internal class CacheItemBieLibrary
    {
        internal IBieLibrary BieLibrary { get; set; }

        internal List<IAbie> AbiesInLibrary { get; set; }

        internal CacheItemBieLibrary(IBieLibrary library)
        {
            BieLibrary = library;
        }
    }
}