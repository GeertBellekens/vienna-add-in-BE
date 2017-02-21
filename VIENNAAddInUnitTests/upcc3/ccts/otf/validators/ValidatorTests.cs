using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using VIENNAAddIn.upcc3;
using VIENNAAddIn.upcc3.otf;

namespace VIENNAAddInUnitTests.upcc3.ccts.otf.validators
{
    public abstract class ValidatorTests
    {
        private readonly TaggedValues[] definedTaggedValues;
        private readonly List<ItemId> expectedOffendingItemIds = new List<ItemId>();
        private readonly TaggedValues[] requiredTaggedValues;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="definedTaggedValues">Tagged Values defined for the validated item that may be empty.</param>
        /// <param name="requiredTaggedValues">Tagged Values defined for the validated item that must not be empty.</param>
        protected ValidatorTests(TaggedValues[] definedTaggedValues, TaggedValues[] requiredTaggedValues)
        {
            this.definedTaggedValues = definedTaggedValues;
            this.requiredTaggedValues = requiredTaggedValues;
        }

        protected static RepositoryItemBuilder ARepositoryItem
        {
            get { return new RepositoryItemBuilder(); }
        }

        protected static RepositoryItemBuilder AnElement
        {
            get { return ARepositoryItem.WithItemType(ItemId.ItemType.Element); }
        }

        protected static RepositoryItemBuilder APackage
        {
            get { return ARepositoryItem.WithItemType(ItemId.ItemType.Package); }
        }

        protected static RepositoryItemBuilder AModel
        {
            get { return ARepositoryItem.WithItemType(ItemId.ItemType.Package); }
        }

        protected abstract RepositoryItemBuilder DefaultItem { get; }

        [SetUp]
        public virtual void Context()
        {
            expectedOffendingItemIds.Clear();
        }

        protected static RepositoryItem AddChild(RepositoryItem parent, RepositoryItemBuilder itemBuilder)
        {
            RepositoryItem newItem = itemBuilder.Build();
            parent.AddOrReplaceChild(newItem);
            return newItem;
        }

        protected static RepositoryItem AddChild(RepositoryItem parent, ItemId.ItemType itemType)
        {
            return itemType == ItemId.ItemType.Package
                       ? AddChild(parent, ARepositoryItem
                                              .WithItemType(itemType).WithParent(new RepositoryItemBuilder().WithId(parent.Id)))
                       : AddChild(parent, ARepositoryItem
                                              .WithItemType(itemType).WithParent(new RepositoryItemBuilder().WithId(parent.Id)));
        }

        protected static RepositoryItem AddSubLibrary(RepositoryItem parent, string stereotype)
        {
            return AddChild(parent, ARepositoryItem
                                        .WithItemType(ItemId.ItemType.Package).WithParent(new RepositoryItemBuilder().WithId(parent.Id))
                                        .WithStereotype(stereotype));
        }

        protected static RepositoryItem AddElement(RepositoryItem library, string stereotype)
        {
            return AddChild(library, ARepositoryItem
                                         .WithItemType(ItemId.ItemType.Element).WithParent(new RepositoryItemBuilder().WithId(library.Id))
                                         .WithStereotype(stereotype));
        }

        protected void VerifyConstraintViolations(RepositoryItemBuilder validatedItem, params RepositoryItemBuilder[] offendingItems)
        {
            foreach (RepositoryItemBuilder item in offendingItems)
            {
                ExpectConstraintViolation(item);
            }
            Validate(validatedItem);
        }

        protected abstract IValidator Validator();

        protected void Validate(RepositoryItemBuilder validatedItemBuilder)
        {
            Validate(validatedItemBuilder, "");
        }

        protected void ExpectConstraintViolation(RepositoryItemBuilder offendingItemBuilder)
        {
            RepositoryItem offendingItem = offendingItemBuilder.Build();
            expectedOffendingItemIds.Add(offendingItem.Id);
        }

        [Test]
        public void ShouldDetectUndefinedTaggedValues()
        {
            foreach (TaggedValues taggedValue in definedTaggedValues)
            {
                RepositoryItemBuilder item = DefaultItem.WithoutTaggedValue(taggedValue);
                ExpectConstraintViolation(item);
                Validate(item, "Undefined Tagged Value '" + taggedValue + "' not detected.");
            }
        }

        private void Validate(RepositoryItemBuilder validatedItemBuilder, string message)
        {
            RepositoryItem validatedItem = validatedItemBuilder.Build();

            IValidator validator = Validator();
            Assert.That(validator.Matches(validatedItem), "validator does not match validated item");

            var constraintViolations = new List<ConstraintViolation>(validator.Validate(validatedItem));
            foreach (ConstraintViolation constraintViolation in constraintViolations)
            {
                Console.WriteLine(constraintViolation);
            }

            var actualOffendingItemIds = new List<ItemId>(validator.Validate(validatedItem).Select(constraintViolation => constraintViolation.OffendingItemId));
            Assert.That(actualOffendingItemIds, Is.EquivalentTo(expectedOffendingItemIds), message);
            expectedOffendingItemIds.Clear();
        }

        [Test]
        public void ShouldDetectEmptyTaggedValues()
        {
            foreach (TaggedValues taggedValue in requiredTaggedValues)
            {
                RepositoryItemBuilder item = DefaultItem.WithTaggedValue(taggedValue, string.Empty);
                ExpectConstraintViolation(item);
                Validate(item, "Empty required Tagged Value '" + taggedValue + "' not detected.");
            }
        }

        [Test]
        public void ShouldNotAllowAnEmptyName()
        {
            RepositoryItemBuilder item = DefaultItem.WithName(string.Empty);
            ExpectConstraintViolation(item);
            Validate(item, "Empty name not detected.");
        }
    }
}