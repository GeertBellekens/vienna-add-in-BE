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

namespace VIENNAAddIn.validator.umm.biv
{
    internal class bInformationVValidator : AbstractValidator
    {
        /// <summary>
        /// Validate the BusinessInformationView
        /// </summary>
        internal override void validate(IValidationContext context, string scope)
        {
            EA.Package biv = context.Repository.GetPackageByID(Int32.Parse(scope));
            checkTV_BusinessInformationView(context, biv);
            checkC100(context, biv);
        }


        /// <summary>
        /// 
        /// Check constraint C100
        /// A BusinessInformationView MUST contain one to many 
        /// InformationEnvelopes or subtypes thereof defined in any other 
        /// extension/specialization modile. Furthermore, it MAY contains 
        /// any other document modeling artifacts.
        /// 
        /// </summary>
        private static void checkC100(IValidationContext context, EA.Package biv)
        {
            var informationEnvelopes = Utility.getAllClasses(biv, new List<EA.Element>());

            var informationEnvelopeFound = false;

            foreach (var e in informationEnvelopes)
            {
                //Is the classifier of type InformationEnvelope?
                if (e.Stereotype == UMM.InfEnvelope.ToString())
                {
                    informationEnvelopeFound = true;
                }
                    //Is the classifier a subtype of an InformationEnvelope?
                else
                {
                    foreach (EA.Element el in e.BaseClasses)
                    {
                        if (el.Stereotype == UMM.InfEnvelope.ToString())
                        {
                            informationEnvelopeFound = true;
                            break;
                        }
                    }
                }
            }

            if (!informationEnvelopeFound)
            {
                context.AddValidationMessage(new ValidationMessage("Violation of constraint C100.",
                                                                   "A BusinessInformationView MUST contain one to many InformationEnvelopes (InfEnvelope) or subtypes thereof defined in any other extension/specialization modile. Furthermore, it MAY contains any other document modeling artifacts. \n\nNo InformationEnvelopes or subtypes thereof could be found.",
                                                                   "BIV", ValidationMessage.errorLevelTypes.ERROR, biv.PackageID));
            }
        }


        /// <summary>
        /// Check the tagged values of the BusinessInformationView
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        private static void checkTV_BusinessInformationView(IValidationContext context, EA.Package p)
        {
            new TaggedValueValidator().validatePackage(context, p);
        }
    }
}