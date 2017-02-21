using System.Collections.Generic;
using NUnit.Framework;
using VIENNAAddIn.upcc3;
using VIENNAAddIn.upcc3.otf;
using VIENNAAddIn.upcc3.otf.validators.constraints;

namespace VIENNAAddInUnitTests.upcc3.ccts.otf.validators.constraints
{
    [TestFixture]
    public class TaggedValuesMustBeDefinedConstraintTests : ConstraintTests
    {
        protected override IConstraint Constraint
        {
            get { return new TaggedValuesMustBeDefined(TaggedValues.owner, TaggedValues.pattern); }
        }

        protected override IEnumerable<RepositoryItemBuilder> ValidItems
        {
            get
            {
                yield return APackage.WithName("a package with the required tagged values defined").WithTaggedValues(new Dictionary<TaggedValues, string>
                                                                                                      {
                                                                                                          {TaggedValues.owner, ""},
                                                                                                          {TaggedValues.pattern, ""}
                                                                                                      });
            }
        }

        protected override IEnumerable<ExpectedViolation> ExpectedViolations
        {
            get
            {
                var aPackageWithoutAnyTaggedValues = APackage.WithStereotype("a package without any tagged values");
                yield return new ExpectedViolation(aPackageWithoutAnyTaggedValues, aPackageWithoutAnyTaggedValues, aPackageWithoutAnyTaggedValues);

                var aPackageWithoutOnlySomeTaggedValuesDefined = APackage.WithStereotype("a package with only some tagged values defined").WithTaggedValues(new Dictionary<TaggedValues, string>
                                                                                                      {
                                                                                                          {TaggedValues.owner, ""},
                                                                                                      });
                yield return new ExpectedViolation(aPackageWithoutOnlySomeTaggedValuesDefined, aPackageWithoutOnlySomeTaggedValuesDefined);
            }
        }
    }
}