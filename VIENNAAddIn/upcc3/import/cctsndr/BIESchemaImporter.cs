// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using CctsRepository;
using CctsRepository.BdtLibrary;
using CctsRepository.BieLibrary;
using CctsRepository.CcLibrary;
using VIENNAAddIn.upcc3.import.util;

namespace VIENNAAddIn.upcc3.import.cctsndr
{
    ///<summary>
    /// The purpose of the static class BIESchemaImporter is to import an XML schema, 
    /// complying with CCTS and UN/CEFACT's NDRs, into an existing BIE library. The 
    /// importer is invoked using the static method "ImportXsd". For more information
    /// on the method "ImportXsd" please refer to the documentation of the method
    /// itself. Furthermore, the class provides a set of methods allowing to cumulate
    /// different artifacts in the CCTS XML schema such as cumulating an ABIE from 
    /// a complex type in the XML schema. 
    ///</summary>
    public static class BIESchemaImporter
    {
        private static ICcLibrary ExistingAccs;
        private static IBdtLibrary ExistingBdts;
        private static IBieLibrary BieLibrary;
        
        ///<summary>
        /// The static method imports an XML schema containing Business Information 
        /// Entities into an existing BIE library. The method has one input parameter
        /// of type ImporterContext specifying different settings utilized while 
        /// importing the XML schema.
        ///</summary>
        ///<param name="context">
        /// The parameter provides the necessary context information required by the
        /// BIE XML schema importer. An example would be the directory location where
        /// all XML schemas to be imported are located, and the repository which contains
        /// the BIE library that all ABIEs should be imported into. Fore more information
        /// plese refer to the documentation of the ImporterContext class. 
        /// </param>
        public static void ImportXSD(ImporterContext context)
        {
            #region Import Preparation

            // TODO: ACC, BDT and BIE library should be configurable through the ImporterContext
            InitLibraries(context);

            // Even though the XML schema is stored in the importer context we need to re-read
            // the XML schema. The reason for doing so is that the impoter context pre-read the
            // XML schema using the XML schema reader class from the .net API. However, the XML
            // schema reader dropped information that is required to import the ABIE schema. 
            // Instead we want to utilitze the CustomSchemaReader class that requires an XML
            // schema document as an input. Therefore we need to re-read the XML schema file based
            // on the file name and directory location as specified in the importer context.
            XmlDocument bieSchemaDocument = GetBieSchemaDocument(context);

            CustomSchemaReader reader = new CustomSchemaReader(bieSchemaDocument);

            #endregion
                   
            IDictionary<string, string> allElementDefinitions = new Dictionary<string, string>();

            #region Processing Step 1: Cumulate all ABIEs and create them in the BIE library

            foreach (object item in reader.Items)
            {
                if (item is ComplexType)
                {
                    ComplexType abieComplexType = (ComplexType)item;

                    AbieSpec singleAbieSpec = CumulateAbieSpecFromComplexType(abieComplexType);

                    BieLibrary.CreateAbie(singleAbieSpec);
                }

                if (item is Element)
                {
                    Element element = (Element)item;
                    allElementDefinitions.Add(element.Name, element.Type.Name);
                }
            }

            #endregion

            #region Processing Step 2: Update all ABIEs with their ASBIEs

            foreach (object item in reader.Items)
            {
                if (item is ComplexType)
                {
                    ComplexType abieComplexType = (ComplexType) item;

                    string abieName = abieComplexType.Name.Substring(0, abieComplexType.Name.Length - 4);

                    IAbie abieToBeUpdated = BieLibrary.GetAbieByName(abieName);
                    AbieSpec updatedAbieSpec = AbieSpec.CloneAbie(abieToBeUpdated);

                    updatedAbieSpec.Asbies = new List<AsbieSpec>(CumulateAbiesSpecsFromComplexType(abieComplexType, allElementDefinitions));

                    BieLibrary.UpdateAbie(abieToBeUpdated, updatedAbieSpec);
                }
            }

            #endregion
        }

        public static void InitLibraries(ImporterContext context)
        {
            ExistingAccs = context.CCLibrary;
            ExistingBdts = context.BDTLibrary;
            BieLibrary = context.BIELibrary;
        }

        #region Public Class Methods

        ///<summary>
        /// The methods resolves the cardinality expressed in XML schema terms, such as 
        /// "1" and "unbounded", and returns a CCTS compliant representation of the
        /// cardinality. 
        ///</summary>
        ///<param name="maxOccurs">
        /// The input parameter specifys the cardinality to be resolved whereas the cardinality
        /// is specified in XML schema terms. Examples include "1" and "unbounded".
        ///</param>
        ///<returns>
        /// The method returns a CCTS compliant representation of the cardinality. In particular 
        /// the method returns "0" for "0", "1" for "1", "*" for "unbounded" and "1" for all other
        /// cardinalities including an empty cardinality. 
        ///</returns>
        public static string ResolveMaxOccurs(string maxOccurs)
        {
            string resolvedMaxOccurs;

            switch (maxOccurs)
            {
                case "0":
                    resolvedMaxOccurs = "0";
                    break;
                case "1":
                    resolvedMaxOccurs = "1";
                    break;
                case "unbounded":
                    resolvedMaxOccurs = "*";
                    break;
                default:
                    resolvedMaxOccurs = "1";
                    break;
            }

            return resolvedMaxOccurs;
        }

