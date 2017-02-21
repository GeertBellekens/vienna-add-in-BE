using NUnit.Framework;
using VIENNAAddIn.upcc3.otf;
using VIENNAAddIn.upcc3.otf.validators;

namespace VIENNAAddInUnitTests.upcc3.ccts.otf.validators
{
    public abstract class ElementLibraryValidatorTests : BusinessLibraryValidatorTests
    {
        private readonly string elementStereotype;

        protected ElementLibraryValidatorTests(string libraryStereotype, string elementStereotype) : base(libraryStereotype)
        {
            this.elementStereotype = elementStereotype;
        }

        protected override IValidator Validator()
        {
            return new ElementLibaryValidator(libraryStereotype, elementStereotype);
        }

        [Test]
        public void ShouldNotAllowAnySubpackages()
        {
            RepositoryItemBuilder aPackage = APackage;
            RepositoryItemBuilder elementLibrary = DefaultItem.WithChild(aPackage);
            VerifyConstraintViolations(elementLibrary, aPackage);
        }

        [Test]
        public void ShouldOnlyAllowABLibraryAsParent()
        {
            RepositoryItemBuilder elementLibrary = DefaultItem.WithParent(APackage.WithStereotype("other than bLibrary"));
            VerifyConstraintViolations(elementLibrary, elementLibrary);
        }

        [Test]
        public void ShouldOnlyAllowElementsWithTheProperStereotype()
        {
            RepositoryItemBuilder element1 = AnElement.WithStereotype(elementStereotype);
            RepositoryItemBuilder element2 = AnElement.WithStereotype("other than " + elementStereotype);
            RepositoryItemBuilder elementLibrary = DefaultItem.WithChild(element1).WithChild(element2);
            ExpectConstraintViolation(element2);
            Validate(elementLibrary);
        }
    }
}