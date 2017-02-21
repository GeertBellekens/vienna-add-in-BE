using System.Collections.Generic;
using System.Reflection;

namespace Upcc
{
    internal class EnumerationLiterals
    {
        internal readonly MetaEnumerationLiteral CodelistEntry;

        internal EnumerationLiterals(TaggedValues taggedValues, Enumerations enumerations)
        {
            CodelistEntry = new MetaEnumerationLiteral
                            {
                                Stereotype = MetaStereotype.CodelistEntry,
                                ClassName = "CodelistEntry",
                                Name = "CodelistEntries",
                                Cardinality = MetaCardinality.Many,
                                ContainingEnumerationType = enumerations.Enum,
                                TaggedValues = new[]
                                               {
                                                   taggedValues.CodeName,
                                                   taggedValues.Status,
                                               }
                            };
        }

        internal IEnumerable<MetaEnumerationLiteral> All
        {
            get
            {
                foreach (FieldInfo field in GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic))
                {
                    yield return (MetaEnumerationLiteral) field.GetValue(this);
                }
            }
        }
    }
}