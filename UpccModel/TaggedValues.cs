using System.Collections.Generic;
using System.Reflection;

namespace Upcc
{
    internal class TaggedValues
    {
        internal readonly MetaTaggedValue BaseUrn = new MetaTaggedValue("baseURN");

        internal readonly MetaTaggedValue BusinessTerm = new MetaTaggedValue("businessTerm")
                                                       {
                                                           Cardinality = MetaCardinality.Many,
                                                       };

        internal readonly MetaTaggedValue CodeListAgencyIdentifier = new MetaTaggedValue("codeListAgencyIdentifier")
                                                                   {
                                                                       Cardinality = MetaCardinality.ZeroOrOne,
                                                                   };

        internal readonly MetaTaggedValue CodeListAgencyName = new MetaTaggedValue("codeListAgencyName")
                                                             {
                                                                 Cardinality = MetaCardinality.ZeroOrOne,
                                                             };

        internal readonly MetaTaggedValue CodeListIdentifier = new MetaTaggedValue("codeListIdentifier")
                                                             {
                                                                 Cardinality = MetaCardinality.ZeroOrOne,
                                                             };

        internal readonly MetaTaggedValue CodeListName = new MetaTaggedValue("codeListName")
                                                       {
                                                           Cardinality = MetaCardinality.ZeroOrOne,
                                                       };

        internal readonly MetaTaggedValue CodeName = new MetaTaggedValue("codeName");

        internal readonly MetaTaggedValue Copyright = new MetaTaggedValue("copyright")
                                                    {
                                                        Cardinality = MetaCardinality.Many,
                                                    };

        internal readonly MetaTaggedValue Definition = new MetaTaggedValue("definition");

        internal readonly MetaTaggedValue DictionaryEntryName = new MetaTaggedValue("dictionaryEntryName")
                                                                    {
                                                                        AutoGenerated = true,
                                                                    };

        internal readonly MetaTaggedValue EnumerationUri = new MetaTaggedValue("enumerationURI")
                                                         {
                                                             Cardinality = MetaCardinality.ZeroOrOne,
                                                         };

        internal readonly MetaTaggedValue Enumeration = new MetaTaggedValue("enumeration")
                                                         {
                                                             Cardinality = MetaCardinality.ZeroOrOne,
                                                         };

        internal readonly MetaTaggedValue FractionDigits = new MetaTaggedValue("fractionDigits");

        internal readonly MetaTaggedValue IdentifierSchemeAgencyIdentifier = new MetaTaggedValue("identifierSchemeAgencyIdentifier")
                                                                           {
                                                                               Cardinality = MetaCardinality.ZeroOrOne,
                                                                           };

        internal readonly MetaTaggedValue IdentifierSchemeAgencyName = new MetaTaggedValue("identifierSchemeAgencyName")
                                                                     {
                                                                         Cardinality = MetaCardinality.ZeroOrOne,
                                                                     };

        internal readonly MetaTaggedValue LanguageCode = new MetaTaggedValue("languageCode")
                                                       {
                                                           Cardinality = MetaCardinality.ZeroOrOne,
                                                       };

        internal readonly MetaTaggedValue Length = new MetaTaggedValue("length");

        internal readonly MetaTaggedValue MaximumExclusive = new MetaTaggedValue("maximumExclusive");

        internal readonly MetaTaggedValue MaximumInclusive = new MetaTaggedValue("maximumInclusive");

        internal readonly MetaTaggedValue MaximumLength = new MetaTaggedValue("maximumLength");

        internal readonly MetaTaggedValue MinimumExclusive = new MetaTaggedValue("minimumExclusive");

        internal readonly MetaTaggedValue MinimumInclusive = new MetaTaggedValue("minimumInclusive");

        internal readonly MetaTaggedValue MinimumLength = new MetaTaggedValue("minimumLength");

        internal readonly MetaTaggedValue ModificationAllowedIndicator = new MetaTaggedValue("modificationAllowedIndicator");

        internal readonly MetaTaggedValue NamespacePrefix = new MetaTaggedValue("namespacePrefix")
                                                          {
                                                              Cardinality = MetaCardinality.ZeroOrOne,
                                                          };

        internal readonly MetaTaggedValue Owner = new MetaTaggedValue("owner")
                                                {
                                                    Cardinality = MetaCardinality.Many,
                                                };

        internal readonly MetaTaggedValue Pattern = new MetaTaggedValue("pattern");

        internal readonly MetaTaggedValue Reference = new MetaTaggedValue("reference")
                                                    {
                                                        Cardinality = MetaCardinality.Many,
                                                    };

        internal readonly MetaTaggedValue RestrictedPrimitive = new MetaTaggedValue("restrictedPrimitive");

        internal readonly MetaTaggedValue SequencingKey = new MetaTaggedValue("sequencingKey")
                                                        {
                                                            Cardinality = MetaCardinality.ZeroOrOne,
                                                        };

        internal readonly MetaTaggedValue Status = new MetaTaggedValue("status")
                                                 {
                                                     Cardinality = MetaCardinality.ZeroOrOne,
                                                 };

        internal readonly MetaTaggedValue TotalDigits = new MetaTaggedValue("totalDigits");

        internal readonly MetaTaggedValue UniqueIdentifier = new MetaTaggedValue("uniqueIdentifier")
                                                           {
                                                               Cardinality = MetaCardinality.ZeroOrOne,
                                                               AutoGenerated = true,
                                                           };

        internal readonly MetaTaggedValue UsageRule = new MetaTaggedValue("usageRule")
                                                    {
                                                        Cardinality = MetaCardinality.Many,
                                                    };

        internal readonly MetaTaggedValue VersionIdentifier = new MetaTaggedValue("versionIdentifier")
                                                            {
                                                                Cardinality = MetaCardinality.ZeroOrOne,
                                                            };

        internal readonly MetaTaggedValue WhiteSpace = new MetaTaggedValue("whiteSpace");

        internal IEnumerable<MetaTaggedValue> All
        {
            get
            {
                foreach (FieldInfo field in GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic))
                {
                    yield return (MetaTaggedValue) field.GetValue(this);
                }
            }
        }
    }
}