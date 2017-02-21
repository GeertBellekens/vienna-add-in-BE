using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Schema;
using CctsRepository;
using CctsRepository.BdtLibrary;
using CctsRepository.CdtLibrary;
using CctsRepository.PrimLibrary;
using Moq;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using VIENNAAddIn;
using VIENNAAddIn.upcc3;
using VIENNAAddIn.upcc3.import.cctsndr;
using VIENNAAddIn.upcc3.import.cctsndr.bdt;
using VIENNAAddInUnitTests.TestRepository;
using VIENNAAddInUtils;

namespace VIENNAAddInUnitTests.upcc3.import.cctsndr
{
    [TestFixture]
    public class BDTXsdImporterTests
    {
        private const string SimpleType = "simple_type";
        private const string ComplexType = "complex_type";

        private static IImporterContext CreateContext(string testCase)
        {
            XmlSchema bdtSchema = XmlSchema.Read(XmlReader.Create(TestUtils.RelativePathToTestResource(typeof (BDTXsdImporterTests), string.Format(@"BDTImporterTestResources\BusinessDataType_{0}.xsd", testCase))), null);

            var eaRepository = new EARepository();
            eaRepository.AddModel("Model", m => m.AddPackage("bLibrary", bLibrary =>
                                                                         {
                                                                             bLibrary.Element.Stereotype = Stereotype.bLibrary;
                                                                             bLibrary.AddPackage("PRIMLibrary", primLib =>
                                                                                                                {
                                                                                                                    primLib.Element.Stereotype = Stereotype.PRIMLibrary;
                                                                                                                    primLib.AddPRIM("String");
                                                                                                                    primLib.AddPRIM("Decimal");
                                                                                                                    primLib.AddPRIM("Date");
                                                                                                                });
                                                                             bLibrary.AddPackage("CDTLibrary", cdtLib =>
                                                                                                               {
                                                                                                                   cdtLib.Element.Stereotype = Stereotype.CDTLibrary;
                                                                                                                   cdtLib.AddCDT("Text");
                                                                                                               });
                                                                             bLibrary.AddPackage("BDTLibrary", bdtLib => { bdtLib.Element.Stereotype = Stereotype.BDTLibrary; });
                                                                         }));
            ICctsRepository cctsRepository = CctsRepositoryFactory.CreateCctsRepository(eaRepository);
            var primLibrary = cctsRepository.GetPrimLibraryByPath((Path) "Model"/"bLibrary"/"PRIMLibrary");
            var bdtLibrary = cctsRepository.GetBdtLibraryByPath((Path) "Model"/"bLibrary"/"BDTLibrary");
            var cdtLibrary = cctsRepository.GetCdtLibraryByPath((Path) "Model"/"bLibrary"/"CDTLibrary");

            var contextMock = new Mock<IImporterContext>();
            contextMock.SetupGet(c => c.PRIMLibrary).Returns(primLibrary);
            contextMock.SetupGet(c => c.CDTLibrary).Returns(cdtLibrary);
            contextMock.SetupGet(c => c.BDTLibrary).Returns(bdtLibrary);
            contextMock.SetupGet(c => c.BDTSchema).Returns(bdtSchema);

            return contextMock.Object;
        }

        private static IBdt AssertThatBDTLibraryContainsBDT(IImporterContext context, string bdtName, string conBasicTypeName)
        {
            var expectedCONBasicType = context.PRIMLibrary.GetPrimByName(conBasicTypeName);
            IBdt bdtText = context.BDTLibrary.GetBdtByName(bdtName);
            Assert.That(bdtText, Is.Not.Null, string.Format("Expected BDT named '{0}' not generated.", bdtName));
            Assert.That(bdtText.Con, Is.Not.Null, string.Format("CON of BDT {0} is null", bdtName));
            Assert.That(bdtText.Con.BasicType, Is.Not.Null, string.Format("BasicType of BDT {0} is null", bdtName));
            Assert.That(bdtText.Con.BasicType.Id, Is.EqualTo(expectedCONBasicType.Id), string.Format("Wrong basic type for CON of BDT {0}:\nExpected: <{1}>\nBut was: <{2}>", bdtName, expectedCONBasicType.Name, bdtText.Con.BasicType.Name));
            return bdtText;
        }

        /// <summary>
        /// [R 9908]
        /// Every BDT devoid of ccts:supplementaryComponents, or whose
        /// ccts:supplementaryComponents BVD facets map directly to the
        /// facets of an XML Schema built-in data type, MUST be defined as a
        /// named xsd:simpleType.
        /// 
        /// Deviation: We currently always use complex types if the BDT has any SUPs,
        /// because we do not really understand the specification in regard to this point.
        /// </summary>
        [Test]
        public void CreatesBDTsWithoutSUPsForSimpleTypes()
        {
            IImporterContext context = CreateContext(SimpleType);

            BDTXsdImporter.ImportXsd(context);

            var bdtText = AssertThatBDTLibraryContainsBDT(context, "Text", "String");
            AssertHasSUPs(bdtText, 0);
        }

