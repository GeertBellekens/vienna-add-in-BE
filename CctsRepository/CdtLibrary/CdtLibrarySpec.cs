
// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************

using System.Collections.Generic;

namespace CctsRepository.CdtLibrary
{
	/// <summary>
	/// Specification for CCTS/UPCC CDTLibrary.
	/// </summary>
    public class CdtLibrarySpec
    {
		/// <summary>
		/// The CDTLibrary's name.
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

        public static CdtLibrarySpec CloneCdtLibrary(ICdtLibrary cdtLibrary)
        {
            return new CdtLibrarySpec
                   {
						Name = cdtLibrary.Name,
						BusinessTerms = new List<string>(cdtLibrary.BusinessTerms),
						Copyrights = new List<string>(cdtLibrary.Copyrights),
						Owners = new List<string>(cdtLibrary.Owners),
						References = new List<string>(cdtLibrary.References),
						Status = cdtLibrary.Status,
						VersionIdentifier = cdtLibrary.VersionIdentifier,
						BaseURN = cdtLibrary.BaseURN,
						NamespacePrefix = cdtLibrary.NamespacePrefix,
                   };
        }
	}
}

