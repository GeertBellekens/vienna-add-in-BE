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
            var enums = new Dictionary<int, TempEnum>();
            foreach (var enumeration in context.Elements.OfType<IEnum>())
            {
                var sourceEnum = enumeration.SourceElement as IEnum;
                if (sourceEnum != null)
                {
                    if (!enums.ContainsKey(sourceEnum.Id))
                        enums.Add(sourceEnum.Id, new TempEnum(sourceEnum));
                    //add the values
                    enums[sourceEnum.Id].addCodeListentries(enumeration);

                }
            }
            //go over the tempEnums and create the actual enum definitions
            foreach (var tempEnum in enums.Values )
            {
                AddSimpleTypeDefinition(schema, context, tempEnum);
            }
            
        }

        
        private static void AddSimpleTypeDefinition(XmlSchema schema, GeneratorContext context, TempEnum tempEnum)
        {
            var restrictedtype = new XmlSchemaSimpleType();
            restrictedtype.Name = NDR.GetBasicTypeName(tempEnum.sourceEnum);

            // enumeration with actual values
		    addEnumRestriction(restrictedtype, tempEnum);

            //add the restrictedType to the schema
            schema.Items.Add(restrictedtype);
        }
        
        static void addEnumRestriction(XmlSchemaSimpleType restrictedtype, TempEnum tempEnum )
		{
        	var restriction = new XmlSchemaSimpleTypeRestriction();
		    restriction.BaseTypeName = new XmlQualifiedName(NSPREFIX_XSD + ":token");
			foreach (var codeListEntry in tempEnum.codelistEntries.OrderBy(x => x.position) )
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
        class TempEnum
        {
            public TempEnum(IEnum sourceEnum)
            {
                this.sourceEnum = sourceEnum;
            }
            public IEnum sourceEnum { get; private set; }
            public List<ICodelistEntry> codelistEntries { get; private set; } = new List<ICodelistEntry>();
            public void addCodeListentries (IEnum enumeration)
            {
                //if the enum doesn't contain any values that means that all values are allowed and thus all values of the source enum should be used.
                var enumToUse = enumeration.CodelistEntries.Any() && (enumeration.SourceElement as IEnum) != null ?
                                enumeration :
                                enumeration.SourceElement as IEnum;
                //do not add duplicates
                foreach(var codeListentry in enumToUse.CodelistEntries)
                {
                    if (!this.codelistEntries.Any(x => x.Name == codeListentry.Name))
                        this.codelistEntries.Add(codeListentry);
                }
            }
        }
    }

}