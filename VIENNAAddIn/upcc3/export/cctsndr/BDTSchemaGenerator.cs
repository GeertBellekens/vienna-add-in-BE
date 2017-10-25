using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Xml;
using System.Xml.Schema;
using CctsRepository;
using CctsRepository.BdtLibrary;
using VIENNAAddIn.upcc3.repo;
using VIENNAAddIn.upcc3.repo.EnumLibrary;
using VIENNAAddInUtils;

namespace VIENNAAddIn.upcc3.export.cctsndr
{
    ///<summary>
    ///</summary>
    public static class BDTSchemaGenerator
    {
		private const string NSPREFIX_NS1 = "ns1";
        private const string NSPREFIX_BDT = "bdt";
        private const string NSPREFIX_DOC = "ccts";
        private const string NSPREFIX_XBT = "xbt";
		private const string NSPREFIX_XSD = "xsd";
		
        private const string NS_DOC = "urn:un:unece:uncefact:documentation:common:3:standard:CoreComponentsTechnicalSpecification:3";
        private const string NS_XBT = "urn:un:unece:uncefact:data:common:1:draft"; 
        private const string NS_XSD = "http://www.w3.org/2001/XMLSchema";
        
    	public static void GenerateXSD(GeneratorContext context, GeneratorContext genericContext, IEnumerable<IBdt> bdts)
    	{
    		genericContext.AddElements(bdts);
        	context.AddElements(bdts);
        	GenerateXSD(context);
    	}
        public static void GenerateXSD(GeneratorContext context)
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
            
            schema.Version = context.VersionID.DefaultTo("1");
			string schemaFileName = getSchemaFileName(context);
			foreach (IBdt bdt in context.Elements.OfType<IBdt>())
            {
                var sups = new List<IBdtSup>(bdt.Sups);
                if (sups.Count == 0)
                {
                    var simpleType = new XmlSchemaSimpleType {Name = NDR.GetXsdTypeNameFromBdt(bdt)};
                    var simpleTypeRestriction = new XmlSchemaSimpleTypeRestriction
                                                {
                                                    BaseTypeName = GetXmlQualifiedName(NDR.getConBasicTypeName(bdt),context)
                                                };
                    simpleType.Content = simpleTypeRestriction;
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
                    simpleContentExtension.BaseTypeName = GetXmlQualifiedName(NDR.getConBasicTypeName(bdt),context);
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
		            			var sourceEnum = basicEnum.SourceElement as UpccEnum;
		            			if (sourceEnum != null
		            			    && sourceEnum.CodelistEntries.Count() != basicEnum.CodelistEntries.Count())
			            		{
			            			var restrictedtype = new XmlSchemaSimpleType();
					            	var restriction = new XmlSchemaSimpleTypeRestriction();
					            	restriction.BaseTypeName = new XmlQualifiedName(NSPREFIX_NS1 + ":" + NDR.GetBasicTypeName(sourceEnum));
					            	addEnumerationValues(restriction, basicEnum);
					            	//add the restriction to the simple type
					            	restrictedtype.Content = restriction;
					            	//set the type of the attribute
					            	attribute.SchemaType = restrictedtype;
			            		}

		            		}
		            	}
                        //set regular type name if not restricted
                        if (attribute.SchemaTypeName == null
                           && attribute.SchemaType == null)
                        {
                        	attribute.SchemaTypeName = GetXmlQualifiedName(NDR.GetBasicTypeName(sup as UpccAttribute),context);
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
            context.AddSchema(schema, schemaFileName,Schematype.BDT);
        }
       	static void addEnumerationValues(XmlSchemaSimpleTypeRestriction restriction, UpccEnum basicEnum )
		{
			foreach (var codeListEntry in basicEnum.CodelistEntries) 
			{
				var xmlEnum = new XmlSchemaEnumerationFacet();
				xmlEnum.Value = codeListEntry.Name;
				restriction.Facets.Add(xmlEnum);
			}
		}
        
        private static string getSchemaFileName(GeneratorContext context)
        {
        	var mainVersion = context.VersionID.Split('.').FirstOrDefault();
        	var minorVersion = context.VersionID.Split('.').LastOrDefault();
        	var docRootName =  context.DocRootName;
        	var bSlash = System.IO.Path.DirectorySeparatorChar;
        	var docOrGeneric = context.isGeneric ? "generic" : "document" 
        					  + (string.IsNullOrEmpty(docRootName) ? string.Empty : bSlash.ToString())
        					  + docRootName ;
        	//TODO set "Ebix" prefix via settings?
        	string filename = context.OutputDirectory + bSlash + mainVersion + bSlash + docOrGeneric + bSlash //directories
        					+ "ebIX_MessageDataType_" + docRootName + (string.IsNullOrEmpty(docRootName) ? string.Empty : "_") 
        					+ mainVersion + "p" + minorVersion + ".xsd"; //filename
        	return  filename;
        }

