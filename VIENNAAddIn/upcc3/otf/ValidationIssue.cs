namespace VIENNAAddIn.upcc3.otf
{
    /// <summary>
    /// A ValidationIssue is a wrapper around a ConstraintViolation, providing a unique ID within the context of a ValidationService.
    /// </summary>
    public class ValidationIssue
    {
        public ValidationIssue(int id, ConstraintViolation constraintViolation)
        {
            Id = id;
            ConstraintViolation = constraintViolation;
        }

        /// <summary>
        /// A unique ID of this issue.
        /// </summary>
        public int Id { get; private set; }

        public ConstraintViolation ConstraintViolation { get; private set; }
    }
}