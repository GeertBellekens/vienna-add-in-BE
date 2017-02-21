
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

namespace CctsRepository.DocLibrary
{
    public partial class AsmaSpec
    {
		public string Name { get; set; }
		
        public string UpperBound { get; set; }
		
        public string LowerBound { get; set; }
		
		public BieAggregator AssociatedBieAggregator { get; set; }

        public static AsmaSpec CloneAsma(IAsma asma)
        {
            return new AsmaSpec
                   {
                   	   Name = asma.Name,
                       UpperBound = asma.UpperBound,
                       LowerBound = asma.LowerBound,
                       AssociatedBieAggregator = asma.AssociatedBieAggregator,
                   };
        }
    }
}

