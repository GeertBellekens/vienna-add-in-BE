using System.Collections.Generic;
using NUnit.Framework;
using VIENNAAddIn.upcc3.otf;
using VIENNAAddIn.upcc3.otf.validators.constraints;

namespace VIENNAAddInUnitTests.upcc3.ccts.otf.validators.constraints
{
    [TestFixture]
    public class NameMustNotBeEmptyConstraintTests : ConstraintTests
    {
        protected override IConstraint Constraint
        {
            get { return new NameMustNotBeEmpty(); }
        }

        protected override IEnumerable<RepositoryItemBuilder> ValidItems
        {
            get
            {
                yield return APackage.WithName("a package with a name");
                yield return AnElement.WithName("an element with a name");
            }
        }

        protected override IEnumerable<ExpectedViolation> ExpectedViolations
        {
            get
            {
                RepositoryItemBuilder aPackageWithoutAName = APackage.WithName(string.Empty);
                yield return new ExpectedViolation(aPackageWithoutAName, aPackageWithoutAName);

                RepositoryItemBuilder anElementWithoutAName = AnElement.WithName(string.Empty);
                yield return new ExpectedViolation(anElementWithoutAName, anElementWithoutAName);
            }
        }
    }
}