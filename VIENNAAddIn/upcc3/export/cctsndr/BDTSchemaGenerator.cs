using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Xml;
using System.Xml.Schema;
using CctsRepository;
using CctsRepository.BdtLibrary;
using CctsRepository.EnumLibrary;
using VIENNAAddIn.upcc3.repo;
using VIENNAAddIn.upcc3.repo.EnumLibrary;
using VIENNAAddInUtils;

namespace VIENNAAddIn.upcc3.export.cctsndr
{
    public static class BDTSchemaGenerator
    {
        private const string NSPREFIX_XSD = "xsd";
        private const string NS_XSD = "http://www.w3.org/2001/XMLSchema";

        public static void GenerateXSD(GeneratorContext context, IEnumerable<IBdt> bdts, XmlSchema schema)
        {
            context.AddElements(bdts);
            GenerateXSD(context, schema);
        }
        public static void GenerateXSD(GeneratorContext context, XmlSchema schema)
        {
            var enums = new List<IEnum>();
            //loop the bdt's
            foreach (IBdt bdt in context.Elements
                                 .OfType<IBdt>().Where(x => !x.isDirectXSDType))
            {
                var sups = new List<IBdtSup>(bdt.Sups);
                if (!sups.Any())
                {
                    var simpleType = new XmlSchemaSimpleType { Name = NDR.GetXsdTypeNameFromBdt(bdt) };
                    var simpleTypeRestriction = new XmlSchemaSimpleTypeRestriction();
                    simpleTypeRestriction.BaseTypeName = GetXmlQualifiedName(NDR.getConBasicTypeName(bdt), context);
                    simpleType.Content = simpleTypeRestriction;
                    if (bdt.Con != null
                        && bdt.Con.BasicType != null
                        && bdt.Con.BasicType.Prim != null)
                    {
                        var XSDtype = bdt.Con.BasicType.Prim.xsdType;
                        if (!string.IsNullOrEmpty(XSDtype))
                        {
                            simpleTypeRestriction.BaseTypeName = new XmlQualifiedName(NSPREFIX_XSD + ":" + XSDtype);
                        }
                    }

                    if (context.Annotate)
                    {
                        simpleType.Annotation = GetTypeAnnotation(bdt);
                    }
                    schema.Items.Add(simpleType);
                }
                else
                {
                    //create the complex type
                    var complexType = new XmlSchemaComplexType();
                    complexType.Name = NDR.GetXsdTypeNameFromBdt(bdt);
                    //add the simple content extension
                    var simpleContent = new XmlSchemaSimpleContent();
                    var simpleContentExtension = new XmlSchemaSimpleContentExtension();
                    //get the xsd type if the con is a primitive
                    if (bdt.Con.BasicType != null
                        && bdt.Con.BasicType.Prim != null)
                    {
                        simpleContentExtension.BaseTypeName = GetXmlQualifiedName(bdt.Con.BasicType.Prim.xsdType, context);
                    }
                    else
                    {
                        var basicEnum = bdt.Con?.BasicType?.Enum;
                        if (basicEnum != null)
                        {
                            //add the enum to the list
                            if (!enums.Any(x => x.Name == basicEnum.Name)) enums.Add(basicEnum);
                            simpleContentExtension.BaseTypeName = new XmlQualifiedName(context.NamespacePrefix + ":" + NDR.GetBasicTypeName(basicEnum));
                        }
                        else
                        {
                            simpleContentExtension.BaseTypeName = GetXmlQualifiedName(NDR.getConBasicTypeName(bdt), context);
                        }
                    }

                    foreach (IBdtSup sup in sups)
                    {
                        var attribute = new XmlSchemaAttribute();
                        // Deviation from rule [R ABC1]: Using only attribute name and type as xml attribute name (instead of complete DEN), following the examples given in the specification.
                        attribute.Name = sup.Name;
                        //set optional or required
                        attribute.Use = sup.IsOptional() ? XmlSchemaUse.Optional : XmlSchemaUse.Required;
                        //set the type of the attribute                                   
                        if (sup.BasicType != null
                           && sup.BasicType.IsEnum)
                        {
                            //figure out if the set of values is restricted
                            var basicEnum = sup.BasicType.Enum as UpccEnum;
                            if (basicEnum != null)
                            {
                                //add the enum to the list
                                if (!enums.Any(x => x.Name == basicEnum.Name)) enums.Add(basicEnum);
                                attribute.SchemaTypeName = new XmlQualifiedName(context.NamespacePrefix + ":" + NDR.GetBasicTypeName(basicEnum));
                            }
                        }
                        //set regular type name if not restricted
                        if (attribute.SchemaTypeName.IsEmpty
                           && attribute.SchemaType == null)
                        {
                            attribute.SchemaTypeName = GetXmlQualifiedName(NDR.GetBasicTypeName(sup as UpccAttribute), context);
                        }
                        //annotate if needed
                        if (context.Annotate)
                        {
                            attribute.Annotation = GetAttributeAnnotation(sup);
                        }
                        //add the attribute
                        simpleContentExtension.Attributes.Add(attribute);
                    }

                    simpleContent.Content = simpleContentExtension;
                    complexType.ContentModel = simpleContent;
                    if (context.Annotate)
                    {
                        complexType.Annotation = GetTypeAnnotation(bdt);
                    }
                    schema.Items.Add(complexType);
                }
            }
            context.AddElements(enums);
        }


