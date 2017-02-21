
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
    public partial class CodelistEntrySpec
    {
		public string Name { get; set; }

		#region Tagged Values

        ///<summary>
        /// Tagged value 'codeName'.
        ///</summary>
		public string CodeName { get; set; }

        ///<summary>
        /// Tagged value 'status'.
        ///</summary>
		public string Status { get; set; }

		#endregion

        public static CodelistEntrySpec CloneCodelistEntry(ICodelistEntry codelistEntry)
        {
            return new CodelistEntrySpec
                   {
                   	   Name = codelistEntry.Name,
					   CodeName = codelistEntry.CodeName,
					   Status = codelistEntry.Status,
                   };
        }
    }
}

