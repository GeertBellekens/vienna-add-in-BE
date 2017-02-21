using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using CctsRepository.BdtLibrary;

namespace VIENNAAddIn.upcc3.import.cctsndr.bdt
{
    public class BDTSchema
    {
        public IImporterContext Context { get; private set; }
        private readonly Dictionary<string, BDTXsdType> bdtTypes = new Dictionary<string, BDTXsdType>();

        public BDTSchema(IImporterContext context)
        {
            Context = context;
        }

        public void CreateBDTs()
        {
            foreach (XmlSchemaObject currentElement in Context.BDTSchema.Items)
            {
                if (currentElement is XmlSchemaComplexType)
                {
                    var complexType = (XmlSchemaComplexType) currentElement;
                    BDTComplexType bdtType;
                    if (complexType.ContentModel.Content is XmlSchemaSimpleContentRestriction)
                    {
                        bdtType = new BDTRestrictionComplexType(complexType, this);
                    }
                    else
                    {
                        bdtType = new BDTExtensionComplexType(complexType, this);
                    }
                    bdtTypes[bdtType.XsdTypeName] = bdtType;
                }
                else if (currentElement is XmlSchemaSimpleType)
                {
                    var bdtType = new BDTSimpleType((XmlSchemaSimpleType) currentElement, this);
                    bdtTypes[bdtType.XsdTypeName] = bdtType;
                }
            }
            foreach (var bdtType in bdtTypes.Values)
            {
                bdtType.CreateBDT();
            }
        }

        public string CCTSNamespacePrefix
        {
            get
            {
                XmlQualifiedName[] qualifiedNames = Context.BDTSchema.Namespaces.ToArray();
                for (int i = 0; i < qualifiedNames.Length; i++)
                {
                    if (qualifiedNames[i].Namespace == NDR.CCTSNamespace)
                    {
                        return qualifiedNames[i].Name;
                    }
                }
                throw new Exception("CCTS namespace not defined: " + NDR.CCTSNamespace);
            }
        }

        public XmlSerializerNamespaces Namespaces
        {
            get { return Context.BDTSchema.Namespaces; }
        }

        public IBdt GetBDTByXsdTypeName(string typeName)
        {
            BDTXsdType bdtType;
            if (bdtTypes.TryGetValue(typeName, out bdtType))
            {
                if (bdtType.BDT == null)
                {
                    bdtType.CreateBDT();
                }
                return bdtType.BDT;
            }
            throw new Exception(string.Format("Invalid XSD type name: <{0}>.", typeName));
        }

        public BDTXsdType GetBDTXsdType(string xsdTypeName)
        {
            return bdtTypes[xsdTypeName];
        }
    }
}