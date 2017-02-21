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
using VIENNAAddIn.upcc3.ea;
using VIENNAAddInUnitTests.TestRepository;
using VIENNAAddInUtils;
using Stereotype = VIENNAAddIn.upcc3.Stereotype;


namespace VIENNAAddInUnitTests.upcc3.Wizards.dev.TestRepository
{
    public class EARepositoryAbieEditor : EARepository
    {
        private Element accAddress;
        private Element accPerson;
        private Element bdtABCCode;
        private Element bdtCode;
        private Element bdtDate;
        private Element bdtMeasure;
        private Element bdtText;
        private Element bieInvoice;
        private Element bieInvoiceInfo;
        private Element bieMyAddress;
        private Element bieMyPerson;
        private Element cdtCode;
        private Element cdtDate;
        private Element cdtMeasure;
        private Element cdtText;
        private Element primDecimal;
        private Element primString;

        public EARepositoryAbieEditor()
        {
            this.AddModel("test model", InitTestModel);

            bdtMeasure.AddBasedOnDependency(cdtMeasure);
            bdtCode.AddBasedOnDependency(cdtCode);
            bdtABCCode.AddBasedOnDependency(cdtCode);
            bdtDate.AddBasedOnDependency(cdtDate);
            bdtText.AddBasedOnDependency(cdtText);
            bieMyAddress.AddBasedOnDependency(accAddress);

            accPerson.AddASCC(accAddress, "homeAddress");
            accPerson.AddASCC(accAddress, "workAddress", "0", "*");

            bieMyPerson.AddASBIE(bieMyAddress, "homeAddress", EaAggregationKind.Shared);
            bieMyPerson.AddBasedOnDependency(accPerson);            
            bieMyPerson.AddASBIE(bieMyAddress, "workAddress", EaAggregationKind.Composite, "0", "*");
            bieInvoice.AddASBIE(bieInvoiceInfo, "info", EaAggregationKind.Shared);
            bieInvoiceInfo.AddASBIE(bieMyAddress, "deliveryAddress", EaAggregationKind.Shared);
        }

        private void InitTestModel(Package m)
        {
            m.AddPackage("blib1", InitBLib1);
            m.AddPackage("Business Information View", InitBusinessInformationView);
            m.AddPackage("A package with an arbitrary stereotype",
                         p =>
                         {
                             p.Element.Stereotype = "Some arbitrary stereotype";
                             p.AddPackage("This bLibrary should _not_ be found because it's in the wrong location in the package hierarchy",
                                          p1 => { p1.Element.Stereotype = Stereotype.bLibrary; });
                             p.AddDiagram("A diagram", "Class");
                         });
        }

        private static void InitBusinessInformationView(Package p)
        {
            p.Element.Stereotype = Stereotype.bInformationV;
            p.AddPackage(
                "A bLibrary nested in a bInformationV",
                bLibrary =>
                {
                    bLibrary.Element.Stereotype = Stereotype.bLibrary;
                    bLibrary.AddPackage("Another PRIMLibrary",
                                        primLibrary =>
                                        {
                                            primLibrary.Element.Stereotype =
                                                Stereotype.PRIMLibrary;
                                        });
                });
        }

        private void InitBLib1(Package bLib1)
        {
            bLib1.Element.Stereotype = Stereotype.bLibrary;
            bLib1.AddDiagram("blib1", "Package");
            bLib1.AddTaggedValue(TaggedValues.baseURN.ToString()).WithValue("urn:test:blib1");
            bLib1.AddPackage("primlib1", InitPRIMLib1);
            bLib1.AddPackage("enumlib1", InitENUMLib1);
            bLib1.AddPackage("cdtlib1", InitCDTLib1);
            bLib1.AddPackage("bdtlib1", InitBDTLib1);
            bLib1.AddPackage("cclib1", InitCCLib1);
            bLib1.AddPackage("bielib1", InitBIELib1);
            bLib1.AddPackage("DOCLibrary", InitDOCLibrary);
        }

        private void InitPRIMLib1(Package primLib1)
        {
            primLib1.Element.Stereotype = Stereotype.PRIMLibrary;
            primLib1.AddDiagram("primlib1", "Class");
            primLib1.AddTaggedValue(TaggedValues.baseURN.ToString()).WithValue("urn:test:blib1:primlib1");
            primString = primLib1.AddPRIM("String");
            primDecimal = primLib1.AddPRIM("Decimal");
            AddInvalidElement(primLib1);
        }

        private void InitENUMLib1(Package enumLib1)
        {
            enumLib1.Element.Stereotype = Stereotype.ENUMLibrary;
            enumLib1.AddDiagram("enumlib1", "Class");
            enumLib1.AddTaggedValue(TaggedValues.baseURN.ToString()).WithValue("urn:test:blib1:enumlib1");
            enumLib1.AddENUM("ABC_Codes", primString, new[] {"ABC Code 1", "abc1", "status"}, new[] {"ABC Code 2", "abc2", "status"});
            AddInvalidElement(enumLib1);
        }

