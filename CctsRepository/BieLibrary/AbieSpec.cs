
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

namespace CctsRepository.BieLibrary
{
    public partial class AbieSpec
    {
		public string Name { get; set; }

		public IAbie IsEquivalentTo { get; set; }

		public IAcc BasedOn { get; set; }

		public List<BbieSpec> Bbies { get; set; }

		public List<AsbieSpec> Asbies { get; set; }

		#region Tagged Values

        ///<summary>
        /// Tagged value 'businessTerm'.
        ///</summary>
		public IEnumerable<string> BusinessTerms { get; set; }

        ///<summary>
        /// Tagged value 'definition'.
        ///</summary>
		public string Definition { get; set; }

        ///<summary>
        /// Tagged value 'dictionaryEntryName'.
        ///</summary>
		public string DictionaryEntryName { get; set; }

        ///<summary>
        /// Tagged value 'languageCode'.
        ///</summary>
		public string LanguageCode { get; set; }

        ///<summary>
        /// Tagged value 'uniqueIdentifier'.
        ///</summary>
		public string UniqueIdentifier { get; set; }

        ///<summary>
        /// Tagged value 'versionIdentifier'.
        ///</summary>
		public string VersionIdentifier { get; set; }

        ///<summary>
        /// Tagged value 'usageRule'.
        ///</summary>
		public IEnumerable<string> UsageRules { get; set; }

		#endregion

        public static AbieSpec CloneAbie(IAbie abie)
        {
            return new AbieSpec
                   {
                   	   Name = abie.Name,
					   IsEquivalentTo = abie.IsEquivalentTo,
					   BasedOn = abie.BasedOn,
					   Bbies = new List<BbieSpec>(abie.Bbies.Convert(o => BbieSpec.CloneBbie(o))),
					   Asbies = new List<AsbieSpec>(abie.Asbies.Convert(o => AsbieSpec.CloneAsbie(o))),
					   BusinessTerms = new List<string>(abie.BusinessTerms),
					   Definition = abie.Definition,
					   LanguageCode = abie.LanguageCode,
					   VersionIdentifier = abie.VersionIdentifier,
					   UsageRules = new List<string>(abie.UsageRules),
                   };
        }
	}
}

