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


        public static XmlSchema GenerateXSD(GeneratorContext context)
        {
            //set bdt and bie prefixes
            NSPREFIX_BDT = context.NamespacePrefix;
            NSPREFIX_BIE = context.NamespacePrefix;

            string targetNameSpace = NDR.getTargetNameSpace(context, false);
            globalAsmas = new List<string>();
            IMa documentRoot = context.DocLibrary.DocumentRoot;
            var schema = new XmlSchema
            {
                TargetNamespace = targetNameSpace
            };
            //namespaces
            schema.Namespaces.Add("xsd", "http://www.w3.org/2001/XMLSchema");
            schema.Namespaces.Add(context.NamespacePrefix, targetNameSpace);
            //TODO make additional namespaces variable via setting
            schema.Namespaces.Add("ccts",
                                  "urn:un:unece:uncefact:documentation:common:3:standard:CoreComponentsTechnicalSpecification:3");
            schema.Namespaces.Add("xbt", "urn:un:unece:uncefact:data:common:1:draft");
            //qualifiedSetting
            schema.ElementFormDefault = XmlSchemaForm.Qualified;
            schema.AttributeFormDefault = XmlSchemaForm.Unqualified;
            //version
            schema.Version = context.DocLibrary.VersionIdentifier.DefaultTo("1");

            string schemaFileName = getSchemaFileName(context, false);

            AddRootElementDeclaration(schema, documentRoot, context);
            GenerateComplexTypeForMa(context, schema, documentRoot);

            //non root elements, not used
            IEnumerable<IMa> nonRootDocLibraryElements = context.DocLibrary.NonRootMas;
            AddGlobalTypeDefinitions(schema, nonRootDocLibraryElements, context);
            AddGlobalElementDeclarations(schema, nonRootDocLibraryElements, context);

            context.AddSchema(schema, schemaFileName, UpccSchematype.ROOT);
            return schema;
        }
        private static string getSchemaFileName(GeneratorContext context, bool generic = false)
        {
            var mainVersion = context.DocLibrary.VersionIdentifier.Split('.').FirstOrDefault();
            var minorVersion = context.DocLibrary.VersionIdentifier.Split('.').LastOrDefault();
            var docRootName = context.DocLibrary.DocumentRoot.Name;
            var bSlash = System.IO.Path.DirectorySeparatorChar;
            //TODO set "ebIX" prefix via settings?
            string filename = context.OutputDirectory + bSlash
                            + "ebIX_" + docRootName + "_" + mainVersion + "p" + minorVersion + ".xsd"; //filename
            return filename;
        }

        private static void AddGlobalElementDeclarations(XmlSchema schema, IEnumerable<IMa> mas,GeneratorContext context)
        {
            foreach (IMa ma in mas)
            {
                var element = new XmlSchemaElement
                {
                    Name = ma.Name,
                    SchemaTypeName =
                                          new XmlQualifiedName(context.NamespacePrefix + ":" + ma.Name)
                };
                schema.Items.Add(element);
            }
        }

        private static void AddGlobalTypeDefinitions(XmlSchema schema, IEnumerable<IMa> mas, GeneratorContext context)
        {
            foreach (IMa ma in mas)
            {
                GenerateComplexTypeForMa(context, schema, ma);
            }
        }

        private static void GenerateComplexTypeForMa(GeneratorContext context, XmlSchema schema, IMa ma)
        {
            var maType = new XmlSchemaComplexType();
            maType.Name = ma.Name + "Type";

            var sequence = new XmlSchemaSequence();
            foreach (IAsma asma in ma.Asmas)
            {
                var rasmaElement = new XmlSchemaElement();
                rasmaElement.Name =  NDR.GetXsdElementNameFromAsma(asma);
                rasmaElement.SchemaTypeName = new XmlQualifiedName(context.NamespacePrefix + ":" +
                                                    NDR.TrimElementName(asma.AssociatedBieAggregator.Name));
                // R 90F9: cardinality of elements within the ABIE
                rasmaElement.MinOccursString = AdjustBound(asma.LowerBound);
                rasmaElement.MaxOccursString = AdjustBound(asma.UpperBound);
                //add to the sequence
                sequence.Items.Add(rasmaElement);
            }
            //set the sequence as particle of the ma complex type
            maType.Particle = sequence;
            //add the maType to the schema
            schema.Items.Add(maType);
        }

        private static void AddRootElementDeclaration(XmlSchema schema, IMa ma, GeneratorContext context)
        {
            var root = new XmlSchemaElement
            {
                Name = ma.Name,
                SchemaTypeName =
                                   new XmlQualifiedName(context.NamespacePrefix + ":" + ma.Name + "Type")
            };
            schema.Items.Add(root);
        }

        private static void UpdateElementTypePrefix(XmlSchema schema, GeneratorContext context, String name)
        {
            String temp = "";
            XmlSchemaElement element;
            foreach (XmlSchemaObject item in schema.Items)
            {
                element = item as XmlSchemaElement;
                if (element != null)
                {
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
            //            if (context.Annotate)
            //            {
            //                var import = new XmlSchemaImport
            //                                 {
            //                                     Namespace = "urn:un:unece:uncefact:documentation:standard:XMLNDRDocumentation:3",
            //                                     SchemaLocation = "documentation/standard/XMLNDR_Documentation_3p0.xsd"
            //                                 };
            //
            //                schema.Includes.Add(import);
            //            }
        }

        private static void AddIncludes(XmlSchema schema, GeneratorContext context, IDocLibrary docLibrary, string schemaFileName, bool generic)
        {
            foreach (SchemaInfo si in context.Schemas.Where(x => x.Schematype == UpccSchematype.BIE
                                                           || x.Schematype == UpccSchematype.BDT))
            {
                var include = new XmlSchemaInclude();
                include.SchemaLocation = generic ?
                    System.IO.Path.GetFileName(si.FileName).Replace(context.DocRootName + "_", string.Empty) :
                                NDR.GetRelativePath(schemaFileName, si.FileName);
                schema.Includes.Add(include);
            }
        }



        private static string AdjustBound(string bound)
        {
            if (string.IsNullOrEmpty(bound))
            {
                return "1";
            }

            if (bound.Equals("*"))
            {
                return "unbounded";
            }

            return bound;
        }

    }
}