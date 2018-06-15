using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using CctsRepository;
using CctsRepository.BdtLibrary;
using CctsRepository.BieLibrary;
using CctsRepository.DocLibrary;
using CctsRepository.EnumLibrary;
using CctsRepository.PrimLibrary;
using VIENNAAddIn.upcc3.repo.EnumLibrary;
using VIENNAAddInUtils;

// XML Naming and Design Rules that are currently not considered:
//
// R 984C, R 8E2D, R 8CED, R B56B, R B387
//    --> namespace design
//
// R 92B8, R 8D58, R 8252
//    --> naming of the generated XSD file

// R 84BE, R 9049, R A735, R AFA8, R BBD5, R 998B
//    --> versioning of the schema file within xsd:schema
//
// R ABD2
//    --> comment within the file
//
// R 90F1, R9623, R 9443
//    --> CCTS metadata file
//
// R 847A, R A9EB, R 9B07, R 88DE, R B851, R A1CF, R A538
//    --> generation of appInfo information is just not clear to me yet.
//
// R 90F9
//    --> sequencing currently ignored 
//

namespace VIENNAAddIn.upcc3.export.cctsndr
{
    ///<summary>
    ///</summary>
    public class BIESchemaGenerator
    {
        private const string NSPREFIX_XSD = "xsd";
        private const string NS_XSD = "http://www.w3.org/2001/XMLSchema";

        private static List<String> globalASBIEs;

        public static void GenerateXSD(GeneratorContext context, IEnumerable<IAbie> abies, XmlSchema schema)
        {
            context.AddElements(abies);
            GenerateXSD(context, schema);
        }

        public static void GenerateXSD(GeneratorContext context, XmlSchema schema)
        {
            globalASBIEs = new List<string>();
            foreach (IAbie abie in context.Elements.OfType<IAbie>().OrderBy(a => a.Name))
            {
                // finally add the complex type to the schema
                schema.Items.Add(GenerateComplexTypeABIE(context, schema, abie));
            }
        }

        internal static XmlSchemaComplexType GenerateComplexTypeABIE(GeneratorContext context, XmlSchema schema, IAbie abie)
        {
            // R A4CE, R AF95: a complex type must be defined for each ABIE   
            XmlSchemaComplexType complexTypeBIE = new XmlSchemaComplexType();

            // R 9D83: the name of the ABIE must be the DictionaryEntryName with all whitespace and separators 
            //         removed. The 'Details' suffix is replaced with 'Type'.
            complexTypeBIE.Name = NDR.TrimElementName(abie.Name);

            var processedProperties = new List<ICctsProperty>();
            // create the sequence for the BBIEs within the ABIE
            XmlSchemaSequence sequenceBBIEs = new XmlSchemaSequence();
            //attributes and associations are mixed in a user defined order
            foreach (var property in abie.Properties)
            {
                if (!processedProperties.Contains(property))
                    AddProperty(property, sequenceBBIEs, context, schema, processedProperties);
            }

            // add the sequence created to the complex type
            complexTypeBIE.Particle = sequenceBBIEs;
            return complexTypeBIE;
        }

