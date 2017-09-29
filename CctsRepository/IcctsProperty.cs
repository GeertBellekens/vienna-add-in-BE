using System.Collections.Generic;
using System.Linq;
using System;

namespace CctsRepository
{
	/// <summary>
	/// Description of IcctsProperty.
	/// </summary>
	public interface ICctsProperty
	{
		int Id { get; }
		
		string Name { get; }
		
        string UpperBound { get; }
		
        string LowerBound { get; }
		
        ICctsElement Owner {get;}
        
        bool IsOptional();
        
        /// <summary>
        /// The relative position of this Property within all attributes and associations of the owner element.
        /// Should be stored in the tagged value SequencingKey
        /// </summary>
        int position {get; set;}
        
        ///<summary>
        /// Tagged value 'businessTerm'.
        ///</summary>
		IEnumerable<string> BusinessTerms { get; }

        ///<summary>
        /// Tagged value 'definition'.
        ///</summary>
		string Definition { get; }

        ///<summary>
        /// Tagged value 'dictionaryEntryName'.
        ///</summary>
		string DictionaryEntryName { get; }

        ///<summary>
        /// Tagged value 'languageCode'.
        ///</summary>
		string LanguageCode { get; }

        ///<summary>
        /// Tagged value 'sequencingKey'.
        ///</summary>
		string SequencingKey { get; }

        ///<summary>
        /// Tagged value 'uniqueIdentifier'.
        ///</summary>
		string UniqueIdentifier { get; }

        ///<summary>
        /// Tagged value 'versionIdentifier'.
        ///</summary>
		string VersionIdentifier { get; }

        ///<summary>
        /// Tagged value 'usageRule'.
        ///</summary>
		IEnumerable<string> UsageRules { get; }
	}
}
