using System;
using System.Collections.Generic;
using System.Text;
using EA;
using NUnit.Framework;
using VIENNAAddIn;
using VIENNAAddIn.upcc3;
using VIENNAAddIn.upcc3.otf;
using VIENNAAddInUnitTests.TestRepository;
using Stereotype=VIENNAAddIn.upcc3.Stereotype;

namespace VIENNAAddInUnitTests.upcc3.ccts.otf
{
    [TestFixture]
    public class RepositoryContentLoaderTests
    {
        private  RepositoryContentLoader contentLoader;
        private List<Package> packages;
        private List<Element> elements;
        private Package bLibrary;

        [SetUp]
        public void Context()
        {
            packages = new List<Package>();
            elements = new List<Element>();

            var eaRepository = new EARepository();

            var model = eaRepository.AddModel("Model", m => { });
            packages.Add(model);

            var otherPackage = CreatePackage(model, "other package");

            var bInformationV = CreatePackage(model, Stereotype.bInformationV);

            bLibrary = CreatePackage(bInformationV, Stereotype.bLibrary);

            CreatePackage(bLibrary, Stereotype.PRIMLibrary);
            CreatePackage(bLibrary, Stereotype.ENUMLibrary);
            CreatePackage(bLibrary, Stereotype.CDTLibrary);
            CreatePackage(bLibrary, Stereotype.CCLibrary);
            CreatePackage(bLibrary, Stereotype.BDTLibrary);
            CreatePackage(bLibrary, Stereotype.BIELibrary);
            CreatePackage(bLibrary, Stereotype.DOCLibrary);

            CreateElement(otherPackage, "other element");

            contentLoader = new RepositoryContentLoader(eaRepository);
        }

        private Package CreatePackage(Package parent, string stereotype)
        {
            Package package = parent.AddPackage(stereotype, p => p.Element.Stereotype = stereotype);
            packages.Add(package);
            return package;
        }

        private void CreateElement(Package parent, string stereotype)
        {
            elements.Add(parent.AddClass(stereotype).With(e => e.Stereotype = stereotype));
        }

        [Test]
        public void TestLoadAll()
        {
            contentLoader.ItemLoaded += RemoveItem;
            contentLoader.LoadRepositoryContent();
            Assert.AreEqual(0, packages.Count, "some packages where not loaded: " + ListToString(packages, package => (package.Element != null ? package.Element.Stereotype : "Model")));
            Assert.AreEqual(0, elements.Count, "some elements where not loaded: " + ListToString(elements, element => element.Stereotype));
        }

        [Test]
        public void When_a_package_element_is_reloaded_Then_the_package_should_be_reloaded()
        {
            // this is a fix for the problem that sometimes (e.g. when changing a package's tagged values),
            // EA generates an event for the package's internal element to have changed instead of the package itself.
            contentLoader.LoadRepositoryContent();
            RepositoryItem loadedItem = null;
            contentLoader.ItemLoaded += itemData => loadedItem = itemData;
            bLibrary.Element.SetTaggedValue(TaggedValues.baseURN, "test");
            contentLoader.LoadElementByID(bLibrary.Element.ElementID);
            Assert.AreEqual(ItemId.ForPackage(bLibrary.PackageID), loadedItem.Id);
        }

        private void RemoveItem(RepositoryItem item)
        {
            int numberOfItemsRemoved;
            if (item.Id.Type == ItemId.ItemType.Package)
            {
                numberOfItemsRemoved = packages.RemoveAll(package => package.PackageID == item.Id.Value);
            }
            else
            {
                numberOfItemsRemoved = elements.RemoveAll(element => element.ElementID == item.Id.Value);
            }
            Assert.AreEqual(1, numberOfItemsRemoved, "unexpected item loaded: " + item.Name);
        }

        private static string ListToString<TElement>(List<TElement> list, Func<TElement, string> toString)
        {
            var buf = new StringBuilder();
            buf.Append("{");
            foreach (var item in list)
            {
                buf.Append(toString(item)).Append(",");
            }
            buf.Append("}");
            return buf.ToString();
        }
    }
}