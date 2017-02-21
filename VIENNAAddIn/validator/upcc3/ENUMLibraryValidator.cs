using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VIENNAAddIn.constants;
using VIENNAAddIn.common;

namespace VIENNAAddIn.validator.upcc3
{
    class ENUMLibraryValidator : AbstractValidator
    {






        /// <summary>
        /// Validate the given ENUMLibrary
        /// </summary>
        /// <param name="context"></param>
        /// <param name="scope"></param>
        internal override void validate(IValidationContext context, string scope)
        {

            EA.Package package = context.Repository.GetPackageByID(Int32.Parse(scope));

            //Get all ENUMs from the given package
            Dictionary<Int32, EA.Element> enums = new Dictionary<int, EA.Element>();
            Utility.getAllElements(package, enums, UPCC.ENUM.ToString());

            checkC514g(context, package);

            checkC514o(context, package);

            checkC554a(context, package, enums);

            checkC554b(context, package, enums);

            checkC554c(context, package, enums);

            checkC554d(context, package, enums);

            checkC554e(context, package, enums);

            checkC554f(context, package, enums);

            checkC554g(context, package, enums);

        }




        /// <summary>
        /// A ENUMLibrary shall only contrain ENUMs
        /// </summary>
        /// <param name="context"></param>
        /// <param name="brv"></param>
        private void checkC514g(IValidationContext context, EA.Package d)
        {
            foreach (EA.Element e in d.Elements)
            {
                if (e.Type != EA_Element.Note.ToString())
                {

                    String stereotype = e.Stereotype;
                    if (stereotype == null || !stereotype.Equals(UPCC.ENUM.ToString()))
                        context.AddValidationMessage(new ValidationMessage("Invalid element found in ENUMLibrary.", "The element " + e.Name + " has an invalid stereotype (" + stereotype + "). A ENUMLibrary shall only contain ENUMs.", "ENUMLibrary", ValidationMessage.errorLevelTypes.ERROR, d.PackageID));
                }
            }
        }



        /// <summary>
        /// A ENUMLibrary must not contain any sub packages
        /// </summary>
        /// <param name="context"></param>
        /// <param name="d"></param>
        private void checkC514o(IValidationContext context, EA.Package d)
        {
            foreach (EA.Package subPackage in d.Packages)
            {
                context.AddValidationMessage(new ValidationMessage("Invalid package found in ENUMLibrary.", "A ENUMLibrary must not contain any sub packages. Please remove sub package " + subPackage.Name, "ENUMLibrary", ValidationMessage.errorLevelTypes.ERROR, d.PackageID));
            }

        }


        /// <summary>
        /// a)	The name of an enumeration type shall be unique for a given ENUM library.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="package"></param>
        /// <param name="enums"></param>
        private void checkC554a(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> enums)
        {

            Dictionary<Int32, string> names = new Dictionary<int, string>();

            foreach (KeyValuePair<Int32, EA.Element> e in enums)
            {

                if (names.ContainsValue(e.Value.Name))
                {
                    context.AddValidationMessage(new ValidationMessage("Two ENUMs with the same name found.", "The name of an enumeration type shall be unique for a given ENUM library. Two ENUMs with the name " + e.Value.Name + " found in ENUMLibrary " + p.Name + ".", "ENUMLibrary", ValidationMessage.errorLevelTypes.ERROR, p.PackageID));
               }

                names.Add(e.Value.ElementID, e.Value.Name);
            }

        }

        /// <summary>
        /// b)	An enumeraton type shall contain one or more code values.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="package"></param>
        /// <param name="enums"></param>
        private void checkC554b(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> enums)
        {


            
            foreach (KeyValuePair<Int32, EA.Element> e in enums)            
            {
                //Code values are in fact just attributes of a class
                //Check whether the given enum has at least one attribute
                int count = 0;
                foreach (EA.Attribute a in e.Value.Attributes)
                {
                    count++;
                }

                if (count < 1)
                {
                    context.AddValidationMessage(new ValidationMessage("Missing code value in an ENUM found.", "An enumeraton type shall contain one or more code values. ENUM " + e.Value.Name + " found in ENUMLibrary " + p.Name + " does not have any code values..", "ENUMLibrary", ValidationMessage.errorLevelTypes.ERROR, p.PackageID));
                }
            }


        }


        /// <summary>
        /// c)	An enumeration type shall either have a tagged value agencyName or agencyIdentifier.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="package"></param>
        /// <param name="enums"></param>
        private void checkC554c(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> enums)
        {

            foreach (KeyValuePair<Int32, EA.Element> e in enums)
            {
                //Check for agencyName or agencyIdentifier tagged value
                EA.TaggedValue agencyName = Utility.getTaggedValue(e.Value, UPCC_TV.agencyName.ToString());
                EA.TaggedValue agencyIdentifier = Utility.getTaggedValue(e.Value, UPCC_TV.agencyIdentifier.ToString());

                if ((agencyName == null || agencyName.Value == "") && (agencyIdentifier == null || agencyIdentifier.Value == "")) {
                    context.AddValidationMessage(new ValidationMessage("Missing agencyIdentifier/agencyName found in ENUM.", "An enumeration type shall either have a tagged value agencyName or agencyIdentifier. ENUM " + e.Value.Name + " found in ENUMLibrary " + p.Name + " does neither have an agencyName tagged value nor an agencyIdentifier tagged value.", "ENUMLibrary", ValidationMessage.errorLevelTypes.ERROR, p.PackageID));
                }

            }


        }


