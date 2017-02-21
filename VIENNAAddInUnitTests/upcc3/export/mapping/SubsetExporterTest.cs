using System.Collections.Generic;
using CctsRepository;
using CctsRepository.DocLibrary;
using EA;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using VIENNAAddIn;
using VIENNAAddIn.upcc3;
using VIENNAAddIn.upcc3.ea;
using VIENNAAddIn.upcc3.export.mapping;
using VIENNAAddInUnitTests.TestRepository;
using VIENNAAddInUtils;
using Stereotype=VIENNAAddIn.upcc3.Stereotype;

namespace VIENNAAddInUnitTests.upcc3.export.mapping
{
    [TestFixture]
    public class SubsetExporterTest
    {
        [Test]
        public void Test_model_diff()
        {
            EARepository eaRepository = new EARepository();
            Element cdtText = null;
            Element bdtStringText = null;
            Element primString = null;
            Element accAddress = null;
            Element accPerson = null;
            Element abieAddressTypeAddress = null;
            Element abieAddressTypePerson = null;

            #region EA Repository Complete

            eaRepository.AddModel(
                "Test Model Complete", m => m.AddPackage("bLibrary", bLibrary =>
                {
                    bLibrary.Element.Stereotype = Stereotype.bLibrary;

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
                            e.AddSUPs(primString, "Language", "LanguageLocale");
                        });
                    });

                    bLibrary.AddPackage("CCLibrary", package =>
                    {
                        package.Element.Stereotype = Stereotype.CCLibrary;

                        accAddress = package.AddClass("Address")
                            .With(e => e.Stereotype = Stereotype.ACC)
                            .With(e => e.AddBCCs(cdtText, "StreetName", "CityName", "AttentionOf"));

                        accPerson = package.AddClass("Person")
                            .With(e => e.Stereotype = Stereotype.ACC)
                            .With(e => e.AddBCCs(cdtText, "Name", "Title", "Salutation"));
                    });

                    bLibrary.AddPackage("BDTLibrary", package =>
                    {
                        package.Element.Stereotype = Stereotype.BDTLibrary;
                        bdtStringText = package.AddBDT("String_Text").With(e =>
                        {
                            e.Stereotype = Stereotype.BDT;
                            e.AddBasedOnDependency(cdtText);
                            e.AddCON(primString);
                        });
                    });


                    bLibrary.AddPackage("BIELibrary", package =>
                    {
                        package.Element.Stereotype = Stereotype.BIELibrary;
                        abieAddressTypeAddress = package.AddClass("AddressType_Address").With(e =>
                        {
                            e.Stereotype = Stereotype.ABIE;
                            e.AddBasedOnDependency(accAddress);
                            e.AddBBIE(bdtStringText, "Town_CityName");
                        });
                        abieAddressTypePerson = package.AddClass("AddressType_Person").With(e =>
                        {
                            e.Stereotype = Stereotype.ABIE;
                            e.AddBasedOnDependency(accPerson);
                            e.AddBBIE(bdtStringText, "PersonName_Name");
                        });
                    });

