using System;
using System.Collections.Generic;
using NUnit.Framework;
using VIENNAAddIn.upcc3.otf;
using VIENNAAddIn.upcc3.otf.validators.constraints;

namespace VIENNAAddInUnitTests.upcc3.ccts.otf.validators.constraints
{
    [TestFixture]
    public class ElementsMustHaveStereotypeConstraintTests : ConstraintTests
    {
        private const string ExpectedStereotype = "valid_stereotype";

        protected override IConstraint Constraint
        {
            get { return new ElementsMustHaveStereotype(ExpectedStereotype); }
        }

        protected override IEnumerable<RepositoryItemBuilder> ValidItems
        {
            get
            {
                yield return APackage;
                yield return APackage
                    .WithChild(AnElementWithTheExpectedStereotype());
                yield return APackage
                    .WithChild(AnElementWithTheExpectedStereotype())
                    .WithChild(AnElementWithTheExpectedStereotype());
            }
        }

        private static RepositoryItemBuilder AnElementWithTheExpectedStereotype()
        {
            return AnElement.WithStereotype(ExpectedStereotype);
        }

        protected override IEnumerable<ExpectedViolation> ExpectedViolations
        {
            get
            {
                var anElementWithAnInvalidStereotype = AnElement.WithStereotype("other than " + ExpectedStereotype).WithName("an element with an invalid stereotype");
                yield return new ExpectedViolation(APackage
                    .WithChild(anElementWithAnInvalidStereotype), 
                    anElementWithAnInvalidStereotype);
                yield return new ExpectedViolation(APackage
                    .WithChild(AnElementWithTheExpectedStereotype())
                    .WithChild(anElementWithAnInvalidStereotype), 
                    anElementWithAnInvalidStereotype);
            }
        }
    }
}