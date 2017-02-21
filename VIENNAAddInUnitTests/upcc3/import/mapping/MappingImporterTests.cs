using System;
using System.Collections.Generic;
using System.Linq;
using CctsRepository;
using CctsRepository.BdtLibrary;
using CctsRepository.BieLibrary;
using CctsRepository.BLibrary;
using CctsRepository.CcLibrary;
using CctsRepository.DocLibrary;
using EA;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using VIENNAAddIn.upcc3;
using VIENNAAddIn.upcc3.import.mapping;
using VIENNAAddInUtils;
using File=System.IO.File;

namespace VIENNAAddInUnitTests.upcc3.import.mapping
{
    [TestFixture]
    public class MappingImporterTests
    {
        #region Setup/Teardown

        [SetUp]
        public void Context()
        {
            cctsRepository = CctsRepositoryFactory.CreateCctsRepository(new MappingTestRepository());
            ccLibrary = cctsRepository.GetCcLibraries().FirstOrDefault();
            bLibrary = ccLibrary.BLibrary;
        }

        #endregion

        private const string DocLibraryName = "Test DOC Library";
        private const string BieLibraryName = "Test BIE Library";
        private const string BdtLibraryName = "Test BDT Library";
        private const string Qualifier = "test";
        private const string RootElementName = "Invoice";

        private ICctsRepository cctsRepository;
        private ICcLibrary ccLibrary;
        private IBLibrary bLibrary;

        #region Helpers

        /// <summary>
        /// Returns an array of the names of all SUPs of the given BDT.
        /// </summary>
        /// <param name="bdt"></param>
        /// <returns></returns>
        private static string[] SupNames(IBdt bdt)
        {
            return (from sup in bdt.Sups select sup.Name).ToArray();
        }

        
        /// <summary>
        /// Returns an array of the names of all BBIEs of the given ABIE.
        /// </summary>
        /// <param name="abie"></param>
        /// <returns></returns>
        private static BbieDescriptor[] BbieDescriptors(IAbie abie)
        {
            return (from bbie in abie.Bbies select new BbieDescriptor(bbie.Name, bbie.Bdt.Id)).ToArray();
        }

        /// <summary>
        /// Returns an array of the associated element IDs of all ASBIEs of the given ABIE.
        /// </summary>
        /// <param name="abie"></param>
        /// <returns></returns>
        private static AsbieDescriptor[] AsbieDescriptors(IAbie abie)
        {
            return (from asbie in abie.Asbies select new AsbieDescriptor(asbie.Name, asbie.AssociatedAbie.Id)).ToArray();
        }

        /// <summary>
        /// Returns an array of the associated element IDs of all ASMAs of the given MA.
        /// </summary>
        /// <param name="ma"></param>
        /// <returns></returns>
        private static AsmaDescriptor[] AsmaDescriptors(IMa ma)
        {
            return (from asma in ma.Asmas select new AsmaDescriptor(asma.Name, asma.AssociatedBieAggregator.Id)).ToArray();
        }

        private IBdtLibrary ShouldContainBdtLibrary(string bdtLibraryName)
        {
            var library = cctsRepository.GetBdtLibraryByPath((Path)"test" / "bLibrary" / bdtLibraryName);
            Assert.That(library, Is.Not.Null, "BDTLibrary '" + bdtLibraryName + "' not generated");
            return library;
        }
 

        private IBieLibrary ShouldContainBieLibrary(string name)
        {
            var library = cctsRepository.GetBieLibraryByPath((Path) "test"/"bLibrary"/name);
            Assert.That(library, Is.Not.Null, "BIELibrary '" + name + "' not generated");
            return library;
        }

        private IDocLibrary ShouldContainDocLibrary(string name)
        {
            var library = cctsRepository.GetDocLibraryByPath((Path) "test"/"bLibrary"/name);
            Assert.That(library, Is.Not.Null, "DOCLibrary '" + name + "' not generated");
            return library;
        }

        private static IBdt ShouldContainBdt(IBdtLibrary bdtLibrary, string name, string cdtName, string[] generatedSups)
        {
            IBdt bdt = bdtLibrary.GetBdtByName(name);
            Assert.IsNotNull(bdt, "BDT '" + name + "' not generated");

            Assert.That(bdt.BasedOn, Is.Not.Null, "BasedOn reference not specified");
            Assert.AreEqual(cdtName, bdt.BasedOn.Name, "BasedOn wrong CDT");

            if (generatedSups == null || generatedSups.Length == 0)
            {
                Assert.That(SupNames(bdt), Is.Empty);
            }
            else
            {
                Assert.That(SupNames(bdt), Is.EquivalentTo(generatedSups));
            }

            return bdt;
        }

