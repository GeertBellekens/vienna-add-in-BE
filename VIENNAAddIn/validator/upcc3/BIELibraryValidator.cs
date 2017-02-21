using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VIENNAAddIn.constants;
using VIENNAAddIn.common;

namespace VIENNAAddIn.validator.upcc3
{
    class BIELibraryValidator : AbstractValidator
    {






        /// <summary>
        /// Validate the given BIELibrary
        /// </summary>
        /// <param name="context"></param>
        /// <param name="scope"></param>
        internal override void validate(IValidationContext context, string scope)
        {

            EA.Package package = context.Repository.GetPackageByID(Int32.Parse(scope));

            //Get all BDTs from the given package
            Dictionary<Int32, EA.Element> bies = new Dictionary<int, EA.Element>();
            Utility.getAllElements(package, bies, UPCC.ABIE.ToString());

            checkC514c(context, package);

            String n = package.Name;

            checkC514l(context, package);

            checkC574a(context, package, bies);

            checkC574b(context, package, bies);

            checkC574c(context, package, bies);

            checkC574d(context, package, bies);

            checkC574e(context, package, bies);

            checkC574f(context, package, bies);

            checkC574g(context, package, bies);

            checkC574h(context, package, bies);

            checkC574i(context, package, bies);

            checkC574j(context, package, bies);

            checkC574k(context, package, bies);

            checkC574l(context, package, bies);

            checkC574m(context, package, bies);

            checkC574n(context, package, bies);

            checkC574o(context, package, bies);

            checkC574p(context, package, bies);

            checkC574q(context, package, bies);

            checkC574r(context, package, bies);

            checkC574s(context, package, bies);

            checkC574t(context, package, bies);

            checkC574u(context, package, bies);

            checkC574v(context, package, bies);

            checkC574w(context, package, bies);

            checkC574x(context, package, bies);

            checkC574y(context, package, bies);

            checkC574z(context, package, bies);

        }



        /// <summary>
        /// A BIELibrary shall only contain ABIEs, BBIEs, and ASBIEs.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="brv"></param>
        private void checkC514c(IValidationContext context, EA.Package d)
        {
            foreach (EA.Element e in d.Elements)
            {
                if (e.Type != EA_Element.Note.ToString())
                {

                    String stereotype = e.Stereotype;
                    if (stereotype == null || !stereotype.Equals(UPCC.ABIE.ToString()))
                        context.AddValidationMessage(new ValidationMessage("Invalid element found in BIELibrary.", "The element " + e.Name + " has an invalid stereotype (" + stereotype + "). A BIELibrary shall only contain ABIEs, BBIEs, and ASBIEs.", "BIELibrary", ValidationMessage.errorLevelTypes.ERROR, d.PackageID));
                }
            }
        }




        /// <summary>
        /// A BIELibrary must not contain any sub packages
        /// </summary>
        /// <param name="context"></param>
        /// <param name="d"></param>
        private void checkC514l(IValidationContext context, EA.Package d)
        {
            foreach (EA.Package subPackage in d.Packages)
            {
                context.AddValidationMessage(new ValidationMessage("Invalid package found in BIELibrary.", "A BIELibrary must not contain any sub packages. Please remove sub package " + subPackage.Name, "BIELibrary", ValidationMessage.errorLevelTypes.ERROR, d.PackageID));
            }

        }


        /// <summary>
        /// An ABIE shall not contain – directly or at any nested level – a mandatory ASBIE whose associated ABIE is the same as the top level ABIE.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="bdts"></param>
        private void checkC574a(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> bies)
        {

            //Will be implemented with a DFS

        }


