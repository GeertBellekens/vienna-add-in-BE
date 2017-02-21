
// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************

using System.Collections.Generic;
// ReSharper disable RedundantUsingDirective
using CctsRepository.BdtLibrary;
using CctsRepository.BieLibrary;
using CctsRepository.BLibrary;
using CctsRepository.CcLibrary;
using CctsRepository.CdtLibrary;
using CctsRepository.DocLibrary;
using CctsRepository.EnumLibrary;
using CctsRepository.PrimLibrary;
// ReSharper restore RedundantUsingDirective

namespace CctsRepository.EnumLibrary
{
    public interface ICodelistEntry
    {
		int Id { get; }
		
		string Name { get; }
		
        IEnum Enum { get; }

		#region Tagged Values

        ///<summary>
        /// Tagged value 'codeName'.
        ///</summary>
		string CodeName { get; }

        ///<summary>
        /// Tagged value 'status'.
        ///</summary>
		string Status { get; }

		#endregion
    }
}
