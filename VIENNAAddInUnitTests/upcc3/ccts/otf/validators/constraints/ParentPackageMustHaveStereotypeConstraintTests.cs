using System.Collections.Generic;
using NUnit.Framework;
using VIENNAAddIn.upcc3.otf;
using VIENNAAddIn.upcc3.otf.validators.constraints;

namespace VIENNAAddInUnitTests.upcc3.ccts.otf.validators.constraints
{
    [TestFixture]
    public class ParentPackageMustHaveStereotypeConstraintTests : ConstraintTests
    {
        private const string ExpectedStereotype = "expected_stereotype";

        protected override IConstraint Constraint
        {
            get { return new ParentPackageMustHaveStereotype(ExpectedStereotype); }
        }

        protected override IEnumerable<RepositoryItemBuilder> ValidItems
        {
            get { yield return APackage.WithParent(APackage.WithStereotype(ExpectedStereotype).WithName("a package with a parent with the expected stereotype")); }
        }

        protected override IEnumerable<ExpectedViolation> ExpectedViolations
        {
            get
            {
                RepositoryItemBuilder aPackageWithoutAParent = APackage.WithName("a package without a parent");
                yield return new ExpectedViolation(aPackageWithoutAParent, aPackageWithoutAParent);

                RepositoryItemBuilder aPackageWithAParentWithAnInvalidStereotype =
                    APackage.WithName("a package with a parent with an invalid stereotype")
                        .WithParent(APackage.WithStereotype("other than the" + ExpectedStereotype));
                yield return new ExpectedViolation(aPackageWithAParentWithAnInvalidStereotype, aPackageWithAParentWithAnInvalidStereotype);
            }
        }

        private static RepositoryItemBuilder APackageThatsNotAModel
        {
            get { return APackage.WithParent(APackage); }
        }
    }
}