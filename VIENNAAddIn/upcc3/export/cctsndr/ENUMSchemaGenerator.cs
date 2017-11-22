// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using CctsRepository;
using CctsRepository.EnumLibrary;
using VIENNAAddIn.upcc3.repo;
using VIENNAAddIn.upcc3.repo.EnumLibrary;
using VIENNAAddInUtils;
using VIENNAAddIn.upcc3.export.cctsndr;

namespace VIENNAAddIn.upcc3.export.cctsndr
{
    public class ENUMSchemaGenerator
    {
    	private const string NSPREFIX_NS1 = "ns1";
        private const string NSPREFIX_BDT = "bdt";
        private const string NSPREFIX_DOC = "ccts";
        private const string NSPREFIX_XBT = "xbt";
		private const string NSPREFIX_XSD = "xsd";
		
        private const string NS_DOC = "urn:un:unece:uncefact:documentation:common:3:standard:CoreComponentsTechnicalSpecification:3";
        private const string NS_XBT = "urn:un:unece:uncefact:data:common:1:draft"; 
        private const string NS_XSD = "http://www.w3.org/2001/XMLSchema";
        
        public static SchemaInfo GenerateXSD(GeneratorContext context, IEnum enumeration)
        {
			var schema = new XmlSchema {TargetNamespace = enumeration.EnumLibrary.BaseURN};
            //add namespaces
            schema.Namespaces.Add(NDR.getEnumPrefixname(enumeration), enumeration.EnumLibrary.BaseURN);
            // R 9B18: all XML schemas must utilize the xsd prefix when referring to the W3C XML schema namespace            
            schema.Namespaces.Add(NSPREFIX_XSD, NS_XSD);
			//add namespace for documentation
            schema.Namespaces.Add(NSPREFIX_DOC, NS_DOC);
            //add namespace xbt
            schema.Namespaces.Add(NSPREFIX_XBT, NS_XBT);
            // R A0E5: all XML schemas must contain elementFormDefault and set it to qualified     
            schema.ElementFormDefault = XmlSchemaForm.Qualified;
            // R A9C5: All XML schemas must contain attributeFormDefault and set it to unqualified
            schema.AttributeFormDefault = XmlSchemaForm.Unqualified;
            //add version
            schema.Version = enumeration.VersionIdentifier.DefaultTo("1");
            //get the schemaFileName
            string schemaFileName = getSchemaFileName(context,enumeration);
            //add includes for enumeration on which this enumeration depends.
            addDependentEnumSchemas(schema, schemaFileName, context, enumeration);
            //create root element
            AddRootElementDeclaration(schema,context, enumeration);
            //add the simpletype definition
            AddSimpleTypeDefinition(schema,context, enumeration);
            //return the schema info
            return context.AddSchema(schema, schemaFileName,UpccSchematype.ENUM);
        }
        //add the root element for the enumeration
        private static void AddRootElementDeclaration(XmlSchema schema, GeneratorContext context, IEnum enumeration)
        {
        	var root = new XmlSchemaElement();
        	root.Name = NDR.GetEnumName(enumeration);
            root.SchemaTypeName =  new XmlQualifiedName(NDR.getEnumPrefixname(enumeration) + ":" + NDR.GetBasicTypeName(enumeration));
            schema.Items.Add(root);
        }
        private static void AddSimpleTypeDefinition(XmlSchema schema, GeneratorContext context, IEnum enumeration)
        {
            var restrictedtype = new XmlSchemaSimpleType();
            restrictedtype.Name = NDR.GetEnumName(enumeration);
            // enumeration with only union
            switch (enumeration.EnumerationType) 
            {
            	case EnumerationType.Assembled:
            		//enum with union
            		AddUnion(restrictedtype, enumeration);
            		break;
            	default:
            		// enumeration with actual values
		        	addEnumRestriction(restrictedtype, enumeration);
		        	break;
            }
            //add the restrictedType to the schema
            schema.Items.Add(restrictedtype);
        }
        