        /// <summary>
        /// An ABIE shall contain only BBIEs and ASBIEs. An ABIE shall contain at least one BBIE or ASBIE.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="bdts"></param>
        private void checkC574b(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> bies)
        {


            foreach (KeyValuePair<Int32, EA.Element> e in bies)
            {
                EA.Element element = e.Value;
                int count_asbie = 0;
                int count_bbie = 0;

                foreach (EA.Connector con in element.Connectors)
                {
                    if (con.Stereotype == UPCC.ASBIE.ToString() && con.ClientID == element.ElementID && (con.Type == AssociationTypes.Association.ToString() || con.Type == AssociationTypes.Aggregation.ToString()))
                    {
                        count_asbie++;
                    }
                }

                foreach (EA.Attribute bbie in element.Attributes)
                {
                    if (bbie.Stereotype == UPCC.BBIE.ToString())
                    {
                        count_bbie++;
                    }
                    else
                    {
                        context.AddValidationMessage(new ValidationMessage("An ABIE shall contain only BBIEs and ASBIEs.", "An ABIE shall contain only BBIEs and ASBIEs. There shall be at least one BBIE or at least one ASBIE. \nABIE " + element.Name + " has an attribute which is not stereotyped as BBIE.", "BIELibrary", ValidationMessage.errorLevelTypes.ERROR, p.PackageID));
                    }
                }


                if (count_asbie == 0 && count_bbie == 0)
                {
                    context.AddValidationMessage(new ValidationMessage("No ASBIEs/BBIEs found for an ABIE.", "An ABIE shall contain only BBIEs and ASBIEs. There shall be at least one BBIE or at least one ASBIE. \nABIE " + element.Name + " has neither any ASBIEs nor any BBIEs.", "BIELibrary", ValidationMessage.errorLevelTypes.ERROR, p.PackageID));
                }

            }

        }




        /// <summary>
        /// An ABIE shall have exactly one dependency of stereotype <<basedOn>> with an ACC as the target.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="bies"></param>
        private void checkC574c(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> bies)
        {


            foreach (KeyValuePair<Int32, EA.Element> e in bies)
            {
                EA.Element element = e.Value;
                bool foundBasedOn = false;

                foreach (EA.Connector con in element.Connectors)
                {

                    if (con.Stereotype == UPCC.basedOn.ToString() && con.ClientID == element.ElementID)
                    {
                        //Get the supplier (target) of the based on dependency
                        EA.Element supplier = context.Repository.GetElementByID(con.SupplierID);
                        //Is the supplier an ACC? If not raise an error
                        if (supplier.Stereotype == UPCC.ACC.ToString())
                        {
                            foundBasedOn = true;
                        }
                    }
                }

                //No based on dependency found - raise an error
                if (!foundBasedOn)
                {
                    context.AddValidationMessage(new ValidationMessage("Missing basedOn dependency found in an ABIE.", "An ABIE shall have exactly one dependency of stereotype <<basedOn>> with an ACC as the target. Errorneous ABIE: " + element.Name + " in BIELibrary " + p.Name, "BIELibrary", ValidationMessage.errorLevelTypes.ERROR, p.PackageID));
                }

            }
        }




        /// <summary>
        /// For a given BIE library the qualified ABIE name (= qualifiers + name) shall be unique
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="bies"></param>
        private void checkC574d(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> bies)
        {
            Dictionary<Int32, string> names = new Dictionary<int, string>();

            foreach (KeyValuePair<Int32, EA.Element> e in bies)
            {

                if (names.ContainsValue(e.Value.Name))
                {
                    context.AddValidationMessage(new ValidationMessage("Two ABIEs with the same name found.", "Each ABIE shall have a unique name within the BIELibrary of which it is part of. Two ABIEs with the name " + e.Value.Name + " found in BIELibrary " + p.Name + ".", "BIELibrary", ValidationMessage.errorLevelTypes.ERROR, p.PackageID));
                }

                names.Add(e.Value.ElementID, e.Value.Name);
            }

        }





