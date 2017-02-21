using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.otf.validators.constraints
{
    public class SubPackagesMustBeBusinessLibraries:SafeConstraint<RepositoryItem>
    {
        protected override IEnumerable<ConstraintViolation> SafeCheck(RepositoryItem item)
        {
            foreach (RepositoryItem child in item.Children)
            {
                if (child.Id.Type == ItemId.ItemType.Package && !(Stereotype.IsBusinessLibraryStereotype(child.Stereotype)))
                {
                    yield return new ConstraintViolation(item.Id, child.Id, "Sub-packages of " + item.Name + " must have a UPCC business library stereotype.");
                }
            }
        }
    }
}