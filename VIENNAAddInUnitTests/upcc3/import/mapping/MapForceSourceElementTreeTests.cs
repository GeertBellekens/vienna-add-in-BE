using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using VIENNAAddIn.upcc3.import.mapping;

namespace VIENNAAddInUnitTests.upcc3.import.mapping
{
    [TestFixture]
    public class MapForceSourceElementTreeTests
    {
        [Test]
        public void TestCreateSourceElementTree()
        {
            var xsdFileName = TestUtils.PathToTestResource(@"XSDImporterTest\mapping\mapForceSourceElementTreeTests\CreateSourceElementTree\source.xsd");
            XmlSchemaSet xmlSchemaSet = new XmlSchemaSet();
            xmlSchemaSet.Add(XmlSchema.Read(XmlReader.Create(xsdFileName), null));

            var mapForceMapping = LinqToXmlMapForceMappingImporter.ImportFromFiles(TestUtils.PathToTestResource(@"XSDImporterTest\mapping\mapForceSourceElementTreeTests\CreateSourceElementTree\mapping.mfd"));

            var expectedAddress = new SourceItem("Address", null, XsdObjectType.Element, null);
            var expectedTown = new SourceItem("Town", null, XsdObjectType.Element, null);
            expectedAddress.AddChild(expectedTown);

            AssertTreesAreEqual(expectedAddress, new MapForceSourceItemTree(mapForceMapping, xmlSchemaSet).RootSourceItem, string.Empty);
        }

        [Test]
        public void ShouldWorkWithASingleInputSchemaComponent()
        {
            var mapForceMapping = new MapForceMapping(new List<SchemaComponent>
                                                      {
                                                          new SchemaComponent("Schema1.xsd", "{http://www.ebinterface.at/schema/3p0/}Entry1", new[] {new Namespace("http://www.ebinterface.at/schema/3p0/"),},
                                                                              new Entry("Entry1", InputOutputKey.None, XsdObjectType.Element,
                                                                                        new[]
                                                                                        {
                                                                                            new Entry("Entry2", InputOutputKey.Output(null, "2"), XsdObjectType.Element,
                                                                                                      new[]
                                                                                                      {
                                                                                                          new Entry("Entry3", InputOutputKey.Output(null, "3"), XsdObjectType.Element),
                                                                                                      }),
                                                                                        })),
                                                      },
                                                      new List<ConstantComponent>(),
                                                      new List<FunctionComponent>(),
                                                      new Graph(new Vertex[0]));
            var expectedRoot = new SourceItem("Entry1", null, XsdObjectType.Element, null);
            var expectedChild1 = new SourceItem("Entry2", null, XsdObjectType.Element, null);
            var expectedChild2 = new SourceItem("Entry3", null, XsdObjectType.Element, null);
            expectedRoot.AddChild(expectedChild1);
            expectedChild1.AddChild(expectedChild2);

            var xsdFileName = TestUtils.PathToTestResource(@"XSDImporterTest\mapping\mapForceSourceElementTreeTests\Schema1.xsd");
            XmlSchemaSet xmlSchemaSet = new XmlSchemaSet();
            xmlSchemaSet.Add(XmlSchema.Read(XmlReader.Create(xsdFileName), null));

            var sourceElementTree = new MapForceSourceItemTree(mapForceMapping, xmlSchemaSet);

            AssertTreesAreEqual(expectedRoot, sourceElementTree.RootSourceItem, string.Empty);
        }

