using System.Collections.Generic;
using System.Xml.Schema;
using CctsRepository;
using CctsRepository.BdtLibrary;
using VIENNAAddIn.upcc3.export.cctsndr;
using VIENNAAddInUtils;

namespace VIENNAAddIn.upcc3.import.cctsndr.bdt
{
    internal class BDTExtensionComplexType : BDTComplexType
    {
        public BDTExtensionComplexType(XmlSchemaComplexType complexType, BDTSchema schema) : base(complexType, schema)
        {
        }

        public override string ContentComponentXsdTypeName
        {
            get { return ((XmlSchemaSimpleContentExtension) ComplexType.ContentModel.Content).BaseTypeName.Name; }
        }

        protected override IEnumerable<BdtSupSpec> SpecifySUPs()
        {
            var extension = (XmlSchemaSimpleContentExtension) ComplexType.ContentModel.Content;
            foreach (XmlSchemaAttribute attribute in extension.Attributes)
            {
                string basicTypeName = NDR.ConvertXsdTypeNameToBasicTypeName(attribute.SchemaTypeName.Name);
                yield return new BdtSupSpec
                             {
                                 Name = attribute.Name.Minus(basicTypeName),
                                 BasicType = new BasicType(FindPRIM(basicTypeName)),
                             };
            }
        }

        protected override BdtConSpec SpecifyCON()
        {
            return new BdtConSpec
                   {
                       Name = "Content",
                       BasicType = new BasicType(FindPRIM(NDR.ConvertXsdTypeNameToBasicTypeName(ContentComponentXsdTypeName)))
                   };
        }
    }
}