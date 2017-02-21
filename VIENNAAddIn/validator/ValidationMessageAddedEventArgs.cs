using System;
using System.Collections.Generic;

namespace VIENNAAddIn.validator
{
    internal class ValidationMessageAddedEventArgs : EventArgs
    {
        public ValidationMessageAddedEventArgs(IList<ValidationMessage> messages)
        {
            Messages = messages;
        }

        public ValidationMessageAddedEventArgs(ValidationMessage message)
        {
            Messages = new[] {message};
        }

        public IList<ValidationMessage> Messages { get; private set; }
    }
}