        [Test]
        [ExpectedException(typeof(MappingError))]
        public void ShouldThrowExceptionIfSchemaComponentCouldNotBeAttached()
        {
            var mapForceMapping = new MapForceMapping(new List<SchemaComponent>
                                                      {
                                                          new SchemaComponent("Schema1.xsd", "{http://www.ebinterface.at/schema/3p0/}Entry1", new[] {new Namespace("http://www.ebinterface.at/schema/3p0/"),},
                                                                              new Entry("Entry1", InputOutputKey.None, XsdObjectType.Element,
                                                                                        new[]
                                                                                        {
                                                                                            new Entry("Entry2", InputOutputKey.Output(null, "2"), XsdObjectType.Element,
                                                                                                      new[]
                                                                                                      {
                                                                                                          new Entry("Entry3", InputOutputKey.Output(null, "3"), XsdObjectType.Element),
                                                                                                      }),
                                                                                        })),
                                                          new SchemaComponent("Schema2.xsd", "{http://www.ebinterface.at/schema/3p0/}Entry4", new[] {new Namespace("http://www.ebinterface.at/schema/3p0/"),},
                                                                              new Entry("Entry4", InputOutputKey.None, XsdObjectType.Element,
                                                                                        new[]
                                                                                        {
                                                                                            new Entry("Entry5", InputOutputKey.Output(null, "5"), XsdObjectType.Element),
                                                                                        })),
                                                      },
                                                      new List<ConstantComponent>
                                                      {
                                                          new ConstantComponent("Root:Entry1", InputOutputKey.Output(null, "6")),
                                                      },
                                                      new List<FunctionComponent>(),
                                                      new Graph(new Vertex[0]));
            var expectedRoot = new SourceItem("Entry1", null, XsdObjectType.Element, null);
            var expectedChild1 = new SourceItem("Entry2", null, XsdObjectType.Element, null);
            var expectedChild2 = new SourceItem("Entry3", null, XsdObjectType.Element, null);
            expectedRoot.AddChild(expectedChild1);
            expectedChild1.AddChild(expectedChild2);


            XmlSchemaSet xmlSchemaSet = new XmlSchemaSet();
            xmlSchemaSet.Add(XmlSchema.Read(XmlReader.Create(TestUtils.PathToTestResource(@"XSDImporterTest\mapping\mapForceSourceElementTreeTests\Schema1.xsd")), null));
            xmlSchemaSet.Add(XmlSchema.Read(XmlReader.Create(TestUtils.PathToTestResource(@"XSDImporterTest\mapping\mapForceSourceElementTreeTests\Schema2.xsd")), null));
            
            var sourceElementTree = new MapForceSourceItemTree(mapForceMapping, xmlSchemaSet);

            AssertTreesAreEqual(expectedRoot, sourceElementTree.RootSourceItem, string.Empty);
        }

        [Test]
        public void ShouldAttachConnectedSchemaComponentsToRootSchemaComponent()
        {
            var mapForceMapping = new MapForceMapping(new List<SchemaComponent>
                                                      {
                                                          new SchemaComponent("Schema3.xsd", "{http://www.ebinterface.at/schema/3p0/}Entry1", new[] {new Namespace("http://www.ebinterface.at/schema/3p0/"),},
                                                                              new Entry("Entry1", InputOutputKey.None, XsdObjectType.Element,
                                                                                        new[]
                                                                                        {
                                                                                            new Entry("Entry2", InputOutputKey.Output(null, "2"), XsdObjectType.Element,
                                                                                                      new[]
                                                                                                      {
                                                                                                          new Entry("Entry3", InputOutputKey.Output(null, "3"), XsdObjectType.Element),
                                                                                                      }),
                                                                                            new Entry("Entry4", InputOutputKey.None, XsdObjectType.Element),
                                                                                        })),
                                                          new SchemaComponent("Schema2.xsd", "{http://www.ebinterface.at/schema/3p0/}Entry4", new[] {new Namespace("http://www.ebinterface.at/schema/3p0/"),},
                                                                              new Entry("Entry4", InputOutputKey.None, XsdObjectType.Element,
                                                                                        new[]
                                                                                        {
                                                                                            new Entry("Entry5", InputOutputKey.Output(null, "5"), XsdObjectType.Element),
                                                                                        })),
                                                      },
                                                      new List<ConstantComponent>
                                                      {
                                                          new ConstantComponent("Root:Entry1", InputOutputKey.Output(null, "6")),
                                                      },
                                                      new List<FunctionComponent>(),
                                                      new Graph(new Vertex[0]));
            var entry1 = new SourceItem("Entry1", null, XsdObjectType.Element, null);
            var entry2 = new SourceItem("Entry2", null, XsdObjectType.Element, null);
            var entry3 = new SourceItem("Entry3", null, XsdObjectType.Element, null);
            var entry4 = new SourceItem("Entry4", null, XsdObjectType.Element, null);
            var entry5 = new SourceItem("Entry5", null, XsdObjectType.Element, null);
            entry1.AddChild(entry2);
            entry2.AddChild(entry3);
            entry1.AddChild(entry4);
            entry4.AddChild(entry5);

            XmlSchemaSet xmlSchemaSet = new XmlSchemaSet();
            xmlSchemaSet.Add(XmlSchema.Read(XmlReader.Create(TestUtils.PathToTestResource(@"XSDImporterTest\mapping\mapForceSourceElementTreeTests\Schema3.xsd")), null));

            var sourceElementTree = new MapForceSourceItemTree(mapForceMapping, xmlSchemaSet);

            AssertTreesAreEqual(entry1, sourceElementTree.RootSourceItem, string.Empty);
        }

