namespace VIENNAAddIn.upcc3.uml
{
    public interface IUmlTaggedValue
    {
        /// <summary>
        /// <c>True</c> if the value is defined for the UML element.
        /// </summary>
        bool IsDefined { get; }

        string Name { get; }

        /// <summary>
        /// The value of the tagged value. If the tagged value is not defined for the UML element, an empty string is returned.
        /// </summary>
        string Value { get; }

        /// <summary>
        /// Split the tagged value's value using <see cref="MultiPartTaggedValue.ValueSeparator"/>. If the tagged value is not defined for the UML element,
        /// an empty array is returned.
        /// </summary>
        string[] SplitValues { get; }

        void Update(UmlTaggedValueSpec spec);
    }
}