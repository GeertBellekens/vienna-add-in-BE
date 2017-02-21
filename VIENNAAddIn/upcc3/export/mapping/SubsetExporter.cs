using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using CctsRepository.DocLibrary;

namespace VIENNAAddIn.upcc3.export.mapping
{
    public class SubsetExporter
    {
        private readonly XmlSchemaSet xmlSchemaSet;
        private readonly string rootDirectory;
        private readonly List<XmlSchema> xmlSchemas;

        public SubsetExporter(string schemaFileComplete)
        {
            XmlSchema rootSchema = XmlSchema.Read(XmlReader.Create(schemaFileComplete), null);
            rootDirectory = rootSchema.SourceUri.Substring(0, rootSchema.SourceUri.LastIndexOf("/") + 1);

            xmlSchemaSet = new XmlSchemaSet();
            xmlSchemaSet.Add(rootSchema);
            xmlSchemaSet.Compile();

            xmlSchemas = new List<XmlSchema>();
            List<string> xmlSchemaNames = new List<string>();

            FlattenXmlSchemaStructure(rootSchema, xmlSchemaNames);
        }

        // The following method the resolves nested file structure such as it might occur in UBL. The
        // output of the method is a list of the XML Schema file names to be processed.
        private void FlattenXmlSchemaStructure(XmlSchema xmlSchema, List<string> xmlSchemaNames)
        {
            if (xmlSchema != null)
            {
                if (!xmlSchemaNames.Contains(xmlSchema.SourceUri))
                {
                    xmlSchemaNames.Add(xmlSchema.SourceUri);
                    xmlSchemas.Add(xmlSchema);
                }
                foreach (XmlSchemaObject include in xmlSchema.Includes)
                {
                    XmlSchema includedSchema = null;

                    if (include is XmlSchemaInclude)
                    {
                        includedSchema = ((XmlSchemaInclude)include).Schema;
                    }
                    else if (include is XmlSchemaImport)
                    {
                        includedSchema = ((XmlSchemaImport)include).Schema;
                    }
                    
                    FlattenXmlSchemaStructure(includedSchema, xmlSchemaNames);
                }
            }
        }

        public static void ExportSubset(IDocLibrary docLibrary, string schemaFileComplete, string schemaDirectorySubset)
        {
            SubsetExporter exporter = new SubsetExporter(schemaFileComplete);

            exporter.ExecuteXmlSchemaSubsetting(docLibrary);

            exporter.WriteXmlSchemaSubset(schemaDirectorySubset);
        }

        public void ExecuteXmlSchemaSubsetting(IDocLibrary docLibrary)
        {
            // The UpccModelXsdTypes contains a list of extracted Xsd Type Names, such as the 
            // name of an ABIE with the qualifier removed. Furthermore, each type, such as an ABIE
            // contains his respective children - in the case of an ABIE this would be the BBIEs
            // of the ABIE.
            UpccModelXsdTypes remainingXsdTypes = new UpccModelXsdTypes(docLibrary);

            HashSet<string> baseTypeNames = new HashSet<string>();

            // Iterate through the XML schema files and for each type we need to check
            // whether the type still exists in our list of remaining types created through
            // the UpccModelXsdTypes constructor (see above). In case the type is contained
            // in the list of types according to the Upcc model, we add the type to a list named 
            // baseTypeNames. The advantage is that we then only have those types from the XML
            // schema in the list which are also contained in the Upcc model.
            foreach (XmlSchema xmlSchema in xmlSchemas)
            {
                foreach (XmlSchemaType type in xmlSchema.SchemaTypes.Values)
                {
                    if (remainingXsdTypes.ContainsXsdType(GetXsdTypeName(type)))
                    {
                        AddBaseTypeNames(type, baseTypeNames);
                    }
                }
            }

            // Based on our knowledge, which types should stay in the XML schema file
            // we are now able to iterate through the XML schema and if necessary delete
            // the complex or simple type from the XML schema.
            foreach (XmlSchema xmlSchema in xmlSchemas)
            {
                foreach (XmlSchemaObject item in CopyValues(xmlSchema.Items))
                {
                    if (item is XmlSchemaComplexType)
                    {
                        string complexTypeName = ((XmlSchemaComplexType)item).QualifiedName.Name;
                        
                        if (remainingXsdTypes.ContainsXsdType(complexTypeName))
                        {
                            RemoveElementsAndAttributesFromComplexType((XmlSchemaComplexType)item, remainingXsdTypes);
                        }
                        else
                        {
                            if (!baseTypeNames.Contains(complexTypeName))
                            {
                                xmlSchema.Items.Remove(item);
                            }
                        }
                    }
                    if (item is XmlSchemaSimpleType)
                    {
                        string simpleTypeName = ((XmlSchemaSimpleType)item).QualifiedName.Name;

                        if (!remainingXsdTypes.ContainsXsdType(simpleTypeName))
                        {                            
                            if (!baseTypeNames.Contains(simpleTypeName))
                            {
                                xmlSchema.Items.Remove(item);
                            }
                        }
                    }
                }

                RemoveGlobalElementsHavingAnInvalidXsdType(xmlSchema, remainingXsdTypes, baseTypeNames);
                RemoveGlobalAttributesHavingAnInvalidXsdType(xmlSchema, remainingXsdTypes, baseTypeNames);
            }
        }

