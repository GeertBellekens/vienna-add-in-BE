// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************

using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;

namespace VIENNAAddIn.upcc3.Wizards.dev.util
{
    public class XMLSchemaReader
    {
        private const double xsdAttributeWeight = 1;
        private const double xsdAnyWeight = 0.5;
        private const double xsdAnyAttributeWeight = 0;
        private const double xsdChoiceWeight = 0.5;
        private const double xsdRedefineWeight = 0.5;
        private const double xsdSubstitutionGroupWeight = 0.5;
        private const double xsiTypeWeight = 0;
        private const double xsdGroupWeight = 0;
        private const double xsdKeyWeight = 0;
        private const double xsdKeyRefWeight = 0;
        private const double xsdUnionWeight = 0;
        private const double xsdListWeight = 0;
        private const double xsdAttributeGroupWeight = 0.5;
        private const double xsdUniqueWeight = 0;
        private const double xsdComplexTypeWeight = 1;
        private const double xsdSimpleTypeWeight = 1;
        private const double xsdElementWeight = 1;
        private const double xsdSequenceWeight = 1;
        private const double xsdAllWeight = 0.5;
        private const double xsdRestrictionWeight = 0;
        private const double xsdExtensionWeight = 0;

        private static int xsdAttribute;
        private static int xsdAny;
        private static int xsdAll;
        private static int xsdAnyAttribute;
        private static int xsdChoice;
        private static int xsdRedefine;
        private static int xsdSubstitutionGroup;
        private static int xsiType;
        private static int xsdGroup;
        private static int xsdKey;
        private static int xsdKeyRef;
        private static int xsdUnion;
        private static int xsdList;
        private static int xsdAttributeGroup;
        private static int xsdUnique;
        private static int xsdComplexType;
        private static int xsdSimpleType;
        private static int xsdElement;
        private static int xsdSequence;
        private static int xsdRestriction;
        private static int xsdExtension;

        private static List<XmlSchemaObject> foundTypes;
        private static HashSet<XmlQualifiedName> foundRefs;
        private static HashSet<XmlQualifiedName> consideredRefs;

        private static XmlSchemaSet xmlSchemaSet;
        private static List<XmlSchema> xmlSchemas;



        private static void FlattenXmlSchemaStructure(XmlSchema xmlSchema, ICollection<string> xmlSchemaNames)
        {
            if (xmlSchema != null)
            {
                if (!xmlSchemaNames.Contains(xmlSchema.SourceUri))
                {
                    xmlSchemaNames.Add(xmlSchema.SourceUri);
                    xmlSchemas.Add(xmlSchema);
                }
                foreach (var include in xmlSchema.Includes)
                {
                    XmlSchema includedSchema = null;

                    if (include is XmlSchemaInclude)
                    {
                        includedSchema = ((XmlSchemaInclude)include).Schema;
                    }
                    else if (include is XmlSchemaImport)
                    {
                        includedSchema = ((XmlSchemaImport)include).Schema;
                        if (includedSchema == null)
                        {
                            var settings = new XmlReaderSettings {ProhibitDtd = false};
                            settings.ValidationEventHandler += ValidationCallBack;

                            var import = ((XmlSchemaImport)include);
                            includedSchema = XmlSchema.Read(XmlReader.Create(import.SchemaLocation,settings),null);
                        }
                    }
                    FlattenXmlSchemaStructure(includedSchema, xmlSchemaNames);
                }
            }
        }

        private static void ResolveFoundTypes()
        {
            var list = new List<XmlSchemaObject>();
            list.AddRange(foundTypes);
            foundTypes.Clear();
            foreach (var item in list)
            {
                var collection = new XmlSchemaObjectCollection {item};
                countXsdElements(collection);
            }
        }

