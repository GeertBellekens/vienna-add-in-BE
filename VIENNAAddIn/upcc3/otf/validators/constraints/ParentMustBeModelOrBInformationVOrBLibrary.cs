using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.otf.validators.constraints
{
    public class ParentMustBeModelOrBInformationVOrBLibrary:SafeConstraint<RepositoryItem>
    {
        protected override IEnumerable<ConstraintViolation> SafeCheck(RepositoryItem item)
        {
            string parentStereotype = item.Parent.Stereotype;
            if (item.Parent == null || item.Parent.Id.IsNull || !(item.Parent.ParentId.IsNull || Stereotype.bInformationV == parentStereotype || Stereotype.bLibrary == parentStereotype))
            {
                yield return new ConstraintViolation(item.Id, item.Id, 
                                                     "The parent package of " + item.Name + 
                                                     " must be a model"+
                                                     " or have stereotype " + Stereotype.bInformationV +
                                                     " or have stereotype " + Stereotype.bLibrary + 
                                                     ".");
            }
        }
    }
}