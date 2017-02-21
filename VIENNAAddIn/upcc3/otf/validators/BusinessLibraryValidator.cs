using VIENNAAddIn.upcc3.otf.validators.constraints;

namespace VIENNAAddIn.upcc3.otf.validators
{
    public abstract class BusinessLibraryValidator : ConstraintBasedValidator<RepositoryItem>
    {
        private readonly string stereotype;

        protected BusinessLibraryValidator(string stereotype)
        {
            this.stereotype = stereotype;
            AddConstraint(new NameMustNotBeEmpty());
            AddConstraint(new TaggedValuesMustBeDefined(
                              TaggedValues.businessTerm,
                              TaggedValues.copyright,
                              TaggedValues.owner,
                              TaggedValues.reference,
                              TaggedValues.status,
                              TaggedValues.namespacePrefix
                              ));
            AddConstraint(new TaggedValuesMustNotBeEmpty(
                              TaggedValues.baseURN, 
                              TaggedValues.uniqueIdentifier, 
                              TaggedValues.versionIdentifier
                              ));
        }

        protected override bool SafeMatches(RepositoryItem item)
        {
            return item.Id.IsPackage && item.Stereotype == stereotype;
        }
    }
}