        private void InitCDTLib1(Package cdtLib1)
        {
            cdtLib1.Element.Stereotype = Stereotype.CDTLibrary;
            cdtLib1.AddDiagram("cdtlib1", "Class");
            cdtLib1.AddTaggedValue(TaggedValues.baseURN.ToString()).WithValue("urn:test:blib1:cdtlib1");
            cdtMeasure = cdtLib1.AddClass("Measure").With(e =>
                                                                     {
                                                                         e.Stereotype = Stereotype.CDT;
                                                                         e.AddCON(primDecimal);
                                                                         e.AddSUP(primString, "MeasureUnit");
                                                                         e.AddAttribute("MeasureUnit.CodeListVersion", primString)
                                                                             .With(a =>
                                                                                   {
                                                                                       a.Stereotype = Stereotype.SUP.ToString();
                                                                                       a.LowerBound = "1";
                                                                                       a.UpperBound = "*";
                                                                                   });
                                                                     });
            cdtCode = cdtLib1.AddClass("Code").With(e =>
                                                               {
                                                                   e.Stereotype = Stereotype.CDT;
                                                                   e.AddCON(primString);
                                                                   e.AddSUPs(primString, "Name", "CodeList.Agency", "CodeList.AgencyName", "CodeList", "CodeList.Name", "CodeList.UniformResourceIdentifier", "CodeList.Version", "CodeListScheme.UniformResourceIdentifier", "Language");
                                                               });
            cdtDate = cdtLib1.AddClass("Date").With(e =>
                                                               {
                                                                   e.Stereotype = Stereotype.CDT;
                                                                   e.AddTaggedValue(TaggedValues.definition.ToString()).WithValue("A Date.");
                                                                   e.AddCON(primString);
                                                                   e.AddSUPs(primString, "Format");
                                                               });
            cdtText = cdtLib1.AddClass("Text").With(e =>
                                                               {
                                                                   e.Stereotype = Stereotype.CDT;
                                                                   e.AddCON(primString);
                                                                   e.AddSUPs(primString, "Language", "Language.Locale");
                                                               });
            AddInvalidElement(cdtLib1);
        }

        private void InitBDTLib1(Package bdtLib1)
        {
            bdtLib1.Element.Stereotype = Stereotype.BDTLibrary;
            bdtLib1.AddDiagram("bdtlib1", "Class");
            bdtLib1.AddTaggedValue(TaggedValues.baseURN.ToString()).WithValue("urn:test:blib1:bdtlib1");
            bdtMeasure = bdtLib1.AddClass("Measure").With(e =>
                                                                     {
                                                                         e.Stereotype = Stereotype.BDT;
                                                                         e.AddCON(primDecimal);
                                                                         e.AddSUP(primString, "MeasureUnit");
                                                                         e.AddAttribute("MeasureUnit.CodeListVersion", primString)
                                                                             .With(a =>
                                                                                   {
                                                                                       a.Stereotype = Stereotype.SUP.ToString();
                                                                                       a.LowerBound = "1";
                                                                                       a.UpperBound = "*";
                                                                                   });
                                                                     });
            bdtCode = bdtLib1.AddClass("Code").With(e =>
                                                               {
                                                                   e.Stereotype = Stereotype.BDT;
                                                                   e.AddCON(primString);
                                                                   e.AddSUPs(primString, "Name", "CodeList.Agency", "CodeList.AgencyName", "CodeList", "CodeList.Name", "CodeList.UniformResourceIdentifier", "CodeList.Version", "CodeListScheme.UniformResourceIdentifier", "Language");
                                                               });
            bdtABCCode = bdtLib1.AddClass("ABC_Code").With(e =>
                                                                      {
                                                                          e.Stereotype = Stereotype.BDT;
                                                                          e.AddCON(primString);
                                                                          e.AddSUPs(primString, "Name", "CodeList.Agency", "CodeList.AgencyName", "CodeList", "CodeList.Name", "CodeList.UniformResourceIdentifier", "CodeList.Version", "CodeListScheme.UniformResourceIdentifier", "Language");
                                                                      });
            bdtDate = bdtLib1.AddClass("Date").With(e =>
                                                               {
                                                                   e.Stereotype = Stereotype.BDT;
                                                                   e.AddCON(primString);
                                                                   e.AddSUPs(primString, "Format");
                                                               });
            bdtText = bdtLib1.AddClass("Text").With(e =>
                                                               {
                                                                   e.Stereotype = Stereotype.BDT;
                                                                   e.AddTaggedValue(TaggedValues.definition.ToString()).WithValue("This is the definition of BDT Text.");
                                                                   e.AddCON(primString);
                                                                   e.AddSUPs(primString, "Language", "Language.Locale");
                                                               });
            AddInvalidElement(bdtLib1);
        }

