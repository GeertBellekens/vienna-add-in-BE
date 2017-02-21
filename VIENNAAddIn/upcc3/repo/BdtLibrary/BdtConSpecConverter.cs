// ReSharper disable RedundantUsingDirective
using CctsRepository;
using CctsRepository.BdtLibrary;
using CctsRepository.BieLibrary;
using CctsRepository.BLibrary;
using CctsRepository.CcLibrary;
using CctsRepository.CdtLibrary;
using CctsRepository.DocLibrary;
using CctsRepository.EnumLibrary;
using CctsRepository.PrimLibrary;
using VIENNAAddIn.upcc3.repo;
using VIENNAAddIn.upcc3.repo.BdtLibrary;
using VIENNAAddIn.upcc3.repo.BieLibrary;
using VIENNAAddIn.upcc3.repo.BLibrary;
using VIENNAAddIn.upcc3.repo.CcLibrary;
using VIENNAAddIn.upcc3.repo.CdtLibrary;
using VIENNAAddIn.upcc3.repo.DocLibrary;
using VIENNAAddIn.upcc3.repo.EnumLibrary;
using VIENNAAddIn.upcc3.repo.PrimLibrary;
// ReSharper restore RedundantUsingDirective
using VIENNAAddIn.upcc3.uml;

namespace VIENNAAddIn.upcc3.repo.BdtLibrary
{
    internal static partial class BdtConSpecConverter
    {
		internal static UmlAttributeSpec Convert(BdtConSpec bdtConSpec, string className)
		{
			IUmlClassifier type;
			var multiType = bdtConSpec.BasicType;
            if (multiType.IsPrim)
            {
                type = ((UpccPrim) multiType.Prim).UmlDataType;
			}
			else
            if (multiType.IsIdScheme)
            {
                type = ((UpccIdScheme) multiType.IdScheme).UmlDataType;
			}
			else
            if (multiType.IsEnum)
            {
                type = ((UpccEnum) multiType.Enum).UmlEnumeration;
			}
			else
			{
				type = null;
			}
			var umlAttributeSpec = new UmlAttributeSpec
				{
					Stereotype = "CON",
					Name = bdtConSpec.Name,
					Type = type,
					UpperBound = bdtConSpec.UpperBound,
					LowerBound = bdtConSpec.LowerBound,
					TaggedValues = new[]
						{
							new UmlTaggedValueSpec("businessTerm", bdtConSpec.BusinessTerms) ,
							new UmlTaggedValueSpec("definition", bdtConSpec.Definition) ,
							new UmlTaggedValueSpec("dictionaryEntryName", bdtConSpec.DictionaryEntryName) { DefaultValue = GenerateDictionaryEntryNameDefaultValue(bdtConSpec, className) },
							new UmlTaggedValueSpec("enumeration", bdtConSpec.Enumeration) ,
							new UmlTaggedValueSpec("fractionDigits", bdtConSpec.FractionDigits) ,
							new UmlTaggedValueSpec("languageCode", bdtConSpec.LanguageCode) ,
							new UmlTaggedValueSpec("maximumExclusive", bdtConSpec.MaximumExclusive) ,
							new UmlTaggedValueSpec("maximumInclusive", bdtConSpec.MaximumInclusive) ,
							new UmlTaggedValueSpec("maximumLength", bdtConSpec.MaximumLength) ,
							new UmlTaggedValueSpec("minimumExclusive", bdtConSpec.MinimumExclusive) ,
							new UmlTaggedValueSpec("minimumInclusive", bdtConSpec.MinimumInclusive) ,
							new UmlTaggedValueSpec("minimumLength", bdtConSpec.MinimumLength) ,
							new UmlTaggedValueSpec("modificationAllowedIndicator", bdtConSpec.ModificationAllowedIndicator) ,
							new UmlTaggedValueSpec("pattern", bdtConSpec.Pattern) ,
							new UmlTaggedValueSpec("totalDigits", bdtConSpec.TotalDigits) ,
							new UmlTaggedValueSpec("uniqueIdentifier", bdtConSpec.UniqueIdentifier) { DefaultValue = GenerateUniqueIdentifierDefaultValue(bdtConSpec, className) },
							new UmlTaggedValueSpec("usageRule", bdtConSpec.UsageRules) ,
							new UmlTaggedValueSpec("versionIdentifier", bdtConSpec.VersionIdentifier) ,
						},
	
				};

			return umlAttributeSpec;
		}
	}
}

