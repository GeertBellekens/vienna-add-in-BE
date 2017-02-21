// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System.Collections.Generic;

namespace CctsRepository.DocLibrary
{
    public partial interface IDocLibrary
    {
        ///<summary>
        /// Returns the root MA of the document defined in this DOCLibrary.
        ///</summary>
        IMa DocumentRoot { get; }

        ///<summary>
        /// Returns the MAs contained in this DOCLibrary that are not the root element of the defined document.
        ///</summary>
        IEnumerable<IMa> NonRootMas { get; }
    }
}