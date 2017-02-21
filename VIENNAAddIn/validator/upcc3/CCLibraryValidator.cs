using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VIENNAAddIn.constants;
using VIENNAAddIn.common;
using System.Collections;

namespace VIENNAAddIn.validator.upcc3
{
    class CCLibraryValidator : AbstractValidator
    {





        /// <summary>
        /// Validate the given CCLibrary
        /// </summary>
        /// <param name="context"></param>
        /// <param name="scope"></param>
        internal override void validate(IValidationContext context, string scope)
        {

            EA.Package package = context.Repository.GetPackageByID(Int32.Parse(scope));

            //Get all ACCs from the given package
            Dictionary<Int32, EA.Element> accs = new Dictionary<int, EA.Element>();
            Utility.getAllElements(package, accs, UPCC.ACC.ToString());

            checkC514d(context, package);

            checkC514k(context, package);

            checkC524a(context, package);

            checkC524b(context, package, accs);

            checkC524c(context, package, accs);

            checkC524d(context, package, accs);

            checkC524e(context, package, accs);

            checkC524f(context, package, accs);

            checkC524g(context, package, accs);

            checkC524h(context, package, accs);

            checkC524i(context, package, accs);

            checkC524j(context, package, accs);

            //checkC524k(context, package, accs);

            checkC524l(context, package, accs);

            checkC524m(context, package, accs);

            checkC524n(context, package, accs);

            checkC524o(context, package, accs);

            checkC524p(context, package, accs);

            checkC524q(context, package, accs);

            checkC524r(context, package, accs);

            checkC524s(context, package, accs);

            checkC524t(context, package, accs);

            checkC524u(context, package, accs);

            checkC524v(context, package, accs);

            checkC524w(context, package, accs);

            checkC524x(context, package, accs);

        }



        /// <summary>
        /// A CCLibrary shall only ACCs, BCCs, and ASCCs.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="brv"></param>
        private void checkC514d(IValidationContext context, EA.Package d)
        {
            foreach (EA.Element e in d.Elements)
            {
                if (e.Type != EA_Element.Note.ToString())
                {

                    String stereotype = e.Stereotype;
                    if (stereotype == null || !stereotype.Equals(UPCC.ACC.ToString()))
                        context.AddValidationMessage(new ValidationMessage("Invalid element found in CCLibrary.", "The element " + e.Name + " has an invalid stereotype (" + stereotype + "). A CCLibrary shall only contain ACCs, BCCs, and ASCCs.", "CCLibrary", ValidationMessage.errorLevelTypes.ERROR, d.PackageID));
                }
            }
        }



        /// <summary>
        /// A CCLibrary must not contain any sub packages
        /// </summary>
        /// <param name="context"></param>
        /// <param name="d"></param>
        private void checkC514k(IValidationContext context, EA.Package d)
        {
            foreach (EA.Package subPackage in d.Packages)
            {
                context.AddValidationMessage(new ValidationMessage("Invalid package found in CCLibrary.", "A CCLibrary must not contain any sub packages. Please remove sub package " + subPackage.Name, "CCLibrary", ValidationMessage.errorLevelTypes.ERROR, d.PackageID));
            }

        }


        /// <summary>
        /// An ACC shall not contain - directly or at any nested level - a mandatory ASCC whose associated ACC is the same as the top level ACC.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        private void checkC524a(IValidationContext context, EA.Package p)
        {

            //TO DO 
            //Implement a DFS algorithm for cycle identification
            

        }


        /// <summary>
        /// An ACC shall contain only BCCs and ASCCs. There shall be at least one BCC or at least one ASCC.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        private void checkC524b(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> accs)
        {


            foreach (KeyValuePair<Int32, EA.Element> e in accs)
            {
                EA.Element element = e.Value;
                int count_ascc = 0;
                int count_bcc  = 0;

                foreach (EA.Connector con in element.Connectors)
                {
                    if (con.Stereotype == UPCC.ASCC.ToString() && con.ClientID == element.ElementID && (con.Type == AssociationTypes.Association.ToString() || con.Type == AssociationTypes.Aggregation.ToString()))
                    {
                        count_ascc++;
                    }
                }

                foreach (EA.Attribute bcc in element.Attributes)
                {
                    if (bcc.Stereotype == UPCC.BCC.ToString())
                    {
                        count_bcc++;
                    }
                    else
                    {
                        context.AddValidationMessage(new ValidationMessage("An ACC shall contain only BCCs and ASCCs.", "An ACC shall contain only BCCs and ASCCs. There shall be at least one BCC or at least one ASCC. \nACC " + element.Name + " has an attribute which is not stereotyped as BCC.", "CCLibrary", ValidationMessage.errorLevelTypes.ERROR, p.PackageID));
                    }
                }


                if (count_ascc == 0 && count_bcc == 0)
                {
                    context.AddValidationMessage(new ValidationMessage("No ASCCs/BCCs found for an ACC.", "An ACC shall contain only BCCs and ASCCs. There shall be at least one BCC or at least one ASCC. \nACC " + element.Name + " has neither any ASCCs nor any BCCs.", "CCLibrary", ValidationMessage.errorLevelTypes.ERROR, p.PackageID));
                }

            }

        }


