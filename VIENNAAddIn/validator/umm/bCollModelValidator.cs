/*******************************************************************************
This file is part of the VIENNAAddIn project

Licensed under GNU General Public License V3 http://gplv3.fsf.org/

For further information on the VIENNAAddIn project please visit 
http://vienna-add-in.googlecode.com
*******************************************************************************/
using System;
using System.Collections.Generic;
using VIENNAAddIn.constants;
using VIENNAAddIn.common;

using VIENNAAddIn.validator.umm.brv;
using VIENNAAddIn.validator.umm.bcv;
using VIENNAAddIn.validator.umm.biv;

namespace VIENNAAddIn.validator.umm

{
    class bCollModelValidator : AbstractValidator
    {
        internal override void validate(IValidationContext context, string scope) 
        {
                //Validate all has been choosen
                if (scope == "ROOT_UMM")
                {

                    EA.Collection rootPackages = ((EA.Package)(context.Repository.Models.GetAt(0))).Packages;
                    int rootModelPackageID = ((EA.Package)(context.Repository.Models.GetAt(0))).PackageID;

                    if (rootPackages == null || rootPackages.Count == 0)
                    {
                        context.AddValidationMessage(new ValidationMessage("No packages found", "The root package of the UMM model does not contain any packages.", "BCM", ValidationMessage.errorLevelTypes.WARN, rootModelPackageID));
                    }
                    else
                    {
                        checkC1(context);
                        checkC2(context);
                        checkC3(context);
                        checkC4(context);

                        //Iterate over the different packages
                        foreach (EA.Package p in rootPackages)
                        {
                            String stereotype = Utility.getStereoTypeFromPackage(p);

                            //Business Requirements View
                            if (stereotype == UMM.bRequirementsV.ToString())
                            {
                                new TaggedValueValidator().validatePackage(context, p);
                                new bRequirementsVValidator().validate(context, p.PackageID.ToString());
                            }
                            //Business Choreography View
                            else if (stereotype == UMM.bChoreographyV.ToString())
                            {
                                new TaggedValueValidator().validatePackage(context, p);
                                new bChoreographyVValidator().validate(context, p.PackageID.ToString());
                            }
                            //Business Information View
                            else if (stereotype == UMM.bInformationV.ToString())
                            {
                                new TaggedValueValidator().validatePackage(context, p);
                                new bInformationVValidator().validate(context, p.PackageID.ToString());
                            }
                            else
                            {
                                context.AddValidationMessage(new ValidationMessage("Unknown or missing stereotype detected.", "The package " + p.Name + " has an unknown or missing stereotype and will be ignored for validation. Please make sure that all packages are stereotyped correctly in order to allow for a validation of the entire model.", "BCM", ValidationMessage.errorLevelTypes.WARN, p.PackageID));
                            }
                        }
                    }
                }
                else
                {
                    //Validation has been invoked by  a click somewhere in the model (bottom-up) validation
                    //Try to determine the stereotype of the selected scope
                    EA.Package p = context.Repository.GetPackageByID(Int32.Parse(scope));
                    String stereotype = Utility.getStereoTypeFromPackage(p);
                        //BusinessRequirementsView
                        if (stereotype == UMM.bRequirementsV.ToString())
                        {
                            new bRequirementsVValidator().validate(context, scope);
                        }
                        //Business Domain View
                        else if (stereotype == UMM.bDomainV.ToString())
                        {
                            new bDomainVValidator().validate(context, scope);
                        }
                        //BusinessAreas and Process Areas are part of the business domain view - hence the whole
                        //business domain view is validated instead of a subpackage
                        else if (stereotype == UMM.bArea.ToString() || stereotype == UMM.ProcessArea.ToString())
                        {
                            //Get the parent BDV of the given process area or business area
                            scope = getParentByStereotype(context, scope, UMM.bDomainV.ToString());
                            if (scope == "")
                            {
                                context.AddValidationMessage(new ValidationMessage("Unable to detect business domain view.", "Business areas and process areas must be located underneath a business domain view. Validation cannot be started.", "BCM", ValidationMessage.errorLevelTypes.ERROR, p.PackageID));
                            }
                            else
                            {
                                new bDomainVValidator().validate(context, scope);
                            }
                        }
                        //Business Partner View
                        else if (stereotype == UMM.bPartnerV.ToString())
                        {
                            new bPartnerVValidator().validate(context, scope);
                        }
                        //Business Data View is a part of the business entity view and has no own validator class
                        //determine the parent business entity view and invoke validation there
                        else if (stereotype == UMM.bDataV.ToString())
                        {
                            //Get the parent business entity view
                            scope = getParentByStereotype(context, scope, UMM.bEntityV.ToString());
                            if (scope == "")
                            {
                                context.AddValidationMessage(new ValidationMessage("Unable to detect business entity view.", "A business data view must be located underneath a business entity view.. Validation cannot be started.", "BCM", ValidationMessage.errorLevelTypes.ERROR, p.PackageID));
                            }
                            else
                            {
                                new bEntityVValidator().validate(context, scope);
                            }
                        }
                        //Business Entity View
                        else if (stereotype == UMM.bEntityV.ToString())
                        {
                            new bEntityVValidator().validate(context, scope);
                        }
                        //Business Choreography View
                        else if (stereotype == UMM.bChoreographyV.ToString())
                        {
                            new bChoreographyVValidator().validate(context, scope);
                        }
                        //Business Transaction View
                        else if (stereotype == UMM.bTransactionV.ToString())
                        {
                            new bTransactionVValidator().validate(context, scope);
                        }
                        //Business Collaboration View
                        else if (stereotype == UMM.bCollaborationV.ToString())
                        {
                            new bCollaborationVValidator().validate(context, scope);
                        }
                        //Business Realization view
                        else if (stereotype == UMM.bRealizationV.ToString())
                        {
                            new bRealizationVValidator().validate(context, scope);
                        }
                        //Business Information View
                        else if (stereotype == UMM.bInformationV.ToString())
                        {
                            new bInformationVValidator().validate(context, scope);
                        }

                   
                }
        }




