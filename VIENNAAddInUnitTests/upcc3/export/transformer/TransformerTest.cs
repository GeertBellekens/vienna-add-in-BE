using CctsRepository;
using CctsRepository.BieLibrary;
using CctsRepository.DocLibrary;
using EA;
using NUnit.Framework;
using VIENNAAddIn.upcc3;
using VIENNAAddIn.upcc3.export.mapping;
using VIENNAAddIn.upcc3.export.transformer;
using VIENNAAddInUtils;

namespace VIENNAAddInUnitTests.upcc3.export.transformer
{
    [TestFixture]
    public class TransformerTest
    {
        [Test]
        [Ignore("manual test case")]
        public void Test_transforming_ebinterface_to_ubl()
        {
            string ublInvoiceSchema = TestUtils.PathToTestResource(@"XSDExporterTest\transformer\transforming_ebinterface_to_ubl\ubl_invoice\UBL-Invoice-2.0.xsd");
            string ebInterfaceAsUblSchemaDirectory = TestUtils.PathToTestResource(@"XSDExporterTest\transformer\transforming_ebinterface_to_ubl\ebinterface_as_ubl\");
            string repositoryFile = TestUtils.PathToTestResource(@"XSDExporterTest\transformer\transforming_ebinterface_to_ubl\invoice.eap");

            Repository eaRepository = new Repository();
            eaRepository.OpenFile(repositoryFile);
            
            ICctsRepository cctsRepository = CctsRepositoryFactory.CreateCctsRepository(eaRepository);

            IBieLibrary ebiBieLibrary = cctsRepository.GetBieLibraryByPath((Path)"Model" / "bLibrary" / "ebInterface BIELibrary");
            IBieLibrary ublBieLibrary = cctsRepository.GetBieLibraryByPath((Path)"Model" / "bLibrary" / "UBL BIE Library");

            IDocLibrary ebiDocLibrary = cctsRepository.GetDocLibraryByPath((Path)"Model" / "bLibrary" / "ebInterface DOCLibrary");
            IDocLibrary ublDocLibrary = cctsRepository.GetDocLibraryByPath((Path)"Model" / "bLibrary" / "UBL DOC Library");

            Transformer.Transform(ebiBieLibrary, ublBieLibrary, ebiDocLibrary, ublDocLibrary);

            SubsetExporter.ExportSubset(ublDocLibrary, ublInvoiceSchema, ebInterfaceAsUblSchemaDirectory);
        }

        [Test]
        [Ignore("manual test case")]
        public void Test_roundtrip_ebinterface_to_ubl_and_back()
        {
            string ebiInvoiceSchema = TestUtils.PathToTestResource(@"XSDExporterTest\transformer\roundtrip_ebinterface_to_ubl_and_back\original_ebinterface_invoice_xml_schema\Invoice.xsd");
            string ebiXmlSchemaExportDirectory_1 = TestUtils.PathToTestResource(@"XSDExporterTest\transformer\roundtrip_ebinterface_to_ubl_and_back\imported_ebinterface_model_exported_as_ebinterface_xml_schema\");
            string ebiXmlSchemaExportDirectory_2 = TestUtils.PathToTestResource(@"XSDExporterTest\transformer\roundtrip_ebinterface_to_ubl_and_back\transformed_ebinterface_as_ubl_as_ebinterface_xml_schema\");
            string repositoryFile = TestUtils.PathToTestResource(@"XSDExporterTest\transformer\roundtrip_ebinterface_to_ubl_and_back\repository_containing_roundtripped_models.eap");
            
            Repository eaRepository = new Repository();
            eaRepository.OpenFile(repositoryFile);

            ICctsRepository cctsRepository = CctsRepositoryFactory.CreateCctsRepository(eaRepository);

            IBieLibrary ebiBieLibrary = cctsRepository.GetBieLibraryByPath((Path)"Model" / "bLibrary" / "ebInterface BIELibrary");
            IBieLibrary ublBieLibrary = cctsRepository.GetBieLibraryByPath((Path)"Model" / "bLibrary" / "UBL BIE Library");

            IDocLibrary ebiDocLibrary = cctsRepository.GetDocLibraryByPath((Path)"Model" / "bLibrary" / "ebInterface DOCLibrary");
            IDocLibrary ublDocLibrary = cctsRepository.GetDocLibraryByPath((Path)"Model" / "bLibrary" / "UBL DOC Library");

            SubsetExporter.ExportSubset(ebiDocLibrary, ebiInvoiceSchema, ebiXmlSchemaExportDirectory_1);

            Transformer.Transform(ebiBieLibrary, ublBieLibrary, ebiDocLibrary, ublDocLibrary);

            Transformer.Transform(ublBieLibrary, ebiBieLibrary, ublDocLibrary, ebiDocLibrary);

            SubsetExporter.ExportSubset(ebiDocLibrary, ebiInvoiceSchema, ebiXmlSchemaExportDirectory_2);
        }
    }
}