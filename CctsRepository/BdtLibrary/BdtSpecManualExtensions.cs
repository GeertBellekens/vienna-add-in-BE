// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System;
using System.Collections.Generic;
using CctsRepository.CdtLibrary;
using VIENNAAddInUtils;

namespace CctsRepository.BdtLibrary
{
    public partial class BdtSpec
    {
        public static BdtSpec CloneCdt(ICdt cdt, string name)
        {
            return new BdtSpec
                   {
                       Name = (String.IsNullOrEmpty(name) ? cdt.Name : name),
                       BasedOn = cdt,
                       Definition = cdt.Definition,
                       LanguageCode = cdt.LanguageCode,
                       BusinessTerms = new List<string>(cdt.BusinessTerms),
                       UsageRules = new List<string>(cdt.UsageRules),
                       Con = CloneCdtCon(cdt.Con),
                       Sups = new List<BdtSupSpec>(cdt.Sups.Convert(sup => CloneCdtSup(sup))),
                   };
        }

        private static BdtConSpec CloneCdtCon(ICdtCon cdtCon)
        {
            return new BdtConSpec
            {
                BasicType = cdtCon.BasicType,
                UpperBound = cdtCon.UpperBound,
                LowerBound = cdtCon.LowerBound,
                Definition = cdtCon.Definition,
                LanguageCode = cdtCon.LanguageCode,
                BusinessTerms = new List<string>(cdtCon.BusinessTerms),
                ModificationAllowedIndicator = cdtCon.ModificationAllowedIndicator,
                UsageRules = new List<string>(cdtCon.UsageRules),
                Name = cdtCon.Name,
            };
        }

        public static BdtSupSpec CloneCdtSup(ICdtSup cdtSup)
        {
            return new BdtSupSpec
                   {
                       Name = cdtSup.Name,
                       Definition = cdtSup.Definition,
                       LanguageCode = cdtSup.LanguageCode,
                       BusinessTerms = new List<string>(cdtSup.BusinessTerms),
                       BasicType = cdtSup.BasicType,
                       UpperBound = cdtSup.UpperBound,
                       LowerBound = cdtSup.LowerBound,
                       ModificationAllowedIndicator = cdtSup.ModificationAllowedIndicator,
                       UsageRules = new List<string>(cdtSup.UsageRules),
                   }
                ;
        }
    }
}