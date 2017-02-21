// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************

using System;
using NUnit.Framework;
using VIENNAAddIn.upcc3.Wizards.dev.util;
namespace VIENNAAddInUnitTests.upcc3.Wizards.dev.util
{
    [TestFixture]
    public class XmlSchemaReaderTest
    {
        [Test]
        public void ShouldAnalyzeSimpleSchema()
        {
            // Setup
            var results = XMLSchemaReader.Read(TestUtils.PathToTestResource(@"simpleSchema.xsd"));

            // Events

            // Assertion and Verification
            foreach (var result in results)
            {
                if (result.Caption.Equals("Attribute"))
                {
                    Assert.That(result.Count.Equals(1));
                }
                if (result.Caption.Equals("Element"))
                {
                    Assert.That(result.Count.Equals(12));
                }

            }
        }

        [Test]
        public void ShouldAnalyzeSimpleSchemaWithComplexTypes()
        {
            // Setup
            var results = XMLSchemaReader.Read(TestUtils.PathToTestResource(@"simpleSchema_withComplexTypes.xsd"));

            // Events

            // Assertion and Verification
            foreach (var result in results)
            {
                Console.WriteLine(result.Caption+": "+result.Count);
            }
        }
    }
}