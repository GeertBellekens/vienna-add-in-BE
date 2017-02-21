using System;
using System.Collections.Generic;
using CctsRepository.CcLibrary;

namespace CctsRepository.BieLibrary
{
    public partial class AsbieSpec
    {
        public static AsbieSpec CloneAscc(IAscc ascc, string name, IAbie associatedAbie)
        {
            return new AsbieSpec
                   {
                       BusinessTerms = new List<string>(ascc.BusinessTerms),
                       Definition = ascc.Definition,
                       LanguageCode = ascc.LanguageCode,
                       SequencingKey = ascc.SequencingKey,
                       UsageRules = new List<string>(ascc.UsageRules),
                       Name = name,
                       AssociatedAbie = associatedAbie,
                       LowerBound = ascc.LowerBound,
                       UpperBound = ascc.UpperBound,
                   };
        }
    }
}