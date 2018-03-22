using System.Collections.Generic;
using System.Linq;
using System;

namespace CctsRepository
{
	/// <summary>
	/// Description of ICctsElement.
	/// </summary>
	public interface ICctsElement
	{
		int Id { get; }
		
		string Name { get; }
		
		ICctsLibrary library {get;}
		
		IEnumerable<ICctsProperty> Properties {get;}
		IEnumerable<ICctsAttribute> Attributes {get;}
		IEnumerable<ICctsAssociation> Associations {get;}
		
		///<summary>
        /// Tagged value 'businessTerm'.
        ///</summary>
		IEnumerable<string> BusinessTerms { get; }
                
        /// <summary>
        /// the element this element is derived from
        /// </summary>
        ICctsElement SourceElement { get; }

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