        private static XmlSchemaAnnotation GetAttributeAnnotation(IBdtSup sup)
        {
            var xml = new XmlDocument();
            // Deviation from rule [R 9C95]: Generating only a subset of the defined annotations and added some additional annotations.
            var annNodes = new List<XmlNode>();
            AddAnnotation(xml, annNodes, "PropertyTermName", sup.Name);
            AddAnnotation(xml, annNodes, "RepresentationTermName", sup.BasicType.Name);
            AddAnnotation(xml, annNodes, "PrimitiveTypeName", sup.BasicType.Name);
            AddAnnotation(xml, annNodes, "DataTypeName", sup.Bdt.Name);
            AddAnnotation(xml, annNodes, "UniqueID", sup.UniqueIdentifier);
            AddAnnotation(xml, annNodes, "VersionID", sup.VersionIdentifier);
            AddAnnotation(xml, annNodes, "DictionaryEntryName", sup.DictionaryEntryName);
            AddAnnotation(xml, annNodes, "Definition", sup.Definition);
            AddAnnotations(xml, annNodes, "BusinessTermName", sup.BusinessTerms);
            AddAnnotation(xml, annNodes, "ModificationAllowedIndicator",
                          sup.ModificationAllowedIndicator.ToString().ToLower());
            AddAnnotation(xml, annNodes, "LanguageCode", sup.LanguageCode);
            AddAnnotation(xml, annNodes, "AcronymCode", "SUP");
            var ann = new XmlSchemaAnnotation();
            ann.Items.Add(new XmlSchemaDocumentation {Language = "en", Markup = annNodes.ToArray()});
            return ann;
        }

        private static XmlSchemaAnnotation GetTypeAnnotation(IBdt bdt)
        {
            var xml = new XmlDocument();
            // Deviation from rule [R BFE5]: Generating only a subset of the defined annotations and added some additional annotations.
            var annNodes = new List<XmlNode>();
            AddAnnotation(xml, annNodes, "UniqueID", bdt.UniqueIdentifier);
            AddAnnotation(xml, annNodes, "VersionID", bdt.VersionIdentifier);
            AddAnnotation(xml, annNodes, "DictionaryEntryName", bdt.DictionaryEntryName);
            AddAnnotation(xml, annNodes, "Definition", bdt.Definition);
            AddAnnotations(xml, annNodes, "BusinessTermName", bdt.BusinessTerms);
            AddAnnotation(xml, annNodes, "PropertyTermName", bdt.Name);
            AddAnnotation(xml, annNodes, "LanguageCode", bdt.LanguageCode);
            AddAnnotation(xml, annNodes, "AcronymCode", "BDT");
            var ann = new XmlSchemaAnnotation();
            ann.Items.Add(new XmlSchemaDocumentation {Language = "en", Markup = annNodes.ToArray()});
            return ann;
        }

        private static void AddAnnotations(XmlDocument xml, List<XmlNode> annNodes, string name,
                                           IEnumerable<string> values)
        {
            foreach (string item in values)
            {
                AddAnnotation(xml, annNodes, name, item);
            }
        }

        private static void AddAnnotation(XmlDocument xml, List<XmlNode> annNodes, string name, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                XmlElement annotation = xml.CreateElement("ccts", name,
                                                          "urn:un:unece:uncefact:documentation:standard:XMLNDRDocumentation:3");
                annotation.InnerText = value;
                annNodes.Add(annotation);
            }
        }

        private static XmlQualifiedName GetXmlQualifiedName(string basicTypeName, GeneratorContext context)
        {
        	if (basicTypeName.StartsWith("Assembled", System.StringComparison.InvariantCulture))
    	    {
        		return new XmlQualifiedName(basicTypeName, context.BaseURN );
    	    }
            return new XmlQualifiedName(GetXSDType(basicTypeName) );
        }

        private static string GetXSDType(string primitiveTypeName)
        {
            switch (primitiveTypeName.ToLower())
            {
                case "string":
                    return "string";
                case "decimal":
                    return "decimal";
                case "binary":
                    return "base64Binary";
                case "base64binary":
                    return "base64Binary";
                case "token":
                    return "token";
                case "double":
                    return "double";
                case "integer":
                    return "integer";
                case "long":
                    return "long";
                case "datetime":
                    return "dateTime";
                case "date":
                    return "date";
                case "time":
                    return "time";
                case "boolean":
                    return "boolean";
                default:
                    return "string";
            }
        }
    }
}