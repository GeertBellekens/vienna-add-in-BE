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
    internal static partial class CdtConSpecConverter
    {
		internal static UmlAttributeSpec Convert(CdtConSpec cdtConSpec, string className)
		{
			IUmlClassifier type;
			var multiType = cdtConSpec.BasicType;
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
					Name = cdtConSpec.Name,
					Type = type,
					UpperBound = cdtConSpec.UpperBound,
					LowerBound = cdtConSpec.LowerBound,
					TaggedValues = new[]
						{
							new UmlTaggedValueSpec("businessTerm", cdtConSpec.BusinessTerms) ,
							new UmlTaggedValueSpec("definition", cdtConSpec.Definition) ,
							new UmlTaggedValueSpec("dictionaryEntryName", cdtConSpec.DictionaryEntryName) { DefaultValue = GenerateDictionaryEntryNameDefaultValue(cdtConSpec, className) },
							new UmlTaggedValueSpec("languageCode", cdtConSpec.LanguageCode) ,
							new UmlTaggedValueSpec("modificationAllowedIndicator", cdtConSpec.ModificationAllowedIndicator) ,
							new UmlTaggedValueSpec("uniqueIdentifier", cdtConSpec.UniqueIdentifier) { DefaultValue = GenerateUniqueIdentifierDefaultValue(cdtConSpec, className) },
							new UmlTaggedValueSpec("versionIdentifier", cdtConSpec.VersionIdentifier) ,
							new UmlTaggedValueSpec("usageRule", cdtConSpec.UsageRules) ,
						},
	
				};

			return umlAttributeSpec;
		}
	}
}