        private static void AddBaseTypeNames(XmlSchemaType xmlSchemaType, HashSet<string> baseTypeNames)
        {
            XmlSchemaType baseType = xmlSchemaType.BaseXmlSchemaType;
            if (baseType != null)
            {
                baseTypeNames.Add(GetXsdTypeName(baseType));
                AddBaseTypeNames(baseType, baseTypeNames);
            }
        }

        private static void RemoveGlobalAttributesHavingAnInvalidXsdType(XmlSchema xmlSchema, UpccModelXsdTypes remainingXsdTypes, HashSet<string> baseTypeNames)
        {           
            foreach (XmlSchemaObject element in CopyValues(xmlSchema.Attributes))
            {
                if (element is XmlSchemaAttribute)
                {
                    XmlSchemaType xsdType = ((XmlSchemaAttribute)element).AttributeSchemaType;
                    string schemaTypeNameInUse = GetXsdTypeName(xsdType);

                    if (!remainingXsdTypes.ContainsXsdType(schemaTypeNameInUse) && !baseTypeNames.Contains(schemaTypeNameInUse))
                    {
                        xmlSchema.Items.Remove(element);                            
                    }
                }
            }
        }

        private static void RemoveGlobalElementsHavingAnInvalidXsdType(XmlSchema xmlSchema, UpccModelXsdTypes remainingXsdTypes, HashSet<string> baseTypeNames)
        {
            foreach (XmlSchemaObject element in CopyValues(xmlSchema.Elements))
            {
                if (element is XmlSchemaElement)
                {
                    XmlSchemaType xsdType = ((XmlSchemaElement)element).ElementSchemaType;
                    string schemaTypeNameInUse = GetXsdTypeName(xsdType);

                    if (!remainingXsdTypes.ContainsXsdType(schemaTypeNameInUse) && !baseTypeNames.Contains(schemaTypeNameInUse))
                    {
                        xmlSchema.Items.Remove(element);
                    }
                }
            }
        }

        private static string GetXsdTypeName(XmlSchemaType xsdType)
        {
            return xsdType.Name ?? xsdType.TypeCode.ToString();
        }

        private static void WriteXmlSchema(XmlSchema xmlSchema, string xmlSchemaFile)
        {
            var xmlWriterSettings = new XmlWriterSettings
                                        {
                                            Indent = true,
                                            Encoding = Encoding.UTF8,
                                        };

            using (var xmlWriter = XmlWriter.Create(xmlSchemaFile, xmlWriterSettings))
            {
                if (xmlWriter != null)
                {
                    xmlSchema.Write(xmlWriter);
                    xmlWriter.Close();
                }
            }
        }

