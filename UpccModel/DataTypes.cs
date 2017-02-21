using System.Collections.Generic;
using System.Reflection;

namespace Upcc
{
    internal class DataTypes
    {
        internal readonly MetaDataType IdScheme;
        internal readonly MetaDataType Prim;

        internal DataTypes(TaggedValues taggedValues)
        {
            Prim = new MetaDataType
                   {
                       Name = "Prim",
                       Stereotype = MetaStereotype.PRIM,
                       TaggedValues = new[]
                                      {
                                          taggedValues.BusinessTerm,
                                          taggedValues.Definition,
                                          taggedValues.DictionaryEntryName.WithDefaultValue("Name"),
                                          taggedValues.FractionDigits,
                                          taggedValues.LanguageCode,
                                          taggedValues.Length,
                                          taggedValues.MaximumExclusive,
                                          taggedValues.MaximumInclusive,
                                          taggedValues.MaximumLength,
                                          taggedValues.MinimumExclusive,
                                          taggedValues.MinimumInclusive,
                                          taggedValues.MinimumLength,
                                          taggedValues.Pattern,
                                          taggedValues.TotalDigits,
                                          taggedValues.UniqueIdentifier,
                                          taggedValues.VersionIdentifier,
                                          taggedValues.WhiteSpace,
                                      },
                   };

            IdScheme = new MetaDataType
                       {
                           Name = "IdScheme",
                           Stereotype = MetaStereotype.IDSCHEME,
                           TaggedValues = new[]
                                          {
                                              taggedValues.BusinessTerm,
                                              taggedValues.Definition,
                                              taggedValues.DictionaryEntryName.WithDefaultValue("Name"),
                                              taggedValues.IdentifierSchemeAgencyIdentifier,
                                              taggedValues.IdentifierSchemeAgencyName,
                                              taggedValues.ModificationAllowedIndicator,
                                              taggedValues.Pattern,
                                              taggedValues.RestrictedPrimitive,
                                              taggedValues.UniqueIdentifier,
                                              taggedValues.VersionIdentifier,
                                          },
                       };
        }

        internal IEnumerable<MetaDataType> All
        {
            get
            {
                foreach (FieldInfo field in GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic))
                {
                    yield return (MetaDataType) field.GetValue(this);
                }
            }
        }
    }
}