
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
    public partial class AsbieSpec
    {
		public string Name { get; set; }
		
        public string UpperBound { get; set; }
		
        public string LowerBound { get; set; }

		public AggregationKind AggregationKind { get; set; }
		
		public IAbie AssociatedAbie { get; set; }

		public IAscc BasedOn { get; set; }

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

        public static AsbieSpec CloneAsbie(IAsbie asbie)
        {
            return new AsbieSpec
                   {
                   	   Name = asbie.Name,
                       UpperBound = asbie.UpperBound,
                       LowerBound = asbie.LowerBound,
                       AggregationKind = asbie.AggregationKind,
                       AssociatedAbie = asbie.AssociatedAbie,
					   BasedOn = asbie.BasedOn,
					   BusinessTerms = new List<string>(asbie.BusinessTerms),
					   Definition = asbie.Definition,
					   LanguageCode = asbie.LanguageCode,
					   SequencingKey = asbie.SequencingKey,
					   VersionIdentifier = asbie.VersionIdentifier,
					   UsageRules = new List<string>(asbie.UsageRules),
                   };
        }
    }
}

