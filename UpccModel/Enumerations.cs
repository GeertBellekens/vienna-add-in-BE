using System.Collections.Generic;
using System.Reflection;

namespace Upcc
{
    internal class Enumerations
    {
        internal readonly MetaEnumeration Enum;

        internal Enumerations(TaggedValues taggedValues)
        {
            Enum = new MetaEnumeration
                   {
                       Name = "Enum",
                       Stereotype = MetaStereotype.ENUM,
                       TaggedValues = new[]
                                      {
                                          taggedValues.BusinessTerm,
                                          taggedValues.CodeListAgencyIdentifier,
                                          taggedValues.CodeListAgencyName,
                                          taggedValues.CodeListIdentifier,
                                          taggedValues.CodeListName,
                                          taggedValues.DictionaryEntryName,
                                          taggedValues.Definition,
                                          taggedValues.EnumerationUri,
                                          taggedValues.LanguageCode,
                                          taggedValues.ModificationAllowedIndicator,
                                          taggedValues.RestrictedPrimitive,
                                          taggedValues.Status,
                                          taggedValues.UniqueIdentifier,
                                          taggedValues.VersionIdentifier,
                                      }
                   };
        }

        internal IEnumerable<MetaEnumeration> All
        {
            get
            {
                foreach (FieldInfo field in GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic))
                {
                    yield return (MetaEnumeration) field.GetValue(this);
                }
            }
        }
    }
}