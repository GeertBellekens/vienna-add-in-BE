using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.otf.validators.constraints
{
    public class TaggedValuesMustBeDefined : SafeConstraint<RepositoryItem>
    {
        private readonly TaggedValues[] taggedValues;

        public TaggedValuesMustBeDefined(params TaggedValues[] taggedValues)
        {
            this.taggedValues = taggedValues;
        }

        protected override IEnumerable<ConstraintViolation> SafeCheck(RepositoryItem item)
        {
            foreach (var taggedValue in taggedValues)
            {
                if (item.GetTaggedValue(taggedValue) == null)
                {
                    yield return new ConstraintViolation(item.Id, item.Id, "Tagged value " + taggedValue + " of " + item.Name + " must be defined.");
                }
            }
        }
    }
}