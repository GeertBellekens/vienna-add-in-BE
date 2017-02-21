using EA;
using VIENNAAddIn;
using VIENNAAddIn.upcc3;
using VIENNAAddIn.upcc3.ea;
using Stereotype=VIENNAAddIn.upcc3.Stereotype;

namespace VIENNAAddInUnitTests.TestRepository
{
    internal class EARepository2 : EARepository
    {
        private Element abieAddress;
        private Element abieInvoice;
        private Element abieInvoiceInfo;
        private Element abiePerson;
        private Element accAddress;
        private Element accPerson;
        private Element bdtCode;
        private Element bdtCurrency;
        private Element bdtDate;
        private Element bdtMeasure;
        private Element bdtSimpleString;
        private Element bdtText;
        private Element cdtCode;
        private Element cdtCurrency;
        private Element cdtDate;
        private Element cdtMeasure;
        private Element cdtSimpleString;
        private Element cdtText;
        private Element enumABCCodes;
        private Element primDecimal;
        private Element primString;

        public EARepository2()
        {
            this.AddModel("test model",
                          m => m.AddPackage("bLibrary",
                                            bLibrary =>
                                            {
                                                bLibrary.Element.Stereotype = Stereotype.bLibrary;
                                                bLibrary.AddTaggedValue(TaggedValues.baseURN.ToString()).WithValue("urn:test:blib1");
                                                bLibrary.AddPackage("PRIMLibrary", InitPRIMLibrary);
                                                bLibrary.AddPackage("ENUMLibrary", InitENUMLibrary);
                                                bLibrary.AddPackage("CDTLibrary", InitCDTLibrary);
                                                bLibrary.AddPackage("BDTLibrary", InitBDTLibrary);
                                                bLibrary.AddPackage("CCLibrary", InitCCLibrary);
                                                bLibrary.AddPackage("BIELibrary", InitBIELibrary);
                                                bLibrary.AddPackage("DOCLibrary", InitDOCLibrary);
                                            }));

            bdtSimpleString.AddBasedOnDependency(cdtSimpleString);
            bdtCurrency.AddBasedOnDependency(cdtCurrency);
            bdtMeasure.AddBasedOnDependency(cdtMeasure);
            bdtCode.AddBasedOnDependency(cdtCode);
            bdtDate.AddBasedOnDependency(cdtDate);
            bdtText.AddBasedOnDependency(cdtText);
            abieAddress.AddBasedOnDependency(accAddress);

            accPerson.AddASCC(accAddress, "homeAddress");
            accPerson.AddASCC(accAddress, "workAddress", "0", "*");

            abiePerson.AddASBIE(abieAddress, "homeAddress", EaAggregationKind.Shared);
            abiePerson.AddASBIE(abieAddress, "workAddress", EaAggregationKind.Composite, "0", "*");
            abieInvoice.AddASBIE(abieInvoiceInfo, "info", EaAggregationKind.Shared);
            abieInvoiceInfo.AddASBIE(abieAddress, "deliveryAddress", EaAggregationKind.Shared);
        }

        private void InitDOCLibrary(Package docLibrary)
        {
            docLibrary.Element.Stereotype = Stereotype.DOCLibrary;
            docLibrary.AddDiagram("DOCLibrary", "Class");
            docLibrary.AddTaggedValue(TaggedValues.baseURN.ToString()).WithValue("urn:test:blib1:doclibrary");
            abieInvoice = docLibrary.AddABIE("Invoice").With(BBIEs(bdtCurrency, "Amount"));
            abieInvoiceInfo = docLibrary.AddABIE("InvoiceInfo").With(BBIEs(bdtText, "Info"));
        }

        private void InitBIELibrary(Package bieLibrary)
        {
            bieLibrary.Element.Stereotype = Stereotype.BIELibrary;
            bieLibrary.AddDiagram("BIELibrary", "Class");
            bieLibrary.AddTaggedValue(TaggedValues.baseURN.ToString()).WithValue("urn:test:blib1:bielib1");
            abieAddress = bieLibrary.AddABIE("Address").With(BBIEs(bdtText, "CountryName", "CityName", "StreetName", "StreetNumber", "Postcode"));
            abiePerson = bieLibrary.AddABIE("Person").With(BBIEs(bdtText, "FirstName", "LastName"));
        }

        private void InitCCLibrary(Package ccLibrary)
        {
            ccLibrary.Element.Stereotype = Stereotype.CCLibrary;
            ccLibrary.AddDiagram("CCLibrary", "Class");
            ccLibrary.AddTaggedValue(TaggedValues.baseURN.ToString()).WithValue("urn:test:blib1:cclib1");
            accAddress = ccLibrary.AddACC("Address").With(BCCs(cdtText, "CountryName", "CityName", "StreetName", "StreetNumber", "Postcode"));
            accPerson = ccLibrary.AddACC("Person").With(BCCs(cdtText, "FirstName", "LastName"),
                                                        e => e.AddBCC(cdtText, "NickName").With(nickName =>
                                                                                                {
                                                                                                    nickName.LowerBound = "0";
                                                                                                    nickName.UpperBound = "*";
                                                                                                }));
        }