        /// <summary>
        /// The source of an isEquivalentTo dependency must be an ENUM. 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="enums"></param>
        private void checkC554d(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> enums)
        {

            foreach (KeyValuePair<Int32, EA.Element> e in enums)
            {
                //The target element of the isEquivalentTo dependency
                EA.Element element = e.Value;
                
                foreach (EA.Connector con in element.Connectors)
                {

                    //Is the element source of an isEquivalentTo dependency?
                    if (con.Type == AssociationTypes.Dependency.ToString() && con.Stereotype == UPCC.isEquivalentTo.ToString())
                    {
                        if (con.SupplierID == element.ElementID)
                        {
                            EA.Element source = context.Repository.GetElementByID(con.ClientID);
                            String sourceStereotype = source.Stereotype;

                            if (sourceStereotype != UPCC.ENUM.ToString())
                            {
                                if (sourceStereotype == "")
                                {
                                    sourceStereotype = "No stereotype specified";
                                }


                                context.AddValidationMessage(new ValidationMessage("Wrong stereotype of isEquivalentTo dependency source element.", "The source of an isEquivalentTo dependency must be an ENUM. ENUM " + source.Name + " found in ENUMLibrary " + p.Name + " is the source of an isEquivalentTo dependency, but has the wrong stereotype: " + sourceStereotype + ".", "ENUMLibrary", ValidationMessage.errorLevelTypes.ERROR, p.PackageID));
                            }
                        }                
                    }
                }

            }

            
        }

        
        /// <summary>
        /// An ENUM which is the source of an isEquivalentTo dependency shall not be the target of an isEquivalentTo dependency.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="package"></param>
        /// <param name="enums"></param>
        private void checkC554e(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> enums)
        {
            foreach (KeyValuePair<Int32, EA.Element> e in enums)
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
                    context.AddValidationMessage(new ValidationMessage("ENUM is source and target of an isEquivalentTo dependency.", "An ENUM which is the source of an isEquivalentTo dependency shall not be the target of an isEquivalentTo dependency. Errorneous ENUM: " + element.Name, "ENUMLibrary", ValidationMessage.errorLevelTypes.ERROR, p.PackageID));
                }
            }

        }

        /// <summary>
        /// e)	The unique identifier tagged value of an ENUM shall be unique for a given ENUM library.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="package"></param>
        /// <param name="enums"></param>
        private void checkC554f(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> enums)
        {


            Dictionary<Int32, string> values = new Dictionary<Int32, string>();

            foreach (KeyValuePair<Int32, EA.Element> e in enums)
            {
                EA.TaggedValue tv = Utility.getTaggedValue(e.Value, UPCC_TV.uniqueIdentifier.ToString());
                if (tv != null)
                {
                    //Has this unique identifier already been used?
                    if (values.ContainsValue(tv.Value))
                    {
                        //Get the other element with the same unique identifier
                        EA.Element duplicateElement = context.Repository.GetElementByID(Utility.findKey(values, tv.Value));

                        context.AddValidationMessage(new ValidationMessage("Two identical unique identifier tagged values of an ENUM detected.", "The unique identifier tagged value of a ENUM shall be unique for a given ENUM library. " + e.Value.Name + " and " + duplicateElement.Name + " have the same unique identifier.", "ENUMLibrary", ValidationMessage.errorLevelTypes.ERROR, p.PackageID));
                    }

                    values.Add(e.Value.ElementID, tv.Value);
                }
            }  

        }


        /// <summary>
        /// f)	The dictionary entry name tagged value of an ENUM shall be unique for a given ENUM library.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="package"></param>
        /// <param name="enums"></param>
        private void checkC554g(IValidationContext context, EA.Package p, Dictionary<Int32, EA.Element> enums)
        {

            Dictionary<Int32, string> values = new Dictionary<Int32, string>();

            foreach (KeyValuePair<Int32, EA.Element> e in enums)
            {
                EA.TaggedValue tv = Utility.getTaggedValue(e.Value, UPCC_TV.dictionaryEntryName.ToString());
                if (tv != null)
                {
                    //Has this unique identifier already been used?
                    if (values.ContainsValue(tv.Value))
                    {
                        //Get the other element with the same unique identifier
                        EA.Element duplicateElement = context.Repository.GetElementByID(Utility.findKey(values, tv.Value));

                        context.AddValidationMessage(new ValidationMessage("Two identical dictionary entry name tagged values of an ENUM detected.", "The dictionary entry name tagged value of a ENUM shall be unique for a given ENUM library. " + e.Value.Name + " and " + duplicateElement.Name + " have the same dictionary entry name.", "ENUMLibrary", ValidationMessage.errorLevelTypes.ERROR, p.PackageID));
                    }

                    values.Add(e.Value.ElementID, tv.Value);
                }
            }  

        }






    }
}