        /// <summary>
        /// An ACC which is the source of an isEquivalentTo dependency shall have the same number of BCCs and ASCCs as the ACC the dependency is targeting.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        private void checkC524c(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> accs)
        {


            foreach (KeyValuePair<Int32, EA.Element> e in accs)
            {
                //The source element of the isEquivalentTo dependency
                EA.Element element = e.Value;
                int count_ascc = 0;
                int count_bcc = 0;

                foreach (EA.Connector con in element.Connectors)
                {

                    if (con.Stereotype == UPCC.ASCC.ToString() && con.SupplierID == element.ElementID && (con.Type == AssociationTypes.Association.ToString() || con.Type == AssociationTypes.Aggregation.ToString()))
                    {
                        count_ascc++;
                    }
                }

                foreach (EA.Attribute bcc in element.Attributes)
                {
                    if (bcc.Stereotype == UPCC.BCC.ToString())
                    {
                        count_bcc++;
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

                        //Is the client an ACC?
                        if (client.Stereotype != UPCC.ACC.ToString())
                        {
                            context.AddValidationMessage(new ValidationMessage("Invalid stereotype found for the supplier of an isEquivalentTo dependency.", "The source of an isEquivalentTo dependeny must be an ACC. An ACC which is the source of an isEquivalentTo dependency shall have the same number of BCCs and ASCCs as the ACC the dependency is targeting. \nACC " + element.Name + " is the target of an isEquivalentTo dependency from element " + client.Name + ". The source of the isEquivalentTo dependency has an invalid stereotype." , "CCLibrary", ValidationMessage.errorLevelTypes.ERROR, p.PackageID));
                            break;
                        }


                        int count_client_ascc = 0;
                        int count_client_bcc = 0;

                        //The client must be an ACC as well and must have the same number of ASCCs and BCCs as the target (the original ACC)
                        foreach (EA.Connector clientcon in client.Connectors)
                        {
                            if (clientcon.Stereotype == UPCC.ASCC.ToString() && clientcon.SupplierID == client.ElementID && (clientcon.Type == AssociationTypes.Association.ToString() || clientcon.Type == AssociationTypes.Aggregation.ToString()))
                            {
                                count_client_ascc++;
                            }
                        }

                        foreach (EA.Attribute bcc in client.Attributes)
                        {
                            if (bcc.Stereotype == UPCC.BCC.ToString())
                            {
                                count_client_bcc++;
                            }
                        }

                        if (count_client_ascc != count_ascc)
                        {
                            context.AddValidationMessage(new ValidationMessage("Invalid number of ASCCs found for an isEquivalentTo dependency.", "The source of an isEquivalentTo dependeny must be an ACC. An ACC which is the source of an isEquivalentTo dependency shall have the same number of BCCs and ASCCs as the ACC the dependency is targeting. \nACC " + element.Name + " is the target of an isEquivalentTo dependency from ACC " + client.Name + ". The number of ASCCs is not equal. " + element.Name + " has " + count_ascc + " ASCCs and " + client.Name + " has " + count_client_ascc + " ASCCs.", "CCLibrary", ValidationMessage.errorLevelTypes.ERROR, p.PackageID));

                        }

                        if (count_client_bcc != count_bcc)
                        {
                            context.AddValidationMessage(new ValidationMessage("Invalid number of ASCCs found for an isEquivalentTo dependency.", "The source of an isEquivalentTo dependeny must be an ACC. An ACC which is the source of an isEquivalentTo dependency shall have the same number of BCCs and ASCCs as the ACC the dependency is targeting. \nACC " + element.Name + " is the target of an isEquivalentTo dependency from ACC " + client.Name + ". The number of BCCs is not equal. ", "CCLibrary", ValidationMessage.errorLevelTypes.ERROR, p.PackageID));
                        }


                    }
                }




            }



        }