        /// <summary>
        /// [R B91F]
        /// The xsd:simpleType definition of a BDT whose content
        /// component BVD is defined by a primitive whose facets map
        /// directly to the facets of an XML Schema built-in datatype MUST
        /// contain an xsd:restriction element with the xsd:base
        /// attribute set to the XML Schema built-in data type that represents
        /// the primitive.
        /// </summary>
        [Test]
        public void SetsTheCONsBasicTypeToTheBuiltinDatatypesOfSimpleTypes()
        {
            IImporterContext context = CreateContext(SimpleType);

            BDTXsdImporter.ImportXsd(context);

            AssertThatBDTLibraryContainsBDT(context, "Text", "String");
        }

        [Test]
        public void SetsBasedOnDependencyForImportedBDTs()
        {
            IImporterContext context = CreateContext(SimpleType);

            BDTXsdImporter.ImportXsd(context);

            var bdtText = AssertThatBDTLibraryContainsBDT(context, "Text", "String");
            AssertBDTIsBasedOn(context, bdtText, "Text");

            var bdtQualifiedText = AssertThatBDTLibraryContainsBDT(context, "Qualified_Text", "String");
            AssertBDTIsBasedOn(context, bdtQualifiedText, "Text");
        }

        private static void AssertBDTIsBasedOn(IImporterContext context, IBdt bdt, string cdtName)
        {
            var cdt = context.CDTLibrary.GetCdtByName(cdtName);
            Assert.That(bdt.BasedOn, Is.Not.Null, "BasedOn is null");
            Assert.That(bdt.BasedOn, Is.Not.Null, "BasedOn.CDT is null");
            Assert.That(bdt.BasedOn.Id, Is.EqualTo(cdt.Id), string.Format("Based on wrong CDT:\nExpected: <{0}>\nBut was:  <{1}>", cdt.Name, bdt.BasedOn.Name));
        }

        /// <summary>
        /// [R A7B8]
        /// The name of a BDT MUST be the:
        ///  - BDT ccts:DataTypeQualifierTerm(s) if any, plus.
        ///  - The ccts:DataTypeTerm, plus.
        ///  - The word Type, plus.
        ///  - The underscore character [_], plus.
        ///  - A six character unique identifier, unique within the given
        ///    namespace, consisting of lowercase alphabetic characters
        ///    [a-z], uppercase alphabetic characters [A-Z], and digit
        ///    characters [0-9].
        /// With the separators removed and approved abbreviations and
        /// acronyms applied.
        ///
        /// [R 8437]
        /// The six character unique identifier used for the BDT Type name
        /// MUST be unique within the namespace in which it is defined.
        /// </summary>
        [Test]
        public void ResolvesBDTTypeNamesAccordingToTheNDR()
        {
            IImporterContext context = CreateContext(MethodBase.GetCurrentMethod().Name);

            BDTXsdImporter.ImportXsd(context);

            AssertThatBDTLibraryContainsBDT(context, "SimpleText", "String");
            AssertThatBDTLibraryContainsBDT(context, "Qualified_SimpleText", "String");
            AssertThatBDTLibraryContainsBDT(context, "Qualifier1_Qualifier2_SimpleText", "String");

            AssertThatBDTLibraryContainsBDT(context, "ComplexText", "String");
            AssertThatBDTLibraryContainsBDT(context, "Qualified_ComplexText", "String");
            AssertThatBDTLibraryContainsBDT(context, "Qualifier1_Qualifier2_ComplexText", "String");
        }

        /// <summary>
        /// [R AB05]
        /// Every BDT that includes one or more Supplementary Components
        /// that do not map directly to the facets of an XSD built-in datatype
        /// MUST be defined as an xsd:complexType.
        /// 
        /// [R 890A]
        /// Every BDT xsd:complexType definition MUST include an
        /// xsd:attribute declaration for each Supplementary Component.
        /// 
        /// [R ABC1]
        /// The name of the Supplementary Component xsd:attribute
        /// must be the the Supplementary Component Property Term Name
        /// and Representation Term Name with periods, spaces, and other
        /// separators removed.
        /// </summary>
        [Test]
        public void CreatesSUPsForXsdAttributesInComplexTypes()
        {
            IImporterContext context = CreateContext(ComplexType);

            BDTXsdImporter.ImportXsd(context);

            var bdtText = AssertThatBDTLibraryContainsBDT(context, "Text", "String");
            AssertHasSUPs(bdtText, 1);
            AssertHasSUP(context, bdtText, 0, "String", "propertyTermName");
        }

