using System.Collections.Generic;
using System.Reflection;

namespace Upcc
{
    internal class Attributes
    {
        internal readonly MetaAttribute Bbie;
        internal readonly MetaAttribute Bcc;
        internal readonly MetaAttribute BdtCon;
        internal readonly MetaAttribute BdtSup;
        internal readonly MetaAttribute CdtCon;
        internal readonly MetaAttribute CdtSup;

        internal Attributes(TaggedValues taggedValues, Classes classes, MultiTypes multiTypes)
        {
            CdtCon = new MetaAttribute
                     {
                         Stereotype = MetaStereotype.CON,
                         ContainingClassifierType = classes.Cdt,
                         ClassName = "CdtCon",
                         AttributeName = "Con",
                         Type = multiTypes.BasicType,
                         Cardinality = MetaCardinality.One,
                         TaggedValues = new[]
                                        {
                                            taggedValues.BusinessTerm,
                                            taggedValues.Definition,
                                            taggedValues.DictionaryEntryName.WithDefaultValue("Cdt.Name + \". Content\""),
                                            taggedValues.LanguageCode,
                                            taggedValues.ModificationAllowedIndicator,
                                            taggedValues.UniqueIdentifier,
                                            taggedValues.VersionIdentifier,
                                            taggedValues.UsageRule,
                                        },
                     };

            CdtSup = new MetaAttribute
                     {
                         Stereotype = MetaStereotype.SUP,
                         ContainingClassifierType = classes.Cdt,
                         ClassName = "CdtSup",
                         AttributeName = "Sups",
                         Type = multiTypes.BasicType,
                         Cardinality = MetaCardinality.Many,
                         TaggedValues = new[]
                                        {
                                            taggedValues.BusinessTerm,
                                            taggedValues.Definition,
                                            taggedValues.DictionaryEntryName.WithDefaultValue("Cdt.Name + \". \" + Name + \". \" + Type.Name"),
                                            taggedValues.LanguageCode,
                                            taggedValues.ModificationAllowedIndicator,
                                            taggedValues.UniqueIdentifier,
                                            taggedValues.VersionIdentifier,
                                            taggedValues.UsageRule,
                                        },
                     };

            Bcc = new MetaAttribute
                  {
                      Stereotype = MetaStereotype.BCC,
                      ContainingClassifierType = classes.Acc,
                      ClassName = "Bcc",
                      AttributeName = "Bccs",
                      Type = classes.Cdt,
                      Cardinality = MetaCardinality.Many,
                      TaggedValues = new[]
                                     {
                                         taggedValues.BusinessTerm,
                                         taggedValues.Definition,
                                         taggedValues.DictionaryEntryName.WithDefaultValue("Acc.Name + \". \" + Name + \". \" + Type.Name"),
                                         taggedValues.LanguageCode,
                                         taggedValues.SequencingKey,
                                         taggedValues.UniqueIdentifier,
                                         taggedValues.VersionIdentifier,
                                         taggedValues.UsageRule,
                                     },
                  };

            BdtCon = new MetaAttribute
                     {
                         Stereotype = MetaStereotype.CON,
                         ContainingClassifierType = classes.Bdt,
                         ClassName = "BdtCon",
                         AttributeName = "Con",
                         Type = multiTypes.BasicType,
                         Cardinality = MetaCardinality.One,
                         TaggedValues = new[]
                                        {
                                            taggedValues.BusinessTerm,
                                            taggedValues.Definition,
                                            taggedValues.DictionaryEntryName.WithDefaultValue("Bdt.Name + \". Content\""),
                                            taggedValues.Enumeration,
                                            taggedValues.FractionDigits,
                                            taggedValues.LanguageCode,
                                            taggedValues.MaximumExclusive,
                                            taggedValues.MaximumInclusive,
                                            taggedValues.MaximumLength,
                                            taggedValues.MinimumExclusive,
                                            taggedValues.MinimumInclusive,
                                            taggedValues.MinimumLength,
                                            taggedValues.ModificationAllowedIndicator,
                                            taggedValues.Pattern,
                                            taggedValues.TotalDigits,
                                            taggedValues.UniqueIdentifier,
                                            taggedValues.UsageRule,
                                            taggedValues.VersionIdentifier,
                                        },
                     };

            BdtSup = new MetaAttribute
                     {
                         Stereotype = MetaStereotype.SUP,
                         ContainingClassifierType = classes.Bdt,
                         ClassName = "BdtSup",
                         AttributeName = "Sups",
                         Type = multiTypes.BasicType,
                         Cardinality = MetaCardinality.Many,
                         TaggedValues = new[]
                                        {
                                            taggedValues.BusinessTerm,
                                            taggedValues.Definition,
                                            taggedValues.DictionaryEntryName.WithDefaultValue("Bdt.Name + \". \" + Name + \". \" + Type.Name"),
                                            taggedValues.Enumeration,
                                            taggedValues.FractionDigits,
                                            taggedValues.LanguageCode,
                                            taggedValues.MaximumExclusive,
                                            taggedValues.MaximumInclusive,
                                            taggedValues.MaximumLength,
                                            taggedValues.MinimumExclusive,
                                            taggedValues.MinimumInclusive,
                                            taggedValues.MinimumLength,
                                            taggedValues.ModificationAllowedIndicator,
                                            taggedValues.Pattern,
                                            taggedValues.TotalDigits,
                                            taggedValues.UniqueIdentifier,
                                            taggedValues.UsageRule,
                                            taggedValues.VersionIdentifier,
                                        },
                     };

            Bbie = new MetaAttribute
                   {
                       Stereotype = MetaStereotype.BBIE,
                       ContainingClassifierType = classes.Abie,
                       ClassName = "Bbie",
                       AttributeName = "Bbies",
                       Type = classes.Bdt,
                       Cardinality = MetaCardinality.Many,
                       TaggedValues = new[]
                                      {
                                          taggedValues.BusinessTerm,
                                          taggedValues.Definition,
                                          taggedValues.DictionaryEntryName.WithDefaultValue("Abie.Name + \". \" + Name + \". \" + Type.Name"),
                                          taggedValues.LanguageCode,
                                          taggedValues.SequencingKey,
                                          taggedValues.UniqueIdentifier,
                                          taggedValues.VersionIdentifier,
                                          taggedValues.UsageRule,
                                      },
                   };
        }

        internal IEnumerable<MetaAttribute> All
        {
            get
            {
                foreach (FieldInfo field in GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic))
                {
                    yield return (MetaAttribute) field.GetValue(this);
                }
            }
        }
    }
}