using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using CctsRepository.BdtLibrary;
using CctsRepository.CdtLibrary;
using CctsRepository.PrimLibrary;
using VIENNAAddIn.upcc3.export.cctsndr;
using VIENNAAddInUtils;

namespace VIENNAAddIn.upcc3.import.cctsndr.bdt
{
    public abstract class BDTXsdType
    {
        protected readonly BDTSchema bdtSchema;

        protected BDTXsdType(XmlSchemaType xsdType, BDTSchema bdtSchema)
        {
            XsdType = xsdType;
            this.bdtSchema = bdtSchema;
        }

        protected XmlSchemaType XsdType { get; private set; }

        public IImporterContext Context
        {
            get { return bdtSchema.Context; }
        }

        public string XsdTypeName
        {
            get { return XsdType.Name; }
        }

        public IBdt BDT { get; private set; }
        public abstract string ContentComponentXsdTypeName { get; }

        public void CreateBDT()
        {
            string bdtName = GetBdtNameFromXsdType();
            ICdt cdt = Context.CDTLibrary.GetCdtByName(GetDataTypeTerm());
            var bdtSpec = new BdtSpec
                          {
                              BasedOn = cdt,
                              Name = bdtName,
                              Con = SpecifyCON(),
                              Sups = new List<BdtSupSpec>(SpecifySUPs()),
                          };

            BDT = Context.BDTLibrary.CreateBdt(bdtSpec);
        }

        protected abstract IEnumerable<BdtSupSpec> SpecifySUPs();

        protected IPrim FindPRIM(string primName)
        {
            IPrim prim = Context.PRIMLibrary.GetPrimByName(primName);
            if (prim == null)
            {
                throw new Exception(String.Format("PRIM not found: {0}", primName));
            }
            return prim;
        }

        protected IBdt GetBDTByXsdTypeName(string typeName)
        {
            return bdtSchema.GetBDTByXsdTypeName(typeName);
        }

        private string GetDataTypeTerm()
        {
            string xsdTypeNameWithoutGuid = XsdType.Name.Substring(0, XsdType.Name.LastIndexOf('_'));

            string xsdTypeNameWithQualifiers = xsdTypeNameWithoutGuid.Minus("Type");

            var qualifierString = new StringBuilder();
            foreach (string qualifier in GetQualifiers())
            {
                qualifierString.Append(qualifier);
            }

            return xsdTypeNameWithQualifiers.Substring(qualifierString.Length);
        }

        private string GetBdtNameFromXsdType()
        {
            var qualifiersForClassName = new StringBuilder();
            foreach (string qualifier in GetQualifiers())
            {
                qualifiersForClassName.Append(qualifier).Append("_");
            }
            return qualifiersForClassName + GetDataTypeTerm();
        }

        private List<string> GetQualifiers()
        {
            string cctsNamespacePrefix = bdtSchema.CCTSNamespacePrefix;

            var qualifiers = new List<string>();

            if (XsdType.Annotation != null)
            {
                XmlSchemaObjectCollection annotationItems = XsdType.Annotation.Items;
                foreach (XmlSchemaObject item in annotationItems)
                {
                    if (item is XmlSchemaDocumentation)
                    {
                        XmlNode[] nodes = ((XmlSchemaDocumentation) item).Markup;
                        foreach (XmlNode node in nodes)
                        {
                            if (node is XmlElement)
                            {
                                if (node.Name == cctsNamespacePrefix + ":DataTypeQualifierTermName")
                                {
                                    string qualifier = node.InnerText;
                                    qualifiers.Add(qualifier);
                                }
                            }
                        }
                    }
                }
            }
            return qualifiers;
        }

        protected abstract BdtConSpec SpecifyCON();
    }
}