        [Test]
        public void ShouldAttachXsdSequenceInformationToSourceElements()
        {
            var mapForceMapping = new MapForceMapping(new List<SchemaComponent>
                                                      {
                                                          new SchemaComponent("Schema3.xsd", "{http://www.ebinterface.at/schema/3p0/}Entry1", new[] {new Namespace("http://www.ebinterface.at/schema/3p0/"),},
                                                                              new Entry("Entry1", InputOutputKey.None, XsdObjectType.Element,
                                                                                        new[]
                                                                                        {
                                                                                            new Entry("Entry2", InputOutputKey.Output(null, "2"), XsdObjectType.Element,
                                                                                                      new[]
                                                                                                      {
                                                                                                          new Entry("Entry3", InputOutputKey.Output(null, "3"), XsdObjectType.Element),
                                                                                                      }),
                                                                                            new Entry("Entry4", InputOutputKey.None, XsdObjectType.Element),
                                                                                        })),
                                                          new SchemaComponent("Schema2.xsd", "{http://www.ebinterface.at/schema/3p0/}Entry4", new[] {new Namespace("http://www.ebinterface.at/schema/3p0/"),},
                                                                              new Entry("Entry4", InputOutputKey.None,  XsdObjectType.Element,
                                                                                        new[]
                                                                                        {
                                                                                            new Entry("Entry5", InputOutputKey.Output(null, "5"), XsdObjectType.Element),
                                                                                        })),
                                                      },
                                                      new List<ConstantComponent>
                                                      {
                                                          new ConstantComponent("Root:Entry1", InputOutputKey.Output(null, "6")),
                                                      },
                                                      new List<FunctionComponent>(),
                                                      new Graph(new Vertex[0]));

            XmlSchemaSet xmlSchemaSet = new XmlSchemaSet();
            xmlSchemaSet.Add(XmlSchema.Read(XmlReader.Create(TestUtils.PathToTestResource(@"XSDImporterTest\mapping\mapForceSourceElementTreeTests\Schema3.xsd")), null));

            var sourceElementTree = new MapForceSourceItemTree(mapForceMapping, xmlSchemaSet);

            Assert.That(sourceElementTree.RootSourceItem.XsdTypeName, Is.EqualTo("Entry1Type"));
            Assert.That(sourceElementTree.RootSourceItem.Children[0].XsdTypeName, Is.EqualTo("Entry2Type"));
            Assert.That(sourceElementTree.RootSourceItem.Children[0].Children[0].XsdTypeName, Is.EqualTo("String"));
            Assert.That(sourceElementTree.RootSourceItem.Children[1].XsdTypeName, Is.EqualTo("Entry4Type"));
            Assert.That(sourceElementTree.RootSourceItem.Children[1].Children[0].XsdTypeName, Is.EqualTo("String"));
        }

