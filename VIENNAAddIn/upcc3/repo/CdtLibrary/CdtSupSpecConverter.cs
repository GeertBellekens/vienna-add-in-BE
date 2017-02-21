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

namespace VIENNAAddIn.upcc3.repo.CdtLibrary
{
    internal static partial class CdtSupSpecConverter
    {
		internal static UmlAttributeSpec Convert(CdtSupSpec cdtSupSpec, string className)
		{
			IUmlClassifier type;
			var multiType = cdtSupSpec.BasicType;
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
					Name = cdtSupSpec.Name,
					Type = type,
					UpperBound = cdtSupSpec.UpperBound,
					LowerBound = cdtSupSpec.LowerBound,
					TaggedValues = new[]
						{
							new UmlTaggedValueSpec("businessTerm", cdtSupSpec.BusinessTerms) ,
							new UmlTaggedValueSpec("definition", cdtSupSpec.Definition) ,
							new UmlTaggedValueSpec("dictionaryEntryName", cdtSupSpec.DictionaryEntryName) { DefaultValue = GenerateDictionaryEntryNameDefaultValue(cdtSupSpec, className) },
							new UmlTaggedValueSpec("languageCode", cdtSupSpec.LanguageCode) ,
							new UmlTaggedValueSpec("modificationAllowedIndicator", cdtSupSpec.ModificationAllowedIndicator) ,
							new UmlTaggedValueSpec("uniqueIdentifier", cdtSupSpec.UniqueIdentifier) { DefaultValue = GenerateUniqueIdentifierDefaultValue(cdtSupSpec, className) },
							new UmlTaggedValueSpec("versionIdentifier", cdtSupSpec.VersionIdentifier) ,
							new UmlTaggedValueSpec("usageRule", cdtSupSpec.UsageRules) ,
						},
	
				};

			return umlAttributeSpec;
		}
	}
}

