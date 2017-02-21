
// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************

using System.Collections.Generic;

namespace CctsRepository.DocLibrary
{
	/// <summary>
	/// Specification for CCTS/UPCC DOCLibrary.
	/// </summary>
    public class DocLibrarySpec
    {
		/// <summary>
		/// The DOCLibrary's name.
		/// </summary>
        public string Name { get; set; }
		
		#region Tagged Values

        ///<summary>
        /// Tagged value 'businessTerm'.
        ///</summary>
		public IEnumerable<string> BusinessTerms { get; set; }

        ///<summary>
        /// Tagged value 'copyright'.
        ///</summary>
		public IEnumerable<string> Copyrights { get; set; }

        ///<summary>
        /// Tagged value 'owner'.
        ///</summary>
		public IEnumerable<string> Owners { get; set; }

        ///<summary>
        /// Tagged value 'reference'.
        ///</summary>
		public IEnumerable<string> References { get; set; }

        ///<summary>
        /// Tagged value 'status'.
        ///</summary>
		public string Status { get; set; }

        ///<summary>
        /// Tagged value 'uniqueIdentifier'.
        ///</summary>
		public string UniqueIdentifier { get; set; }

        ///<summary>
        /// Tagged value 'versionIdentifier'.
        ///</summary>
		public string VersionIdentifier { get; set; }

        ///<summary>
        /// Tagged value 'baseURN'.
        ///</summary>
		public string BaseURN { get; set; }

        ///<summary>
        /// Tagged value 'namespacePrefix'.
        ///</summary>
		public string NamespacePrefix { get; set; }

		#endregion

        public static DocLibrarySpec CloneDocLibrary(IDocLibrary docLibrary)
        {
            return new DocLibrarySpec
                   {
						Name = docLibrary.Name,
						BusinessTerms = new List<string>(docLibrary.BusinessTerms),
						Copyrights = new List<string>(docLibrary.Copyrights),
						Owners = new List<string>(docLibrary.Owners),
						References = new List<string>(docLibrary.References),
						Status = docLibrary.Status,
						VersionIdentifier = docLibrary.VersionIdentifier,
						BaseURN = docLibrary.BaseURN,
						NamespacePrefix = docLibrary.NamespacePrefix,
                   };
        }
	}
}

