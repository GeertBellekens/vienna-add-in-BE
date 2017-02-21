using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VIENNAAddIn.constants;
using VIENNAAddIn.common;

namespace VIENNAAddIn.validator.upcc3
{
    class BDTLibraryValidator : AbstractValidator
    {













        /// <summary>
        /// Validate the given bLibrary
        /// </summary>
        /// <param name="context"></param>
        /// <param name="scope"></param>
        internal override void validate(IValidationContext context, string scope)
        {

            EA.Package package = context.Repository.GetPackageByID(Int32.Parse(scope));
            //Get all BDTs from the given package
            Dictionary<Int32, EA.Element> bdts = new Dictionary<int, EA.Element>();
            Utility.getAllElements(package, bdts, UPCC.BDT.ToString());

            checkC514b(context, package);

            checkC514i(context, package);

            checkC564a(context, package, bdts);

            checkC564b(context, package, bdts);

            checkC564c(context, package, bdts);

            checkC564d(context, package, bdts);

            checkC564e(context, package, bdts);

            checkC564f(context, package, bdts);

            checkC564g(context, package, bdts);

            checkC564h(context, package, bdts);

            checkC564i(context, package, bdts);

            checkC564j(context, package, bdts);

            checkC564k(context, package, bdts);

            checkC564l(context, package, bdts);

            checkC564m(context, package, bdts);

            checkC564n(context, package, bdts);

            checkC564o(context, package, bdts);

            checkC564p(context, package, bdts);

            checkC564q(context, package, bdts);

            checkC564r(context, package, bdts);

            checkC564s(context, package, bdts);

            checkC564t(context, package, bdts);

            checkC564u(context, package, bdts);

            checkC564v(context, package, bdts);

            checkC564w(context, package, bdts);

            checkC564x(context, package, bdts);

            checkC564y(context, package, bdts);

            checkC564z(context, package, bdts);

            checkC564aa(context, package, bdts);

            checkC564bb(context, package, bdts);

            checkC564cc(context, package, bdts);

            checkC564dd(context, package, bdts);

            checkC564ee(context, package, bdts);

            checkC564ff(context, package, bdts);

            checkC564gg(context, package, bdts);

            checkC564hh(context, package, bdts);

            checkC564ii(context, package, bdts);

            checkC564jj(context, package, bdts);

            checkC564kk(context, package, bdts);

            checkC564ll(context, package, bdts);

            checkC564mm(context, package, bdts);

            checkC564nn(context, package, bdts);

            checkC564oo(context, package, bdts);


        }



        
        /// <summary>
        /// A BDTLibrary shall only contain BDTs
        /// </summary>
        /// <param name="context"></param>
        /// <param name="brv"></param>
        private void checkC514b(IValidationContext context, EA.Package d)
        {
            foreach (EA.Element e in d.Elements)
            {
                if (e.Type != EA_Element.Note.ToString())
                {

                    String stereotype = e.Stereotype;
                    if (stereotype == null || !stereotype.Equals(UPCC.BDT.ToString()))
                        context.AddValidationMessage(new ValidationMessage("Invalid element found in BDTLibrary.", "The element " + e.Name + " has an invalid stereotype (" + stereotype + "). A BDTLibrary shall only contain BDTs.", "BDTLibrary", ValidationMessage.errorLevelTypes.ERROR, d.PackageID));
                }                
            }
        }


        /// <summary>
        /// A BDTLibrary must not contain any sub packages
        /// </summary>
        /// <param name="context"></param>
        /// <param name="d"></param>
        private void checkC514i(IValidationContext context, EA.Package d)
        {
            foreach (EA.Package subPackage in d.Packages)
            {
                context.AddValidationMessage(new ValidationMessage("Invalid package found in BDTLibrary.", "A BDTLibrary must not contain any sub packages. Please remove sub package " + subPackage.Name, "BDTLibrary", ValidationMessage.errorLevelTypes.ERROR, d.PackageID));
            }

        }