        /// <summary>
        /// Check constraint C1
        /// </summary>
        private void checkC1(IValidationContext context)
        {
            bool found = false;
            EA.Collection rootPackages = ((EA.Package)(context.Repository.Models.GetAt(0))).Packages;

            if (rootPackages != null)
            {
                //Iterate over the different packages
                foreach (EA.Package p in rootPackages)
                {
                    if (p.Element != null && p.Element.Stereotype == UMM.bChoreographyV.ToString())
                    {
                        found = true;
                        break;
                    }

                }
            }

            if (!found)
            {
                context.AddValidationMessage(new ValidationMessage("Violation of constraint C1.", "A BusinessCollaborationModel MUST contain one to many BusinessChoregraphyViews.", "BCM", ValidationMessage.errorLevelTypes.ERROR, 0));
            }
        }


        /// <summary>
        /// Check constraint C2
        /// </summary>
        private void checkC2(IValidationContext context)
        {
            bool found = false;
            EA.Collection rootPackages = ((EA.Package)(context.Repository.Models.GetAt(0))).Packages;

            if (rootPackages != null)
            {
                //Iterate over the different packages
                foreach (EA.Package p in rootPackages)
                {
                    if (p.Element != null && p.Element.Stereotype == UMM.bInformationV.ToString())
                    {
                        found = true;
                        break;
                    }

                }
            }

            if (!found)
            {
                context.AddValidationMessage(new ValidationMessage("Violation of constraint C2.", "A BusinessCollaborationModel MUST contain one to many BusinessInformationViews.", "BCM", ValidationMessage.errorLevelTypes.ERROR, 0));
            }
        }




