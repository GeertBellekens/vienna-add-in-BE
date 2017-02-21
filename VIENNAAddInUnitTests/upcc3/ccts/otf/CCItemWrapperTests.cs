using CctsRepository.BdtLibrary;
using CctsRepository.BieLibrary;
using CctsRepository.BLibrary;
using CctsRepository.CcLibrary;
using CctsRepository.CdtLibrary;
using CctsRepository.DocLibrary;
using CctsRepository.EnumLibrary;
using CctsRepository.PrimLibrary;
using NUnit.Framework;
using VIENNAAddIn.upcc3;
using VIENNAAddIn.upcc3.otf;

namespace VIENNAAddInUnitTests.upcc3.ccts.otf
{
    [TestFixture]
    public class CCItemWrapperTests
    {
        private static void AssertPackageWithStereotypeIsWrappedAs<TExpected>(string stereotype)
        {
            var parent = new RepositoryItemBuilder().WithParent(new RepositoryItemBuilder().WithId(ItemId.ForPackage(1))).WithItemType(ItemId.ItemType.Package).Build();
            var repositoryItem = new RepositoryItemBuilder()
                .WithItemType(ItemId.ItemType.Package).WithParent(new RepositoryItemBuilder().WithId(parent.Id))
                .WithStereotype(stereotype)
                .Build();
            object wrappedObject = CCItemWrapper.Wrap(repositoryItem);
            Assert.IsNotNull(wrappedObject, "Wrapped object is null.");
            Assert.IsTrue(wrappedObject is TExpected, "\nWrapped object is not of expected type:\n  expected: " + typeof (TExpected) + "\n  but was:  " + wrappedObject.GetType());
        }

        [Test]
        public void When_a_package_has_a_non_UPCC_stereotype_Then_return_null()
        {
            var repositoryItem = new RepositoryItemBuilder().WithParent(new RepositoryItemBuilder().WithId(ItemId.ForPackage(1)))
                .WithItemType(ItemId.ItemType.Package)
                .WithStereotype("foobar")
                .Build();
            object wrappedObject = CCItemWrapper.Wrap(repositoryItem);
            Assert.IsNull(wrappedObject, "Object with unknown stereotype not wrapped to null, but wrapped as " + (wrappedObject == null ? null : wrappedObject.GetType()));
        }

        [Test]
        public void When_a_package_with_stereotype_bLibrary_is_wrapped_Then_create_an_IBLibrary()
        {
            AssertPackageWithStereotypeIsWrappedAs<IBLibrary>(Stereotype.bLibrary);
        }

        [Test]
        public void When_a_package_with_stereotype_PRIMLibrary_is_wrapped_Then_create_an_IPRIMLibrary()
        {
            AssertPackageWithStereotypeIsWrappedAs<IPrimLibrary>(Stereotype.PRIMLibrary);
        }

        [Test]
        public void When_a_package_with_stereotype_ENUMLibrary_is_wrapped_Then_create_an_IENUMLibrary()
        {
            AssertPackageWithStereotypeIsWrappedAs<IEnumLibrary>(Stereotype.ENUMLibrary);
        }

        [Test]
        public void When_a_package_with_stereotype_CDTLibrary_is_wrapped_Then_create_an_ICDTLibrary()
        {
            AssertPackageWithStereotypeIsWrappedAs<ICdtLibrary>(Stereotype.CDTLibrary);
        }

        [Test]
        public void When_a_package_with_stereotype_CCLibrary_is_wrapped_Then_create_an_ICCLibrary()
        {
            AssertPackageWithStereotypeIsWrappedAs<ICcLibrary>(Stereotype.CCLibrary);
        }

        [Test]
        public void When_a_package_with_stereotype_BDTLibrary_is_wrapped_Then_create_an_IBDTLibrary()
        {
            AssertPackageWithStereotypeIsWrappedAs<IBdtLibrary>(Stereotype.BDTLibrary);
        }

        [Test]
        public void When_a_package_with_stereotype_BIELibrary_is_wrapped_Then_create_an_IBIELibrary()
        {
            AssertPackageWithStereotypeIsWrappedAs<IBieLibrary>(Stereotype.BIELibrary);
        }

        [Test]
        public void When_a_package_with_stereotype_DOCLibrary_is_wrapped_Then_create_an_IDOCLibrary()
        {
            AssertPackageWithStereotypeIsWrappedAs<IDocLibrary>(Stereotype.DOCLibrary);
        }
    }
}