        static void addEnumRestriction(XmlSchemaSimpleType restrictedtype, IEnum basicEnum )
		{
        	var restriction = new XmlSchemaSimpleTypeRestriction();
		    restriction.BaseTypeName = new XmlQualifiedName(NSPREFIX_XSD + ":token");
			foreach (var codeListEntry in basicEnum.CodelistEntries) 
			{
				var xmlEnum = new XmlSchemaEnumerationFacet();
				xmlEnum.Value = codeListEntry.Name;
				//add annoation
				xmlEnum.Annotation = GetCodeListentryAnnotation(codeListEntry);
				restriction.Facets.Add(xmlEnum);
			}
			//add the restriction to the simple type
		    restrictedtype.Content = restriction;
		}
        private static XmlSchemaAnnotation GetCodeListentryAnnotation(ICodelistEntry codeListEntry)
        {
            var xml = new XmlDocument();
            // Deviation from rule [R 9C95]: Generating only a subset of the defined annotations and added some additional annotations.
            var annNodes = new List<XmlNode>();
            SchemaGeneratorUtils.AddAnnotation(xml, annNodes, "Name", codeListEntry.Name);
            SchemaGeneratorUtils.AddAnnotation(xml, annNodes, "Description", codeListEntry.CodeName);
            var ann = new XmlSchemaAnnotation();
            ann.Items.Add(new XmlSchemaDocumentation {Language = "en", Markup = annNodes.ToArray()});
            return ann;
        }

		static void AddUnion(XmlSchemaSimpleType restrictedtype, IEnum enumeration)
		{
			//add union
			var unionElement = new XmlSchemaSimpleTypeUnion();
			List<XmlQualifiedName> memberTypes = new List<XmlQualifiedName>();
			foreach (var baseEnum in enumeration.BaseEnums) 
			{
				memberTypes.Add(new XmlQualifiedName(NDR.getEnumPrefixname(baseEnum) +":" + NDR.GetBasicTypeName(baseEnum)));
			}
			unionElement.MemberTypes = memberTypes.ToArray();
			restrictedtype.Content = unionElement;
		}

        //add simpletype for the enuemration
        private static void addDependentEnumSchemas(XmlSchema schema, string schemaFileName, GeneratorContext context, IEnum enumeration)
        {
        	//check all the parent enums
        	foreach (var baseEnum in enumeration.BaseEnums) 
        	{
        		//generate the xsd for the base enum
        		var dependentSchemaInfo = GenerateXSD(context,baseEnum);
        		//add namespace declaration
        		schema.Namespaces.Add(NDR.getEnumPrefixname(baseEnum),dependentSchemaInfo.Schema.TargetNamespace);
        		//include/import schema
        		if (schema.TargetNamespace == dependentSchemaInfo.Schema.TargetNamespace)
        		{
	        		//include the schema for the base enum in this schema
	        		var include = new XmlSchemaInclude();
	                include.SchemaLocation = NDR.GetRelativePath(schemaFileName, dependentSchemaInfo.FileName);
	                schema.Includes.Add(include);
        		}
        		else
        		{
        			//import the schema
        			var schemaImport = new XmlSchemaImport();
					schemaImport.Namespace = dependentSchemaInfo.Schema.TargetNamespace;
					schemaImport.SchemaLocation = NDR.GetRelativePath(schemaFileName, dependentSchemaInfo.FileName);
					schema.Includes.Add(schemaImport);
        		}
        	}
        }
        private static string getSchemaFileName(GeneratorContext context, IEnum enumeration)
        {
        	var mainVersion = context.VersionID.Split('.').FirstOrDefault();
        	var minorVersion = context.VersionID.Split('.').LastOrDefault();
        	var bSlash = System.IO.Path.DirectorySeparatorChar;
        	string filename = context.OutputDirectory + bSlash + "generic" + bSlash //directories
        					+ enumeration.CodeListAgencyIdentifier + "_" + enumeration.UniqueIdentifier + "_"
        					+ enumeration.VersionIdentifier.Replace(".","p") +".xsd"; //filename
        	return  filename;
        }
    }
}