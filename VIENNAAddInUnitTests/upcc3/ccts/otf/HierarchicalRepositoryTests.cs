using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using VIENNAAddIn.upcc3.otf;

namespace VIENNAAddInUnitTests.upcc3.ccts.otf
{
    [TestFixture]
    public class HierarchicalRepositoryTests
    {
        private HierarchicalRepository repository;

        [SetUp]
        public void Context()
        {
            repository = new HierarchicalRepository();
        }

        [Test]
        public void When_an_item_is_loaded_Then_it_should_be_retrievable_by_ID()
        {
            var item = new RepositoryItemBuilder().WithItemType(ItemId.ItemType.Package).Build();
            repository.ItemLoaded(item);
            Assert.IsNotNull(repository.GetItemById(item.Id), "The item is not retrievable.");
        }

        [Test]
        public void When_an_item_is_deleted_Then_it_should_no_longer_be_retrievable_by_ID()
        {
            var item = new RepositoryItemBuilder().WithItemType(ItemId.ItemType.Package).Build();
            repository.ItemLoaded(item);
            repository.ItemDeleted(item.Id);
            Assert.IsNull(repository.GetItemById(item.Id), "The item is still retrievable.");
        }

        [Test]
        public void When_an_item_with_parentId_0_is_loaded_Then_it_should_be_inserted_right_below_the_root()
        {
            var item = new RepositoryItemBuilder().WithItemType(ItemId.ItemType.Package).Build();
            repository.ItemLoaded(item);
            var root = repository.Root;
            var firstChild = root.Children.First();
            Assert.AreEqual(item.Id, firstChild.Id);
        }

        [Test]
        [ExpectedException(typeof (ArgumentException))]
        public void When_an_item_with_an_unknown_parentId_is_loaded_Then_it_should_throw_an_ArgumentException()
        {
            var item = new RepositoryItemBuilder().WithItemType(ItemId.ItemType.Package).WithParent(new RepositoryItemBuilder().WithId(ItemId.ForPackage(2))).Build();
            repository.ItemLoaded(item);
        }

        [Test]
        public void When_an_item_is_loaded_Then_it_should_be_added_as_a_child_to_its_parent()
        {
            var parent = new RepositoryItemBuilder().WithItemType(ItemId.ItemType.Package).Build();
            repository.ItemLoaded(parent);
            var child = new RepositoryItemBuilder().WithItemType(ItemId.ItemType.Package).WithParent(new RepositoryItemBuilder().WithId(parent.Id)).Build();
            repository.ItemLoaded(child);
            var retrievedParent = repository.GetItemById(parent.Id);
            var firstChild = retrievedParent.Children.First();
            Assert.AreEqual(child.Id, firstChild.Id);
        }

        [Test]
        public void When_an_item_is_loaded_Then_the_parents_previous_children_should_still_be_there()
        {
            var parentData = new RepositoryItemBuilder().WithItemType(ItemId.ItemType.Package).Build();
            repository.ItemLoaded(parentData);
            var parent = repository.GetItemById(parentData.Id);

            var child1 = new RepositoryItemBuilder().WithItemType(ItemId.ItemType.Package).WithParent(new RepositoryItemBuilder().WithId(parent.Id)).Build();
            repository.ItemLoaded(child1);
            Assert.AreEqual(1, parent.Children.Count());
            Assert.AreEqual(child1.Id, parent.Children.First().Id);

            var child2 = new RepositoryItemBuilder().WithItemType(ItemId.ItemType.Package).WithParent(new RepositoryItemBuilder().WithId(parent.Id)).Build();
            repository.ItemLoaded(child2);
            Assert.AreEqual(2, parent.Children.Count());
            Assert.AreEqual(child1.Id, parent.Children.First().Id);
        }

        [Test]
        public void When_an_item_is_deleted_Then_it_should_be_removed_from_its_parent()
        {
            var parentData = new RepositoryItemBuilder().WithItemType(ItemId.ItemType.Package).Build();
            repository.ItemLoaded(parentData);
            var childData = new RepositoryItemBuilder().WithItemType(ItemId.ItemType.Package).WithParent(new RepositoryItemBuilder().WithId(parentData.Id)).Build();
            repository.ItemLoaded(childData);
            repository.ItemDeleted(childData.Id);
            var parent = repository.GetItemById(parentData.Id);
            Assert.AreEqual(0, parent.Children.Count(), "parent still has a child");
        }

        [Test]
        public void When_an_item_is_reloaded_Then_the_old_item_should_be_replaced()
        {
            var parentData = new RepositoryItemBuilder().WithItemType(ItemId.ItemType.Package).Build();
            repository.ItemLoaded(parentData);

            var childBuilder = new RepositoryItemBuilder().WithItemType(ItemId.ItemType.Package).WithParent(new RepositoryItemBuilder().WithId(parentData.Id)).WithName("old");
            var childData = childBuilder.Build();
            repository.ItemLoaded(childData);

            childBuilder.WithName("new");
            var childDataNew = childBuilder.Build();
            repository.ItemLoaded(childDataNew);

            var parent = repository.GetItemById(parentData.Id);
            var firstChild = parent.Children.First();
            Assert.AreEqual(childDataNew.Name, firstChild.Name);

            var child = repository.GetItemById(childData.Id);
            Assert.AreEqual(childDataNew.Name, child.Name);
        }

