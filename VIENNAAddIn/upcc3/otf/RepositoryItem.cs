using System.Collections.Generic;
using EA;
using VIENNAAddIn.upcc3.uml;

namespace VIENNAAddIn.upcc3.otf
{
    public class RepositoryItem
    {
        private readonly Dictionary<string, string> taggedValues;
        private Dictionary<ItemId, RepositoryItem> children = new Dictionary<ItemId, RepositoryItem>();

        public RepositoryItem(ItemId id, ItemId parentId, string name, string stereotype, Dictionary<string, string> taggedValues)
        {
            this.taggedValues = taggedValues ?? new Dictionary<string, string>();
            Id = id ?? ItemId.Null;
            ParentId = parentId;
            Name = name;
            Stereotype = stereotype;
        }

        public RepositoryItem Parent { get; set; }

        public IEnumerable<RepositoryItem> Children
        {
            get { return new List<RepositoryItem>(children.Values); }
        }

        public ItemId Id { get; private set; }
        public ItemId ParentId { get; private set; }
        public string Name { get; private set; }
        public string Stereotype { get; private set; }

        public string GetTaggedValue(TaggedValues key)
        {
            string value;
            taggedValues.TryGetValue(key.ToString(), out value);
            return value;
        }

        public IEnumerable<string> GetTaggedValues(TaggedValues key)
        {
            return MultiPartTaggedValue.Split(GetTaggedValue(key));
        }

        public RepositoryItem AddOrReplaceChild(RepositoryItem item)
        {
            RepositoryItem oldItem;
            children.TryGetValue(item.Id, out oldItem);
            children[item.Id] = item;
            item.Parent = this;
            return oldItem;
        }

        public void RemoveChild(ItemId id)
        {
            children.Remove(id);
        }

        public void CopyChildren(RepositoryItem item)
        {
            if (item != null)
            {
                children = new Dictionary<ItemId, RepositoryItem>(item.children);
            }
        }

        public static RepositoryItem FromPackage(Package package)
        {
            ItemId id = ItemId.ForPackage(package.PackageID);
            ItemId parentId = ItemId.ForPackage(package.ParentID);
            string name = package.Name;
            string stereotype;
            var taggedValues = new Dictionary<string, string>();
            if (package.Element != null)
            {
                stereotype = package.Element.Stereotype;
                foreach (TaggedValue taggedValue in package.Element.TaggedValues)
                {
                    taggedValues[taggedValue.Name] = taggedValue.Value;
                }
            }
            else
            {
                stereotype = string.Empty;
            }
            return new RepositoryItem(id, parentId, name, stereotype, taggedValues);
        }

        public static RepositoryItem FromElement(Element element)
        {
            return new RepositoryItem(
                ItemId.ForElement(element.ElementID),
                ItemId.ForPackage(element.PackageID),
                element.Name,
                element.Stereotype,
                new Dictionary<string, string>()
                );
        }
    }
}