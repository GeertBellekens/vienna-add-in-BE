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

		private const string NSPREFIX_XSD = "xsd";
        private const string NS_DOC = "urn:un:unece:uncefact:documentation:common:3:standard:CoreComponentsTechnicalSpecification:3";
        private const string NS_XSD = "http://www.w3.org/2001/XMLSchema";

        public static void GenerateXSD(GeneratorContext context, XmlSchema schema)
        {
            foreach (var enumeration in context.Elements.OfType<IEnum>())
            {
                AddSimpleTypeDefinition(schema, context, enumeration);
            }
        }

        private static void AddSimpleTypeDefinition(XmlSchema schema, GeneratorContext context, IEnum enumeration)
        {
            var restrictedtype = new XmlSchemaSimpleType();
            restrictedtype.Name = NDR.GetBasicTypeName(enumeration);

            // enumeration with actual values
		    addEnumRestriction(restrictedtype, enumeration);

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
    }
}