        private void InitBDTLibrary(Package bdtLibrary)
        {
            bdtLibrary.Element.Stereotype = Stereotype.BDTLibrary;
            bdtLibrary.AddDiagram("BDTLibrary", "Class");
            bdtLibrary.AddTaggedValue(TaggedValues.baseURN.ToString()).WithValue("urn:test:blib1:bdtlib1");
            bdtSimpleString = bdtLibrary.AddBDT("SimpleString").With(CON(primString));
            bdtText = bdtLibrary.AddBDT("Text").With(CON(primString),
                                                     SUPs(primString, "Language", "Language.Locale"),
                                                     TaggedValue(TaggedValues.uniqueIdentifier, "234235235"),
                                                     TaggedValue(TaggedValues.versionIdentifier, "1.0"),
                                                     TaggedValue(TaggedValues.definition, "This is the definition of BDT Text."),
                                                     TaggedValue(TaggedValues.definition, "business term 1|business term 2"));
            bdtDate = bdtLibrary.AddBDT("Date").With(CON(primString),
                                                     SUPs(primString, "Format"));
            bdtCode = bdtLibrary.AddBDT("Code").With(CON(enumABCCodes),
                                                     SUPs(primString,
                                                          "Name",
                                                          "CodeList.Agency",
                                                          "CodeList.AgencyName",
                                                          "CodeList",
                                                          "CodeList.Name",
                                                          "CodeList.UniformResourceIdentifier",
                                                          "CodeList.Version",
                                                          "CodeListScheme.UniformResourceIdentifier",
                                                          "Language"));
            bdtMeasure = bdtLibrary.AddBDT("Measure").With(CON(primDecimal),
                                                           SUPs(primString, "MeasureUnit"),
                                                           e => e.AddSUP(primString, "MeasureUnit.CodeListVersion").With(a =>
                                                                                                                         {
                                                                                                                             a.LowerBound = "1";
                                                                                                                             a.UpperBound = "*";
                                                                                                                         }));
            bdtCurrency = bdtLibrary.AddBDT("Currency").With(CON(primDecimal),
                                                             SUPs(primString, "CurrencyCode"));
        }

        private void InitCDTLibrary(Package cdtLibrary)
        {
            cdtLibrary.Element.Stereotype = Stereotype.CDTLibrary;
            cdtLibrary.AddTaggedValue(TaggedValues.baseURN.ToString()).WithValue("urn:test:blib1:cdtlibrary");
            cdtLibrary.AddDiagram("CDTLibrary", "Class");
            cdtSimpleString = cdtLibrary.AddCDT("SimpleString").With(CON(primString));
            cdtText = cdtLibrary.AddCDT("Text").With(CON(primString),
                                                     SUPs(primString, "Language", "Language.Locale"));
            cdtDate = cdtLibrary.AddCDT("Date").With(CON(primString),
                                                     SUPs(primString, "Format"),
                                                     TaggedValue(TaggedValues.definition, "A Date."));
            cdtCode = cdtLibrary.AddCDT("Code").With(CON(primString),
                                                     SUPs(primString,
                                                          "Name",
                                                          "CodeList.Agency",
                                                          "CodeList.AgencyName",
                                                          "CodeList",
                                                          "CodeList.Name",
                                                          "CodeList.UniformResourceIdentifier",
                                                          "CodeList.Version",
                                                          "CodeListScheme.UniformResourceIdentifier",
                                                          "Language"));
            cdtMeasure = cdtLibrary.AddCDT("Measure").With(CON(primDecimal),
                                                           SUPs(primString, "MeasureUnit"),
                                                           cdt => cdt.AddSUP(primString, "MeasureUnit.CodeListVersion").With(a =>
                                                                                                                             {
                                                                                                                                 a.LowerBound = "1";
                                                                                                                                 a.UpperBound = "*";
                                                                                                                             }));
            cdtCurrency = cdtLibrary.AddCDT("Currency").With(CON(primDecimal),
                                                             SUPs(primString, "CurrencyCode"));
        }

        private void InitENUMLibrary(Package enumLibrary)
        {
            enumLibrary.Element.Stereotype = Stereotype.ENUMLibrary;
            enumLibrary.AddTaggedValue(TaggedValues.baseURN.ToString()).WithValue("urn:test:blib1:enumlibrary");
            enumLibrary.AddDiagram("ENUMLibrary", "Class");
            enumABCCodes = enumLibrary.AddENUM("ABCCodes", primString,
                                               new[] {"ABC Code 1", "abc1", "status"},
                                               new[] {"ABC Code 2", "abc2", "status"},
                                               new[] {"ABC Code 3", "abc3", "status"});
        }

        private void InitPRIMLibrary(Package primLibrary)
        {
            primLibrary.Element.Stereotype = Stereotype.PRIMLibrary;
            primLibrary.AddTaggedValue(TaggedValues.baseURN.ToString()).WithValue("urn:test:blib1:primlibrary");
            primLibrary.AddDiagram("PRIMLibrary", "Class");
            primString = AddPRIM(primLibrary, "String", "A sequence of characters in some suitable character set.");
            primDecimal = AddPRIM(primLibrary, "Decimal", "A subset of the real numbers, which can be represented by decimal numerals.");
            AddPRIM(primLibrary, "Binary", "A set of (in)finite-length sequences of binary digits.");
            AddPRIM(primLibrary, "Boolean", "A logical expression consisting of predefined values.");
            AddPRIM(primLibrary, "Date", "A point in time to a common resolution (year, month, day, hour,...).");
            AddPRIM(primLibrary, "Integer", "An element in the infinite set (...-2,-1,0,1,...).");
        }

        private static Element AddPRIM(Package primLibrary, string name, string definition)
        {
            return primLibrary.AddPRIM(name).With(TaggedValue(TaggedValues.businessTerm, name),
                                                  TaggedValue(TaggedValues.definition, definition),
                                                  TaggedValue(TaggedValues.dictionaryEntryName, name));
        }
    }
}