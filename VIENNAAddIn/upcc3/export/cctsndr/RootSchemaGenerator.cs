using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using CctsRepository.BieLibrary;
using CctsRepository.DocLibrary;
using VIENNAAddInUtils;

// XML Naming and Design Rules that are currently not considered:
//
// R 8A68, R B0AD, R 942D, R A8A6 Currently there is no codelist generated, and thus not inclcuded within the BDT schema files
// R AB90; R A154, R BD2F, R AFEB Business Identifier Schemas are currently not supported by this generator
// R 84BE, R 9049, R A735, R AFA8, R R BBD5, R 998B Only simple versioning is currently supported by this generator. The suggested template is not supported yet.
// R ABD2, R BD41 The XMLSchema API of .NET does not support comments. Comments are not treated at the moment.

namespace VIENNAAddIn.upcc3.export.cctsndr
{
    ///<summary>
    ///</summary>
    public class RootSchemaGenerator
    {
        private static List<string> globalAsmas;
        private static string NSPREFIX_BDT = "bdt";
        private static string NSPREFIX_BIE = "bie";

        ///<summary>
        ///</summary>
        ///<param name="context"></param>
        public static void GenerateXSD(GeneratorContext context)
        {
            globalAsmas = new List<string>();
            IMa documentRoot = context.DocLibrary.DocumentRoot;
            var schema = new XmlSchema
                             {
                                 TargetNamespace = context.TargetNamespace
                             };
            schema.Namespaces.Add(context.NamespacePrefix, context.TargetNamespace);
            schema.Namespaces.Add("xsd", "http://www.w3.org/2001/XMLSchema");
            schema.Namespaces.Add("ccts",
                                  "urn:un:unece:uncefact:documentation:standard:XMLNDRDocumentation:3");
            schema.Namespaces.Add(NSPREFIX_BDT, context.TargetNamespace);
            schema.Namespaces.Add(NSPREFIX_BIE, context.TargetNamespace);

            schema.ElementFormDefault = XmlSchemaForm.Qualified;
            schema.AttributeFormDefault = XmlSchemaForm.Unqualified;
            schema.Version = context.DocLibrary.VersionIdentifier.DefaultTo("1");

            //TODO how do i now what schemas to include and what to import
            AddImports(schema, context, context.DocLibrary);
            AddIncludes(schema, context, context.DocLibrary);

            AddRootElementDeclaration(schema, documentRoot, context);
            AddRootTypeDefinition(schema, documentRoot, context, context.DocLibrary);

            IEnumerable<IMa> nonRootDocLibraryElements = context.DocLibrary.NonRootMas;
            AddGlobalElementDeclarations(schema, nonRootDocLibraryElements, context);
            AddGlobalTypeDefinitions(schema, nonRootDocLibraryElements, context);
            context.AddSchema(schema, documentRoot.Name + "_" + schema.Version + ".xsd");
        }

        private static void AddGlobalElementDeclarations(XmlSchema schema, IEnumerable<IMa> mas,
                                                         GeneratorContext context)
        {
            foreach (IMa ma in mas)
            {
                var element = new XmlSchemaElement
                                  {
                                      Name = ma.Name,
                                      SchemaTypeName =
                                          new XmlQualifiedName(context.NamespacePrefix + ":" + ma.Name +
                                                               "Type")
                                  };
                //if (context.Annotate)
                //    element.Annotation = GetMaAnnotation(ma);
                schema.Items.Add(element);
            }
        }

        private static void AddGlobalTypeDefinitions(XmlSchema schema, IEnumerable<IMa> mas, GeneratorContext context)
        {
            foreach (IMa ma in mas)
            {
                schema.Items.Add(GenerateComplexTypeForMa(context, schema, ma,
                                                                             context.NamespacePrefix));
            }
        }

        private static XmlSchemaComplexType GenerateComplexTypeForMa(GeneratorContext context, XmlSchema schema, IMa ma, string abiePrefix)
        {
            var maType = new XmlSchemaComplexType();
            maType.Name = ma.Name + "Type";

            var sequence = new XmlSchemaSequence();
            foreach (IAsma asma in ma.Asmas.OrderBy(a => a.Name))
            {
                XmlSchemaElement elementAsma = null;

                // Take care of ASMAS aggregating ABIEs
                if (asma.AssociatedBieAggregator.IsAbie)
                {
                    elementAsma = new XmlSchemaElement
                    {
                        Name = NDR.GetXsdElementNameFromAsma(asma),
                        SchemaTypeName =
                            new XmlQualifiedName(NSPREFIX_BIE + ":" +
                                                 asma.AssociatedBieAggregator.Name +
                                                 "Type")
                    };
                }
                else if (asma.AssociatedBieAggregator.IsMa)
                {
                    elementAsma = new XmlSchemaElement
                    {
                        Name = NDR.GetXsdElementNameFromAsma(asma),
                        SchemaTypeName =
                            new XmlQualifiedName(context.NamespacePrefix + ":" +
                                                 asma.AssociatedBieAggregator.Name +
                                                 "Type")
                    };
                }

                var refAsma = new XmlSchemaElement
                {
                    RefName =
                        new XmlQualifiedName(context.NamespacePrefix + ":" +
                                             NDR.GetXsdElementNameFromAsma(asma))
                };

                // every shared ASCC may only appear once in the XSD file
                if (!globalAsmas.Contains(elementAsma.Name))
                {
                    // R 9241: for ASBIEs with AggregationKind = shared a global element must be declared.   
                    schema.Items.Add(elementAsma);
                    globalAsmas.Add(elementAsma.Name);
                }

                sequence.Items.Add(refAsma);
            }
            maType.Particle = sequence;
            if (context.Annotate)
                maType.Annotation = GetMaAnnotation(ma);

            return maType;
        }