        /// <summary>
        /// The name on an ABIE shall be the same as the name of the ACC on which it is based.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="bdts"></param>
        private void checkC574e(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> bies)
        {


            foreach (KeyValuePair<Int32, EA.Element> e in bies)
            {
                //Getthe ACC on which this BIE is based on 
                EA.Element targetElement = Utility.getTargetOfisbasedOnDependency(e.Value, context.Repository);

                if (targetElement != null)
                {
                    //The unqualified name of the BIE
                    String unqualifiedName = Utility.getUnqualifiedName(e.Value.Name);

                    //Compare the unqualified name of the BIE to the name of the underlying ACC
                    if (targetElement.Name != unqualifiedName)
                    {
                        context.AddValidationMessage(new ValidationMessage("Mismatch between ABIE and ACC name found.", "The name on an ABIE shall be the same as the name of the ACC on which it is based. The ABIE " + e.Value.Name + " found in BIELibrary " + p.Name + " does not have the same name as the ACC " + targetElement.Name + " it is based on. \n Pelase note, that Qualifiers (e.g. US_Internal_) are not part of an ABIE's name.", "BIELibrary", ValidationMessage.errorLevelTypes.ERROR, p.PackageID));

                    }
                }
            }

        }




        /// <summary>
        /// An ABIE which is the source of an isEquivalentTo dependency shall not be the target of an isEquivalentTo dependency.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="bdts"></param>
        private void checkC574f(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> bies)
        {

            foreach (KeyValuePair<Int32, EA.Element> e in bies)
            {
                //The source element of the isEquivalentTo dependency
                EA.Element element = e.Value;
                bool isSourceOfAnIsEquivalentToDependency = false;
                bool isTargetOfAnIsEquivlanetToDependency = false;

                foreach (EA.Connector con in element.Connectors)
                {

                    //Is the element source of an isEquivalentTo dependency?
                    if (con.Type == AssociationTypes.Dependency.ToString())
                    {
                        //Is Source
                        if (con.Stereotype == UPCC.isEquivalentTo.ToString() && con.ClientID == element.ElementID)
                        {
                            isSourceOfAnIsEquivalentToDependency = true;
                        }
                        else if (con.Stereotype == UPCC.isEquivalentTo.ToString() && con.SupplierID == element.ElementID)
                        {
                            isTargetOfAnIsEquivlanetToDependency = true;
                        }
                    }
                }

                if (isSourceOfAnIsEquivalentToDependency && isTargetOfAnIsEquivlanetToDependency)
                {
                    context.AddValidationMessage(new ValidationMessage("ABIE is source and target of an isEquivalentTo dependency.", "An ABIE which is the source of an isEquivalentTo dependency must not be the target of an isEquivalentTo dependency. Errorneous ABIE: " + element.Name, "BIELibrary", ValidationMessage.errorLevelTypes.ERROR, p.PackageID));
                }
            }


        }




        /// <summary>
        /// An ABIE which is the source of an isEquivalentTo dependency shall have the same number of BBIEs and ASBIEs as the ABIE the dependency is targeting.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="bdts"></param>
        private void checkC574g(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> bies)
        {


            foreach (KeyValuePair<Int32, EA.Element> e in bies)
            {

                if (Utility.isEquivalentToAnotherElement(e.Value))
                {

                    int countBBIEs = Utility.countAttributesOfElement(e.Value, UPCC.BBIE.ToString());
                    int countASBIEs = Utility.countConnectorsOfElement(e.Value, UPCC.ASBIE.ToString());


                    //Count the supplementary attributes of the BDT which is the target of the isEquivalentDependency
                    EA.Element supplierElement = Utility.getTargetOfisEquivalentToDependency(e.Value, context.Repository);
                    int countBBIEs_of_supplier = Utility.countAttributesOfElement(supplierElement, UPCC.BBIE.ToString());
                    int countASBIEs_of_supplier = Utility.countConnectorsOfElement(supplierElement, UPCC.ASBIE.ToString());

                    //Do both BIEs have the same amount of BBIEs?
                    if (countBBIEs != countBBIEs_of_supplier)
                    {
                        context.AddValidationMessage(new ValidationMessage("Invalid number of BBIEs found.", "An ABIE which is the source of an isEquivalentTo dependency shall have the same number of BBIEs as the ABIE the dependency is targeting. The source ABIE " + e.Value.Name + " has " + countBBIEs + " BBIEs but the ABIE " + supplierElement.Name + " which is targeted by an isEquivalent dependency has " + countBBIEs_of_supplier + " BBIEs.", "BIELibrary", ValidationMessage.errorLevelTypes.ERROR, p.PackageID));

                    }

                    //Do both BIEs have the same number of ASBIEs?
                    if (countASBIEs != countASBIEs_of_supplier)
                    {
                        context.AddValidationMessage(new ValidationMessage("Invalid number of ASBIEs found.", "An ABIE which is the source of an isEquivalentTo dependency shall have the same number of ASBIEs as the ABIE the dependency is targeting. The source ABIE " + e.Value.Name + " has " + countASBIEs + " ASBIEs but the ABIE " + supplierElement.Name + " which is targeted by an isEquivalent dependency has " + countASBIEs_of_supplier + " ASBIEs.", "BIELibrary", ValidationMessage.errorLevelTypes.ERROR, p.PackageID));

                    }


                }
            }

        }



