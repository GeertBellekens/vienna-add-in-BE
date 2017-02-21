using System;
using VIENNAAddIn.upcc3.otf.validators.constraints;

namespace VIENNAAddIn.upcc3.otf.validators
{
    public class ElementLibaryValidator : BusinessLibraryValidator
    {
        public ElementLibaryValidator(string libraryStereotype, string elementStereotype):base(libraryStereotype)
        {
            AddConstraint(new ParentPackageMustHaveStereotype(Stereotype.bLibrary));
            AddConstraint(new ElementsMustHaveStereotype(elementStereotype));
            AddConstraint(new MustNotContainAnyPackages());
            // TODO implement constraint 5.4.4a and equivalent constraints: element names must be unique
        }
    }
}