        [Test]
        public void ShouldAttachXsdChoiceInformationToSourceElements()
        {
            var mapForceMapping = new MapForceMapping(new List<SchemaComponent>
                                                      {
                                                          new SchemaComponent("Schema4.xsd", "{http://www.ebinterface.at/schema/3p0/}Entry1", new[] {new Namespace("http://www.ebinterface.at/schema/3p0/"),},
                                                                              new Entry("Entry1", InputOutputKey.None, XsdObjectType.Element,
                                                                                        new[]
                                                                                        {
                                                                                            new Entry("Entry2", InputOutputKey.Output(null, "2"),  XsdObjectType.Element,
                                                                                                      new[]
                                                                                                      {
                                                                                                          new Entry("Entry3", InputOutputKey.Output(null, "3"), XsdObjectType.Element),
                                                                                                      }),
                                                                                            new Entry("Entry4", InputOutputKey.None, XsdObjectType.Element),
                                                                                        })),
                                                          new SchemaComponent("Schema2.xsd", "{http://www.ebinterface.at/schema/3p0/}Entry4", new[] {new Namespace("http://www.ebinterface.at/schema/3p0/"),},
                                                                              new Entry("Entry4", InputOutputKey.None, XsdObjectType.Element,
                                                                                        new[]
                                                                                        {
                                                                                            new Entry("Entry5", InputOutputKey.Output(null, "5"), XsdObjectType.Element),
                                                                                        })),
                                                      },
                                                      new List<ConstantComponent>
                                                      {
                                                          new ConstantComponent("Root:Entry1", InputOutputKey.Output(null, "6")),
                                                      },
                                                      new List<FunctionComponent>(),
                                                      new Graph(new Vertex[0]));

            XmlSchemaSet xmlSchemaSet = new XmlSchemaSet();
            xmlSchemaSet.Add(XmlSchema.Read(XmlReader.Create(TestUtils.PathToTestResource(@"XSDImporterTest\mapping\mapForceSourceElementTreeTests\Schema4.xsd")), null));

            var sourceElementTree = new MapForceSourceItemTree(mapForceMapping, xmlSchemaSet);

            Assert.That(sourceElementTree.RootSourceItem.XsdTypeName, Is.EqualTo("Entry1Type"));
            Assert.That(sourceElementTree.RootSourceItem.Children[0].XsdTypeName, Is.EqualTo("Entry2Type"));
            Assert.That(sourceElementTree.RootSourceItem.Children[0].Children[0].XsdTypeName, Is.EqualTo("String"));
            Assert.That(sourceElementTree.RootSourceItem.Children[1].XsdTypeName, Is.EqualTo("Entry4Type"));
            Assert.That(sourceElementTree.RootSourceItem.Children[1].Children[0].XsdTypeName, Is.EqualTo("String"));
        }

        [Test]
        [ExpectedException(ExceptionType = typeof (MappingError))]
        public void ShouldThrowExceptionIfNoRootIsDefinedForMultipleInputSchemaComponents()
        {
            var mapForceMapping = new MapForceMapping(new List<SchemaComponent>
                                                      {
                                                          new SchemaComponent("Schema1.xsd", "{http://www.ebinterface.at/schema/3p0/}Entry1", new[] {new Namespace("http://www.ebinterface.at/schema/3p0/"),},
                                                                              new Entry("Entry1", InputOutputKey.None, XsdObjectType.Element,
                                                                                        new[]
                                                                                        {
                                                                                            new Entry("Entry2", InputOutputKey.Output(null, "2"), XsdObjectType.Element,
                                                                                                      new[]
                                                                                                      {
                                                                                                          new Entry("Entry3", InputOutputKey.Output(null, "3"), XsdObjectType.Element),
                                                                                                      }),
                                                                                        })),
                                                          new SchemaComponent("Schema2.xsd", "{http://www.ebinterface.at/schema/3p0/}Entry4", new[] {new Namespace("http://www.ebinterface.at/schema/3p0/"),},
                                                                              new Entry("Entry4", InputOutputKey.None, XsdObjectType.Element,
                                                                                        new[]
                                                                                        {
                                                                                            new Entry("Entry5", InputOutputKey.Output(null, "5"), XsdObjectType.Element),
                                                                                        })),
                                                      },
                                                      new List<ConstantComponent>(),
                                                      new List<FunctionComponent>(),
                                                      new Graph(new Vertex[0]));
            
            XmlSchemaSet xmlSchemaSet = new XmlSchemaSet();
            
            xmlSchemaSet.Add(XmlSchema.Read(XmlReader.Create(TestUtils.PathToTestResource(@"XSDImporterTest\mapping\mapForceSourceElementTreeTests\Schema1.xsd")), null));
            xmlSchemaSet.Add(XmlSchema.Read(XmlReader.Create(TestUtils.PathToTestResource(@"XSDImporterTest\mapping\mapForceSourceElementTreeTests\Schema2.xsd")), null));

            new MapForceSourceItemTree(mapForceMapping, xmlSchemaSet);
        }

