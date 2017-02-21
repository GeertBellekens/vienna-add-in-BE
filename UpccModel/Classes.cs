using System.Collections.Generic;
using System.Reflection;

namespace Upcc
{
    internal class Classes
    {
        internal readonly MetaClass Abie;
        internal readonly MetaClass Acc;
        internal readonly MetaClass Bdt;
        internal readonly MetaClass Cdt;
        internal readonly MetaClass Ma;

        internal Classes(TaggedValues taggedValues)
        {
            Cdt = new MetaClass
                  {
                      Name = "Cdt",
                      Stereotype = MetaStereotype.CDT,
                      TaggedValues = new[]
                                     {
                                         taggedValues.BusinessTerm,
                                         taggedValues.Definition,
                                         taggedValues.DictionaryEntryName.WithDefaultValue("Name + \". Type\""),
                                         taggedValues.LanguageCode,
                                         taggedValues.UniqueIdentifier,
                                         taggedValues.VersionIdentifier,
                                         taggedValues.UsageRule,
                                     },
                  };

            Acc = new MetaClass
                  {
                      Name = "Acc",
                      Stereotype = MetaStereotype.ACC,
                      TaggedValues = new[]
                                     {
                                         taggedValues.BusinessTerm,
                                         taggedValues.Definition,
                                         taggedValues.DictionaryEntryName.WithDefaultValue("Name + \". Details\""),
                                         taggedValues.LanguageCode,
                                         taggedValues.UniqueIdentifier,
                                         taggedValues.VersionIdentifier,
                                         taggedValues.UsageRule,
                                     },
                  };

            Bdt = new MetaClass
                  {
                      Name = "Bdt",
                      Stereotype = MetaStereotype.BDT,
                      TaggedValues = new[]
                                     {
                                         taggedValues.BusinessTerm,
                                         taggedValues.Definition,
                                         taggedValues.DictionaryEntryName.WithDefaultValue("Name + \". Type\""),
                                         taggedValues.LanguageCode,
                                         taggedValues.UniqueIdentifier,
                                         taggedValues.VersionIdentifier,
                                         taggedValues.UsageRule,
                                     },
                  };

            Abie = new MetaClass
                   {
                       Name = "Abie",
                       Stereotype = MetaStereotype.ABIE,
                       TaggedValues = new[]
                                      {
                                          taggedValues.BusinessTerm,
                                          taggedValues.Definition,
                                          taggedValues.DictionaryEntryName.WithDefaultValue("Name + \". Details\""),
                                          taggedValues.LanguageCode,
                                          taggedValues.UniqueIdentifier,
                                          taggedValues.VersionIdentifier,
                                          taggedValues.UsageRule,
                                      },
                   };

            Ma = new MetaClass
                 {
                     Name = "Ma",
                     Stereotype = MetaStereotype.MA,
                     TaggedValues = new MetaTaggedValue[0],
                 };
        }

        internal IEnumerable<MetaClass> All
        {
            get
            {
                foreach (FieldInfo field in GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic))
                {
                    yield return (MetaClass) field.GetValue(this);
                }
            }
        }
    }
}