        /// <summary>
        /// An ACC which is the source of an isEquivalentTo dependency shall not be the target of an isEquivalentTo dependency.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="accs"></param>
        private void checkC524d(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> accs)
        {

            foreach (KeyValuePair<Int32, EA.Element> e in accs)
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
                    context.AddValidationMessage(new ValidationMessage("ACC is source and target of an isEquivalentTo dependeny.", "An ACC which is the source of an isEquivalentTo dependency shall not be the target of an isEquivalentTo dependency. Errorneous ACC: " + element.Name, "CCLibrary", ValidationMessage.errorLevelTypes.ERROR, p.PackageID));
                }                
            }
        }



        /// <summary>
        /// For a given CCLibrary there shall not be two ACCs with the same dictionary entry name.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="accs"></param>
        private void checkC524e(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> accs)
        {
            Dictionary<Int32, string> values = new Dictionary<Int32, string>();

            foreach (KeyValuePair<Int32, EA.Element> e in accs)
            {
                EA.TaggedValue tv = Utility.getTaggedValue(e.Value, UPCC_TV.dictionaryEntryName.ToString());
                if (tv != null)
                {
                    //Has this dictionary entry name already been used?
                    if (values.ContainsValue(tv.Value))
                    {
                        //Get the other element with the same dictionary entry name
                        EA.Element duplicateDENElement = context.Repository.GetElementByID(Utility.findKey(values, tv.Value));

                        context.AddValidationMessage(new ValidationMessage("Two identical dictionary entry names of an ACC detected.", "For a given CCLibrary there shall not be two ACCs with the same dictionary entry name. " + e.Value.Name + " and " + duplicateDENElement.Name + " have the same dictionary entry name.", "CCLibrary", ValidationMessage.errorLevelTypes.ERROR, p.PackageID));
                    }                    
                    
                    values.Add(e.Value.ElementID, tv.Value);
                }
            }
        }



        /// <summary>
        /// For a given CCLibrary there shall not be two ACCs with the same unique identifier tagged value.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="accs"></param>
        private void checkC524f(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> accs)
        {
            Dictionary<Int32, string> values = new Dictionary<Int32, string>();

            foreach (KeyValuePair<Int32, EA.Element> e in accs)
            {
                EA.TaggedValue tv = Utility.getTaggedValue(e.Value, UPCC_TV.dictionaryEntryName.ToString());
                if (tv != null)
                {
                    //Has this unique identifier already been used?
                    if (values.ContainsValue(tv.Value))
                    {
                        //Get the other element with the same unique identifier
                        EA.Element duplicateElement = context.Repository.GetElementByID(Utility.findKey(values, tv.Value));

                        context.AddValidationMessage(new ValidationMessage("Two identical unique identifier tagged values of an ACC detected.", "For a given CCLibrary there shall not be two ACCs with the same unique identifier tagged value. " + e.Value.Name + " and " + duplicateElement.Name + " have the same unique identifier.", "CCLibrary", ValidationMessage.errorLevelTypes.ERROR, p.PackageID));
                    }

                    values.Add(e.Value.ElementID, tv.Value);
                }
            }


        }


        /// <summary>
        ///  A BCC shall be typed with a class of stereotype <<CDT>>.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="accs"></param>
        private void checkC524g(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> accs)
        {

            foreach (KeyValuePair<Int32, EA.Element> e in accs)
            {
                foreach (EA.Attribute bcc in e.Value.AttributesEx)
                {
                    //Is there a classifier for the attribute?
                    int i = bcc.ClassifierID;
                    if (i == 0)
                    {
                        context.AddValidationMessage(new ValidationMessage("BCC with invalid type detected.", "A BCC shall be typed with a class of stereotype <<CDT>>. BCC " + bcc.Name + " of ACC " + e.Value.Name + " has an invalid type.", "CCLibrary", ValidationMessage.errorLevelTypes.ERROR, p.PackageID));
                    }
                    else
                    {
                        EA.Element classifier = context.Repository.GetElementByID(bcc.ClassifierID);
                        if (classifier.Stereotype != UPCC.CDT.ToString())
                        {
                            context.AddValidationMessage(new ValidationMessage("BCC with invalid type detected.", "A BCC shall be typed with a class of stereotype <<CDT>>. BCC " + bcc.Name + " of ACC " + e.Value.Name + " has an invalid type.", "CCLibrary", ValidationMessage.errorLevelTypes.ERROR, p.PackageID));
                        }
                    }
                }


            }
        }



        /// <summary>
        /// For a given ACC there shall not be two BCCs with the same dictionary entry name.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="accs"></param>
        private void checkC524h(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> accs)
        {

            Dictionary<string, string> values = new Dictionary<string, string>();

            foreach (KeyValuePair<Int32, EA.Element> e in accs)
            {
                foreach (EA.Attribute bcc in e.Value.AttributesEx)
                {                    
                    EA.AttributeTag tv = Utility.getTaggedValue(bcc, UPCC_TV.dictionaryEntryName.ToString());
                    if (tv != null)
                    {
                        //Has this dictionary entry name already been used?
                        if (values.ContainsValue(tv.Value))
                        {
                            //Get the other element with the same dictionary entry name
                            EA.Attribute duplicateDENAttribute = context.Repository.GetAttributeByGuid(Utility.findKey(values, tv.Value));

                            context.AddValidationMessage(new ValidationMessage("Two identical dictionary entry names of a BCC detected.", "For a given ACC there shall not be two BCCs with the same dictionary entry name. " + bcc.Name + " and " + duplicateDENAttribute.Name + " of ACC " + e.Value.Name + " have the same dictionary entry name.", "CCLibrary", ValidationMessage.errorLevelTypes.ERROR, p.PackageID));
                        }

                        values.Add(bcc.AttributeGUID, tv.Value);
                    }
                }
                values = new Dictionary<string, string>();
            }


        }


        /// <summary>
        /// For a given ACC there shall not be two BCCs with the same unique identifier tagged value.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="accs"></param>
        private void checkC524i(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> accs)
        {

            Dictionary<string, string> values = new Dictionary<string, string>();

            foreach (KeyValuePair<Int32, EA.Element> e in accs)
            {
                foreach (EA.Attribute bcc in e.Value.AttributesEx)
                {
                    EA.AttributeTag tv = Utility.getTaggedValue(bcc, UPCC_TV.uniqueIdentifier.ToString());
                    if (tv != null)
                    {
                        //Has this unique identifier already been used?
                        if (values.ContainsValue(tv.Value))
                        {
                            //Get the other element with the same unique identifier
                            EA.Attribute duplicateDENAttribute = context.Repository.GetAttributeByGuid(Utility.findKey(values, tv.Value));

                            context.AddValidationMessage(new ValidationMessage("Two identical unique identifiers of a BCC detected.", "For a given ACC there shall not be two BCCs with the same unique identifier tagged value. " + bcc.Name + " and " + duplicateDENAttribute.Name + " of ACC " + e.Value.Name + " have the same unique identifier.", "CCLibrary", ValidationMessage.errorLevelTypes.ERROR, p.PackageID));
                        }

                        values.Add(bcc.AttributeGUID, tv.Value);
                    }
                }
                values = new Dictionary<string, string>();
            }


        }



        /// <summary>
        /// All ASCCs of a given ACC shall have unique names.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="accs"></param>
        private void checkC524j(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> accs)
        {

            Dictionary<string, string> values = new Dictionary<string, string>();

            foreach (KeyValuePair<Int32, EA.Element> e in accs)
            {
                //Get all ASCCs
                foreach (EA.Connector con in e.Value.Connectors)
                {
                    //Get all emanating ASCCs of this ACC
                    if (con.Stereotype == UPCC.ASCC.ToString() && con.ClientID == e.Value.ElementID)
                    {
                        //Get the role name of the ASCC
                        String rolename = con.SupplierEnd.Role;

                        //Has this role name already been used?
                        if (values.ContainsValue(rolename))
                        {
                            if (rolename == null || rolename == "")
                                rolename = "No rolename has been specified for two or more ASCCs.";

                            context.AddValidationMessage(new ValidationMessage("Duplicate ASCC name detected.", "All ASCCs of a given ACC shall have unique names. ACC " + e.Value.Name + " has two ASCCs with the identical name: " + rolename, "CCLibrary", ValidationMessage.errorLevelTypes.ERROR, p.PackageID));
                        }
                        values.Add(rolename, rolename);
                    }

                }
                values = new Dictionary<string, string>();
            }


        }



        /// <summary>
        /// The originating association end of an assocation stereotyped as ASCC shall be set to aggregationKind::shared.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="accs"></param>
        private void checkC524k(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> accs)
        {

            foreach (KeyValuePair<Int32, EA.Element> e in accs)
            {
                //Get all ASCCs
                foreach (EA.Connector con in e.Value.Connectors)
                {
                    //Get all emanating ASCCs of this ACC
                    if (con.Stereotype == UPCC.ASCC.ToString() && con.SupplierID == e.Value.ElementID)
                    {
                        //Check the supplier end
                        //0 = none
                        //1 = shared
                        //2 = composite
                        int connectorEnd = con.SupplierEnd.Aggregation;
                                                     
                        if (!(connectorEnd == 1 || connectorEnd == 2))
                        {
                            context.AddValidationMessage(new ValidationMessage("Invalid ASCC assocation end.", "The originating association end of an assocation stereotyped as ASCC shall be set to aggregationKind::shared. An ASCC emanating from the ACC " + e.Value.Name + " has an invalid assocation end.", "CCLibrary", ValidationMessage.errorLevelTypes.ERROR, p.PackageID));
                        }
                    }

                }


            }
        }



        /// <summary>
        /// A BCC name and an ASCC name shall not be identical when used in an ACC.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="accs"></param>
        private void checkC524l(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> accs)
        {

            Dictionary<string, string> names = new Dictionary<string, string>();

            foreach (KeyValuePair<Int32, EA.Element> e in accs)
            {
                //Get all ASCCs
                foreach (EA.Connector con in e.Value.Connectors)
                {
                    //Get all emanating ASCCs of this ACC
                    if (con.Stereotype == UPCC.ASCC.ToString() && con.ClientID == e.Value.ElementID)
                    {
                        //Get the role name of the ASCC
                        String rolename = con.SupplierEnd.Role;


                        if (names.ContainsValue(rolename))
                        {
                            if (rolename == null || rolename == "")
                            {
                                rolename = "No name specified.";
                            }

                            context.AddValidationMessage(new ValidationMessage("Duplicate names of BCCs/ASCCs found.", "A BCC name and an ASCC name shall not be identical when used in an ACC. ACC " + e.Value.Name + " has a BBIE and an ASCC with the same name: " + rolename, "CCLibrary", ValidationMessage.errorLevelTypes.ERROR, p.PackageID));
                        }
                        names.Add(rolename, rolename);

                    }
                }

                //Get all BCCs
                foreach (EA.Attribute bcc in e.Value.Attributes)
                {

                    if (names.ContainsValue(bcc.Name))
                    {
                        context.AddValidationMessage(new ValidationMessage("Duplicate names of BCCs/ASCCs found.", "A BCC name and an ASCC name shall not be identical when used in an ACC. ACC " + e.Value.Name + " has a BBIE and an ASCC with the same name: " + bcc.Name, "CCLibrary", ValidationMessage.errorLevelTypes.ERROR, p.PackageID));
                    }
                    names.Add(bcc.Name, bcc.Name);

                }
                names = new Dictionary<string, string>();

            }


        }


        /// <summary>
        /// For a given CCLibrary there shall not be two ASCCs with the same dictionary entry name.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="accs"></param>
        private void checkC524m(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> accs)
        {


            Dictionary<Int32, string> names = new Dictionary<Int32, string>();

            foreach (KeyValuePair<Int32, EA.Element> e in accs)
            {
                //Get all ASCCs
                foreach (EA.Connector con in e.Value.Connectors)
                {
                    //Get all emanating ASCCs of this ACC
                    if (con.Stereotype == UPCC.ASCC.ToString() && con.SupplierID == e.Value.ElementID)
                    {

                       
                        EA.IConnectorTag tv = Utility.getTaggedValue(con, UPCC_TV.dictionaryEntryName.ToString());
                        if (names.ContainsValue(tv.Value.ToString()))
                        {
                            //We have the connector ID  - the ACC is the supplier
                            EA.Connector _con = context.Repository.GetConnectorByID(Utility.findKey(names, tv.Value.ToString()));
                            
                            EA.Element duplicateACC = context.Repository.GetElementByID(_con.SupplierID);
                                

                            String den = tv.Value.ToString();
                            if (den == null || den == "") 
                                den = "No name specified.";

                            context.AddValidationMessage(new ValidationMessage("Duplicate dictionary entry names of ASCCs found.", "For a given CCLibrary there shall not be two ASCCs with the same dictionary entry name. ACC " + e.Value.Name + " and ACC " + duplicateACC.Name + "  both have an ASCC with the same dictionary entry name value: " + den, "CCLibrary", ValidationMessage.errorLevelTypes.ERROR, p.PackageID));                                                                                    
                        }
                        names.Add(con.ConnectorID, tv.Value.ToString());                        
                    }
                }
            }
        }



        /// <summary>
        /// For a given CCLibrary there shall not be two ASCCs with the same unique identifier tagged value.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="accs"></param>
        private void checkC524n(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> accs)
        {


            Dictionary<Int32, string> names = new Dictionary<Int32, string>();

            foreach (KeyValuePair<Int32, EA.Element> e in accs)
            {
                //Get all ASCCs
                foreach (EA.Connector con in e.Value.Connectors)
                {
                    //Get all emanating ASCCs of this ACC
                    if (con.Stereotype == UPCC.ASCC.ToString() && con.SupplierID == e.Value.ElementID)
                    {


                        EA.IConnectorTag tv = Utility.getTaggedValue(con, UPCC_TV.uniqueIdentifier.ToString());
                        if (names.ContainsValue(tv.Value.ToString()))
                        {

                            EA.Connector _con = context.Repository.GetConnectorByID(Utility.findKey(names, tv.Value.ToString()));

                            EA.Element duplicateACC = context.Repository.GetElementByID(_con.SupplierID);

                            String den = tv.Value.ToString();
                            if (den == null || den == "")
                                den = "No name specified.";

                            context.AddValidationMessage(new ValidationMessage("Duplicate unique identifier of ASCCs found.", "For a given CCLibrary there shall not be two ASCCs with the same unique identifier tagged value. ACC " + e.Value.Name + " and ACC " + duplicateACC.Name + "  both have an ASCC with the same unique identifier value: " + den, "CCLibrary", ValidationMessage.errorLevelTypes.ERROR, p.PackageID));
                        }
                        names.Add(con.ConnectorID, tv.Value.ToString());
                    }
                }
            }


        }


        /// <summary>
        /// o)	A dictionary entry name tagged value of an ACC/BCC/ASCC shall not include consecutive identical words or terms.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="accs"></param>
        private void checkC524o(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> accs)
        {


        }


        /// <summary>
        /// p)	A dictionary entry name tagged value of an ACC/BCC/ASCC shall only use alphabetic character plus the dot and space characters.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="accs"></param>
        private void checkC524p(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> accs)
        {


        }


        /// <summary>
        /// q)	Each word in a dictionary entry name tagged value of an ACC/BCC/ASCC shall start with a capital letter.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="accs"></param>
        private void checkC524q(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> accs)
        {


        }


        /// <summary>
        /// r)	The dictionary entry name of an ACC shall consist of the ACC’s name followed by a dot and the term “Details”.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="accs"></param>
        private void checkC524r(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> accs)
        {


        }


        /// <summary>
        /// s)	The dictionary entry name of an ASCC shall consist of the name of the associating ACC, followed by a dot and the assocation’s role name, followed by a dot, followed by the name of the associated ACC.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="accs"></param>
        private void checkC524s(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> accs)
        {


        }


        /// <summary>
        /// t)	The dictionary entry name of a BCC shall consist of the name of the ACC owning the corresponding BCC, followed by a dot and the name of the BCC, followed by a dot and the data type of the BCC.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="accs"></param>
        private void checkC524t(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> accs)
        {


        }


        /// <summary>
        /// u)	The text of a definition tagged value of an ACC/BCC/ASCC should start with the dictionary entry name followed by “is”.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="accs"></param>
        private void checkC524u(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> accs)
        {


        }


        /// <summary>
        /// v)	The definition tagged value of an ACC shall include the name of the ACC.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="accs"></param>
        private void checkC524v(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> accs)
        {


        }


        /// <summary>
        /// w)	The definition tagged value of a BCC shall include the name of the ACC to which it belongs, the name of the BCC, and the name of the business data type.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="accs"></param>
        private void checkC524w(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> accs)
        {


        }


        /// <summary>
        /// x)	The definition tagged value of an ASCC shall include the name of the associating ACC, the ASCC’s role name and the name of the associated ACC.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="accs"></param>
        private void checkC524x(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> accs)
        {


        }









    }
}