        /// <summary>
        /// The name of a BDT shall be unique for a given BDT library.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="bdts"></param>
        private void checkC564a(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> bdts) {

            Dictionary<Int32, string> names = new Dictionary<int, string>();

            foreach (KeyValuePair<Int32, EA.Element> e in bdts)
            {

                if (names.ContainsValue(e.Value.Name))
                {
                    context.AddValidationMessage(new ValidationMessage("Two BDTs with the same name found.", "Each BDT shall have a unique name within the BDT library of which it is part of. Two BDTs with the name " + e.Value.Name + " found in BDTLibrary " + p.Name + ".", "BDTLibrary", ValidationMessage.errorLevelTypes.ERROR, p.PackageID));
                }

                names.Add(e.Value.ElementID, e.Value.Name);
            }



        }


        /// <summary>
        /// A BDT shall not contain any attributes other than those of stereotype <<CON>> or <<SUP>>.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="bdts"></param>
        private void checkC564b(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> bdts)
        {

            foreach (KeyValuePair<Int32, EA.Element> e in bdts)
            {

                EA.Element element = e.Value;

                foreach (EA.Attribute attr in element.Attributes)
                {
                    if (!(attr.Stereotype == UPCC.CON.ToString() || attr.Stereotype == UPCC.SUP.ToString()))
                    {
                        context.AddValidationMessage(new ValidationMessage("Invalid attribute stereotype found in a BDT.", "A BDT must not contain any attributes other than those of stereotype <<CON>> or <<SUP>>. Attribute " + attr.Name + " of BDT " + element.Name + " in BDTLibrary "+p.Name+" has an invalid stereotype.", "BDTLibrary", ValidationMessage.errorLevelTypes.ERROR, p.PackageID));
                    }
                }


            }

        }


        /// <summary>
        /// A BDT shall contain exactly one content component. The name of a BDT content component shall have the fixed value “Content”.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="bdts"></param>
        private void checkC564c(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> bdts)
        {


            foreach (KeyValuePair<Int32, EA.Element> e in bdts)
            {
                int count = 0;
                foreach (EA.Attribute attr in e.Value.Attributes)
                {
                    if (attr.Stereotype == UPCC.CON.ToString())
                    {
                        count++;

                        String wrongname = attr.Name;

                        if (wrongname != "Content")
                        {
                            if (wrongname == "")
                                wrongname = "No name specified.";

                            context.AddValidationMessage(new ValidationMessage("Wrong name of <<CON>> attribute found in a BDT.", "A BDT shall contain exactly one content component The name of the BDT content component shall have the fixed value 'Content'. BDT " + e.Value.Name + " in BDTLibrary " + p.Name + " has a <<CON>> attribute with a wrong name: "+wrongname+" .", "BDTLibrary", ValidationMessage.errorLevelTypes.ERROR, p.PackageID));
                        }
                    }
                }

                if (count != 1)
                {
                    context.AddValidationMessage(new ValidationMessage("Invalid number of <<CON>> attributes found in a BDT.", "A BDT shall contain exactly one content component. BDT " + e.Value.Name + " in BDTLibrary "+p.Name+" contains " + count + " attributes stereotyped as <<CON>>.", "BDTLibrary", ValidationMessage.errorLevelTypes.ERROR, p.PackageID));
                }

            }



        }

        /// <summary>
        /// A BDT shall be the source of a dependency stereotyped basedOn leading to a CDT or another BDT, unless it is the source of an isEquivalentTo dependency.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="bdts"></param>
        private void checkC564d(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> bdts)
        {
            foreach (KeyValuePair<Int32, EA.Element> e in bdts)
            {
                //Search for basedOn dependencies leading for which the given BDT is a source
                bool found = false;

                //Ignore any BDT which are connected to another BDT via a isEquivalentTo dependency since the are implicitly
                //based on the BDT/CDT the target of the dependency is based on
                if (!Utility.isEquivalentToAnotherElement(e.Value))
                {

                    foreach (EA.Connector con in e.Value.Connectors)
                    {

                        if (con.Stereotype == UPCC.basedOn.ToString() && con.Type == AssociationTypes.Dependency.ToString() && con.ClientID == e.Value.ElementID)
                        {
                            //Found a target
                            EA.Element target = context.Repository.GetElementByID(con.SupplierID);
                            if (target.Stereotype == UPCC.BDT.ToString() || target.Stereotype == UPCC.CDT.ToString())
                            {
                                found = true;
                            }
                        }
                    }
                    //Missing basedOn dependency
                    if (!found)
                    {
                        //Target does not have the correct stereotype
                        context.AddValidationMessage(new ValidationMessage("Missing basedOn dependency found for a BDT.", "A BDT shall be the source of a dependency stereotyped basedOn leading to a CDT or another BDT, unless it is the source of an isEquivalentTo dependency. BDT " + e.Value.Name + " in BDTLibrary " + p.Name + " is missing a basedOn dependency to another BDT or to a CDT.", "BDTLibrary", ValidationMessage.errorLevelTypes.ERROR, p.PackageID));
                    }
                }
            }
        }


