using System;
using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.otf.validators
{
    /// <summary>
    /// A validator that matches non-null objects of a given type.
    /// 
    /// Sub-class this abstract class to implement type- and null-safe validators.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class SafeValidator<T> : IValidator where T : class
    {
        public bool Matches(object item)
        {
            if (item != null && item is T)
            {
                return SafeMatches(item as T);
            }
            return false;
        }

        protected abstract bool SafeMatches(T item);

        public IEnumerable<ConstraintViolation> Validate(object item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item", "This indicates a programming error: Check() was called without checking that this constraint Matches().");
            }
            if (!(item is T))
            {
                throw new ArgumentException("Item is not of expected type. This indicates a programming error: Check() was called without checking that this constraint Matches().", "item");
            }
            return SafeValidate(item as T);
        }

        protected abstract IEnumerable<ConstraintViolation> SafeValidate(T item);
    }
}