        private static IAbie ShouldContainAbie(IBieLibrary bieLibrary, string name, string accName, BbieDescriptor[] bbieDescriptors, AsbieDescriptor[] asbieDescriptors)
        {
            IAbie abie = bieLibrary.GetAbieByName(name);
            Assert.IsNotNull(abie, "ABIE '" + name + "' not generated");

            if (accName != null)
            {
                Assert.That(abie.BasedOn, Is.Not.Null, "BasedOn reference not specified");
                Assert.AreEqual(accName, abie.BasedOn.Name, "BasedOn wrong ACC");
            }
            else
            {
                Assert.That(abie.BasedOn, Is.Null, "Unexpected BasedOn reference to ACC '" + abie.BasedOn + "'");
            }

            if (bbieDescriptors == null || bbieDescriptors.Length == 0)
            {
                Assert.That(BbieDescriptors(abie), Is.Empty);
            }
            else
            {
                Assert.That(BbieDescriptors(abie), Is.EquivalentTo(bbieDescriptors));
            }

            if (asbieDescriptors == null || asbieDescriptors.Length == 0)
            {
                Assert.That(AsbieDescriptors(abie), Is.Empty);
            }
            else
            {
                Assert.That(AsbieDescriptors(abie), Is.EquivalentTo(asbieDescriptors));
            }
            return abie;
        }

        private static IMa ShouldContainMa(IDocLibrary docLibrary, string name, AsmaDescriptor[] asmaDescriptors)
        {
            var ma = docLibrary.GetMaByName(name);
            Assert.IsNotNull(ma, "MA '" + name + "' not generated");

            if (asmaDescriptors == null || asmaDescriptors.Length == 0)
            {
                Assert.That(AsmaDescriptors(ma), Is.Empty);
            }
            else
            {
                Assert.That(AsmaDescriptors(ma), Is.EquivalentTo(asmaDescriptors));
            }
            return ma;
        }

        #endregion

        #region Manual Testing

