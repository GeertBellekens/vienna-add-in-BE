using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.otf
{
    public interface IValidator
    {
        bool Matches(object item);
        IEnumerable<ConstraintViolation> Validate(object item);
    }
}