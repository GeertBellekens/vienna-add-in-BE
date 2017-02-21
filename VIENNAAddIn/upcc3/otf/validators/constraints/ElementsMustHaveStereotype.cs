using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.otf.validators.constraints
{
    public class ElementsMustHaveStereotype : SafeConstraint<RepositoryItem>
    {
        private readonly string stereotype;

        public ElementsMustHaveStereotype(string stereotype)
        {
            this.stereotype = stereotype;
        }

        protected override IEnumerable<ConstraintViolation> SafeCheck(RepositoryItem item)
        {
            foreach (RepositoryItem child in item.Children)
            {
                if (child.Id.Type == ItemId.ItemType.Element)
                {
                    if (child.Stereotype != stereotype)
                    {
                        yield return new ConstraintViolation(item.Id, child.Id, "Elements of " + item.Name + " must have stereotype '" + stereotype + "'.");
                    }
                }
            }
        }
    }
}