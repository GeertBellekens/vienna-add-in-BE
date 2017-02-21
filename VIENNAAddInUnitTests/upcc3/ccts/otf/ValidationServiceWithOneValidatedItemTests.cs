using System.Linq;
using Moq;
using NUnit.Framework;
using VIENNAAddIn.upcc3.otf;

namespace VIENNAAddInUnitTests.upcc3.ccts.otf
{
    [TestFixture]
    public class ValidationServiceWithOneValidatedItemTests
    {
        private ValidationService validationService;
        private RepositoryItem item;
        private ConstraintViolation[] itemConstraintViolations;
        private Mock<IValidator> validatorMock;

        [SetUp]
        public void Context()
        {
            validationService = new ValidationService();
            ItemId itemId = ItemId.ForElement(1);
            itemConstraintViolations = new[] { new ConstraintViolation(itemId, itemId, "A constraint has been violated.") };

            item = new RepositoryItemBuilder().WithId(itemId).Build();

            validatorMock = new Mock<IValidator>();
            validatorMock.Setup(c => c.Matches(It.IsAny<RepositoryItem>())).Returns(true);
            validatorMock.Setup(c => c.Validate(It.IsAny<RepositoryItem>())).Returns(itemConstraintViolations);
            validationService.AddValidator(validatorMock.Object);

            validationService.ItemCreatedOrModified(item);
            validationService.Validate();
        }

        [Test]
        public void When_the_item_is_modified_Then_its_validation_issues_should_be_removed()
        {
            validationService.ItemCreatedOrModified(item);
            Assert.AreEqual(0, validationService.ValidationIssues.Count());
        }

        [Test]
        public void When_the_item_is_modified_Then_it_should_be_revalidated()
        {
            validationService.ItemCreatedOrModified(item);
            validationService.Validate();
            Assert.AreEqual(1, validationService.ValidationIssues.Count());
            validatorMock.Verify(c => c.Validate(It.IsAny<RepositoryItem>()), Times.Exactly(2));
        }

        [Test]
        public void When_the_item_is_not_modified_Then_it_should_not_be_revalidated()
        {
            validationService.Validate();
            validatorMock.Verify(c => c.Validate(It.IsAny<RepositoryItem>()), Times.Exactly(1));
        }

        [Test]
        public void When_the_item_is_deleted_Then_its_validation_issues_should_be_removed()
        {
            validationService.ItemDeleted(item);
            Assert.AreEqual(0, validationService.ValidationIssues.Count());
        }

        [Test]
        public void When_the_item_is_modified_and_then_deleted_Then_it_should_not_be_validated()
        {
            validationService.ItemCreatedOrModified(item);
            validationService.ItemDeleted(item);
            validationService.Validate();
            validatorMock.Verify(c => c.Validate(It.IsAny<RepositoryItem>()), Times.Exactly(1));
        }
    }
}