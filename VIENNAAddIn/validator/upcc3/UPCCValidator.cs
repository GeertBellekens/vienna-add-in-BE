using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VIENNAAddIn.common;
using VIENNAAddIn.constants;
using System.Collections;

namespace VIENNAAddIn.validator.upcc3
{
    class UPCCValidator : AbstractValidator
    {


        /// <summary>
        /// Validate the given UPCC model
        /// </summary>
        /// <param name="context"></param>
        /// <param name="scope"></param>
        internal override void validate(IValidationContext context, string scope)
        {
            //Top-down validation of the entire UPCC model
            EA.Package p = null;
            if (scope == "ROOT_UPCC")
            {
                //Get the root package
                p = (EA.Package)(context.Repository.Models.GetAt(0));


                //No packages found at all
                if (p == null || p.Packages.Count == 0)
                {
                    context.AddValidationMessage(new ValidationMessage("No packages found", "The root package of the UPCC model does not contain any packages.", "UPCC", ValidationMessage.errorLevelTypes.ERROR, p.PackageID));
                    //Terminate validation here
                    return;
                }
                else
                {
                    //Iterate over the different packages of the root package
                    foreach (EA.Package subPackage in p.Packages) {
                        invokeValidation(context, subPackage.PackageID.ToString());
                    }
                }
            }
            //Bottom-up validation
            else
            {
                
                p = context.Repository.GetPackageByID(Int32.Parse(scope));
                
                //The user has clicked somewhere in the model
                //Determine the correct validator for the root Package
                String stereotype = p.Element.Stereotype;

                //bLibrary
                if (stereotype == UPCC_Packages.bLibrary.ToString())
                {
                    invokeValidation(context, scope);
                }
                //DOCLibrary
                else if (stereotype == UPCC_Packages.DOCLibrary.ToString())
                {
                    new DOCLibraryValidator().validate(context, scope);
                }
                //BIELibrary
                else if (stereotype == UPCC_Packages.BIELibrary.ToString())
                {
                    new BIELibraryValidator().validate(context, scope);
                }
                //BDTLibrary
                else if (stereotype == UPCC_Packages.BDTLibrary.ToString())
                {
                    new BDTLibraryValidator().validate(context, scope);
                }
                //ENUMLibrary
                else if (stereotype == UPCC_Packages.ENUMLibrary.ToString())
                {
                    new ENUMLibraryValidator().validate(context, scope);
                }
                //PRIMLibrary
                else if (stereotype == UPCC_Packages.PRIMLibrary.ToString())
                {
                    new PRIMLibraryValidator().validate(context, scope);
                }
                //CDTLibrary
                else if (stereotype == UPCC_Packages.CDTLibrary.ToString())
                {
                    new CDTLibraryValidator().validate(context, scope);
                }
                //CCLibrary
                else if (stereotype == UPCC_Packages.CCLibrary.ToString())
                {
                    new CCLibraryValidator().validate(context, scope);
                }
                //Unknown stereotype
                else
                {
                    context.AddValidationMessage(new ValidationMessage("Package with unknown stereotype found.", "A package with the unknown stereotype " + stereotype + " has been found. Allowed stereotypes for UPCC packages are bLibrary, DOCLibrary, BIELibrary, BDTLibrary, ENUMLibrary, PRIMLibrary, CDTLibrary and CCLibrary.", "UPCC", ValidationMessage.errorLevelTypes.ERROR, p.PackageID));
                }

                

            }



        }


        /// <summary>
        /// Invoke a validation run
        /// </summary>
        private void invokeValidation(IValidationContext context, String scope)
        {
            
            //UPCC models are structured using the concept of bLibrary. A bLibrary is the 
            //top library of every other library - bLibrary can be nested recursively
            //In the first step get all bLibrary from the UPCC model            
            EA.Package p = context.Repository.GetPackageByID(Int32.Parse(scope));
            IList bLibraryList = new ArrayList();
            Utility.getAllSubPackagesRecursively2(p, bLibraryList, UPCC.bLibrary.ToString());
            //Füge das eigentliche Package auch hinzu
            bLibraryList.Add(scope);

            foreach (string packageID in bLibraryList)
            {
                new bLibraryValidator().validate(context, packageID);
            }

        }





    }








}