        private static void AddRootElementDeclaration(XmlSchema schema, IMa ma, GeneratorContext context)
        {
            var root = new XmlSchemaElement
                           {
                               Name = ma.Name,
                               SchemaTypeName =
                                   new XmlQualifiedName(context.NamespacePrefix + ":" + ma.Name + "Type")
                           };
            //if (context.Annotate)
            //    root.Annotation = GetMaAnnotation(ma);
            schema.Items.Add(root);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="schema"></param>
        /// <param name="ma"></param>
        /// <param name="context"></param>
        /// <param name="docLibrary"></param>
        private static void AddRootTypeDefinition(XmlSchema schema, IMa ma, GeneratorContext context,
                                                  IDocLibrary docLibrary)
        {
            //XmlSchemaComplexType type = BIESchemaGenerator.GenerateComplexTypeABIE(context, schema, ma, NSPREFIX_BIE);

            var rootType = new XmlSchemaComplexType();
            rootType.Name = ma.Name + "Type";

            var sequence = new XmlSchemaSequence();
            

            foreach (IAsma asma in ma.Asmas.OrderBy(a => a.Name))
            {
                XmlSchemaElement elementAsma = null;

                // Take care of ASMAS aggregating ABIEs
                if (asma.AssociatedBieAggregator.IsAbie)
                {
                    elementAsma = new XmlSchemaElement
                                      {
                                          Name = NDR.GetXsdElementNameFromAsma(asma),
                                          SchemaTypeName =
                                              new XmlQualifiedName(NSPREFIX_BIE + ":" +
                                                                   asma.AssociatedBieAggregator.Name +
                                                                   "Type")
                                      };
                }
                else if(asma.AssociatedBieAggregator.IsMa)
                {
                    elementAsma = new XmlSchemaElement
                                      {
                                          Name = NDR.GetXsdElementNameFromAsma(asma),
                                          SchemaTypeName =
                                              new XmlQualifiedName(context.NamespacePrefix + ":" +
                                                                   asma.AssociatedBieAggregator.Name +
                                                                   "Type")
                                      };
                }

                var refAsma = new XmlSchemaElement
                                  {
                                      RefName =
                                          new XmlQualifiedName(context.NamespacePrefix + ":" +
                                                               NDR.GetXsdElementNameFromAsma(asma))
                                  };

                // every shared ASCC may only appear once in the XSD file
                if (!globalAsmas.Contains(elementAsma.Name))
                {
                    // R 9241: for ASBIEs with AggregationKind = shared a global element must be declared.   
                    schema.Items.Add(elementAsma);
                    globalAsmas.Add(elementAsma.Name);
                }

                sequence.Items.Add(refAsma);
            }

            rootType.Particle = sequence;
            if (context.Annotate)
                rootType.Annotation = GetMaAnnotation(ma);

            schema.Items.Add(rootType);
     
        }


        private static void UpdateElementTypePrefix(XmlSchema schema, GeneratorContext context, String name)
        {
            String temp = "";
            XmlSchemaElement element;
            foreach (XmlSchemaObject item in schema.Items)
            {
                if (item is XmlSchemaElement)
                {
                    element = item as XmlSchemaElement;
                    if (name.Contains(element.Name))
                    {
                        temp = element.SchemaTypeName.ToString();
                        temp = temp.Replace(NSPREFIX_BIE, context.NamespacePrefix);
                        element.SchemaTypeName = new XmlQualifiedName(temp);
                        return;
                    }
                }
            }
        }

       private static void AddImports(XmlSchema schema, GeneratorContext context, IDocLibrary docLibrary)
        {
            if (context.Annotate)
            {
                var import = new XmlSchemaImport
                                 {
                                     Namespace = "urn:un:unece:uncefact:documentation:standard:XMLNDRDocumentation:3",
                                     SchemaLocation = "documentation/standard/XMLNDR_Documentation_3p0.xsd"
                                 };

                schema.Includes.Add(import);
            }
        }

        private static void AddIncludes(XmlSchema schema, GeneratorContext context, IDocLibrary docLibrary)
        {
            foreach (SchemaInfo si in context.Schemas)
            {
                //XmlTextReader textReader = new XmlTextReader(context.OutputDirectory + si.FileName);
                //XmlSchema includeSchema = XmlSchema.Read(textReader, null);
                var include = new XmlSchemaInclude();
                //include.Schema = includeSchema;
                include.SchemaLocation = si.FileName;
                schema.Includes.Add(include);
            }
        }

        private static XmlSchemaAnnotation GetMaAnnotation(IMa ma)
        {

            // Contains all the documentation items such as DictionaryEntryName
            IList<XmlNode> documentation = new List<XmlNode>();

            BIESchemaGenerator.AddDocumentation(documentation, "ObjectClassTermName", ma.Name);
            BIESchemaGenerator.AddDocumentation(documentation, "AcronymCode", "MA");
 
            var annotation = new XmlSchemaAnnotation();
            annotation.Items.Add(new XmlSchemaDocumentation { Language = "en", Markup = documentation.ToArray() });

            return annotation;
        }
    }
}