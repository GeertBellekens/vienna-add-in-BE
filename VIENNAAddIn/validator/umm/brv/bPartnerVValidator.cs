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
    class bPartnerVValidator : AbstractValidator
    {
        /// <summary>
        /// Validate the BusinessDomainView
        /// </summary> 
        internal override void validate(IValidationContext context, string scope)
        {
            EA.Package bpv = context.Repository.GetPackageByID(Int32.Parse(scope));
            checkTV_BusinessPartnerView(context, bpv);
            checkC21(context, bpv);
            checkC22(context, bpv);
        }




        /// <summary>
        /// Check constraint C21
        /// </summary>
        /// <param name="context"></param>
        /// <param name="bpv"></param>
        private void checkC21(IValidationContext context, EA.Package bpv)
        {
            //Get all BusinessPartners from the BusinessPartnerView
            IList<EA.Element> bpartners = Utility.getAllElements(bpv, new List<EA.Element>(), UMM.bPartner.ToString());

            //There must be at least two business partners
            if (bpartners.Count < 2)
            {
                context.AddValidationMessage(new ValidationMessage("Violation of constraint C21.","A BusinessPartnerView MUST contain at least two to many BusinessPartners. If the BusinessPartnerView is hierarchically decomposed into subpackages these BusinessPartners MAY be contained in any of these subpackages. \n\nThe BusinessPartnerView " + bpv.Name + " contains " + bpartners.Count + " BusinessPartners.","BRV",ValidationMessage.errorLevelTypes.ERROR,bpv.PackageID));
            }
            else
            {
                context.AddValidationMessage(new ValidationMessage("Info for constraint C21.","A BusinessPartnerView MUST contain at least two to many BusinessPartners. If the BusinessPartnerView is hierarchically decomposed into subpackages these BusinessPartners MAY be contained in any of these subpackages. \n\nThe BusinessPartnerView " + bpv.Name + " contains " + bpartners.Count + " Stakeholders.","BRV",ValidationMessage.errorLevelTypes.INFO,bpv.PackageID));
            }
        }


        /// <summary>
        /// Check constraint C22
        /// </summary>
        /// <param name="context"></param>
        /// <param name="bpv"></param>
        private void checkC22(IValidationContext context, EA.Package bpv)
        {
            //Get all Stakeholders from the BusinessPartnerView
            IList<EA.Element> stakeholders = Utility.getAllElements(bpv, new List<EA.Element>(), UMM.Stakeholder.ToString());
            context.AddValidationMessage(new ValidationMessage("Info for constraint C22.", "A BusinessPartnerView MAY contain zero to many Stakeholders.\n\nThe BusinessPartnerView " + bpv.Name + " contains " + stakeholders.Count, "BRV", ValidationMessage.errorLevelTypes.INFO, bpv.PackageID));
        }



        /// <summary>
        /// Check the tagged values of the business partner view
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        private void checkTV_BusinessPartnerView(IValidationContext context, EA.Package p)
        {
            //Check the TaggedValues of the bPartnerV
            new TaggedValueValidator().validatePackage(context, p);

            //Check the TaggedValues of the different Stakeholders
            IList<EA.Element> stakeholders = Utility.getAllElements(p, new List<EA.Element>(), UMM.Stakeholder.ToString());
            foreach (EA.Element sth in stakeholders)
            {
                new TaggedValueValidator().validateElement(context, sth);
            }

            //Check the TaggedValues of the different BusinessParnters
            IList<EA.Element> businessPartners = Utility.getAllElements(p, new List<EA.Element>(), UMM.bPartner.ToString());
            foreach (EA.Element bpartner in businessPartners)
            {
                new TaggedValueValidator().validateElement(context, bpartner);
            }
        }
    }
}
