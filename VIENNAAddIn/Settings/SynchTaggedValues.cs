/*******************************************************************************
This file is part of the VIENNAAddIn project

Licensed under GNU General Public License V3 http://gplv3.fsf.org/

For further information on the VIENNAAddIn project please visit 
http://vienna-add-in.googlecode.com
*******************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using EA;
using VIENNAAddIn.upcc3;
using VIENNAAddInUtils;
using Attribute=EA.Attribute;

namespace VIENNAAddIn.Settings
{
    /// <summary>
    /// This class should be used to check for missing <see cref="TaggedValues"/> of Elements/Connectors/Packages according to UPCC 3.0 spefication.
    /// Fix methods to add missing <see cref="TaggedValues"/> are also available.
    /// </summary>
    public partial class SynchTaggedValues
    {
        private readonly Repository repository;
        private bool removeUndefinedTaggedValues;

        public event Action<Path> TaggedValueFixed = path => { };

        public SynchTaggedValues(Repository repository)
        {
            this.repository = repository;
        }
        public SynchTaggedValues(Repository repository, bool removeUndefinedTaggedValues)
        {
            this.removeUndefinedTaggedValues = removeUndefinedTaggedValues;
            this.repository = repository;
        }

        private void AddMissingTaggedValues(Path path, Package package, params string[] requiredTaggedValues)
        {
            if (package.Element != null)
            {
                AddMissingTaggedValues(path, package.Element, requiredTaggedValues);
            }
        }

        private void AddMissingTaggedValues(Path path, Element element, params string[] requiredTaggedValues)
        {
            IEnumerable<string> existingTaggedValues = AsEnumerable<TaggedValue>(element.TaggedValues).Convert(tv => tv.Name);
            foreach (string missingTaggedValue in requiredTaggedValues.Except(existingTaggedValues))
            {
                var taggedValue = (TaggedValue) element.TaggedValues.AddNew(missingTaggedValue, string.Empty);
                taggedValue.Value = string.Empty;
                taggedValue.Update();
                TaggedValueFixed(path/missingTaggedValue);
            }
            if(removeUndefinedTaggedValues)
            {
                foreach (string unneededTaggedValue in existingTaggedValues.Except(requiredTaggedValues))
                {

                    element.TaggedValues.Delete(indexOf(element.TaggedValues, unneededTaggedValue));
                }
            }
            element.TaggedValues.Refresh();
        }

        private void AddMissingTaggedValues(Path path, Attribute attribute, params string[] requiredTaggedValues)
        {
            IEnumerable<string> existingTaggedValues = AsEnumerable<AttributeTag>(attribute.TaggedValues).Convert(tv => tv.Name);
            foreach (string missingTaggedValue in requiredTaggedValues.Except(existingTaggedValues))
            {
                var taggedValue = (AttributeTag) attribute.TaggedValues.AddNew(missingTaggedValue, string.Empty);
                taggedValue.Value = string.Empty;
                taggedValue.Update();
                TaggedValueFixed(path/missingTaggedValue);
            }
            if (removeUndefinedTaggedValues)
            {
                foreach (string unneededTaggedValue in existingTaggedValues.Except(requiredTaggedValues))
                {

                    attribute.TaggedValues.Delete(indexOf(attribute.TaggedValues, unneededTaggedValue));
                }
            }
            attribute.TaggedValues.Refresh();
        }

        private void AddMissingTaggedValues(Path path, Connector connector, params string[] requiredTaggedValues)
        {
            IEnumerable<string> existingTaggedValues = AsEnumerable<ConnectorTag>(connector.TaggedValues).Convert(tv => tv.Name);
            foreach (string missingTaggedValue in requiredTaggedValues.Except(existingTaggedValues))
            {
                var taggedValue = (ConnectorTag) connector.TaggedValues.AddNew(missingTaggedValue, string.Empty);
                taggedValue.Value = string.Empty;
                taggedValue.Update();
                TaggedValueFixed(path/missingTaggedValue);
            }
            if (removeUndefinedTaggedValues)
            {
                foreach (string unneededTaggedValue in existingTaggedValues.Except(requiredTaggedValues))
                {

                    connector.TaggedValues.Delete(indexOf(connector.TaggedValues, unneededTaggedValue));
                }
            }
            connector.TaggedValues.Refresh();
        }

        private static IEnumerable<T> AsEnumerable<T>(Collection collection)
        {
            foreach (T item in collection)
            {
                yield return item;
            }
        }

        private static short indexOf(Collection listToSearch, string unneededTaggedValue)
        {
            short position = 0;
            foreach (TaggedValue taggedValue in listToSearch)
            {
                if (taggedValue.Name.Equals(unneededTaggedValue))
                {
                    return position;
                }
                position++;
            }
            return -1;
        }

        public void FixTaggedValues()
        {
            foreach (Package model in repository.Models)
            {
                var path = (Path) model.Name;
                foreach (Package package in model.Packages)
                {
                    FixPackage(path/package.Name, package);
                }
            }
        }
    }
}