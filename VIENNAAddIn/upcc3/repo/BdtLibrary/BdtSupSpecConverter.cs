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
    internal static partial class BdtSupSpecConverter
    {
		internal static UmlAttributeSpec Convert(BdtSupSpec bdtSupSpec, string className)
		{
			IUmlClassifier type;
			var multiType = bdtSupSpec.BasicType;
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
					Stereotype = "SUP",
					Name = bdtSupSpec.Name,
					Type = type,
					UpperBound = bdtSupSpec.UpperBound,
					LowerBound = bdtSupSpec.LowerBound,
					TaggedValues = new[]
						{
							new UmlTaggedValueSpec("businessTerm", bdtSupSpec.BusinessTerms) ,
							new UmlTaggedValueSpec("definition", bdtSupSpec.Definition) ,
							new UmlTaggedValueSpec("dictionaryEntryName", bdtSupSpec.DictionaryEntryName) { DefaultValue = GenerateDictionaryEntryNameDefaultValue(bdtSupSpec, className) },
							new UmlTaggedValueSpec("enumeration", bdtSupSpec.Enumeration) ,
							new UmlTaggedValueSpec("fractionDigits", bdtSupSpec.FractionDigits) ,
							new UmlTaggedValueSpec("languageCode", bdtSupSpec.LanguageCode) ,
							new UmlTaggedValueSpec("maximumExclusive", bdtSupSpec.MaximumExclusive) ,
							new UmlTaggedValueSpec("maximumInclusive", bdtSupSpec.MaximumInclusive) ,
							new UmlTaggedValueSpec("maximumLength", bdtSupSpec.MaximumLength) ,
							new UmlTaggedValueSpec("minimumExclusive", bdtSupSpec.MinimumExclusive) ,
							new UmlTaggedValueSpec("minimumInclusive", bdtSupSpec.MinimumInclusive) ,
							new UmlTaggedValueSpec("minimumLength", bdtSupSpec.MinimumLength) ,
							new UmlTaggedValueSpec("modificationAllowedIndicator", bdtSupSpec.ModificationAllowedIndicator) ,
							new UmlTaggedValueSpec("pattern", bdtSupSpec.Pattern) ,
							new UmlTaggedValueSpec("totalDigits", bdtSupSpec.TotalDigits) ,
							new UmlTaggedValueSpec("uniqueIdentifier", bdtSupSpec.UniqueIdentifier) { DefaultValue = GenerateUniqueIdentifierDefaultValue(bdtSupSpec, className) },
							new UmlTaggedValueSpec("usageRule", bdtSupSpec.UsageRules) ,
							new UmlTaggedValueSpec("versionIdentifier", bdtSupSpec.VersionIdentifier) ,
						},
	
				};

			return umlAttributeSpec;
		}
	}
}

