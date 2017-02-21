using System.Collections.Generic;
using VIENNAAddIn.upcc3.otf.validators.constraints;

namespace VIENNAAddIn.upcc3.otf.validators
{
    public abstract class ConstraintBasedValidator<T> : SafeValidator<T> where T : class
    {
        private readonly List<SafeConstraint<T>> constraints = new List<SafeConstraint<T>>();

        protected void AddConstraint(SafeConstraint<T> constraint)
        {
            constraints.Add(constraint);
        }

        protected override IEnumerable<ConstraintViolation> SafeValidate(T item)
        {
            foreach (var constraint in constraints)
            {
                foreach(var constraintViolation in constraint.Check(item))
                {
                    yield return constraintViolation;
                }
            }
        }
    }
}