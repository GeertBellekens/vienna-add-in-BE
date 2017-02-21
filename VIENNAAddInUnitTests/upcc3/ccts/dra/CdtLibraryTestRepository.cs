// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using EA;
using VIENNAAddIn;
using VIENNAAddIn.upcc3;
using VIENNAAddInUnitTests.TestRepository;
using VIENNAAddInUtils;
using Stereotype=VIENNAAddIn.upcc3.Stereotype;

namespace VIENNAAddInUnitTests.upcc3.ccts.dra
{
    internal class CdtLibraryTestRepository : EARepository
    {
        private Element primString;

        public CdtLibraryTestRepository()
        {
            this.AddModel("test model", m => { m.AddPackage("blib1", InitBLib1); });
        }

        private void InitBLib1(Package bLib1)
        {
            bLib1.Element.Stereotype = Stereotype.bLibrary;
            bLib1.AddDiagram("blib1", "Package");
            bLib1.AddPackage("primlib1", InitPRIMLib1);
            bLib1.AddPackage("cdtlib1", InitCDTLib1);
        }

        private void InitPRIMLib1(Package primLib1)
        {
            primLib1.Element.Stereotype = Stereotype.PRIMLibrary;
            primLib1.AddDiagram("primlib1", "Class");
            primLib1.AddTaggedValue(TaggedValues.baseURN.ToString()).WithValue("urn:test:blib1:primlib1");
            primString = primLib1.AddPRIM("String");
        }

        private void InitCDTLib1(Package cdtLib1)
        {
            cdtLib1.Element.Stereotype = Stereotype.CDTLibrary;
            cdtLib1.AddDiagram("cdtlib1", "Class");
            cdtLib1.AddClass("Text").With(e =>
                                              {
                                                  e.Stereotype = Stereotype.CDT;
                                                  e.AddTaggedValue(TaggedValues.dictionaryEntryName.ToString()).WithValue("Text. Type");
                                                  e.AddTaggedValue(TaggedValues.uniqueIdentifier.ToString()).WithValue("abc");
                                                  e.AddCON(primString);
                                                  e.AddSUPs(primString, "Language", "Language.Locale");
                                              });
        }

        #region Paths

        public static Path PathToCdtLibrary()
        {
            return (Path) "test model"/"blib1"/"cdtlib1";
        }

        public static Path PathToCdtText()
        {
            return (Path) "test model"/"blib1"/"cdtlib1"/"Text";
        }

        public static Path PathToPrimString()
        {
            return (Path) "test model"/"blib1"/"primlib1"/"String";
        }

        #endregion
    }
}