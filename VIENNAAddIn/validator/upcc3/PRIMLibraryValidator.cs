using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VIENNAAddIn.constants;
using VIENNAAddIn.common;

namespace VIENNAAddIn.validator.upcc3
{
    class PRIMLibraryValidator : AbstractValidator
    {





        /// <summary>
        /// Validate the given PRIMLibrary
        /// </summary>
        /// <param name="context"></param>
        /// <param name="scope"></param>
        internal override void validate(IValidationContext context, string scope)
        {

            EA.Package package = context.Repository.GetPackageByID(Int32.Parse(scope));

            //Get all PRIMs from the given package
            Dictionary<Int32, EA.Element> prims = new Dictionary<int, EA.Element>();
            Utility.getAllElements(package, prims, UPCC.PRIM.ToString());
            
            checkC514h(context, package);
            
            checkC514n(context, package);

            checkC544a(context, package, prims);

            checkC544b(context, package, prims);

            checkC544c(context, package, prims);

            checkC544d(context, package, prims);

        }



        /// <summary>
        /// A PRIMLibrary shall only contain PRIMs
        /// </summary>
        /// <param name="context"></param>
        /// <param name="brv"></param>
        private void checkC514h(IValidationContext context, EA.Package d)
        {
            foreach (EA.Element e in d.Elements)
            {
                if (e.Type != EA_Element.Note.ToString())
                {

                    String stereotype = e.Stereotype;
                    if (stereotype == null || !stereotype.Equals(UPCC.PRIM.ToString()))
                        context.AddValidationMessage(new ValidationMessage("Invalid element found in PRIMLibrary.", "The element " + e.Name + " has an invalid stereotype (" + stereotype + "). A PRIMLibrary shall only contain PRIMs.", "PRIMLibrary", ValidationMessage.errorLevelTypes.ERROR, d.PackageID));
                }
            }
        }



        /// <summary>
        /// A PRIMLibrary must not contain any sub packages
        /// </summary>
        /// <param name="context"></param>
        /// <param name="d"></param>
        private void checkC514n(IValidationContext context, EA.Package d)
        {
            foreach (EA.Package subPackage in d.Packages)
            {
                context.AddValidationMessage(new ValidationMessage("Invalid package found in PRIMLibrary.", "A PRIMLibrary must not contain any sub packages. Please remove sub package " + subPackage.Name, "PRIMLibrary", ValidationMessage.errorLevelTypes.ERROR, d.PackageID));
            }

        }

        
        /// <summary>
        /// The name of a primitive type shall be unique for a given PRIM library.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private void checkC544a(IValidationContext context, EA.Package p, Dictionary<int, EA.Element> prims)
        {

            Dictionary<Int32, string> names = new Dictionary<int, string>();

            foreach (KeyValuePair<Int32, EA.Element> e in prims)
            {

                if (names.ContainsValue(e.Value.Name))
                {
                    context.AddValidationMessage(new ValidationMessage("Two PRIMs with the same name found.", "The name of a primitive type shall be unique for a given PRIM library. Two PRIMs with the name " + e.Value.Name + " found in PRIMLibrary " + p.Name + ".", "PRIMLibrary", ValidationMessage.errorLevelTypes.ERROR, p.PackageID));                }

                names.Add(e.Value.ElementID, e.Value.Name);
            }
        }


        /// <summary>
        /// The source of an isEquivalentTo dependency must be a PRIM. A PRIM which is the source of an isEquivalentTo dependency must not be the target of an isEquivalentTo dependency.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="d"></param>
        /// <param name="prims"></param>
        private void checkC544b(IValidationContext context, EA.Package p, Dictionary<int, EA.Element> prims)
        {

            foreach (KeyValuePair<Int32, EA.Element> e in prims)
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
                    context.AddValidationMessage(new ValidationMessage("PRIM is source and target of an isEquivalentTo dependency.", "The source of an isEquivalentTo dependency must be a PRIM. A PRIM which is the source of an isEquivalentTo dependency must not be the target of an isEquivalentTo dependency. Errorneous PRIM: " + element.Name + " in PRIMLibrary " + p.Name, "PRIMLibrary", ValidationMessage.errorLevelTypes.ERROR, p.PackageID));
                }
            }
        }


        /// <summary>
        /// The unique identifier tagged value of a PRIM shall be unique for a given PRIM library.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="prims"></param>
        private void checkC544c(IValidationContext context, EA.Package p, Dictionary<int, EA.Element> prims)
        {

            Dictionary<Int32, string> values = new Dictionary<Int32, string>();

            foreach (KeyValuePair<Int32, EA.Element> e in prims)
            {
                EA.TaggedValue tv = Utility.getTaggedValue(e.Value, UPCC_TV.uniqueIdentifier.ToString());
                if (tv != null)
                {
                    //Has this unique identifier already been used?
                    if (values.ContainsValue(tv.Value))
                    {
                        //Get the other element with the same unique identifier
                        EA.Element duplicateElement = context.Repository.GetElementByID(Utility.findKey(values, tv.Value));

                        context.AddValidationMessage(new ValidationMessage("Two identical unique identifier tagged values of a PRIM detected.", "TThe unique identifier tagged value of a PRIM shall be unique for a given PRIM library. " + e.Value.Name + " and " + duplicateElement.Name + " have the same unique identifier.", "PRIMLibrary", ValidationMessage.errorLevelTypes.ERROR, p.PackageID));
                    }

                    values.Add(e.Value.ElementID, tv.Value);
                }
            }   

        }


        /// <summary>
        /// The dictionary entry name tagged value of a CDT shall be unique for a given PRIM library.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        /// <param name="prims"></param>
        private void checkC544d(IValidationContext context, EA.Package p, Dictionary<int, EA.Element> prims)
        {


            Dictionary<Int32, string> values = new Dictionary<Int32, string>();

            foreach (KeyValuePair<Int32, EA.Element> e in prims)
            {
                EA.TaggedValue tv = Utility.getTaggedValue(e.Value, UPCC_TV.dictionaryEntryName.ToString());
                if (tv != null)
                {
                    //Has this unique identifier already been used?
                    if (values.ContainsValue(tv.Value))
                    {
                        //Get the other element with the same unique identifier
                        EA.Element duplicateElement = context.Repository.GetElementByID(Utility.findKey(values, tv.Value));

                        context.AddValidationMessage(new ValidationMessage("Two identical dictionary entry name tagged values of a PRIM detected.", "The dictionary entry name tagged value of a PRIM shall be unique for a given PRIM library. " + e.Value.Name + " and " + duplicateElement.Name + " have the same dictionary entry name.", "PRIMLibrary", ValidationMessage.errorLevelTypes.ERROR, p.PackageID));
                    }

                    values.Add(e.Value.ElementID, tv.Value);
                }
            } 


        }



    }
}
