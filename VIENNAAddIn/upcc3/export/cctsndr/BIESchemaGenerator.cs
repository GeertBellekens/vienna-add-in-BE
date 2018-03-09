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
        private const string NSPREFIX_NS1 = "ns1";
        private const string NSPREFIX_BIE = "bie";
        private const string NSPREFIX_DOC = "ccts";
        private const string NSPREFIX_XBT = "xbt";
        private const string NSPREFIX_XSD = "xsd";

        private const string NS_DOC = "urn:un:unece:uncefact:documentation:common:3:standard:CoreComponentsTechnicalSpecification:3";
        private const string NS_XBT = "urn:un:unece:uncefact:data:common:1:draft";
        private const string NS_XSD = "http://www.w3.org/2001/XMLSchema";

        private static List<String> globalASBIEs;

        public static void GenerateXSD(GeneratorContext context, GeneratorContext genericContext, IEnumerable<IAbie> abies)
        {
            genericContext.AddElements(abies);
            context.AddElements(abies);
            GenerateXSD(context);
        }

        public static void GenerateXSD(GeneratorContext context)
        {
            // Create XML schema file and prepare the XML schema header
            // R 88E2: all XML schema files must use UTF-8 encoding
            // R B387: every XML schema must have a declared target namespace
            globalASBIEs = new List<String>();

            var schema = new XmlSchema { TargetNamespace = context.TargetNamespace };
            schema.Namespaces.Add(NSPREFIX_BIE, context.TargetNamespace);
            schema.Version = context.VersionID.DefaultTo("1");

            // TODO: discuss R A0E5 and R A9C5 with Christian E. and Michi S. since this is something that should be added to the context
            // R A0E5: all XML schemas must contain elementFormDefault and set it to qualified     
            schema.ElementFormDefault = XmlSchemaForm.Qualified;
            // R A9C5: All XML schemas must contain attributeFormDefault and set it to unqualified
            schema.AttributeFormDefault = XmlSchemaForm.Unqualified;

            // R 9B18: all XML schemas must utilize the xsd prefix when referring to the W3C XML schema namespace            
            schema.Namespaces.Add(NSPREFIX_XSD, NS_XSD);
            //add namespace for documentation
            schema.Namespaces.Add(NSPREFIX_DOC, NS_DOC);
            // add namespace ns1
            schema.Namespaces.Add(NSPREFIX_NS1, context.BaseURN);
            //add namespace xbt
            schema.Namespaces.Add(NSPREFIX_XBT, NS_XBT);


            string schemaFileName = getSchemaFileName(context);
            // R 8FE2: include BDT XML schema file
            AddIncludes(schema, context, schemaFileName);

            foreach (IAbie abie in context.Elements.OfType<IAbie>().OrderBy(a => a.Name))
            {
                // finally add the complex type to the schema
                schema.Items.Add(GenerateComplexTypeABIE(context, schema, abie));

                // R 9DA0: for each ABIE a named element must be globally declared
                // R 9A25: the name of the ABIE element must be the DictionaryEntryName with whitespace 
                //         and the 'Details' suffix removed
                // R B27B: every ABIE global element declaration must be of the complexType that represents
                //         the ABIE
                // Global element definitions are apparently not required
                //                XmlSchemaElement elementBIE = new XmlSchemaElement();
                //                elementBIE.Name = abie.Name;
                //                elementBIE.SchemaTypeName = new XmlQualifiedName(NSPREFIX_BIE + ":" + NDR.TrimElementName(abie.Name) + "Type");
                //                schema.Items.Add(elementBIE);
            }

            context.AddSchema(schema, schemaFileName, UpccSchematype.BIE);
        }
        private static string getSchemaFileName(GeneratorContext context)
        {
            var mainVersion = context.VersionID.Split('.').FirstOrDefault();
            var minorVersion = context.VersionID.Split('.').LastOrDefault();
            var docRootName = context.DocRootName;
            var bSlash = System.IO.Path.DirectorySeparatorChar;
            var docOrGeneric = context.isGeneric ? "generic" : "document" + bSlash
                                                    + docRootName;
            //TODO set "ebIX" prefix via settings?
            string filename = context.OutputDirectory + bSlash + docOrGeneric + bSlash //directories
                + "ebIX_MessageBusinessInformationEntities_"; // filename
            if (!context.isGeneric) filename += docRootName + "_";
            filename += mainVersion + "p" + minorVersion + ".xsd";
            return filename;
        }
        private static void AddIncludes(XmlSchema schema, GeneratorContext context, string schemaFileName)
        {
            foreach (var incSchemaInfo in context.Schemas.Where(x => x.Schematype == UpccSchematype.BDT))
            {
                var include = new XmlSchemaInclude();
                include.SchemaLocation = NDR.GetRelativePath(schemaFileName, incSchemaInfo.FileName);
                schema.Includes.Add(include);
            }
        }

        internal static XmlSchemaComplexType GenerateComplexTypeABIE(GeneratorContext context, XmlSchema schema, IAbie abie)
        {
            // R A4CE, R AF95: a complex type must be defined for each ABIE   
            XmlSchemaComplexType complexTypeBIE = new XmlSchemaComplexType();

            // R 9D83: the name of the ABIE must be the DictionaryEntryName with all whitespace and separators 
            //         removed. The 'Details' suffix is replaced with 'Type'.
            complexTypeBIE.Name = NDR.TrimElementName(abie.Name);

            if (context.Annotate)
            {
                complexTypeBIE.Annotation = GetABIEAnnotation(abie);
            }
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
                new XmlQualifiedName(NSPREFIX_BIE + ":" + NDR.TrimElementName(asbie.AssociatedAbie.Name));

            if (context.Annotate)
            {
                elementASBIE.Annotation = GetASBIEAnnotiation(asbie);
            }

            if (asbie.AggregationKind == AggregationKind.Shared)
            {
                XmlSchemaElement refASBIE = new XmlSchemaElement();
                refASBIE.RefName = new XmlQualifiedName(NSPREFIX_BIE + ":" + elementASBIE.Name);
                refASBIE.MinOccursString = AdjustBound(asbie.LowerBound);
                refASBIE.MaxOccursString = AdjustBound(asbie.UpperBound);

                // every shared ASCC may only appear once in the XSD file
                if (!globalASBIEs.Contains(elementASBIE.Name))
                {
                    // R 9241: for ASBIEs with AggregationKind = shared a global element must be declared.   
                    schema.Items.Add(elementASBIE);
                    globalASBIEs.Add(elementASBIE.Name);
                }
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
                            restriction.BaseTypeName = new XmlQualifiedName(NSPREFIX_BIE + ":" + NDR.GetXsdTypeNameFromBdt(bbie.Bdt));
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
                        restriction.BaseTypeName = new XmlQualifiedName(NSPREFIX_BIE + ":" + NDR.GetXsdTypeNameFromBdt(bbie.Bdt));
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
                        restriction.BaseTypeName = new XmlQualifiedName(NSPREFIX_BIE + ":" + NDR.GetXsdTypeNameFromBdt(bbie.Bdt));
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
                    elementBBIE.SchemaTypeName = new XmlQualifiedName(NSPREFIX_BIE + ":" + NDR.GetXsdTypeNameFromBdt(bbie.Bdt));
                }
            }
            // R 90F9: cardinality of elements within the ABIE
            elementBBIE.MinOccursString = AdjustLowerBound(bbie.LowerBound);
            elementBBIE.MaxOccursString = AdjustUpperBound(bbie.UpperBound);
            if (context.Annotate)
            {
                elementBBIE.Annotation = GetBBIEAnnotation(bbie);
            }
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


        
        ///<summary>
        ///</summary>
        ///<param name="bbie"></param>
        ///<returns></returns>
        public static XmlSchemaAnnotation GetBBIEAnnotation(IBbie bbie)
        {
            // Contains all the documentation items such as DictionaryEntryName
            IList<XmlNode> documentation = new List<XmlNode>();

            AddDocumentation(documentation, "UniqueID", bbie.UniqueIdentifier);
            AddDocumentation(documentation, "VersionID", bbie.VersionIdentifier);
            AddDocumentation(documentation, "Cardinality", bbie.LowerBound + ".." + bbie.UpperBound);
            AddDocumentation(documentation, "SequencingKey", bbie.SequencingKey);
            AddDocumentation(documentation, "DictionaryEntryName", bbie.DictionaryEntryName);
            AddDocumentation(documentation, "Definition", bbie.Definition);
            AddDocumentation(documentation, "BusinessTermName", bbie.BusinessTerms);
            AddDocumentation(documentation, "PropertyTermName", bbie.Name);
            AddDocumentation(documentation, "RepresentationTermName", bbie.Bdt.Name);
            AddDocumentation(documentation, "AcronymCode", "BBIE");

            XmlSchemaAnnotation annotation = new XmlSchemaAnnotation();
            annotation.Items.Add(new XmlSchemaDocumentation { Language = "en", Markup = documentation.ToArray() });

            return annotation;
        }

        ///<summary>
        ///</summary>
        ///<param name="abie"></param>
        public static XmlSchemaAnnotation GetABIEAnnotation(IAbie abie)
        {
            // Contains all the documentation items such as DictionaryEntryName
            IList<XmlNode> documentation = new List<XmlNode>();

            AddDocumentation(documentation, "UniqueID", abie.UniqueIdentifier);
            AddDocumentation(documentation, "VersionID", abie.VersionIdentifier);
            AddDocumentation(documentation, "ObjectClassQualifierName", getObjectClassQualifier(abie.Name));
            AddDocumentation(documentation, "ObjectClassTermName", getObjectClassTerm(abie.Name));
            AddDocumentation(documentation, "DictionaryEntryName", abie.DictionaryEntryName);
            AddDocumentation(documentation, "Definition", abie.Definition);
            AddDocumentation(documentation, "BusinessTermName", abie.BusinessTerms);
            AddDocumentation(documentation, "AcronymCode", "ABIE");

            XmlSchemaAnnotation annotation = new XmlSchemaAnnotation();
            annotation.Items.Add(new XmlSchemaDocumentation { Language = "en", Markup = documentation.ToArray() });

            return annotation;
        }

        ///<summary>
        ///</summary>
        ///<param name="ma"></param>
        public static XmlSchemaAnnotation GetMaAnnotation(IMa ma)
        {
            //throw new NotImplementedException();
            return null;
        }

        ///<summary>
        ///</summary>
        ///<param name="asbie"></param>
        ///<returns></returns>
        public static XmlSchemaAnnotation GetASBIEAnnotiation(IAsbie asbie)
        {
            // Contains all the documentation items such as DictionaryEntryName
            IList<XmlNode> documentation = new List<XmlNode>();

            AddDocumentation(documentation, "UniqueID", asbie.UniqueIdentifier);
            AddDocumentation(documentation, "VersionID", asbie.VersionIdentifier);
            AddDocumentation(documentation, "Cardinality", asbie.LowerBound + ".." + asbie.UpperBound);
            AddDocumentation(documentation, "SequencingKey", asbie.SequencingKey);
            AddDocumentation(documentation, "DictionaryEntryName", asbie.DictionaryEntryName);
            AddDocumentation(documentation, "Definition", asbie.Definition);
            AddDocumentation(documentation, "BusinessTermName", asbie.BusinessTerms);
            AddDocumentation(documentation, "AssociationType", asbie.AggregationKind.ToString());
            AddDocumentation(documentation, "PropertyTermName", asbie.Name);
            // PropertyQualifierName could be extracted from the PropertyTermName (e.g. "My" in 
            // "My_Address") but is not implement at this point 
            AddDocumentation(documentation, "PropertyQualifierName", "");
            AddDocumentation(documentation, "AssociatedObjectClassTermName", asbie.AssociatedAbie.Name);
            // AssociatedObjectClassQualifierTermName could be extracted from the AssociatedObjectClassTermName
            // (e.g. "My" in "My_Address") but is not implement at this point 
            AddDocumentation(documentation, "AcronymCode", "ASBIE");


            XmlSchemaAnnotation annotation = new XmlSchemaAnnotation();
            annotation.Items.Add(new XmlSchemaDocumentation { Language = "en", Markup = documentation.ToArray() });

            return annotation;
        }

        public static void AddDocumentation(IList<XmlNode> doc, string name, string value)
        {
            // Dummy XML Document needed to create various XML schema elements (e.g. text nodes)
            XmlDocument dummyDoc = new XmlDocument();

            XmlElement documentationElement = dummyDoc.CreateElement(NSPREFIX_DOC, name, NS_DOC);
            documentationElement.InnerText = value;
            doc.Add(documentationElement);
        }

        public static void AddDocumentation(IList<XmlNode> doc, string name, IEnumerable<string> values)
        {
            int countBusinessTerms = 0;

            foreach (string value in values)
            {
                AddDocumentation(doc, name, value);
                countBusinessTerms++;
            }

            if (countBusinessTerms < 1)
            {
                AddDocumentation(doc, name, "");
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