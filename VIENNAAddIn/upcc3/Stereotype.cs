// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System.Collections.Generic;
using System.Linq;

namespace VIENNAAddIn.upcc3
{
    ///<summary>
    /// Definition of stereotype strings for UPCC elements.
    ///</summary>
    public static class Stereotype
    {
		public const string ABIE = "ABIE";
		public const string ACC = "ACC";
		public const string ASBIE = "ASBIE";
		public const string ASCC = "ASCC";
		public const string ASMA = "ASMA";
		public const string BBIE = "BBIE";
		public const string BCC = "BCC";
		public const string BDT = "BDT";
		public const string BDTLibrary = "BDTLibrary";
		public const string BIELibrary = "BIELibrary";
		public const string CCLibrary = "CCLibrary";
		public const string CDT = "CDT";
		public const string CDTLibrary = "CDTLibrary";
		public const string CON = "CON";
		public const string CodelistEntry = "CodelistEntry";
		public const string DOCLibrary = "DOCLibrary";
		public const string e_DocLibrary = "e_DocLibrary";
		public const string ENUM = "ENUM";
		public const string ENUMLibrary = "ENUMLibrary";
		public const string IDSCHEME = "IDSCHEME";
		public const string MA = "MA";
		public const string PRIM = "PRIM";
		public const string PRIMLibrary = "PRIMLibrary";
		public const string SUP = "SUP";
		public const string bInformationV = "bInformationV";
		public const string bLibrary = "bLibrary";
		public const string basedOn = "basedOn";
		public const string isEquivalentTo = "isEquivalentTo";

        private static readonly List<string> BusinessLibraryStereotypes = new List<string>
                                                                          {
																			  BDTLibrary,
																			  BIELibrary,
																			  bLibrary,
																			  CCLibrary,
																			  CDTLibrary,
																			  DOCLibrary,
																			  ENUMLibrary,
																			  PRIMLibrary,
                                                                          };
        public static readonly List<string> DocLibraryStereotypes = new List<string>{
        																DOCLibrary,
        																e_DocLibrary};
        			

        /// <returns>True if the given stereotype is one of the stereotypes for business libraries, false otherwise.</returns>
        public static bool IsBusinessLibraryStereotype(string stereotype)
        {
            return BusinessLibraryStereotypes.Contains(stereotype);
        }
        /// <summary>
        /// Used to verify if the given stereotype is a DocLibrary stereotype
        /// </summary>
        /// <param name="stereotype">the stereotype to check</param>
        /// <returns>true if the stereotype is one of the DocLibrary stereotypes</returns>
        public static bool IsDocLibraryStereotype(string stereotype)
        {
        	return DocLibraryStereotypes.Contains(stereotype);
        }
        public static bool HasStereotype(string stereotypeList,string stereotype)
        {
        	return stereotypeList.Split(new []{","},System.StringSplitOptions.RemoveEmptyEntries).Contains(stereotype);
        }
    }
}