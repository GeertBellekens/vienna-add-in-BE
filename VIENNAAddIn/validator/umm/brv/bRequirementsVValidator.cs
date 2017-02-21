/*******************************************************************************
This file is part of the VIENNAAddIn project

Licensed under GNU General Public License V3 http://gplv3.fsf.org/

For further information on the VIENNAAddIn project please visit 
http://vienna-add-in.googlecode.com
*******************************************************************************/
using System;
using System.Collections.Generic;
using VIENNAAddIn.common;
using VIENNAAddIn.constants;

namespace VIENNAAddIn.validator.umm.brv
{
    internal class bRequirementsVValidator : AbstractValidator
    {
        internal override void validate(IValidationContext context, string scope)
        {
            //Get the BRV package
            EA.Package p = context.Repository.GetPackageByID(Int32.Parse(scope));

            //Check the TaggedValues of the bRequirementsV
            new TaggedValueValidator().validatePackage(context, p);

            checkC5(context, p);
            checkC6(context, p);
            checkC7(context, p);
            checkC8(context, p);

            //Iterate over the subpackages of the BusinessRequirementsView and check the 
            //constraints of the different subviews
            foreach (EA.Package subpackage in p.Packages)
            {
                String stereotype = Utility.getStereoTypeFromPackage(subpackage);
                if (stereotype == UMM.bDomainV.ToString())
                {
                    new TaggedValueValidator().validatePackage(context, subpackage);
                    new bDomainVValidator().validate(context, subpackage.PackageID.ToString());
                }
                else if (stereotype == UMM.bPartnerV.ToString())
                {
                    new TaggedValueValidator().validatePackage(context, subpackage);
                    new bPartnerVValidator().validate(context, subpackage.PackageID.ToString());
                }
                else if (stereotype == UMM.bEntityV.ToString())
                {
                    new TaggedValueValidator().validatePackage(context, subpackage);
                    new bEntityVValidator().validate(context, subpackage.PackageID.ToString());
                }
            }
        }

        /// <summary>
        /// Check constraint C5
        /// </summary>
        private void checkC5(IValidationContext context, EA.Package package)
        {
            int count = 0;
            foreach (EA.Package p in package.Packages)
            {
                if (Utility.getStereoTypeFromPackage(p) == UMM.bDomainV.ToString())
                {
                    count++;
                }
            }

            //More than one BDV - error
            if (count > 1)
            {
                context.AddValidationMessage(new ValidationMessage("Violation of constraint C5.",
                                                                   "A BusinessRequirementsView MAY contain zero or one BusinessDomainView. \n\nYour model contains " +
                                                                   count + " BusinessDomainViews.", "BRV",
                                                                   ValidationMessage.errorLevelTypes.ERROR, package.PackageID));
            }
                //Zero or one - info
            else
            {
                context.AddValidationMessage(new ValidationMessage("Information to constraint C5.",
                                                                   "A BusinessRequirementsView MAY contain zero or one BusinessDomainView. \n\nYour model contains " +
                                                                   count + " BusinessDomainViews.", "BRV",
                                                                   ValidationMessage.errorLevelTypes.INFO, package.PackageID));
            }
        }

        /// <summary>
        /// Check constraint C6
        /// </summary>
        private void checkC6(IValidationContext context, EA.Package package)
        {
            int count = 0;
            foreach (EA.Package p in package.Packages)
            {
                if (Utility.getStereoTypeFromPackage(p) == UMM.bPartnerV.ToString())
                {
                    count++;
                }
            }

            //More than one business partner view - error
            if (count > 1)
            {
                context.AddValidationMessage(new ValidationMessage("Violation of constraint C6.",
                                                                   "A BusinessRequirementsView MAY contain zero or one BusinessPartnerView. \n\nYour model contains " +
                                                                   count + " BusinessPartnerView.", "BRV",
                                                                   ValidationMessage.errorLevelTypes.ERROR, package.PackageID));
            }
                //Zero or business partner view - info
            else
            {
                context.AddValidationMessage(new ValidationMessage("Information to constraint C6.",
                                                                   "A BusinessRequirementsView MAY contain zero or one BusinessPartnerView. \n\nYour model contains " +
                                                                   count + " BusinessPartnerView.", "BRV",
                                                                   ValidationMessage.errorLevelTypes.INFO, package.PackageID));
            }
        }