        /// <summary>
        /// Each qualifier of an ABIE shall be followed by an underscore (_).
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="bdts"></param>
        private void checkC574h(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> bies)
        {

            //We cannot really check this constraint here but have to leave it in the UPCC specification in order to 
            //show users how qualifiers are represented correctly

        }




        /// <summary>
        /// If an ABIE does not have any qualifier its definition tagged value shall be the same as the definition tagged value of the ACC the ABIE is based on.
        /// 
        /// This is a WARN check
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="bdts"></param>
        private void checkC574i(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> bies)
        {
            
            foreach (KeyValuePair<Int32, EA.Element> e in bies)
            {
                //Is the ABIE qualified?
                if (!Utility.isAQualifiedElement(e.Value.Name))
                {
                    //Get the ACC this ABIE is based on
                    EA.Element acc = Utility.getTargetOfisbasedOnDependency(e.Value, context.Repository);

                    if (acc != null)
                    {
                        EA.TaggedValue definition = Utility.getTaggedValue(e.Value, UPCC_TV.definition.ToString());
                        EA.TaggedValue definition_acc = Utility.getTaggedValue(acc, UPCC_TV.definition.ToString());

                        //Both definitions must be the same
                        if (definition.Value != definition_acc.Value)
                        {
                            context.AddValidationMessage(new ValidationMessage("Mismatch in  definition tagged values.", "If an ABIE does not have any qualifier its definition tagged value shall be the same as the definition tagged value of the ACC the ABIE is based on. The source ABIE " + e.Value.Name + " in BIELibrary "+p.Name+" has a definition tagged value entry '" + definition.Value + "' but the ACC " + acc.Name + " on which the ABIE is based on has a definition tagged value entry '" + definition_acc.Value + "'.", "BIELibrary", ValidationMessage.errorLevelTypes.WARN, p.PackageID));
                        }
                    }
                }
            }
        }



        /// <summary>
        /// For a given BIELibrary there shall not be two ABIEs with the same unique identifier tagged value.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="bies"></param>
        private void checkC574j(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> bies)
        {

            Dictionary<Int32, string> values = new Dictionary<Int32, string>();

            foreach (KeyValuePair<Int32, EA.Element> e in bies)
            {
                EA.TaggedValue tv = Utility.getTaggedValue(e.Value, UPCC_TV.uniqueIdentifier.ToString());
                if (tv != null)
                {
                    //Has this unique identifier already been used?
                    if (values.ContainsValue(tv.Value))
                    {
                        //Get the other element with the same unique identifier
                        EA.Element duplicateElement = context.Repository.GetElementByID(Utility.findKey(values, tv.Value));

                        context.AddValidationMessage(new ValidationMessage("Two identical unique identifier tagged values of a BIE detected.", "The unique identifier tagged value of a BIE shall be unique for a given BIE library. " + e.Value.Name + " and " + duplicateElement.Name + " in BIELibrary "+p.Name+" have the same unique identifier.", "BIELibrary", ValidationMessage.errorLevelTypes.ERROR, p.PackageID));
                    }

                    values.Add(e.Value.ElementID, tv.Value);
                }
            }  


        }

        

