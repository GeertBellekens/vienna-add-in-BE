using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VIENNAAddIn.constants;
using VIENNAAddIn.common;

namespace VIENNAAddIn.validator.upcc3
{
    class CDTLibraryValidator : AbstractValidator
    {





        /// <summary>
        /// Validate the given CDTLibrary
        /// </summary>
        /// <param name="context"></param>
        /// <param name="scope"></param>
        internal override void validate(IValidationContext context, string scope)
        {

            EA.Package package = context.Repository.GetPackageByID(Int32.Parse(scope));

            //Get all CDTs from the given package
            Dictionary<Int32, EA.Element> cdts = new Dictionary<int, EA.Element>();
            Utility.getAllElements(package, cdts, UPCC.CDT.ToString());

            checkC514e(context, package);

            checkC514j(context, package);

            checkC534a(context, package, cdts);

            checkC534b(context, package, cdts);

            checkC534c(context, package, cdts);

            checkC534d(context, package, cdts);

            checkC534e(context, package, cdts);

            checkC534f(context, package, cdts);

            checkC534g(context, package, cdts);

            checkC534h(context, package, cdts);

            checkC534i(context, package, cdts);

            checkC534j(context, package, cdts);

            checkC534k(context, package, cdts);

            checkC534l(context, package, cdts);

            checkC534m(context, package, cdts);

            checkC534n(context, package, cdts);

            checkC534o(context, package, cdts);

            checkC534p(context, package, cdts);

            checkC534q(context, package, cdts);

            checkC534r(context, package, cdts);

            checkC534s(context, package, cdts);

            checkC534t(context, package, cdts);

            checkC534u(context, package, cdts);

            checkC534v(context, package, cdts);

            checkC534w(context, package, cdts);


        }



        /// <summary>
        /// A CDTLibrary shall only contain CDTs
        /// </summary>
        /// <param name="context"></param>
        /// <param name="brv"></param>
        private void checkC514e(IValidationContext context, EA.Package d)
        {
            foreach (EA.Element e in d.Elements)
            {
                if (e.Type != EA_Element.Note.ToString())
                {

                    String stereotype = e.Stereotype;
                    if (stereotype == null || !stereotype.Equals(UPCC.CDT.ToString()))
                        context.AddValidationMessage(new ValidationMessage("Invalid element found in CDTLibrary.", "The element " + e.Name + " has an invalid stereotype (" + stereotype + "). A CDTLibrary shall only contain CDTs.", "CDTLibrary", ValidationMessage.errorLevelTypes.ERROR, d.PackageID));
                }
            }
        }


        /// <summary>
        /// A CDTLibrary must not contain any sub packages
        /// </summary>
        /// <param name="context"></param>
        /// <param name="d"></param>
        private void checkC514j(IValidationContext context, EA.Package d)
        {
            foreach (EA.Package subPackage in d.Packages)
            {
                context.AddValidationMessage(new ValidationMessage("Invalid package found in CDTLibrary.", "A CDTLibrary must not contain any sub packages. Please remove sub package " + subPackage.Name, "CDTLibrary", ValidationMessage.errorLevelTypes.ERROR, d.PackageID));
            }

        }


        /// <summary>
        /// Each CDT shall have a unique name within the CDT library of which it is part of.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="d"></param>
        /// <param name="cdts"></param>
        private void checkC534a(IValidationContext context, EA.Package d, Dictionary<Int32, EA.Element> cdts)
        {
            Dictionary<Int32, string> names = new Dictionary<int, string>();

            foreach (KeyValuePair<Int32, EA.Element> e in cdts)
            {

                if (names.ContainsValue(e.Value.Name))
                {
                    context.AddValidationMessage(new ValidationMessage("Two CDTs with the same name found.", "Each CDT shall have a unique name within the CDT library of which it is part of. Two CDTs with the name " + e.Value.Name + " found.", "CDTLibrary", ValidationMessage.errorLevelTypes.ERROR, d.PackageID));                    
                }

                names.Add(e.Value.ElementID, e.Value.Name);
            }

        }


