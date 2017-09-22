using System.Collections.Generic;
using System.Linq;
using System;

namespace CctsRepository
{
	/// <summary>
	/// Description of ICctsLibrary.
	/// </summary>
	public interface ICctsLibrary
	{
		/// <summary>
		/// The ICctsLibrary's unique ID.
		/// </summary>
        int Id { get; }
		
		/// <summary>
		/// The ICctsLibrary's name.
		/// </summary>
        string Name { get; }
        
        /// <summary>
        /// the elements contained in this library
        /// </summary>
        IEnumerable<ICctsElement> Elements {get;}
        
        ///<summary>
        /// Tagged value 'businessTerm'.
        ///</summary>
		IEnumerable<string> BusinessTerms { get; }

        ///<summary>
        /// Tagged value 'copyright'.
        ///</summary>
		IEnumerable<string> Copyrights { get; }

        ///<summary>
        /// Tagged value 'owner'.
        ///</summary>
		IEnumerable<string> Owners { get; }

        ///<summary>
        /// Tagged value 'reference'.
        ///</summary>
		IEnumerable<string> References { get; }

        ///<summary>
        /// Tagged value 'status'.
        ///</summary>
		string Status { get; }

        ///<summary>
        /// Tagged value 'uniqueIdentifier'.
        ///</summary>
		string UniqueIdentifier { get; }

        ///<summary>
        /// Tagged value 'versionIdentifier'.
        ///</summary>
		string VersionIdentifier { get; }

        ///<summary>
        /// Tagged value 'baseURN'.
        ///</summary>
		string BaseURN { get; }

        ///<summary>
        /// Tagged value 'namespacePrefix'.
        ///</summary>
		string NamespacePrefix { get; }
		
	}
}
