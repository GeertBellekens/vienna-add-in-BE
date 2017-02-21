using System;

namespace VIENNAAddIn.upcc3.otf
{
    public class ConstraintViolation : IEquatable<ConstraintViolation>
    {
        public ConstraintViolation(ItemId validatedItemId, ItemId offendingItemId, string message)
        {
            ValidatedItemId = validatedItemId;
            OffendingItemId = offendingItemId;
            Message = message;
        }

        /// <summary>
        /// The ID of the item that was validated.
        /// </summary>
        public ItemId ValidatedItemId { get; private set; }

        /// <summary>
        /// The ID of the item that actually violates the constraint. E.g. if a PRIMLibrary is validated and contains a non-PRIM element, 
        /// the ValidatedItemId is the library's ID, whereas the OffendingItemId is the ID of the element.
        /// </summary>
        public ItemId OffendingItemId { get; private set; }

        public string Message { get; private set; }

        public bool Equals(ConstraintViolation other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.ValidatedItemId, ValidatedItemId) && Equals(other.OffendingItemId, OffendingItemId) && Equals(other.Message, Message);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (ConstraintViolation)) return false;
            return Equals((ConstraintViolation) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = (ValidatedItemId != null ? ValidatedItemId.GetHashCode() : 0);
                result = (result*397) ^ (OffendingItemId != null ? OffendingItemId.GetHashCode() : 0);
                result = (result*397) ^ (Message != null ? Message.GetHashCode() : 0);
                return result;
            }
        }

        public static bool operator ==(ConstraintViolation left, ConstraintViolation right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ConstraintViolation left, ConstraintViolation right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return "Constraint violation: " + Message;
        }
    }
}