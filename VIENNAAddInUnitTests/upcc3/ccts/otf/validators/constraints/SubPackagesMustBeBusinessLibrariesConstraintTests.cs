using System.Collections.Generic;
using NUnit.Framework;
using VIENNAAddIn.upcc3;
using VIENNAAddIn.upcc3.otf;
using VIENNAAddIn.upcc3.otf.validators.constraints;

namespace VIENNAAddInUnitTests.upcc3.ccts.otf.validators.constraints
{
    [TestFixture]
    public class SubPackagesMustBeBusinessLibrariesConstraintTests : ConstraintTests
    {
        protected override IConstraint Constraint
        {
            get { return new SubPackagesMustBeBusinessLibraries(); }
        }

        protected override IEnumerable<RepositoryItemBuilder> ValidItems
        {
            get
            {
                yield return APackage.WithName("a package without any sub-packages");
                yield return APackage.WithName("a package with business library sub-packages")
                    .WithChild(APackage.WithStereotype(Stereotype.PRIMLibrary))
                    .WithChild(APackage.WithStereotype(Stereotype.ENUMLibrary))
                    .WithChild(APackage.WithStereotype(Stereotype.CDTLibrary))
                    .WithChild(APackage.WithStereotype(Stereotype.CCLibrary))
                    .WithChild(APackage.WithStereotype(Stereotype.BDTLibrary))
                    .WithChild(APackage.WithStereotype(Stereotype.BIELibrary))
                    .WithChild(APackage.WithStereotype(Stereotype.DOCLibrary))
                    ;
            }
        }

        protected override IEnumerable<ExpectedViolation> ExpectedViolations
        {
            get
            {
                var aPackageWithANonBusinessLibraryStereotype = APackage.WithStereotype("non-business-library");
                RepositoryItemBuilder aPackageWithANonBusinessLibrarySubPackage = APackage.WithName("a package with a sub-package with a non-business-library stereotype")
                    .WithChild(aPackageWithANonBusinessLibraryStereotype);
                yield return new ExpectedViolation(aPackageWithANonBusinessLibrarySubPackage, aPackageWithANonBusinessLibraryStereotype);

                var aPackageWithoutAStereotype = APackage;
                RepositoryItemBuilder aPackageWithASubPackageWithoutAStereotype = APackage.WithName("a package with a sub-package with a non-business-library stereotype")
                    .WithChild(aPackageWithoutAStereotype);
                yield return new ExpectedViolation(aPackageWithASubPackageWithoutAStereotype, aPackageWithoutAStereotype);
            }
        }
    }
}