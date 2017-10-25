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
			var schema = new XmlSchema {TargetNamespace = context.TargetNamespace};
            //add namespaces
            schema.Namespaces.Add(context.NamespacePrefix, context.TargetNamespace);
            // R 9B18: all XML schemas must utilize the xsd prefix when referring to the W3C XML schema namespace            
            schema.Namespaces.Add(NSPREFIX_XSD, NS_XSD);
			//add namespace for documentation
            schema.Namespaces.Add(NSPREFIX_DOC, NS_DOC);
            // add namespace ns1
            schema.Namespaces.Add(NSPREFIX_NS1, context.BaseURN);
            //add namespace xbt
            schema.Namespaces.Add(NSPREFIX_XBT, NS_XBT);
            //add version
            schema.Version = context.VersionID.DefaultTo("1");
            //get the schemaFileName
            string schemaFileName = getSchemaFileName(context,enumeration);
            //TODO add the rest of the schema
            //return the schema info
            return context.AddSchema(schema, schemaFileName,UpccSchematype.ENUM);
        }
        private static string getSchemaFileName(GeneratorContext context, IEnum enumeration)
        {
        	var mainVersion = context.VersionID.Split('.').FirstOrDefault();
        	var minorVersion = context.VersionID.Split('.').LastOrDefault();
        	var bSlash = System.IO.Path.DirectorySeparatorChar;
        	string filename = context.OutputDirectory + bSlash + mainVersion + bSlash + "generic" + bSlash //directories
        					+ enumeration.CodeListAgencyIdentifier + "_" + enumeration.UniqueIdentifier + "_"
        					+ enumeration.VersionIdentifier.Replace(".","p") +".xsd"; //filename
        	return  filename;
        }
    }
}