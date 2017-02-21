using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VIENNAAddIn.constants;

namespace VIENNAAddIn.validator.upcc3
{
    class DOCLibraryValidator : AbstractValidator
    {








        /// <summary>
        /// Validate the given DOCLibrary
        /// </summary>
        /// <param name="context"></param>
        /// <param name="scope"></param>
        internal override void validate(IValidationContext context, string scope)
        {

            EA.Package package = context.Repository.GetPackageByID(Int32.Parse(scope));

            checkC514f(context, package);

            checkC514m(context, package);

        }




        /// <summary>
        /// A DOCLibrary shall only contain ABIEs, BBIEs, and ASBIEs.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="brv"></param>
        private void checkC514f(IValidationContext context, EA.Package d)
        {
            foreach (EA.Element e in d.Elements)
            {
                if (e.Type != EA_Element.Note.ToString())
                {
                    String stereotype = e.Stereotype;
                    if (stereotype == null || !stereotype.Equals(UPCC.ABIE.ToString()))
                        context.AddValidationMessage(new ValidationMessage("Invalid element found in DOCLibrary.", "The element " + e.Name + " has an invalid stereotype (" + stereotype + "). A DOCLibrary shall only contain ABIEs, BBIEs, and ASBIEs.", "DOCLibrary", ValidationMessage.errorLevelTypes.ERROR, d.PackageID));

                }
            }
        }


        /// <summary>
        /// A DOCLibrary must not contain any sub packages
        /// </summary>
        /// <param name="context"></param>
        /// <param name="d"></param>
        private void checkC514m(IValidationContext context, EA.Package d)
        {
            foreach (EA.Package subPackage in d.Packages)
            {
                context.AddValidationMessage(new ValidationMessage("Invalid package found in DOCLibrary.", "A DOCLibrary must not contain any sub packages. Please remove sub package " + subPackage.Name, "DOCLibrary", ValidationMessage.errorLevelTypes.ERROR, d.PackageID));
            }

        }






    }
}