        /// <summary>
        /// All BBIEs of a given ABIE shall have unique names.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="bdts"></param>
        private void checkC574k(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> bies)
        {

            Dictionary<Int32, string> names = new Dictionary<int, string>();

            foreach (KeyValuePair<Int32, EA.Element> e in bies)
            {

                foreach (EA.Attribute a in e.Value.Attributes)
                {
                    
                    if (names.ContainsValue(a.Name))
                    {
                        context.AddValidationMessage(new ValidationMessage("Two BBIEs with the same name found.", "Each BBIE shall have a unique name within its ABIE it is contained in. BBIE name " + a.Name + " found in ABIE "+e.Value.Name+" in BIELibrary " + p.Name + " is not unique.", "BIELibrary", ValidationMessage.errorLevelTypes.ERROR, p.PackageID));
                    }

                    names.Add(a.AttributeID, a.Name);
                }

                names = new Dictionary<int, string>();

            }

        }




        /// <summary>
        /// A BBIE type shall be typed with a class of stereotype <<BDT>>.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="bies"></param>
        private void checkC574l(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> bies)
        {


            foreach (KeyValuePair<Int32, EA.Element> e in bies)
            {
                //Iterate over the different attributes of every BIE
                foreach (EA.Attribute a in e.Value.Attributes)
                {
                    //Get the classifying element
                    int classifierID = a.ClassifierID;
                    bool error = false;
                    if (classifierID == 0)
                    {
                        error = true;
                    }
                    else
                    {
                        EA.Element classifyingElement = context.Repository.GetElementByID(classifierID);
                        if (!(classifyingElement.Stereotype == UPCC.BDT.ToString()))
                        {
                            error = true;
                        }
                    }

                    //Did an error occur?
                    if (error)
                    {
                        context.AddValidationMessage(new ValidationMessage("A BBIE with an invalid type has been found.", "A BBIE type shall be typed with a class of stereotype <<BDT>>. BBIE " + a.Name + " in ABIE " + e.Value.Name + " in BIELibrary " + p.Name + " has an invalid type.", "BIELibrary", ValidationMessage.errorLevelTypes.ERROR, p.PackageID));
                    }


                }
            }


        }


        /// <summary>
        /// Each qualifier of a BBIE shall be followed by an underscore (_).
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="bies"></param>
        private void checkC574m(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> bies)
        {
            //We cannot really enfore this constraint

        }


        /// <summary>
        /// For every BBIE of an ABIE there shall be a BCC with the same name in the underlying ACC the ABIE is based on.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="bies"></param>
        private void checkC574n(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> bies)
        {
            foreach (KeyValuePair<Int32, EA.Element> e in bies)
            {
                if (Utility.isBasedOnAnotherElement(e.Value))
                {

                    EA.Element ACC_base = Utility.getTargetOfisbasedOnDependency(e.Value, context.Repository);
                    if (ACC_base != null)
                    {
                        //Get a collection of the attribute names of the base ACC
                        IList<String> baseAttributes = Utility.getAttributesOfElement(ACC_base, UPCC.BCC.ToString());


                        //Every BBIE name of this ABIE must also be contained in the underlying ACC
                        foreach (EA.Attribute a in e.Value.Attributes)
                        {
                            if (a.Stereotype == UPCC.BBIE.ToString())
                            {

                                if (!baseAttributes.Contains(Utility.getUnqualifiedName(a.Name)))
                                {
                                    context.AddValidationMessage(new ValidationMessage("BBIE with missing BCC equivalent found.", "For every BBIE of an ABIE there shall be a BCC with the same name in the underlying ACC the ABIE is based on. BBIE " + a.Name + " in ABIE " + e.Value.Name + " in BIELibrary "+p.Name+" does not have a BCC with the same name in the underlying ACC.", "BIELibrary", ValidationMessage.errorLevelTypes.ERROR, p.PackageID));
                                }
                            }
                        }
                    }

                }
            }
                        

        }


