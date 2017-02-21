using System.Collections.Generic;
using NUnit.Framework;
using VIENNAAddIn.upcc3.otf;
using VIENNAAddIn.upcc3.otf.validators.constraints;

namespace VIENNAAddInUnitTests.upcc3.ccts.otf.validators.constraints
{
    [TestFixture]
    public class MustNotContainAnyElementsConstraintTests : ConstraintTests
    {
        protected override IConstraint Constraint
        {
            get { return new MustNotContainAnyElements(); }
        }

        protected override IEnumerable<RepositoryItemBuilder> ValidItems
        {
            get
            {
                yield return APackage.WithName("a package without any children");
                yield return APackage.WithName("a package with a single sub-package").WithChild(APackage);
            }
        }

        protected override IEnumerable<ExpectedViolation> ExpectedViolations
        {
            get
            {
                RepositoryItemBuilder anElement = AnElement;
                yield return new ExpectedViolation(APackage.WithName("a package with a single element").WithChild(anElement), anElement);
            }
        }
    }
}