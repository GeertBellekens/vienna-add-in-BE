
// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************

using System.Collections.Generic;
using VIENNAAddInUtils;
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

namespace CctsRepository.DocLibrary
{
    public partial class MaSpec
    {
		public string Name { get; set; }

		public List<AsmaSpec> Asmas { get; set; }

        public static MaSpec CloneMa(IMa ma)
        {
            return new MaSpec
                   {
                   	   Name = ma.Name,
					   Asmas = new List<AsmaSpec>(ma.Asmas.Convert(o => AsmaSpec.CloneAsma(o))),
                   };
        }
	}
}

