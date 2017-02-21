using System.Collections.Generic;
using System.Xml.Schema;
using CctsRepository;
using CctsRepository.BdtLibrary;

namespace VIENNAAddIn.upcc3.import.cctsndr.bdt
{
    internal class BDTSimpleType : BDTXsdType
    {
        public BDTSimpleType(XmlSchemaSimpleType simpleType, BDTSchema bdtSchema) : base(simpleType, bdtSchema)
        {
        }

        private XmlSchemaSimpleType SimpleType
        {
            get { return (XmlSchemaSimpleType) XsdType; }
        }

        public override string ContentComponentXsdTypeName
        {
            get
            {
                BDTXsdType parent = Parent;
                if (parent != null)
                {
                    return parent.ContentComponentXsdTypeName;
                }
                return Restriction.BaseTypeName.Name;
            }
        }

        private BDTXsdType Parent
        {
            get { return NDR.IsXsdDataTypeName(Restriction.BaseTypeName) ? null : bdtSchema.GetBDTXsdType(Restriction.BaseTypeName.Name); }
        }

        private XmlSchemaSimpleTypeRestriction Restriction
        {
            get { return (XmlSchemaSimpleTypeRestriction) SimpleType.Content; }
        }

        protected override IEnumerable<BdtSupSpec> SpecifySUPs()
        {
            yield break;
        }

        private BdtConSpec ApplyCONRestrictions(BdtConSpec conSpec)
        {
            foreach (XmlSchemaObject facet in Restriction.Facets)
            {
                if (facet is XmlSchemaLengthFacet)
                {
                    // TODO tagged value 'length' is no longer defined as of UPCC 3.0 ODP5
                    //                    conSpec.Length = ((XmlSchemaFacet) facet).Value;
                }
                else if (facet is XmlSchemaMinLengthFacet)
                {
                    conSpec.MinimumLength = ((XmlSchemaFacet) facet).Value;
                }
                else if (facet is XmlSchemaMaxLengthFacet)
                {
                    conSpec.MaximumLength = ((XmlSchemaFacet) facet).Value;
                }
                else if (facet is XmlSchemaPatternFacet)
                {
                    conSpec.Pattern = ((XmlSchemaFacet) facet).Value;
                }
                else if (facet is XmlSchemaMaxInclusiveFacet)
                {
                    conSpec.MaximumInclusive = ((XmlSchemaFacet) facet).Value;
                }
                else if (facet is XmlSchemaMaxExclusiveFacet)
                {
                    conSpec.MaximumExclusive = ((XmlSchemaFacet) facet).Value;
                }
                else if (facet is XmlSchemaMinInclusiveFacet)
                {
                    conSpec.MinimumInclusive = ((XmlSchemaFacet) facet).Value;
                }
                else if (facet is XmlSchemaMinExclusiveFacet)
                {
                    conSpec.MinimumExclusive = ((XmlSchemaFacet) facet).Value;
                }
                else if (facet is XmlSchemaFractionDigitsFacet)
                {
                    conSpec.FractionDigits = ((XmlSchemaFacet) facet).Value;
                }
                else if (facet is XmlSchemaTotalDigitsFacet)
                {
                    conSpec.TotalDigits = ((XmlSchemaFacet) facet).Value;
                }
                else if (facet is XmlSchemaWhiteSpaceFacet)
                {
                    // TODO whitespace no longer defined by UPCC:
//                    conSpec.WhiteSpace = ((XmlSchemaFacet) facet).Value;
                }
            }

            return conSpec;
        }

        protected override BdtConSpec SpecifyCON()
        {
            BdtConSpec conSpec;
            BDTXsdType parent = Parent;
            if (parent != null)
            {
                conSpec = BdtConSpec.CloneBdtCon(parent.BDT.Con);
            }
            else
            {
                conSpec = new BdtConSpec
                          {
                              Name = "Content",
                              BasicType = new BasicType(FindPRIM(NDR.ConvertXsdTypeNameToBasicTypeName(ContentComponentXsdTypeName)))
                          };
            }
            return ApplyCONRestrictions(conSpec);
        }
    }
}