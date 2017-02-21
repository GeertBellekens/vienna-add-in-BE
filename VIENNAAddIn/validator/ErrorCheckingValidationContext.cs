using EA;

namespace VIENNAAddIn.validator
{
    internal class ErrorCheckingValidationContext : IValidationContext
    {
        private readonly IValidationContext parentContext;
        public bool HasError { get; private set; }

        public ErrorCheckingValidationContext(IValidationContext parentContext)
        {
            this.parentContext = parentContext;
        }

        public Repository Repository
        {
            get { return parentContext.Repository; }
        }

        public void AddValidationMessage(ValidationMessage message)
        {
            if (message.ErrorLevel == ValidationMessage.errorLevelTypes.ERROR)
            {
                HasError = true;
            }
            parentContext.AddValidationMessage(message);
        }
    }
}