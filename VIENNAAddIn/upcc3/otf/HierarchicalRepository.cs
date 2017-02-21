using System;
using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.otf
{
    public class HierarchicalRepository
    {
        private readonly Dictionary<ItemId, RepositoryItem> itemsById = new Dictionary<ItemId, RepositoryItem>();

        public HierarchicalRepository()
        {
            Root = new RepositoryItem(ItemId.Null,
                                      ItemId.Null,
                                      "root",
                                      string.Empty,
                                      new Dictionary<string, string>());
        }

        public RepositoryItem Root { get; private set; }

        /// <summary>
        /// Throws ArgumentException if itemData.ParentId is an unknown ID.
        /// </summary>
        /// <param name="item"></param>
        public void ItemLoaded(RepositoryItem item)
        {
            var parent = GetItemById(item.ParentId);
            if (parent == null)
            {
                throw new ArgumentException("itemData.ParentId is not a known item ID");
            }
            var oldItem = parent.AddOrReplaceChild(item);
            item.CopyChildren(oldItem);
            itemsById[item.Id] = item;
            if (OnItemCreatedOrModified != null)
            {
                OnItemCreatedOrModified(parent);
                OnItemCreatedOrModified(item);
            }
        }

        public RepositoryItem GetItemById(ItemId id)
        {
            if (id.IsNull)
            {
                return Root;
            }
            RepositoryItem item;
            itemsById.TryGetValue(id, out item);
            return item;
        }

        public event Action<RepositoryItem> OnItemCreatedOrModified;
        public event Action<RepositoryItem> OnItemDeleted;

        public void ItemDeleted(ItemId id)
        {
            var item = GetItemById(id);
            if (item != null)
            {
                RemoveChildFromParent(item);
                DeleteItem(item);
            }
        }

        private void RemoveChildFromParent(RepositoryItem item)
        {
            var parent = GetItemById(item.ParentId);
            if (parent != null)
            {
                parent.RemoveChild(item.Id);
                if (OnItemCreatedOrModified != null)
                {
                    OnItemCreatedOrModified(parent);
                }
            }
        }

        private void DeleteItem(RepositoryItem item)
        {
            itemsById.Remove(item.Id);
            if (OnItemDeleted != null)
            {
                OnItemDeleted(item);
            }
            foreach (var child in item.Children)
            {
                DeleteItem(child);
            }
        }

        public IEnumerable<RepositoryItem> AllItems()
        {
            return AllChildren(Root);
        }

        private static IEnumerable<RepositoryItem> AllChildren(RepositoryItem item)
        {
            foreach (var child in item.Children)
            {
                yield return child;
                foreach (var descendant in AllChildren(child))
                {
                    yield return descendant;
                }
            }
        }

        public IEnumerable<RepositoryItem> FindAllMatching(Predicate<RepositoryItem> predicate)
        {
            return FindAllMatching(predicate, Root);
        }

        private static IEnumerable<RepositoryItem> FindAllMatching(Predicate<RepositoryItem> predicate, RepositoryItem item)
        {
            if (predicate(item))
            {
                yield return item;
            }
            foreach (var child in item.Children)
            {
                foreach (var match in FindAllMatching(predicate, child))
                {
                    yield return match;
                }
            }
        }
    }
}