using System;
using System.Collections.Generic;
using CctsRepository.BLibrary;
using EA;
using NUnit.Framework;
using VIENNAAddIn;
using VIENNAAddIn.upcc3;
using VIENNAAddIn.upcc3.otf;
using VIENNAAddInUnitTests.TestRepository;
using Stereotype=VIENNAAddIn.upcc3.Stereotype;

namespace VIENNAAddInUnitTests.upcc3.ccts.otf
{
    /// <summary>
    /// These are tests for a complete ValidatingCCRepository, so in fact they are testing the integration of
    /// the various components of the ValidatingCCRepository.
    /// </summary>
    [TestFixture]
    public class ValidatingCCRepositoryIntegrationTests
    {
        #region Setup/Teardown

        [SetUp]
        public void Context()
        {
            eaRepository = new EARepository();
            bLibrary = null;
            eaRepository.AddModel("Model", model => { bLibrary = ABLibraryWithoutIssues("bLibrary", model); });

            validatingCCRepository = new ValidatingCCRepository(eaRepository);
            issueHandler = new ValidationIssueHandler();
            validatingCCRepository.ValidationIssuesUpdated += issueHandler.ValidationIssuesUpdated;
            validatingCCRepository.LoadRepositoryContent();
            issueHandler.AssertReceivedIssuesTotal(0);
            issueHandler.Reset();
        }

        #endregion

        private EARepository eaRepository;
        private Package bLibrary;
        private ValidationIssueHandler issueHandler;
        private ValidatingCCRepository validatingCCRepository;

        private static Package ABLibraryWithOneIssue(string name, Package parent)
        {
            Package newBLibrary = ABLibraryWithoutIssues(name, parent);
            // introduce a constraint violation by setting a required tagged value to ""
            newBLibrary.Element.SetTaggedValue(TaggedValues.baseURN, string.Empty);
            return newBLibrary;
        }

        private static Package ABLibraryWithoutIssues(string name, Package parent)
        {
            return AddBLibrary(name, parent);
        }

        private static Package AddBLibrary(string name, Package parent)
        {
            var bLibrary = parent.AddPackage(name, p => { p.Element.Stereotype = Stereotype.bLibrary; });
            foreach (TaggedValues tv in new[] {TaggedValues.businessTerm, TaggedValues.copyright, TaggedValues.owner, TaggedValues.reference, TaggedValues.status, TaggedValues.namespacePrefix})
            {
                bLibrary.AddTaggedValue(tv.ToString());
            }
            foreach (TaggedValues tv in new[] {TaggedValues.uniqueIdentifier, TaggedValues.versionIdentifier, TaggedValues.baseURN})
            {
                bLibrary.AddTaggedValue(tv.ToString()).WithValue("foo");
            }
            return bLibrary;
        }

        [Test]
        public void When_a_package_is_deleted_Then_its_descendants_should_no_longer_be_in_the_repository()
        {
            Package newBLibrary1 = ABLibraryWithoutIssues("New bLibrary 1", bLibrary);
            validatingCCRepository.LoadPackageByID(newBLibrary1.PackageID);

            Package newBLibrary2 = ABLibraryWithoutIssues("New bLibrary 2", newBLibrary1);
            validatingCCRepository.LoadPackageByID(newBLibrary2.PackageID);

            validatingCCRepository.ItemDeleted(ItemId.ForPackage(newBLibrary1.PackageID));

            var bLibraries = new List<IBLibrary>(validatingCCRepository.GetBLibraries());

            Assert.AreEqual(1, bLibraries.Count, "Wrong number of BLibraries");
            Assert.AreEqual(bLibrary.PackageID, bLibraries[0].Id);
        }

        [Test]
        public void When_a_package_is_deleted_Then_its_descendants_validation_issues_should_be_removed()
        {
            Package bLibraryWithoutIssues = ABLibraryWithoutIssues("BLibrary2", bLibrary);
            Package aBLibraryWithOneIssue = ABLibraryWithOneIssue("BLibrary3", bLibraryWithoutIssues);

            validatingCCRepository.LoadPackageByID(bLibraryWithoutIssues.PackageID);
            validatingCCRepository.LoadPackageByID(aBLibraryWithOneIssue.PackageID);
            issueHandler.AssertReceivedIssuesTotal(1);
            issueHandler.AssertReceivedIssues(1, aBLibraryWithOneIssue.PackageID, aBLibraryWithOneIssue.PackageID);

            validatingCCRepository.ItemDeleted(ItemId.ForPackage(bLibraryWithoutIssues.PackageID));
            issueHandler.AssertReceivedIssuesTotal(0);
        }

        [Test]
        public void When_a_package_is_deleted_Then_its_parent_package_should_be_validated()
        {
            Package subPackage = bLibrary.AddPackage("Package 1.1", package_1_1 => { package_1_1.Element.Stereotype = "other package"; });

            validatingCCRepository.LoadPackageByID(subPackage.PackageID);
            issueHandler.AssertReceivedIssuesTotal(1);
            issueHandler.AssertReceivedIssues(1, bLibrary.PackageID, subPackage.PackageID);

            validatingCCRepository.ItemDeleted(ItemId.ForPackage(subPackage.PackageID));
            issueHandler.AssertReceivedIssuesTotal(0);
        }

        [Test]
        public void When_a_package_is_deleted_Then_its_validation_issues_should_be_removed()
        {
            Package newBLibrary = ABLibraryWithOneIssue("BLibrary2", bLibrary);

            validatingCCRepository.LoadPackageByID(newBLibrary.PackageID);
            issueHandler.AssertReceivedIssuesTotal(1);
            issueHandler.AssertReceivedIssues(1, newBLibrary.PackageID, newBLibrary.PackageID);

            validatingCCRepository.ItemDeleted(ItemId.ForPackage(newBLibrary.PackageID));
            issueHandler.AssertReceivedIssuesTotal(0);
        }

