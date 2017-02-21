using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VIENNAAddIn.constants;

namespace VIENNAAddIn.validator.upcc3
{
    class bLibraryValidator : AbstractValidator
    {



        /// <summary>
        /// Validate a given bLibrary
        /// </summary>
        /// <param name="context"></param>
        /// <param name="scope"></param>
        internal override void validate(IValidationContext context, string scope)
        {

            EA.Package package = context.Repository.GetPackageByID(Int32.Parse(scope));

            checkC514a(context, package);  


            //Iterate through the subpackages of the bLibrary and apply the correct validators
            foreach (EA.Package p in package.Packages)
            {
                //Determine the correct validator for the root Package
                String stereotype = p.Element.Stereotype;
                //bLibrary
                if (stereotype == UPCC_Packages.bLibrary.ToString())
                {
                    //Ignore since every bLibrary is validated separately
                }
                //DOCLibrary
                else if (stereotype == UPCC_Packages.DOCLibrary.ToString())
                {
                    new DOCLibraryValidator().validate(context, p.PackageID.ToString());
                }
                //BIELibrary
                else if (stereotype == UPCC_Packages.BIELibrary.ToString())
                {
                    new BIELibraryValidator().validate(context, p.PackageID.ToString());
                }
                //BDTLibrary
                else if (stereotype == UPCC_Packages.BDTLibrary.ToString())
                {
                    new BDTLibraryValidator().validate(context, p.PackageID.ToString());
                }
                //ENUMLibrary
                else if (stereotype == UPCC_Packages.ENUMLibrary.ToString())
                {
                    new ENUMLibraryValidator().validate(context, p.PackageID.ToString());
                }
                //PRIMLibrary
                else if (stereotype == UPCC_Packages.PRIMLibrary.ToString())
                {
                    new PRIMLibraryValidator().validate(context, p.PackageID.ToString());
                }
                //CDTLibrary
                else if (stereotype == UPCC_Packages.CDTLibrary.ToString())
                {
                    new CDTLibraryValidator().validate(context, p.PackageID.ToString());
                }
                //CCLibrary
                else if (stereotype == UPCC_Packages.CDTLibrary.ToString())
                {
                    new CCLibraryValidator().validate(context, p.PackageID.ToString());
                }
                //Unknown stereotype
                else
                {
                    //No need to raise an error here since this is done by every subvalidator for the respective subpackage

                    //context.AddValidationMessage(new ValidationMessage("Package with unknown stereotype found.", "A package with the unknown stereotype " + stereotype + " has been found. Allowed stereotypes for UPCC packages are bLibrary, DOCLibrary, BIELibrary, BDTLibrary, ENUMLibrary, PRIMLibrary, CDTLibrary and CCLibrary.", "UPCC", ValidationMessage.errorLevelTypes.ERROR, p.PackageID));
                }

            }

        }





        /// <summary>
        /// A bLibrary may only containt packages of type
        /// BDTLibrary, BIELibrary, CCLibrary, DOCLibrary, ENUMLibrary and PRIMLibrary
        /// </summary>
        /// <param name="context"></param>
        /// <param name="brv"></param>
        private void checkC514a(IValidationContext context, EA.Package d)
        {
            foreach (EA.Package p in d.Packages)
            {
                bool found = false;
                //Get the stereotype of the package
                String stereotype = p.Element.Stereotype;
                foreach (UPCC_Packages upcc in Enum.GetValues(typeof(UPCC_Packages)))
                {
                    if (upcc.ToString().Equals(stereotype))
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    context.AddValidationMessage(new ValidationMessage("Package with invalid stereotype found.", "Package " + p.Name + " has an invalid stereotype (" + stereotype + "). A bLibrary shall only contain packages of type bLibrary, BDTLibrary, BIELibrary, CCLibrary, CDTLibrary, DOCLibrary, ENUMLibrary and no other elements.", "bLibrary", ValidationMessage.errorLevelTypes.ERROR, d.PackageID));
                }
            }
        }









    }
}
