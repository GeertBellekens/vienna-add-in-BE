using System.Collections.Generic;
using NUnit.Framework;
using VIENNAAddIn.upcc3;
using VIENNAAddIn.upcc3.otf;

namespace VIENNAAddInUnitTests.upcc3.ccts.otf.validators
{
    public abstract class BusinessLibraryValidatorTests : ValidatorTests
    {
        private static readonly TaggedValues[] DefinedTaggedValues = new[]
                                                                     {
                                                                         TaggedValues.businessTerm,
                                                                         TaggedValues.copyright,
                                                                         TaggedValues.owner,
                                                                         TaggedValues.reference,
                                                                         TaggedValues.status,
                                                                         TaggedValues.namespacePrefix,
                                                                     };

        private static readonly TaggedValues[] RequiredTaggedValues = new[]
                                                                      {
                                                                          TaggedValues.uniqueIdentifier,
                                                                          TaggedValues.versionIdentifier,
                                                                          TaggedValues.baseURN,
                                                                      };

        protected readonly string libraryStereotype;

        protected BusinessLibraryValidatorTests(string libraryStereotype) : base(DefinedTaggedValues, RequiredTaggedValues)
        {
            this.libraryStereotype = libraryStereotype;
        }

        protected override RepositoryItemBuilder DefaultItem
        {
            get
            {
                return APackage
                    .WithStereotype(libraryStereotype)
                    .WithParent(APackage.WithStereotype(Stereotype.bLibrary))
                    .WithTaggedValues(new Dictionary<TaggedValues, string>
                                      {
                                          {TaggedValues.uniqueIdentifier, "foo"},
                                          {TaggedValues.versionIdentifier, "foo"},
                                          {TaggedValues.baseURN, "foo"},
                                          {TaggedValues.businessTerm, ""},
                                          {TaggedValues.copyright, ""},
                                          {TaggedValues.owner, ""},
                                          {TaggedValues.reference, ""},
                                          {TaggedValues.status, ""},
                                          {TaggedValues.namespacePrefix, ""},
                                      });
            }
        }

        [Test]
        public void ShouldOnlyMatchPackagesWithTheProperStereotype()
        {
            AssertValidatorDoesNotMatch(ARepositoryItem.WithItemType(ItemId.ItemType.Package).WithStereotype("other than " + libraryStereotype));
        }

        [Test]
        public void ShouldNotMatchElements()
        {
            AssertValidatorDoesNotMatch(ARepositoryItem.WithItemType(ItemId.ItemType.Element).WithStereotype(libraryStereotype));
        }

        private void AssertValidatorDoesNotMatch(RepositoryItemBuilder repositoryItemBuilder)
        {
            RepositoryItem repositoryItem = repositoryItemBuilder.Build();
            Assert.IsFalse(Validator().Matches(repositoryItem), libraryStereotype + " validator wrongly matches a " + repositoryItem.Id.Type + " with stereotype " + repositoryItem.Stereotype + ".");
        }
    }
}