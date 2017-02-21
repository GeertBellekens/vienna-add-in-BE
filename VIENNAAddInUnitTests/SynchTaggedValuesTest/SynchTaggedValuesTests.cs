using EA;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using VIENNAAddIn;
using VIENNAAddIn.Settings;
using VIENNAAddInUtils;

namespace VIENNAAddInUnitTests.SynchTaggedValuesTest
{
    [TestFixture]
    public class SynchTaggedValuesTests : SynchTaggedValuesTestsBase
    {

		[Test]
		public void ShouldCreateMissingTaggedValuesForBdtLibrary()
		{
            Repository repo = CreateRepositoryWithoutTaggedValues();

            var synchStereotypes = new SynchTaggedValues(repo);
            synchStereotypes.FixTaggedValues();

            var package = repo.Resolve<Package>((Path) "Model"/"bLibrary"/"BDTLibrary");
			Assert.That(package, Is.Not.Null, "Package not found");
            Assert.That(package, HasTaggedValues(new[]{"businessTerm", "copyright", "owner", "reference", "status", "uniqueIdentifier", "versionIdentifier", "baseURN", "namespacePrefix", }), "missing tagged values");
		}

		[Test]
		public void ShouldCreateMissingTaggedValuesForBdt()
		{
            Repository repo = CreateRepositoryWithoutTaggedValues();

            var synchStereotypes = new SynchTaggedValues(repo);
            synchStereotypes.FixTaggedValues();

            var element = repo.Resolve<Element>((Path) "Model"/"bLibrary"/"BDTLibrary"/"BDT");
			Assert.That(element, Is.Not.Null, "Element not found");
            Assert.That(element, HasTaggedValues(new[]{"businessTerm", "definition", "dictionaryEntryName", "languageCode", "uniqueIdentifier", "versionIdentifier", "usageRule", }), "missing tagged values");
		}

		[Test]
		public void ShouldCreateMissingTaggedValuesForBdtCon()
		{
            Repository repo = CreateRepositoryWithoutTaggedValues();

            var synchStereotypes = new SynchTaggedValues(repo);
            synchStereotypes.FixTaggedValues();

            var element = repo.Resolve<Element>((Path) "Model"/"bLibrary"/"BDTLibrary"/"BDT");
			Assert.That(element, Is.Not.Null, "Element not found");
			var attribute = (Attribute) element.Attributes.GetByName("CON");
			Assert.That(attribute, Is.Not.Null, "Attribute not found");
            Assert.That(attribute, HasTaggedValues(new[]{"businessTerm", "definition", "dictionaryEntryName", "enumeration", "fractionDigits", "languageCode", "maximumExclusive", "maximumInclusive", "maximumLength", "minimumExclusive", "minimumInclusive", "minimumLength", "modificationAllowedIndicator", "pattern", "totalDigits", "uniqueIdentifier", "usageRule", "versionIdentifier", }), "missing tagged values");
		}

		[Test]
		public void ShouldCreateMissingTaggedValuesForBdtSup()
		{
            Repository repo = CreateRepositoryWithoutTaggedValues();

            var synchStereotypes = new SynchTaggedValues(repo);
            synchStereotypes.FixTaggedValues();

            var element = repo.Resolve<Element>((Path) "Model"/"bLibrary"/"BDTLibrary"/"BDT");
			Assert.That(element, Is.Not.Null, "Element not found");
			var attribute = (Attribute) element.Attributes.GetByName("SUP");
			Assert.That(attribute, Is.Not.Null, "Attribute not found");
            Assert.That(attribute, HasTaggedValues(new[]{"businessTerm", "definition", "dictionaryEntryName", "enumeration", "fractionDigits", "languageCode", "maximumExclusive", "maximumInclusive", "maximumLength", "minimumExclusive", "minimumInclusive", "minimumLength", "modificationAllowedIndicator", "pattern", "totalDigits", "uniqueIdentifier", "usageRule", "versionIdentifier", }), "missing tagged values");
		}

		[Test]
		public void ShouldCreateMissingTaggedValuesForBieLibrary()
		{
            Repository repo = CreateRepositoryWithoutTaggedValues();

            var synchStereotypes = new SynchTaggedValues(repo);
            synchStereotypes.FixTaggedValues();

            var package = repo.Resolve<Package>((Path) "Model"/"bLibrary"/"BIELibrary");
			Assert.That(package, Is.Not.Null, "Package not found");
            Assert.That(package, HasTaggedValues(new[]{"businessTerm", "copyright", "owner", "reference", "status", "uniqueIdentifier", "versionIdentifier", "baseURN", "namespacePrefix", }), "missing tagged values");
		}

