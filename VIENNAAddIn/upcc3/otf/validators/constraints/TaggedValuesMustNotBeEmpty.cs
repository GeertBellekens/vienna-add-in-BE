using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.otf.validators.constraints
{
    public class TaggedValuesMustNotBeEmpty : SafeConstraint<RepositoryItem>
    {
        private readonly TaggedValues[] taggedValues;

        public TaggedValuesMustNotBeEmpty(params TaggedValues[] taggedValues)
        {
            this.taggedValues = taggedValues;
        }

        protected override IEnumerable<ConstraintViolation> SafeCheck(RepositoryItem item)
        {
            foreach (var taggedValue in taggedValues)
            {
                if (string.IsNullOrEmpty(item.GetTaggedValue(taggedValue)))
                {
                    yield return new ConstraintViolation(item.Id, item.Id, "Tagged value " + taggedValue + " of " + item.Name + " must not be empty.");
                }
            }
        }
    }
}