using System;
using System.Collections.Generic;
using VIENNAAddIn.upcc3;
using VIENNAAddIn.upcc3.otf;

namespace VIENNAAddInUnitTests.upcc3.ccts.otf
{
    public class RepositoryItemBuilder
    {
        private static int NextId;

        private ItemId id = ItemId.Null;
        private string name;
        private string stereotype = string.Empty;
        private Dictionary<TaggedValues, string> taggedValues = new Dictionary<TaggedValues, string>();

        private RepositoryItemBuilder parent;
        private List<RepositoryItemBuilder> children = new List<RepositoryItemBuilder>();

        public RepositoryItemBuilder()
        {
        }

        private RepositoryItemBuilder(RepositoryItemBuilder other)
        {
            id = other.id;
            name = other.name;
            stereotype = other.stereotype;
            taggedValues = new Dictionary<TaggedValues, string>(other.taggedValues);
            parent = other.parent != null ? new RepositoryItemBuilder(other.parent) : null;
            children = new List<RepositoryItemBuilder>();
            if (other.children != null)
            {
                foreach (var otherChild in other.children)
                {
                    children.Add(new RepositoryItemBuilder(otherChild));
                }
            }
        }

        public RepositoryItem Build()
        {
            RepositoryItem parentItem;
            if (parent == null)
            {
                parentItem = new RepositoryItem(ItemId.Null, ItemId.Null, null, null, null);
            }
            else
            {
                parentItem = parent.Build();
            }
            var repositoryItem = new RepositoryItem(
                id,
                parentItem.Id,
                name ?? (string.IsNullOrEmpty(stereotype) ? id.Type.ToString() : stereotype) + "_" + id.Value,
                stereotype,
                ConvertTaggedValues());
            parentItem.AddOrReplaceChild(repositoryItem);
            foreach (var child in children)
            {
                var childItem = child.Build();
                repositoryItem.AddOrReplaceChild(childItem);
            }
            return repositoryItem;
        }

        private Dictionary<string, string> ConvertTaggedValues()
        {
            var tv = new Dictionary<string, string>();
            foreach (var keyValuePair in taggedValues)
            {
                tv[keyValuePair.Key.ToString()] = keyValuePair.Value;
            }
            return tv;
        }

        public RepositoryItemBuilder WithItemType(ItemId.ItemType itemType)
        {
            return new RepositoryItemBuilder(this)
                   {
                       id = new ItemId(itemType, ++NextId)
                   };
        }

        public RepositoryItemBuilder WithName(string value)
        {
            return new RepositoryItemBuilder(this)
                   {
                       name = value
                   };
        }

        public RepositoryItemBuilder WithStereotype(string value)
        {
            return new RepositoryItemBuilder(this)
                   {
                       stereotype = value
                   };
        }

        public RepositoryItemBuilder WithTaggedValues(Dictionary<TaggedValues, string> value)
        {
            return new RepositoryItemBuilder(this)
                   {
                       taggedValues = value
                   };
        }

        public RepositoryItemBuilder WithTaggedValue(TaggedValues key, string value)
        {
            var copy = new RepositoryItemBuilder(this);
            copy.taggedValues[key] = value;
            return copy;
        }

        public RepositoryItemBuilder WithoutTaggedValue(TaggedValues key)
        {
            var copy = new RepositoryItemBuilder(this);
            copy.taggedValues.Remove(key);
            return copy;
        }

        public RepositoryItemBuilder WithId(ItemId value)
        {
            return new RepositoryItemBuilder(this)
                   {
                       id = value
                   };
        }

        public RepositoryItemBuilder WithChild(RepositoryItemBuilder value)
        {
            var copy = new RepositoryItemBuilder(this);
            copy.children.Add(value);
            return copy;
        }

        public RepositoryItemBuilder WithParent(RepositoryItemBuilder value)
        {
            return new RepositoryItemBuilder(this)
            {
                parent = value
            };
        }
    }
}