		[Test]
		public void ShouldCreateMissingTaggedValuesForAbie()
		{
            Repository repo = CreateRepositoryWithoutTaggedValues();

            var synchStereotypes = new SynchTaggedValues(repo);
            synchStereotypes.FixTaggedValues();

            var element = repo.Resolve<Element>((Path) "Model"/"bLibrary"/"BIELibrary"/"ABIE");
			Assert.That(element, Is.Not.Null, "Element not found");
            Assert.That(element, HasTaggedValues(new[]{"businessTerm", "definition", "dictionaryEntryName", "languageCode", "uniqueIdentifier", "versionIdentifier", "usageRule", }), "missing tagged values");
		}

		[Test]
		public void ShouldCreateMissingTaggedValuesForBbie()
		{
            Repository repo = CreateRepositoryWithoutTaggedValues();

            var synchStereotypes = new SynchTaggedValues(repo);
            synchStereotypes.FixTaggedValues();

            var element = repo.Resolve<Element>((Path) "Model"/"bLibrary"/"BIELibrary"/"ABIE");
			Assert.That(element, Is.Not.Null, "Element not found");
			var attribute = (Attribute) element.Attributes.GetByName("BBIE");
			Assert.That(attribute, Is.Not.Null, "Attribute not found");
            Assert.That(attribute, HasTaggedValues(new[]{"businessTerm", "definition", "dictionaryEntryName", "languageCode", "sequencingKey", "uniqueIdentifier", "versionIdentifier", "usageRule", }), "missing tagged values");
		}

		[Test]
		public void ShouldCreateMissingTaggedValuesForAsbie()
		{
            Repository repo = CreateRepositoryWithoutTaggedValues();

            var synchStereotypes = new SynchTaggedValues(repo);
            synchStereotypes.FixTaggedValues();

            var element = repo.Resolve<Element>((Path) "Model"/"bLibrary"/"BIELibrary"/"ABIE");
			Assert.That(element, Is.Not.Null, "Element not found");
			var connector = (Connector) element.Connectors.GetByName("ASBIE");
			Assert.That(connector, Is.Not.Null, "Connector not found");
            Assert.That(connector, HasTaggedValues(new[]{"businessTerm", "definition", "dictionaryEntryName", "languageCode", "sequencingKey", "uniqueIdentifier", "versionIdentifier", "usageRule", }), "missing tagged values");
		}

		[Test]
		public void ShouldCreateMissingTaggedValuesForBLibrary()
		{
            Repository repo = CreateRepositoryWithoutTaggedValues();

            var synchStereotypes = new SynchTaggedValues(repo);
            synchStereotypes.FixTaggedValues();

            var package = repo.Resolve<Package>((Path) "Model"/"bLibrary");
			Assert.That(package, Is.Not.Null, "Package not found");
            Assert.That(package, HasTaggedValues(new[]{"businessTerm", "copyright", "owner", "reference", "status", "uniqueIdentifier", "versionIdentifier", }), "missing tagged values");
		}

		[Test]
		public void ShouldCreateMissingTaggedValuesForCcLibrary()
		{
            Repository repo = CreateRepositoryWithoutTaggedValues();

            var synchStereotypes = new SynchTaggedValues(repo);
            synchStereotypes.FixTaggedValues();

            var package = repo.Resolve<Package>((Path) "Model"/"bLibrary"/"CCLibrary");
			Assert.That(package, Is.Not.Null, "Package not found");
            Assert.That(package, HasTaggedValues(new[]{"businessTerm", "copyright", "owner", "reference", "status", "uniqueIdentifier", "versionIdentifier", "baseURN", "namespacePrefix", }), "missing tagged values");
		}

		[Test]
		public void ShouldCreateMissingTaggedValuesForAcc()
		{
            Repository repo = CreateRepositoryWithoutTaggedValues();

            var synchStereotypes = new SynchTaggedValues(repo);
            synchStereotypes.FixTaggedValues();

            var element = repo.Resolve<Element>((Path) "Model"/"bLibrary"/"CCLibrary"/"ACC");
			Assert.That(element, Is.Not.Null, "Element not found");
            Assert.That(element, HasTaggedValues(new[]{"businessTerm", "definition", "dictionaryEntryName", "languageCode", "uniqueIdentifier", "versionIdentifier", "usageRule", }), "missing tagged values");
		}

		[Test]
		public void ShouldCreateMissingTaggedValuesForBcc()
		{
            Repository repo = CreateRepositoryWithoutTaggedValues();

            var synchStereotypes = new SynchTaggedValues(repo);
            synchStereotypes.FixTaggedValues();

            var element = repo.Resolve<Element>((Path) "Model"/"bLibrary"/"CCLibrary"/"ACC");
			Assert.That(element, Is.Not.Null, "Element not found");
			var attribute = (Attribute) element.Attributes.GetByName("BCC");
			Assert.That(attribute, Is.Not.Null, "Attribute not found");
            Assert.That(attribute, HasTaggedValues(new[]{"businessTerm", "definition", "dictionaryEntryName", "languageCode", "sequencingKey", "uniqueIdentifier", "versionIdentifier", "usageRule", }), "missing tagged values");
		}