                    bLibrary.AddPackage("DOCLibrary", package =>
                    {
                        package.Element.Stereotype = Stereotype.DOCLibrary;

                        Element maAddressType = package.AddClass("AddressType").With(e =>
                        {
                            e.Stereotype = Stereotype.MA;
                            e.AddASMA(abieAddressTypeAddress, "Address");
                            e.AddASMA(abieAddressTypePerson, "Person");
                        });

                        Element maInvoiceType = package.AddClass("InvoiceType").With(e =>
                        {
                            e.Stereotype = Stereotype.MA;
                            e.AddASMA(maAddressType, "Address");
                        });

                        package.AddClass("Test_Invoice").With(e =>
                        {
                            e.Stereotype = Stereotype.MA;
                            e.AddASMA(maInvoiceType, "Invoice");
                        });


                    });
                }));

            #endregion

            #region EA Repository Subset

            eaRepository.AddModel(
                "Test Model Subset", m => m.AddPackage("bLibrary", bLibrary =>
                {
                    bLibrary.Element.Stereotype = Stereotype.bLibrary;

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
                            e.AddSUPs(primString, "Language", "LanguageLocale");
                        });
                    });

                    bLibrary.AddPackage("CCLibrary", package =>
                    {
                        package.Element.Stereotype = Stereotype.CCLibrary;

                        accAddress = package.AddClass("Address")
                            .With(e => e.Stereotype = Stereotype.ACC)
                            .With(e => e.AddBCCs(cdtText, "StreetName", "CityName", "AttentionOf"));

                        accPerson = package.AddClass("Person")
                            .With(e => e.Stereotype = Stereotype.ACC)
                            .With(e => e.AddBCCs(cdtText, "Name", "Title", "Salutation"));
                    });

                    bLibrary.AddPackage("BDTLibrary", package =>
                    {
                        package.Element.Stereotype = Stereotype.BDTLibrary;
                        bdtStringText = package.AddBDT("String_Text").With(e =>
                        {
                            e.Stereotype = Stereotype.BDT;
                            e.AddBasedOnDependency(cdtText);
                            e.AddCON(primString);
                        });
                    });


                    bLibrary.AddPackage("BIELibrary", package =>
                    {
                        package.Element.Stereotype = Stereotype.BIELibrary;
                        abieAddressTypeAddress = package.AddClass("AddressType_Address").With(e =>
                        {
                            e.Stereotype = Stereotype.ABIE;
                            e.AddBasedOnDependency(accAddress);
                            e.AddBBIE(bdtStringText, "Town_CityName");
                        });
                        abieAddressTypePerson = package.AddClass("AddressType_Person").With(e =>
                        {
                            e.Stereotype = Stereotype.ABIE;
                            e.AddBasedOnDependency(accPerson);
                        });
                    });

                    bLibrary.AddPackage("DOCLibrary", package =>
                    {
                        package.Element.Stereotype = Stereotype.DOCLibrary;

                        Element maAddressType = package.AddClass("AddressType").With(e =>
                        {
                            e.Stereotype = Stereotype.MA;
                            e.AddASMA(abieAddressTypeAddress, "Address");
                            e.AddASMA(abieAddressTypePerson, "Person");
                        });

                        Element maInvoiceType = package.AddClass("InvoiceType").With(e =>
                        {
                            e.Stereotype = Stereotype.MA;
                            e.AddASMA(maAddressType, "Address");
                        });

                        package.AddClass("Test_Invoice").With(e =>
                        {
                            e.Stereotype = Stereotype.MA;
                            e.AddASMA(maInvoiceType, "Invoice");
                        });


                    });
                }));

            #endregion

            ICctsRepository cctsRepository = CctsRepositoryFactory.CreateCctsRepository(eaRepository);

            IDocLibrary docLibraryComplete = cctsRepository.GetDocLibraryByPath((Path)"Test Model Complete" / "bLibrary" / "DOCLibrary");
            IDocLibrary docLibrarySubset = cctsRepository.GetDocLibraryByPath((Path)"Test Model Subset" / "bLibrary" / "DOCLibrary");

            Dictionary<string, List<string>> mutatedComplexTypes = new UpccModelDiff(docLibraryComplete, docLibrarySubset).CalculateDiff();

            Dictionary<string, List<string>> expectedMutatedComplexTypes = new Dictionary<string, List<string>>();
            expectedMutatedComplexTypes.Add("AddressType", new List<string> { "PersonName" });

            Assert.That(mutatedComplexTypes.Keys, Is.EquivalentTo(expectedMutatedComplexTypes.Keys), "Defective Complex Type Mutation.");

            foreach (string complexTypeName in expectedMutatedComplexTypes.Keys)
            {
                Assert.That(mutatedComplexTypes[complexTypeName], Is.EquivalentTo(expectedMutatedComplexTypes[complexTypeName]), "Difference between Complex Type Mutation and expected Complex Type Mutation: " + complexTypeName + ".");
            }
        }

        [Test]
        public void Test_calculate_remaining_xsd_types_from_document_model_with_cycle()
        {
            EARepository eaRepository = new EARepository();
            Element bdtStringText = null;
            Element primString = null;
            Element abiePersonTypePerson = null;
            Element abiePersonTypeNote = null;

            #region EA Repository Subset

            eaRepository.AddModel(
                "Test Model Subset", m => m.AddPackage("bLibrary", bLibrary =>
                {
                    bLibrary.Element.Stereotype = Stereotype.bLibrary;

                    bLibrary.AddPackage("PRIMLibrary", package =>
                    {
                        package.Element.Stereotype = Stereotype.PRIMLibrary;

                        primString = package.AddPRIM("String");
                    });

                    bLibrary.AddPackage("BDTLibrary", package =>
                    {
                        package.Element.Stereotype = Stereotype.BDTLibrary;
                        bdtStringText = package.AddBDT("String_Text").With(e =>
                        {
                            e.Stereotype = Stereotype.BDT;
                            e.AddCON(primString);
                        });
                    });


                    bLibrary.AddPackage("BIELibrary", package =>
                    {
                        package.Element.Stereotype = Stereotype.BIELibrary;
                        Element abieAddressTypeAddress = package.AddClass("AddressType_Address").With(e =>
                                                                                                          {
                                                                                                              e.Stereotype = Stereotype.ABIE;
                                                                                                              e.AddBBIE(bdtStringText, "City_CityName");
                                                                                                          });
                        abiePersonTypePerson = package.AddClass("PersonType_Person").With(e =>
                        {
                            e.Stereotype = Stereotype.ABIE;
                            e.AddBBIE(bdtStringText, "Name_Name");
                            e.AddASBIE(abieAddressTypeAddress, "Address_Residence", EaAggregationKind.Composite);
                        });
                        abiePersonTypePerson.AddASBIE(abiePersonTypePerson, "Child_Child", EaAggregationKind.Shared);

                        abiePersonTypeNote = package.AddClass("PersonType_Note").With(e =>
                        {
                            e.Stereotype = Stereotype.ABIE;
                        });

                    });

                    bLibrary.AddPackage("DOCLibrary", package =>
                    {
                        package.Element.Stereotype = Stereotype.DOCLibrary;

                        Element maPersonType = package.AddClass("PersonType").With(e =>
                        {
                            e.Stereotype = Stereotype.MA;
                            e.AddASMA(abiePersonTypePerson, "Person");
                            e.AddASMA(abiePersonTypeNote, "Note");
                        });

                        Element maPassportType = package.AddClass("PassportType").With(e =>
                        {
                            e.Stereotype = Stereotype.MA;
                            e.AddASMA(maPersonType, "Person");
                        });

                        package.AddClass("Austrian_Passport").With(e =>
                        {
                            e.Stereotype = Stereotype.MA;
                            e.AddASMA(maPassportType, "Passport");
                        });
                    });
                }));

            #endregion

            ICctsRepository cctsRepository = CctsRepositoryFactory.CreateCctsRepository(eaRepository);

            IDocLibrary docLibrarySubset = cctsRepository.GetDocLibraryByPath((Path)"Test Model Subset" / "bLibrary" / "DOCLibrary");

            UpccModelXsdTypes existingXsdTypes = new UpccModelXsdTypes(docLibrarySubset);

            // Positive Assertions
            Assert.That(existingXsdTypes.Count, Is.EqualTo(4));

            Assert.That(existingXsdTypes.ContainsXsdType("String"), Is.True);
            Assert.That(existingXsdTypes.ContainsXsdType("AddressType"), Is.True);
            Assert.That(existingXsdTypes.ContainsXsdType("PersonType"), Is.True);
            Assert.That(existingXsdTypes.ContainsXsdType("PassportType"), Is.True);

            Assert.That(existingXsdTypes.NumberOfChildren("String"), Is.EqualTo(0));
            Assert.That(existingXsdTypes.NumberOfChildren("AddressType"), Is.EqualTo(1));
            Assert.That(existingXsdTypes.NumberOfChildren("PersonType"), Is.EqualTo(3));
            Assert.That(existingXsdTypes.NumberOfChildren("PassportType"), Is.EqualTo(1));

            Assert.That(existingXsdTypes.XsdTypeContainsChild("AddressType", "City"), Is.True);
            Assert.That(existingXsdTypes.XsdTypeContainsChild("PersonType", "Name"), Is.True);
            Assert.That(existingXsdTypes.XsdTypeContainsChild("PersonType", "Address"), Is.True);
            Assert.That(existingXsdTypes.XsdTypeContainsChild("PersonType", "Child"), Is.True);
            Assert.That(existingXsdTypes.XsdTypeContainsChild("PassportType", "Person"), Is.True);

            //Negative Assertions
            Assert.That(existingXsdTypes.ContainsXsdType("SuperDooperNonExistingType"), Is.False);

            Assert.That(existingXsdTypes.NumberOfChildren("SuperDooperNonExistingType"), Is.EqualTo(0));

            Assert.That(existingXsdTypes.XsdTypeContainsChild("AddressType", "NonExistingChild"), Is.False);
        }

        [Test]
        public void Test_calculate_remaining_xsd_types_from_document_model()
        {
            EARepository eaRepository = new EARepository();
            Element bdtStringText = null;
            Element primString = null;
            Element abiePersonTypePerson = null;
            Element abiePersonTypeNote = null;

            #region EA Repository Subset

            eaRepository.AddModel(
                "Test Model Subset", m => m.AddPackage("bLibrary", bLibrary =>
                {
                    bLibrary.Element.Stereotype = Stereotype.bLibrary;

                    bLibrary.AddPackage("PRIMLibrary", package =>
                    {
                        package.Element.Stereotype = Stereotype.PRIMLibrary;

                        primString = package.AddPRIM("String");
                    });

                    bLibrary.AddPackage("BDTLibrary", package =>
                    {
                        package.Element.Stereotype = Stereotype.BDTLibrary;
                        bdtStringText = package.AddBDT("String_Text").With(e =>
                        {
                            e.Stereotype = Stereotype.BDT;
                            e.AddCON(primString);
                        });
                    });


                    bLibrary.AddPackage("BIELibrary", package =>
                    {
                        package.Element.Stereotype = Stereotype.BIELibrary;
                        Element abieAddressTypeAddress = package.AddClass("AddressType_Address").With(e =>
                                                                                                          {
                                                                                                              e.Stereotype = Stereotype.ABIE;
                                                                                                              e.AddBBIE(bdtStringText, "City_CityName");
                                                                                                          });
                        abiePersonTypePerson = package.AddClass("PersonType_Person").With(e =>
                        {
                            e.Stereotype = Stereotype.ABIE;
                            e.AddBBIE(bdtStringText, "Name_Name");
                            e.AddASBIE(abieAddressTypeAddress, "Address_Residence", EaAggregationKind.Composite);
                        });
                        abiePersonTypeNote = package.AddClass("PersonType_Note").With(e =>
                        {
                            e.Stereotype = Stereotype.ABIE;
                        });

                    });

                    bLibrary.AddPackage("DOCLibrary", package =>
                    {
                        package.Element.Stereotype = Stereotype.DOCLibrary;

                        Element maPersonType = package.AddClass("PersonType").With(e =>
                        {
                            e.Stereotype = Stereotype.MA;
                            e.AddASMA(abiePersonTypePerson, "Person");
                            e.AddASMA(abiePersonTypeNote, "Note");
                        });

                        Element maPassportType = package.AddClass("PassportType").With(e =>
                        {
                            e.Stereotype = Stereotype.MA;
                            e.AddASMA(maPersonType, "Person");
                        });

                        package.AddClass("Austrian_Passport").With(e =>
                        {
                            e.Stereotype = Stereotype.MA;
                            e.AddASMA(maPassportType, "Passport");
                        });
                    });
                }));

            #endregion

            ICctsRepository cctsRepository = CctsRepositoryFactory.CreateCctsRepository(eaRepository);

            IDocLibrary docLibrarySubset = cctsRepository.GetDocLibraryByPath((Path)"Test Model Subset" / "bLibrary" / "DOCLibrary");

            UpccModelXsdTypes existingXsdTypes = new UpccModelXsdTypes(docLibrarySubset);

            // Positive Assertions
            Assert.That(existingXsdTypes.Count, Is.EqualTo(4));

            Assert.That(existingXsdTypes.ContainsXsdType("String"), Is.True);
            Assert.That(existingXsdTypes.ContainsXsdType("AddressType"), Is.True);
            Assert.That(existingXsdTypes.ContainsXsdType("PersonType"), Is.True);
            Assert.That(existingXsdTypes.ContainsXsdType("PassportType"), Is.True);

            Assert.That(existingXsdTypes.NumberOfChildren("String"), Is.EqualTo(0));
            Assert.That(existingXsdTypes.NumberOfChildren("AddressType"), Is.EqualTo(1));
            Assert.That(existingXsdTypes.NumberOfChildren("PersonType"), Is.EqualTo(2));
            Assert.That(existingXsdTypes.NumberOfChildren("PassportType"), Is.EqualTo(1));

            Assert.That(existingXsdTypes.XsdTypeContainsChild("AddressType", "City"), Is.True);
            Assert.That(existingXsdTypes.XsdTypeContainsChild("PersonType", "Name"), Is.True);
            Assert.That(existingXsdTypes.XsdTypeContainsChild("PersonType", "Address"), Is.True);
            Assert.That(existingXsdTypes.XsdTypeContainsChild("PassportType", "Person"), Is.True);

            //Negative Assertions
            Assert.That(existingXsdTypes.ContainsXsdType("SuperDooperNonExistingType"), Is.False);

            Assert.That(existingXsdTypes.NumberOfChildren("SuperDooperNonExistingType"), Is.EqualTo(0));

            Assert.That(existingXsdTypes.XsdTypeContainsChild("AddressType", "NonExistingChild"), Is.False);
        }

        [Test]
        public void Test_exporting_subset_of_complex_type_mapped_to_multiple_accs()
        {
            string schemaFileComplete = TestUtils.PathToTestResource(@"XSDExporterTest\mapping\SubsetExporter\exporting_subset_of_complex_type_mapped_to_multiple_accs\source.xsd");
            string schemaFileSubset = TestUtils.PathToTestResource(@"XSDExporterTest\mapping\SubsetExporter\exporting_subset_of_complex_type_mapped_to_multiple_accs\subset\");            

            EARepository eaRepository = new EARepository();
            Element bdtStringText = null;
            Element primString = null;            
            Element abiePersonTypePerson = null;
            Element abiePersonTypeNote = null;

            #region EA Repository Subset

            eaRepository.AddModel(
                "Test Model Subset", m => m.AddPackage("bLibrary", bLibrary =>
                {
                    bLibrary.Element.Stereotype = Stereotype.bLibrary;

                    bLibrary.AddPackage("PRIMLibrary", package =>
                    {
                        package.Element.Stereotype = Stereotype.PRIMLibrary;

                        primString = package.AddPRIM("String");
                    });

                    bLibrary.AddPackage("BDTLibrary", package =>
                    {
                        package.Element.Stereotype = Stereotype.BDTLibrary;
                        bdtStringText = package.AddBDT("String_Text").With(e =>
                        {
                            e.Stereotype = Stereotype.BDT;                            
                            e.AddCON(primString);
                        });
                    });


                    bLibrary.AddPackage("BIELibrary", package =>
                    {
                        package.Element.Stereotype = Stereotype.BIELibrary;
                        Element abieAddressTypeAddress = package.AddClass("AddressType_Address").With(e =>
                                                                                                          {
                                                                                                              e.Stereotype = Stereotype.ABIE;
                                                                                                              e.AddBBIE(bdtStringText, "City_CityName");
                                                                                                          });
                        abiePersonTypePerson = package.AddClass("PersonType_Person").With(e =>
                        {
                            e.Stereotype = Stereotype.ABIE;
                            e.AddBBIE(bdtStringText, "Name_Name"); 
                            e.AddASBIE(abieAddressTypeAddress, "Address_Residence", EaAggregationKind.Composite);
                        });
                        abiePersonTypeNote = package.AddClass("PersonType_Note").With(e =>
                        {
                            e.Stereotype = Stereotype.ABIE;
                        });

                    });

                    bLibrary.AddPackage("DOCLibrary", package =>
                    {
                        package.Element.Stereotype = Stereotype.DOCLibrary;

                        Element maPersonType = package.AddClass("PersonType").With(e =>
                        {
                            e.Stereotype = Stereotype.MA;
                            e.AddASMA(abiePersonTypePerson, "Person");
                            e.AddASMA(abiePersonTypeNote, "Note");
                        });

                        Element maPassportType = package.AddClass("PassportType").With(e =>
                        {
                            e.Stereotype = Stereotype.MA;
                            e.AddASMA(maPersonType, "Person");
                        });

                        package.AddClass("Austrian_Passport").With(e =>
                        {
                            e.Stereotype = Stereotype.MA;
                            e.AddASMA(maPassportType, "Passport");
                        });
                    });
                }));

            #endregion

            ICctsRepository cctsRepository = CctsRepositoryFactory.CreateCctsRepository(eaRepository);

            IDocLibrary docLibrary = cctsRepository.GetDocLibraryByPath((Path)"Test Model Subset"/"bLibrary"/"DOCLibrary");

            SubsetExporter.ExportSubset(docLibrary, schemaFileComplete, schemaFileSubset);            
        }

        [Test]
        [Ignore("manual test case")]
        public void Test_exporting_subset_of_ebinterface()
        {
            string schemaFileComplete = TestUtils.PathToTestResource(@"XSDExporterTest\mapping\SubsetExporter\exporting_subset_of_ebinterface\Invoice.xsd");
            string schemaDirectorySubset = TestUtils.PathToTestResource(@"XSDExporterTest\mapping\SubsetExporter\exporting_subset_of_ebinterface\subset\");
            string repositoryFile = TestUtils.PathToTestResource(@"XSDExporterTest\mapping\SubsetExporter\exporting_subset_of_ebinterface\Invoice_complete.eap");

            Repository eaRepository = new Repository();
            eaRepository.OpenFile(repositoryFile);
            
            ICctsRepository cctsRepository = CctsRepositoryFactory.CreateCctsRepository(eaRepository);

            IDocLibrary docLibrary = cctsRepository.GetDocLibraryByPath((Path)"Model" / "bLibrary" / "DOCLibrary");

            SubsetExporter.ExportSubset(docLibrary, schemaFileComplete, schemaDirectorySubset);
        }

        [Test]
        [Ignore("manual test case")]
        public void Test_exporting_subset_of_ubl()
        {
            string schemaFileComplete = TestUtils.PathToTestResource(@"XSDExporterTest\mapping\SubsetExporter\exporting_subset_of_ubl\invoice\UBL-Invoice-2.0.xsd");
            string schemaDirectorySubset = TestUtils.PathToTestResource(@"XSDExporterTest\mapping\SubsetExporter\exporting_subset_of_ubl\subset\");
            string repositoryFile = TestUtils.PathToTestResource(@"XSDExporterTest\mapping\SubsetExporter\exporting_subset_of_ubl\Invoice_complete.eap");

            Repository eaRepository = new Repository();
            eaRepository.OpenFile(repositoryFile);
            
            ICctsRepository cctsRepository = CctsRepositoryFactory.CreateCctsRepository(eaRepository);

            IDocLibrary docLibrary = cctsRepository.GetDocLibraryByPath((Path)"Model" / "bLibrary" / "Test DOC Library");

            SubsetExporter.ExportSubset(docLibrary, schemaFileComplete, schemaDirectorySubset);
        }
    }
}