        static void AddProperty(ICctsProperty property, XmlSchemaSequence propertiesSequence, GeneratorContext context, XmlSchema schema, List<ICctsProperty> processedProperties)
        {
            var elementProperty = CreatePropertyElement(property, context, schema);
            if (property.otherPropertiesInChoice.Any())
            {
                var choiceElement = new XmlSchemaChoice();
                choiceElement.Items.Add(elementProperty);
                bool isOptional = property.LowerBound == "0";
                foreach (var otherProperty in property.otherPropertiesInChoice)
                {
                    if (!isOptional) isOptional = otherProperty.LowerBound == "0";
                    choiceElement.Items.Add(CreatePropertyElement(otherProperty, context, schema));
                    processedProperties.Add(otherProperty);
                }
                //set minoccurs to 0 is the choice is optional
                if (isOptional)
                {
                    choiceElement.MinOccurs = 0;
                }
                //add the choice tot the sequence
                propertiesSequence.Items.Add(choiceElement);
            }
            else
            {
                // add the element created to the sequence
                propertiesSequence.Items.Add(elementProperty);
            }
            // add the property to the list of processed properties
            processedProperties.Add(property);
        }
        static XmlSchemaElement CreatePropertyElement(ICctsProperty property, GeneratorContext context, XmlSchema schema)
        {
            if (property is IBbie) return CreateBbieSchemaElement((IBbie)property, context);
            if (property is IAsbie) return CreateAsbieSchemaElement((IAsbie)property, context, schema);
            throw new ArgumentException("Invalid type of argument property. Expected IBbie or IAsbie");
        }
        static XmlSchemaElement CreateAsbieSchemaElement(IAsbie asbie, GeneratorContext context, XmlSchema schema)
        {
            XmlSchemaElement elementASBIE = new XmlSchemaElement();

            // R A08A: name of the ASBIE
            elementASBIE.Name = NDR.GetXsdElementNameFromAsbie(asbie);
            elementASBIE.SchemaTypeName =
                new XmlQualifiedName(context.NamespacePrefix + ":" + NDR.TrimElementName(asbie.AssociatedAbie.Name));

            if (asbie.AggregationKind == AggregationKind.Shared)
            {
                XmlSchemaElement refASBIE = new XmlSchemaElement();
                refASBIE.RefName = new XmlQualifiedName(context.NamespacePrefix + ":" + elementASBIE.Name);
                refASBIE.MinOccursString = AdjustBound(asbie.LowerBound);
                refASBIE.MaxOccursString = AdjustBound(asbie.UpperBound);
                // R 9241: for ASBIEs with AggregationKind = shared a global element must be declared.   
                schema.Items.Add(elementASBIE);
                return refASBIE;
            }
            else
            {
                //R 9025: ASBIEs with Aggregation Kind = composite a local element for the
                //        associated ABIE must be declared in the associating ABIE complex type.
                return elementASBIE;
            }

        }
        static XmlSchemaElement CreateBbieSchemaElement(IBbie bbie, GeneratorContext context)
        {
            // R 89A6: for every BBIE a named element must be locally declared
            XmlSchemaElement elementBBIE = new XmlSchemaElement();
            // R AEFE, R 96D9, R9A40, R A34A are implemented in GetXsdElementNameFromBbie(...)
            elementBBIE.Name = NDR.GetXsdElementNameFromBbie(bbie);
            // R 8B85: every BBIE type must be named the property term and qualifiers and the
            //         representation term of the basic business information entity (BBIE) it represents
            //         with the word 'Type' appended. 
            if (bbie.Bdt != null && bbie.Bdt.Con != null)
            {
                if (bbie.Bdt.Con.BasicType != null && bbie.Bdt.Con.BasicType.IsEnum)
                {
                    //figure out if the set of values is restricted
                    var basicEnum = bbie.Bdt.Con.BasicType.Enum as UpccEnum;
                    //use this method only if there are values specified in the basic enum. If not then we simply use the type
                    if (basicEnum != null && basicEnum.CodelistEntries.Any())
                    {
                        var sourceEnum = basicEnum.SourceElement as UpccEnum;
                        if (sourceEnum != null)
                        {
                            var restrictedtype = new XmlSchemaComplexType();
                            var sympleContent = new XmlSchemaSimpleContent();
                            var restriction = new XmlSchemaSimpleContentRestriction();
                            restriction.BaseTypeName = new XmlQualifiedName(context.NamespacePrefix + ":" + NDR.GetXsdTypeNameFromBdt(bbie.Bdt));
                            addEnumerationValues(restriction, basicEnum);
                            //add restriction to simplecontent
                            sympleContent.Content = restriction;
                            //add the restriction to the simple type
                            restrictedtype.ContentModel = sympleContent;
                            //set the type of the BBIE
                            elementBBIE.SchemaType = restrictedtype;
                        }
                    }
                }
                if (bbie.Bdt.isDirectXSDType)
                {
                    var XSDtype = bbie.Bdt.xsdType;
                    if (!string.IsNullOrEmpty(XSDtype))
                    {
                        if (bbie.Bdt.Con.AllFacets.Any())
                        {
                            //add facets
                            var restrictedtype = new XmlSchemaSimpleType();
                            var restriction = new XmlSchemaSimpleTypeRestriction();
                            restriction.BaseTypeName = new XmlQualifiedName(NSPREFIX_XSD + ":" + XSDtype);
                            NDR.addFacets(restriction, bbie.Bdt.Con.AllFacets);
                            //add the restriction to the simple type
                            restrictedtype.Content = restriction;
                            //set the type of the BBIE
                            elementBBIE.SchemaType = restrictedtype;
                        }
                        else
                        {
                            elementBBIE.SchemaTypeName = new XmlQualifiedName(NSPREFIX_XSD + ":" + XSDtype);
                        }
                    }
                }
                if (bbie.Facets.Any())
                {
                    //add facets
                    if (bbie.Bdt.Sups.Any())
                    {
                        //create a complex type
                        var complexType = new XmlSchemaComplexType();
                        //add the simple content extension
                        var simpleContent = new XmlSchemaSimpleContent();
                        var restriction = new XmlSchemaSimpleContentRestriction();
                        restriction.BaseTypeName = new XmlQualifiedName(context.NamespacePrefix + ":" + NDR.GetXsdTypeNameFromBdt(bbie.Bdt));
                        //add the facets
                        NDR.addFacets(restriction, bbie.Facets);
                        //add the simple content to the complex object
                        simpleContent.Content = restriction;
                        complexType.ContentModel = simpleContent;
                        //add the complex type to the element
                        elementBBIE.SchemaType = complexType;
                    }
                    else
                    {
                        //create a simple type
                        var restrictedtype = new XmlSchemaSimpleType();
                        var restriction = new XmlSchemaSimpleTypeRestriction();
                        restriction.BaseTypeName = new XmlQualifiedName(context.NamespacePrefix + ":" + NDR.GetXsdTypeNameFromBdt(bbie.Bdt));
                        NDR.addFacets(restriction, bbie.Facets);
                        //add the restriction to the simple type
                        restrictedtype.Content = restriction;
                        //set the type of the BBIE
                        elementBBIE.SchemaType = restrictedtype;
                    }
                }
                if (elementBBIE.SchemaType == null && elementBBIE.SchemaTypeName.IsEmpty)
                {
                    //use type without facets
                    elementBBIE.SchemaTypeName = new XmlQualifiedName(context.NamespacePrefix + ":" + NDR.GetXsdTypeNameFromBdt(bbie.Bdt));
                }
            }
            // R 90F9: cardinality of elements within the ABIE
            elementBBIE.MinOccursString = AdjustLowerBound(bbie.LowerBound);
            elementBBIE.MaxOccursString = AdjustUpperBound(bbie.UpperBound);

            return elementBBIE;
        }

        static void addEnumerationValues(XmlSchemaSimpleContentRestriction restriction, UpccEnum basicEnum)
        {
            foreach (var codeListEntry in basicEnum.CodelistEntries)
            {
                var xmlEnum = new XmlSchemaEnumerationFacet();
                xmlEnum.Value = codeListEntry.Name;
                restriction.Facets.Add(xmlEnum);
            }
        }


        private static string AdjustLowerBound(string lb)
        {
            return AdjustBound(lb);
        }

        private static string AdjustUpperBound(string ub)
        {
            return AdjustBound(ub);
        }

        private static string AdjustBound(string bound)
        {
            if (bound.Equals(""))
            {
                return "1";
            }

            if (bound.Equals("*"))
            {
                return "unbounded";
            }

            return bound;
        }

        public static string getObjectClassTerm(string name)
        {
            if (name.LastIndexOf('_') != -1)
            {
                return name.Substring(name.LastIndexOf('_') + 1);
            }
            return name;
        }

        public static string getObjectClassQualifier(string name)
        {
            if (name.LastIndexOf('_') != -1)
            {
                return name.Substring(0, name.LastIndexOf('_'));
            }
            return name;
        }

    }
}