        [Test]
        public void When_an_item_is_reloaded_Then_it_should_retain_its_children()
        {
            var parentData = new RepositoryItemBuilder().WithItemType(ItemId.ItemType.Package).Build();
            repository.ItemLoaded(parentData);
            var parent = repository.GetItemById(parentData.Id);

            var childData = new RepositoryItemBuilder().WithItemType(ItemId.ItemType.Package).WithParent(new RepositoryItemBuilder().WithId(parentData.Id)).Build();
            repository.ItemLoaded(childData);
            Assert.AreEqual(1, parent.Children.Count());

            repository.ItemLoaded(parentData);
            var parentReloaded = repository.GetItemById(parentData.Id);
            Assert.AreEqual(1, parentReloaded.Children.Count());
        }

        [Test]
        public void When_an_item_is_loaded_Then_a_modification_event_should_be_generated_for_the_item()
        {
            var parentData = new RepositoryItemBuilder().WithItemType(ItemId.ItemType.Package).Build();
            repository.ItemLoaded(parentData);

            var childData = new RepositoryItemBuilder().WithItemType(ItemId.ItemType.Package).WithParent(new RepositoryItemBuilder().WithId(parentData.Id)).Build();

            bool expectedEventGenerated = false;
            repository.OnItemCreatedOrModified += item =>
                                                  {
                                                      if (item.Id == childData.Id)
                                                      {
                                                          expectedEventGenerated = true;
                                                      }
                                                  };

            repository.ItemLoaded(childData);
            Assert.IsTrue(expectedEventGenerated, "the expected event was not generated");
        }

        [Test]
        public void When_an_item_is_loaded_Then_a_modification_event_should_be_generated_for_its_parent()
        {
            var parentData = new RepositoryItemBuilder().WithItemType(ItemId.ItemType.Package).Build();
            repository.ItemLoaded(parentData);

            var childData = new RepositoryItemBuilder().WithItemType(ItemId.ItemType.Package).WithParent(new RepositoryItemBuilder().WithId(parentData.Id)).Build();

            bool expectedEventGenerated = false;
            repository.OnItemCreatedOrModified += item =>
                                                  {
                                                      if (item.Id == parentData.Id)
                                                      {
                                                          expectedEventGenerated = true;
                                                      }
                                                  };

            repository.ItemLoaded(childData);
            Assert.IsTrue(expectedEventGenerated, "the expected event was not generated");
        }

        [Test]
        public void When_an_item_is_deleted_Then_a_deletion_event_should_be_generated_for_the_item()
        {
            var parentData = new RepositoryItemBuilder().WithItemType(ItemId.ItemType.Package).Build();
            var childData = new RepositoryItemBuilder().WithItemType(ItemId.ItemType.Package).WithParent(new RepositoryItemBuilder().WithId(parentData.Id)).Build();
            repository.ItemLoaded(parentData);
            repository.ItemLoaded(childData);

            bool expectedEventGenerated = false;
            repository.OnItemDeleted += item =>
                                        {
                                            if (item.Id == childData.Id)
                                            {
                                                expectedEventGenerated = true;
                                            }
                                        };

            repository.ItemDeleted(childData.Id);
            Assert.IsTrue(expectedEventGenerated, "the expected event was not generated");
        }

        [Test]
        public void When_an_item_is_deleted_Then_a_deletion_event_should_be_generated_for_its_children()
        {
            var parentData = new RepositoryItemBuilder().WithItemType(ItemId.ItemType.Package).Build();
            var childData1 = new RepositoryItemBuilder().WithItemType(ItemId.ItemType.Package).WithParent(new RepositoryItemBuilder().WithId(parentData.Id)).Build();
            var childData2 = new RepositoryItemBuilder().WithItemType(ItemId.ItemType.Package).WithParent(new RepositoryItemBuilder().WithId(parentData.Id)).Build();
            var childData3 = new RepositoryItemBuilder().WithItemType(ItemId.ItemType.Package).WithParent(new RepositoryItemBuilder().WithId(childData2.Id)).Build();
            repository.ItemLoaded(parentData);
            repository.ItemLoaded(childData1);
            repository.ItemLoaded(childData2);
            repository.ItemLoaded(childData3);

            var expectedItemIds = new List<ItemId>
                                  {
                                      parentData.Id,
                                      childData1.Id,
                                      childData2.Id,
                                      childData3.Id
                                  };
            repository.OnItemDeleted += item => expectedItemIds.Remove(item.Id);

            repository.ItemDeleted(parentData.Id);
            Assert.AreEqual(0, expectedItemIds.Count, "the expected events were not generated");
        }

        [Test]
        public void When_an_item_is_deleted_Then_a_modification_event_should_be_generated_for_its_parent()
        {
            var parentData = new RepositoryItemBuilder().WithItemType(ItemId.ItemType.Package).Build();
            var childData = new RepositoryItemBuilder().WithItemType(ItemId.ItemType.Package).WithParent(new RepositoryItemBuilder().WithId(parentData.Id)).Build();
            repository.ItemLoaded(parentData);
            repository.ItemLoaded(childData);

            bool expectedEventGenerated = false;
            repository.OnItemCreatedOrModified += item =>
                                                  {
                                                      if (item.Id == parentData.Id)
                                                      {
                                                          expectedEventGenerated = true;
                                                      }
                                                  };

            repository.ItemDeleted(childData.Id);
            Assert.IsTrue(expectedEventGenerated, "the expected event was not generated");
        }
    }
}