        /// <summary>
        /// Check constraint C7
        /// </summary>
        private void checkC7(IValidationContext context, EA.Package package)
        {
            int count = 0;
            foreach (EA.Package p in package.Packages)
            {
                if (Utility.getStereoTypeFromPackage(p) == UMM.bEntityV.ToString())
                {
                    count++;
                }
            }

            context.AddValidationMessage(new ValidationMessage("Information to constraint C7.",
                                                               "A BusinessRequirementsView MAY contain zero to many  BusinessEntityViews. \n\nYour model contains " +
                                                               count + " BusinessEntityViews.", "BRV",
                                                               ValidationMessage.errorLevelTypes.INFO, package.PackageID));
        }


        /// <summary>
        /// Check constraint C8
        /// </summary>
        /// <param name="context"></param>
        /// <param name="pa"></param>
        private void checkC8(IValidationContext context, EA.Package pa)
        {
            EA.Collection packages = pa.Packages;

            if (packages != null)
            {
                //Iterate over the packages under the business requirements view
                foreach (EA.Package p in packages)
                {
                    //No BusinessDomain View must be located underneath any of the first level packages underneath the business requirements view
                    IList<EA.Package> bdvs = Utility.getAllSubPackagesWithGivenStereotypeRecursively(p,
                                                                                                     new List
                                                                                                         <EA.Package>(),
                                                                                                     UMM.bDomainV.
                                                                                                         ToString());
                    if (bdvs != null && bdvs.Count != 0)
                    {
                        context.AddValidationMessage(new ValidationMessage("Violation of constraint C8.",
                                                                           "A BusinessDomainView, a BusinessPartnerView, and a BusinessEntityView MUST be located directly under a BusinessRequirementsView.. \n\nPackage " +
                                                                           p.Name + " contains an invalid BusinessDomainView.", "BRV",
                                                                           ValidationMessage.errorLevelTypes.ERROR, p.PackageID));
                    }

                    //No BusinessPartnerView must be located underneath any of the first level packages underneath the business requirements view
                    IList<EA.Package> bpvs = Utility.getAllSubPackagesWithGivenStereotypeRecursively(p,
                                                                                                     new List
                                                                                                         <EA.Package>(),
                                                                                                     UMM.bPartnerV.
                                                                                                         ToString());
                    if (bpvs != null && bpvs.Count != 0)
                    {
                        context.AddValidationMessage(new ValidationMessage("Violation of constraint C8.",
                                                                           "A BusinessDomainView, a BusinessPartnerView, and a BusinessEntityView MUST be located directly under a BusinessRequirementsView.. \n\nPackage " +
                                                                           p.Name + " contains an invalid BusinessPartnerView.",
                                                                           "BRV", ValidationMessage.errorLevelTypes.ERROR,
                                                                           p.PackageID));
                    }

                    //No BusinessEntityView must be located underneath any of the first level packages underneath the business requirements view
                    IList<EA.Package> bevs = Utility.getAllSubPackagesWithGivenStereotypeRecursively(p,
                                                                                                     new List
                                                                                                         <EA.Package>(),
                                                                                                     UMM.bEntityV.
                                                                                                         ToString());
                    if (bevs != null && bevs.Count != 0)
                    {
                        context.AddValidationMessage(new ValidationMessage("Violation of constraint C8.",
                                                                           "A BusinessDomainView, a BusinessPartnerView, and a BusinessEntityView MUST be located directly under a BusinessRequirementsView.. \n\nPackage " +
                                                                           p.Name + " contains an invalid BusinessEntityView.", "BRV",
                                                                           ValidationMessage.errorLevelTypes.ERROR, p.PackageID));
                    }
                }

                //Make sure there are'nt any other packages in underneath the BRV
                foreach (EA.Package p in packages)
                {
                    String stereotype = Utility.getStereoTypeFromPackage(p);
                    if (
                        !(stereotype == UMM.bDomainV.ToString() || stereotype == UMM.bPartnerV.ToString() ||
                          stereotype == UMM.bEntityV.ToString()))
                    {
                        context.AddValidationMessage(new ValidationMessage("Violation of constraint C8.",
                                                                           "A BusinessDomainView, a BusinessPartnerView, and a BusinessEntityView MUST be located directly under a BusinessRequirementsView.. \n\nPackage " +
                                                                           p.Name + " has an invalid stereotype.", "BRV",
                                                                           ValidationMessage.errorLevelTypes.ERROR, p.PackageID));
                    }
                }
            }
        }
    }
}