using System;
using EA;
using Attribute=EA.Attribute;

namespace VIENNAAddInUnitTests.TestRepository
{
    /// <summary>
    /// Extension methods for EA.Attribute.
    /// </summary>
    public static class EAAttributeExtensions
    {
        /// <summary>
        /// Execute the given actions for the attribute (e.g. setting its tagged value, ...) and save the attribute to the EA.Repository.
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="doSomethingWith"></param>
        /// <returns></returns>
        public static Attribute With(this Attribute attribute, params Action<Attribute>[] doSomethingWith)
        {
            foreach (var action in doSomethingWith)
            {
                action(attribute);
            }
            attribute.Update();
            return attribute;
        }

        public static AttributeTag AddTaggedValue(this Attribute attribute, string name)
        {
            var taggedValue = (AttributeTag) attribute.TaggedValues.AddNew(name, string.Empty);
            taggedValue.Value = string.Empty;
            taggedValue.Update();
            return taggedValue;
        }
    }
}