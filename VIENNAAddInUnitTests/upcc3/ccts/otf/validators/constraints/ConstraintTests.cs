using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using VIENNAAddIn.upcc3.otf;

namespace VIENNAAddInUnitTests.upcc3.ccts.otf.validators.constraints
{
    public abstract class ConstraintTests
    {
        protected abstract IConstraint Constraint { get; }
        protected abstract IEnumerable<RepositoryItemBuilder> ValidItems { get; }
        protected abstract IEnumerable<ExpectedViolation> ExpectedViolations { get; }

        protected static RepositoryItemBuilder APackage
        {
            get { return new RepositoryItemBuilder().WithItemType(ItemId.ItemType.Package); }
        }

        /// <summary>
        /// A model is a package without a parent.
        /// </summary>
        protected static RepositoryItemBuilder AModel
        {
            get { return APackage; }
        }

        protected static RepositoryItemBuilder AnElement
        {
            get { return new RepositoryItemBuilder().WithItemType(ItemId.ItemType.Element); }
        }

        [Test]
        public void ShouldNotReportViolationsForValidItems()
        {
            foreach (RepositoryItemBuilder item in ValidItems)
            {
                RepositoryItem validatedItem = item.Build();
                var violations = new List<ConstraintViolation>(Constraint.Check(validatedItem));
                Assert.That(violations, Is.Empty, "" + violations.Count + " constraint violation(s) reported for " + validatedItem.Name + ", which should be valid.");
            }
        }

        [Test]
        public void ShouldReportViolationsForInvalidItems()
        {
            foreach (ExpectedViolation expectedViolation in ExpectedViolations)
            {
                RepositoryItem validatedItem = expectedViolation.ValidatedItem.Build();
                IEnumerable<ConstraintViolation> actualViolations = Constraint.Check(validatedItem);
                var actualOffendingItemIds = new List<ItemId>(actualViolations.Select(v => v.OffendingItemId));
                Assert.That(actualOffendingItemIds, Is.EquivalentTo(expectedViolation.OffendingItemIds), "Unexpected constraint violation(s) reported for " + validatedItem.Name + ". Offending items: ");
            }
        }

        #region Nested type: ExpectedViolation

        protected class ExpectedViolation
        {
            public ExpectedViolation(RepositoryItemBuilder validatedItem, params RepositoryItemBuilder[] offendingItems)
            {
                ValidatedItem = validatedItem;
                OffendingItems = offendingItems;
            }

            internal RepositoryItemBuilder ValidatedItem { get; private set; }
            internal RepositoryItemBuilder[] OffendingItems { get; private set; }

            public List<ItemId> OffendingItemIds
            {
                get
                {
                    var offendingItemIds = new List<ItemId>();
                    foreach (RepositoryItemBuilder item in OffendingItems)
                    {
                        offendingItemIds.Add(item.Build().Id);
                    }
                    return offendingItemIds;
                }
            }
        }

        #endregion
    }
}