        public static SchemaAnalyzerResults Read(string filename)
        {
            var results = new SchemaAnalyzerResults();
            resetResults();
            foundTypes = new List<XmlSchemaObject>();
            foundRefs = new HashSet<XmlQualifiedName>();
            consideredRefs = new HashSet<XmlQualifiedName>();
            
            var rootSchema = XmlSchema.Read(XmlReader.Create(filename), null);

            xmlSchemaSet = new XmlSchemaSet();
            xmlSchemaSet.Add(rootSchema);

            xmlSchemas = new List<XmlSchema>();
            var xmlSchemaNames = new List<string>();
            FlattenXmlSchemaStructure(rootSchema, xmlSchemaNames);
            
            foreach (var schema in xmlSchemas)
            {
                countXsdElements(schema.Items);
            }
            ResolveFoundTypes();
            ResolveFoundTypes(); //TODO: check if this second iteration is necessary for finding base types!

            results.Clear();

            results.Add(new SchemaAnalyzerResult("All", xsdAll, xsdAllWeight));
            results.Add(new SchemaAnalyzerResult("Any", xsdAny, xsdAnyWeight));
            results.Add(new SchemaAnalyzerResult("AnyAttribute", xsdAnyAttribute, xsdAnyAttributeWeight));
            results.Add(new SchemaAnalyzerResult("Attribute", xsdAttribute, xsdAttributeWeight));
            results.Add(new SchemaAnalyzerResult("AttributeGroup", xsdAttributeGroup, xsdAttributeGroupWeight));
            results.Add(new SchemaAnalyzerResult("Choice", xsdChoice, xsdChoiceWeight));
            results.Add(new SchemaAnalyzerResult("ComplexType", xsdComplexType, xsdComplexTypeWeight));
            results.Add(new SchemaAnalyzerResult("Element", xsdElement, xsdElementWeight));
            results.Add(new SchemaAnalyzerResult("Extension", xsdExtension, xsdExtensionWeight));
            results.Add(new SchemaAnalyzerResult("Group", xsdGroup, xsdGroupWeight));
            results.Add(new SchemaAnalyzerResult("Key", xsdKey, xsdKeyWeight));
            results.Add(new SchemaAnalyzerResult("KeyRef", xsdKeyRef, xsdKeyRefWeight));
            results.Add(new SchemaAnalyzerResult("List", xsdList, xsdListWeight));
            results.Add(new SchemaAnalyzerResult("Redefine", xsdRedefine, xsdRedefineWeight));
            results.Add(new SchemaAnalyzerResult("Restriction",xsdRestriction,xsdRestrictionWeight));
            results.Add(new SchemaAnalyzerResult("Sequence", xsdSequence, xsdSequenceWeight));
            results.Add(new SchemaAnalyzerResult("SimpleType", xsdSimpleType, xsdSimpleTypeWeight));
            results.Add(new SchemaAnalyzerResult("SubstitutionGroup", xsdSubstitutionGroup, xsdSubstitutionGroupWeight));
            results.Add(new SchemaAnalyzerResult("Union", xsdUnion, xsdUnionWeight));
            results.Add(new SchemaAnalyzerResult("Unique", xsdUnique, xsdUniqueWeight));
            results.Add(new SchemaAnalyzerResult("xsi:Type", xsiType, xsiTypeWeight));

            #region Console Notifications
            //Console.WriteLine("Schema features " + xsdAll + " All.");
            //Console.WriteLine("Schema features " + xsdAny + " xsAny.");
            //Console.WriteLine("Schema features " + xsdAnyAttribute + " AnyAttributes.");
            //Console.WriteLine("Schema features " + xsdAttribute + " Attributes.");
            //Console.WriteLine("Schema features " + xsdAttributeGroup + " AttributeGroups.");
            //Console.WriteLine("Schema features " + xsdChoice + " Choices.");
            //Console.WriteLine("Schema features " + xsdComplexType + " ComplexTypes.");
            //Console.WriteLine("Schema features " + xsdElement + " Elements.");
            //Console.WriteLine("Schema features " + xsdExtension + " Extensions.");
            //Console.WriteLine("Schema features " + xsdGroup + " Groups.");
            //Console.WriteLine("Schema features " + xsdKey + " xsKey.");
            //Console.WriteLine("Schema features " + xsdKeyRef + " KeyRefs.");
            //Console.WriteLine("Schema features " + xsiType + " xsi:Types.");
            //Console.WriteLine("Schema features " + xsdUnique + " Uniques.");
            //Console.WriteLine("Schema features " + xsdUnion + " Unions.");
            //Console.WriteLine("Schema features " + xsdList + " Lists.");
            //Console.WriteLine("Schema features " + xsdRedefine + " Redefines.");
            //Console.WriteLine("Schema features " + xsdRestriction + " Restrictions.");
            //Console.WriteLine("Schema features " + xsdSequence + " Sequences.");
            //Console.WriteLine("Schema features " + xsdSimpleType + " SimpleTypes.");
            //Console.WriteLine("Schema features " + xsdSubstitutionGroup + " SubstitutionGroups.");
            #endregion

            results.Sort(new SchemaAnalyzerResultComparerByValue());

            return results;
        }

