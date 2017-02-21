// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System;
using EA;
using NUnit.Framework;
using Attribute=EA.Attribute;

namespace VIENNAAddInUnitTests.TestRepository
{
    [TestFixture]
    public class EARepositoryTest
    {
        [Test]
        public void TestEARepository()
        {
            Repository repository = new EARepository1();
            Assert.AreEqual(1, repository.Models.Count);

            var model = (Package) repository.Models.GetAt(0);
            Assert.AreEqual(3, model.Packages.Count);
            Assert.IsNull(model.Element);
            Assert.AreEqual(0, model.ParentID);

            var bLib1 = AssertLibrary(model, 1, "bLibrary", "blib1", "urn:test:blib1", 7, 0);

            var primLib1 = AssertLibrary(bLib1, 6, "PRIMLibrary", "primlib1", "urn:test:blib1:primlib1", 0, 3);
            var stringType = AssertPRIM(primLib1, 2, "String");
            var decimalType = AssertPRIM(primLib1, 0, "Decimal");

            AssertLibrary(bLib1, 5, "ENUMLibrary", "enumlib1", "urn:test:blib1:enumlib1", 0, 2);

            var cdtLib1 = AssertLibrary(bLib1, 3, "CDTLibrary", "cdtlib1", "urn:test:blib1:cdtlib1", 0, 5);
            AssertCDT(cdtLib1, 4, "Text", stringType, 2, 0);
            var cdtDate = AssertCDT(cdtLib1, 1, "Date", stringType, 1, 0);
            AssertSUP(cdtDate, 1, "Format", stringType, "1", "1");
            AssertCDT(cdtLib1, 0, "Code", stringType, 9, 7);
            var cdtMeasure = AssertCDT(cdtLib1, 3, "Measure", decimalType, 2, 0);
            AssertSUP(cdtMeasure, 1, "MeasureUnit", stringType, "1", "1");
            AssertSUP(cdtMeasure, 2, "MeasureUnit.CodeListVersion", stringType, "1", "*");

            var bdtLib1 = AssertLibrary(bLib1, 0, "BDTLibrary", "bdtlib1", "urn:test:blib1:bdtlib1", 0, 6);
            var bdtDate = AssertBDT(bdtLib1, 2, "Date", stringType, 1, cdtDate, 0);
            AssertSUP(bdtDate, 1, "Format", stringType, "1", "1");
            var bdtMeasure = AssertBDT(bdtLib1, 4, "Measure", decimalType, 2, cdtMeasure, 0);
            AssertSUP(bdtMeasure, 1, "MeasureUnit", stringType, "1", "1");
            AssertSUP(bdtMeasure, 2, "MeasureUnit.CodeListVersion", stringType, "1", "*");

            AssertLibrary(bLib1, 2, "CCLibrary", "cclib1", "urn:test:blib1:cclib1", 0, 3);
            AssertLibrary(bLib1, 1, "BIELibrary", "bielib1", "urn:test:blib1:bielib1", 0, 3);
        }

        private static Element AssertPRIM(Package library, short index, string name)
        {
            var prim = (Element) library.Elements.GetAt(index);
            Assert.AreEqual("PRIM", prim.Stereotype);
            Assert.AreEqual(name, prim.Name);
            return prim;
        }

        private static Element AssertCDT(Package library, short index, string name, Element contentType, int numberOfSUPs, short conIndex)
        {
            var cdt = (Element)library.Elements.GetAt(index);
            Assert.AreEqual("CDT", cdt.Stereotype);
            Assert.AreEqual(name, cdt.Name);
            Assert.AreEqual(numberOfSUPs, cdt.Attributes.Count - 1);
            AssertCON(cdt, contentType, conIndex);
            return cdt;
        }

        private static Element AssertBDT(Package library, short index, string name, Element contentType, int numberOfSUPs, Element basedOnType, short conIndex)
        {
            var bdt = (Element)library.Elements.GetAt(index);
            Assert.AreEqual("BDT", bdt.Stereotype);
            Assert.AreEqual(name, bdt.Name);
            Assert.AreEqual(numberOfSUPs + 1, bdt.Attributes.Count);
            AssertCON(bdt, contentType, conIndex);
            AssertBasedOn(basedOnType, bdt);
            return bdt;
        }

        private static void AssertBasedOn(Element basedOnType, Element element)
        {
            foreach (Connector connector in element.Connectors)
            {
                if (connector.Stereotype == "basedOn")
                {
                    Assert.AreEqual(basedOnType.ElementID, connector.SupplierID);
                    return;
                }
            }
            Assert.Fail("no <<basedOn>> dependency found");
        }

        private static void AssertSUP(Element dataType, short index, string name, Element type, string lowerBound, string upperBound)
        {
            AssertDataTypeComponent(dataType, index, "SUP", name, type, lowerBound, upperBound);
        }

        private static void AssertCON(Element dataType, Element type, short conIndex)
        {
            AssertDataTypeComponent(dataType, conIndex, "CON", "Content", type, "1", "1");
        }

        private static void AssertDataTypeComponent(Element dataType, short index, string stereotype, string name, Element type, string lowerBound, string upperBound)
        {
            var component = (Attribute) dataType.Attributes.GetAt(index);
            Assert.AreEqual(stereotype, component.Stereotype);
            Assert.AreEqual(name, component.Name);
            Assert.AreEqual(type.ElementID, component.ClassifierID, "wrong classifier ID\n    expected:<{0}>\n     but was:<{1}>", type.Name, component.Type);
            Assert.AreEqual(lowerBound, component.LowerBound);
            Assert.AreEqual(upperBound, component.UpperBound);
        }

        private static Package AssertLibrary(Package parentLibrary, short index, string stereotype, string name, string baseURN, int numberOfSubpackages, int numberOfElements)
        {
            var library = (Package)parentLibrary.Packages.GetAt(index);
            Assert.AreEqual(parentLibrary.PackageID, library.ParentID);
            Assert.AreEqual(stereotype, library.Element.Stereotype);
            Assert.AreEqual(name, library.Name);
            Assert.AreEqual(numberOfSubpackages, library.Packages.Count);
            Assert.AreEqual(numberOfElements, library.Elements.Count);
            AssertTaggedValue("baseURN", baseURN, library);
            return library;
        }

        private static void AssertTaggedValue(string tvName, string tvValue, Package library)
        {
            var tv = (TaggedValue)library.Element.TaggedValues.GetByName(tvName);
            Assert.AreEqual(tvValue, tv.Value);
        }

        public void TestAsFileBasedRepository()
        {
            using (var fileBaseRepository = new TemporaryFileBasedRepository(new EARepository1()))
            {
                
            }
        }
    }
}