        ///<summary>
        /// The methods resolves the cardinality expressed in XML schema terms, such as 
        /// "1", and returns a CCTS compliant representation of the
        /// cardinality. 
        ///</summary>
        ///<param name="minOccurs">
        /// The input parameter specifys the cardinality to be resolved whereas the cardinality
        /// is specified in XML schema terms. Examples include "0" and "1".
        ///</param>
        ///<returns>
        /// The method returns a CCTS compliant representation of the cardinality. In particular 
        /// the method returns "0" for "0", "1" for "1", and "1" for all other
        /// cardinalities including an empty cardinality. 
        ///</returns>
        public static string ResolveMinOccurs(string minOccurs)
        {
            string resolvedMinOccurs;

            switch (minOccurs)
            {
                case "0":
                    resolvedMinOccurs = "0";
                    break;
                case "1":
                    resolvedMinOccurs = "1";
                    break;
                default:
                    resolvedMinOccurs = "1";
                    break;
            }

            return resolvedMinOccurs;
        }

        ///<summary>
        /// The method creates an ABIESpec based on the complex type definition of an ABIE
        /// as it occurs in an XML schema. Therefore, the method requires on input parameter
        /// containing the complex type definition. A corresponding ABIESpec of the complex
        /// type definition is then returned through the return parameter. Realize that the 
        /// method does not process any ASBIE declarations. Therefore, only element declarations
        /// representing BBIEs within the ABIE are processed. To distinguish between ASBIEs and 
        /// BBIEs the prefix of the type in the element declaration is used. The prefix of the type
        /// used in BBIE element delcarations is assumed to be "bdt". Hence an element declaration
        /// for a BBIE must use the prefix "bdt" in order to be processed properly. Realize that for 
        /// processing ASBIEs within a complex type the method CumulateAbiesSpecsFromComplexType
        /// must be used. 
        ///</summary>
        ///<param name="abieComplexType">
        /// The input parameter contains the complex type declaration of the ABIE to be processed. An 
        /// example of a complex type definition is illustrated in the following.
        /// <code>
        ///   &lt;xsd:complexType name="AddressType"&gt;
        ///     &lt;xsd:sequence&gt;
        ///       &lt;xsd:element name="StreetName" type="bdt:TextStringType" minOccurs="0" maxOccurs="unbounded"/&gt;
        ///       &lt;xsd:element name="ZIP_PostcodeCode" type="bdt:CodeStringType" minOccurs="0" maxOccurs="unbounded"/&gt;
        ///       &lt;xsd:element ref="tns:Included_PersonPerson"/&gt;
        ///     &lt;/xsd:sequence&gt;
        ///   &lt;/xsd:complexType&gt;
        /// </code>
        ///</param>
        ///<returns>
        /// The method returns an ABIESpec for the complex type passed to the method through the 
        /// parameter <paramref name="abieComplexType"/>
        ///</returns>
        public static AbieSpec CumulateAbieSpecFromComplexType(ComplexType abieComplexType)
        {
            AbieSpec singleAbieSpec = new AbieSpec();

            string abieName = abieComplexType.Name.Substring(0, abieComplexType.Name.Length - 4);

            singleAbieSpec.Name = abieName;
            singleAbieSpec.BasedOn = FindBaseACCForABIE(abieName);

            List<BbieSpec> bbieSpecs = new List<BbieSpec>();

            foreach (object ctItem in abieComplexType.Items)
            {
                if (ctItem is Element)
                {
                    Element element = (Element)ctItem;

                    if (element.Ref.Name.Equals(""))
                    {
                        if (element.Type.Prefix.Equals("bdt"))
                        {
                            BbieSpec bbieSpec = new BbieSpec();

                            bbieSpec.Name = element.Name;
                            bbieSpec.LowerBound = ResolveMinOccurs(element.MinOccurs);
                            bbieSpec.UpperBound = ResolveMaxOccurs(element.MaxOccurs);

                            string bdtName = element.Type.Name.Substring(0, element.Type.Name.Length - 4);

                            foreach (IBdt bdt in ExistingBdts.Bdts)
                            {
                                if ((bdt.Name + bdt.Con.BasicType.DictionaryEntryName).Equals(bdtName))
                                {
                                    bbieSpec.Bdt = bdt;
                                    break;
                                }
                            }

                            if (bbieSpec.Bdt == null)
                            {
                                throw new Exception("Expected BDT not found: " + bdtName);
                            }

                            bbieSpecs.Add(bbieSpec);
                        }
                    }
                }
            }
            singleAbieSpec.Bbies = bbieSpecs;

            return singleAbieSpec;
        }