		[Test]
		public void ShouldCreateMissingTaggedValuesForAscc()
		{
            Repository repo = CreateRepositoryWithoutTaggedValues();

            var synchStereotypes = new SynchTaggedValues(repo);
            synchStereotypes.FixTaggedValues();

            var element = repo.Resolve<Element>((Path) "Model"/"bLibrary"/"CCLibrary"/"ACC");
			Assert.That(element, Is.Not.Null, "Element not found");
			var connector = (Connector) element.Connectors.GetByName("ASCC");
			Assert.That(connector, Is.Not.Null, "Connector not found");
            Assert.That(connector, HasTaggedValues(new[]{"businessTerm", "definition", "dictionaryEntryName", "languageCode", "sequencingKey", "uniqueIdentifier", "versionIdentifier", "usageRule", }), "missing tagged values");
		}

		[Test]
		public void ShouldCreateMissingTaggedValuesForCdtLibrary()
		{
            Repository repo = CreateRepositoryWithoutTaggedValues();

            var synchStereotypes = new SynchTaggedValues(repo);
            synchStereotypes.FixTaggedValues();

            var package = repo.Resolve<Package>((Path) "Model"/"bLibrary"/"CDTLibrary");
			Assert.That(package, Is.Not.Null, "Package not found");
            Assert.That(package, HasTaggedValues(new[]{"businessTerm", "copyright", "owner", "reference", "status", "uniqueIdentifier", "versionIdentifier", "baseURN", "namespacePrefix", }), "missing tagged values");
		}

		[Test]
		public void ShouldCreateMissingTaggedValuesForCdt()
		{
            Repository repo = CreateRepositoryWithoutTaggedValues();

            var synchStereotypes = new SynchTaggedValues(repo);
            synchStereotypes.FixTaggedValues();

            var element = repo.Resolve<Element>((Path) "Model"/"bLibrary"/"CDTLibrary"/"CDT");
			Assert.That(element, Is.Not.Null, "Element not found");
            Assert.That(element, HasTaggedValues(new[]{"businessTerm", "definition", "dictionaryEntryName", "languageCode", "uniqueIdentifier", "versionIdentifier", "usageRule", }), "missing tagged values");
		}

		[Test]
		public void ShouldCreateMissingTaggedValuesForCdtCon()
		{
            Repository repo = CreateRepositoryWithoutTaggedValues();

            var synchStereotypes = new SynchTaggedValues(repo);
            synchStereotypes.FixTaggedValues();

            var element = repo.Resolve<Element>((Path) "Model"/"bLibrary"/"CDTLibrary"/"CDT");
			Assert.That(element, Is.Not.Null, "Element not found");
			var attribute = (Attribute) element.Attributes.GetByName("CON");
			Assert.That(attribute, Is.Not.Null, "Attribute not found");
            Assert.That(attribute, HasTaggedValues(new[]{"businessTerm", "definition", "dictionaryEntryName", "languageCode", "modificationAllowedIndicator", "uniqueIdentifier", "versionIdentifier", "usageRule", }), "missing tagged values");
		}

		[Test]
		public void ShouldCreateMissingTaggedValuesForCdtSup()
		{
            Repository repo = CreateRepositoryWithoutTaggedValues();

            var synchStereotypes = new SynchTaggedValues(repo);
            synchStereotypes.FixTaggedValues();

            var element = repo.Resolve<Element>((Path) "Model"/"bLibrary"/"CDTLibrary"/"CDT");
			Assert.That(element, Is.Not.Null, "Element not found");
			var attribute = (Attribute) element.Attributes.GetByName("SUP");
			Assert.That(attribute, Is.Not.Null, "Attribute not found");
            Assert.That(attribute, HasTaggedValues(new[]{"businessTerm", "definition", "dictionaryEntryName", "languageCode", "modificationAllowedIndicator", "uniqueIdentifier", "versionIdentifier", "usageRule", }), "missing tagged values");
		}

		[Test]
		public void ShouldCreateMissingTaggedValuesForDocLibrary()
		{
            Repository repo = CreateRepositoryWithoutTaggedValues();

            var synchStereotypes = new SynchTaggedValues(repo);
            synchStereotypes.FixTaggedValues();

            var package = repo.Resolve<Package>((Path) "Model"/"bLibrary"/"DOCLibrary");
			Assert.That(package, Is.Not.Null, "Package not found");
            Assert.That(package, HasTaggedValues(new[]{"businessTerm", "copyright", "owner", "reference", "status", "uniqueIdentifier", "versionIdentifier", "baseURN", "namespacePrefix", }), "missing tagged values");
		}

