using System;
using EA;

namespace VIENNAAddIn.validator
{
    internal interface IValidationContext
    {
        Repository Repository { get; }
        void AddValidationMessage(ValidationMessage message);
    }

    /// <summary>
    /// This class encapsulates the event processing logic for validator events.
    /// </summary>
    internal class ValidationContext : IValidationContext
    {
        public Repository Repository { get; private set; }

        public ValidationContext(Repository repository)
        {
            Repository = repository;
        }

        public event EventHandler<ValidationMessageAddedEventArgs> ValidationMessageAdded;

        public void AddValidationMessage(ValidationMessage message)
        {
            ValidationMessageAdded(this, new ValidationMessageAddedEventArgs(message));
        }
    }
}