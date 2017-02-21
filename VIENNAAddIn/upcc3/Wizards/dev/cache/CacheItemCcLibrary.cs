// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************

using System.Collections.Generic;
using CctsRepository.DocLibrary;

namespace VIENNAAddIn.upcc3.Wizards.dev.cache
{
    internal class CacheItemDocLibrary
    {
        internal IDocLibrary DocLibrary { get; set; }

        internal List<IMa> MasInLibrary { get; set; }

        internal CacheItemDocLibrary(IDocLibrary library)
        {
            DocLibrary = library;
        }
    }
}