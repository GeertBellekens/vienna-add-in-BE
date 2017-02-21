using System;
using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.otf.validators.constraints
{
    /// <summary>
    /// A constraint that matches non-null objects of a given type.
    /// 
    /// Sub-class this abstract class to implement type- and null-safe constraints.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class SafeConstraint<T> : IConstraint where T : class
    {
        public IEnumerable<ConstraintViolation> Check(object item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item", "This indicates a programming error: Check() was called without checking that the validator matches the item.");
            }
            if (!(item is T))
            {
                throw new ArgumentException("Item is not of expected type. This indicates a programming error: Check() was called without checking that the validator matches the item.", "item");
            }
            return SafeCheck(item as T);
        }

        protected abstract IEnumerable<ConstraintViolation> SafeCheck(T item);
    }
}