using System;
using System.Collections;
using Castle.Core.Interceptor;
using Castle.DynamicProxy;
using EA;
using NUnit.Framework;
using VIENNAAddInUtils;
using Attribute=EA.Attribute;

namespace VIENNAAddInUnitTests.Utils
{
    /// <summary>
    /// <para>Assertion methods for EA repository classes.</para>
    /// 
    /// <para><b>Attention:</b> This implementation is not complete and must be extended as the need arises. A complete implementation is out of scope for the moment.</para>
    /// </summary>
    public static class EAAssert
    {
        #region Delegates

        public delegate void AssertAreEqual<T>(T expected, T actual, Path path);

        #endregion

        private static readonly PropertyAssertion<Attribute> attributePropertiesAreEqual = new PropertyAssertion<Attribute>(o => new[]
                                                                                                                                 {
                                                                                                                                     o.Name,
                                                                                                                                     o.Default,
                                                                                                                                     o.LowerBound,
                                                                                                                                     o.UpperBound,
                                                                                                                                 }, Assert.AreEqual, "should be equal");

        private static readonly PropertyAssertion<AttributeTag> attributeTagPropertiesAreEqual = new PropertyAssertion<AttributeTag>(o => new[]
                                                                                                                                          {
                                                                                                                                              o.Name,
                                                                                                                                              o.Value,
                                                                                                                                          }, Assert.AreEqual, "should be equal");

        private static readonly PropertyAssertion<ConnectorEnd> connectorEndPropertiesAreEqual = new PropertyAssertion<ConnectorEnd>(o => new[]
                                                                                                                                          {
                                                                                                                                              o.Role,
                                                                                                                                          }, Assert.AreEqual, "should be equal");

        private static readonly PropertyAssertion<Connector> connectorPropertiesAreEqual = new PropertyAssertion<Connector>(o => new[]
                                                                                                                                 {
                                                                                                                                     o.Name,
                                                                                                                                 }, Assert.AreEqual, "should be equal");

        private static readonly PropertyAssertion<Diagram> diagramPropertiesAreEqual = new PropertyAssertion<Diagram>(o => new[]
                                                                                                                           {
                                                                                                                               o.Name,
                                                                                                                               o.Type,
                                                                                                                           }, Assert.AreEqual, "should be equal");

        private static readonly PropertyAssertion<Element> elementPropertiesAreEqual = new PropertyAssertion<Element>(o => new[]
                                                                                                                           {
                                                                                                                               o.Name,
                                                                                                                               o.Stereotype,
                                                                                                                               o.StereotypeEx,
                                                                                                                           }, Assert.AreEqual, "should be equal");

        private static readonly PropertyAssertion<Package> packagePropertiesAreEqual = new PropertyAssertion<Package>(o => new[]
                                                                                                                           {
                                                                                                                               o.Name,
                                                                                                                           }, Assert.AreEqual, "should be equal");

        private static readonly PropertyAssertion<TaggedValue> taggedValuePropertiesAreEqual = new PropertyAssertion<TaggedValue>(o => new[]
                                                                                                                                       {
                                                                                                                                           o.Name,
                                                                                                                                           o.Value,
                                                                                                                                       }, Assert.AreEqual, "should be equal");

        public static void AssertCollectionsAreEqual<T>(Collection expectedElements, Collection actualElements,
                                                        Path path, AssertAreEqual<T> assertAreEqual)
        {
            Assert.AreEqual(expectedElements.Count, actualElements.Count,
                            "Different number of " + typeof (T).Name + "s at /" + path);
            IEnumerator actualElementsEnumerator = actualElements.GetEnumerator();
            foreach (T expectedElement in expectedElements)
            {
                actualElementsEnumerator.MoveNext();
                var actualElement = (T) actualElementsEnumerator.Current;
                assertAreEqual(expectedElement, actualElement, path);
            }
        }

        public static void RepositoriesAreEqual(Repository expected, Repository actual)
        {
            AssertCollectionsAreEqual<Package>(expected.Models, actual.Models, Path.EmptyPath, PackagesAreEqual);
        }

        public static void PackagesAreEqual(Package expectedPackage, Package actualPackage, Path path)
        {
            Path packagePath = path/expectedPackage.Name;
            packagePropertiesAreEqual.AssertFor(expectedPackage, actualPackage, packagePath);
            ElementsAreEqual(expectedPackage.Element, actualPackage.Element, packagePath);
            AssertCollectionsAreEqual<Package>(expectedPackage.Packages, actualPackage.Packages, packagePath,
                                               PackagesAreEqual);
            AssertCollectionsAreEqual<Element>(expectedPackage.Elements, actualPackage.Elements, packagePath,
                                               ElementsAreEqual);
            AssertCollectionsAreEqual<Diagram>(expectedPackage.Diagrams, actualPackage.Diagrams, packagePath,
                                               DiagramsAreEqual);
        }

        public static void DiagramsAreEqual(Diagram expectedDiagram, Diagram actualDiagram, Path path)
        {
            if (expectedDiagram == null)
            {
                Assert.IsNull(actualDiagram);
            }
            else
            {
                Assert.IsNotNull(actualDiagram, "Target diagram for " + expectedDiagram.Name + " is null at /" + path);
                Path diagramPath = path/expectedDiagram.Name;
                diagramPropertiesAreEqual.AssertFor(expectedDiagram, actualDiagram, diagramPath);
            }
        }