        [Test]
        public void ShouldBuildCompleteSourceElementTree()
        {
            var mappingFileName = TestUtils.PathToTestResource(@"XSDImporterTest\mapping\mapForceSourceElementTreeTests\BuildCompleteSourceElementTree\mapping.mfd");
            var mapForceMapping = LinqToXmlMapForceMappingImporter.ImportFromFiles(mappingFileName);

            var xsdFileName = TestUtils.PathToTestResource(@"XSDImporterTest\mapping\mapForceSourceElementTreeTests\BuildCompleteSourceElementTree\source.xsd");
            XmlSchemaSet xmlSchemaSet = new XmlSchemaSet();
            xmlSchemaSet.Add(XmlSchema.Read(XmlReader.Create(xsdFileName), null));

            var person = new SourceItem("Person", null, XsdObjectType.Element, null);
            var personNameElement = new SourceItem("Name", null, XsdObjectType.Element, null);
            var personNameAttribute = new SourceItem("Name", null, XsdObjectType.Attribute, null);
            var personHomeAddress = new SourceItem("HomeAddress", null, XsdObjectType.Element, null);
            var addressTown = new SourceItem("Town", null, XsdObjectType.Element, null);
            person.AddChild(personNameElement);
            person.AddChild(personHomeAddress);
            person.AddChild(personNameAttribute);
            personHomeAddress.AddChild(addressTown);

            var sourceElementTree = new MapForceSourceItemTree(mapForceMapping, xmlSchemaSet);

            AssertTreesAreEqual(person, sourceElementTree.RootSourceItem, string.Empty);
        }

        [Test]
        public void ShouldBuildCompleteSourceItemTreeForUbl()
        {
            var mappingFileNames = new List<string> { "ubl2cll_1_1.mfd", "ubl2cll_2_1.mfd", "ubl2cll_3_1.mfd", "ubl2cll_4_1.mfd", "ubl2cll_5_1.mfd", "ubl2cll_6_1.mfd", "ubl2cll_7_1.mfd", "ubl2cll_8_1.mfd", "ubl2cll_9_1.mfd", "ubl2cll_10_1.mfd", "ubl2cll_11_1.mfd", "ubl2cll_12_1.mfd", "ubl2cll_13_1.mfd" };
            var mappingFiles = new List<string>();

            foreach (var mappingFile in mappingFileNames)
            {
                mappingFiles.Add(TestUtils.PathToTestResource(@"XSDImporterTest\mapping\MappingImporterTests\mapping_ubl_to_ccl\" + mappingFile));
            }

            var mapForceMapping = LinqToXmlMapForceMappingImporter.ImportFromFiles(mappingFiles.ToArray());

            XmlSchemaSet xmlSchemaSet = new XmlSchemaSet();
            xmlSchemaSet.Add(XmlSchema.Read(XmlReader.Create(TestUtils.PathToTestResource(@"XSDImporterTest\mapping\MappingImporterTests\mapping_ubl_to_ccl\invoice\maindoc\UBL-Invoice-2.0.xsd")), null));

            new MapForceSourceItemTree(mapForceMapping, xmlSchemaSet);
        }

        private static void AssertTreesAreEqual(SourceItem expected, SourceItem actual, string path)
        {
            Assert.AreEqual(expected.Name, actual.Name, "Name mismatch at " + path);
            Assert.AreEqual(expected.Children.Count, actual.Children.Count, "Unequal number of children at " + path + "/" + expected.Name);
            for (int i = 0; i < expected.Children.Count; i++)
            {
                AssertTreesAreEqual(expected.Children[i], actual.Children[i], path + "/" + expected.Name);
            }
        }
    }
}