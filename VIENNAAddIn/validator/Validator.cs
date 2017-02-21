/*******************************************************************************
This file is part of the VIENNAAddIn project

Licensed under GNU General Public License V3 http://gplv3.fsf.org/

For further information on the VIENNAAddIn project please visit 
http://vienna-add-in.googlecode.com
*******************************************************************************/
using System;
using System.Collections.Generic;
using VIENNAAddIn.validator.umm;
using VIENNAAddIn.validator.upcc3;
using VIENNAAddIn.common;

namespace VIENNAAddIn.validator
{
    internal class Validator : AbstractValidator
    {

        /// <summary>
        /// A validation scope can either be UMM, UPCC or it is unknown prior to the start
        /// of the validation (e.g. if the user has clicked somewhere in the treeview in order to invoke
        /// a validation
        /// </summary>
        
        internal override void validate(IValidationContext context, string scope)
        {

            //Validate the entire UMM model
            if (scope == "ROOT_UMM")
            {
                new bCollModelValidator().validate(context, scope);
            }
            //Validate the entire UPCC model
            else if (scope == "ROOT_UPCC")
            {
                new UPCCValidator().validate(context, scope);
            }
            else
            {
                //Try to determine whether that is a UMM or UPCC bottom up validation
                EA.Package p = context.Repository.GetPackageByID(Int32.Parse(scope));
                String stereotype = Utility.getStereoTypeFromPackage(p);
                if (Utility.isValidUMMStereotype(stereotype))
                {
                    new bCollModelValidator().validate(context, scope);
                }
                else if (Utility.isValidUPCCStereotype(stereotype))
                {
                    new UPCCValidator().validate(context, scope);
                }
                else
                {
                    context.AddValidationMessage(new ValidationMessage("Unknown or missing stereotype detected.", "The package " + p.Name + " has an unknown or missing stereotype. Validation cannot be started.", "-", ValidationMessage.errorLevelTypes.ERROR, p.PackageID));

                }

            }                                
            
        }
    }
}