        private static IAcc FindBaseACCForABIE(string abieName)
        {
            string accName = abieName.Split('_').Last();
            return ExistingAccs.GetAccByName(accName);
        }

        ///<summary>
        ///</summary>
        ///<param name="abieComplexType">
        ///</param>
        ///<param name="allElementDefinitions">
        ///</param>
        ///<returns>
        ///</returns>
        public static IList<AsbieSpec> CumulateAbiesSpecsFromComplexType(ComplexType abieComplexType, IDictionary<string, string> allElementDefinitions)
        {
            IList<AsbieSpec> newAsbieSpecs = new List<AsbieSpec>();

            string abieName = abieComplexType.Name.Substring(0, abieComplexType.Name.Length - 4);

            foreach (Element element in abieComplexType.Items)
            {
                if (!(element.Ref.Name.Equals("")))
                {
                    string associatedABIEName = allElementDefinitions[element.Ref.Name];
                    associatedABIEName = associatedABIEName.Substring(0, associatedABIEName.Length - 4);

                    IAbie associatedAbie = BieLibrary.GetAbieByName(associatedABIEName);

                    string asbieName = NDR.GetAsbieNameFromXsdElement(element, associatedABIEName);

                    AsbieSpec asbieSpec = MatchAsbieToAscc(FindBaseACCForABIE(abieName), asbieName,
                                                           associatedAbie);

                    if (asbieSpec == null)
                    {
                        asbieSpec = CumulateAsbieSpec(element, asbieName, associatedAbie, AggregationKind.Shared);
                    }

                    newAsbieSpecs.Add(asbieSpec);
                }
                else
                {
                    if (element.Type.Prefix.Equals("tns"))
                    {
                        string associatedAbieName = element.Type.Name;
                        associatedAbieName = associatedAbieName.Substring(0, associatedAbieName.Length - 4);

                        IAbie associatedAbie = BieLibrary.GetAbieByName(associatedAbieName);

                        string asbieName = element.Name.Substring(0, element.Name.Length - associatedAbieName.Length);

                        AsbieSpec asbieSpec = MatchAsbieToAscc(FindBaseACCForABIE(abieName), asbieName,
                                                               associatedAbie);

                        if (asbieSpec == null)
                        {
                            asbieSpec = CumulateAsbieSpec(element, asbieName, associatedAbie,
                                                          AggregationKind.Composite);
                        }

                        newAsbieSpecs.Add(asbieSpec);
                    }
                }
            }

            return newAsbieSpecs;
        }

        public static IAbie getElementByName(string abieName)
        {
            return BieLibrary.GetAbieByName(abieName);
        }
        ///<summary>
        ///</summary>
        ///<param name="element">
        ///</param>
        ///<param name="asbieName">
        ///</param>
        ///<param name="associatedAbie">
        ///</param>
        ///<param name="aggregationKind">
        ///</param>
        ///<returns>
        ///</returns>
        public static AsbieSpec CumulateAsbieSpec(Element element, string asbieName, IAbie associatedAbie, AggregationKind aggregationKind)
        {
            AsbieSpec asbieSpec = new AsbieSpec
                                  {
                                      AssociatedAbie = associatedAbie,
                                      Name = asbieName,
                                      LowerBound = ResolveMinOccurs(element.MinOccurs),
                                      UpperBound = ResolveMaxOccurs(element.MaxOccurs),
                                      AggregationKind = aggregationKind
                                  };

            return asbieSpec;
        }

        #endregion

        #region Private Class Methods       

        /// <exception cref="Exception">Missing schema file for BIE schema</exception>
        private static XmlDocument GetBieSchemaDocument(ImporterContext context)
        {
            var bieSchemaDocument = new XmlDocument();
            bieSchemaDocument.Load(context.BIESchemaPath);
            return bieSchemaDocument;
        }

        ///<summary>
        ///</summary>
        ///<param name="acc">
        ///</param>
        ///<param name="asbieName">
        ///</param>
        ///<param name="associatedAbie">
        ///</param>
        ///<returns>
        ///</returns>
        private static AsbieSpec MatchAsbieToAscc(IAcc acc, string asbieName, IAbie associatedAbie)
        {
            foreach (IAscc ascc in acc.Asccs)
            {
                if (ascc.Name.Equals(asbieName))
                {
                    return AsbieSpec.CloneAscc(ascc, asbieName, associatedAbie);
                }
            }

            return null;
        }

        #endregion
    }
}