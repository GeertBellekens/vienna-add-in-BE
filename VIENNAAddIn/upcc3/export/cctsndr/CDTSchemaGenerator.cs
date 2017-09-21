using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using CctsRepository;
using CctsRepository.CdtLibrary;
using VIENNAAddIn.upcc3.repo;
using VIENNAAddInUtils;

namespace VIENNAAddIn.upcc3.export.cctsndr
{
    ///<summary>
    ///</summary>
    public class CDTSchemaGenerator
    {

        ///<summary>
        ///</summary>
        ///<param name="context"></param>
        ///<param name="cdts"></param>
        public static void GenerateXSD(GeneratorContext context, IEnumerable<ICdt> cdts)
        {
            var schema = new XmlSchema { TargetNamespace = context.TargetNamespace };
            schema.Namespaces.Add(context.NamespacePrefix, context.TargetNamespace);
            schema.Namespaces.Add("xsd", "http://www.w3.org/2001/XMLSchema");
            schema.Namespaces.Add("ccts", "urn:un:unece:uncefact:documentation:standard:XMLNDRDocumentation:3");
            schema.Version = context.DocLibrary.VersionIdentifier.DefaultTo("1");

            foreach (ICdt cdt in cdts)
            {
                var sups = new List<ICdtSup>(cdt.Sups);
                if (sups.Count == 0)
                {
                    var simpleType = new XmlSchemaSimpleType { Name = GetTypeName(cdt) };
                    var simpleTypeRestriction = new XmlSchemaSimpleTypeRestriction
                                                {
                    								BaseTypeName = GetXmlQualifiedName(NDR.getConBasicTypeName(cdt))
                                                };
                    simpleType.Content = simpleTypeRestriction;
                    if (context.Annotate)
                    {
                        simpleType.Annotation = GetTypeAnnotation(cdt);
                    }
                    schema.Items.Add(simpleType);
                }
                else
                {
                    var complexType = new XmlSchemaComplexType
                                      {
                                          // Deviation from rule [R 90FB]: Using simpler and shorter names for CDT complex types.
                                          Name = GetTypeName(cdt)
                                      };
                    var simpleContent = new XmlSchemaSimpleContent();
                    var simpleContentExtension = new XmlSchemaSimpleContentExtension
                                                 {
                                                     BaseTypeName = GetXmlQualifiedName(NDR.getConBasicTypeName(cdt))
                                                 };
                    foreach (ICdtSup sup in sups)
                    {
                        var attribute = new XmlSchemaAttribute
                                        {
                                            // Deviation from rule [R ABC1]: Using only attribute name and type as xml attribute name (instead of complete DEN), following the examples given in the specification.
                                            Name = GetAttributeName(sup),
                                            SchemaTypeName = new XmlQualifiedName(GetXSDType(NDR.GetBasicTypeName(sup as UpccUmlAttribute)),
                                                                                  "http://www.w3.org/2001/XMLSchema"),
                                        };
                        if (context.Annotate)
                        {
                            attribute.Annotation = GetAttributeAnnotation(sup);
                        }
                        simpleContentExtension.Attributes.Add(attribute);
                    }

                    simpleContent.Content = simpleContentExtension;
                    complexType.ContentModel = simpleContent;
                    if (context.Annotate)
                    {
                        complexType.Annotation = GetTypeAnnotation(cdt);
                    }
                    schema.Items.Add(complexType);
                }
            }

            context.AddSchema(schema, "CoreDataType_" + schema.Version + ".xsd", Schematype.CDT);
        
        }