        public static void ElementsAreEqual(Element expectedElement, Element actualElement, Path path)
        {
            if (expectedElement == null)
            {
                Assert.IsNull(actualElement);
            }
            else
            {
                Assert.IsNotNull(actualElement, "Target element for " + expectedElement.Name + " is null at /" + path);
                Path elementPath = path/expectedElement.Name;
                elementPropertiesAreEqual.AssertFor(expectedElement, actualElement, elementPath);
                AssertCollectionsAreEqual<Attribute>(expectedElement.Attributes, actualElement.Attributes, elementPath,
                                                     AttributesAreEqual);
                AssertCollectionsAreEqual<Connector>(expectedElement.Connectors, actualElement.Connectors, elementPath,
                                                     ConnectorsAreEqual);
                AssertCollectionsAreEqual<TaggedValue>(expectedElement.TaggedValues, actualElement.TaggedValues,
                                                       elementPath, TaggedValuesAreEqual);
            }
        }

        public static void TaggedValuesAreEqual(TaggedValue expectedTag, TaggedValue actualTag, Path path)
        {
            if (expectedTag == null)
            {
                Assert.IsNull(actualTag);
            }
            else
            {
                Assert.IsNotNull(actualTag, "Target tagged value for " + expectedTag.Name + " is null at /" + path);
                Path tagPath = path/expectedTag.Name;
                taggedValuePropertiesAreEqual.AssertFor(expectedTag, actualTag, tagPath);
            }
        }

        public static void AttributeTagsAreEqual(AttributeTag expectedTag, AttributeTag actualTag, Path path)
        {
            if (expectedTag == null)
            {
                Assert.IsNull(actualTag);
            }
            else
            {
                Assert.IsNotNull(actualTag, "Target attribute tag for " + expectedTag.Name + " is null at /" + path);
                Path tagPath = path/expectedTag.Name;
                attributeTagPropertiesAreEqual.AssertFor(expectedTag, actualTag, tagPath);
            }
        }

        public static void ConnectorsAreEqual(Connector expected, Connector actual, Path path)
        {
            if (expected == null)
            {
                Assert.IsNull(actual);
            }
            else
            {
                Assert.IsNotNull(actual, "Target connector for " + expected.Name + " is null at /" + path);
                Path connectorPath = path/expected.Name;
                connectorPropertiesAreEqual.AssertFor(expected, actual, connectorPath);
                AssertConnectorEndsAreEqual(expected.ClientEnd, actual.ClientEnd, connectorPath);
                AssertConnectorEndsAreEqual(expected.SupplierEnd, actual.SupplierEnd, connectorPath);
            }
        }

        private static void AssertConnectorEndsAreEqual(ConnectorEnd expected, ConnectorEnd actual, Path path)
        {
            if (expected == null)
            {
                Assert.IsNull(actual);
            }
            else
            {
                Assert.IsNotNull(actual, "Target connector end for " + expected.Role + " is null at /" + path);
                Path connectorEndPath = path/expected.Role;
                connectorEndPropertiesAreEqual.AssertFor(expected, actual, connectorEndPath);
            }
        }

        public static void AttributesAreEqual(Attribute expectedAttribute, Attribute actualAttribute, Path path)
        {
            if (expectedAttribute == null)
            {
                Assert.IsNull(actualAttribute);
            }
            else
            {
                Assert.IsNotNull(actualAttribute,
                                 "Target attribute for " + expectedAttribute.Name + " is null at /" + path);
                Path attributePath = path/expectedAttribute.Name;
                attributePropertiesAreEqual.AssertFor(expectedAttribute, actualAttribute, attributePath);
                AssertCollectionsAreEqual<AttributeTag>(expectedAttribute.TaggedValues, actualAttribute.TaggedValues,
                                                        attributePath, AttributeTagsAreEqual);
            }
        }
    }

    internal class PropertyAssertion<T>
    {
        private readonly PropertyAssertionInterceptor interceptor;
        private readonly Func<T, object> invokeProperties;
        private readonly T proxy;

        public PropertyAssertion(Func<T, object> invokeProperties, Assertion assert, string assertionDescription)
        {
            this.invokeProperties = invokeProperties;
            interceptor = new PropertyAssertionInterceptor(assert, assertionDescription);
            proxy = new ProxyGenerator().CreateInterfaceProxyWithoutTarget<T>(interceptor);
        }

        public void AssertFor(T expected, T actual, Path path)
        {
            interceptor.Expected = expected;
            interceptor.Actual = actual;
            interceptor.Path = path;
            invokeProperties(proxy);
        }

        #region Nested type: Assertion

        internal delegate void Assertion(object expected, object actual, string message);

        #endregion

        #region Nested type: PropertyAssertionInterceptor

        private class PropertyAssertionInterceptor : IInterceptor
        {
            private readonly Assertion assert;
            private readonly string assertionDescription;

            public PropertyAssertionInterceptor(Assertion assert, string assertionDescription)
            {
                this.assert = assert;
                this.assertionDescription = assertionDescription;
            }

            public T Actual { private get; set; }
            public T Expected { private get; set; }
            public Path Path { private get; set; }

            #region IInterceptor Members

            public void Intercept(IInvocation invocation)
            {
                object expectedValue = invocation.Method.Invoke(Expected, invocation.Arguments);
                object actualValue = invocation.Method.Invoke(Actual, invocation.Arguments);
                invocation.ReturnValue = expectedValue;
                assert(expectedValue, actualValue,
                       "The property " + typeof (T).Name + "." +
                       invocation.Method.Name.Substring(4) +
                       " of " + Path + " " + assertionDescription + ".");
            }

            #endregion
        }

        #endregion
    }
}