        /// <summary>
        /// The cardinality of a BBIE shall not be an extension (i.e. be higher than) of the cardinality of the BCC, the BBIE is based on. A BBIE is based on another BCC if their names are the same and the ACC/ABIE they are properties of have, a basedOn dependency.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="bdts"></param>
        private void checkC574o(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> bies)
        {

            foreach (KeyValuePair<Int32, EA.Element> e in bies)
            {

                                    EA.Element ACC_base = Utility.getTargetOfisbasedOnDependency(e.Value, context.Repository);
                                    if (ACC_base != null)
                                    {
                                        //Get a collection of the attribute names of the base ACC
                                        Dictionary<String, String> bccs = Utility.getAttributesOfElementAsDictionary(ACC_base, UPCC.BCC.ToString());


                                        //Every BBIE name of this ABIE must also be contained in the underlying ACC
                                        foreach (EA.Attribute a in e.Value.Attributes)
                                        {
                                            if (a.Stereotype == UPCC.BBIE.ToString())
                                            {

                                                if (bccs.ContainsValue(a.Name))
                                                {
                                                    //Get the BCC
                                                    EA.Attribute bcc = context.Repository.GetAttributeByGuid(Utility.findKey(bccs, a.Name));
                                                    String bcc_lowerBound = bcc.LowerBound;
                                                    String bcc_upperBound = bcc.UpperBound;

                                                    //Check whether the lower bound is O.K.
                                                    if (Utility.isValidCardinality(bcc_lowerBound) && Utility.isValidCardinality(a.LowerBound))
                                                    {
                                                        //Raise an error if the lowerbound of the underlying BCC is higher than the lower bound of the 
                                                        if (Utility.isLowerBound(bcc_lowerBound, a.LowerBound))
                                                        {
                                                            context.AddValidationMessage(new ValidationMessage("BBIE has higher lower bound cardinality than underlying BCC.", "The cardinality of a BBIE shall not be an extension (i.e. be higher than) of the cardinality of the BCC, the BBIE is based on. A BBIE is based on another BCC if their names are the same and the ACC/ABIE they are properties of have, a basedOn dependency. \n\nLower bound cardinality of the underlying BCC " + bcc.Name + " of BBIE " + a.Name + " in ABIE " + e.Value.Name + " in BIELibrary " + p.Name + " is higher than the cardinality of the BBIE.", "BIELibrary", ValidationMessage.errorLevelTypes.ERROR, p.PackageID));
                                                        }

                                                    }
                                                        //Invalid cardinality - raise an error
                                                    else
                                                    {
                                                        context.AddValidationMessage(new ValidationMessage("Unable to determine cardinality of BBIE/underlying BCC.", "The cardinality of a BBIE shall not be an extension (i.e. be higher than) of the cardinality of the BCC, the BBIE is based on. A BBIE is based on another BCC if their names are the same and the ACC/ABIE they are properties of have, a basedOn dependency. \n\nUnable to determine the lower bound cardinality of the BBIE/underlying BCC " + bcc.Name + " of BBIE " + a.Name + " in ABIE " + e.Value.Name + " in BIELibrary " + p.Name + ".", "BIELibrary", ValidationMessage.errorLevelTypes.ERROR, p.PackageID));
                                                    }




                                                    if (Utility.isValidCardinality(bcc_upperBound) && Utility.isValidCardinality(a.UpperBound))
                                                    {
                                                        //Raise an error if the higher bound of the underlying BCC is higher than the lower bound of the 
                                                        if (Utility.isHigherBound(a.UpperBound, bcc_upperBound))
                                                        {
                                                            context.AddValidationMessage(new ValidationMessage("BBIE has higher upper bound cardinality than underlying BCC.", "The cardinality of a BBIE shall not be an extension (i.e. be higher than) of the cardinality of the BCC, the BBIE is based on. A BBIE is based on another BCC if their names are the same and the ACC/ABIE they are properties of have, a basedOn dependency. \n\nHigher bound cardinality of of BBIE " + a.Name + " in ABIE " + e.Value.Name + " in BIELibrary " + p.Name + " is higher than the cardinality of the underlying BCC.", "BIELibrary", ValidationMessage.errorLevelTypes.ERROR, p.PackageID));
                                                        }

                                                    }
                                                    //Invalid cardinality - raise an error
                                                    else
                                                    {
                                                        context.AddValidationMessage(new ValidationMessage("Unable to determine cardinality of BBIE/underlying BCC.", "The cardinality of a BBIE shall not be an extension (i.e. be higher than) of the cardinality of the BCC, the BBIE is based on. A BBIE is based on another BCC if their names are the same and the ACC/ABIE they are properties of have, a basedOn dependency. \n\nUnable to determine the higher bound cardinality of the BBIE/underlying BCC " + bcc.Name + " of BBIE " + a.Name + " in ABIE " + e.Value.Name + " in BIELibrary " + p.Name + ".", "BIELibrary", ValidationMessage.errorLevelTypes.ERROR, p.PackageID));
                                                    }    
                                                }
                                            }
                                        }
                                    }
            }
        }


