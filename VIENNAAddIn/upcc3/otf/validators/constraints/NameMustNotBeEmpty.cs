using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.otf.validators.constraints
{
    public class NameMustNotBeEmpty : SafeConstraint<RepositoryItem>
    {
        protected override IEnumerable<ConstraintViolation> SafeCheck(RepositoryItem item)
        {
            if (string.IsNullOrEmpty(item.Name))
            {
                yield return new ConstraintViolation(item.Id, item.Id, "The name of " + item.Id + " is not specified.");
            }
        }
    }
}