        [Test]
        public void When_a_package_is_deleted_Then_the_package_should_no_longer_be_in_the_repository()
        {
            Package newBLibrary = ABLibraryWithoutIssues("New BLibrary", bLibrary);

            validatingCCRepository.LoadPackageByID(newBLibrary.PackageID);

            validatingCCRepository.ItemDeleted(ItemId.ForPackage(newBLibrary.PackageID));
            var bLibraries = new List<IBLibrary>(validatingCCRepository.GetBLibraries());
            Assert.AreEqual(1, bLibraries.Count, "Wrong number of BLibraries");
            Assert.AreEqual(bLibrary.PackageID, bLibraries[0].Id);
        }

        [Test]
        public void When_a_package_is_loaded_Then_it_should_be_in_the_repository()
        {
            Package newBLibrary = ABLibraryWithoutIssues("New bLibrary", bLibrary);

            validatingCCRepository.LoadPackageByID(newBLibrary.PackageID);
            var bLibraries = new List<IBLibrary>(validatingCCRepository.GetBLibraries());
            Assert.AreEqual(2, bLibraries.Count, "Wrong number of BLibraries");
            Assert.AreEqual(bLibrary.PackageID, bLibraries[0].Id);
            Assert.AreEqual(newBLibrary.PackageID, bLibraries[1].Id);
        }

        [Test]
        public void When_a_package_is_loaded_Then_its_parent_package_should_be_validated()
        {
            Package subPackage = bLibrary.AddPackage("Package 1.1", package_1_1 => { package_1_1.Element.Stereotype = "other package"; });
            validatingCCRepository.LoadPackageByID(subPackage.PackageID);
            // expect one issue for invalid sub-package
            issueHandler.AssertReceivedIssuesTotal(1);
            issueHandler.AssertReceivedIssues(1, bLibrary.PackageID, subPackage.PackageID);
        }

        [Test]
        public void When_a_package_is_loaded_Then_the_package_should_be_validated()
        {
            Package aBLibraryWithOneIssue = ABLibraryWithOneIssue("New bLibrary", bLibrary);
            validatingCCRepository.LoadPackageByID(aBLibraryWithOneIssue.PackageID);
            // expect issues for missing tagged value
            issueHandler.AssertReceivedIssuesTotal(1);
            issueHandler.AssertReceivedIssues(1, aBLibraryWithOneIssue.PackageID, aBLibraryWithOneIssue.PackageID);
        }

        [Test]
        public void When_an_element_is_deleted_Then_it_should_no_longer_be_in_the_repository()
        {
            Assert.Ignore("Cannot be tested, because no element libraries are implemented yet.");
        }

        [Test]
        public void When_an_element_is_deleted_Then_its_package_should_be_validated()
        {
            Element element = bLibrary.AddClass("An element");
            validatingCCRepository.LoadElementByID(element.ElementID);
            validatingCCRepository.ItemDeleted(ItemId.ForElement(element.ElementID));
            issueHandler.AssertReceivedIssuesTotal(0);
        }

        [Test]
        public void When_an_element_is_deleted_Then_its_validation_issues_should_be_removed()
        {
            Assert.Ignore("Cannot be tested, because no element validations are implemented yet.");
        }

        [Test]
        public void When_an_element_is_loaded_Then_it_should_be_in_the_repository()
        {
            Assert.Ignore("Cannot be tested, because no element libraries are implemented yet.");
        }

        [Test]
        public void When_an_element_is_loaded_Then_it_should_be_validated()
        {
            Assert.Ignore("Cannot be tested, because no element validations are implemented yet.");
        }

        [Test]
        public void When_an_element_is_loaded_Then_its_package_should_be_validated()
        {
            Element element = bLibrary.AddClass("An element");
            validatingCCRepository.LoadElementByID(element.ElementID);
            // expect one issue for invalid element
            issueHandler.AssertReceivedIssuesTotal(1);
            issueHandler.AssertReceivedIssues(1, bLibrary.PackageID, element.ElementID);
        }
    }

    public class ValidationIssueHandler
    {
        private List<ValidationIssue> issues;

        public void ValidationIssuesUpdated(IEnumerable<ValidationIssue> updatedIssues)
        {
            issues = new List<ValidationIssue>(updatedIssues);
            Console.WriteLine("received issues:");
            foreach (ValidationIssue issue in updatedIssues)
            {
                Console.WriteLine("  " + issue.ConstraintViolation.Message);
            }
        }

        public void AssertReceivedIssuesTotal(int count)
        {
            Assert.IsNotNull(issues, "No issues have been received, but expected a total number of " + count + " issues.");
            Assert.AreEqual(count, issues.Count, "Wrong total number of issues received.");
        }

        public void AssertReceivedIssues(int count, int validatedItemId, int itemId)
        {
            Assert.AreEqual(count, issues.FindAll(AnIssueWith(itemId, validatedItemId)).Count, "The expected number of issues with ItemId=" + itemId + " and ValidatedItemId=" + validatedItemId + " has not been received.");
        }

        private static Predicate<ValidationIssue> AnIssueWith(int itemId, int validatedItemId)
        {
            return issue => issue.ConstraintViolation.OffendingItemId.Value == itemId && issue.ConstraintViolation.ValidatedItemId.Value == validatedItemId;
        }

        public void Reset()
        {
            issues = null;
        }
    }
}