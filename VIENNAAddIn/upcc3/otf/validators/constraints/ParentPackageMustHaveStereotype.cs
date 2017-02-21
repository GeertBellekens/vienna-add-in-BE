using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.otf.validators.constraints
{
    public class ParentPackageMustHaveStereotype : SafeConstraint<RepositoryItem>
    {
        private readonly string stereotype;

        public ParentPackageMustHaveStereotype(string stereotype)
        {
            this.stereotype = stereotype;
        }

        protected override IEnumerable<ConstraintViolation> SafeCheck(RepositoryItem item)
        {
            if (stereotype != item.Parent.Stereotype)
            {
                yield return new ConstraintViolation(item.Id, item.Id, "The parent package of " + item.Name + " must have stereotype " + stereotype + ", but has " + item.Parent.Stereotype + ".");
            }
        }
    }
}