        /// <summary>
        /// The type of BDT attributes shall be a class stereotyped either as <<PRIM>> or <<ENUM>>.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="bdts"></param>
        private void checkC564e(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> bdts)
        {


            foreach (KeyValuePair<Int32, EA.Element> e in bdts)
            {
                //Iterate over the different attributes of every BDT
                foreach (EA.Attribute a in e.Value.Attributes)
                {
                    //There are only valid attribute stereotyped as CON or SUP (has been taken care of in a previous constraint)
                    int classifierID = a.ClassifierID;
                    bool error = false;
                    if (classifierID == 0)
                    {
                        error = true;
                    }
                    else
                    {
                        EA.Element classifyingElement = context.Repository.GetElementByID(classifierID);
                        if (!(classifyingElement.Stereotype == UPCC.PRIM.ToString() || classifyingElement.Stereotype == UPCC.ENUM.ToString()))
                        {
                            error = true;
                        }
                    }

                    //Did an error occur?
                    if (error)
                    {
                        context.AddValidationMessage(new ValidationMessage("A BDT with an invalid type has been found.", "The type of BDT attributes shall be a class stereotyped either as <<PRIM>> or <<ENUM>>. The attribute "+ a.Name + " in BDT " + e.Value.Name + " in BDTLibrary " + p.Name + " has an invalid type.", "BDTLibrary", ValidationMessage.errorLevelTypes.ERROR, p.PackageID));                        
                    }


                }
            }


        }


        /// <summary>
        /// The source of an isEquivalentTo dependeny must be a BDT.  
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="bdts"></param>
        private void checkC564f(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> bdts)
        {

            foreach (KeyValuePair<Int32, EA.Element> e in bdts)
            {
                                
                //Search for basedOn dependencies leading for which the given BDT is a source                
                foreach (EA.Connector con in e.Value.Connectors)
                {

                    if (con.Stereotype == UPCC.isEquivalentTo.ToString() && con.Type == AssociationTypes.Dependency.ToString() && con.SupplierID == e.Value.ElementID)
                    {
                        //Get the client (source) of this basedOn dependency
                        EA.Element client = context.Repository.GetElementByID(con.ClientID);
                        if (client.Stereotype != UPCC.BDT.ToString())
                        {
                            context.AddValidationMessage(new ValidationMessage("Invalid stereotype of a source of an isEquivalentTo dependency.", "The source of an isEquivalentTo dependeny must be a BDT. Element " + client.Name + " in BDTLibrary " + p.Name + " has an isEquivalentTo dependency to BDT " + e.Value.Name + " but does not have the stereotype BDT.", "BDTLibrary", ValidationMessage.errorLevelTypes.ERROR, p.PackageID)); 
                        }                        
                    }
                }
            }

        }


        /// <summary>
        /// A BDT which is the source of an isEquivalentTo dependency must not be the source of a basedOn dependency.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="bdts"></param>
        private void checkC564g(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> bdts)
        {


            foreach (KeyValuePair<Int32, EA.Element> e in bdts)
            {
                if (Utility.isEquivalentToAnotherElement(e.Value) && Utility.isBasedOnAnotherElement(e.Value)) 
                {
                    context.AddValidationMessage(new ValidationMessage("BDT is source of isEquivalentTo and basedOn dependency.", " A BDT which is the source of an isEquivalentTo dependency must not be the source of a basedOn dependency. Errorneous BCC " + e.Value.Name + " in BDTLibrary " + p.Name + ".", "BDTLibrary", ValidationMessage.errorLevelTypes.ERROR, p.PackageID)); 
                }
            }
        }


