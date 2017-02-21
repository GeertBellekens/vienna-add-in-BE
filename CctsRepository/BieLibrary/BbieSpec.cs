
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

namespace CctsRepository.BieLibrary
{
    public partial class BbieSpec
    {
		public string Name { get; set; }
		
        public string UpperBound { get; set; }
		
        public string LowerBound { get; set; }
		
		public IBdt Bdt { get; set; }

		public IBcc BasedOn { get; set; }

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
        /// Tagged value 'sequencingKey'.
        ///</summary>
		public string SequencingKey { get; set; }

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

        public static BbieSpec CloneBbie(IBbie bbie)
        {
            return new BbieSpec
                   {
                   	   Name = bbie.Name,
                       UpperBound = bbie.UpperBound,
                       LowerBound = bbie.LowerBound,
                       Bdt = bbie.Bdt,
					   BasedOn = bbie.BasedOn,
					   BusinessTerms = new List<string>(bbie.BusinessTerms),
					   Definition = bbie.Definition,
					   LanguageCode = bbie.LanguageCode,
					   SequencingKey = bbie.SequencingKey,
					   VersionIdentifier = bbie.VersionIdentifier,
					   UsageRules = new List<string>(bbie.UsageRules),
                   };
        }
    }
}

