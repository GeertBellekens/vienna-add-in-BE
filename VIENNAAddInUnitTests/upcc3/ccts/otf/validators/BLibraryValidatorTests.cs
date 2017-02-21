using NUnit.Framework;
using VIENNAAddIn.upcc3;
using VIENNAAddIn.upcc3.otf;
using VIENNAAddIn.upcc3.otf.validators;

namespace VIENNAAddInUnitTests.upcc3.ccts.otf.validators
{
    [TestFixture]
    public class BLibraryValidatorTests : BusinessLibraryValidatorTests
    {
        public BLibraryValidatorTests() : base(Stereotype.bLibrary)
        {
        }

        protected override IValidator Validator()
        {
            return new BLibraryValidator();
        }

        [Test]
        public void ShouldAllowABInformationVAsParent()
        {
            VerifyConstraintViolations(DefaultItem.WithParent(APackage.WithStereotype(Stereotype.bInformationV).WithParent(APackage)));
        }

        [Test]
        public void ShouldAllowABLibraryAsParent()
        {
            VerifyConstraintViolations(DefaultItem.WithParent(APackage.WithStereotype(Stereotype.bLibrary).WithParent(APackage)));
        }

        [Test]
        public void ShouldAllowAModelAsParent()
        {
            VerifyConstraintViolations(DefaultItem.WithParent(AModel));
        }

        [Test]
        public void ShouldNotAllowAnInvalidParent()
        {
            RepositoryItemBuilder bLibrary = DefaultItem.WithParent(APackage.WithStereotype("SOME-OTHER-KIND-OF-PACKAGE").WithParent(APackage));
            VerifyConstraintViolations(bLibrary, bLibrary);
        }

        [Test]
        public void ShouldNotAllowElements()
        {
            RepositoryItemBuilder element1 = AnElement;
            RepositoryItemBuilder element2 = AnElement;
            RepositoryItemBuilder bLibrary = DefaultItem.WithChild(element1).WithChild(element2);
            VerifyConstraintViolations(bLibrary, element1, element2);
        }

        [Test]
        public void ShouldOnlyAllowBusinessLibrariesAsSubpackages()
        {
            RepositoryItemBuilder invalidSubPackage = APackage.WithStereotype("other-that-a-business-library-stereotype");
            RepositoryItemBuilder bLibrary = DefaultItem
                .WithChild(APackage.WithStereotype(Stereotype.bLibrary))
                .WithChild(APackage.WithStereotype(Stereotype.PRIMLibrary))
                .WithChild(APackage.WithStereotype(Stereotype.ENUMLibrary))
                .WithChild(APackage.WithStereotype(Stereotype.CDTLibrary))
                .WithChild(APackage.WithStereotype(Stereotype.CCLibrary))
                .WithChild(APackage.WithStereotype(Stereotype.BDTLibrary))
                .WithChild(APackage.WithStereotype(Stereotype.BIELibrary))
                .WithChild(APackage.WithStereotype(Stereotype.DOCLibrary))
                .WithChild(invalidSubPackage);
            VerifyConstraintViolations(bLibrary, invalidSubPackage);
        }
    }
}