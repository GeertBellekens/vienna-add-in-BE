// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using NUnit.Framework;
using VIENNAAddIn.upcc3.import.util;

namespace VIENNAAddInUnitTests.upcc3.import.util
{
    [TestFixture]
    public class CustomSchemaReaderTest
    {
        #region Test Preparation 

        private static XmlDocument PrepareTest()
        {
            string testSchemaFile = Directory.GetCurrentDirectory() + "\\..\\..\\testresources\\XSDImporterTest\\util\\BusinessInformationEntity_1.xsd";

            XmlDocument document = new XmlDocument();
            document.Load(testSchemaFile);

            return document;
        }

        #endregion 

        [Test]
        public void UselessTest()
        {
            Console.WriteLine("Test: Parsing an XML Schema\n");

            XmlDocument testDocument = PrepareTest();

            CustomSchemaReader reader = new CustomSchemaReader(testDocument);

            // print namespace declarations
            PrintNamespaceTable(reader);

            // print includes
            PrintIncludes(reader);
            
            // print items 
            PrintItems(reader);
        }

        #region Convenience Methods

        private static void PrintIncludes(CustomSchemaReader reader)
        {
            foreach (Include include in reader.Includes)
            {
                Console.WriteLine("Include: {0}", include.SchemaLocation);
            }

            Console.WriteLine("");
        }

        private static void PrintItems(CustomSchemaReader reader)
        {
            foreach (object item in reader.Items)
            {
                if (item is ComplexType)
                {
                    ComplexType ct = (ComplexType)item;

                    Console.WriteLine("Complex Type\n"
                                      + "   Name: " + ct.Name);

                    foreach (object i in ct.Items)
                    {
                        Element element = (Element)i;                        
                        Console.WriteLine("   Item: " + element.Name);
                    }

                    Console.WriteLine("");
                }

                if (item is Element)
                {
                    Element element = (Element)item;

                    Console.WriteLine("Element\n"
                                      + "   Name: " + element.Name + "\n"
                                      + "   Ref " + "\n"
                                      + "      Prefix:" + element.Ref.Prefix + "\n"
                                      + "      Name: " + element.Ref.Name + "\n"
                                      + "   Type " + "\n"
                                      + "      Prefix:" + element.Type.Prefix + "\n"
                                      + "      Name: " + element.Type.Name + "\n"
                                      + "   MinOccurs: " + element.MinOccurs + "\n"
                                      + "   MaxOccurs: " + element.MaxOccurs
                                      + "\n");
                }
            }

            Console.WriteLine("");
        }

        private static void PrintNamespaceTable(CustomSchemaReader reader)
        {
            foreach (KeyValuePair<string, string> pair in reader.NamespaceTable)
            {
                Console.WriteLine("Key: {0}, Value: {1}", pair.Key, pair.Value);
            }

            Console.WriteLine("");
        }

        #endregion
    }
}