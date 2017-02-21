using System.Collections.Generic;
using NUnit.Framework;
using VIENNAAddIn.upcc3;
using VIENNAAddIn.upcc3.otf;
using VIENNAAddIn.upcc3.otf.validators.constraints;

namespace VIENNAAddInUnitTests.upcc3.ccts.otf.validators.constraints
{
    [TestFixture]
    public class ParentMustBeModelOrBInformationVOrBLibraryConstraintTests : ConstraintTests
    {
        protected override IConstraint Constraint
        {
            get { return new ParentMustBeModelOrBInformationVOrBLibrary(); }
        }

        protected override IEnumerable<RepositoryItemBuilder> ValidItems
        {
            get
            {
                yield return APackage.WithParent(APackageThatsNotAModel.WithStereotype(Stereotype.bInformationV).WithName("a package with a BInformationV parent"));
                yield return APackage.WithParent(APackageThatsNotAModel.WithStereotype(Stereotype.bLibrary).WithName("a package with a bLibrary parent"));
                yield return APackage.WithParent(AModel).WithName("a package with a Model parent");
            }
        }

        protected override IEnumerable<ExpectedViolation> ExpectedViolations
        {
            get
            {
                RepositoryItemBuilder aPackageWithoutAParent = APackage.WithName("a package without a parent");
                yield return new ExpectedViolation(aPackageWithoutAParent, aPackageWithoutAParent);

                RepositoryItemBuilder aPackageWithAnInvalidParent =
                    APackage.WithName("a package with an invalid parent")
                        .WithParent(APackageThatsNotAModel.WithStereotype("invalid stereotype"));
                yield return new ExpectedViolation(aPackageWithAnInvalidParent, aPackageWithAnInvalidParent);
            }
        }

        private static RepositoryItemBuilder APackageThatsNotAModel
        {
            get { return APackage.WithParent(APackage); }
        }
    }
}