        static void addEnumerationValues(XmlSchemaSimpleTypeRestriction restriction, UpccEnum basicEnum)
        {
            foreach (var codeListEntry in basicEnum.CodelistEntries)
            {
                var xmlEnum = new XmlSchemaEnumerationFacet();
                xmlEnum.Value = codeListEntry.Name;
                restriction.Facets.Add(xmlEnum);
            }
        }

        private static XmlSchemaAnnotation GetAttributeAnnotation(IBdtSup sup)
        {
            var xml = new XmlDocument();
            // Deviation from rule [R 9C95]: Generating only a subset of the defined annotations and added some additional annotations.
            var annNodes = new List<XmlNode>();
            SchemaGeneratorUtils.AddAnnotation(xml, annNodes, "PropertyTermName", sup.Name);
            SchemaGeneratorUtils.AddAnnotation(xml, annNodes, "RepresentationTermName", sup.BasicType.Name);
            SchemaGeneratorUtils.AddAnnotation(xml, annNodes, "PrimitiveTypeName", sup.BasicType.Name);
            SchemaGeneratorUtils.AddAnnotation(xml, annNodes, "DataTypeName", sup.Bdt.Name);
            SchemaGeneratorUtils.AddAnnotation(xml, annNodes, "UniqueID", sup.UniqueIdentifier);
            SchemaGeneratorUtils.AddAnnotation(xml, annNodes, "VersionID", sup.VersionIdentifier);
            SchemaGeneratorUtils.AddAnnotation(xml, annNodes, "DictionaryEntryName", sup.DictionaryEntryName);
            SchemaGeneratorUtils.AddAnnotation(xml, annNodes, "Definition", sup.Definition);
            SchemaGeneratorUtils.AddAnnotations(xml, annNodes, "BusinessTermName", sup.BusinessTerms);
            SchemaGeneratorUtils.AddAnnotation(xml, annNodes, "ModificationAllowedIndicator",
                          sup.ModificationAllowedIndicator.ToString().ToLower());
            SchemaGeneratorUtils.AddAnnotation(xml, annNodes, "LanguageCode", sup.LanguageCode);
            SchemaGeneratorUtils.AddAnnotation(xml, annNodes, "AcronymCode", "SUP");
            var ann = new XmlSchemaAnnotation();
            ann.Items.Add(new XmlSchemaDocumentation { Language = "en", Markup = annNodes.ToArray() });
            return ann;
        }

        private static XmlSchemaAnnotation GetTypeAnnotation(IBdt bdt)
        {
            var xml = new XmlDocument();
            // Deviation from rule [R BFE5]: Generating only a subset of the defined annotations and added some additional annotations.
            var annNodes = new List<XmlNode>();
            SchemaGeneratorUtils.AddAnnotation(xml, annNodes, "UniqueID", bdt.UniqueIdentifier);
            SchemaGeneratorUtils.AddAnnotation(xml, annNodes, "VersionID", bdt.VersionIdentifier);
            SchemaGeneratorUtils.AddAnnotation(xml, annNodes, "DictionaryEntryName", bdt.DictionaryEntryName);
            SchemaGeneratorUtils.AddAnnotation(xml, annNodes, "Definition", bdt.Definition);
            SchemaGeneratorUtils.AddAnnotations(xml, annNodes, "BusinessTermName", bdt.BusinessTerms);
            SchemaGeneratorUtils.AddAnnotation(xml, annNodes, "PropertyTermName", bdt.Name);
            SchemaGeneratorUtils.AddAnnotation(xml, annNodes, "LanguageCode", bdt.LanguageCode);
            SchemaGeneratorUtils.AddAnnotation(xml, annNodes, "AcronymCode", "BDT");
            var ann = new XmlSchemaAnnotation();
            ann.Items.Add(new XmlSchemaDocumentation { Language = "en", Markup = annNodes.ToArray() });
            return ann;
        }


        private static XmlQualifiedName GetXmlQualifiedName(string basicTypeName, GeneratorContext context)
        {
            if (basicTypeName.StartsWith("Assembled", System.StringComparison.InvariantCulture))
            {
                return new XmlQualifiedName(basicTypeName, context.BaseURN);
            }
            return new XmlQualifiedName(basicTypeName, NS_XSD);
        }


    }
}