		[Test]
		public void ShouldCreateMissingTaggedValuesForEnumLibrary()
		{
            Repository repo = CreateRepositoryWithoutTaggedValues();

            var synchStereotypes = new SynchTaggedValues(repo);
            synchStereotypes.FixTaggedValues();

            var package = repo.Resolve<Package>((Path) "Model"/"bLibrary"/"ENUMLibrary");
			Assert.That(package, Is.Not.Null, "Package not found");
            Assert.That(package, HasTaggedValues(new[]{"businessTerm", "copyright", "owner", "reference", "status", "uniqueIdentifier", "versionIdentifier", "baseURN", "namespacePrefix", }), "missing tagged values");
		}

		[Test]
		public void ShouldCreateMissingTaggedValuesForEnum()
		{
            Repository repo = CreateRepositoryWithoutTaggedValues();

            var synchStereotypes = new SynchTaggedValues(repo);
            synchStereotypes.FixTaggedValues();

            var element = repo.Resolve<Element>((Path) "Model"/"bLibrary"/"ENUMLibrary"/"ENUM");
			Assert.That(element, Is.Not.Null, "Element not found");
            Assert.That(element, HasTaggedValues(new[]{"businessTerm", "codeListAgencyIdentifier", "codeListAgencyName", "codeListIdentifier", "codeListName", "dictionaryEntryName", "definition", "enumerationURI", "languageCode", "modificationAllowedIndicator", "restrictedPrimitive", "status", "uniqueIdentifier", "versionIdentifier", }), "missing tagged values");
		}

		[Test]
		public void ShouldCreateMissingTaggedValuesForCodelistEntries()
		{
            Repository repo = CreateRepositoryWithoutTaggedValues();

            var synchStereotypes = new SynchTaggedValues(repo);
            synchStereotypes.FixTaggedValues();

            var element = repo.Resolve<Element>((Path) "Model"/"bLibrary"/"ENUMLibrary"/"ENUM");
			Assert.That(element, Is.Not.Null, "Element not found");
			var attribute = (Attribute) element.Attributes.GetByName("CodelistEntry");
			Assert.That(attribute, Is.Not.Null, "Attribute not found");
            Assert.That(attribute, HasTaggedValues(new[]{"codeName", "status", }), "missing tagged values");
		}

		[Test]
		public void ShouldCreateMissingTaggedValuesForIdScheme()
		{
            Repository repo = CreateRepositoryWithoutTaggedValues();

            var synchStereotypes = new SynchTaggedValues(repo);
            synchStereotypes.FixTaggedValues();

            var element = repo.Resolve<Element>((Path) "Model"/"bLibrary"/"ENUMLibrary"/"IDSCHEME");
			Assert.That(element, Is.Not.Null, "Element not found");
            Assert.That(element, HasTaggedValues(new[]{"businessTerm", "definition", "dictionaryEntryName", "identifierSchemeAgencyIdentifier", "identifierSchemeAgencyName", "modificationAllowedIndicator", "pattern", "restrictedPrimitive", "uniqueIdentifier", "versionIdentifier", }), "missing tagged values");
		}

		[Test]
		public void ShouldCreateMissingTaggedValuesForPrimLibrary()
		{
            Repository repo = CreateRepositoryWithoutTaggedValues();

            var synchStereotypes = new SynchTaggedValues(repo);
            synchStereotypes.FixTaggedValues();

            var package = repo.Resolve<Package>((Path) "Model"/"bLibrary"/"PRIMLibrary");
			Assert.That(package, Is.Not.Null, "Package not found");
            Assert.That(package, HasTaggedValues(new[]{"businessTerm", "copyright", "owner", "reference", "status", "uniqueIdentifier", "versionIdentifier", "baseURN", "namespacePrefix", }), "missing tagged values");
		}

		[Test]
		public void ShouldCreateMissingTaggedValuesForPrim()
		{
            Repository repo = CreateRepositoryWithoutTaggedValues();

            var synchStereotypes = new SynchTaggedValues(repo);
            synchStereotypes.FixTaggedValues();

            var element = repo.Resolve<Element>((Path) "Model"/"bLibrary"/"PRIMLibrary"/"PRIM");
			Assert.That(element, Is.Not.Null, "Element not found");
            Assert.That(element, HasTaggedValues(new[]{"businessTerm", "definition", "dictionaryEntryName", "fractionDigits", "languageCode", "length", "maximumExclusive", "maximumInclusive", "maximumLength", "minimumExclusive", "minimumInclusive", "minimumLength", "pattern", "totalDigits", "uniqueIdentifier", "versionIdentifier", "whiteSpace", }), "missing tagged values");
		}
	}
}