        private void InitCCLib1(Package ccLib1)
        {
            ccLib1.Element.Stereotype = Stereotype.CCLibrary;
            ccLib1.AddDiagram("cclib1", "Class");
            ccLib1.AddTaggedValue(TaggedValues.baseURN.ToString()).WithValue("urn:test:blib1:cclib1");
            accAddress = ccLib1.AddClass("Address").With(
                ElementStereotype(Stereotype.ACC),
                BCCs(cdtText, "CountryName", "CityName", "StreetName", "StreetNumber"),
                e => e.AddBCC(cdtText, "Postcode").With(postcode =>
                                                        {
                                                            postcode.LowerBound = "0";
                                                            postcode.UpperBound = "*";
                                                        }));
            accPerson = ccLib1.AddClass("Person").With(
                ElementStereotype(Stereotype.ACC),
                BCCs(cdtText, "FirstName", "LastName"),
                e => e.AddBCC(cdtText, "NickName").With(nickName =>
                                                        {
                                                            nickName.LowerBound = "0";
                                                            nickName.UpperBound = "*";
                                                        }));
            AddInvalidElement(ccLib1);
        }

        private void InitBIELib1(Package bieLib1)
        {
            bieLib1.Element.Stereotype = Stereotype.BIELibrary;
            bieLib1.AddDiagram("bielib1", "Class");
            bieLib1.AddTaggedValue(TaggedValues.baseURN.ToString()).WithValue("urn:test:blib1:bielib1");
            bieMyAddress = bieLib1.AddClass("MyAddress").With(
                ElementStereotype(Stereotype.ABIE),
                BBIEs(bdtText, "CountryName", "CityName", "StreetName", "StreetNumber"),
                e => e.AddBBIE(bdtText, "Postcode").With(postcode =>
                                                               {
                                                                   postcode.LowerBound = "0";
                                                                   postcode.UpperBound = "*";
                                                               }));
            bieMyPerson = bieLib1.AddClass("MyPerson").With(ElementStereotype(Stereotype.ABIE), BBIEs(bdtText, "FirstName", "AnotherFirstName", "LastName"));            
            AddInvalidElement(bieLib1);
        }

        private void InitDOCLibrary(Package docLibrary)
        {
            docLibrary.Element.Stereotype = Stereotype.DOCLibrary;
            docLibrary.AddDiagram("DOCLibrary", "Class");
            docLibrary.AddTaggedValue(TaggedValues.baseURN.ToString()).WithValue("urn:test:blib1:doclibrary");
            bieInvoice = docLibrary.AddClass("Invoice").With(ElementStereotype(Stereotype.ABIE), BBIEs(bdtText, "Amount"));
            bieInvoiceInfo = docLibrary.AddClass("InvoiceInfo").With(ElementStereotype(Stereotype.ABIE), BBIEs(bdtText, "Info"));
            AddInvalidElement(docLibrary);
        }

        private static void AddInvalidElement(Package package)
        {
            package.AddClass("InvalidElement").With(e => { e.Stereotype = "InvalidStereotype"; });
        }

        #region Paths

        public static Path PathToDate()
        {
            return (Path) "test model"/"blib1"/"cdtlib1"/"Date";
        }

        public static Path PathToCode()
        {
            return (Path) "test model"/"blib1"/"cdtlib1"/"Code";
        }

        public static Path PathToText()
        {
            return (Path) "test model"/"blib1"/"cdtlib1"/"Text";
        }

        public static Path PathToBDTText()
        {
            return (Path) "test model"/"blib1"/"bdtlib1"/"Text";
        }

        public static Path PathToBDTCode()
        {
            return (Path) "test model"/"blib1"/"bdtlib1"/"Code";
        }

        public static Path PathToBdtAbcCode()
        {
            return (Path) "test model"/"blib1"/"bdtlib1"/"ABC_Code";
        }

        public static Path PathToBdtDate()
        {
            return (Path) "test model"/"blib1"/"bdtlib1"/"Date";
        }

        public static Path PathToBdtMeasure()
        {
            return (Path) "test model"/"blib1"/"bdtlib1"/"Measure";
        }

        public static Path PathToEnumAbcCodes()
        {
            return (Path) "test model"/"blib1"/"enumlib1"/"ABC_Codes";
        }

        public static Path PathToDecimal()
        {
            return (Path) "test model"/"blib1"/"primlib1"/"Decimal";
        }

        public static Path PathToString()
        {
            return (Path) "test model"/"blib1"/"primlib1"/"String";
        }

        public static Path PathToAddress()
        {
            return (Path) "test model"/"blib1"/"cclib1"/"Address";
        }

        public static Path PathToACCPerson()
        {
            return (Path) "test model"/"blib1"/"cclib1"/"Person";
        }

        public static Path PathToBIEAddress()
        {
            return (Path) "test model"/"blib1"/"bielib1"/"MyAddress";
        }

        public static Path PathToBIEPerson()
        {
            return (Path) "test model"/"blib1"/"bielib1"/"MyPerson";
        }

        public static Path PathToInvoice()
        {
            return (Path) "test model"/"blib1"/"DOCLibrary"/"Invoice";
        }

        public static Path PathToInvoiceInfo()
        {
            return (Path) "test model"/"blib1"/"DOCLibrary"/"InvoiceInfo";
        }

        #endregion 
    }
}