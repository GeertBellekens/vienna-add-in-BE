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

namespace VIENNAAddIn.validator.umm.bcv
{
    class bChoreographyVValidator : AbstractValidator
    {
        /// <summary>
        /// Validate the BusinessChoreographyView
        /// </summary>
        internal override void validate(IValidationContext context, string scope)
        {
            EA.Package bcv = context.Repository.GetPackageByID(Int32.Parse(scope));

            //Check the TaggedValues of the bChoreographyV
            new TaggedValueValidator().validatePackage(context, bcv); 
            
            checkC30(context, bcv);
            checkC31(context, bcv);
            checkC32(context, bcv);
            checkC33(context, bcv);

            //Iterate of the different packages of the business choreography view
            foreach (EA.Package p in bcv.Packages)
            {

                String stereotype = Utility.getStereoTypeFromPackage(p);
                //Invoke BusinessCollaborationView Validator
                if (stereotype == UMM.bCollaborationV.ToString())
                {
                    new bCollaborationVValidator().validate(context, p.PackageID.ToString());
                }
                //Invoke Business TransactionVValidator
                else if (stereotype == UMM.bTransactionV.ToString())
                {
                    new bTransactionVValidator().validate(context, p.PackageID.ToString());
                }
                //Invoke BusinessRealizationViewValidator
                else if (stereotype == UMM.bRealizationV.ToString())
                {
                    new bRealizationVValidator().validate(context, p.PackageID.ToString());
                }
                //Unknown stereotype
                else
                {
                    context.AddValidationMessage(new ValidationMessage("Unknown stereotype detected.", "An unknown stereotype has been detected within the BusinessChorographyView: " + stereotype, "BCV", ValidationMessage.errorLevelTypes.ERROR, p.PackageID));
                }
            }
        }



        /// <summary>
        /// Check constraint C30
        /// </summary>
        /// <param name="context"></param>
        /// <param name="bcv"></param>
        private void checkC30(IValidationContext context, EA.Package bcv)
        {

            int count_bcollv = 0;
            //Iterate of the different packages of the business choreography view
            foreach (EA.Package p in bcv.Packages)
            {

                String stereotype = Utility.getStereoTypeFromPackage(p);
                if (stereotype == UMM.bCollaborationV.ToString())
                {
                    count_bcollv++;
                }
            }

            //There must be at least one BusinessCollaborationView
            if (count_bcollv < 1)
            {
                context.AddValidationMessage(new ValidationMessage("Violation of constraint C30.", "A BusinessChoreographyView MUST contain one to many BusinessCollaborationViews. \n\nNo BusinessCollaborationView could be found.", "BCV", ValidationMessage.errorLevelTypes.ERROR, bcv.PackageID));
            }
        }

        /// <summary>
        /// Check constraint C31
        /// </summary>
        /// <param name="context"></param>
        /// <param name="bcv"></param>
        private void checkC31(IValidationContext context, EA.Package bcv)
        {
            int count_btv = 0;
            //Iterate of the different packages of the business choreography view
            foreach (EA.Package p in bcv.Packages)
            {

                String stereotype = Utility.getStereoTypeFromPackage(p);
                if (stereotype == UMM.bTransactionV.ToString())
                {
                    count_btv++;
                }
            }

            //There must be at least one BusinessTransactionView
            if (count_btv < 1)
            {
                context.AddValidationMessage(new ValidationMessage("Violation of constraint C31.", "A BusinessChoreographyView MUST contain one to many BusinessTransactionViews. \n\nNo BusinessTransactionView could be found.", "BCV", ValidationMessage.errorLevelTypes.ERROR, bcv.PackageID));
            }
        }



        /// <summary>
        /// Check constraint C32
        /// </summary>
        /// <param name="context"></param>
        /// <param name="bcv"></param>
        private void checkC32(IValidationContext context, EA.Package bcv)
        {
            int count_brv = 0;
            //Iterate of the different packages of the business choreography view
            foreach (EA.Package p in bcv.Packages)
            {

                String stereotype = Utility.getStereoTypeFromPackage(p);
                if (stereotype == UMM.bRealizationV.ToString())
                {
                    count_brv++;
                }
            }

            //There may be zero to many BRVs
            context.AddValidationMessage(new ValidationMessage("Info for constraint C32.", "A BusinessChoreographyView MAY contain zero to many BusinessRealizationViews\n\n" + count_brv + " BusinessRealizationVies have been found.", "BCV", ValidationMessage.errorLevelTypes.INFO, bcv.PackageID));
        }

        /// <summary>
        /// Check constraint C33
        /// </summary>
        /// <param name="context"></param>
        /// <param name="bcv"></param>
        private void checkC33(IValidationContext context, EA.Package bcv)
        {
            //Iterate over the packages under the bcv
            foreach (EA.Package p in bcv.Packages)
            {
                //No BusinessTransactionView must be located under any of the packages on the first level
                IList<EA.Package> btvs = Utility.getAllSubPackagesWithGivenStereotypeRecursively(p, new List<EA.Package>(), UMM.bTransactionV.ToString());
                if (btvs != null && btvs.Count != 0)
                {
                    context.AddValidationMessage(new ValidationMessage("Violation of constraint C33.", "A BusinessTransactionView, a BusinessCollaborationView, and a BusinessRealizationView MUST be directly located under a BusinessChoreographyView \n\nPackage " + p.Name + " contains an invalid BusinessTransactionView.", "BCV", ValidationMessage.errorLevelTypes.ERROR, p.PackageID));
                }


                //No BusinessCollaborationView must be located under any of the packages on the first level
                IList<EA.Package> bcvs = Utility.getAllSubPackagesWithGivenStereotypeRecursively(p, new List<EA.Package>(), UMM.bCollaborationV.ToString());
                if (bcvs != null && bcvs.Count != 0)
                {
                    context.AddValidationMessage(new ValidationMessage("Violation of constraint C33.", "A BusinessTransactionView, a BusinessCollaborationView, and a BusinessRealizationView MUST be directly located under a BusinessChoreographyView \n\nPackage " + p.Name + " contains an invalid BusinessCollaborationView.", "BCV", ValidationMessage.errorLevelTypes.ERROR, p.PackageID));
                }

                //No BusinessRealizationView must be located under any of the packages on the first level
                IList<EA.Package> brvs = Utility.getAllSubPackagesWithGivenStereotypeRecursively(p, new List<EA.Package>(), UMM.bRealizationV.ToString());
                if (brvs != null && brvs.Count != 0)
                {
                    context.AddValidationMessage(new ValidationMessage("Violation of constraint C33.", "A BusinessTransactionView, a BusinessCollaborationView, and a BusinessRealizationView MUST be directly located under a BusinessChoreographyView \n\nPackage " + p.Name + " contains an invalid BusinessRealizationview.", "BCV", ValidationMessage.errorLevelTypes.ERROR, p.PackageID));
                }
            }
        }
    }
}