        private static void ValidationCallBack(object sender, ValidationEventArgs e)
        {
            throw new XmlSchemaValidationException(e.Message);
        }
        private static void resetResults()
        {
        xsdAttribute = 0;
        xsdAny = 0;
        xsdAll = 0;
        xsdAnyAttribute = 0;
        xsdChoice = 0;
        xsdRedefine = 0;
        xsdSubstitutionGroup = 0;
        xsiType = 0;
        xsdGroup = 0;
        xsdKey = 0;
        xsdKeyRef = 0;
        xsdUnion = 0;
        xsdList = 0;
        xsdAttributeGroup = 0;
        xsdUnique = 0;
        xsdComplexType = 0;
        xsdSimpleType = 0;
        xsdElement = 0;
        xsdSequence = 0;
        xsdRestriction = 0;
        xsdExtension = 0;
        }
        private static void countXsdElements(XmlSchemaObjectCollection items)
        {
            foreach (var item in items)
            {
                if (item is XmlSchemaComplexType)
                {
                    var complexType = (XmlSchemaComplexType)item;

                    if (foundRefs.Contains(complexType.QualifiedName))
                    {
                        xsdComplexType++;

                        if (complexType.Particle is XmlSchemaAll)
                        {
                            xsdAll++;
                            var complexTypeParticle = (XmlSchemaAll) complexType.Particle;
                            countXsdElements(complexTypeParticle.Items);
                        }
                        if (complexType.Particle is XmlSchemaChoice)
                        {
                            xsdChoice++;
                            var complexTypeParticle = (XmlSchemaChoice) complexType.Particle;
                            countXsdElements(complexTypeParticle.Items);
                        }
                        if (complexType.Particle is XmlSchemaSequence)
                        {
                            xsdSequence++;
                            var complexTypeParticle = (XmlSchemaSequence) complexType.Particle;
                            countXsdElements(complexTypeParticle.Items);
                        }
                        countXsdElements(complexType.Attributes);

                        var obj = (XmlSchemaObject) complexType.ContentModel;
                        if (obj != null)
                        {
                            var list = new XmlSchemaObjectCollection {obj};
                            countXsdElements(list);
                        }

                        foundRefs.Remove(complexType.QualifiedName);
                        consideredRefs.Add(complexType.QualifiedName);
                    }
                    else if (consideredRefs.Contains(complexType.QualifiedName))
                    {
                        continue;
                    }
                    else
                    {
                        foundTypes.Add(item);
                    }
                }
                if(item is XmlSchemaSimpleContentExtension)
                {
                    xsdExtension++;
                    var extension = (XmlSchemaSimpleContentExtension)item;
                    countXsdElements(extension.Attributes);
                    if (extension.BaseTypeName.Name.Length > 0)
                    {
                        if (!consideredRefs.Contains(extension.BaseTypeName))
                        {
                            foundRefs.Add(extension.BaseTypeName);
                        }
                    }
                }
                if(item is XmlSchemaSimpleContentRestriction)
                {
                    xsdRestriction++;
                    var restriction = (XmlSchemaSimpleContentRestriction) item;
                    countXsdElements(restriction.Attributes);
                    countXsdElements(restriction.Facets);
                    if (restriction.BaseTypeName.Name.Length > 0)
                    {
                        if (!consideredRefs.Contains(restriction.BaseTypeName))
                        {
                            foundRefs.Add(restriction.BaseTypeName);
                        }
                    }
                }
                if (item is XmlSchemaSimpleContent)
                {
                    var xmlSchemaSimpleContent = (XmlSchemaSimpleContent) item;

                    var obj = (XmlSchemaObject) xmlSchemaSimpleContent.Content;
                    if (obj != null)
                    {
                        var list = new XmlSchemaObjectCollection {obj};
                        countXsdElements(list);
                    }
                }
                if(item is XmlSchemaAttributeGroup)
                {
                    xsdAttributeGroup++;
                    var xmlSchemaAttributeGroup = (XmlSchemaAttributeGroup) item;
                    countXsdElements(xmlSchemaAttributeGroup.Attributes);
                }
                if(item is XmlSchemaSimpleTypeUnion)
                {
                    var xmlSchemaSimpleTypeUnion = (XmlSchemaSimpleTypeUnion) item;
                    if(xmlSchemaSimpleTypeUnion.MemberTypes.Length > 0)
                    {
                        xsiType++;
                    }
                    xsdUnion++;
                    countXsdElements(new XmlSchemaObjectCollection(xmlSchemaSimpleTypeUnion));
                }
                if(item is XmlSchemaSimpleTypeList)
                {
                    xsdList++;
                }
                if(item is XmlSchemaSimpleTypeRestriction)
                {
                    xsdRestriction++;
                    var xmlSchemaSimpleTypeRestriction = (XmlSchemaSimpleTypeRestriction) item;
                    countXsdElements(xmlSchemaSimpleTypeRestriction.Facets);
                    if (xmlSchemaSimpleTypeRestriction.BaseTypeName.Name.Length > 0)
                    {
                        if (!consideredRefs.Contains(xmlSchemaSimpleTypeRestriction.BaseTypeName))
                        {
                            foundRefs.Add(xmlSchemaSimpleTypeRestriction.BaseTypeName);
                        }
                    }
                }
                if (item is XmlSchemaSimpleType)
                {
                    var xmlSchemaSimpleType = (XmlSchemaSimpleType) item;

                    if (foundRefs.Contains(xmlSchemaSimpleType.QualifiedName))
                    {
                        xsdSimpleType++;

                        var obj = (XmlSchemaObject)xmlSchemaSimpleType.Content;
                        if (obj != null)
                        {
                            var list = new XmlSchemaObjectCollection {obj};
                            countXsdElements(list);
                        }
                        foundRefs.Remove(xmlSchemaSimpleType.QualifiedName);
                        consideredRefs.Add(xmlSchemaSimpleType.QualifiedName);
                    }
                    else if (consideredRefs.Contains(xmlSchemaSimpleType.QualifiedName))
                    {
                        continue;
                    }
                    else
                    {
                        foundTypes.Add(item);
                    }
                }
                if(item is XmlSchemaAny)
                {
                    xsdAny++;
                }
                if(item is XmlSchemaAnyAttribute)
                {
                    xsdAnyAttribute++;
                }
                if(item is XmlSchemaRedefine)
                {
                    xsdRedefine++;
                    var xmlSchemaRedefine = (XmlSchemaRedefine) item;
                    countXsdElements(xmlSchemaRedefine.Items);
                }
                if(item is XmlSchemaUnique)
                {
                    xsdUnique++;
                    var xmlSchemaUnique = (XmlSchemaUnique) item;
                    countXsdElements(xmlSchemaUnique.Fields);
                }
                if(item is XmlSchemaKey)
                {
                    xsdKey++;
                    var xmlSchemaKey = (XmlSchemaKey) item;
                    countXsdElements(xmlSchemaKey.Fields);
                }
                if(item is XmlSchemaKeyref)
                {
                    xsdKeyRef++;
                    var xmlSchemaKeyref = (XmlSchemaKeyref)item;
                    countXsdElements(xmlSchemaKeyref.Fields);
                }
                if (item is XmlSchemaElement)
                {
                    var element = (XmlSchemaElement)item;
                    if (element.RefName.IsEmpty)
                    {
                        xsdElement++;
                    }
                    if(!element.SubstitutionGroup.IsEmpty)
                    {
                        xsdSubstitutionGroup++;
                    }
                    if (element.SchemaTypeName.Name.Length > 0)
                    {
                        if (!consideredRefs.Contains(element.SchemaTypeName))
                        {
                            foundRefs.Add(element.SchemaTypeName);
                        }
                    }
                }
                if (item is XmlSchemaAttribute)
                {
                    var attribute = (XmlSchemaAttribute)item;
                    if (attribute.RefName.IsEmpty)
                    {
                        xsdAttribute++;
                    }
                    if (attribute.SchemaTypeName.Name.Length > 0)
                    {
                        if (!consideredRefs.Contains(attribute.SchemaTypeName))
                        {
                            foundRefs.Add(attribute.SchemaTypeName);
                        }
                    }
                }
                if (item is XmlSchemaGroup)
                {
                    xsdGroup++;
                    var xmlSchemaGroup = (XmlSchemaGroup) item;
                    countXsdElements(new XmlSchemaObjectCollection(xmlSchemaGroup.Particle));
                }
                #region SAXImplementation:old
                //switch (element.Name)
                // {
                //     case "xs:element":
                //         xsdElement++;
                //         break;
                //     case "xs:complexType":
                //         xsdComplexType++;
                //         break;
                //     case "xs:simpleType":
                //         xsdSimpleType++;
                //         break;
                //     case "xs:choice":
                //         xsdChoice++;
                //         break;
                //     case "xs:key":
                //         xsdKey++;
                //         break;
                //     case "xs:any":
                //         xsdAny++;
                //         break;
                //     case "xs:group":
                //         xsdGroup++;
                //         break;
                //     case "xs:sequence":
                //         xsdSequence++;
                //         break;
                //     case "xs:redefine":
                //         xsdRedefine++;
                //         break;
                //     case "xs:substitutionGroup":
                //         xsdSubstitutionGroup++;
                //         break;
                //     case "xsi:type":
                //         xsiType++;
                //         break;
                //     case "xs:attribute":
                //         xsdAttribute++;
                //         break;
                //     case "xs:anyAttribute":
                //         xsdAnyAttribute++;
                //         break;
                //     case "xs:keyRef":
                //         xsdKeyRef++;
                //         break;
                //     case "xs:union":
                //         xsdUnion++;
                //         break;
                //     case "xs:list":
                //         xsdList++;
                //         break;
                //     case "xs:attributeGroup":
                //         xsdAttributeGroup++;
                //         break;
                //     case "xs:unique":
                //         xsdUnique++;
                //         break;
                // }
                #endregion
            }
        }
    }
}
