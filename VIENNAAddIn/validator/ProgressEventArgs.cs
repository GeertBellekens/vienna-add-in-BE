using System;
using System.Collections.Generic;

namespace VIENNAAddIn.validator
{
    class ProgressEventArgs: EventArgs
    {
        public List<ValidationMessage> Messages { get; private set; }
        public short PercentDone { get; private set; }

        public ProgressEventArgs(List<ValidationMessage> messages, short percentDone)
        {
            Messages = messages;
            PercentDone = percentDone;
        }
    }
}