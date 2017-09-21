using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using CctsRepository;
using CctsRepository.BdtLibrary;
using CctsRepository.BieLibrary;
using CctsRepository.DocLibrary;
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
        private const string NSPREFIX_BDT = "bdt";
        private const string NSPREFIX_TNS = "tns";
        private const string NSPREFIX_DOC = "ccts";

        private const string NS_DOC = "urn:un:unece:uncefact:documentation:standard:XMLNDRDocumentation:3";

        //private const string NSPREFIX_CCTS = "ccts";
        //private const string NS_CCTS = "urn:un:unece:uncefact:documentation:standard:CoreComponentsTechnicalSpecification:2";
        private const string NSPREFIX_XSD = "xsd";
        private const string NS_XSD = "http://www.w3.org/2001/XMLSchema";
        private static List<String> globalASBIEs;

        ///<summary>
        ///</summary>
        ///<param name="context"></param>
        ///<param name="abies"></param>
        public static void GenerateXSD(GeneratorContext context, IEnumerable<IAbie> abies)
        {
            // Create XML schema file and prepare the XML schema header
            // R 88E2: all XML schema files must use UTF-8 encoding
            // R B387: every XML schema must have a declared target namespace
            globalASBIEs = new List<String>();

            var schema = new XmlSchema {TargetNamespace = context.TargetNamespace};
            schema.Namespaces.Add(context.NamespacePrefix, context.TargetNamespace);
            schema.Version = context.DocLibrary.VersionIdentifier.DefaultTo("1");

            // TODO: discuss R A0E5 and R A9C5 with Christian E. and Michi S. since this is something that should be added to the context
            // R A0E5: all XML schemas must contain elementFormDefault and set it to qualified     
            schema.ElementFormDefault = XmlSchemaForm.Qualified;
            // R A9C5: All XML schemas must contain attributeFormDefault and set it to unqualified
            schema.AttributeFormDefault = XmlSchemaForm.Unqualified;

            // R 9B18: all XML schemas must utilize the xsd prefix when referring to the W3C XML schema namespace            
            schema.Namespaces.Add(NSPREFIX_XSD, NS_XSD);

            schema.Namespaces.Add(NSPREFIX_DOC, NS_DOC);

            // add namespace to be able to utilize BDTs
            schema.Namespaces.Add(NSPREFIX_BDT, context.TargetNamespace);
            // add namespace to be able to utilize ABIEs 
            schema.Namespaces.Add(NSPREFIX_TNS, context.TargetNamespace);

			string schemaFileName = getSchemaFileName(context,false);
            // R 8FE2: include BDT XML schema file
            AddIncludes(schema, context, context.DocLibrary, schemaFileName);

            foreach (IAbie abie in abies.OrderBy(a => a.Name))
            {
                // finally add the complex type to the schema
                schema.Items.Add(GenerateComplexTypeABIE(context, schema, abie));

                // R 9DA0: for each ABIE a named element must be globally declared
                // R 9A25: the name of the ABIE element must be the DictionaryEntryName with whitespace 
                //         and the 'Details' suffix removed
                // R B27B: every ABIE global element declaration must be of the complexType that represents
                //         the ABIE
                XmlSchemaElement elementBIE = new XmlSchemaElement();
                elementBIE.Name = abie.Name;
                elementBIE.SchemaTypeName = new XmlQualifiedName(NSPREFIX_TNS + ":" + abie.Name + "Type");
                schema.Items.Add(elementBIE);
            }

            context.AddSchema(schema, schemaFileName, Schematype.BIE);
        }
        private static string getSchemaFileName(GeneratorContext context, bool generic = false)
        {
        	var mainVersion = context.DocLibrary.VersionIdentifier.Split('.').FirstOrDefault();
        	var minorVersion = context.DocLibrary.VersionIdentifier.Split('.').LastOrDefault();
        	var docRootName =  context.DocLibrary.DocumentRoot.Name;
        	var bSlash = System.IO.Path.DirectorySeparatorChar;
        	var docOrGeneric = generic ? "generic" : "document" + bSlash
        											+ docRootName ;
        	//TODO set "Ebix" prefix via settings?
        	string filename = context.OutputDirectory + bSlash + mainVersion + bSlash + docOrGeneric + bSlash //directories
        					+ "ebIX_MessageBusinessInformationEntities_" + docRootName +"_"+ mainVersion + "p" + minorVersion + ".xsd"; //filename
        	return  filename;
        }
		private static void AddIncludes(XmlSchema schema, GeneratorContext context, IDocLibrary docLibrary, string schemaFileName)
        {
			foreach (var incSchemaInfo in context.Schemas.Where(x => x.Schematype == Schematype.BDT))
            {
                var include = new XmlSchemaInclude();
                include.SchemaLocation = NDR.GetRelativePath(schemaFileName, incSchemaInfo.FileName);
                schema.Includes.Add(include);
            }
        }
        internal static XmlSchemaComplexType GenerateComplexTypeABIE(GeneratorContext context, XmlSchema schema, IAbie abie)
        {
            return GenerateComplexTypeABIE(context, schema, abie, NSPREFIX_TNS);
        }

        internal static XmlSchemaComplexType GenerateComplexTypeABIE(GeneratorContext context, XmlSchema schema, IAbie abie, string abiePrefix)
        {
            // R A4CE, R AF95: a complex type must be defined for each ABIE   
            XmlSchemaComplexType complexTypeBIE = new XmlSchemaComplexType();

            // R 9D83: the name of the ABIE must be the DictionaryEntryName with all whitespace and separators 
            //         removed. The 'Details' suffix is replaced with 'Type'.
            complexTypeBIE.Name = abie.Name + "Type";

            if (context.Annotate)
            {
                complexTypeBIE.Annotation = GetABIEAnnotation(abie);
            }

            // create the sequence for the BBIEs within the ABIE
            XmlSchemaSequence sequenceBBIEs = new XmlSchemaSequence();

            foreach (IBbie bbie in abie.Bbies)
            {
                // R 89A6: for every BBIE a named element must be locally declared
                XmlSchemaElement elementBBIE = new XmlSchemaElement();

                // R AEFE, R 96D9, R9A40, R A34A are implemented in GetXsdElementNameFromBbie(...)
                elementBBIE.Name = NDR.GetXsdElementNameFromBbie(bbie);

                // R 8B85: every BBIE type must be named the property term and qualifiers and the
                //         representation term of the basic business information entity (BBIE) it represents
                //         with the word 'Type' appended. 
                elementBBIE.SchemaTypeName =
                    new XmlQualifiedName(NSPREFIX_BDT + ":" + NDR.GetXsdTypeNameFromBdt(bbie.Bdt));


                // R 90F9: cardinality of elements within the ABIE
                elementBBIE.MinOccursString = AdjustLowerBound(bbie.LowerBound);
                elementBBIE.MaxOccursString = AdjustUpperBound(bbie.UpperBound);

                if (context.Annotate)
                {
                    elementBBIE.Annotation = GetBBIEAnnotation(bbie);
                }

                // add the element created to the sequence
                sequenceBBIEs.Items.Add(elementBBIE);
            }


            foreach (IAsbie asbie in abie.Asbies.OrderBy(a => a.Name))
            {
                XmlSchemaElement elementASBIE = new XmlSchemaElement();

                // R A08A: name of the ASBIE
                elementASBIE.Name = NDR.GetXsdElementNameFromAsbie(asbie);
                elementASBIE.SchemaTypeName =
                    new XmlQualifiedName(abiePrefix + ":" + asbie.AssociatedAbie.Name + "Type");

                if (context.Annotate)
                {
                    elementASBIE.Annotation = GetASBIEAnnotiation(asbie);
                }

                if (asbie.AggregationKind == AggregationKind.Shared)
                {
                    XmlSchemaElement refASBIE = new XmlSchemaElement();
                    refASBIE.RefName = new XmlQualifiedName(abiePrefix + ":" + elementASBIE.Name);

                    // every shared ASCC may only appear once in the XSD file
                    if (!globalASBIEs.Contains(elementASBIE.Name))
                    {
                        // R 9241: for ASBIEs with AggregationKind = shared a global element must be declared.   
                        schema.Items.Add(elementASBIE);
                        globalASBIEs.Add(elementASBIE.Name);
                    }
                    sequenceBBIEs.Items.Add(refASBIE);
                }
                else
                {
                    //R 9025: ASBIEs with Aggregation Kind = composite a local element for the
                    //        associated ABIE must be declared in the associating ABIE complex type.
                    sequenceBBIEs.Items.Add(elementASBIE);
                }
            }

            // add the sequence created to the complex type
            complexTypeBIE.Particle = sequenceBBIEs;
            return complexTypeBIE;
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
            annotation.Items.Add(new XmlSchemaDocumentation {Language = "en", Markup = documentation.ToArray()});

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
            annotation.Items.Add(new XmlSchemaDocumentation {Language = "en", Markup = documentation.ToArray()});

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
            annotation.Items.Add(new XmlSchemaDocumentation {Language = "en", Markup = documentation.ToArray()});

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