        /// <summary>
        /// [R BBCB]
        /// The xsd:complexType definition of a BDT whose Content
        /// Component BVD is defined by a primitive whose facets do not map
        /// directly to the facets of an XML Schema built-in datatype MUST
        /// contain an xsd:simpleContent element that contains an
        /// xsd:extension whose base attribute is set to the XML Schema
        /// built-in data type that represents the primitive.
        /// </summary>
        [Test]
        public void SetsTheCONsBasicTypeToTheBuiltinDatatypesOfTheSimpleContentExtensionOfComplexTypes()
        {
            IImporterContext context = CreateContext(ComplexType);

            BDTXsdImporter.ImportXsd(context);

            AssertThatBDTLibraryContainsBDT(context, "Text", "String");
        }

        [Test]
        public void SUPBasicTypesAreDeterminedFromXsdTypes()
        {
            IImporterContext context = CreateContext(MethodBase.GetCurrentMethod().Name);

            BDTXsdImporter.ImportXsd(context);

            var bdtText = AssertThatBDTLibraryContainsBDT(context, "BDT", "String");
            AssertHasSUPs(bdtText, 2);
            AssertHasSUP(context, bdtText, 0, "Decimal", "decimalSUP");
            AssertHasSUP(context, bdtText, 1, "String", "stringSUP");
        }

        [Test]
        public void BDTInheritsCONFromParentBDT()
        {
            IImporterContext context = CreateContext(MethodBase.GetCurrentMethod().Name);

            BDTXsdImporter.ImportXsd(context);

            AssertThatBDTLibraryContainsBDT(context, "Text", "Decimal");
            AssertThatBDTLibraryContainsBDT(context, "Restricted_Text", "Decimal");

            AssertThatBDTLibraryContainsBDT(context, "SimpleText", "Decimal");
            AssertThatBDTLibraryContainsBDT(context, "Restricted_SimpleText", "Decimal");
        }

        [Test]
        public void BDTInheritsSUPsFromParentBDT()
        {
            IImporterContext context = CreateContext(MethodBase.GetCurrentMethod().Name);

            BDTXsdImporter.ImportXsd(context);

            var bdtText = AssertThatBDTLibraryContainsBDT(context, "Text", "String");
            AssertHasSUPs(bdtText, 1);
            AssertHasSUP(context, bdtText, 0, "String", "sup");

            var bdtRestrictedText = AssertThatBDTLibraryContainsBDT(context, "Restricted_Text", "String");
            AssertHasSUPs(bdtRestrictedText, 1);
            AssertHasSUP(context, bdtRestrictedText, 0, "String", "sup");
        }

        [Test]
        public void BDTCanProhibitInheritanceOfSUPFromParentBDT()
        {
            IImporterContext context = CreateContext(MethodBase.GetCurrentMethod().Name);

            BDTXsdImporter.ImportXsd(context);

            var bdtText = AssertThatBDTLibraryContainsBDT(context, "Text", "Decimal");
            AssertHasSUPs(bdtText, 1);
            AssertHasSUP(context, bdtText, 0, "String", "sup");

            var bdtRestrictedText = AssertThatBDTLibraryContainsBDT(context, "Restricted_Text", "Decimal");
            AssertHasSUPs(bdtRestrictedText, 0);
        }