        /// <summary>
        /// Check constraint C3
        /// </summary>
        private void checkC3(IValidationContext context)
        {
            //This constraint is not validated but only an info is given back to the user
            
            EA.Collection rootPackages = ((EA.Package)(context.Repository.Models.GetAt(0))).Packages;
            int count = 0;

            if (rootPackages != null)
            {
                //Iterate over the different packages
                foreach (EA.Package p in rootPackages)
                {
                    if (p.Element != null && p.Element.Stereotype == UMM.bRequirementsV.ToString())
                    {                       
                        count++;                        
                    }

                }
            }

            context.AddValidationMessage(new ValidationMessage("Information to constraint C3.", "A BusinessCollaborationModel MAY containt zero to many BusinessRequirementsViews. \n\nYour model contains " + count + " BusinessRequirementsViews.", "BCM", ValidationMessage.errorLevelTypes.INFO, 0));
        }



        /// <summary>
        /// Check constraint C4
        /// </summary>
        private void checkC4(IValidationContext context)
        {
            EA.Collection rootPackages = ((EA.Package)(context.Repository.Models.GetAt(0))).Packages;

            if (rootPackages != null)
            {
                //Iterate over the packages under the model root
                foreach (EA.Package p in rootPackages)
                {
                    //No BusinessRequirementsViews must be located under any of the packages on the first level
                    IList<EA.Package> brvs = Utility.getAllSubPackagesWithGivenStereotypeRecursively(p, new List<EA.Package>(), UMM.bRequirementsV.ToString());
                    if (brvs != null && brvs.Count != 0)
                    {
                        context.AddValidationMessage(new ValidationMessage("Violation of constraint C4.","A BusinessRequirementsView, a BusinessChoreographyView, and a BusinessInformationView MUST be directly located under a BusinessCollaborationModel. \n\nPackage " + p.Name + " contains an invalid BusinessRequirementsView.","BCM",ValidationMessage.errorLevelTypes.ERROR,p.PackageID));
                    }

                    //No BusinessChoreographyViews must be located under any of the packages on the first level
                    IList<EA.Package> bcvs = Utility.getAllSubPackagesWithGivenStereotypeRecursively(p, new List<EA.Package>(), UMM.bChoreographyV.ToString());
                    if (bcvs != null && bcvs.Count != 0)
                    {
                        context.AddValidationMessage(new ValidationMessage("Violation of constraint C4.", "A BusinessRequirementsView, a BusinessChoreographyView, and a BusinessInformationView MUST be directly located under a BusinessCollaborationModel. \n\nPackage " + p.Name + " contains an invalid BusinessChoreographyView.", "BCM", ValidationMessage.errorLevelTypes.ERROR, p.PackageID));
                    }

                    //No BusinessInformationViews must be located under any of the packages on the first level
                    IList<EA.Package> bivs = Utility.getAllSubPackagesWithGivenStereotypeRecursively(p, new List<EA.Package>(), UMM.bInformationV.ToString());
                    if (bivs != null && bivs.Count != 0)
                    {
                        context.AddValidationMessage(new ValidationMessage("Violation of constraint C4.", "A BusinessRequirementsView, a BusinessChoreographyView, and a BusinessInformationView MUST be directly located under a BusinessCollaborationModel. \n\nPackage " + p.Name + " contains an invalid BusinessInformationView.", "BCM", ValidationMessage.errorLevelTypes.ERROR, p.PackageID));
                    }
                }
            }
        }





        /// <summary>
        /// Return the parent with the given stereotype
        /// be found
        /// </summary>
        /// <param name="context"></param>
        /// <param name="scope"></param>
        /// <param name="stereotype_"></param>
        /// <returns></returns>
        private String getParentByStereotype(IValidationContext context, String scope, String stereotype_)
        {

            EA.Package childPackage = context.Repository.GetPackageByID(Int32.Parse(scope));

            if (childPackage.ParentID != 0)
            {
                EA.Package parentPackage = context.Repository.GetPackageByID(childPackage.ParentID);
                String stereotype = Utility.getStereoTypeFromPackage(parentPackage);
                if (stereotype == stereotype_)
                {
                    return parentPackage.PackageID.ToString();
                }
                return getParentByStereotype(context, parentPackage.PackageID.ToString(), stereotype_);
            }
            return "";
        }




    }
}
