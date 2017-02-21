using System.Collections.Generic;
using EA;
using NUnit.Framework;
using Constraint=NUnit.Framework.Constraints.Constraint;

namespace VIENNAAddInUnitTests.SynchTaggedValuesTest
{
    internal class HasTaggedValuesConstraint : Constraint
    {
        private readonly List<string> expectedTaggedValues;
        private List<string> missingTaggedValues;

        public HasTaggedValuesConstraint(IEnumerable<string> expectedTaggedValues)
        {
            this.expectedTaggedValues = new List<string>(expectedTaggedValues);
        }

        public override bool Matches(object actual)
        {
            List<string> actualTaggedValues;
            if (actual is Package)
            {
                var package = (Package) actual;
                actualTaggedValues = new List<string>(GetTaggedValues(package.Element));
            }
            else if (actual is Element)
            {
                var element = (Element) actual;
                actualTaggedValues = new List<string>(GetTaggedValues(element));
            }
            else if (actual is Attribute)
            {
                var attribute = (Attribute) actual;
                actualTaggedValues = new List<string>(GetTaggedValues(attribute));
            }
            else if (actual is Connector)
            {
                var connector = (Connector) actual;
                actualTaggedValues = new List<string>(GetTaggedValues(connector));
            }
            else
            {
                actualTaggedValues = new List<string>();
            }

            this.actual = actualTaggedValues;

            missingTaggedValues = new List<string>();
            foreach (var expectedTaggedValue in expectedTaggedValues)
            {
                if (!actualTaggedValues.Contains(expectedTaggedValue))
                {
                    missingTaggedValues.Add(expectedTaggedValue);
                }
            }
            return missingTaggedValues.Count == 0;
        }

        private IEnumerable<string> GetTaggedValues(Element element)
        {
            foreach (TaggedValue actualTaggedValue in element.TaggedValues)
            {
                yield return actualTaggedValue.Name;
            }
        }

        private IEnumerable<string> GetTaggedValues(Attribute attribute)
        {
            foreach (AttributeTag actualTaggedValue in attribute.TaggedValues)
            {
                yield return actualTaggedValue.Name;
            }
        }

        private IEnumerable<string> GetTaggedValues(Connector connector)
        {
            foreach (ConnectorTag actualTaggedValue in connector.TaggedValues)
            {
                yield return actualTaggedValue.Name;
            }
        }

        public override void WriteDescriptionTo(MessageWriter writer)
        {
            writer.WriteExpectedValue(expectedTaggedValues);
        }
    }
}