        /// <summary>
        /// A CDT shall be one of the approved CDTs published in the UN/CEFACT Data Type Catalogue.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="d"></param>
        /// <param name="cdts"></param>
        private void checkC534b(IValidationContext context, EA.Package d, Dictionary<Int32, EA.Element> cdts)
        {

            //Not implemented yet since the UN/CEFACt Data Type Catalogue has not been released yet.

        }



        /// <summary>
        /// A CDT must not contain any attributes other than those of stereotype <<CON>> or <<SUP>>.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="d"></param>
        /// <param name="cdts"></param>
        private void checkC534c(IValidationContext context, EA.Package d, Dictionary<Int32, EA.Element> cdts)
        {

            foreach (KeyValuePair<Int32, EA.Element> e in cdts)
            {

                EA.Element element = e.Value;

                foreach (EA.Attribute attr in element.Attributes)
                {
                    if (!(attr.Stereotype == UPCC.CON.ToString() || attr.Stereotype == UPCC.SUP.ToString())) {
                        context.AddValidationMessage(new ValidationMessage("Invalid attribute stereotype found in a CDT.", "A CDT must not contain any attributes other than those of stereotype <<CON>> or <<SUP>>. Attribute " + attr.Name + " of CDT "+ element.Name +" has an invalid stereotype.", "CDTLibrary", ValidationMessage.errorLevelTypes.ERROR, d.PackageID)); 
                    }
                }


            }
        }

        /// <summary>
        /// A CDT shall contain exactly one content component.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="d"></param>
        /// <param name="cdts"></param>
        private void checkC534d(IValidationContext context, EA.Package d, Dictionary<Int32, EA.Element> cdts)
        {

            foreach (KeyValuePair<Int32, EA.Element> e in cdts)
            {
                int count = 0;
                foreach (EA.Attribute attr in e.Value.Attributes)
                {
                    if (attr.Stereotype == UPCC.CON.ToString())
                    {
                        count++;
                    }
                }

                if (count != 1)
                {
                    context.AddValidationMessage(new ValidationMessage("Invalid number of <<CON>> attributes found in a CDT.", "A CDT shall contain exactly one content component. CDT " + e.Value.Name + " contains " + count + " attributes stereotyped as <<CON>>.", "CDTLibrary", ValidationMessage.errorLevelTypes.ERROR, d.PackageID));
                }

            }


        }