        [Test]
        public void CONFacetsAreAppliedToComplexTypes()
        {
            IImporterContext context = CreateContext(MethodBase.GetCurrentMethod().Name);

            BDTXsdImporter.ImportXsd(context);

            var aRestrictedTextBDT = AssertThatBDTLibraryContainsBDT(context, "A_Restricted_Text", "String");
            Assert.That(aRestrictedTextBDT.Con.Pattern, Is.EqualTo("[abc]*"));

            var anotherRestrictedTextBDT = AssertThatBDTLibraryContainsBDT(context, "Another_Restricted_Text", "String");
            Assert.That(anotherRestrictedTextBDT.Con.MinimumLength, Is.EqualTo("5"));
            Assert.That(anotherRestrictedTextBDT.Con.MaximumLength, Is.EqualTo("9"));

            var aRestrictedNumberBDT = AssertThatBDTLibraryContainsBDT(context, "A_Restricted_Number", "Decimal");
            Assert.That(aRestrictedNumberBDT.Con.MinimumInclusive, Is.EqualTo("4"));
            Assert.That(aRestrictedNumberBDT.Con.MaximumInclusive, Is.EqualTo("6"));
            Assert.That(aRestrictedNumberBDT.Con.FractionDigits, Is.EqualTo("3"));
            Assert.That(aRestrictedNumberBDT.Con.TotalDigits, Is.EqualTo("5"));

            var furtherRestrictedNumberBDT = AssertThatBDTLibraryContainsBDT(context, "Further_A_Restricted_Number", "Decimal");
            Assert.That(furtherRestrictedNumberBDT.Con.MinimumInclusive, Is.EqualTo("5"));
            Assert.That(furtherRestrictedNumberBDT.Con.MaximumInclusive, Is.EqualTo("6"));
            Assert.That(furtherRestrictedNumberBDT.Con.FractionDigits, Is.EqualTo("3"));
            Assert.That(furtherRestrictedNumberBDT.Con.TotalDigits, Is.EqualTo("5"));

            var anotherRestrictedNumberBDT = AssertThatBDTLibraryContainsBDT(context, "Another_Restricted_Number", "Decimal");
            Assert.That(anotherRestrictedNumberBDT.Con.MinimumExclusive, Is.EqualTo("6"));
            Assert.That(anotherRestrictedNumberBDT.Con.MaximumExclusive, Is.EqualTo("8"));
        }

        [Test]
        public void CONFacetsAreAppliedToSimpleTypes()
        {
            IImporterContext context = CreateContext(MethodBase.GetCurrentMethod().Name);

            BDTXsdImporter.ImportXsd(context);

            var aRestrictedTextBDT = AssertThatBDTLibraryContainsBDT(context, "A_Restricted_Text", "String");
            Assert.That(aRestrictedTextBDT.Con.Pattern, Is.EqualTo("[abc]*"));

            var anotherRestrictedTextBDT = AssertThatBDTLibraryContainsBDT(context, "Another_Restricted_Text", "String");
            Assert.That(anotherRestrictedTextBDT.Con.MinimumLength, Is.EqualTo("5"));
            Assert.That(anotherRestrictedTextBDT.Con.MaximumLength, Is.EqualTo("9"));

            var aRestrictedNumberBDT = AssertThatBDTLibraryContainsBDT(context, "A_Restricted_Number", "Decimal");
            Assert.That(aRestrictedNumberBDT.Con.MinimumInclusive, Is.EqualTo("4"));
            Assert.That(aRestrictedNumberBDT.Con.MaximumInclusive, Is.EqualTo("6"));
            Assert.That(aRestrictedNumberBDT.Con.FractionDigits, Is.EqualTo("3"));
            Assert.That(aRestrictedNumberBDT.Con.TotalDigits, Is.EqualTo("5"));

            var furtherRestrictedNumberBDT = AssertThatBDTLibraryContainsBDT(context, "Further_A_Restricted_Number", "Decimal");
            Assert.That(furtherRestrictedNumberBDT.Con.MinimumInclusive, Is.EqualTo("5"));
            Assert.That(furtherRestrictedNumberBDT.Con.MaximumInclusive, Is.EqualTo("6"));
            Assert.That(furtherRestrictedNumberBDT.Con.FractionDigits, Is.EqualTo("3"));
            Assert.That(furtherRestrictedNumberBDT.Con.TotalDigits, Is.EqualTo("5"));

            var anotherRestrictedNumberBDT = AssertThatBDTLibraryContainsBDT(context, "Another_Restricted_Number", "Decimal");
            Assert.That(anotherRestrictedNumberBDT.Con.MinimumExclusive, Is.EqualTo("6"));
            Assert.That(anotherRestrictedNumberBDT.Con.MaximumExclusive, Is.EqualTo("8"));
        }

        private static void AssertHasSUP(IImporterContext context, IBdt bdtText, int index, string basicTypeName, string name)
        {
            var expectedBasicType = context.PRIMLibrary.GetPrimByName(basicTypeName);
            var sup = bdtText.Sups.ElementAt(index);
            Assert.That(sup.Name, Is.EqualTo(name));
            Assert.That(sup.BasicType.Id, Is.EqualTo(expectedBasicType.Id));
        }

        private static void AssertHasSUPs(IBdt bdtText, int expectedCount)
        {
            if (expectedCount > 0)
            {
                Assert.That(bdtText.Sups, Is.Not.Null, string.Format("Expected {0} SUPs.", expectedCount));
                Assert.That(bdtText.Sups.Count(), Is.EqualTo(expectedCount), string.Format("Wrong number of SUPs"));
            }
            else
            {
                if (bdtText.Sups != null)
                {
                    Assert.That(bdtText.Sups.Count(), Is.EqualTo(0), "Expected no SUPs.");
                }
            }
        }
    }
}