        private static IEnumerable<XmlSchemaObject> CopyValues(XmlSchemaObjectTable objectTable)
        {
            XmlSchemaObject[] objectTableCopy = new XmlSchemaObject[objectTable.Count];

            objectTable.Values.CopyTo(objectTableCopy, 0);

            return objectTableCopy;
        }

        private static IEnumerable<XmlSchemaObject> CopyValues(XmlSchemaObjectCollection objectCollection)
        {
            XmlSchemaObject[] objectCollectionCopy = new XmlSchemaObject[objectCollection.Count];

            objectCollection.CopyTo(objectCollectionCopy, 0);

            return objectCollectionCopy;
        }

        private static void RemoveElementsAndAttributesFromComplexType(XmlSchemaComplexType complexType, UpccModelXsdTypes remainingXsdTypes)
        {
            if (complexType.Particle is XmlSchemaGroupBase)
            {
                RemoveElementsFromXsdGroup((XmlSchemaGroupBase)complexType.Particle, complexType.QualifiedName.Name, remainingXsdTypes);
            }

            if (complexType.ContentModel is XmlSchemaSimpleContent)
            {
                XmlSchemaSimpleContent contentModel = (XmlSchemaSimpleContent)complexType.ContentModel;

                if (contentModel.Content is XmlSchemaSimpleContentExtension)
                {
                    RemoveAttributesFromComplexType(((XmlSchemaSimpleContentExtension)contentModel.Content).Attributes, complexType.QualifiedName.Name, remainingXsdTypes);                    
                }
                else if (contentModel.Content is XmlSchemaSimpleContentRestriction)
                {
                    RemoveAttributesFromComplexType(((XmlSchemaSimpleContentRestriction)contentModel.Content).Attributes, complexType.QualifiedName.Name, remainingXsdTypes);                    
                }
            }

            RemoveAttributesFromComplexType(complexType.Attributes, complexType.QualifiedName.Name, remainingXsdTypes);

            if (complexType.BaseXmlSchemaType != null)
            {
                if (complexType.BaseXmlSchemaType is XmlSchemaComplexType)
                {
                    RemoveElementsAndAttributesFromComplexType((XmlSchemaComplexType)complexType.BaseXmlSchemaType, remainingXsdTypes);    
                }
            }
        }

        private static void RemoveElementsFromXsdGroup(XmlSchemaGroupBase xsdGroup, string xsdTypeName, UpccModelXsdTypes remainingXsdTypes)
        {
            foreach (XmlSchemaObject item in CopyValues(xsdGroup.Items))
            {
                if (item is XmlSchemaElement)
                {
                    string childName = ((XmlSchemaElement) item).QualifiedName.Name;
                   
                    if (!(remainingXsdTypes.XsdTypeContainsChild(xsdTypeName, childName)))
                    {
                        xsdGroup.Items.Remove(item);
                    }
                }
                else if (item is XmlSchemaGroupBase)
                {
                    RemoveElementsFromXsdGroup((XmlSchemaGroupBase) item, xsdTypeName, remainingXsdTypes);
                }
            }
        }

        private static void RemoveAttributesFromComplexType(XmlSchemaObjectCollection attributes, string xsdTypeName, UpccModelXsdTypes remainingXsdTypes)
        {
            foreach (XmlSchemaAttribute attribute in CopyValues(attributes))
            {
                string attributeName = attribute.QualifiedName.Name;

                if (!(remainingXsdTypes.XsdTypeContainsChild(xsdTypeName, attributeName)))
                {
                    attributes.Remove(attribute);                    
                }                
            }
        }

        public void WriteXmlSchemaSubset(string schemaDirectorySubset)
        {
            if (!(Directory.Exists(schemaDirectorySubset)))
            {
                Directory.CreateDirectory(schemaDirectorySubset);
            }
            
            foreach (XmlSchema xmlSchema in xmlSchemas)
            {
                string relativPath = xmlSchema.SourceUri.Substring(rootDirectory.Length - 1);
                WriteXmlSchema(xmlSchema, schemaDirectorySubset + relativPath);
            }  
        }
    }
}