        /// <summary>
        /// A BDT which is the source of an isEquivalentTo dependency shall not be the target of an isEquivalentTo dependency.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="bdts"></param>
        private void checkC564h(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> bdts)
        {

            foreach (KeyValuePair<Int32, EA.Element> e in bdts)
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
                    context.AddValidationMessage(new ValidationMessage("BDT is source and target of an isEquivalentTo dependency.", "A BDT which is the source of an isEquivalentTo dependency must not be the target of an isEquivalentTo dependency. Errorneous BDT: " + element.Name, "BDTLibrary", ValidationMessage.errorLevelTypes.ERROR, p.PackageID));
                }
            }

        }


        /// <summary>
        /// A BDT which is the source of an isEquivalentTo dependency shall have the same number of supplementary 
        /// components as the BDT the dependency is targeting.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="bdts"></param>
        private void checkC564i(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> bdts)
        {

            foreach (KeyValuePair<Int32, EA.Element> e in bdts)            
            {

                if (Utility.isEquivalentToAnotherElement(e.Value))
                {

                    int countSUP = Utility.countAttributesOfElement(e.Value, UPCC.SUP.ToString());
                    

                    //Count the supplementary attributes of the BDT which is the target of the isEquivalentDependency
                    EA.Element supplierElement = Utility.getTargetOfisEquivalentToDependency(e.Value, context.Repository);
                    int countSUP_of_supplier = Utility.countAttributesOfElement(supplierElement, UPCC.SUP.ToString());

                    //Do both BDT have the same amount of SUPs?
                    if (countSUP != countSUP_of_supplier) {
                        context.AddValidationMessage(new ValidationMessage("Invalid number of supplementary components found.", "A BDT which is the source of an isEquivalentTo dependency shall have the same number of supplementary components as the BDT the dependency is targeting. The source BDT " + e.Value.Name + " has "+ countSUP + " supplementary components but the BDT " + supplierElement.Name + " which is targeted by an isEquivalent dependency has " + countSUP_of_supplier + " supplementary components.", "BDTLibrary", ValidationMessage.errorLevelTypes.ERROR, p.PackageID));

                    }
                }
            }


        }


        /// <summary>
        /// An unqualified BDT shall include the same supplementary components as its source CDT.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="bdts"></param>
        private void checkC564j(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> bdts)
        {


            foreach (KeyValuePair<Int32, EA.Element> e in bdts)
            {

                    //Check only unqualified Elements
                    if (!Utility.isAQualifiedElement(e.Value.Name))
                    {
                        //Get the CDT
                        EA.Element supplier = Utility.getTargetOfisbasedOnDependency(e.Value, context.Repository);
                        if (supplier != null)
                        {
                            if (supplier.Stereotype == UPCC.CDT.ToString() || supplier.Stereotype == UPCC.BDT.ToString())
                            {

                                int countSUP = Utility.countAttributesOfElement(e.Value, UPCC.SUP.ToString());
                                int countSUP_supplier = Utility.countAttributesOfElement(supplier, UPCC.SUP.ToString());

                                if (countSUP != countSUP_supplier)
                                {
                                    context.AddValidationMessage(new ValidationMessage("Invalid number of supplementary components found.", "An unqualified BDT shall include the same supplementary components as its source CDT. BDT " + e.Value.Name + " is based on " + supplier.Stereotype + " " + supplier.Name + ". " + e.Value.Stereotype + " " + e.Value.Name + " has " + countSUP + " SUPs and " + supplier.Stereotype + " " + supplier.Name + " has " + countSUP_supplier + " SUPs.", "BDTLibrary", ValidationMessage.errorLevelTypes.ERROR, p.PackageID));

                                }
                            }
                        }
                    }                
            }




        }


        /// <summary>
        /// An unqualified BDT shall have the same name as the CDT the BDT is based on.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="bdts"></param>
        private void checkC564k(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> bdts)
        {


            foreach (KeyValuePair<Int32, EA.Element> e in bdts)
            {

                //Determine whether the given BDT is qualified or not
                if (!Utility.isAQualifiedElement(e.Value.Name))
                {
                    EA.Element supplier = Utility.getTargetOfisbasedOnDependency(e.Value, context.Repository);
                    if (supplier != null)
                    {
                        if (supplier.Name.Trim() != e.Value.Name.Trim())
                        {
                            context.AddValidationMessage(new ValidationMessage("Mismatch of names between BDT and underlying CDT.", "An unqualified BDT shall have the same name as the CDT the BDT is based on. BDT " + e.Value.Name + " and the CDT/BDT " + supplier.Name + " do not have the same name.", "BDTLibrary", ValidationMessage.errorLevelTypes.ERROR, p.PackageID));
                        }
                    }
                }
            }

        }


        /// <summary>
        /// A BDT shall not contain attributes which have not been defined in the CDT the BDT is based on or in the less qualified BDT the given BDT is based on. 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="bdts"></param>
        private void checkC564l(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> bdts)
        {

            foreach (KeyValuePair<Int32, EA.Element> e in bdts)
            {
                if (Utility.isBasedOnAnotherElement(e.Value))
                {

                    EA.Element CDT_BDT_base = Utility.getTargetOfisbasedOnDependency(e.Value, context.Repository);
                    //Get a collection of the attribute names of the base CDT/BDT
                    IList<String> baseAttributes = Utility.getSupplementaryComponentsOfElement(CDT_BDT_base);

                    //Every SUP attribute of this BDT must also be contained in the underlying BDT/CDT
                    foreach (EA.Attribute a in e.Value.Attributes)
                    {
                        if (a.Stereotype == UPCC.SUP.ToString())
                        {

                            if (!baseAttributes.Contains(a.Name))
                            {
                                context.AddValidationMessage(new ValidationMessage("Invalid attribute found in BDT.", "A BDT shall not contain attributes which have not been defined in the CDT the BDT is based on or in the less qualified BDT the given BDT is based on. Invalid attribute " + a.Name + " in BDT " + e.Value.Name, "BDTLibrary", ValidationMessage.errorLevelTypes.ERROR, p.PackageID));
                            }
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Each BDT content component enumeration set of allowed values shall be equal to or less than the set of enumeration values of its source CDT content component enumeration or less restricted BDT content component enumeration.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="bdts"></param>
        private void checkC564m(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> bdts)
        {
            //Not a crucial constraint - will be implemented at a later stage
        }


        /// <summary>
        /// Each BDT supplementary component enumeration set of allowed values shall be equal to or less than the set of enumeration values of its source CDT supplementary component enumeration or less restricted BDT supplementary component enumeration.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="bdts"></param>
        private void checkC564n(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> bdts)
        {
            //Not a crucial constraint - will be implemented at a later stage
        }


        /// <summary>
        /// BDT supplementary component cardinality shall be equal to [0..1] if the BDT supplementary component is optional, or [1..1] if mandatory.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="bdts"></param>
        private void checkC564o(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> bdts)
        {


            foreach (KeyValuePair<Int32, EA.Element> e in bdts)
            {
                //Fetch all attributes of the given element
                Dictionary<string, EA.Attribute> attributes = Utility.fetchAllAttributesFromElement(e.Value, UPCC.SUP.ToString());

                foreach (KeyValuePair<string, EA.Attribute> a in attributes)
                {
                    String lowerBound = a.Value.LowerBound;
                    String upperBound = a.Value.UpperBound;

                    //The only allowed combinations for the cardinality are [0..1] and [1..1]
                    //[0..1] = lowerBound = 0, upperBound = 1
                    //[1..1] = lowerBound = 1, upperBound = 1 || lowerBound = "", upperBound = ""
                    if (!Utility.isValid_0_1_or_1_1_Cardinality(lowerBound, upperBound))
                    {
                        context.AddValidationMessage(new ValidationMessage("Invalid cardinality found for a BDT.", "BDT supplementary component cardinality shall be equal to [0..1] if the BDT supplementary component is optional, or [1..1] if mandatory. Attribute " + a.Value.Name + " in ACC " + e.Value.Name + " has an invalid cardinality: lower bound: " + lowerBound + " upper bound " + upperBound + ".", "BDTLibrary", ValidationMessage.errorLevelTypes.ERROR, p.PackageID));
                    }
                }

            }

        }


        /// <summary>
        /// The cardinality of a supplementary component of an unqualified BDT shall be cardinality of the supplementary component of the CDT the BDT is based on.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="bdts"></param>
        private void checkC564p(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> bdts)
        {
            //Not a crucial constraint - will be implemented at a later stage

        }


        /// <summary>
        /// A BDT supplementary component’s cardinality shall be a restriction of the cardinality of the underlying CDT or less qualified BDT but never an extension thereof.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="bdts"></param>
        private void checkC564q(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> bdts)
        {
            //Not a crucial constraint - will be implemented at a later stage
        }


        /// <summary>
        /// The tagged value unique identifier of a BDT shall be unique for a given BDT library.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="bdts"></param>
        private void checkC564r(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> bdts)
        {

            Dictionary<Int32, string> values = new Dictionary<Int32, string>();

            foreach (KeyValuePair<Int32, EA.Element> e in bdts)
            {
                EA.TaggedValue tv = Utility.getTaggedValue(e.Value, UPCC_TV.uniqueIdentifier.ToString());
                if (tv != null)
                {
                    //Has this unique identifier already been used?
                    if (values.ContainsValue(tv.Value))
                    {
                        //Get the other element with the same unique identifier
                        EA.Element duplicateElement = context.Repository.GetElementByID(Utility.findKey(values, tv.Value));

                        context.AddValidationMessage(new ValidationMessage("Two identical unique identifier tagged values of a BDT detected.", "The unique identifier tagged value of a BDT shall be unique for a given BDT library. " + e.Value.Name + " and " + duplicateElement.Name + " have the same unique identifier.", "BDTLibrary", ValidationMessage.errorLevelTypes.ERROR, p.PackageID));
                    }

                    values.Add(e.Value.ElementID, tv.Value);
                }
            }  

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="bdts"></param>
        private void checkC564s(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> bdts)
        {

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="bdts"></param>
        private void checkC564t(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> bdts)
        {

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="bdts"></param>
        private void checkC564u(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> bdts)
        {

        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="bdts"></param>
        private void checkC564v(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> bdts)
        {

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="bdts"></param>
        private void checkC564w(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> bdts)
        {

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="bdts"></param>
        private void checkC564x(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> bdts)
        {

        }


        /// <summary>
        /// The dictionary entry name tagged value of a BDT shall be unique for a given BDT library.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="bdts"></param>
        private void checkC564y(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> bdts)
        {

            Dictionary<Int32, string> values = new Dictionary<Int32, string>();

            foreach (KeyValuePair<Int32, EA.Element> e in bdts)
            {
                EA.TaggedValue tv = Utility.getTaggedValue(e.Value, UPCC_TV.dictionaryEntryName.ToString());
                if (tv != null)
                {
                    //Has this dictionary entry name already been used?
                    if (values.ContainsValue(tv.Value))
                    {
                        //Get the other element with the same dictionary entry name
                        EA.Element duplicateElement = context.Repository.GetElementByID(Utility.findKey(values, tv.Value));

                        context.AddValidationMessage(new ValidationMessage("Two identical dictionary entry name tagged values of a BDT detected.", "The dictionary entry name tagged value of a BDT shall be unique for a given BDT library. " + e.Value.Name + " and " + duplicateElement.Name + " have the same dictionary entry name.", "BDTLibrary", ValidationMessage.errorLevelTypes.ERROR, p.PackageID));
                    }

                    values.Add(e.Value.ElementID, tv.Value);
                }
            }  

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="bdts"></param>
        private void checkC564z(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> bdts)
        {

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="bdts"></param>
        private void checkC564aa(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> bdts)
        {

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="bdts"></param>
        private void checkC564bb(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> bdts)
        {

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="bdts"></param>
        private void checkC564cc(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> bdts)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="bdts"></param>
        private void checkC564dd(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> bdts)
        {

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="bdts"></param>
        private void checkC564ee(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> bdts)
        {

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="bdts"></param>
        private void checkC564ff(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> bdts)
        {

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="bdts"></param>
        private void checkC564gg(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> bdts)
        {

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="bdts"></param>
        private void checkC564hh(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> bdts)
        {

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="bdts"></param>
        private void checkC564ii(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> bdts)
        {

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="bdts"></param>
        private void checkC564jj(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> bdts)
        {

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="bdts"></param>
        private void checkC564kk(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> bdts)
        {

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="bdts"></param>
        private void checkC564ll(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> bdts)
        {

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="bdts"></param>
        private void checkC564mm(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> bdts)
        {

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="bdts"></param>
        private void checkC564nn(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> bdts)
        {

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="bdts"></param>
        private void checkC564oo(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> bdts)
        {

        }











    }
}
