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
using CctsRepository.CcLibrary;

namespace CctsRepository.BieLibrary
{
    public partial class BbieSpec
    {
        public static BbieSpec CloneBcc(IBcc bcc, IBdt bdt)
        {
            return new BbieSpec
                   {
                       Name = bcc.Name,
                       Bdt = bdt,
                       LowerBound = bcc.LowerBound,
                       UpperBound = bcc.UpperBound,
                       BusinessTerms = new List<string>(bcc.BusinessTerms),
                       Definition = bcc.Definition,
                       LanguageCode = bcc.LanguageCode,
                       SequencingKey = bcc.SequencingKey,
                       UsageRules = new List<string>(bcc.UsageRules),
                   };
        }
    }
}