        [Test]
        [Ignore("for manual testing")]
        public void Test_mapping_ebInterface_to_ccl()
        {
            string repoPath = TestUtils.PathToTestResource(@"XSDImporterTest\mapping\MappingImporterTests\mapping_ebInterface_to_ccl\Invoice.eap");
            File.Copy(TestUtils.PathToTestResource(@"XSDImporterTest\mapping\MappingImporterTests\mapping_ebInterface_to_ccl\Repository-with-CDTs-and-CCs.eap"), repoPath, true);
            var repo = new Repository();
            repo.OpenFile(repoPath);

            var mappingFileNames = new List<string> { "ebInterface2CCTS_1_1.mfd", "ebInterface2CCTS_2_1.mfd", "ebInterface2CCTS_3_1.mfd", "ebInterface2CCTS_4_1.mfd", "ebInterface2CCTS_5_1.mfd", "ebInterface2CCTS_6_1.mfd", "ebInterface2CCTS_7_1.mfd", "ebInterface2CCTS_8_1.mfd" };            
            var mappingFiles = new List<string>();

            foreach (var mappingFile in mappingFileNames)
            {
                mappingFiles.Add(TestUtils.PathToTestResource(@"XSDImporterTest\mapping\MappingImporterTests\mapping_ebInterface_to_ccl\" + mappingFile));
            }

            string[] schemaFiles = new[] { TestUtils.PathToTestResource(@"XSDImporterTest\mapping\MappingImporterTests\mapping_ebInterface_to_ccl\Invoice.xsd") };

            Console.Out.WriteLine("Starting mapping");
            var repository = CctsRepositoryFactory.CreateCctsRepository(repo);
            var ccLib = repository.GetCcLibraries().FirstOrDefault();

            new MappingImporter(mappingFiles, schemaFiles, ccLib, ccLib.BLibrary, DocLibraryName, BieLibraryName, BdtLibraryName, Qualifier, RootElementName, repository).ImportMapping();
        }

        [Test]
        [Ignore("for manual testing")]
        public void Test_mapping_ubl_to_ccl()
        {
            string repoPath = TestUtils.PathToTestResource(@"XSDImporterTest\mapping\MappingImporterTests\mapping_ubl_to_ccl\Invoice.eap");
            File.Copy(TestUtils.PathToTestResource(@"XSDImporterTest\mapping\MappingImporterTests\mapping_ubl_to_ccl\Repository-with-CDTs-and-CCs.eap"), repoPath, true);
            var repo = new Repository();
            repo.OpenFile(repoPath);

            var mappingFileNames = new List<string> { "ubl2cll_1_1.mfd", "ubl2cll_2_1.mfd", "ubl2cll_3_1.mfd", "ubl2cll_4_1.mfd", "ubl2cll_5_1.mfd", "ubl2cll_6_1.mfd", "ubl2cll_7_1.mfd", "ubl2cll_8_1.mfd", "ubl2cll_9_1.mfd", "ubl2cll_10_1.mfd", "ubl2cll_11_1.mfd", "ubl2cll_12_1.mfd", "ubl2cll_13_1.mfd" };
            var mappingFiles = new List<string>();

            foreach (var mappingFile in mappingFileNames)
            {
                mappingFiles.Add(TestUtils.PathToTestResource(@"XSDImporterTest\mapping\MappingImporterTests\mapping_ubl_to_ccl\" + mappingFile));
            }

            string[] schemaFiles = new[] { TestUtils.PathToTestResource(@"XSDImporterTest\mapping\MappingImporterTests\mapping_ubl_to_ccl\invoice\maindoc\UBL-Invoice-2.0.xsd") };

            Console.Out.WriteLine("Starting mapping");
            var repository = CctsRepositoryFactory.CreateCctsRepository(repo);
            var ccLib = repository.GetCcLibraries().FirstOrDefault();

            new MappingImporter(mappingFiles, schemaFiles, ccLib, ccLib.BLibrary, DocLibraryName, BieLibraryName, BdtLibraryName, Qualifier, RootElementName, repository).ImportMapping();
        }

        #endregion

        [Test]
        public void Test_mapping_complex_type_with_nested_complex_type_to_multiple_accs()
        {
            string mappingFile = TestUtils.PathToTestResource(@"XSDImporterTest\mapping\MappingImporterTests\mapping_complex_type_with_nested_complex_type_to_multiple_accs\mapping.mfd");
            string[] schemaFiles = new[] { TestUtils.PathToTestResource(@"XSDImporterTest\mapping\MappingImporterTests\mapping_complex_type_with_nested_complex_type_to_multiple_accs\source.xsd") };

            new MappingImporter(new[] { mappingFile }, schemaFiles, ccLibrary, bLibrary, DocLibraryName, BieLibraryName, BdtLibraryName, Qualifier, RootElementName, cctsRepository).ImportMapping();

            var bieLibrary = ShouldContainBieLibrary(BieLibraryName);
            var docLibrary = ShouldContainDocLibrary(DocLibraryName);
            IBdtLibrary bdtLibrary = ShouldContainBdtLibrary(BdtLibraryName);

            IBdt bdtText = ShouldContainBdt(bdtLibrary, "String_Text", "Text", null);

            Assert.That(bdtLibrary.Bdts.Count(), Is.EqualTo(1));
            
            IAbie biePerson = ShouldContainAbie(bieLibrary, "PersonType_Person", "Person", new[] { new BbieDescriptor("Name_Name", bdtText.Id) }, null);
            IAbie bieAddress = ShouldContainAbie(bieLibrary, "AddressType_Address", "Address", new[] { new BbieDescriptor("Town_CityName", bdtText.Id) }, null);


            IMa maAddressType = ShouldContainMa(docLibrary, "AddressType", new[]
                                                                               {
                                                                                   new AsmaDescriptor("Person", biePerson.Id),
                                                                                   new AsmaDescriptor("Address", bieAddress.Id),
                                                                               });

            IMa maInvoiceType = ShouldContainMa(docLibrary, "InvoiceType", new[]
                                                                               {
                                                                                   new AsmaDescriptor("Address", maAddressType.Id),
                                                                               });
            ShouldContainMa(docLibrary, "test_Invoice", new[]
                                                                   {
                                                                       new AsmaDescriptor("Invoice", maInvoiceType.Id),
                                                                   });
        }

        [Test]
        public void Test_mapping_complex_type_with_nested_complex_type_to_single_acc()
        {
            string mappingFile = TestUtils.PathToTestResource(@"XSDImporterTest\mapping\MappingImporterTests\mapping_complex_type_with_nested_complex_type_to_single_acc\mapping.mfd");
            string[] schemaFiles = new[] { TestUtils.PathToTestResource(@"XSDImporterTest\mapping\MappingImporterTests\mapping_complex_type_with_nested_complex_type_to_single_acc\source.xsd") };


            new MappingImporter(new[] { mappingFile }, schemaFiles, ccLibrary, bLibrary, DocLibraryName, BieLibraryName, BdtLibraryName, Qualifier, RootElementName, cctsRepository).ImportMapping();

            var bieLibrary = ShouldContainBieLibrary(BieLibraryName);
            var docLibrary = ShouldContainDocLibrary(DocLibraryName);

            IBdtLibrary bdtLibrary = ShouldContainBdtLibrary(BdtLibraryName);

            IBdt bdtText = ShouldContainBdt(bdtLibrary, "String_Text", "Text", null);

            Assert.That(bdtLibrary.Bdts.Count(), Is.EqualTo(1));

            IAbie bieAddress = ShouldContainAbie(bieLibrary, "AddressType_Address", "Address", new[] { new BbieDescriptor("Town_CityName", bdtText.Id)  }, null);

            IAbie biePerson = ShouldContainAbie(bieLibrary, "PersonType_Party", "Party", new[] { new BbieDescriptor("Name_Name", bdtText.Id), }, new[]
                                                                                                                                                     {
                                                                                                                                                         new AsbieDescriptor("Address_Residence", bieAddress.Id),
                                                                                                                                                     });



            IMa maInvoice = ShouldContainMa(docLibrary, "InvoiceType", new[]
                                                                           {
                                                                               new AsmaDescriptor("Person", biePerson.Id),
                                                                           });
            ShouldContainMa(docLibrary, "test_Invoice", new[]
                                                                   {
                                                                       new AsmaDescriptor("Invoice", maInvoice.Id),
                                                                   });
        }

        [Test]
        public void Test_mapping_complex_type_to_multiple_accs()
        {
            string mappingFile = TestUtils.PathToTestResource(@"XSDImporterTest\mapping\MappingImporterTests\mapping_complex_type_to_multiple_accs\mapping.mfd");
            string[] schemaFiles = new[] { TestUtils.PathToTestResource(@"XSDImporterTest\mapping\MappingImporterTests\mapping_complex_type_to_multiple_accs\source.xsd") };

            new MappingImporter(new[] { mappingFile }, schemaFiles, ccLibrary, bLibrary, DocLibraryName, BieLibraryName, BdtLibraryName, Qualifier, RootElementName, cctsRepository).ImportMapping();

            var bieLibrary = ShouldContainBieLibrary(BieLibraryName);
            var docLibrary = ShouldContainDocLibrary(DocLibraryName);

            IBdtLibrary bdtLibrary = ShouldContainBdtLibrary(BdtLibraryName);

            IBdt bdtText = ShouldContainBdt(bdtLibrary, "String_Text", "Text", null);

            Assert.That(bdtLibrary.Bdts.Count(), Is.EqualTo(1));

            IAbie biePerson = ShouldContainAbie(bieLibrary, "AddressType_Person", "Person", new[] { new BbieDescriptor("PersonName_Name", bdtText.Id) }, null);
            IAbie bieAddress = ShouldContainAbie(bieLibrary, "AddressType_Address", "Address", new[] { new BbieDescriptor("Town_CityName", bdtText.Id) }, null);

            var maAddress = ShouldContainMa(docLibrary, "AddressType", new[]
                                                                           {
                                                                               new AsmaDescriptor("Address", bieAddress.Id),
                                                                               new AsmaDescriptor("Person", biePerson.Id),
                                                                           });
            var maInvoice = ShouldContainMa(docLibrary, "InvoiceType", new[]
                                                                           {
                                                                               new AsmaDescriptor("Address", maAddress.Id),
                                                                           });
            ShouldContainMa(docLibrary, "test_Invoice", new[]
                                                                   {
                                                                       new AsmaDescriptor("Invoice", maInvoice.Id),
                                                                   });
        }

        [Test]
        public void Test_mapping_complex_type_with_one_simple_element_to_single_acc()
        {
            string mappingFile = TestUtils.PathToTestResource(@"XSDImporterTest\mapping\MappingImporterTests\mapping_complex_type_with_one_simple_element_to_single_acc\mapping.mfd");
            string[] schemaFiles = new[] { TestUtils.PathToTestResource(@"XSDImporterTest\mapping\MappingImporterTests\mapping_complex_type_with_one_simple_element_to_single_acc\source.xsd") };

            new MappingImporter(new[] { mappingFile }, schemaFiles, ccLibrary, bLibrary, DocLibraryName, BieLibraryName, BdtLibraryName, Qualifier, RootElementName, cctsRepository).ImportMapping();

            var bieLibrary = ShouldContainBieLibrary(BieLibraryName);

            IBdtLibrary bdtLibrary = ShouldContainBdtLibrary(BdtLibraryName);

            IBdt bdtText = ShouldContainBdt(bdtLibrary, "String_Text", "Text", null);

            Assert.That(bdtLibrary.Bdts.Count(), Is.EqualTo(1));

            IAbie bieAddress = ShouldContainAbie(bieLibrary, "AddressType_Address", "Address", new[] { new BbieDescriptor("Town_CityName", bdtText.Id) }, null);

            var docLibrary = ShouldContainDocLibrary(DocLibraryName);
            ShouldContainMa(docLibrary, "test_Invoice", new[]
                                                                   {
                                                                       new AsmaDescriptor("Address", bieAddress.Id),
                                                                   });
        }

        [Test]
        public void Test_mapping_complex_type_with_simple_elements_choice_to_single_acc()
        {
            string mappingFile = TestUtils.PathToTestResource(@"XSDImporterTest\mapping\MappingImporterTests\mapping_complex_type_with_simple_elements_choice_to_single_acc\mapping.mfd");
            string[] schemaFiles = new[] { TestUtils.PathToTestResource(@"XSDImporterTest\mapping\MappingImporterTests\mapping_complex_type_with_simple_elements_choice_to_single_acc\source.xsd") };

            new MappingImporter(new[] { mappingFile }, schemaFiles, ccLibrary, bLibrary, DocLibraryName, BieLibraryName, BdtLibraryName, Qualifier, RootElementName, cctsRepository).ImportMapping();

            IBdtLibrary bdtLibrary = ShouldContainBdtLibrary(BdtLibraryName);
            IBdt bdtText = ShouldContainBdt(bdtLibrary, "String_Text", "Text", null);
            Assert.That(bdtLibrary.Bdts.Count(), Is.EqualTo(1));

            var bieLibrary = ShouldContainBieLibrary(BieLibraryName);
            IAbie bieAddress = ShouldContainAbie(bieLibrary, "AddressType_Address", "Address", new[] { new BbieDescriptor("StreetName_StreetName", bdtText.Id), new BbieDescriptor("CityName_CityName", bdtText.Id), new BbieDescriptor("CountryName_CountryName", bdtText.Id) }, null);

            var docLibrary = ShouldContainDocLibrary(DocLibraryName);
            ShouldContainMa(docLibrary, "test_Invoice", new[]
                                                                   {
                                                                       new AsmaDescriptor("Address", bieAddress.Id),
                                                                   });
        }

        [Test]
        public void Test_mapping_complex_type_with_simple_elements_all_to_single_acc()
        {
            string mappingFile = TestUtils.PathToTestResource(@"XSDImporterTest\mapping\MappingImporterTests\mapping_complex_type_with_simple_elements_all_to_single_acc\mapping.mfd");
            string[] schemaFiles = new[] { TestUtils.PathToTestResource(@"XSDImporterTest\mapping\MappingImporterTests\mapping_complex_type_with_simple_elements_all_to_single_acc\source.xsd") };

            new MappingImporter(new[] { mappingFile }, schemaFiles, ccLibrary, bLibrary, DocLibraryName, BieLibraryName, BdtLibraryName, Qualifier, RootElementName, cctsRepository).ImportMapping();

            IBdtLibrary bdtLibrary = ShouldContainBdtLibrary(BdtLibraryName);
            IBdt bdtText = ShouldContainBdt(bdtLibrary, "String_Text", "Text", null);
            Assert.That(bdtLibrary.Bdts.Count(), Is.EqualTo(1));

            var bieLibrary = ShouldContainBieLibrary(BieLibraryName);
            IAbie bieAddress = ShouldContainAbie(bieLibrary, "AddressType_Address", "Address", new[] { new BbieDescriptor("StreetName_StreetName", bdtText.Id), new BbieDescriptor("CityName_CityName", bdtText.Id), new BbieDescriptor("CountryName_CountryName", bdtText.Id) }, null);

            var docLibrary = ShouldContainDocLibrary(DocLibraryName);
            ShouldContainMa(docLibrary, "test_Invoice", new[]
                                                                   {
                                                                       new AsmaDescriptor("Address", bieAddress.Id),
                                                                   });
        }

        [Test]
        public void Test_mapping_complex_type_with_two_nested_complex_types_to_multiple_accs()
        {
            string mappingFile = TestUtils.PathToTestResource(@"XSDImporterTest\mapping\MappingImporterTests\mapping_complex_type_with_two_nested_complex_types_to_multiple_accs\mapping.mfd");
            string[] schemaFiles = new[] { TestUtils.PathToTestResource(@"XSDImporterTest\mapping\MappingImporterTests\mapping_complex_type_with_two_nested_complex_types_to_multiple_accs\source.xsd") };

            new MappingImporter(new[] { mappingFile }, schemaFiles, ccLibrary, bLibrary, DocLibraryName, BieLibraryName, BdtLibraryName, Qualifier, RootElementName, cctsRepository).ImportMapping();

            var bieLibrary = ShouldContainBieLibrary(BieLibraryName);
            var docLibrary = ShouldContainDocLibrary(DocLibraryName);

            IBdtLibrary bdtLibrary = ShouldContainBdtLibrary(BdtLibraryName);

            IBdt bdtText = ShouldContainBdt(bdtLibrary, "String_Text", "Text", null);

            Assert.That(bdtLibrary.Bdts.Count(), Is.EqualTo(1));

            IAbie biePerson = ShouldContainAbie(bieLibrary, "PersonType_Person", "Person", new[] { new BbieDescriptor("Name_Name", bdtText.Id) }, null);
            IAbie bieAddress = ShouldContainAbie(bieLibrary, "AddressType_Address", "Address", new[] { new BbieDescriptor("Town_CityName", bdtText.Id) }, null);

            var maInvoice = ShouldContainMa(docLibrary, "InvoiceType", new[]
                                                                           {
                                                                               new AsmaDescriptor("Address", bieAddress.Id),
                                                                               new AsmaDescriptor("Person", biePerson.Id),
                                                                           });

            ShouldContainMa(docLibrary, "test_Invoice", new[]
                                                                   {
                                                                       new AsmaDescriptor("Invoice", maInvoice.Id),
                                                                   });
        }

        [Test]
        public void Test_mapping_single_simple_typed_element_to_bcc()
        {
            string mappingFile = TestUtils.PathToTestResource(@"XSDImporterTest\mapping\MappingImporterTests\mapping_single_simple_typed_element_to_bcc\mapping.mfd");
            string[] schemaFiles = new[] { TestUtils.PathToTestResource(@"XSDImporterTest\mapping\MappingImporterTests\mapping_single_simple_typed_element_to_bcc\source.xsd") };

            new MappingImporter(new[] { mappingFile }, schemaFiles, ccLibrary, bLibrary, DocLibraryName, BieLibraryName, BdtLibraryName, Qualifier, RootElementName, cctsRepository).ImportMapping();
                      
            var bieLibrary = ShouldContainBieLibrary(BieLibraryName);
            var docLibrary = ShouldContainDocLibrary(DocLibraryName);

            IBdtLibrary bdtLibrary = ShouldContainBdtLibrary(BdtLibraryName);

            IBdt bdtText = ShouldContainBdt(bdtLibrary, "String_Text", "Text", null);

            Assert.That(bdtLibrary.Bdts.Count(), Is.EqualTo(1));

            IAbie bieParty = ShouldContainAbie(bieLibrary, "test_Party", "Party", new[] { new BbieDescriptor("PersonName_Name", bdtText.Id),  }, null);

            ShouldContainMa(docLibrary, "test_Invoice", new[]
                                                                   {
                                                                       new AsmaDescriptor("test_Party", bieParty.Id),
                                                                   });
        }

        [Test]
        public void Test_mapping_complex_type_with_one_attribute_to_single_acc()
        {
            string mappingFile = TestUtils.PathToTestResource(@"XSDImporterTest\mapping\MappingImporterTests\mapping_complex_type_with_one_attribute_to_single_acc\mapping.mfd");
            string[] schemaFiles = new[] { TestUtils.PathToTestResource(@"XSDImporterTest\mapping\MappingImporterTests\mapping_complex_type_with_one_attribute_to_single_acc\source.xsd") };

            new MappingImporter(new[] { mappingFile }, schemaFiles, ccLibrary, bLibrary, DocLibraryName, BieLibraryName, BdtLibraryName, Qualifier, RootElementName, cctsRepository).ImportMapping();

            var bieLibrary = ShouldContainBieLibrary(BieLibraryName);

            IBdtLibrary bdtLibrary = ShouldContainBdtLibrary(BdtLibraryName);

            IBdt bdtText = ShouldContainBdt(bdtLibrary, "String_Text", "Text", null);

            Assert.That(bdtLibrary.Bdts.Count(), Is.EqualTo(1));

            IAbie bieAddress = ShouldContainAbie(bieLibrary, "AddressType_Address", "Address", new[] { new BbieDescriptor("Town_CityName", bdtText.Id) }, null);

            var docLibrary = ShouldContainDocLibrary(DocLibraryName);
            ShouldContainMa(docLibrary, "test_Invoice", new[]
                                                                   {
                                                                       new AsmaDescriptor("Address", bieAddress.Id),
                                                                   });
        }

        [Test]
        public void Test_mapping_complex_type_with_simple_elements_and_attributes_to_cdt()
        {
            string mappingFile = TestUtils.PathToTestResource(@"XSDImporterTest\mapping\MappingImporterTests\mapping_complex_type_with_simple_elements_and_attributes_to_cdt\mapping.mfd");
            string[] schemaFiles = new[] { TestUtils.PathToTestResource(@"XSDImporterTest\mapping\MappingImporterTests\mapping_complex_type_with_simple_elements_and_attributes_to_cdt\source.xsd") };

            new MappingImporter(new[] { mappingFile }, schemaFiles, ccLibrary, bLibrary, DocLibraryName, BieLibraryName, BdtLibraryName, Qualifier, RootElementName, cctsRepository).ImportMapping();

            var bdtLibrary = ShouldContainBdtLibrary(BdtLibraryName);
            IBdt bdtText = ShouldContainBdt(bdtLibrary, "TextType_Text", "Text", new[] { "Language_Language", "LanguageLocale_LanguageLocale" });
            Assert.That(bdtLibrary.Bdts.Count(), Is.EqualTo(1));

            var bieLibrary = ShouldContainBieLibrary(BieLibraryName);
            IAbie bieAddress = ShouldContainAbie(bieLibrary, "AddressType_Address", "Address", new[] { new BbieDescriptor("CityName_CityName", bdtText.Id) }, null);

            var docLibrary = ShouldContainDocLibrary(DocLibraryName);
            ShouldContainMa(docLibrary, "test_Invoice", new[]
                                                                   {
                                                                       new AsmaDescriptor("Address", bieAddress.Id),
                                                                   });
        }

        [Test]
        public void Test_mapping_with_semisemantic_loss()
        {
            string mappingFile = TestUtils.PathToTestResource(@"XSDImporterTest\mapping\MappingImporterTests\mapping_with_semisemantic_loss\mapping.mfd");
            string[] schemaFiles = new[] { TestUtils.PathToTestResource(@"XSDImporterTest\mapping\MappingImporterTests\mapping_with_semisemantic_loss\source.xsd") };

            new MappingImporter(new[] { mappingFile }, schemaFiles, ccLibrary, bLibrary, DocLibraryName, BieLibraryName, BdtLibraryName, Qualifier, RootElementName, cctsRepository).ImportMapping();

            var bieLibrary = ShouldContainBieLibrary(BieLibraryName);
            var docLibrary = ShouldContainDocLibrary(DocLibraryName);
            IBdtLibrary bdtLibrary = ShouldContainBdtLibrary(BdtLibraryName);

            IBdt bdtText = ShouldContainBdt(bdtLibrary, "String_Text", "Text", null);

            Assert.That(bdtLibrary.Bdts.Count(), Is.EqualTo(1));
            
            IAbie bieAddress = ShouldContainAbie(bieLibrary, "AddressType_Address", "Address", new[] { new BbieDescriptor("StreetName_StreetName", bdtText.Id), new BbieDescriptor("Town_CityName", bdtText.Id) }, null);

            IAbie biePerson = ShouldContainAbie(bieLibrary, "PersonType_Party", "Party", new[] { new BbieDescriptor("FirstName_Name", bdtText.Id), new BbieDescriptor("LastName_Name", bdtText.Id) }, new[] { new AsbieDescriptor("HomeAddress_Residence", bieAddress.Id), new AsbieDescriptor("WorkAddress_Residence", bieAddress.Id) });

            ShouldContainMa(docLibrary, "test_Invoice", new[]
                                                                   {
                                                                       new AsmaDescriptor("Person", biePerson.Id),
                                                                   });
        }

        [Test]
        public void Test_mapping_simple_element_and_attributes_to_acc_with_mapping_function_split()
        {
            string mappingFile = TestUtils.PathToTestResource(@"XSDImporterTest\mapping\MappingImporterTests\mapping_simple_element_and_attributes_to_acc_with_mapping_function_split\mapping.mfd");
            string[] schemaFiles = new[] { TestUtils.PathToTestResource(@"XSDImporterTest\mapping\MappingImporterTests\mapping_simple_element_and_attributes_to_acc_with_mapping_function_split\source.xsd") };

            new MappingImporter(new[] { mappingFile }, schemaFiles, ccLibrary, bLibrary, DocLibraryName, BieLibraryName, BdtLibraryName, Qualifier, RootElementName, cctsRepository).ImportMapping();

            IBdtLibrary bdtLibrary = ShouldContainBdtLibrary(BdtLibraryName);
            IBdt bdtText = ShouldContainBdt(bdtLibrary, "String_Text", "Text", null);
            Assert.That(bdtLibrary.Bdts.Count(), Is.EqualTo(1));

            IBieLibrary bieLibrary = ShouldContainBieLibrary(BieLibraryName);
            IAbie bieAddress = ShouldContainAbie(bieLibrary, "AddressType_Address", "Address", new[] { new BbieDescriptor("Street_StreetName", bdtText.Id), new BbieDescriptor("Street_BuildingNumber", bdtText.Id), new BbieDescriptor("Town_CityName", bdtText.Id) }, null);
            Assert.That(bieLibrary.Abies.Count(), Is.EqualTo(1));
            
            var docLibrary = ShouldContainDocLibrary(DocLibraryName);
            ShouldContainMa(docLibrary, "test_Invoice", new[] { new AsmaDescriptor("Address", bieAddress.Id) });
            Assert.That(docLibrary.Mas.Count(), Is.EqualTo(1));
        }
    }

    internal class AsbieDescriptor : IEquatable<AsbieDescriptor>
    {
        private readonly int associatedElementId;
        private readonly string name;

        public AsbieDescriptor(string name, int associatedElementId)
        {
            this.name = name;
            this.associatedElementId = associatedElementId;
        }

        #region IEquatable<AsbieDescriptor> Members

        public bool Equals(AsbieDescriptor other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.name, name) && other.associatedElementId == associatedElementId;
        }

        #endregion

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (AsbieDescriptor)) return false;
            return Equals((AsbieDescriptor) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((name != null ? name.GetHashCode() : 0)*397) ^ associatedElementId;
            }
        }

        public static bool operator ==(AsbieDescriptor left, AsbieDescriptor right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(AsbieDescriptor left, AsbieDescriptor right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return string.Format("Name: {0}, AssociatedElementId: {1}", name, associatedElementId);
        }
    }

    internal class AsmaDescriptor : IEquatable<AsmaDescriptor>
    {
        private readonly int associatedElementId;
        private readonly string name;

        public AsmaDescriptor(string name, int associatedElementId)
        {
            this.name = name;
            this.associatedElementId = associatedElementId;
        }

        #region IEquatable<AsmaDescriptor> Members

        public bool Equals(AsmaDescriptor other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.name, name) && other.associatedElementId == associatedElementId;
        }

        #endregion

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(AsmaDescriptor)) return false;
            return Equals((AsmaDescriptor)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((name != null ? name.GetHashCode() : 0) * 397) ^ associatedElementId;
            }
        }

        public static bool operator ==(AsmaDescriptor left, AsmaDescriptor right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(AsmaDescriptor left, AsmaDescriptor right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return string.Format("Name: {0}, AssociatedElementId: {1}", name, associatedElementId);
        }
    }

    internal class BbieDescriptor : IEquatable<BbieDescriptor>
    {
        private readonly int bdtId;
        private readonly string bbieName;

        public BbieDescriptor(string bbieName, int bdtId)
        {
            this.bbieName = bbieName;
            this.bdtId = bdtId;
        }

        public bool Equals(BbieDescriptor other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.bbieName, bbieName) && other.bdtId == bdtId;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(BbieDescriptor)) return false;
            return Equals((BbieDescriptor)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((bbieName != null ? bbieName.GetHashCode() : 0) * 397) ^ bdtId;
            }
        }

        public static bool operator ==(BbieDescriptor left, BbieDescriptor right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(BbieDescriptor left, BbieDescriptor right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return string.Format("BBIE Name: {0}, BDT Id: {1}", bbieName, bdtId);
        }
    }
}