using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.otf.validators.constraints
{
    public class MustNotContainAnyElements: SafeConstraint<RepositoryItem>
    {
        protected override IEnumerable<ConstraintViolation> SafeCheck(RepositoryItem item)
        {
            foreach (RepositoryItem child in item.Children)
            {
                if (child.Id.Type == ItemId.ItemType.Element)
                {
                    yield return new ConstraintViolation(item.Id, child.Id, item.Name + " must not contain any elements.");
                }
            }
        }
    }
}