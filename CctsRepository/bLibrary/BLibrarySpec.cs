
// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************

using System.Collections.Generic;

namespace CctsRepository.BLibrary
{
	/// <summary>
	/// Specification for CCTS/UPCC bLibrary.
	/// </summary>
    public class BLibrarySpec
    {
		/// <summary>
		/// The bLibrary's name.
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

		#endregion

        public static BLibrarySpec CloneBLibrary(IBLibrary bLibrary)
        {
            return new BLibrarySpec
                   {
						Name = bLibrary.Name,
						BusinessTerms = new List<string>(bLibrary.BusinessTerms),
						Copyrights = new List<string>(bLibrary.Copyrights),
						Owners = new List<string>(bLibrary.Owners),
						References = new List<string>(bLibrary.References),
						Status = bLibrary.Status,
						VersionIdentifier = bLibrary.VersionIdentifier,
                   };
        }
	}
}