        /// <summary>
        /// The source of an isEquivalentTo dependency must be a CDT. A CDT which is the source of an isEquivalentTo dependency must not be the target of an isEquivalentTo dependency.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="cdts"></param>
        private void checkC534e(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> cdts)
        {

            foreach (KeyValuePair<Int32, EA.Element> e in cdts)
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
                    context.AddValidationMessage(new ValidationMessage("CDT is source and target of an isEquivalentTo dependency.", "The source of an isEquivalentTo dependency must be a CDT. A CDT which is the source of an isEquivalentTo dependency must not be the target of an isEquivalentTo dependency. Errorneous CDT: " + element.Name, "CDTLibrary", ValidationMessage.errorLevelTypes.ERROR, p.PackageID));
                }
            }


        }




        /// <summary>
        /// A CDT which is the source of an isEquivalentTo dependency must have the same number of supplementary components as the CDT the dependency is targeting.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="d"></param>
        /// <param name="cdts"></param>
        private void checkC534f(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> cdts)
        {


            foreach (KeyValuePair<Int32, EA.Element> e in cdts)
            {
                //The source element of the isEquivalentTo dependency
                EA.Element element = e.Value;
                int count_sup = 0;

                //Count the number of supplementar components
                foreach (EA.Attribute bcc in element.Attributes)
                {
                    if (bcc.Stereotype == UPCC.SUP.ToString())
                    {
                        count_sup++;
                    }
                }

                //Get the targets of the isEquivalentTo dependencies
                foreach (EA.Connector con in element.Connectors)
                {
                    if (con.Stereotype == UPCC.isEquivalentTo.ToString() && con.Type == AssociationTypes.Dependency.ToString() &&
                        con.SupplierID == element.ElementID)
                    {
                        //Get the client element
                        EA.Element client = context.Repository.GetElementByID(con.ClientID);

                        //Is the client a CDT?
                        if (client.Stereotype != UPCC.CDT.ToString())
                        {
                            context.AddValidationMessage(new ValidationMessage("Invalid stereotype found for the supplier of an isEquivalentTo dependency.", "The source of an isEquivalentTo dependency must be a CDT. \nCDT " + element.Name + " is the target of an isEquivalentTo dependency from element " + client.Name + ". The source of the isEquivalentTo dependency has an invalid stereotype.", "CDTLibrary", ValidationMessage.errorLevelTypes.ERROR, p.PackageID));
                            break;
                        }


                        int count_client_sup = 0;

                        //Count the number of SUPs in the client
                        foreach (EA.Attribute bcc in client.Attributes)
                        {
                            if (bcc.Stereotype == UPCC.SUP.ToString())
                            {
                                count_client_sup++;
                            }
                        }

                        if (count_client_sup != count_sup)
                        {
                            context.AddValidationMessage(new ValidationMessage("Invalid number of SUPs found for an isEquivalentTo dependency.", "A CDT which is the source of an isEquivalentTo dependency must have the same number of supplementary components as the CDT the dependency is targeting. \nCDT " + element.Name + " is the target of an isEquivalentTo dependency from CDT " + client.Name + ". The number of SUPs is not equal. " + element.Name + " has " + count_sup + " SUPs and " + client.Name + " has " + count_client_sup + " SUPs.", "CDTLibrary", ValidationMessage.errorLevelTypes.ERROR, p.PackageID));

                        }
                    }
                }



            }
        }


        /// <summary>
        /// A CDT content component shall be the specified CDT content component as defined in the UN/CEFACT Data Type Catalogue.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="cdts"></param>
        private void checkC534g(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> cdts)
        {

            //Not implemented yet since we do not have a valid UN/CEFACT Data Type Catalogue yet
        }




        /// <summary>
        /// The name of the content component shall be “Content”.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="cdts"></param>
        private void checkC534h(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> cdts)
        {


            foreach (KeyValuePair<Int32, EA.Element> e in cdts)
            {
                //The source element of the isEquivalentTo dependency
                EA.Element element = e.Value;

                EA.Attribute cdt = Utility.fetchFirstAttributeFromElement(element, UPCC.CON.ToString());

                if (cdt == null)
                {
                    context.AddValidationMessage(new ValidationMessage("Missing <<CON>> attribute.", "A CDT must contain exactly one attribute stereotyped as <<CON>>.  \nCDT " + element.Name + " does not have an attribute stereotyped as <<CON>>.", "CCLibrary", ValidationMessage.errorLevelTypes.ERROR, p.PackageID));
                }
                else
                {
                    String name = cdt.Name;
                    if (name != "Content")
                    {
                        context.AddValidationMessage(new ValidationMessage("The name of the content component shall be “Content”.", "A CDT must contain exactly one attribute stereotyped as <<CON>>. The name of the <<CON>> attribute must be 'Content'. \nCDT " + element.Name + " does not have an attribute stereotyped as <<CON>> witht the name 'Content'. Wrong name: " + name, "CDTLibrary", ValidationMessage.errorLevelTypes.ERROR, p.PackageID));
                    }

                }


            }
        }


        /// <summary>
        /// The type of CON and SUP attributes shall be a class of stereotype <<PRIM>> or <<ENUM>>.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="cdts"></param>
        private void checkC534i(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> cdts)
        {


            foreach (KeyValuePair<Int32, EA.Element> e in cdts)
            {                
                EA.Element element = e.Value;
             
                //Check every CON and SUP attribute
                foreach (EA.Attribute bcc in element.Attributes)
                {
                    //We do not check whether the attribute has the correct stereotype or not since that has already been checked by a previous constraint
                    int classifier = bcc.ClassifierID;
                    if (classifier == 0)
                    {
                        context.AddValidationMessage(new ValidationMessage("Invalid type of content or supplementary component in a CDT.", "The type of CON and SUP attributes in a CDT shall be a class of stereotype <<PRIM>> or <<ENUM>>. Attribute " + bcc.Name  + " in CDT "+ element.Name+" has an invalid type." , "CCLibrary", ValidationMessage.errorLevelTypes.ERROR, p.PackageID));
                    }
                    else
                    {
                        EA.Element classifyingElement = context.Repository.GetElementByID(classifier);
                        if (!(classifyingElement.Stereotype == UPCC.PRIM.ToString() || classifyingElement.Stereotype == UPCC.ENUM.ToString()))
                        {
                            context.AddValidationMessage(new ValidationMessage("Invalid type of content or supplementary component in a CDT.", "The type of CON and SUP attributes in a CDT shall be a class of stereotype <<PRIM>> or <<ENUM>>. Attribute " + bcc.Name + " in CDT " + element.Name + " has an invalid type.", "CDTLibrary", ValidationMessage.errorLevelTypes.ERROR, p.PackageID));
                        }

                    }
                }

            }
        }


        /// <summary>
        /// A CDT supplementary component shall be one of the specified CDT supplementary components as defined in the UN/CEFACT Data Type Catalogue.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="cdts"></param>
        private void checkC534j(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> cdts)
        {
            //Not implemented yet since we do not have a valid UN/CEFACT Data Type Catalogue yet
        }



        /// <summary>
        /// CDT supplementary component cardinality shall be equal to [0..1] if the CDT supplementary component is optional, or [1..1] if mandatory.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="cdts"></param>
        private void checkC534k(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> cdts)
        {

            foreach (KeyValuePair<Int32, EA.Element> e in cdts)
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
                        context.AddValidationMessage(new ValidationMessage("Invalid cardinality found for a CDT.", "CDT supplementary component cardinality shall be equal to [0..1] if the CDT supplementary component is optional, or [1..1] if mandatory. Attribute " + a.Value.Name + " in CDT " + e.Value.Name + " has an invalid cardinality: lower bound: " + lowerBound + " upper bound " + upperBound + ".", "CDTLibrary", ValidationMessage.errorLevelTypes.ERROR, p.PackageID));
                    }
                }

            }
        }


        /// <summary>
        /// The unique identifier tagged value of a CDT shall be unique for a given CDT library.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="cdts"></param>
        private void checkC534l(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> cdts)
        {

            
            Dictionary<Int32, string> values = new Dictionary<Int32, string>();

            foreach (KeyValuePair<Int32, EA.Element> e in cdts)
            {
                EA.TaggedValue tv = Utility.getTaggedValue(e.Value, UPCC_TV.uniqueIdentifier.ToString());
                if (tv != null)
                {
                    //Has this unique identifier already been used?
                    if (values.ContainsValue(tv.Value))
                    {
                        //Get the other element with the same unique identifier
                        EA.Element duplicateElement = context.Repository.GetElementByID(Utility.findKey(values, tv.Value));

                        context.AddValidationMessage(new ValidationMessage("Two identical unique identifier tagged values of a CDT detected.", "The unique identifier tagged value of a CDT shall be unique for a given CDT library. " + e.Value.Name + " and " + duplicateElement.Name + " have the same unique identifier.", "CDTLibrary", ValidationMessage.errorLevelTypes.ERROR, p.PackageID));
                    }

                    values.Add(e.Value.ElementID, tv.Value);
                }
            }            
        }


        /// <summary>
        /// The dictionary entry name tagged value of a CDT shall be unique for a given CDT library.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="cdts"></param>
        private void checkC534m(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> cdts)
        {

            Dictionary<Int32, string> values = new Dictionary<Int32, string>();

            foreach (KeyValuePair<Int32, EA.Element> e in cdts)
            {
                EA.TaggedValue tv = Utility.getTaggedValue(e.Value, UPCC_TV.dictionaryEntryName.ToString());
                if (tv != null)
                {
                    //Has this unique identifier already been used?
                    if (values.ContainsValue(tv.Value))
                    {
                        //Get the other element with the same unique identifier
                        EA.Element duplicateElement = context.Repository.GetElementByID(Utility.findKey(values, tv.Value));

                        context.AddValidationMessage(new ValidationMessage("Two identical dictionary entry name tagged values of a CDT detected.", "The dictionary entry name tagged value of a CDT shall be unique for a given CDT library. " + e.Value.Name + " and " + duplicateElement.Name + " have the same dictionary entry name.", "CDTLibrary", ValidationMessage.errorLevelTypes.ERROR, p.PackageID));
                    }

                    values.Add(e.Value.ElementID, tv.Value);
                }
            }  
        }


        /// <summary>
        /// The dictionary entry name tagged value of a SUP shall be unique for a given CDT.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="cdts"></param>
        private void checkC534n(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> cdts)
        {


            Dictionary<string, string> values = new Dictionary<string, string>();

            foreach (KeyValuePair<Int32, EA.Element> e in cdts)
            {
                //Fetch all attributes of the given element
                Dictionary<string, EA.Attribute> attributes = Utility.fetchAllAttributesFromElement(e.Value, UPCC.SUP.ToString());

                foreach (KeyValuePair<string, EA.Attribute> a in attributes)
                {
                    
                    EA.AttributeTag tv = Utility.getTaggedValue(a.Value, UPCC_TV.dictionaryEntryName.ToString());
                   
                    if (tv != null)
                    {
                        //Has this unique identifier already been used?
                        if (values.ContainsValue(tv.Value))
                        {
                            //Get the other attribute with the same unique identifier
                            EA.Attribute duplicateAttribute = context.Repository.GetAttributeByGuid(Utility.findKey(values, tv.Value));

                            context.AddValidationMessage(new ValidationMessage("Two identical dictionary entry name tagged values of a SUP in a CDT detected.", "The dictionary entry name tagged value of a SUP shall be unique for a given CDT. The supplementary components " + a.Value.Name + " and " + duplicateAttribute.Name + " of the CDT " + e.Value.Name + " have the same dictionary entry name.", "CDTLibrary", ValidationMessage.errorLevelTypes.ERROR, p.PackageID));
                        }

                        values.Add(a.Value.AttributeGUID, tv.Value);
                    }
                }
                values = new Dictionary<string, string>();
            } 
        }


        /// <summary>
        /// o)	A dictionary entry name tagged value shall not include consecutive identical words.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="cdts"></param>
        private void checkC534o(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> cdts)
        {

        }

        /// <summary>
        /// p)	A dictionary entry name tagged value shall use alphabetic characters plus the dot, underscore, and space characters.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="cdts"></param>
        private void checkC534p(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> cdts)
        {

        }


        /// <summary>
        /// q)	Each word in a dictionary entry name tagged value shall start with a capital letter.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="cdts"></param>
        private void checkC534q(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> cdts)
        {

        }


        /// <summary>
        /// r)	The dictionary entry name tagged value of a CDT shall consist of the CDT’s name, followed by a dot, followed by the term “Type”.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="cdts"></param>
        private void checkC534r(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> cdts)
        {

        }


        /// <summary>
        /// s)	The dictionary entry name tagged value of a CDT content component shall consist of the name of the CDT to which it is assigned, followed by a dot, followed by the term “Content”.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="cdts"></param>
        private void checkC534s(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> cdts)
        {

        }


        /// <summary>
        /// t)	The dictionary entry name tagged value of a supplementary component shall consist of the following parts in the specified order: name of the CDT containing the SUP followed by a dot, followed by the name of the supplementary component, followed by a dot, followed by the name of either the primitive type or the enumeration type setting the value domain of the supplementary component.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="cdts"></param>
        private void checkC534t(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> cdts)
        {

        }


        /// <summary>
        /// u)	The definition tagged value of a CDT shall include the name of the CDT.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="cdts"></param>
        private void checkC534u(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> cdts)
        {

        }


        /// <summary>
        /// v)	The definition tagged value of a CON shall include the name of the primitive type of the CON.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="cdts"></param>
        private void checkC534v(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> cdts)
        {

        }


        /// <summary>
        /// w)	The definition tagged value of a SUP shall include the name of the CDT containing the SUP, the name of the SUP itself and the name of the primitive type or enumeration type setting the value of the SUP.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="cdts"></param>
        private void checkC534w(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> cdts)
        {

        }







    }
}