        /// <summary>
        /// For a given BIELibrary there shall not be two BBIEs with the same unique identifier tagged value.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="bdts"></param>
        private void checkC574p(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> bies)
        {


            Dictionary<Int32, String> names = new Dictionary<Int32, string>();


            //Iterate over all ABIEs of a given BIELibrary
            foreach (KeyValuePair<Int32, EA.Element> e in bies)
            {

                //Get every BBIE unique identifier tagged value of every ABIE in this library
                foreach (EA.Attribute a in e.Value.Attributes)
                {
                    if (a.Stereotype == UPCC.BBIE.ToString())
                    {
                        EA.AttributeTag at = Utility.getTaggedValue(a, UPCC_TV.uniqueIdentifier.ToString());

                        //Found double name
                        if (at != null)
                        {
                            if (names.ContainsValue(at.Value))
                            {
                                context.AddValidationMessage(new ValidationMessage("Found two BBIEs with the same unique identifier tagged value.", "For a given BIELibrary there shall not be two BBIEs with the same unique identifier tagged value. ABIE " + e.Value.Name + " has more than one BBIE with the name " + at.Name + ".", "BIELibrary", ValidationMessage.errorLevelTypes.ERROR, p.PackageID));
                            }

                            names.Add(at.AttributeID, at.Value);
                        }
                    }
                }
            }
        }


        /// <summary>
        /// If a BBIE does not have any qualifiers its definition shall be the same as the definition of the BCC the BBIE is based on.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="bdts"></param>
        private void checkC574q(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> bies)
        {


        }


    
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="bdts"></param>
        private void checkC574r(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> bies)
        {


        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="bdts"></param>
        private void checkC574s(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> bies)
        {


        }
        
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="bdts"></param>
        private void checkC574t(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> bies)
        {


        }
        
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="bdts"></param>
        private void checkC574u(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> bies)
        {


        }
    
    
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="bdts"></param>
        private void checkC574v(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> bies)
        {


        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="bdts"></param>
        private void checkC574w(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> bies)
        {


        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="bdts"></param>
        private void checkC574x(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> bies)
        {


        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="bdts"></param>
        private void checkC574y(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> bies)
        {


        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="bdts"></param>
        private void checkC574z(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> bies)
        {


        }

        
       










































        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="bdts"></param>
        private void checkC574(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> bies)
        {


        }












    }
}
