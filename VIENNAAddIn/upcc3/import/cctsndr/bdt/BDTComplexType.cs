using System.Collections.Generic;
using System.Xml.Schema;
using CctsRepository;
using CctsRepository.BdtLibrary;
using VIENNAAddIn.upcc3.export.cctsndr;
using VIENNAAddInUtils;

namespace VIENNAAddIn.upcc3.import.cctsndr.bdt
{
    public abstract class BDTComplexType : BDTXsdType
    {
        protected BDTComplexType(XmlSchemaComplexType complexType, BDTSchema schema) : base(complexType, schema)
        {
        }

        protected XmlSchemaComplexType ComplexType
        {
            get { return (XmlSchemaComplexType) XsdType; }
        }

        protected IEnumerable<BdtSupSpec> SpecifySUPs(XmlSchemaObjectCollection xsdAttributes)
        {
            foreach (XmlSchemaAttribute attribute in xsdAttributes)
            {
                string basicTypeName = NDR.ConvertXsdTypeNameToBasicTypeName(attribute.SchemaTypeName.Name);
                yield return new BdtSupSpec
                             {
                                 Name = attribute.Name.Minus(basicTypeName),
                                 BasicType = new BasicType(FindPRIM(basicTypeName)),
                             };
            }
        }
    }
}