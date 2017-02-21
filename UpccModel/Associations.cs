using System.Collections.Generic;
using System.Reflection;

namespace Upcc
{
    internal class Associations
    {
        internal readonly MetaAssociation Asbie;
        internal readonly MetaAssociation Ascc;
        internal readonly MetaAssociation Asma;

        internal Associations(TaggedValues taggedValues, Classes classes, MultiTypes multiTypes)
        {
            Ascc = new MetaAssociation
                   {
                       Stereotype = MetaStereotype.ASCC,
                       ClassName = "Ascc",
                       Name = "Asccs",
                       Cardinality = MetaCardinality.Many,
                       AggregationKind = MetaAggregationKind.Shared,
                       AssociatingClassifierType = classes.Acc,
                       AssociatedClassifierType = classes.Acc,
                       TaggedValues = new[]
                                      {
                                          taggedValues.BusinessTerm,
                                          taggedValues.Definition,
                                          taggedValues.DictionaryEntryName,
                                          taggedValues.LanguageCode,
                                          taggedValues.SequencingKey,
                                          taggedValues.UniqueIdentifier,
                                          taggedValues.VersionIdentifier,
                                          taggedValues.UsageRule,
                                      },
                   };

            Asbie = new MetaAssociation
                    {
                        Stereotype = MetaStereotype.ASBIE,
                        ClassName = "Asbie",
                        Name = "Asbies",
                        Cardinality = MetaCardinality.Many,
                        AggregationKind = MetaAggregationKind.SharedOrComposite,
                        AssociatingClassifierType = classes.Abie,
                        AssociatedClassifierType = classes.Abie,
                        TaggedValues = new[]
                                       {
                                           taggedValues.BusinessTerm,
                                           taggedValues.Definition,
                                           taggedValues.DictionaryEntryName,
                                           taggedValues.LanguageCode,
                                           taggedValues.SequencingKey,
                                           taggedValues.UniqueIdentifier,
                                           taggedValues.VersionIdentifier,
                                           taggedValues.UsageRule,
                                       },
                    };

            Asma = new MetaAssociation
                     {
                         Stereotype = MetaStereotype.ASMA,
                         ClassName = "Asma",
                         Name = "Asmas",
                         Cardinality = MetaCardinality.Many,
                         AggregationKind = MetaAggregationKind.Shared,
                         AssociatingClassifierType = classes.Ma,
                         AssociatedClassifierType = multiTypes.BieAggregator,
                         TaggedValues = new MetaTaggedValue[0],
                     };
        }

        internal IEnumerable<MetaAssociation> All
        {
            get
            {
                foreach (FieldInfo field in GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic))
                {
                    yield return (MetaAssociation) field.GetValue(this);
                }
            }
        }
    }
}