        private static XmlSchemaAnnotation GetAttributeAnnotation(ICdtSup sup)
        {
            var xml = new XmlDocument();
            // Deviation from rule [R 9C95]: Generating only a subset of the defined annotations and added some additional annotations.
            var annNodes = new List<XmlNode>();
            AddAnnotation(xml, annNodes, "PropertyTermName", sup.Name);
            AddAnnotation(xml, annNodes, "RepresentationTermName", NDR.GetBasicTypeName(sup as UpccUmlAttribute));
            AddAnnotation(xml, annNodes, "PrimitiveTypeName", NDR.GetBasicTypeName(sup as UpccUmlAttribute));
            AddAnnotation(xml, annNodes, "DataTypeName", sup.Cdt.Name);
            AddAnnotation(xml, annNodes, "UniqueID", sup.UniqueIdentifier);
            AddAnnotation(xml, annNodes, "VersionID", sup.VersionIdentifier);
            AddAnnotation(xml, annNodes, "DictionaryEntryName", sup.DictionaryEntryName);
            AddAnnotation(xml, annNodes, "Definition", sup.Definition);
            AddAnnotations(xml, annNodes, "BusinessTermName", sup.BusinessTerms);
            AddAnnotation(xml, annNodes, "ModificationAllowedIndicator",
                          sup.ModificationAllowedIndicator.ToString().ToLower());
            AddAnnotation(xml, annNodes, "LanguageCode", sup.LanguageCode);
            AddAnnotation(xml, annNodes, "AcronymCode", "SUP");
            var ann = new XmlSchemaAnnotation();
            ann.Items.Add(new XmlSchemaDocumentation { Language = "en", Markup = annNodes.ToArray() });
            return ann;
        }

        private static XmlSchemaAnnotation GetTypeAnnotation(ICdt cdt)
        {
            var xml = new XmlDocument();
            // Deviation from rule [R BFE5]: Generating only a subset of the defined annotations and added some additional annotations.
            var annNodes = new List<XmlNode>();
            AddAnnotation(xml, annNodes, "UniqueID", cdt.UniqueIdentifier);
            AddAnnotation(xml, annNodes, "VersionID", cdt.VersionIdentifier);
            AddAnnotation(xml, annNodes, "DictionaryEntryName", cdt.DictionaryEntryName);
            AddAnnotation(xml, annNodes, "Definition", cdt.Definition);
            AddAnnotations(xml, annNodes, "BusinessTermName", cdt.BusinessTerms);
            AddAnnotation(xml, annNodes, "PropertyTermName", cdt.Name);
            AddAnnotation(xml, annNodes, "LanguageCode", cdt.LanguageCode);
            AddAnnotation(xml, annNodes, "AcronymCode", "CDT");
            var ann = new XmlSchemaAnnotation();
            ann.Items.Add(new XmlSchemaDocumentation { Language = "en", Markup = annNodes.ToArray() });
            return ann;
        }

        private static void AddAnnotations(XmlDocument xml, List<XmlNode> annNodes, string name,
                                           IEnumerable<string> values)
        {
            foreach (string item in values)
            {
                AddAnnotation(xml, annNodes, name, item);
            }
        }

        private static void AddAnnotation(XmlDocument xml, List<XmlNode> annNodes, string name, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                XmlElement annotation = xml.CreateElement("ccts", name,
                                                          "urn:un:unece:uncefact:documentation:standard:XMLNDRDocumentation:3");
                annotation.InnerText = value;
                annNodes.Add(annotation);
            }
        }

        private static XmlQualifiedName GetXmlQualifiedName(string basicTypeName)
        {
            return new XmlQualifiedName(GetXSDType(basicTypeName), "http://www.w3.org/2001/XMLSchema");
        }

        private static string GetAttributeName(ICdtSup sup)
        {
            string name = sup.Name + NDR.GetBasicTypeName(sup as UpccUmlAttribute);
            return name.Replace(".", "");
        }

        private static string GetTypeName(ICdt cdt)
        {
            return cdt.Name + NDR.getConBasicTypeName(cdt) + "Type";
        }

        private static string GetXSDType(string primitiveTypeName)
        {
            switch (primitiveTypeName.ToLower())
            {
                case "string":
                    return "string";
                case "decimal":
                    return "decimal";
                case "binary":
                    return "base64Binary";
                case "base64binary":
                    return "base64Binary";
                case "token":
                    return "token";
                case "double":
                    return "double";
                case "integer":
                    return "integer";
                case "long":
                    return "long";
                case "datetime":
                    return "dateTime";
                case "date":
                    return "date";
                case "time":
                    return "time";
                case "boolean":
                    return "boolean";
                default:
                    return "string";
            }
        }
    }
}