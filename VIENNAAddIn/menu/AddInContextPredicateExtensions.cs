using System;

namespace VIENNAAddIn.menu
{
    ///<summary>
    /// Extension methods for combining AddInContext predicates.
    ///</summary>
    public static class AddInContextPredicateExtensions
    {
        ///<summary>
        ///</summary>
        ///<param name="lhs"></param>
        ///<param name="rhs"></param>
        ///<returns></returns>
        public static Predicate<AddInContext> And
            (this Predicate<AddInContext> lhs, Predicate<AddInContext> rhs)
        {
            if (lhs == null)
            {
                return rhs;
            }
            if (rhs == null)
            {
                return lhs;
            }
            return context => lhs(context) && rhs(context);
        }

        ///<summary>
        ///</summary>
        ///<param name="lhs"></param>
        ///<param name="rhs"></param>
        ///<returns></returns>
        public static Predicate<AddInContext> Or
            (this Predicate<AddInContext> lhs, Predicate<AddInContext> rhs)
        {
            if (lhs == null)
            {
                return rhs;
            }
            if (rhs == null)
            {
                return lhs;
            }
            return context => lhs(context) || rhs(context);
        }
    }
}