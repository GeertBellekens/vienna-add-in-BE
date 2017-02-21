using EA;
using VIENNAAddIn;
using VIENNAAddInUnitTests.TestRepository;
using Stereotype=VIENNAAddIn.upcc3.Stereotype;

namespace VIENNAAddInUnitTests.upcc3.import.mapping
{
    internal class MappingTestRepository : EARepository
    {

        public MappingTestRepository()
        {
            Element cdtText = null;
            Element cdtCode = null;
            Element cdtDateTime = null;
            Element primString = null;
            this.AddModel(
                "test", m => m.AddPackage("bLibrary", bLibrary =>
                                                      {
                                                          bLibrary.Element.Stereotype = Stereotype.bLibrary;
                                                          bLibrary.AddDiagram("bLibrary", "Class");
                                                          bLibrary.AddPackage("PRIMLibrary", package =>
                                                                                             {
                                                                                                 package.Element.Stereotype = Stereotype.PRIMLibrary;
                                                                                                 primString = package.AddPRIM("String");
                                                                                             });
                                                          bLibrary.AddPackage("CDTLibrary", package =>
                                                                                            {
                                                                                                package.Element.Stereotype = Stereotype.CDTLibrary;
                                                                                                cdtText = package.AddCDT("Text").With(e =>
                                                                                                                                      {
                                                                                                                                          e.Stereotype = Stereotype.CDT;
                                                                                                                                          e.AddCON(primString);
                                                                                                                                          e.AddSUPs(primString, "Language", "UnusedSup", "LanguageLocale");
                                                                                                                                      });
                                                                                                cdtCode = package.AddCDT("Code").With(e =>
                                                                                                {
                                                                                                    e.Stereotype = Stereotype.CDT;
                                                                                                    e.AddCON(primString);
                                                                                                    e.AddSUPs(primString, "Language", "Name");
                                                                                                });

                                                                                                cdtDateTime = package.AddCDT("DateTime").With(e =>
                                                                                                                                      {
                                                                                                                                          e.Stereotype = Stereotype.CDT;
                                                                                                                                          e.AddCON(primString);
                                                                                                                                      });
                                                                                            });
                                                          bLibrary.AddPackage("CCLibrary", package =>
                                                                                           {
                                                                                               package.Element.Stereotype = Stereotype.CCLibrary;
                                                                                               package.AddClass("Foo").With(e => e.Stereotype = Stereotype.ACC);
                                                                                               Element accAddress = package.AddClass("Address")
                                                                                                   .With(e => e.Stereotype = Stereotype.ACC)
                                                                                                   .With(e => e.AddBCCs(cdtText, "CountryName", "StreetName", "CityName", "BuildingNumber"))
                                                                                                   .With(e => e.AddBCCs(cdtCode, "Country"));
                                                                                               package.AddClass("AccountingVoucher")
                                                                                                   .With(e => e.Stereotype = Stereotype.ACC);
                                                                                               package.AddClass("Person")
                                                                                                   .With(e => e.Stereotype = Stereotype.ACC)
                                                                                                   .With(e => e.AddBCCs(cdtText, "Name"));
                                                                                               Element accParty = package.AddClass("Party")
                                                                                                   .With(e => e.Stereotype = Stereotype.ACC)
                                                                                                   .With(e => e.AddBCCs(cdtText, "Name"))
                                                                                                   .With(e => e.AddASCC(accAddress, "Residence"));
                                                                                               accParty.AddASCC(accParty, "Children");
                                                                                               package.AddClass("TradeLineItem")
                                                                                                   .With(e => e.Stereotype = Stereotype.ACC)
                                                                                                   .With(e => e.AddBCCs(cdtText, "Identifer", "SequenceNumeric", "GrossWeightMeasure", "NetWeightMeasure", "GrossVolumeMeasure", "ChargeAmount"));
                                                                                               Element accNote = package.AddClass("Note")
                                                                                                   .With(e => e.Stereotype = Stereotype.ACC)
                                                                                                   .With(e => e.AddBCCs(cdtText, "Content"));
                                                                                               package.AddClass("Document")
                                                                                                   .With(e => e.Stereotype = Stereotype.ACC)
                                                                                                   .With(e => e.AddBCCs(cdtDateTime, "Issue"))
                                                                                                   .With(e => e.AddASCC(accNote, "Included"));
                                                                                           });
                                                      }));
        }
    }
}