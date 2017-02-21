// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System.Collections.Generic;
using CctsRepository.EnumLibrary;

namespace VIENNAAddIn.upcc3.export.cctsndr
{
    public class ENUMLibraryGenerator
    {
        public void GenerateXSD(GeneratorContext context, IEnumerable<IEnum> enums)
        {
//            foreach (var ENUM in library.ENUMs)
//            {
//                var schema = new XmlSchema
//                             {
//                                 ElementFormDefault = XmlSchemaForm.Qualified,
//                                 AttributeFormDefault = XmlSchemaForm.Unqualified
//                             };
//                schema.Namespaces.Add("xsd", "http://www.w3.org/2001/XMLSchema");
//                schema.Namespaces.Add("ccts", "urn:un:unece:uncefact:documentation:standard:CoreComponentsTechnicalSpecification:2");
//                var simpleType = new XmlSchemaSimpleType { Name = ENUM.Name + "Type" };
//                var simpleTypeRestriction = new XmlSchemaSimpleTypeRestriction();
//                foreach (var value in ENUM.Values)
//                {
//                    var enumeration = new XmlSchemaEnumerationFacet { Value = value.Value };
//                    simpleTypeRestriction.Facets.Add(enumeration);
//                }
//                simpleType.Content = simpleTypeRestriction;
//                schema.Items.Add(simpleType);
//                Context.AddSchema(schema);
//            }
        }
    }
}